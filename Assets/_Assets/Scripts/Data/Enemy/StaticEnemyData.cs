using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StaticEnemyData : List<EnemyData>
{
    public StaticEnemyData()
    {
        List<EnemyData> data = Resources.LoadAll<EnemyData>("Scriptable Objects/Enemies").ToList();
        data = data.OrderBy(x => x.id).ToList();
        AddRange(data);
    }

    public EnemyData GetData(int id)
    {
        for (int i = 0; i < this.Count; i++)
        {
            EnemyData data = this[i];

            if ((int)data.id == id)
            {
                return data;
            }
        }

        Debug.Log("[StaticEnemyData] Not found=" + id);
        return null;
    }

    public EnemyData GetData(EnemyID id)
    {
        return GetData((int)id);
    }
}
