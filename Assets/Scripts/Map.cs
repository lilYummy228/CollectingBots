using UnityEngine;

public class Map : MonoBehaviour, ISelectable
{
    [SerializeField] private Terrain _terrain;

    private float _borderOffset = 50f;

    public float BoundsX { get; private set; }
    public float BoundsZ { get; private set; }

    private void Awake() => MakeBounds();

    private void MakeBounds()
    {
        BoundsX = _borderOffset;
        BoundsZ = _terrain.terrainData.size.z - _borderOffset;
    }

}
