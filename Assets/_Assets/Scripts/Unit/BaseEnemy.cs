using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : BaseCharacter
{
    protected Vector3 destinationMove;
    protected BaseMap map;

    protected float startDistanceToEnd;
    protected Vector3 baseScale = Vector3.one;
    protected float baseNavMeshRadius;

    [SerializeField] protected float minScale = 1f;
    [SerializeField] protected float maxScale = 2.5f;

    protected override void Awake()
    {
        base.Awake();

        if (navMesh != null)
        {
            navMesh.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            navMesh.avoidancePriority = 50;
            baseNavMeshRadius = navMesh.radius;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        TrackingNavMesh();
    }

    public override void Active()
    {
        base.Active();

        map = FindObjectOfType<BaseMap>();

        if (map != null && map.EndPoint != null)
        {
            destinationMove = map.EndPoint.position;

            startDistanceToEnd = Vector3.Distance(transform.position, destinationMove);
            transform.localScale = baseScale * minScale;

            ChangeState(BattleState.Move, true);
        }
        else
        {
            ChangeState(BattleState.Idle, true);
        }
    }

    public override void Active(Vector3 position)
    {
        base.Active(position);

        map = FindObjectOfType<BaseMap>();

        if (map != null && map.EndPoint != null)
        {
            destinationMove = map.EndPoint.position;

            startDistanceToEnd = Vector3.Distance(transform.position, destinationMove);
            transform.localScale = baseScale * minScale;

            ChangeState(BattleState.Move, true);
        }
        else
        {
            ChangeState(BattleState.Idle, true);
        }
    }

    protected override void Moving()
    {
        if (isEnableMove == false || isMoving == false || isPause)
        {
            return;
        }

        if (navMesh != null && navMesh.isOnNavMesh)
        {
            UpdateDestinationMove();
            UpdateScale();
            float distance = Vector3.Distance(transform.position, destinationMove);
            if (distance <= 0.25f)
            {
                Deactive();
                Debug.Log(name + " reached EndPoint");
            }
        }
    }

    protected virtual void UpdateDestinationMove()
    {
        if (navMesh == null)
        {
            return;
        }

        navMesh.SetDestination(destinationMove);
    }

    protected override void UpdateIdle()
    {
        ChangeState(BattleState.Move);
    }

    protected override void UpdateMove()
    {
    }

    public override void Deactive()
    {
        if (navMesh != null && navMesh.isOnNavMesh)
            navMesh.isStopped = true;

        base.Deactive();
    }

    public override void UpdateScale()
    {
        if (map == null || map.EndPoint == null)
            return;

        float currentDistance = Vector3.Distance(transform.position, destinationMove);

        if (startDistanceToEnd <= 0.01f)
        {
            transform.localScale = baseScale * maxScale;
            return;
        }

        float progress = 1f - Mathf.Clamp01(currentDistance / startDistanceToEnd);
        float currentScale = Mathf.Lerp(minScale, maxScale, progress);
        transform.localScale = baseScale * currentScale;
    }
}