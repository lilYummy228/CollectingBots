using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(BotSpawner))]
public class Base : MonoBehaviour
{
    private const int Unit = 1;

    [SerializeField] private Researcher _researcher;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] protected Transform _botsContainer;

    protected List<Bot> _bots = new();
    protected PlayerInput _playerInput;
    protected BotSpawner _spawner;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Base.Spawn.performed += OnSpawn;

        _spawner = GetComponent<BotSpawner>();
    }

    public virtual void OnEnable() => _playerInput.Enable();

    public virtual void OnDisable() => _playerInput.Disable();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _researcher.ResourceGenerator.PutResource(resource);
            _resourceStorage.AddResource();
        }
    }

    public void TakeResource(Resource resource)
    {
        _researcher.ResourceGenerator.RemoveResource(resource);
    }

    private void OnSpawn(InputAction.CallbackContext context)
    {
        if (_resourceStorage.ResourceCount >= _spawner.BotSpawnPrice)
        {
            _resourceStorage.RemoveResources(_spawner.BotSpawnPrice);

            _bots.AddRange(_spawner.SpawnBots(Unit, _botsContainer));
        }
    }

    public void Init(Researcher researcher, ResourceStorage resourceStorage)
    {
        _researcher = researcher;
        _resourceStorage = resourceStorage;
    }

    public void RemoveBot(Bot bot)
    {
        _bots?.Remove(bot);
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void Explore(Resource resource)
    {
        foreach (Bot bot in _bots)
        {
            if (bot.ExplorationCoroutine == null)
            {
                bot.StartExploration(resource);
                break;
            }
        }
    }
}
