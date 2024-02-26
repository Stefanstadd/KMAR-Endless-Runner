using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Coms class will make communication between classes, objects, managers, ect. way easier by finding and caching the best active instance of a type, this class will speed up coding efficiency and readability
/// </summary>
public static class Coms
{
    private static WorldGenerator worldGenerator;

    public static WorldGenerator WorldGenerator
    {
        get
        {
            return Find (ref worldGenerator);
        }
    }

    private static TerrainGenerator terrainGenerator;

    public static TerrainGenerator TerrainGenerator
    {
        get
        {
            return Find (ref terrainGenerator);
        }
    }

    private static ChunkManager chunkManager;

    public static ChunkManager ChunkManager
    {
        get
        {
            return Find (ref chunkManager);
        }
    }



    // Private methods
    private static T Find<T> (ref T original ) where T : Object
    {
        if(original == null)
            original = Object.FindObjectOfType<T> (false);
        return original;
    }

    private static T FindInactive<T> ( ref T original ) where T : Object
    {
        if ( original == null )
            original = Object.FindObjectOfType<T> (true);
        return original;
    }
}
