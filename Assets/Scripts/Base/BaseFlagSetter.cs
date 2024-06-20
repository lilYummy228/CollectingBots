using System;
using UnityEngine;

[RequireComponent(typeof(Selector))]
public class BaseFlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Camera _camera;

    private Selector _selector;
    private float _offset = 3f;
    private bool _isBaseSelected = false;

    public event Action<Flag> FlagSet;

    private void Awake() => _selector = GetComponent<Selector>();

    public void TrySetFlag()
    {
        ISelectable selected = _selector.GetSelected(_camera);

        if (selected is MainBase)
            _isBaseSelected = true;
        else if (selected is Map && _isBaseSelected)
            SetFlag(_selector.Position);
    }

    private void SetFlag(Vector3 position)
    {
        if (_flag.isActiveAndEnabled == false)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = position + Vector3.up * _offset;

            FlagSet?.Invoke(_flag);
        }
    }
}
