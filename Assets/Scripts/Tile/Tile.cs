using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public GameObject GameObject;
    private SpriteRenderer spriteRenderer;
    private TileEngine TileEngine;
    public TileType TileType;
    public Vector2 Position;

    public Tile(TileEngine tileEngine, string tileName, TileType TileType, Vector2 startPosition, GameObject parent = null, float opacity = 1f)
    {
        TileEngine = tileEngine;
        GameObject = new GameObject(tileName);
        this.TileType = TileType;
        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = TileType.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        if (parent != null)
        {
            GameObject.transform.parent = parent.transform;
        }
        SetPosition(startPosition);
        Position = startPosition;
    }

    public void Activate()
    {
        GameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameObject.SetActive(false);
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
        GameObject.transform.position = position;
    }

    public void ChangeTileSprite(TileType tileType)
    {
        TileType = tileType;
        spriteRenderer.sprite = tileType.Sprite;
    }

    public void ChangeNextTo()
    {
        int x = (int)Position.x, y = (int)Position.y;
        var dir = "";
        if (x > 0)
        {
            if (TileEngine.Tiles[x + 1, y].TileType.Type == "wall")
            {
                dir += "w";
            }
        }
        if (x < TileEngine.Width - 1)
        {
            if (TileEngine.Tiles[x - 1, y].TileType.Type == "wall")
            {
                dir += "e";
            }
        }
        if (y < TileEngine.Heigth - 1)
        {
            if (TileEngine.Tiles[x, y + 1].TileType.Type == "wall")
            {
                dir += "n";
            }
        }

        if (y > 0)
        {
            if (TileEngine.Tiles[x, y - 1].TileType.Type == "wall")
            {
                dir += "s";
            }
        }

        if ("wall-" + dir != TileType.Name)
        {
            ChangeTileSprite(TileEngine.GetTileType("wall-" + dir));
            if (dir.Contains("n"))
            {
                TileEngine.Tiles[x, y + 1].ChangeNextTo();
            }
            if (dir.Contains("s"))
            {
                TileEngine.Tiles[x, y - 1].ChangeNextTo();
            }
            if (dir.Contains("w"))
            {
                TileEngine.Tiles[x + 1, y].ChangeNextTo();
            }
            if (dir.Contains("e"))
            {
                TileEngine.Tiles[x - 1, y].ChangeNextTo();
            }

        }
    }
}
