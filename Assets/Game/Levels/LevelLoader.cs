﻿/* --- Libraries --- */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using LDtkUnity;

/* --- Definitions --- */
using LDtkLevel = LDtkUnity.Level;

/// <summary>
/// Loads a level from the lDtk Data into the level using the environment.
/// </summary>
public class LevelLoader : MonoBehaviour {

    /* --- Static Variables --- */
    // Layer Names
    public static string AnimalLayer = "Animals";
    public static string FloorLayer = "Floor";

    /* --- Data Structures --- */
    public struct LDtkTileData {

        /* --- Properties --- */
        public Vector2Int vectorID;
        public Vector2Int gridPosition;
        public int index;

        /* --- Constructor --- */
        public LDtkTileData(Vector2Int vectorID, Vector2Int gridPosition, int index = 0) {
            this.vectorID = vectorID;
            this.gridPosition = gridPosition;
            this.index = index;
        }

    }

    /* --- Components --- */
    [SerializeField] public LDtkComponentProject lDtkData;

    /* --- Components --- */
    [SerializeField] private Level level;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        OpenLevel(0);
    }

    /* --- Methods --- */
    public void OpenRoom(string str_id) {
        OpenLevel(Int32.Parse(str_id));
    }

    public virtual void OpenLevel(int id) {
        LDtkLevel ldtkLevel = GetLevelByID(lDtkData, id);
        OpenLevel(ldtkLevel);
    }

    private LDtkLevel GetLevelByID(LDtkComponentProject lDtkData, int id) {

        // Get the json file from the LDtk Data.
        LdtkJson json = lDtkData.FromJson();

        // Read the json data.
        level.gridSize = (int)json.DefaultGridSize;
        level.height = (int)(json.DefaultLevelHeight / json.DefaultGridSize);
        level.width = (int)(json.DefaultLevelWidth / json.DefaultGridSize);

        // Grab the level by the id.
        if (id < json.Levels.Length && id > 0) {
            return json.Levels[id];
        }
        Debug.Log("Could not find room");
        return null;
    }

    protected void OpenLevel(LDtkLevel ldtkLevel) {

        ResetRoom();
        if (ldtkLevel != null) {

            // Load the entity data.
            List<LDtkTileData> entityData = LoadLayer(ldtkLevel, AnimalLayer, level.gridSize);
            List<LDtkTileData> floorData = LoadLayer(ldtkLevel, FloorLayer, level.gridSize);

            // Instatiantate and set up the entities using the data.
            List<Entity> entities = LoadEntities(entityData);
            LoadTiles(level, level.floorMap, floorData);
        }

    }

    private void ResetRoom() {
        // Reset the entities.
        // Reset the checkpoints.
        // Load the tiles.
    }

    private LDtkUnity.LayerInstance GetLayer(LDtkUnity.Level ldtkLevel, string layerName) {
        // Itterate through the layers in the level until we find the layer.
        for (int i = 0; i < ldtkLevel.LayerInstances.Length; i++) {
            LDtkUnity.LayerInstance layer = ldtkLevel.LayerInstances[i];
            if (layer.IsTilesLayer && layer.Identifier == layerName) {
                return layer;
            }
        }
        return null;
    }

    // Set all the tiles in a tilemap.
    private void LoadTiles(Level level, Tilemap tilemap, List<LDtkTileData> data) {
        for (int i = 0; i < level.height; i++) {
            for (int j = 0; j < level.width; j++) {
                // Set the tile.
                Vector2Int gridPosition = new Vector2Int(j, i);
                Vector2Int? vectorID = GetTileID(data, gridPosition);
                // TileBase tile = level.environment.GetTile(vectorID);
                TileBase tile = level.floorTile;
                if (vectorID != null) {
                    tilemap.SetTile((Vector3Int)gridPosition, tile);
                }
            }
        }
    }

    private Vector2Int? GetTileID(List<LDtkTileData> data, Vector2Int gridPosition) {
        for (int i = 0; i < data.Count; i++) {
            if (gridPosition == data[i].gridPosition) {
                return (Vector2Int?)data[i].vectorID;
            }
        }
        return null;
    }

    // Returns the vector ID's of all the tiles in the layer.
    private List<LDtkTileData> LoadLayer(LDtkUnity.Level ldtkLevel, string layerName, int gridSize, List<LDtkTileData> layerData = null) {

        if (layerData == null) { layerData = new List<LDtkTileData>(); }

        LDtkUnity.LayerInstance layer = GetLayer(ldtkLevel, layerName);
        if (layer != null) {
            // Itterate through the tiles in the layer and get the directions at each position.
            for (int index = 0; index < layer.GridTiles.Length; index++) {

                // Get the tile at this index.
                LDtkUnity.TileInstance tile = layer.GridTiles[index];

                // Get the position that this tile is at.
                Vector2Int gridPosition = tile.UnityPx / gridSize;
                Vector2Int vectorID = new Vector2Int((int)(tile.Src[0]), (int)(tile.Src[1])) / gridSize;

                // Construct the data piece.
                LDtkTileData tileData = new LDtkTileData(vectorID, gridPosition, index);
                layerData.Add(tileData);
            }

        }
        return layerData;
    }

    private List<Entity> LoadEntities(List<LDtkTileData> entityData, List<Entity> entities = null) {

        if (entities == null) { entities = new List<Entity>(); }

        for (int i = 0; i < entityData.Count; i++) {
            // Get the entity based on the environment.
            Entity entityBase = level.environment.GetEntityByVectorID(entityData[i].vectorID);
            if (entityBase != null) {

                // Instantiate the entity
                Entity newEntity = Instantiate(entityBase.gameObject, level.GridToWorld(entityData[i].gridPosition), Quaternion.identity, level.transform).GetComponent<Entity>();

                // Set up the entity.
                newEntity.gameObject.SetActive(true);
                newEntity.gridPosition = entityData[i].gridPosition;

                // Add the entity to the list
                entities.Add(newEntity);
            }
        }
        // print("Loaded this many entities: " + entities.Count + " out of " + entityData.Count);
        return entities;
    }

}
