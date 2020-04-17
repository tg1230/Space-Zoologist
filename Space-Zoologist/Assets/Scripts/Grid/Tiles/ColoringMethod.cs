﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColoringMethod : MonoBehaviour
{
    private float[,] interpolationArray = null;
    [SerializeField] public int affectedRange;
    public virtual void SetColor(float[] composition, Vector3Int cellLocation, TerrainTile tile, Tilemap tilemap, List<TerrainTile> managedTiles, List<TerrainTile> linkedTiles, TileSystem tileSystem, TilePlacementController tilePlacementController)
    {
        tilemap.SetTileFlags(cellLocation, TileFlags.None);
        Color color = GridUtils.RYBValuesToRGBColor(composition, interpolationArray);
        tilemap.SetColor(cellLocation, color);
    }
}