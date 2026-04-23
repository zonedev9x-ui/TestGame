using UnityEngine;
using System.Collections;

public class TransformBasedAnimation : AnimationController
{
    public Animator animator;
    public AnimationClip clipAttack;
    public SpriteRenderer spRenderer;

    protected const float TIME_FLASH = 0.2f;
    protected const string PARAM_MOVE = "move";
    protected const string PARAM_MOVE_TIME_SCALE = "move_time_scale";
    protected const string PARAM_IDLE = "idle";
    protected const string PARAM_IDLE_TIME_SCALE = "idle_time_scale";
    protected const string PARAM_ATTACK = "attack";
    protected const string PARAM_ATTACK_TIME_SCALE = "attack_time_scale";

    public override bool isFacingRight
    {
        get => animator.transform.localScale.x > 0;
        set => animator.transform.localScale = new Vector3(value ? 1 : -1, 1, 1);
    }

    protected override void Awake()
    {
        base.Awake();
        SetColorDefault();

        TransformBasedAnimationEvent eventHandlers = animator.GetComponent<TransformBasedAnimationEvent>();
        if (eventHandlers)
        {
            eventHandlers.Initilize(this);
        }
    }

    public override void SetColorDefault()
    {
        base.SetColorDefault();
        SetColor(Color.white, 0f);
    }

    public override void SetColor(Color color, float amount)
    {
        spRenderer.color = color;
    }

    public override void OnGameSpeedChange(object obj)
    {
        animator.SetFloat(PARAM_MOVE_TIME_SCALE, GetTimeScaleMove());
        animator.SetFloat(PARAM_IDLE_TIME_SCALE, GetTimeScaleIdle());
        animator.SetFloat(PARAM_ATTACK_TIME_SCALE, GetTimeScaleAttack());
    }

    public override void PlayAnimMove()
    {
        if (unit.isEnableMove)
        {
            float scale = GetTimeScaleMove();
            animator.SetTrigger(PARAM_MOVE);
            animator.SetFloat(PARAM_MOVE_TIME_SCALE, scale);
        }
    }

    public override void PlayAnimIdle()
    {
        float scale = GetTimeScaleIdle();
        animator.SetTrigger(PARAM_IDLE);
        animator.SetFloat(PARAM_IDLE_TIME_SCALE, scale);
    }

    public override bool IsAttackable()
    {
        return clipAttack != null;
    }

    public override void PlayAnimAttack(int totalAttacks = 1)
    {
        this.totalAttacks = totalAttacks;

        float scale = GetTimeScaleAttack();
        animator.SetTrigger(PARAM_ATTACK);
        animator.SetFloat(PARAM_ATTACK_TIME_SCALE, scale);
    }

    protected override float GetTimeScaleAttack()
    {
        float scale = 1f;
        float attackRate = 1f / unit.stats.attackPerSecond;
        float clipDuration = clipAttack.length;
        if (clipDuration > attackRate)
        {
            scale = clipDuration / attackRate;
        }

        scale *= totalAttacks;
        scale *= 1f;

        return scale;
    }

    protected override IEnumerator RountineFlash(Color color)
    {
        float timer = 0f;

        while (timer < TIME_FLASH)
        {
            SetColor(Color.red, 0f);
            timer += Time.deltaTime;
            yield return null;
        }

        flashRoutine = null;
        SetColorByCC();
    }

}
