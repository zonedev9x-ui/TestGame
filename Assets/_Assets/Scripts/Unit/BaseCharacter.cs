using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BaseCharacter : BaseUnit
{
    protected IEnumerator routineRegen;

    protected override void Awake()
    {
        base.Awake();
        navMesh = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {
        Moving();
    }

    protected override void Initialize()
    {
        base.Initialize();
        LoadHpBar();
    }

    public override void LoadHpBar()
    {
        base.LoadHpBar();

        Transform hpBarTransform = transform.Find("health_bar");
        if (hpBarTransform)
        {
            hpBar = hpBarTransform.GetComponent<HealthBar>();
            hpBar.Init(this);

            if (headPoint != null)
            {
                Vector3 v = transform.position;
                v.y = headPoint.position.y + 0.1f;
                hpBar.transform.position = v;
            }
        }
    }

    public override void Active(Vector3 position)
    {
        base.Active(position);

        if (navMesh == null)
            navMesh = GetComponent<NavMeshAgent>();

        if (navMesh != null)
        {
            navMesh.updateRotation = false;
            navMesh.updateUpAxis = false;
            if (navMesh.isOnNavMesh)
            {
                navMesh.Warp(position);
            }
        }
    }

    public override void ChangeState(BattleState newState, bool isForceChange = false)
    {
        if (isDisableChangeState)
        {
            return;
        }

        if (isForceChange || state != newState)
        {
            state = newState;
            switch (state)
            {
                case BattleState.Idle: BeginIdle(); break;
                case BattleState.Move: BeginMove(); break;
                case BattleState.Attack: BeginAttack(); break;
            }
        }
    }

    public override void Deactive()
    {
        base.Deactive();
        //GameManager.Instance.RemoveUnit(battleId);
        gameObject.SetActive(false);
    }
}
