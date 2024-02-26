using UnityEngine;

[System.Serializable]
public class Biome
{
    [Header ("Biome Data")]
    public string biomeName;

    public Color biomeColor;
    public float sideWayChance;

    [Header ("Tiles")]
    public GameObject forwardTile;

    public GameObject forwardSidewaysTile;
    public GameObject leftTile;
    public GameObject rightTile;
}