using UnityEngine;

public class PersonAnimator : MonoBehaviour
{
    public readonly int Run = Animator.StringToHash("Run");
    public readonly int Idle = Animator.StringToHash("Idle");
    public readonly int Sit = Animator.StringToHash("Sit");
    bool isSit = false;
    public Animator Animator;
    private void OnValidate()
    {
        Animator = GetComponentInChildren<Animator>();
    }
    public void PlayRun()
    {
        if (isSit) return;
        Animator.ResetTrigger(Idle);
        Animator.SetTrigger(Run);
    }
    public void PlayIdle()
    {
        if (isSit) return;
        Animator.ResetTrigger(Run);
        Animator.SetTrigger(Idle);
    }
    public void PlaySit()
    {
        Animator.ResetTrigger(Idle);
        Animator.ResetTrigger(Run);
        Animator.SetTrigger(Sit);
        isSit = true;
    }
}