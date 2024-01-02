using UnityEngine;

public class FX_Streak : MonoBehaviour
{
    private ParticleSystem streakParticleSystem;

    private void Awake()
    {
        streakParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    private void Start()
    {
        streakParticleSystem.Stop();
        ScoreManager.Instance.OnStreakChange += HandleStreakChange;
    }
    public void EnableStreakFX()
    {
        streakParticleSystem.Play();
    }
    public void DisableStreakFX()
    {
        streakParticleSystem.Stop();
    }

    private void HandleStreakChange(int streak)
    {
        if (streak > 0)
        {
            EnableStreakFX();
        }
        else
        {
            DisableStreakFX();
        }
    }
}
