using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileEngine : MonoBehaviour
{
    public List<TileType> TileTypes;
    public string DefaultTileTypeName = "empty";
    private TileType defaultTileType;
    public GameObject TileContainer;
    public int Width;
    public int Heigth;
    public int TileWidth = 32;
    public int TileHeight = 32;


    private Tile[,] Tiles;
    private List<Tile> ShadowTiles = new List<Tile>();

    private TileType GetTileType(string name)
    {
        return TileTypes.FirstOrDefault(c => c.Name == name);
    }

    void Start()
    {
        defaultTileType = GetTileType(DefaultTileTypeName);

        for (var i = 0; i < 100; i++)
        {
            SpawnShadow();
        }

        Tiles = new Tile[Width, Heigth];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Heigth; y++)
            {
                var tile = new Tile("tile-" + x + "_" + y, defaultTileType, new Vector2(x, y), TileContainer);
                Tiles[x, y] = tile;
            }
        }
    }

    public Tile SpawnShadow()
    {
        var shadowTile = new Tile("shadow" + ShadowTiles.Count, defaultTileType, Vector2.zero, TileContainer, 0.5f);
        shadowTile.Deactivate();
        ShadowTiles.Add(shadowTile);
        return shadowTile;
    }

    bool dragging = false, relesed = false;
    int startX, startY;

    int tmpX, tmpY, stopX, stopY;
    // Update is called once per frame
    void Update()
    {
        var buildType = GetTileType("wall");
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitCollider = Physics2D.OverlapPoint(mousePosition);

        var mouseTileX = (int)mousePosition.x;
        var mouseTileY = (int)mousePosition.y;
        if (Input.GetMouseButtonDown(0))
        {
            startX = mouseTileX;
            startY = mouseTileY;

            dragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            relesed = true;
        }

        if (dragging)
        {
            tmpX = startX;
            tmpY = startY;
            stopX = mouseTileX;
            stopY = mouseTileY;
            if (stopX < startX)
            {
                var tmp = stopX;
                stopX = tmpX;
                tmpX = tmp;
            }
            if (stopY < startY)
            {
                var tmp = stopY;
                stopY = tmpY;
                tmpY = tmp;
            }
            Debug.Log("x" + tmpX + "_y" + tmpY + "_sx" + stopX + "_sy" + stopY);
            int shadowUsages = 0;

            foreach (var shadow in ShadowTiles)
            {
                shadow.Deactivate();
            }

            for (var x = tmpX; x <= stopX; x++)
            {
                for (var y = tmpY; y <= stopY; y++)
                {
                    Tile shadow;
                    if (shadowUsages > ShadowTiles.Count() - 1)
                    {
                        shadow = SpawnShadow();
                    }
                    else
                    {
                        shadow = ShadowTiles[shadowUsages];
                    }

                    shadow.Activate();
                    shadow.SetPosition(new Vector2(x, y));
                    shadow.ChangeTileSprite(buildType);

                    shadowUsages++;
                }
            }
        }
        else if (relesed)
        {
            relesed = false;
            for (var x = tmpX; x <= stopX; x++)
            {
                for (var y = tmpY; y <= stopY; y++)
                {
                    Tiles[x, y].ChangeTileSprite(buildType);
                }
            }
            foreach (var shadow in ShadowTiles)
            {
                shadow.Deactivate();
            }
        }
    }
}
