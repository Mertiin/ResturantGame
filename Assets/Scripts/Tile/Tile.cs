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
        string dir = "";
        if (x < TileEngine.Width - 1)
        {
            if (TileEngine.Tiles[x + 1, y].TileType.Type == "wall")
            {
                dir += "w";
            }
        }
        if (x > 0)
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
            Debug.Log(dir);
            ChangeTileSprite(TileEngine.GetTileType("wall-" + dir));
            if (dir.Contains("n") && y < TileEngine.Heigth - 1)
            {
                Debug.Log(1);
                if (TileEngine.Tiles[x, y + 1].TileType.Type == "wall")
                    TileEngine.Tiles[x, y + 1].ChangeNextTo();
            }
            if (dir.Contains("s") && y > 0)
            {
                Debug.Log(2);
                if (TileEngine.Tiles[x, y - 1].TileType.Type == "wall")
                    TileEngine.Tiles[x, y - 1].ChangeNextTo();
            }
            if (dir.Contains("w") && x < TileEngine.Width - 1)
            {
                Debug.Log(TileEngine.Tiles[x + 1, y].TileType.Type);
                if (TileEngine.Tiles[x + 1, y].TileType.Type == "wall")
                    TileEngine.Tiles[x + 1, y].ChangeNextTo();
            }
            if (dir.Contains("e") && x > 0)
            {
                Debug.Log(4);
                Debug.Log(TileEngine.Tiles[x - 1, y].TileType.Type);
                if (TileEngine.Tiles[x - 1, y].TileType.Type == "wall")
                    TileEngine.Tiles[x - 1, y].ChangeNextTo();
            }

        }
    }
}
