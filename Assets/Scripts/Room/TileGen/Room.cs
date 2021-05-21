using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenRoom
{
    public DoorTile RoomInformation;
    public EnemySpawnController Spawner;
    public GameObject Self;
    public DoorTile.Side LocationSelector;
    public Door DoorSelector;
    public EnemySpawnPositions Positions;
    public GenRoom(GameObject room)
    {
        Self = room;
        RoomInformation = room.GetComponent<DoorTile>();
        Spawner = room.GetComponent<EnemySpawnController>();
        Positions = room.GetComponent<EnemySpawnPositions>();
    }

    public Door AssignDoorLocation(GenRoom nextRoom, GenRoom previousRoom, DoorTile.Side attachAbleLocation, int currentTiles)
    {
        Door door = new Door(nextRoom.RoomInformation.spawnDoor(attachAbleLocation, nextRoom.RoomInformation.door));
        door.DeleteCheck.direction = attachAbleLocation;
        door.Self.transform.parent = nextRoom.Self.transform;
        if (currentTiles > 0)
            door.SetController(previousRoom.Spawner, nextRoom.Spawner, currentTiles);
        else door.SetController(nextRoom.Spawner);
        nextRoom.DoorSelector = door;
        nextRoom.Spawner.Entrance = door.DeleteCheck;
        return door;
    }
}
