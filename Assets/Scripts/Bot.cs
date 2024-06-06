using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Base _base;
    [SerializeField] private Transform _container;

    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;
    private Resource _grabbedResource;
    private WaitForFixedUpdate _waitForFixedUpdate;

    public Coroutine ExplorationCoroutine { get; private set; }

    public void StartExploration(Resource resource) => ExplorationCoroutine = StartCoroutine(GoAfterResource(resource));

    public IEnumerator GoAfterResource(Resource scannedResource)
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
        _grabbedResource.Pick();

        StartCoroutine(GetBack());
    }

    private IEnumerator GetBack()
    {
        while (_grabbedResource.isActiveAndEnabled)
        {
            transform.LookAt(_base.transform);

            transform.position =
                Vector3.MoveTowards(transform.position, _base.transform.position, _moveSpeed * Time.deltaTime);

            yield return _waitForFixedUpdate;
        }

        _grabbedResource.Bring(_container);

        ExplorationCoroutine = null;
    }
}
