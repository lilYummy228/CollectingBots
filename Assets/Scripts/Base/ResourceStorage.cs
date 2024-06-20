using System;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public event Action<int> ResourceAdded;

    public int ResourceCount { get; private set; } = 0;

    public void AddResource() => ResourceAdded?.Invoke(++ResourceCount);

    public void RemoveResources(int count) => ResourceAdded?.Invoke(ResourceCount -= count);
}
