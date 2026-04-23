using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum BulletMovingType
{
    Straight,
    Parabol,
}

public class BaseBullet : MonoBehaviour
{
    public bool isPooling;
    public Transform Transform;
    public float speed = 5f;
    public float rotationSpeed = 2400f;
    public bool isRotate = false;
    public int poolingId { get; set; }
    public bool isPiercing { get; protected set; }
    public GameObject bulletRotation;
    public float durationPerDistance => 1f / speed;
    public bool isTrackingOutMap = true;

    protected bool isActive;
    protected BaseUnit attacker;
    protected BaseFx fxImpact;
    protected bool isHit = false;

    //protected virtual void Update()
    //{
    //    Rotate();
    //    Moving();
    //    Tracking();
    //}

    //public virtual void Active(BaseUnit attacker, Vector3 targetPoint, AttackData attackData = null, BaseFx fxImpact = null, bool isPiercing = false)
    //{
    //    this.attacker = attacker;
    //    this.attackData = attackData;
    //    this.fxImpact = fxImpact;

    //    SetPiercing(isPiercing);

    //    Transform.localScale = Vector3.one;
    //    Transform.position = attacker.firePoint.position;
    //    Transform.FaceUpAxisToPoint(targetPoint);
    //    isActive = true;
    //    isHit = false;

    //    gameObject.SetActive(true);
    //}

    //protected virtual void Moving()
    //{
    //    Transform.Translate(Vector3.up * Time.deltaTime * speed);
    //}

    //protected virtual void Tracking()
    //{
    //    if (GameManager.Instance.mode.map.IsObjectOutOfMarginMap(transform))
    //    {
    //        Deactive();
    //    }
    //}

    //public virtual void SetPiercing(bool isOn)
    //{
    //    isPiercing = isOn;
    //}

    //public virtual void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (attacker == null || isHit && !isPiercing)
    //    {
    //        return;
    //    }

    //    string tagAttacker = attacker.tag;
    //    string tagOpp = string.Empty;

    //    if (tagAttacker == ConstantValues.TAG_TEAM_A)
    //    {
    //        tagOpp = ConstantValues.TAG_TEAM_B;
    //    }
    //    else if (tagAttacker == ConstantValues.TAG_TEAM_B)
    //    {
    //        tagOpp = ConstantValues.TAG_TEAM_A;
    //    }

    //    if (!string.IsNullOrEmpty(tagOpp) && other.transform.root.CompareTag(tagOpp))
    //    {
    //        isHit = true;
    //        if (!isPiercing)
    //        {
    //            Deactive();
    //        }

    //        BaseUnit unit = other.transform.root.GetComponent<BaseUnit>();
    //        if (unit != null && unit is not BaseTower)
    //        {
    //            if (fxImpact == null)
    //                fxImpact = attacker.impactAttack;

    //            unit.TakeAttack(attackData, attacker.impactAttack);
    //        }
    //    }
    //}

    //public virtual void Deactive()
    //{
    //    isActive = false;
    //    gameObject.SetActive(false);
    //    isHit = false;

    //    if (isPooling)
    //    {
    //        PoolingManager.Instance.StoreBullet(this);
    //    }
    //}

    //protected virtual void OnResetMode(object obj)
    //{
    //    try
    //    {
    //        if (gameObject.activeInHierarchy)
    //        {
    //            Deactive();
    //        }
    //    }
    //    catch { }
    //}

    //public virtual void Rotate()
    //{
    //    if (isRotate)
    //    {
    //        float rotationStep = rotationSpeed * Time.deltaTime;
    //        bulletRotation.transform.Rotate(Vector3.forward, rotationStep);
    //    }
    //}
}
