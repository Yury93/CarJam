using UnityEngine;

public class PersonAnimator : MonoBehaviour
{
    public readonly int Run = Animator.StringToHash("Run");
    public readonly int Idle = Animator.StringToHash("Idle");
    public readonly int Sit= Animator.StringToHash("Sit");
    public Animator Animator;
    private void OnValidate()
    {
        Animator = GetComponentInChildren<Animator>();
    }
    public void PlayRun() => Animator.SetTrigger(Run);
    public void PlayIdle() => Animator.SetTrigger(Idle);
    public void PlaySit() => Animator.SetTrigger(Sit);
}
