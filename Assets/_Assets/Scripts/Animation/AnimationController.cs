
using System;
using System.Collections;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    protected int totalAttacks;
    protected bool isFlashingColor;
    protected IEnumerator flashRoutine;
    protected const float MOVEMENT_SPEED_DEFAULT = 2f;

    protected readonly float ANIMATION_DURATION_RATIO = 1f;
    protected readonly Color colorFreeze = new Color(0f, 124f, 183f, 255f);

    public BaseUnit unit { get; protected set; }
    public virtual bool isFacingRight { get; set; }
    protected bool isDisableFlashing => unit.isDead || isFlashingColor;

    protected virtual void Awake()
    {
        unit = GetComponent<BaseUnit>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public virtual void Renew()
    {
        Reset();
        SetColorDefault();
    }

    public virtual void Reset()
    {
        try
        {
            if (unit.isDead)
            {
                return;
            }

            unit.state = BattleState.None;
        }
        catch { }
    }

    public virtual void UpdateSortingOrder()
    {
        unit.UpdateSortingOrder();
    }

    public virtual void OnGameSpeedChange(object obj) { }
    public virtual void PlayAnimMove() { }
    public virtual void PlayAnimIdle() { }
    public virtual void PlayAnimAttack(int totalAttacks = 1) { }

    public virtual bool IsAttackable()
    {
        return false;
    }

    protected virtual float GetTimeScaleMove()
    {
        float scale = 1f;

        if (unit.stats.speedMove > MOVEMENT_SPEED_DEFAULT)
        {
            scale = unit.stats.speedMove / MOVEMENT_SPEED_DEFAULT;
        }

        return scale;
    }

    protected virtual float GetTimeScaleIdle()
    {
        //float scale = GameManager.Instance.gameSpeed;
        float scale = 1f;
        return scale;
    }

    protected virtual float GetTimeScaleAttack()
    {
        return 1f;
    }

    #region Colors
    public virtual void SetColorDefault()
    {
        isFlashingColor = false;
    }

    public virtual void SetColorByCC()
    {
        //if (unit.crowdController.isFrozen)
        //{
        //    SetColor(colorFreeze, 0.5f);
        //}
        //else
        //{
        SetColorDefault();
        //}
    }

    public virtual void SetColor(Color color, float amount) { }

    public virtual void FlashColor(Color color)
    {
        if (isDisableFlashing || unit.gameObject.activeInHierarchy == false)
        {
            return;
        }

        isFlashingColor = true;

        if (flashRoutine == null)
        {
            flashRoutine = RountineFlash(color);
            StartCoroutine(flashRoutine);
        }
    }

    protected virtual IEnumerator RountineFlash(Color color)
    {
        yield return null;
    }
    #endregion

    //#region CCs
    //public virtual void AnimateCC(CrowdControlData ccData)
    //{
    //    try
    //    {
    //        unit.InterruptAction();

    //        switch (ccData.type)
    //        {
    //            case CrowdControlType.STUN: Stun(); break;
    //            case CrowdControlType.PARALYZE: Paralyze(); break;
    //            case CrowdControlType.FREEZE: Freeze(true); break;
    //            case CrowdControlType.KNOCK_BACK: KnockBackByAttacker(ccData.attacker, ccData.value, ccData.duration); break;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        DebugCustom.LogError(e.Message);
    //        unit.ResetAction();
    //    }
    //}

    //public virtual void RemoveCC(CrowdControlType type)
    //{
    //    Reset();

    //    if (type == CrowdControlType.FREEZE)
    //    {
    //        Freeze(false);
    //    }
    //}

    //protected virtual void ShowFxCC(CrowdControlType type, Vector3 v)
    //{
    //    BaseFx fx = null;

    //    switch (type)
    //    {
    //        case CrowdControlType.STUN:
    //            fx = FxManager.Instance.SpawnFx(CommonFxType.STUN, v);
    //            break;
    //    }

    //    if (fx != null)
    //    {
    //        fx.transform.parent = unit.flipPoints;
    //        fx.transform.localRotation = Quaternion.identity;
    //        unit.crowdController.remainingFxInstances[type] = fx;
    //    }
    //}

    //protected virtual void Stun()
    //{
    //    Reset();

    //    if (unit.headPoint != null)
    //    {
    //        ShowFxCC(CrowdControlType.STUN, unit.headPoint.position);
    //    }
    //    else
    //    {
    //        DebugCustom.LogError("HeadPoint NULL");
    //    }
    //}

    //protected virtual void Paralyze()
    //{
    //    Reset();
    //    ShowFxCC(CrowdControlType.PARALYZE, unit.Transform.position);
    //}

    //private void Freeze(bool isOn)
    //{
    //    SetColorDefault();

    //    if (isOn)
    //    {
    //        //Pause();
    //        SetColor(colorFreeze, 0.5f);
    //    }
    //    else
    //    {
    //        //Resume();
    //        SetColorByCC();
    //    }
    //}

    //public virtual void KnockBackToDirection(Vector3 direction, float range = 1f, float duration = 0.3f)
    //{
    //    Reset();
    //    PlayAnimIdle();

    //    Vector3 backPos = unit.Transform.position + direction * range;

    //    //Rigidbody2D rigid = unit.GetComponent<Rigidbody2D>();
    //    //rigid.DOMove(backPos, duration).OnComplete(() =>
    //    //{
    //    //    unit.ResetAction(isClearTarget: false);
    //    //});

    //    if (unit.navMesh != null)
    //        unit.navMesh.Warp(backPos);
    //}

    //public virtual void KnockBackByAttacker(BaseUnit attacker, float range = 1f, float duration = 0.3f)
    //{
    //    Vector3 dir = Vector3.zero;

    //    if (attacker != null)
    //    {
    //        dir = (unit.Transform.position - attacker.Transform.position).normalized;
    //    }
    //    else
    //    {
    //        float euler = isFacingRight ? -90f : 90f;
    //        dir = Quaternion.Euler(0, euler, 0) * transform.up;
    //    }

    //    KnockBackToDirection(dir, range, duration);
    //}
    //#endregion

}
