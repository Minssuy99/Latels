using UnityEngine;

public static class AnimHash
{
    // Bool
    public static readonly int IsLockedOn = Animator.StringToHash("isLockedOn");
    public static readonly int IsRunning = Animator.StringToHash("isRunning");
    public static readonly int IsReady = Animator.StringToHash("isReady");

    // Trigger
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Skill = Animator.StringToHash("Skill");
    public static readonly int Dash = Animator.StringToHash("Dash");
    public static readonly int Die = Animator.StringToHash("Die");
    public static readonly int Hit = Animator.StringToHash("Hit");
    public static readonly int Select = Animator.StringToHash("Select");
    public static readonly int Clear = Animator.StringToHash("Clear");
    public static readonly int Kick1 = Animator.StringToHash("Kick1");
    public static readonly int Kick2 = Animator.StringToHash("Kick2");

    // Float
    public static readonly int VelocityX = Animator.StringToHash("VelocityX");
    public static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
    public static readonly int Velocity = Animator.StringToHash("Velocity");

    // Int
    public static readonly int AttackType = Animator.StringToHash("AttackType");
    public static readonly int AttackCount = Animator.StringToHash("AttackCount");

    // Play (State Name)
    public static readonly int LockOn = Animator.StringToHash("LockOn");
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Idle = Animator.StringToHash("Idle");
}