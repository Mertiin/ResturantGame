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
    System.Random random = new System.Random();
    private List<TileSprite> TileSprites;
    public string DefaultTileTypeName = "empty";
    private TileSprite defaultTileSprite;
    public GameObject TileContainer;
    public GameObject FurnitureContainer;
    public int Width;
    public int Heigth;

    private Tile[,] Tiles;

    public List<Furniture> Furnitures = new List<Furniture>();
    private List<Tile> ShadowTiles = new List<Tile>();
    public List<Character> Characters = new List<Character>();

    public TileSprite GetTileSprite(string name)
    {
        return TileSprites.FirstOrDefault(c => c.FullName.ToLower() == name.ToLower());
    }

    public Furniture GetFurnitureFromPosition(int x, int y)
    {
        return Furnitures.FirstOrDefault(c => (c.IsInBound(x, y)));
    }

    void Start()
    {
        var spritedata = File.ReadAllText("assets/spritedata.json");
        TileSprites = JsonConvert.DeserializeObject<List<TileSprite>>(spritedata);
        var sprite = GetTileSprite("teen");
        Debug.Log("test_" + sprite.Name);

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
        Tiles[0, 0].GetNeighbors();
        for (var i = 0; i < 100; i++)
        {
            var character = new Character(new Vector2(random.Next(0, Width - 1), random.Next(0, Heigth - 1)), GetTileSprite("teen"), this);
            Characters.Add(character);
            character.TargetPosition = new Vector2(random.Next(0, Width - 1), random.Next(0, Heigth - 1));
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Heigth)
            return null;
        return Tiles[x, y];
    }

    public Tile GetTile(Vector2 p)
    {
        return GetTile((int)p.x, (int)p.y);
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
        foreach (var item in Characters)
        {
            item.Update();
        }

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

            if (buildSprite.Type == ObjectType.Wall)
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
            else if (buildSprite.Type == ObjectType.Furniture)
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
                    var tile = Tiles[x, y];
                    if (buildSprite.Name == "empty")
                    {
                        var furniture = GetFurnitureFromPosition(x, y);
                        if (furniture != null)
                        {
                            furniture.Destroy();
                            Furnitures.Remove(furniture);
                        }
                    }
                    else if (buildSprite.Type == ObjectType.Furniture)
                    {
                        var furniture = GetFurnitureFromPosition(x, y);
                        if (furniture == null)
                        {
                            Furnitures.Add(new Furniture(tile, buildSprite, FurnitureContainer));
                        }
                    }
                    else
                    {
                        tile.Build(buildSprite);
                    }
                }
            }

            foreach (var shadow in ShadowTiles)
            {
                shadow.Destroy();
            }
            ShadowTiles.Clear();

            foreach (var item in Characters)
            {
                item.ClearPath();
            }

            foreach(var item in Tiles){
                item.SetColor(new Color(1,1,1,1));
            }
        }
    }
}
