using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private const float Direction = 1f;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private Map _map;

    private PlayerInput _playerInput;
    private Vector2 _moveDirection;    

    private void Awake() => _playerInput = new PlayerInput();

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void Update()
    {
        _moveDirection = _playerInput.Camera.Move.ReadValue<Vector2>();

        Move();
    }

    private void Move()
    {
        if (_moveDirection.sqrMagnitude < 0.1f)
            return;

        MoveByBound();

        Vector3 direction = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
        transform.Translate(direction * _moveSpeed * Time.deltaTime);
    }

    private void MoveByBound()
    {
        if (transform.position.x <= _map.BoundsX)
            _moveDirection.x = Direction;
        else if (transform.position.x >= _map.BoundsZ)
            _moveDirection.x = -Direction;
        else if (transform.position.z <= _map.BoundsX)
            _moveDirection.y = Direction;
        else if (transform.position.z >= _map.BoundsZ)
            _moveDirection.y = -Direction;
    }
}
