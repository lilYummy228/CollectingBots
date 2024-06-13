using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private int _count;

    public event Action<int> ResourceAdded;

    public List<Resource> ReceivedResources {  get; private set; }

    private void Awake()
    {
        ReceivedResources = new List<Resource>();
        _count = ReceivedResources.Count;
    }

    public void AddResource(Resource resource)
    {
        ReceivedResources.Add(resource);

        _count = ReceivedResources.Count;
        ResourceAdded?.Invoke(_count);
    }

    public void RemoveResource(int count)
    {
        ReceivedResources.RemoveRange(0, count);

        _count = ReceivedResources.Count;
        ResourceAdded?.Invoke(_count);
    }
}
