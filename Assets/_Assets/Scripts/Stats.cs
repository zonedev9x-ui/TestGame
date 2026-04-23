using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stats
{
    public float attack;
    public float attackPerSecond;
    public float attackRangeByCells;
    public float critRate;
    public float critDamage;
    public float splashDamage;
    public float splashRangeByCells;
    public float bonusBullet;
    public float instantKillRate;
    public float instantKillRateFlyingEnemy;
    public float hpPercentKill;
    public float doubleAttack;
    public float tripleAttack;
    public float bonusDamagePerAilmentStatus;
    public float extraDamageToBoss;
    public float extraDamageToStunnedEnemy;
    public float extraDamageToSlowedEnemy;
    public float extraDamageToFlyingEnemy;
    public float extraDamgeToImmuneEnemy;
    public float multiplyAttack;
    public float multiplyProjectileSize;

    public float maxHp;
    public float damageTakenReduce;
    public float damageTakenAmplify;
    public float immuneCC;
    public float immuneInstantKill;
    public float immuneHpPercentKill;
    public float blockDamageRate;

    public float speedMove;
    public float cardObtainedWhenPlacingTower;
    public float cardObtainedWhenUpgradingTower;
    public float autoPickCardBuff; //todo: nghialm
    public float pickBuffWhenEndWave;
    public float expDropRate;
    public float expGainBonus;
    public float hpPotionDropRateWhenKillingEnemy;
    public float hpPotionDropRateWhenAttacked;
    public float bonusTowerAttackCardBuff;
    public float shovelReplacePickaxeRate;

    public Dictionary<int, List<float>> towerObtainedEachWave = new Dictionary<int, List<float>>();
    public Dictionary<int, List<float>> pickAxeObtainedEachWave = new Dictionary<int, List<float>>();
    public Dictionary<int, List<float>> bombObtainedEachWave = new Dictionary<int, List<float>>();

    public Stats() { }

    public Stats(BaseStats baseStats)
    {
        attack = baseStats.attack;
        attackPerSecond = baseStats.attackPerSecond;
        attackRangeByCells = baseStats.attackRangeByCells;
        splashDamage = baseStats.splashDamage;
        splashRangeByCells = baseStats.splashRangeByCells;
        maxHp = baseStats.maxHp;
        speedMove = baseStats.speedMove;
    }
}
