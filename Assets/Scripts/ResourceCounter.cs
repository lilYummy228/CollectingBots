using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private List<Resource> _receivedResources;
    private int _count;

    public event Action<int> ResourceAdded;

    private void Awake()
    {
        _receivedResources = new List<Resource>();
        _count = _receivedResources.Count;
    }

    public void AddResource(Resource resource)
    {
        _receivedResources.Add(resource);

        _count++;
        ResourceAdded?.Invoke(_count);
    }
}
