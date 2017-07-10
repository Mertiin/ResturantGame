using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public class TileEngine : MonoBehaviour
{
    private List<TileSprite> TileSprites;
    public string DefaultTileTypeName = "empty";
    private TileSprite defaultTileSprite;
    public GameObject TileContainer;
    public int Width;
    public int Heigth;

    public Tile[,] Tiles;

    public List<Furniture> Furnitures;
    private List<Tile> ShadowTiles = new List<Tile>();

    public TileSprite GetTileSprite(string name)
    {
        return TileSprites.FirstOrDefault(c => c.FullName.ToLower() == name.ToLower());
    }

    void Start()
    {
        var spritedata = File.ReadAllText("assets/spritedata.json");
        TileSprites = JsonConvert.DeserializeObject<List<TileSprite>>(spritedata);

        foreach (var tileType in TileSprites)
        {
            var x = tileType.Sprite;
        }


        GameObject obj = new GameObject();

        obj.transform.position = new Vector2(-1, -1);

        defaultTileSprite = GetTileSprite(DefaultTileTypeName);

        Tiles = new Tile[Width, Heigth];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Heigth; y++)
            {
                var tile = new Tile(this, "tile-" + x + "_" + y, defaultTileSprite, new Vector2(x, y), TileContainer);
                Tiles[x, y] = tile;
            }
        }
    }

    public Tile SpawnShadow()
    {
        var shadowTile = new Tile(this, "shadow" + ShadowTiles.Count, defaultTileSprite, Vector2.zero, TileContainer, 0.5f);
        shadowTile.Deactivate();
        ShadowTiles.Add(shadowTile);
        return shadowTile;
    }

    bool dragging = false, relesed = false;
    int startX, startY;

    int tmpX, tmpY, stopX, stopY;
    // Update is called once per frame
    TileSprite buildSprite;

    public void SetBuild(string sprite)
    {
        buildSprite = GetTileSprite(sprite);
    }

    void Update()
    {
        if (buildSprite == null)
        {
            buildSprite = GetTileSprite("wall-");
        }

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitCollider = Physics2D.OverlapPoint(mousePosition);

        var mouseTileX = (int)mousePosition.x;
        var mouseTileY = (int)mousePosition.y;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            startX = mouseTileX;
            startY = mouseTileY;

            dragging = true;
        }
        else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
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

            if (buildSprite.Type == TileType.Wall)
            {
                if (stopX - tmpX < stopY - tmpY)
                {
                    stopX = startX;
                    tmpX = startX;
                }
                else
                {
                    stopY = startY;
                    tmpY = startY;
                }
            }
            else if (buildSprite.Type == TileType.Furniture)
            {
                stopX = mouseTileX;
                tmpX = mouseTileX;
                stopY = mouseTileY;
                tmpY = mouseTileY;
            }

            if (stopX >= Width)
            {
                stopX = Width - 1;
            }
            if (tmpX < 0)
            {
                tmpX = 0;
            }
            if (stopY >= Heigth)
            {
                stopY = Heigth - 1;
            }
            if (tmpY < 0)
            {
                tmpY = 0;
            }

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
                    shadow.ChangeTileSprite(buildSprite);

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
                    Tiles[x, y].Build(buildSprite);
                }
            }
            foreach (var shadow in ShadowTiles)
            {
                shadow.Destroy();
            }
            ShadowTiles.Clear();
        }
    }
}
