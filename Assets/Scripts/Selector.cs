using UnityEngine;

public class Selector : MonoBehaviour
{
    public Vector3 Position { get; private set; }

    public ISelectable GetSelected(Camera camera)
    {
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Position = hit.point;

            if (hit.transform.TryGetComponent(out ISelectable selected))
                return selected;
        }

        return null;
    }
}
