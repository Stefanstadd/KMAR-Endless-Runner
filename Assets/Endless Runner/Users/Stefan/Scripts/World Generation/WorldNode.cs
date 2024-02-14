using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class WorldNode
{
    public Vector3 position;
    public Biome biome;
    public Direction direction;

    public WorldNode next;

    public static int FindIndex(WorldNode head, WorldNode target )
    {
        int result = 0;

        var temp = head;
        while( temp.next != null )
        {
            if ( temp == target )
                return result;

            temp = temp.next;
            result++;
        }

        return -1;
    }

    public static WorldNode NodeAtIndex(WorldNode head,int index )
    {
        int i = 0;

        var temp = head;
        while( temp.next != null )
        {
            if ( i == index )
                return temp;

            temp = temp.next;
        }

        return null;
    }

    public static int Count( WorldNode head )
    {
        int result = 0;
        var temp = head;

        while(temp.next != null )
        {
            result++;
            temp = temp.next;
        }

        return result;
    }

    public static WorldNode Tail (WorldNode head )
    {
        var temp = head;
        while(temp.next != null )
        {
            temp = temp.next;
        }

        return temp;
    }

    public static WorldNode NodeContainingPosition(WorldNode head, Vector3 position )
    {
        WorldNode result = null;

        var temp = head;
        while(temp.next != null )
        {
            if ( PositionInNode (temp, position) )
            {
                result = temp;
                break;
            }

            temp = temp.next;
        }

        return result;
    }

    public static bool PositionInNode(WorldNode node, Vector3 position )
    {
        var tilePos = WorldGenerator.ToTilePosition (position);

        float half = WorldGenerator.TILE_DIMENSION / 2;

        return node.position.x - half > tilePos.x && node.position.x + half < tilePos.x &&
               node.position.y - half > tilePos.y && node.position.y + half < tilePos.y &&
               node.position.z - half > tilePos.z && node.position.z + half < tilePos.z;
    }
}
