using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    public void PickUp(Transform parent, float holdDistance)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0f, holdDistance);
    }

    public void Bring(Transform container) => transform.SetParent(container);
}
