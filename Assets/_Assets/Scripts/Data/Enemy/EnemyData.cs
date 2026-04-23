using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy-", menuName = "Scriptable Objects/Enemies/EnemyData")]

public class EnemyData : MonoBehaviour
{
    public EnemyID id;
    public EnemyType type;
    public BaseEnemy prefabIngame;

}
