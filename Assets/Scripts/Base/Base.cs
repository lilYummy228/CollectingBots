using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(BotSpawner), typeof(ResourceStorage))]
[RequireComponent(typeof(Researcher), typeof(BaseFlagSetter))]
public class Base : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private float _scanCooldown = 6f;
    [SerializeField] private int _botSpawnCount = 3;

    private BaseFlagSetter _baseFlagSetter;
    private ResourceStorage _resourceStorage;
    private Researcher _researcher;
    private BotSpawner _spawner;
    private PlayerInput _playerInput;
    private float _cooldown = 0f;

    private void Awake()
    {
        _baseFlagSetter = GetComponent<BaseFlagSetter>();

        _resourceStorage = GetComponent<ResourceStorage>();

        _researcher = GetComponent<Researcher>();

        _spawner = GetComponent<BotSpawner>();
        _spawner.SpawnBots(_botSpawnCount);

        _playerInput = new PlayerInput();
        _playerInput.Base.Scan.performed += OnScan;
        _playerInput.Base.Spawn.performed += OnSpawn;
        _playerInput.Base.Migrate.performed += OnMigrate;

        StartCoroutine(_researcher.SendBots(_spawner.SpawnedBots));
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _researcher.ResourceGenerator.RemoveResource(resource);
            _resourceStorage.AddResource();
        }
    }

    private void OnDisable() => _playerInput.Disable();

    private void OnScan(InputAction.CallbackContext context)
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();

            _researcher.ResourceGenerator.ShowResources();

            _cooldown = Time.time + _scanCooldown;
        }
    }

    public void TakeResource(Resource resource) => _researcher.ResourceGenerator.Resources.Remove(resource);

    private void OnSpawn(InputAction.CallbackContext context)
    {
        if (_resourceStorage.ResourceCount >= _spawner.BotSpawnPrice)
        {
            _resourceStorage.RemoveResource(_spawner.BotSpawnPrice);

            _spawner.SpawnBots(1);
        }
    }

    private void OnMigrate(InputAction.CallbackContext context) => _baseFlagSetter.TrySetFlag();
}
