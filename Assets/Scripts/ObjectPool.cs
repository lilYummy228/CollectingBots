using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Resource[] _resources;
    [SerializeField] private Transform _container;

    private Queue<GameObject> _pool = new();

    public GameObject GetResource()
    {
        if (_pool.Count == 0)
        {
            int resourceIndex = Random.Range(0, _resources.Length);

            GameObject resource = Instantiate(_resources[resourceIndex].Prefab);
            resource.transform.parent = _container;
            resource.gameObject.SetActive(false);

            return resource;
        }

        return _pool.Dequeue();
    }

    public void PutResource(GameObject resource)
    {
        _pool.Enqueue(resource);
        resource.gameObject.SetActive(false);
    }
}
