using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private Bot[] _bots;
    [SerializeField] private float _cooldownTime = 6f;

    private List<Resource> _receivedResources;
    private PlayerInput _playerInput;
    private float _cooldown = 0f;
    public int _resourcesCount;

    public event Action Scanned;

    private void Awake()
    {
        _receivedResources = new List<Resource>();

        _playerInput = new PlayerInput();
        _playerInput.Base.Scan.performed += OnScan;

        StartCoroutine(nameof(SendBots));
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

    private IEnumerator SendBots()
    {
        while (enabled)
        {
            yield return new WaitForFixedUpdate();

            if (_resourceGenerator.Resources.Count > 0)
            {
                foreach (Resource resource in _resourceGenerator.Resources)
                {
                    if (resource.isActiveAndEnabled && resource.IsExplored == false)
                    {
                        foreach (Bot bot in _bots)
                        {
                            if (bot.ExplorationCoroutine == null)
                            {
                                bot.StartExploration(resource);
                                resource.Explore();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _resourceGenerator.RemoveResource(resource);
            _receivedResources.Add(resource);

            _resourcesCount = _receivedResources.Count;
        }
    }
}
