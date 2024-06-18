using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private float _scanCooldown = 3f;

    private float _cooldown = 0f;

    public void Scan(Researcher researcher)
    {
        if (_cooldown <= Time.time)
        {
            _scanEffect.Play();

            researcher.ResourceGenerator.ShowResources();

            _cooldown = Time.time + _scanCooldown;
        }
    }
}
