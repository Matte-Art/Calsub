using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public FX_Clock fxClock;
    public FX_Streak fxStreak;
    public FX_Background fxBackground;
    public FX_Shooting fxShooting;

    private void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;

        InputController inputController = FindObjectOfType<InputController>();
        inputController.OnMathOperationTypeChanged += HandleMathOperationTypeChanged;

    }
    private void HandleRoundStart(MathTask task)
    {
        fxClock.SetClockParticleType(task.operationType);
        fxClock.PlayParticleEmission();
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        fxClock.StopParticleEmission();
        fxShooting.StartShooting((int)args.Task.result);
    }
    private void HandleIdleEnabled()
    {
        fxClock.StopParticleEmission();
    }

    private void HandleMathOperationTypeChanged(MathOperationType mathOperationType)
    {
        var color = ColorManager.Instance.GetColorByMathOperationType(mathOperationType);
        fxBackground.ChangeBackgroundColor(color);
        fxBackground.SetBackgroundParticleType(mathOperationType);
        fxClock.ChangeExtraClockColor(color);
    }
}
