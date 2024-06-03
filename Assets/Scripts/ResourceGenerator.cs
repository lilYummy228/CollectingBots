using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private Base _base;
    [SerializeField] private float _spawnDelay;

    private List<GameObject> _resources;
    private WaitForSeconds _delay;

    private void Start()
    {
        _delay = new WaitForSeconds(_spawnDelay);
        _resources = new List<GameObject>();

        StartCoroutine(nameof(SpawnResources));
    }

    private void OnEnable()
    {
        _base.Scanned += ShowResources;
    }

    private void OnDisable()
    {
        _base.Scanned -= ShowResources;
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            float spawnPointX = Random.Range(_map.BoundsX, _map.BoundsZ);
            float spawnPointZ = Random.Range(_map.BoundsX, _map.BoundsZ);
            Vector3 spawnPoint = new Vector3(spawnPointX, 0f, spawnPointZ);

            GameObject resource = _pool.GetResource();
            resource.transform.position = spawnPoint;
            _resources.Add(resource);

            yield return _delay;
        }
    }

    private void ShowResources()
    {
        foreach (GameObject resource in _resources)
            resource.SetActive(true);
    }
}
