using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    public enum enemyType
    {
        dustboy,
        nurse
    }
    [FormerlySerializedAs("roomPosition")] public EnemySpawnPositions roomEnemySpawnPosition;
    public DoorDeletion Entrance;
    public List<DoorDeletion> Exits;
    public GameObject enemyPrefab;
    public GameObject clearLight;

    public GameObject _player;
    public bool enemiesSpawned = false;
    public bool done = false;
    private bool lightened = false;
    public int _currentEnemies = 0;
    public enemyType type;
    public CheckTriggerGround ctg;
    public Animator anim;
    public UnityEvent ClearEvent = new UnityEvent();

    private bool failsafe;
    private void Start()
    {
        ClearEvent.AddListener(whenCleared);
        _player = GameObject.FindGameObjectWithTag("Player");
        roomEnemySpawnPosition = GetComponent<EnemySpawnPositions>();
        if (GetComponent<GenerateTiles>() == null)
        {
            foreach (var door in Exits)
            {   
                if (door == null) continue;
                door._enemyMax = roomEnemySpawnPosition.MaxEnemies;
            }    
        }
        
    }

    private void Update()
    {
        if (Entrance == null && !failsafe) return;
        if (Entrance.deleted)
        {
            failsafe = true;
            enemiesSpawned = true;
        }
        if (enemiesSpawned && Exits.Count == 0) enabled = false;
        for (int i = 0; i < Exits.Count; i++)
        {
            if (Exits[i] == null)
            {
                Exits.Remove(Exits[i]);
                break;
            }

            Exits[i]._currentEnemy = _currentEnemies;
        }
    }

    public void SpawnEnemies()
    {
        if (!enemiesSpawned || done) return;
        Vector2[] spawnPos = roomEnemySpawnPosition.GetEnemySpawns();
        foreach (Vector3 pos in spawnPos)
        {
            SpawnEnemies(pos);
        }
        done = true;
    }

    public void SpawnEnemies(Vector3 pos)
    {
        switch (type)
        {
            case enemyType.dustboy:
                SpawnDustBoys(pos);
                break;
            case enemyType.nurse:
                SpawnNurses(pos);
                break;
        }
    }
    private void SpawnDustBoys(Vector3 pos)
    {
        GameObject spawned = Instantiate(enemyPrefab, transform.position + pos, Quaternion.identity);
        DustBoyController script = spawned.GetComponent<DustBoyController>();
        script.typing = Random.Range(0, 100) >= 50
            ? DustBoyController.EnemyType.Charging
            : DustBoyController.EnemyType.Shooting;
        script._player = _player;
        script.dc = this;
        _currentEnemies += 1;
    }
    private void SpawnNurses(Vector3 pos)
    {
        GameObject spawned = Instantiate(enemyPrefab, transform.position + pos, Quaternion.identity);
        Nurse script = spawned.GetComponent<Nurse>();
        script.player = _player;
        script.dc = this;
        _currentEnemies += 1;
    }

    public void whenCleared()
    {
        if (clearLight != null) clearLight.SetActive(true);
        else return;
        foreach (var singleDoor in Exits)
        {
            singleDoor.gameObject.GetComponent<DoorAnimator>().ForceDespawn();
        }
        Entrance.gameObject.GetComponent<DoorAnimator>().ForceDespawn();
        lightened = true;
    }

}
