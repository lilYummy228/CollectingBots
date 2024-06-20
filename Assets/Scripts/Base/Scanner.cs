using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private float _scanCooldown = 3f;

    private float _cooldown = 0f;

    public bool HasScan()
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();
            _cooldown = Time.time + _scanCooldown;

            return true;
        }

        return false;
    }
}
