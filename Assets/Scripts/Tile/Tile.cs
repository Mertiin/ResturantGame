using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : TileObject
{
    private TileEngine TileEngine;
    public Furniture Furniture;

    public Tile(TileEngine tileEngine, string tileName, TileSprite tileSprite, Vector2 startPosition, GameObject parent = null, float opacity = 1f)
    {
        TileName = tileName;
        TileEngine = tileEngine;
        GameObject = new GameObject(tileName);
        TileSprite = tileSprite;
        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = TileSprite.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        if (parent != null)
        {
            GameObject.transform.parent = parent.transform;
        }
        SetPosition(startPosition);
        Position = startPosition;
    }

    public void AddFurniture(TileSprite tileSprite)
    {
        if (this.TileSprite.Walkspeed > 0)
        {
            if (Furniture != null)
                Furniture.Destroy();
            Furniture = new Furniture(this, tileSprite);
        }
    }

    private TileSprite NewSpirte;
    public void Build(TileSprite newSprite)
    {
        NewSpirte = newSprite;
        if (newSprite.Name != TileSprite.Name)
        {
            if (newSprite.Type == TileType.Wall)
            {
                BuildWall();
            }
            else if (newSprite.Type == TileType.Furniture)
            {
                AddFurniture(newSprite);
            }
            else
            {
                ChangeTileSprite(newSprite);
                CheckNextTo();
            }
        }
    }

    public void CheckNextTo()
    {
        Tile tile;
        int x = (int)Position.x;
        int y = (int)Position.y;
        if (x < TileEngine.Width - 1)
        {
            tile = TileEngine.Tiles[x + 1, y];
            if (tile.TileSprite.Type == TileType.Wall)
            {
                tile.TileEngine.Tiles[x + 1, y].BuildWall();
            }
        }
        if (x > 0)
        {
            tile = TileEngine.Tiles[x - 1, y];
            if (tile.TileSprite.Type == TileType.Wall)
            {
                tile.BuildWall();
            }
        }
        if (y < TileEngine.Heigth - 1)
        {
            tile = TileEngine.Tiles[x, y + 1];
            if (tile.TileSprite.Type == TileType.Wall)
            {
                tile.BuildWall();
            }
        }

        if (y > 0)
        {
            tile = TileEngine.Tiles[x, y - 1];
            if (tile.TileSprite.Type == TileType.Wall)
            {
                tile.BuildWall();
            }
        }
    }

    public void BuildWall()
    {
        int x = (int)Position.x, y = (int)Position.y;
        if (Furniture != null)
        {
            Furniture.Destroy();
            Furniture = null;
        }

        string dir = "";
        if (x < TileEngine.Width - 1)
        {
            if (TileEngine.Tiles[x + 1, y].TileSprite.Type == TileType.Wall)
            {
                dir += "w";
            }
        }
        if (x > 0)
        {
            if (TileEngine.Tiles[x - 1, y].TileSprite.Type == TileType.Wall)
            {
                dir += "e";
            }
        }
        if (y < TileEngine.Heigth - 1)
        {
            if (TileEngine.Tiles[x, y + 1].TileSprite.Type == TileType.Wall)
            {
                dir += "n";
            }
        }

        if (y > 0)
        {
            if (TileEngine.Tiles[x, y - 1].TileSprite.Type == TileType.Wall)
            {
                dir += "s";
            }
        }

        if (NewSpirte.FullName + dir != TileSprite.FullName)
        {
            ChangeTileSprite(TileEngine.GetTileSprite(NewSpirte.FullName + dir));
            CheckNextTo();
        }
    }
}
