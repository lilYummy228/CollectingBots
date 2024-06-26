using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] private Resource[] _resources;
    [SerializeField] private Transform _container;

    public Queue<Resource> _pool = new();

    public Resource GetResource()
    {
        if (_pool.Count == 0)
        {
            int resourceIndex = Random.Range(0, _resources.Length);

            Resource resource = Instantiate(_resources[resourceIndex]);
            resource.transform.parent = _container;

            return resource;
        }

        return _pool.Dequeue();
    }

    public void PutResource(Resource resource)
    {
        _pool.Enqueue(resource);
        resource.gameObject.SetActive(false);
    }
}
