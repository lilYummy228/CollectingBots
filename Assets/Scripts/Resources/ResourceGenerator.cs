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

    private List<Resource> _resources = new();
    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_spawnDelay);

        StartCoroutine(nameof(SpawnResources));
    }

    public void PutResource(Resource resource)
    {
        _pool.PutResource(resource);
    }

    public void RemoveResource(Resource resource) => _resources.Remove(resource);

    public int GetCount()
    {
        return _resources.Count;
    }

    public Resource GetResourceByIndex(int index)
    {
        return _resources[index];
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            if (_resources.Count < _spawnLimit)
            {
                int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount + 1);

                for (int i = 0; i < spawnCount; i++)
                {
                    float spawnPointX = Random.Range(_map.BoundsX, _map.BoundsZ);
                    float spawnPointZ = Random.Range(_map.BoundsX, _map.BoundsZ);
                    Vector3 spawnPoint = new Vector3(spawnPointX, 0f, spawnPointZ);

                    Resource resource = _pool.GetResource();
                    resource.gameObject.SetActive(true);
                    resource.transform.position = spawnPoint;
                    _resources.Add(resource);
                }
            }

            yield return _delay;
        }
    }
}