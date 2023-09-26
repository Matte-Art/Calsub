using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public FX_Clock fxClock;
    public FX_Streak fxStreak;
    public FX_Background fxBackground;
    public FX_Shooting fxShooting;
    public FX_Keyboard fxKeyboard;
    public FX_Enemy fxEnemy;
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
        fxKeyboard.ShowKeyboard();
        fxEnemy.ShowEnemy();
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        fxClock.StopParticleEmission();

        if (args.IsTaskCorrect)
        {
            fxShooting.StartShooting((int)args.Task.result);
        }
        else
        {
            // wrong answer
        }
    }
    private void HandleIdleEnabled()
    {
        fxClock.StopParticleEmission();
        fxKeyboard.CollapseKeyboard();
        fxEnemy.CollapseEnemy();
    }

    private void HandleMathOperationTypeChanged(MathOperationType mathOperationType)
    {
        var color = ColorManager.Instance.GetColorByMathOperationType(mathOperationType);
        fxBackground.ChangeBackgroundColor(color);
        fxBackground.SetBackgroundParticleType(mathOperationType);
        fxShooting.ChangeBulletType(mathOperationType);
        fxShooting.ChangeBulletColor(color);
        fxClock.ChangeExtraClockColor(color);
    }
}
