using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineBasedAnimation_207 : SpineBasedAnimation
{
    [SpineAnimation] public string animUp, animDown, animAttackDown, animIdleDown;

    public bool isChangingForm = false;

    private bool isOutSide;
    private string currentAnimIdle;

    protected override void OnAnimationComplete(TrackEntry entry)
    {
        base.OnAnimationComplete(entry);

        string anim = entry.Animation.Name;

        if (string.CompareOrdinal(anim, animUp) == 0)
        {
            isChangingForm = false;
            PlayAnimIdle();
        }
        else if (string.CompareOrdinal(anim, animDown) == 0)
        {
            isChangingForm = false;
            PlayAnimIdle();
        }
    }

    public override void OnGameSpeedChange(object obj)
    {
        base.OnGameSpeedChange(obj);

        TrackEntry entry = skeletonAnimation.AnimationState.GetCurrent(0);
        if (entry != null)
        {
            float scale = 1f;

            if (string.CompareOrdinal(currentAnim, animUp) == 0 || string.CompareOrdinal(currentAnim, animDown) == 0)
            {
                //scale = GameManager.Instance.gameSpeed;
            }
            else if (string.CompareOrdinal(currentAnim, currentAnimIdle) == 0)
            {
                scale = GetTimeScaleIdle();
            }

            entry.TimeScale = scale;
        }
    }

    public override void PlayAnimAttack(int totalAttacks = 1)
    {
        currentAnimAttack = isOutSide ? animAttack : animAttackDown;
        base.PlayAnimAttack(totalAttacks);
    }

    public override void PlayAnimIdle()
    {
        currentAnimIdle = isOutSide ? animIdle : animIdleDown;

        TrackEntry entry = skeletonAnimation.AnimationState.SetAnimation(0, currentAnimIdle, true);
        entry.TimeScale = GetTimeScaleIdle();
        currentAnim = currentAnimIdle;
    }
}
