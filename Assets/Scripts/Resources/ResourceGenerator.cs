using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private ResourcePool _pool;

    [Header("Resources Spawn Settings")]
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _minSpawnCount = 1;
    [SerializeField] private int _maxSpawnCount = 6;
    [SerializeField] private int _spawnLimit = 24;

    private WaitForSeconds _delay;

    public List<Resource> Resources { get; private set; }

    private void Awake()
    {
        Resources = new();
        _delay = new WaitForSeconds(_spawnDelay);

        StartCoroutine(nameof(SpawnResources));
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            if (Resources.Count < _spawnLimit)
            {
                int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount + 1);

                for (int i = 0; i < spawnCount; i++)
                {
                    float spawnPointX = Random.Range(_map.BoundsX, _map.BoundsZ);
                    float spawnPointZ = Random.Range(_map.BoundsX, _map.BoundsZ);
                    Vector3 spawnPoint = new Vector3(spawnPointX, 0f, spawnPointZ);

                    Resource resource = _pool.GetResource();
                    resource.transform.position = spawnPoint;
                    Resources.Add(resource);
                }
            }

            yield return _delay;
        }
    }

    public void RemoveResource(Resource resource) => _pool.PutResource(resource);

    public void ShowResources()
    {
        foreach (Resource resource in Resources)
            resource.gameObject.SetActive(true);
    }
}