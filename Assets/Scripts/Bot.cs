using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private WaitForFixedUpdate _waitForFixedUpdate;
    private Transform _resourceContainer;
    private Base _base;
    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;

    private void OnEnable()
    {
        _base = GameObject.Find("Base").GetComponent<Base>();
        _resourceContainer = GameObject.Find("ResourcePool").GetComponent<Transform>();
    }

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

        resource.PickUp(transform, _holdDistance);

        while (resource.isActiveAndEnabled)
        {
            MoveTo(_base.transform);

            yield return _waitForFixedUpdate;
        }

        resource.Bring(_resourceContainer); 

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
