using System.Collections;
using System.Collections.Generic;

public interface IAnimatable
{
    void Show();
    void Collapse();
    void Toggle();
    IEnumerator Animate();
    IEnumerator SuspendButtons(float time, List<AnimatedButton> buttons);
}