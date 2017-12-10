using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : WorldObject
{
    private TileEngine TileEngine;
    // public Furniture Furniture;

    public Tile(TileEngine tileEngine, string tileName, TileSprite tileSprite, Vector2 startPosition, GameObject parent = null, float opacity = 1f)
    {
        TileName = tileName;
        TileEngine = tileEngine;
        GameObject = new GameObject(tileName);
        this.tileSprite = tileSprite;
        Debug.Log(tileSprite.Name);

        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = tileSprite.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);

        if (parent != null)
        {
            GameObject.transform.parent = parent.transform;
        }
        SetPosition(startPosition);
    }


    public List<Tile> GetNeighbors(bool dinagonal = true)
    {
        List<Tile> neighbors = new List<Tile>();
        Tile tile;

        tile = TileEngine.GetTile((int)Position.x - 1, (int)Position.y);
        if (tile != null)
            neighbors.Add(tile);
        tile = TileEngine.GetTile((int)Position.x + 1, (int)Position.y);
        if (tile != null)
            neighbors.Add(tile);
        tile = TileEngine.GetTile((int)Position.x, (int)Position.y - 1);
        if (tile != null)
            neighbors.Add(tile);
        tile = TileEngine.GetTile((int)Position.x, (int)Position.y + 1);
        if (tile != null)
            neighbors.Add(tile);

        if (dinagonal)
        {
            tile = TileEngine.GetTile((int)Position.x + 1, (int)Position.y + 1);
            if (tile != null)
                neighbors.Add(tile);
            tile = TileEngine.GetTile((int)Position.x - 1, (int)Position.y + 1);
            if (tile != null)
                neighbors.Add(tile);                
            tile = TileEngine.GetTile((int)Position.x + 1, (int)Position.y - 1);
            if (tile != null)
                neighbors.Add(tile);
            tile = TileEngine.GetTile((int)Position.x - 1, (int)Position.y - 1);
            if (tile != null)
                neighbors.Add(tile);
        }

        return neighbors;
    }

    private TileSprite NewSpirte;
    public void Build(TileSprite newSprite)
    {
        NewSpirte = newSprite;
        if (newSprite.Name != tileSprite.Name)
        {
            if (newSprite.Type == ObjectType.Wall)
            {
                BuildWall();
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
            tile = TileEngine.GetTile(x + 1, y);
            if (tile.tileSprite.Type == ObjectType.Wall)
            {
                tile.TileEngine.GetTile(x + 1, y).BuildWall();
            }
        }
        if (x > 0)
        {
            tile = TileEngine.GetTile(x - 1, y);
            if (tile.tileSprite.Type == ObjectType.Wall)
            {
                tile.BuildWall();
            }
        }
        if (y < TileEngine.Heigth - 1)
        {
            tile = TileEngine.GetTile(x, y + 1);
            if (tile.tileSprite.Type == ObjectType.Wall)
            {
                tile.BuildWall();
            }
        }

        if (y > 0)
        {
            tile = TileEngine.GetTile(x, y - 1);
            if (tile.tileSprite.Type == ObjectType.Wall)
            {
                tile.BuildWall();
            }
        }
    }

    public void BuildWall()
    {
        int x = (int)Position.x, y = (int)Position.y;

        string dir = "";
        if (x < TileEngine.Width - 1)
        {
            if (TileEngine.GetTile(x + 1, y).Type == ObjectType.Wall)
            {
                dir += "w";
            }
        }
        if (x > 0)
        {
            if (TileEngine.GetTile(x - 1, y).Type == ObjectType.Wall)
            {
                dir += "e";
            }
        }
        if (y < TileEngine.Heigth - 1)
        {
            if (TileEngine.GetTile(x, y + 1).Type == ObjectType.Wall)
            {
                dir += "n";
            }
        }

        if (y > 0)
        {
            if (TileEngine.GetTile(x, y - 1).Type == ObjectType.Wall)
            {
                dir += "s";
            }
        }

        if (NewSpirte.FullName + dir != tileSprite.FullName)
        {
            ChangeTileSprite(TileEngine.GetTileSprite(NewSpirte.FullName + dir));
            CheckNextTo();
        }
    }
}
