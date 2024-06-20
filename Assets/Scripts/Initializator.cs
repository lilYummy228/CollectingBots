using UnityEngine;

public class Initializator : MonoBehaviour
{
    [SerializeField] private Transform _resourceContainer;
    [SerializeField] private MainBase _mainBase;
    [SerializeField] private Base _base;
    [SerializeField] private Bot _bot;
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Researcher _researcher;
    [SerializeField] private BaseSpawner _baseSpawner;

    private void Awake()
    {
        _bot.Init(_resourceContainer, _baseSpawner);
        _base.Init(_researcher, _storage);
        _mainBase.Init(_researcher, _storage);
        _mainBase.SetBaseSpawner(_baseSpawner);
    }
}
