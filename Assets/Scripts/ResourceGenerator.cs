using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private ResourcePool _pool;
    [SerializeField] private Base _base;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _spawnCount = 5;

    private WaitForSeconds _delay;

    public List<Resource> Resources;

    private void Awake()
    {
        _delay = new WaitForSeconds(_spawnDelay);
        Resources = new List<Resource>();

        StartCoroutine(nameof(SpawnResources));
    }

    private void OnEnable() => _base.Scanned += ShowResources;

    private void OnDisable() => _base.Scanned -= ShowResources;

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            for (int i = 0; i < _spawnCount; i++)
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

    private void ShowResources()
    {
        foreach (Resource resource in Resources)
            resource.gameObject.SetActive(true);
    }

    public void RemoveResource(Resource resource)
    {        
        Resources.Remove(resource);
        _pool.PutResource(resource);
    }
}
