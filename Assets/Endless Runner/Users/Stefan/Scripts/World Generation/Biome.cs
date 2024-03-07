using UnityEngine;

[System.Serializable]
public class Biome
{
    [Header ("Biome Data")]
    public string biomeName;

    public Color biomeColor;
    public float sideWayChance;

    [Header ("Obstacles")]
    public float generalObstacleChance;

    [Header ("Tiles")]
    public TileData forwardTile;

    public TileData forwardSidewaysTile;
    public TileData leftTile;
    public TileData rightTile;
}