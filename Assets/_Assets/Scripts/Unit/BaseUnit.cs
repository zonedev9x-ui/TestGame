using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public enum BattleState
{
    None,
    Idle,
    Move,
    Attack,
    Die,
}

public class BaseUnit : MonoBehaviour
{
    public HealthBar hpBar;

    public BaseFx impactAttack;
    public BaseBullet prefabBullet;

    protected Transform groupColliders;
    protected Collider2D[] bodyColliders;
    protected Rigidbody2D rigid;

    protected int basicAttackCount;
    protected int basicAttackQueue = 1;
    protected float timeLeftUntilNextAttack;

    public virtual UnitType unitType => UnitType.NONE;
    public Transform Transform { get; protected set; }
    public Transform flipPoints { get; protected set; }
    public Transform groupLayers { get; protected set; }
    public Transform headPoint { get; protected set; }
    public Transform centerBodyPoint { get; protected set; }
    public Transform firePoint { get; protected set; }
    public NavMeshAgent navMesh { get; protected set; }
    public AnimationController animationController { get; protected set; }

    public Stats stats { get; protected set; } = new Stats();

    public int id { get; set; }
    public int battleId { get; set; }
    public BattleState state { get; set; } = BattleState.None;
    public float hp { get; protected set; }
    public virtual bool isRanged => prefabBullet != null;
    public float hpPercent => stats != null ? Mathf.Clamp01(hp / stats.maxHp) : 0f;
    public bool isTeamA { get; set; }
    public bool isPause { get; protected set; }
    public virtual bool isDead => hp <= 0f;
    public virtual bool isMoving { get; protected set; }
    public virtual bool isAttacking { get; protected set; }
    public virtual bool isDisableChangeState => isDead;

    public bool isEnableMove = true;

    //=> crowdController?.isDisableMove == false;


    public BaseUnit target
    {
        get
        {
            return _target;
        }

        set
        {
            if (_target != value)
            {
                OnChangeTarget(value);
            }

            _target = value;
        }
    }

    private BaseUnit _target;
    #region Unity Methods

    protected virtual void Awake()
    {
        Transform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        animationController = GetComponent<AnimationController>();
        Debug.Log($"{name} | animationController = {animationController}");

    }

    #endregion

    #region Behaviours
    public virtual void UpdateBehaviour()
    {

        switch (state)
        {
            case BattleState.None: ChangeState(BattleState.Idle); break;
            case BattleState.Idle: UpdateIdle(); break;
            case BattleState.Move: UpdateMove(); break;
                //case BattleState.Attack: UpdateAttack(); break;
        }

        //UpdateSortingOrder();
    }

    public virtual void ChangeState(BattleState newState, bool isForceChange = false) { }

    public virtual void ResetAction(bool isClearTarget = true)
    {
        if (isDead)
        {
            return;
        }

        if (isClearTarget)
        {
            target = null;
        }

        isMoving = false;

        if (animationController)
            animationController.Reset();
    }

    public virtual void InterruptAction() { }

    protected virtual void BeginIdle()
    {
        isMoving = false;
        animationController.PlayAnimIdle();
    }

    protected virtual void UpdateIdle() { }

    protected virtual void BeginMove()
    {
        isMoving = true;
        animationController.PlayAnimMove();
    }

    protected virtual void UpdateMove() { }

    protected virtual void Moving() { }

    protected virtual void TrackingNavMesh()
    {
        if (isPause)
        {
            return;
        }

        if (navMesh != null && navMesh.isOnNavMesh)
        {
            navMesh.isStopped = !isEnableMove || !isMoving;

            if (navMesh.velocity.magnitude > 0)
                navMesh.velocity = navMesh.velocity.normalized * navMesh.speed;
        }
    }
    protected virtual void BeginAttack()
    {
        //if (isEnableAttack)
        //{
        //    if (IsTargetAvailable())
        //    {
        //        CalculateNumberBullets();
        //        CalculateNumberAttacks();
        //        float delayNextAttack = (1f / (stats.attackPerSecond * basicAttackQueue));
        //        timeLeftUntilNextAttack = delayNextAttack;
        //        isMoving = false;
        //        animationController.PlayAnimAttack(basicAttackQueue);
        //    }
        //    else
        //    {
        //        animationController.Reset();
        //    }
        //}
        //else
        //{
        //    ResetAction();
        //}
    }
    #endregion

    #region Initiliaze
    protected virtual void Initialize()
    {
        FindTranformPoints();

    }

    protected virtual void FindTranformPoints()
    {
        if (groupLayers == null)
            groupLayers = transform.Find("Layers");

        if (flipPoints == null)
        {
            flipPoints = transform.Find("FlipPoints");
            headPoint = flipPoints.Find("Head");
            centerBodyPoint = flipPoints.Find("CenterBody");
            firePoint = flipPoints.Find("FirePoint");
        }

        if (groupColliders == null)
        {
            groupColliders = transform.Find("Colliders");
            if (groupColliders != null)
            {
                bodyColliders = groupColliders.GetComponentsInChildren<Collider2D>();
            }
        }
    }

    public virtual void Active()
    {
        Initialize();
        Renew();
        gameObject.SetActive(true);
    }

    public virtual void Active(Vector3 position)
    {
        Transform.position = position;
        Active();
    }

    protected virtual void Renew()
    {

        if (hpBar != null)
        {
            hpBar.Reset();
            hpBar.Deactive();
        }

        rigid.DOKill();
        transform.DOKill();
        animationController.Renew();
        //ActiveHpAndCollider(true);
        //ReloadStats();
        if (stats.maxHp <= 0)
            stats.maxHp = 100;

        hp = stats.maxHp;
        ResetAction();
    }

    #endregion

    #region Direction
    public virtual void UpdateSortingOrder()
    {
        if (groupLayers != null)
        {
            //Vector3 v = groupLayers.position;
            //v.z = v.y;
            //groupLayers.position = v;

            Vector3 v = groupLayers.localPosition;
            v.z = transform.position.y;
            groupLayers.localPosition = v;
        }
    }

    //protected virtual void SetFacingDirection(Vector3 dir)
    //{
    //    if (dir.x > 0.1f)
    //    {
    //        if (animationController.isFacingRight == false)
    //        {
    //            Face(isRight: true);
    //        }
    //    }
    //    else if (dir.x < -0.1f)
    //    {
    //        if (animationController.isFacingRight)
    //        {
    //            Face(isRight: false);
    //        }
    //    }
    //}

    public virtual void Face(bool isRight)
    {
        animationController.isFacingRight = isRight;
        flipPoints.right = isRight ? Vector3.right : Vector3.left;

        //if (hpBar != null)
        //{
        //    Vector3 v = hpBar.transform.localScale;
        //    v.x = isRight ? Mathf.Abs(v.x) : -Mathf.Abs(v.x);
        //    hpBar.transform.localScale = v;
        //}
    }

    public virtual void Face(Vector3 point)
    {
        bool isRight = point.x > Transform.position.x;
        Face(isRight);
    }

    #endregion

    #region Events
    protected virtual void OnChangeTarget(BaseUnit newTarget) { }

    protected virtual void ReleaseBasicAttack()
    {
        if (isRanged)
        {
            CreateBullets();
        }
        else
        {
            if (target != null)
            {
                //AttackData atkData = GetBasicAttackData();
                //target.TakeAttack(atkData, impactAttack);
            }
        }
    }

    protected virtual List<BaseBullet> CreateBullets()
    {
        List<BaseBullet> bullets = new List<BaseBullet>();

        //if (prefabBullet != null)
        //{
        //    CalculateNumberBullets();
        //    AttackData attackData = GetBasicAttackData();

        //    BaseBullet bullet = PoolingManager.Instance.GetBullet(prefabBullet);
        //    if (bullet != null)
        //    {
        //        bullet.Active(this, bulletTargetPoint, attackData);
        //        bullets.Add(bullet);

        //        if (bonusBulletTargetPoints.Count > 0)
        //        {
        //            for (int j = 0; j < bonusBulletTargetPoints.Count; j++)
        //            {
        //                BaseBullet bonusBullet = PoolingManager.Instance.GetBullet(prefabBullet);
        //                bonusBullet.Active(this, bonusBulletTargetPoints[j], attackData);
        //                bullets.Add(bonusBullet);
        //            }
        //        }
        //    }
        //}

        return bullets;
    }

    public virtual void OnBasicAttackRelease()
    {
        ReleaseBasicAttack();
        basicAttackCount++;
    }

    public virtual void Deactive() { }

    #endregion

    public virtual void LoadHpBar() { }

    public virtual void UpdateScale()
    {

    }
}
