using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyDataList")]
public class EnemyDataListEntity : ScriptableObject
{
    public List<EnemyDataList> DataList = new List<EnemyDataList>();
}