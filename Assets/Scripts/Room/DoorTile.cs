using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : MonoBehaviour
{
    public float tileWidth;
    public float tileHeight;

    public List<Side> attachableSides;
    public Side entrance;
    public DoorDeletion optionalDoor;

    public GameObject door;

    private void Start()
    {
        tileHeight *= 0.98f;
        tileWidth *= 0.98f;
    }

    public enum Side
    {
        north,
        south,
        west,
        east,
        none
    }
    
    public List<Side> getAttachableLocation(DoorTile other)
    {
        List<Side> attachableLocations = new List<Side>();
        foreach (var side in attachableSides)
        {
            foreach (var otherSide in other.attachableSides)
            {
                if (otherSide == getOposite(side))
                {
                    attachableLocations.Add(getOposite(side));
                }
            }
        }

        return attachableLocations;
    }

    public Side getOposite(Side side)
    {
        switch (side)
        {
            case Side.north:
                return Side.south;
                break;
            case Side.west:
                return Side.east;
                break;
            case Side.east:
                return Side.west;
                break;
            case Side.south:
                return Side.north;
                break;
        }

        return Side.none;
    }

    public GameObject spawnDoor(Side side, GameObject Door)
    {
        Vector2 doorPosition = transform.position;
        switch (side)
        {
            case Side.north:
                doorPosition.y += tileHeight / 2;
                break;
            case Side.east:
                doorPosition.x += tileWidth / 2;
                break;
            case Side.west:
                doorPosition.x -= tileWidth / 2;
                break;
            case Side.south:
                doorPosition.y -= tileHeight / 2 ;
                break;
        }

        GameObject DoorClone = Instantiate(Door, doorPosition, Quaternion.identity);
        return DoorClone;
    }
    

    public Vector3 getTransformedPosition(Side side, DoorTile other)
    {
        Vector2 Position = transform.position;
        switch (side)
        {
            case Side.north:
                Position.y -= (tileHeight /2 + other.tileHeight/2) * 0.98f;
                break;
            case Side.west:
                Position.x += (tileWidth /2 + other.tileWidth/2) * 0.98f;
                break;
            case Side.east:
                Position.x -= (tileWidth /2 + other.tileWidth/2) * 0.98f;
                break;
            case Side.south:
                Position.y += (tileHeight /2 + other.tileHeight/2) * 0.98f;
                break;
        }
        return Position;
    }

}
