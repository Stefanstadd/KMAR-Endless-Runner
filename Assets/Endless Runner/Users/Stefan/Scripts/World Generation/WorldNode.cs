using UnityEngine;

[System.Serializable]
public class WorldNode
{
    public Vector3 position;
    public Biome biome;
    public Direction direction;
    public TileType type;

    //Lane States
    public LaneState leftLane, middleLane, rightLane;

    public WorldNode next;

    public static int FindIndex ( WorldNode head, WorldNode target )
    {
        int result = 0;

        var temp = head;
        while ( temp.next != null )
        {
            if ( temp == target )
                return result;

            temp = temp.next;
            result++;
        }

        return -1;
    }

    public static WorldNode NodeAtIndex ( WorldNode head, int index )
    {
        int i = 0;

        var temp = head;
        while ( temp.next != null )
        {
            if ( i == index )
                return temp;

            temp = temp.next;
            i++;
        }

        return null;
    }

    public static int Count ( WorldNode head )
    {
        int result = 1;
        var temp = head;

        while ( temp.next != null )
        {
            result++;
            temp = temp.next;
        }

        return result;
    }

    public static WorldNode Tail ( WorldNode head )
    {
        var temp = head;
        while ( temp.next != null )
        {
            temp = temp.next;
        }

        return temp;
    }

    public static WorldNode NodeContainingPosition ( WorldNode head, Vector3 position )
    {
        WorldNode result = null;

        var temp = head;
        while ( temp.next != null )
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

    public static bool PositionInNode ( WorldNode node, Vector3 position )
    {
        return PositionInNode (node, position, WorldGenerator.TILE_DIMENSION / 2);
    }

    public static bool PositionInNode ( WorldNode node, Vector3 position, float radius )
    {
        //var tilePos = WorldGenerator.ToTilePosition (position);

        return node.position.x - radius < position.x && node.position.x + radius > position.x &&
               node.position.y - radius < position.y && node.position.y + radius > position.y &&
               node.position.z - radius < position.z && node.position.z + radius > position.z;
    }
}