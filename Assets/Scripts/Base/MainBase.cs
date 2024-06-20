using UnityEngine;
using UnityEngine.InputSystem;

public class MainBase : Base, ISelectable
{
    [SerializeField] private BaseFlagSetter _baseFlagSetter;
    [SerializeField] private int _botSpawnCount = 3;
    [SerializeField] private int _baseBuildPrice = 5;

    private BaseSpawner _baseSpawner;

    public override void OnEnable()
    {
        base.OnEnable();
        _baseFlagSetter.FlagSet += StartBuildBase;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _baseFlagSetter.FlagSet -= StartBuildBase;
    }

    public void Start()
    {
        _bots.AddRange(_spawner.SpawnBots(_botSpawnCount, _botsContainer));
        _playerInput.Base.Select.performed += OnSelect;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        _baseFlagSetter.TrySetFlag();
    }

    private void StartBuildBase(Flag flag)
    {
        StartCoroutine(_baseSpawner.BuildNewBase(flag, _baseBuildPrice, _bots, this));
    }
    
    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }
}
