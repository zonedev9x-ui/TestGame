using System.Collections;
using UnityEngine;


[System.Serializable]
public class BaseStats
{
    public float attack;
    public float attackPerSecond;
    public float attackRangeByCells;
    public float splashDamage;
    public float splashRangeByCells;
    public float maxHp;
    public float speedMove;

    public BaseStats()
    {
    }

    public BaseStats(BaseStats data)
    {
        this.attack = data.attack;
        this.attackPerSecond = data.attackPerSecond;
        this.attackRangeByCells = data.attackRangeByCells;
        this.splashDamage = data.splashDamage;
        this.splashRangeByCells = data.splashRangeByCells;
        this.maxHp = data.maxHp;
        this.speedMove = data.speedMove;
    }
}