using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : TileObject
{
    public Tile Tile;

    public Furniture(Tile tile, TileSprite tileSprite)
    {
        Tile = tile;
        TileSprite = tileSprite;

        GameObject = new GameObject(tile.TileName + "_" + tileSprite.FullName);
        TileSprite = tileSprite;
        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = TileSprite.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f);

        GameObject.transform.parent = tile.GameObject.transform;
        GameObject.transform.position = tile.Position;
    }
}