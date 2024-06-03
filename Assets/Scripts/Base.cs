using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Base : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private Bot[] _bots = new Bot[3];
    [SerializeField] private float _cooldownTime = 5f;
        
    private PlayerInput _playerInput;
    private float _cooldown = 0f;

    public event Action Scanned;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Base.Scan.performed += OnScan;
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    public void OnScan(InputAction.CallbackContext context)
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();

            Scanned?.Invoke();

            _cooldown = Time.time + _cooldownTime;
        }
    }
}
