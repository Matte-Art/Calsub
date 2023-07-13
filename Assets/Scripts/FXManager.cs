using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public FX_Clock fxClock;
    public FX_Streak fxStreak;

    private void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;
    }
    private void HandleRoundStart(MathTask task)
    {
        fxClock.SetClockParticleType(task.operationType);
        fxClock.PlayParticleEmission();
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        fxClock.StopParticleEmission();
    }
    private void HandleIdleEnabled()
    {
        fxClock.StopParticleEmission();
    }
}
