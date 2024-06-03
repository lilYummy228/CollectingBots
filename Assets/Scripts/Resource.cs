using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    [SerializeField] private GameObject _prefab;

    public GameObject Prefab => _prefab;
}
