using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Resource : MonoBehaviour
{
    public bool IsExplored {  get; private set; }
    public bool IsPicked { get; private set; }

    private void OnEnable()
    {
        IsExplored = false;
        IsPicked = false;
    }

    public void PickUp(Transform parent, float holdDistance)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0f, holdDistance);
    }

    public void Bring(Transform container)
    {
        transform.SetParent(container);
    }

    public void Explore()
    {
        IsExplored = true;
    }

    public void Pick()
    {
        IsPicked = true;
    }
}
