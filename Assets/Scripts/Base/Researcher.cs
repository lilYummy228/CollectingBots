using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Researcher : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Base _base;

    private PlayerInput _playerInput;
    private List<Base> _bases = new();

    public ResourceGenerator ResourceGenerator => _resourceGenerator;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Base.Scan.performed += OnScan;

        _bases.Add(_base);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    public void AddBase(Base @base)
    {
        _bases.Add(@base);
    }

    private void OnScan(InputAction.CallbackContext context)
    {
        if (_scanner.HasScan())
            Explore();
    }

    private void Explore()
    {
        foreach (Base @base in _bases)
        {
            int count = _resourceGenerator.GetCount();

            for (int i = 0; i < count; i++)
            {
                Resource resource = _resourceGenerator.GetResourceByIndex(0);
                @base.Explore(resource);
            }
        }
    }
}
