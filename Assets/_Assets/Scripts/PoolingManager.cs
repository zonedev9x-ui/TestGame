using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolingManager : Singleton<PoolingManager>
{
    [Header("Groups")]
    public Transform groupFx;
    public Transform groupCombatText;
    public Transform groupBullet;
    public Transform groupOrb;

    public Dictionary<int, ObjectPooling<BaseBullet>> poolBullets = new Dictionary<int, ObjectPooling<BaseBullet>>();
    private Dictionary<string, int> bulletIdByNames = new Dictionary<string, int>();
    private int idBullet;

    //public ObjectPooling<CombatText> poolCombatText = new ObjectPooling<CombatText>();
    public Dictionary<int, ObjectPooling<BaseEnemy>> poolEnemies = new Dictionary<int, ObjectPooling<BaseEnemy>>();
    private static Dictionary<int, BaseEnemy> enemyPrefabs = new Dictionary<int, BaseEnemy>();

    public Dictionary<int, ObjectPooling<BaseFx>> poolFx = new Dictionary<int, ObjectPooling<BaseFx>>();
    public Dictionary<int, int> activeFxInstances = new Dictionary<int, int>();
    private const int MAX_FX_INSTANCES_IN_SCENE = 20;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        poolBullets.Clear();
        bulletIdByNames.Clear();
        idBullet = 0;

        poolEnemies.Clear();
        enemyPrefabs.Clear();

        poolFx.Clear();
        activeFxInstances.Clear();
    }

    #region Fx   
    public BaseFx GetFx(int id, BaseFx fxPrefab)
    {
        if (poolFx.ContainsKey(id) == false)
        {
            poolFx[id] = new ObjectPooling<BaseFx>();
        }

        int currentActiveFx = GetActiveInstances(id);
        if (currentActiveFx > MAX_FX_INSTANCES_IN_SCENE)
        {
            return null;
        }

        BaseFx fx = poolFx[id].New();
        if (fx == null)
        {
            fx = Instantiate(fxPrefab, groupFx);
        }

        AddFxInScene(id);

        return fx;
    }

    public void StoreFx(BaseFx fx)
    {
        int id = fx.idFx;

        if (id == 0)
        {
            DebugCustom.LogError("[StoreFx] fx=" + fx.name);
            return;
        }

        if (poolFx.ContainsKey(id) == false)
        {
            poolFx[id] = new ObjectPooling<BaseFx>();
        }

        if (poolFx[id].Contains(fx) == false)
        {
            poolFx[id].Store(fx);
        }

        fx.transform.parent = groupFx;
        transform.position = Vector3.zero;
        HideFxInScene(id);
    }

    private int GetActiveInstances(int id)
    {
        int current = 0;

        if (activeFxInstances.ContainsKey(id))
        {
            current = activeFxInstances[id];
        }

        return current;
    }

    private void AddFxInScene(int id)
    {
        int current = 0;

        if (activeFxInstances.ContainsKey(id))
        {
            current = activeFxInstances[id];
        }

        current++;
        activeFxInstances[id] = current;
    }

    private void HideFxInScene(int id)
    {
        int current = 0;

        if (activeFxInstances.ContainsKey(id))
        {
            current = activeFxInstances[id];
        }

        current--;
        activeFxInstances[id] = current;

        if (current < 0)
        {
            activeFxInstances.Remove(id);
        }
    }
    #endregion

    #region Bullets
    public BaseBullet GetBullet(BaseBullet prefab)
    {
        int id = 0;
        string prefabName = prefab.name;
        if (bulletIdByNames.ContainsKey(prefabName))
        {
            id = bulletIdByNames[prefabName];
        }
        else
        {
            idBullet++;
            id = idBullet;
            bulletIdByNames[prefabName] = idBullet;
        }

        if (!poolBullets.ContainsKey(id))
        {
            poolBullets[id] = new ObjectPooling<BaseBullet>();
        }

        BaseBullet bullet = poolBullets[id].New();
        if (bullet == null)
            bullet = Instantiate(prefab);

        bullet.poolingId = id;
        return bullet;
    }

    public void StoreBullet(BaseBullet bullet)
    {
        int id = bullet.poolingId;

        if (poolBullets.ContainsKey(id))
        {
            if (poolBullets[id].Contains(bullet) == false)
                poolBullets[id].Store(bullet);

            return;
        }

        DebugCustom.LogError("Bullet type not found=" + id);
    }
    #endregion

    #region Enemies
    //private Dictionary<int, int> totalCreatedEnemies = new Dictionary<int, int>();

    public BaseEnemy GetEnemy(int id)
    {
        BaseEnemy enemy = null;

        if (poolEnemies.ContainsKey(id))
        {
            enemy = poolEnemies[id].New();
        }

        if (enemy == null)
        {
            BaseEnemy prefab = GetEnemyPrefab(id);
            if (prefab != null)
            {
                enemy = Instantiate(prefab);

                //int created = 0;
                //if (totalCreatedEnemies.ContainsKey(id))
                //{
                //    created = totalCreatedEnemies[id];
                //}
                //created++;
                //totalCreatedEnemies[id] = created;
                //DebugCustom.LogFormat("[CreateEnemy] id={0}, total={1}", (EnemyID)id, created);
            }
        }

        return enemy;
    }

    public BaseEnemy GetEnemy(EnemyID id)
    {
        return GetEnemy((int)id);
    }

    public void StoreEnemy(BaseEnemy enemy)
    {
        if (!poolEnemies.ContainsKey(enemy.id))
        {
            poolEnemies[enemy.id] = new ObjectPooling<BaseEnemy>();
        }

        poolEnemies[enemy.id].Store(enemy);
    }

    private BaseEnemy GetEnemyPrefab(int id)
    {
        BaseEnemy prefab = null;

        if (enemyPrefabs.ContainsKey(id))
        {
            prefab = enemyPrefabs[id];
        }
        else
        {

            EnemyData enemyData = GameData.staticData.enemies.GetData(id);
            if (enemyData != null)
            {
                prefab = enemyData.prefabIngame;
                enemyPrefabs[id] = prefab;
            }
        }

        if (prefab == null)
        {
            DebugCustom.LogError("[GetEnemyPrefab] Not found=" + id);
        }

        return prefab;
    }
    #endregion

    //#region Exp Orb
    //public ExpOrb GetExpOrb()
    //{
    //    ExpOrb orb = poolExpOrb.New();

    //    if (orb == null)
    //    {
    //        orb = Instantiate(prefabExpOrb);
    //    }

    //    return orb;
    //}

    //public void StoreExpOrb(ExpOrb orb)
    //{
    //    orb.gameObject.SetActive(false);
    //    orb.transform.SetParent(groupOrb);
    //    poolExpOrb.Store(orb);
    //}

    //public CoinOrb GetCoinOrb()
    //{
    //    CoinOrb orb = poolCoinOrb.New();
    //    if (orb == null)
    //    {
    //        orb = Instantiate(prefabCoinOrb);
    //    }
    //    return orb;
    //}

    //public void StoreCoinOrb(CoinOrb orb)
    //{
    //    orb.gameObject.SetActive(false);
    //    orb.transform.SetParent(groupOrb);
    //    poolCoinOrb.Store(orb);
    //}
    //#endregion
}
