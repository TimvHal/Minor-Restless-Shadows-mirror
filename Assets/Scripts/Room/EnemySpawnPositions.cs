using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPositions : MonoBehaviour
{
    public int MaxEnemies;
    public Vector2 entrance;

    public Vector2[] doors;

    public List<Vector2> enemyPositions;

    public Vector2[] GetEnemySpawns()
    {
        Vector2[] result = new Vector2[MaxEnemies];
        for (int i = 0; i < MaxEnemies; i++)
        {
            result[i] = enemyPositions[Random.Range(0, MaxEnemies-1)];
            enemyPositions.Remove(result[i]);
            MaxEnemies -= 1;
        }

        return result;
    }
}
