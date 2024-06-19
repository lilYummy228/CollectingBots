using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour, ISelectable
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Base _constructedBase;

    private WaitForFixedUpdate _waitForFixedUpdate;
    private Transform _resourceContainer;
    private float _lookDistance = 2.5f;
    private float _holdDistance = 1.5f;
    private Base _currentBase;

    public Coroutine ExplorationCoroutine { get; private set; }

    private void OnEnable() => _resourceContainer = GameObject.Find("ResourcePool").GetComponent<Transform>();

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

    private IEnumerator Build(Flag flag)
    {
        bool isFound = false;

        while (isFound == false)
        {
            isFound = IsLookingFor(flag);

            MoveTo(flag.transform);

            yield return _waitForFixedUpdate;
        }

        flag.gameObject.SetActive(false);

        Base newBase = Instantiate(_constructedBase, flag.transform.position, Quaternion.identity);
        _currentBase.Spawner.SpawnedBots.Remove(this);
        _currentBase = newBase;
        newBase.Spawner.SpawnedBots.Add(this);
    }

    public void SetBase(Base @base) => _currentBase = @base;

    public void StartExploration(Resource resource) => ExplorationCoroutine = StartCoroutine(GatherResource(resource));

    public void StartBuild(Flag flag) => StartCoroutine(Build(flag));

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

