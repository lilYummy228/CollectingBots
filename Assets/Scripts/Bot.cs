using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Base _base;
    [SerializeField] private Transform _container;

    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;
    private WaitForFixedUpdate _waitForFixedUpdate;

    public Coroutine ExplorationCoroutine { get; private set; }

    private IEnumerator GatherResource(Resource resource)
    {
        _base.TakeResource(resource);

        while (enabled)
        {
            yield return _waitForFixedUpdate; 

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _lookDistance))
                if (hitInfo.transform.TryGetComponent(out Resource foundResource) == resource)
                    break;

            MoveTo(resource.transform);
        }

        resource.PickUp(gameObject.transform, _holdDistance);

        while (resource.isActiveAndEnabled)
        {
            MoveTo(_base.transform);

            yield return _waitForFixedUpdate;
        }

        resource.Bring(_container); 

        ExplorationCoroutine = null;
    }

    public void StartExploration(Resource resource) => ExplorationCoroutine = StartCoroutine(GatherResource(resource));

    private void MoveTo(Transform target)
    {
        transform.LookAt(target);

        transform.position =
            Vector3.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);
    }
}
