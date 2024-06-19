using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(BotSpawner), typeof(Scanner))]
[RequireComponent(typeof(Researcher))]
public class Base : MonoBehaviour
{
    private const int Unit = 1;

    private Scanner _scanner;
    private Researcher _researcher;

    public ResourceStorage ResourceStorage { get; protected set; }
    public BotSpawner Spawner { get; protected set; }
    public PlayerInput PlayerInput { get; protected set; }
    public WaitForFixedUpdate WaitForFixedUpdate { get; protected set; }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        PlayerInput.Base.Scan.performed += OnScan;
        PlayerInput.Base.Spawn.performed += OnSpawn;

        _scanner = GetComponent<Scanner>();

        _researcher = GetComponent<Researcher>();

        ResourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();

        Spawner = GetComponent<BotSpawner>();
    }

    public virtual void OnEnable() => PlayerInput.Enable();

    public virtual void OnDisable() => PlayerInput.Disable();

    public virtual void Start() => StartCoroutine(_researcher.SendBots(Spawner.SpawnedBots));

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _researcher.ResourceGenerator.RemoveResource(resource);
            ResourceStorage.AddResource();
        }
    }

    public void TakeResource(Resource resource) => _researcher.ResourceGenerator.Resources.Remove(resource);

    private void OnScan(InputAction.CallbackContext context) => _scanner.Scan(_researcher);

    private void OnSpawn(InputAction.CallbackContext context)
    {
        if (ResourceStorage.ResourceCount >= Spawner.BotSpawnPrice)
        {
            ResourceStorage.RemoveResource(Spawner.BotSpawnPrice);

            Spawner.SpawnBots(Unit);
        }
    }
}
