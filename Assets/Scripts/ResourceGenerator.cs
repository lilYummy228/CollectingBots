using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private ResourcePool _pool;
    [SerializeField] private Base _base;

    [Header("Spawn Settings")]
    [SerializeField, Range(1, 12)] private float _spawnDelay;
    [SerializeField, Range(0, 1)] private int _minSpawnCount;
    [SerializeField, Range(1, 5)] private int _maxSpawnCount;

    private WaitForSeconds _delay;

    public List<Resource> Resources { get; private set; }

    private void Awake()
    {
        _delay = new WaitForSeconds(_spawnDelay);
        Resources = new List<Resource>();

        StartCoroutine(nameof(SpawnResources));
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
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

            yield return _delay;
        }
    }

    public void RemoveResource(Resource resource)
    {
        _pool.PutResource(resource);
    }

    public void ShowResources()
    {
        foreach (Resource resource in Resources)
            resource.gameObject.SetActive(true);
    }
}
