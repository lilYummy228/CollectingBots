using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField, Range(0f, 6f)] private float _cooldownTime = 6f;

    [Header("Bots Spawn Settings")]
    [SerializeField, Range(1, 3)] private int _botCount = 3;
    [SerializeField, Range(3, 5)] private int _botSpawnPrice = 3;
    [SerializeField, Range(1, 10)] private float _maxSpawnRange = 10;
    [SerializeField] private Bot _prefab;
    [SerializeField] private Transform _botsContainer;

    private float _cooldown = 0f;
    private float _minSpawnRange;
    private List<Bot> _bots = new();
    private PlayerInput _playerInput;
    private WaitForFixedUpdate _waitForFixedUpdate;

    private void Awake()
    {
        SpawnBots(_botCount);

        _minSpawnRange = -_maxSpawnRange;

        _playerInput = new PlayerInput();
        _playerInput.Base.Scan.performed += OnScan;
        _playerInput.Base.Spawn.performed += OnSpawn;

        StartCoroutine(nameof(SendBots));
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _resourceGenerator.RemoveResource(resource);
            _resourceCounter.AddResource(resource);
        }
    }

    private void OnDisable() => _playerInput.Disable();

    private IEnumerator SendBots()
    {
        while (enabled)
        {
            if (_resourceGenerator.Resources.Count > 0)
            {
                Resource resource = _resourceGenerator.Resources[0];

                if (resource.isActiveAndEnabled)
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

            yield return _waitForFixedUpdate;
        }
    }

    public void OnScan(InputAction.CallbackContext context)
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();

            _resourceGenerator.ShowResources();

            _cooldown = Time.time + _cooldownTime;
        }
    }

    public void OnSpawn(InputAction.CallbackContext context)
    {
        if (_resourceCounter.ReceivedResources.Count >= _botSpawnPrice)
        {
            _resourceCounter.RemoveResource(_botSpawnPrice);

            SpawnBots(1);
        }
    }

    public void TakeResource(Resource resource)
    {
        _resourceGenerator.Resources.Remove(resource);
    }

    private void SpawnBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float offsetX = UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange);
            float offsetZ = UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange);
            Vector3 spawnpoint = 
                new(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ);

            Bot bot = Instantiate(_prefab);
            _bots.Add(bot);
            bot.transform.parent = _botsContainer.transform;
            bot.transform.position = spawnpoint;
        }
    }
}
