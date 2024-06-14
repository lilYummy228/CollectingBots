using UnityEngine;

public class Selector<T> where T : MonoBehaviour
{
    private Camera _camera;

    public Selector(Camera camera)
    {
        _camera = camera;
    }

    public Vector3 Position { get; private set; }

    public T GetSelected()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Position = hit.point;

            if (hit.transform.TryGetComponent(out T component))
                return component;
        }

        return null;
    }
}
