using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : WorldObject
{
    public int Width, Height;

    public Furniture(Tile tile, TileSprite tileSprite, GameObject FurnitureContainer, int width = 1, int height = 1)
    {
        this.tileSprite = tileSprite;
        Width = width;
        Height = height;

        GameObject = new GameObject(tile.TileName + "_" + tileSprite.FullName);
        this.tileSprite = tileSprite;
        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = tileSprite.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f);

        GameObject.transform.parent = FurnitureContainer.transform;
        SetPosition(tile.Position);
    }

    public bool IsInBound(int x, int y)
    {
        if (x >= Position.x && x <= Position.x + (Width - 1))
        {
            if (y >= Position.y && y <= Position.y + (Height - 1))
            {
                return true;
            }
        }
        return false;
    }
}