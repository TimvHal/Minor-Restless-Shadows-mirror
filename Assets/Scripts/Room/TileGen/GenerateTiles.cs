using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

// Dear programmer
// when i wrote this code, only god and
// I knew how it worked
// Now, only god knows it!
// 
// Therefore, if you are trying to optimise
// this routine and it fails (most surely),
// please increase this counter as a 
// warning for the next person
// 
// total_hours_wasted_here = 4
//
public class GenerateTiles : MonoBehaviour    
{
    public List<GameObject> tiles;
    public List<GameObject> endTiles;
    public List<GameObject> FinalTiles;
    public List<GameObject> treasurerooms;
    public int maxTiles;
    public int currentTiles = 0;
    private List<GameObject> instantiated = new List<GameObject>();
    public GameObject chestsPrefab;
    public List<Vector3> roomLocations = new List<Vector3>();
    private List<Tuple<DoorTile.Side,GameObject>> looseEnds = new List<Tuple<DoorTile.Side,GameObject>>();
    private GameObject current;
    private DoorTile _selfTile;
    public GameObject BlockingObstacle;

    public bool finalised = false;
    public int treasureroom = 0;
    public GameObject treasuroomInit;

    public bool isSpecial = false;
    public GameObject SpecialDoor;
    private GameObject currentPrefab = null;
    

    
    // Start is called before the first frame update
    void Start()
    {
        current = gameObject;
        _selfTile = GetComponent<DoorTile>();
        SpawnRooms();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // SpawnRooms();
    }
    
    
    void SpawnRooms()
    {
        for (int i = 0; i + looseEnds.Count < maxTiles; i++ )
        {
            GenRoom nextRoom = SpawnRoom(new GenRoom(current), tiles[new Random().Next(tiles.Count)], i);
            CheckTreasureRooms(i);
            instantiated.Add(nextRoom.Self);
            current = nextRoom.Self;
        }
        Tuple<DoorTile.Side,GameObject> lastRoom = looseEnds.Last();
        looseEnds.Remove(lastRoom);
        foreach (var loosEnd in looseEnds)
        {
            SpawnLoosEnd(loosEnd, false, maxTiles);
        }
        GenRoom Final = SpawnLoosEnd(lastRoom, true, maxTiles);
        
        Final.RoomInformation.optionalDoor = Final.DoorSelector.DeleteCheck;
        try
        {
            Final.Self.GetComponent<DisableSnowball>().dc = Final.DoorSelector.DeleteCheck;
        }
        catch (Exception e)
        {
            Final.Self.GetComponent<CheckPlagaActive>().doorAnimator = Final.DoorSelector.DoorCheck;
        }
    }
    
    GenRoom SpawnRoom(GenRoom currentRoom, GameObject nextPrefab, int currentTileCount)
    {
        GenRoom nextRoom = TryNextRoom(currentRoom, nextPrefab);
        currentRoom.RoomInformation.attachableSides.Remove(nextRoom.RoomInformation.getOposite(nextRoom.LocationSelector));
        DeleteFromLooseEndListIfFound(current, nextRoom.RoomInformation.getOposite(nextRoom.LocationSelector));
        nextRoom.RoomInformation.attachableSides.Remove(nextRoom.LocationSelector);
            
        List<DoorTile.Side> looseEndDoors = nextRoom.RoomInformation.attachableSides;
        looseEndDoors.Remove(nextRoom.LocationSelector);
        nextRoom.Self.transform.position =
            currentRoom.RoomInformation.getTransformedPosition(nextRoom.LocationSelector, nextRoom.RoomInformation);
        roomLocations.Add(nextRoom.Self.transform.position);
        foreach (var end in looseEndDoors)
            looseEnds.Add(new Tuple<DoorTile.Side, GameObject>(nextRoom.RoomInformation.getOposite(end),nextRoom.Self));

        nextRoom.AssignDoorLocation(nextRoom, currentRoom, nextRoom.LocationSelector, currentTileCount);
        currentPrefab = nextPrefab;
        return nextRoom;
    }
    
    private GenRoom TryNextRoom(GenRoom currentRoom, GameObject nextPrefab)
    {
        GenRoom nextRoom = new GenRoom(Instantiate(nextPrefab, transform.position, Quaternion.identity) as GameObject);
        List<DoorTile.Side> doorLocation = currentRoom.RoomInformation.getAttachableLocation(nextRoom.RoomInformation);
        DoorTile.Side location = doorLocation.Count > 0 ? doorLocation[new Random().Next(doorLocation.Count-1)] : DoorTile.Side.none;
        if (location == DoorTile.Side.none || (treasurerooms.Contains(nextPrefab) && treasurerooms.Contains(currentPrefab)))
        {
            Destroy(nextRoom.Self);
            return TryNextRoom(currentRoom, tiles[new Random().Next(tiles.Count)]);
        }
        if (treasurerooms.Contains(nextPrefab)) treasureroom += 1;
        nextRoom.LocationSelector = location;
        return nextRoom;
    }
    
    
  
    private GenRoom SpawnLoosEnd(Tuple<DoorTile.Side, GameObject> loosEnd, bool final, int currentTileCount)
    {
        GenRoom currentRoom = new GenRoom(loosEnd.Item2);
        GenRoom nextRoom = new GenRoom(
            Instantiate(
                getRightEndTIle(loosEnd.Item1, final ? FinalTiles : endTiles), 
                transform.position, 
                Quaternion.identity
            )
        );
        var pos = currentRoom.RoomInformation.getTransformedPosition(loosEnd.Item1, nextRoom.RoomInformation);
        if (roomLocations.Contains(pos))
        {
            Destroy(nextRoom.Self);
            return null;
        }

        RemoveObjectInTheWay(pos);
        nextRoom.Self.transform.position = pos;
        roomLocations.Add(pos);
            
        nextRoom.AssignDoorLocation(nextRoom, currentRoom, loosEnd.Item1, currentTileCount);
        nextRoom.RoomInformation.attachableSides.Remove(loosEnd.Item1);
        instantiated.Add(nextRoom.Self);
        if (treasureroom <= 0 && !final)
        {
            nextRoom.Positions.MaxEnemies = 0;
            nextRoom.Spawner.clearLight.SetActive(true);
            var chests = Instantiate(chestsPrefab);
            chests.transform.parent = nextRoom.Self.transform;
            chests.transform.localPosition = Vector3.zero;
            treasuroomInit = chests;
            treasureroom = 1;
        }

        return nextRoom;
    }
    
    private void CheckTreasureRooms(int currentTileCount)
    {
        if (currentTileCount == 2)
        {
            tiles.AddRange(treasurerooms);
        }
        if (treasureroom > 2)
        {
            foreach (var room in treasurerooms)
            {
                tiles.Remove(room);
            }
        }
    }
    GameObject getRightEndTIle(DoorTile.Side side, List<GameObject> tiles)
    {
        foreach (var gameObject in tiles)
        {
            if (gameObject.GetComponent<DoorTile>().attachableSides[0] == side) return gameObject;
        }

        return null;
    }

    void DeleteFromLooseEndListIfFound(GameObject current, DoorTile.Side newSide)
    {
        newSide = _selfTile.getOposite(newSide);
        for (int i = 0; i < looseEnds.Count; i++)
        {
            if (looseEnds[i].Item2 == current && looseEnds[i].Item1 == newSide) looseEnds.Remove(looseEnds[i]);
        }
    }

    private void RemoveObjectInTheWay(Vector3 _position)
    {   
        for (int i = 0; i < instantiated.Count; i++)
        {
            var end = instantiated[i];
            DoorTile tile = end.GetComponent<DoorTile>();
            Vector3Int roundedEnd = Vector3Int.CeilToInt( end.transform.position);
            Vector3Int roundedPos = Vector3Int.CeilToInt( _position);
            
            if (roundedPos.x <= roundedEnd.x + tile.tileWidth / 2 && roundedPos.x >= roundedEnd.x - tile.tileWidth / 2 &&
                roundedPos.y <= roundedEnd.y + tile.tileHeight / 2  && roundedPos.y >= roundedEnd.y - tile.tileHeight / 2)
            {
                Instantiate(
                    BlockingObstacle, 
                    end.GetComponent<EnemySpawnController>().Entrance.transform.localPosition + end.transform.position,
                    Quaternion.identity
                    );
                Destroy(end);
            }
        }
    }
}
