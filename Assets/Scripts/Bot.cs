using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Base _base;

    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;
    private Resource _grabbedResource;
    private WaitForFixedUpdate _waitForFixedUpdate;

    public Coroutine Coroutine { get; private set; }

    public IEnumerator GatherResource(Resource scannedResource)
    {
        while (enabled)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _lookDistance))
            {
                if (hitInfo.transform.TryGetComponent(out Resource exploredResource))
                {
                    _grabbedResource = exploredResource;
                    break;
                }
            }

            transform.LookAt(scannedResource.transform);

            transform.position =
                Vector3.MoveTowards(transform.position, scannedResource.transform.position, _moveSpeed * Time.deltaTime);

            yield return _waitForFixedUpdate;
        }

        _grabbedResource.PickUp(gameObject.transform, _holdDistance);

        while (_grabbedResource.isActiveAndEnabled)
        {       
            transform.LookAt(_base.transform);

            transform.position =
                Vector3.MoveTowards(transform.position, _base.transform.position, _moveSpeed * Time.deltaTime);

            yield return _waitForFixedUpdate;
        }

        _grabbedResource.Bring();

        Coroutine = null;
    }

    public void StartExploration(Resource resource) => Coroutine = StartCoroutine(GatherResource(resource));
}
