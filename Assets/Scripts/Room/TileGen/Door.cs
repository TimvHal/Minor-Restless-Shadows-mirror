using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public EnemySpawnController AssignedRoom;
    public EnemySpawnController NextRoom;
    public DoorAnimator DoorCheck;
    public DoorDeletion DeleteCheck;
    public GameObject Self;
    public Door(GameObject door)
    {
        Self = door;
        DoorCheck = Self.GetComponent<DoorAnimator>();
        DeleteCheck = Self.GetComponent<DoorDeletion>();
    }

    public void SetController(EnemySpawnController previousRoom, EnemySpawnController nextRoom, int currentTiles)
    {
        AssignedRoom = nextRoom;
        NextRoom = nextRoom;
            
        DoorCheck.dc = nextRoom;
        DoorCheck.ctg = nextRoom.ctg;
        DoorCheck.forward = previousRoom;
        if (currentTiles > 1)
            previousRoom.Exits.Add(DeleteCheck);
    }
    public void SetController(EnemySpawnController assignedRoom)
    {
        DeleteCheck._currentEnemy = assignedRoom.roomEnemySpawnPosition.MaxEnemies;
        AssignedRoom = assignedRoom;
        DoorCheck.dc = assignedRoom;
        DoorCheck.ctg = assignedRoom.ctg;
        AssignedRoom.Entrance = DeleteCheck;
    }
        
}
