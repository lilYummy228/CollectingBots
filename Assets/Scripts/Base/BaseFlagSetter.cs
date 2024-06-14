using UnityEngine;

public class BaseFlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Camera _camera;

    private Selector<Base> _selector;
    private bool _isBaseSelected = false;

    private void Awake()
    {
        _selector = new Selector<Base>(_camera);
    }

    public void TrySetFlag()
    {
        Base @base = _selector.GetSelected();

        if (@base is Base)
        {
            _isBaseSelected = true;
        }
        else if (@base is not Base && _isBaseSelected)
        {
            SetFlag(_selector.Position);

            _isBaseSelected = false;
        }

    }

    private void SetFlag(Vector3 position)
    {
        if (_flag.isActiveAndEnabled == false)
            _flag.gameObject.SetActive(true);

        _flag.transform.position = position;
    }
}
