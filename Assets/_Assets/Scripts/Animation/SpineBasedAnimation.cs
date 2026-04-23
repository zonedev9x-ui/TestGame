using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class SpineBasedAnimation : AnimationController
{
    public SkeletonAnimation skeletonAnimation;

    [SpineAnimation] public string animMove, animAttack, animIdle;
    [SpineEvent] public string eventAttack;

    protected float defaultDurationAttack;
    protected string currentAnimAttack;
    protected string currentAnim;

    protected MeshRenderer meshRenderer;
    protected MaterialPropertyBlock block;
    protected const string MATERIAL_PROPERTY_COLOR = "_FillColor";
    protected const string MATERIAL_PROPERTY_PHASE = "_FillPhase";
    protected const float TIME_FLASH = 0.1f;

    public override bool isFacingRight
    {
        get => skeletonAnimation.skeleton.ScaleX > 0;
        set => skeletonAnimation.skeleton.ScaleX = value ? 1 : -1;
    }

    protected override void Awake()
    {
        base.Awake();
        RegisterAnimationEvents();

        SetColorDefault();
        currentAnimAttack = animAttack;
    }

    public virtual void RegisterAnimationEvents()
    {
        RemoveAnimationEvents();

        if (skeletonAnimation)
        {
            skeletonAnimation.AnimationState.Event += OnAnimationEventExecute;
            skeletonAnimation.AnimationState.Complete += OnAnimationComplete;

            block = new MaterialPropertyBlock();
            meshRenderer = skeletonAnimation.GetComponent<MeshRenderer>();
            meshRenderer.GetPropertyBlock(block);
        }
    }

    public virtual void RemoveAnimationEvents()
    {
        try
        {
            if (skeletonAnimation)
            {
                skeletonAnimation.AnimationState.Event -= OnAnimationEventExecute;
                skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
            }
        }
        catch { }
    }

    protected virtual void OnAnimationEventExecute(TrackEntry entry, Spine.Event e)
    {
        if (string.CompareOrdinal(e.Data.Name, eventAttack) == 0)
        {
            unit.OnBasicAttackRelease();
        }
    }

    protected virtual void OnAnimationComplete(TrackEntry entry)
    {
        string anim = entry.Animation.Name;
        if (string.CompareOrdinal(anim, currentAnimAttack) == 0)
        {
            PlayAnimIdle();
        }
    }

    public override void Renew()
    {
        base.Renew();
        ActiveMeshRenderer(true);

        try
        {
            skeletonAnimation.ClearState();
        }
        catch { }
    }

    public override void SetColorDefault()
    {
        base.SetColorDefault();
        SetColor(Color.white, 0f);

        SkeletonAnimation[] skeletons = GetComponentsInChildren<SkeletonAnimation>();
        for (int i = 0; i < skeletons.Length; i++)
        {
            skeletons[i].skeleton.A = 1f;
        }
    }

    public override void Reset()
    {
        base.Reset();
        currentAnim = null;
    }

    protected void ActiveMeshRenderer(bool isOn)
    {
        if (meshRenderer != null)
            meshRenderer.enabled = isOn;
    }

    public override void OnGameSpeedChange(object obj)
    {
        TrackEntry entry = skeletonAnimation.AnimationState.GetCurrent(0);
        if (entry != null)
        {
            float scale = 1f;

            if (string.CompareOrdinal(currentAnim, animMove) == 0)
            {
                scale = GetTimeScaleMove();
            }
            else if (string.CompareOrdinal(currentAnim, animIdle) == 0)
            {
                scale = GetTimeScaleIdle();
            }
            else if (string.CompareOrdinal(currentAnim, currentAnimAttack) == 0)
            {
                scale = GetTimeScaleAttack();
            }

            entry.TimeScale = scale;
        }
    }

    public override void PlayAnimMove()
    {
        if (string.IsNullOrEmpty(animMove))
        {
            return;
        }

        if (unit.isEnableMove && string.CompareOrdinal(currentAnim, animMove) != 0)
        {
            TrackEntry entry = skeletonAnimation.AnimationState.SetAnimation(0, animMove, true);
            entry.TimeScale = GetTimeScaleMove();
            currentAnim = animMove;
        }
    }

    public override void PlayAnimIdle()
    {
        if (string.IsNullOrEmpty(animIdle))
        {
            return;
        }

        if (string.CompareOrdinal(currentAnim, animIdle) != 0)
        {
            TrackEntry entry = skeletonAnimation.AnimationState.SetAnimation(0, animIdle, true);
            entry.TimeScale = GetTimeScaleIdle();
            currentAnim = animIdle;
        }
    }

    public override bool IsAttackable()
    {
        return !string.IsNullOrEmpty(animAttack);
    }

    public override void PlayAnimAttack(int totalAttacks = 1)
    {
        this.totalAttacks = totalAttacks;

        if (string.IsNullOrEmpty(currentAnimAttack))
        {
            return;
        }

        if (string.CompareOrdinal(currentAnim, currentAnimAttack) != 0)
        {
            TrackEntry entry = skeletonAnimation.AnimationState.SetAnimation(0, currentAnimAttack, false);
            defaultDurationAttack = entry.Animation.Duration;
            entry.TimeScale = GetTimeScaleAttack();
            currentAnim = currentAnimAttack;
        }
    }

    protected override float GetTimeScaleAttack()
    {
        float scale = 1f;
        float baseAttackTime = 1 / unit.stats.attackPerSecond;

        if (defaultDurationAttack > baseAttackTime)
        {
            float animAttackDuration = ANIMATION_DURATION_RATIO * baseAttackTime;
            scale = defaultDurationAttack / animAttackDuration;
        }

        scale *= totalAttacks;
        return scale;
    }

    public override void SetColor(Color color, float amount)
    {
        if (block == null || meshRenderer == null)
        {
            return;
        }

        block.SetColor(MATERIAL_PROPERTY_COLOR, color);
        block.SetFloat(MATERIAL_PROPERTY_PHASE, amount);
        meshRenderer.SetPropertyBlock(block);
    }

    protected override IEnumerator RountineFlash(Color color)
    {
        float timer = 0f;
        block.SetColor(MATERIAL_PROPERTY_COLOR, color);

        while (timer < TIME_FLASH)
        {
            float percent = timer / TIME_FLASH;
            block.SetFloat(MATERIAL_PROPERTY_PHASE, 0.8f * percent);
            meshRenderer.SetPropertyBlock(block);

            timer += Time.deltaTime;
            yield return null;
        }

        flashRoutine = null;
        SetColorByCC();
    }


}
