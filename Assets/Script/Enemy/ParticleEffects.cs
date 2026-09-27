using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem damageEffect;

    private void Awake()
    {
        if (damageEffect == null)
        {
            Debug.LogWarning(
                "ParticleEffects: Damage Effect is not assigned.",
                this
            );
        }
    }

    public void StartEffect()
    {
        if (damageEffect == null)
            return;

        damageEffect.Play();
    }

    public void StopEffect()
    {
        if (damageEffect == null)
            return;

        damageEffect.Stop();
    }
}