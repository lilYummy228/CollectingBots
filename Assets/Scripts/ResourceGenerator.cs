using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private Resource[] _resources;
    [SerializeField] private float _spawnDelay;

    private WaitForSeconds _delay;

    private void Start()
    {
        _delay = new WaitForSeconds(_spawnDelay);

        StartCoroutine(nameof(SpawnResources));
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            float spawnPointX = Random.Range(_map.BoundsX, _map.BoundsZ);
            float spawnPointZ = Random.Range(_map.BoundsX, _map.BoundsZ);
            Vector3 spawnPoint = new Vector3(spawnPointX, 0f, spawnPointZ);

            int resourceIndex = Random.Range(0, _resources.Length);
            GameObject gameObject = Instantiate(_resources[resourceIndex].Prefab); //сделать pool
            gameObject.transform.position = spawnPoint;

            yield return _delay;
        }
    }
}
