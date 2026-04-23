using UnityEngine;
using System.Collections;

public class TransformBasedAnimationEvent : MonoBehaviour
{
    private TransformBasedAnimation controller;

    private BaseUnit unit => controller.unit;

    public void Initilize(TransformBasedAnimation controller)
    {
        this.controller = controller;
    }

    public void OnAttackRelease()
    {
        //unit.OnBasicAttackRelease();
    }

    public void OnAttackDone()
    {
        controller.PlayAnimIdle();
    }
}
