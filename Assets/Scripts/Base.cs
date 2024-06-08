using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private Bot[] _bots;
    [SerializeField, Range(0f, 6f)] private float _cooldownTime = 6f;

    private PlayerInput _playerInput;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private float _cooldown = 0f;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Base.Scan.performed += OnScan;

        StartCoroutine(nameof(SendBots));
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _resourceGenerator.RemoveResource(resource);
            _resourceCounter.AddResource(resource);
        }
    }

    private void OnDisable() => _playerInput.Disable();

    private IEnumerator SendBots()
    {
        while (enabled)
        {
            if (_resourceGenerator.Resources.Count > 0)
            {
                Resource resource = _resourceGenerator.Resources[0];

                if (resource.isActiveAndEnabled)
                {
                    foreach (Bot bot in _bots)
                    {
                        if (bot.ExplorationCoroutine == null)
                        {
                            bot.StartExploration(resource);
                            break;
                        }
                    }
                }
            }

            yield return _waitForFixedUpdate;
        }
    }

    public void OnScan(InputAction.CallbackContext context)
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();

            _resourceGenerator.ShowResources();

            _cooldown = Time.time + _cooldownTime;
        }
    }

    public void TakeResource(Resource resource)
    {
        _resourceGenerator.Resources.Remove(resource);
    }
}
