using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour, ISelectable
{
    [SerializeField] private float _moveSpeed;

    public BaseSpawner _baseSpawner;
    public Transform _resourceContainer;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private Base _currentBase;
    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;

    public Coroutine ExplorationCoroutine { get; private set; }

    public void StartBuild(Flag flag) => StartCoroutine(BuildBase(flag));

    public void StartExploration(Resource resource) => ExplorationCoroutine = StartCoroutine(GatherResource(resource));

    private IEnumerator GatherResource(Resource resource)
    {
        _currentBase.TakeResource(resource);

        bool isFound = false;

        while (isFound == false)
        {
            isFound = IsLookingFor(resource);

            MoveTo(resource.transform);

            yield return _waitForFixedUpdate;
        }

        resource.PickUp(transform, _holdDistance);

        while (resource.isActiveAndEnabled)
        {
            MoveTo(_currentBase.transform);

            yield return _waitForFixedUpdate;
        }

        resource.Bring(_resourceContainer);

        ExplorationCoroutine = null;
    }

    private IEnumerator BuildBase(Flag flag)
    {
        bool isFound = false;

        while (isFound == false)
        {
            isFound = IsLookingFor(flag);

            MoveTo(flag.transform);

            yield return _waitForFixedUpdate;
        }

        _currentBase.RemoveBot(this);
        Base newBase = _baseSpawner.GetNewBase(flag);
        _currentBase = newBase;
        newBase.AddBot(this);
    }

    public void SetBase(Base @base)
    {
        _currentBase = @base;
    }

    public void Init(Transform container, BaseSpawner baseSpawner)
    {
        _resourceContainer = container;
        _baseSpawner = baseSpawner;
    }

    private void MoveTo(Transform target)
    {
        transform.LookAt(target);

        transform.position =
            Vector3.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);
    }

    private bool IsLookingFor<T>(T obj) where T : MonoBehaviour
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _lookDistance))
            if (hitInfo.transform.TryGetComponent(out T component) == obj)
                return true;

        return false;
    }
}

