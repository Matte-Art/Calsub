using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public FX_Timer fxTimer;
    public FX_Streak fxStreak;
    public FX_Background fxBackground;
    public FX_Shooting fxShooting;
    public FX_Keyboard fxKeyboard;
    public FX_Enemy fxEnemy;
    public FX_Check_Result fxCheckResult;
    public FX_Start fxStart;

    private void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnGameStarting += HandleOnGameStarting;
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;

        InputController inputController = FindObjectOfType<InputController>();
        inputController.OnMathOperationTypeChanged += HandleMathOperationTypeChanged;

    }

    private void HandleOnGameStarting()
    {
        fxKeyboard.Show();
        
    }

    private void HandleRoundStart(MathTask task)
    {
        fxTimer.SetClockParticleType(task.operationType);
        fxTimer.PlayParticleEmission();
        fxEnemy.ShowEnemy();
        fxTimer.Show();
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        fxTimer.StopParticleEmission();

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
        fxTimer.StopParticleEmission();
        fxKeyboard.Collapse();
        fxEnemy.CollapseEnemy();
        fxStart.PlayRandomTextFX();
        fxTimer.Collapse();
    }

    private void HandleMathOperationTypeChanged(MathOperationType mathOperationType)
    {
        var color = ColorManager.Instance.GetColorByMathOperationType(mathOperationType);
        fxBackground.ChangeBackgroundColor(color);
        fxBackground.SetBackgroundParticleType(mathOperationType);
        fxShooting.ChangeBulletType(mathOperationType);
        fxShooting.ChangeBulletColor(color);
        fxTimer.ChangeExtraClockColor(color);
    }
}
