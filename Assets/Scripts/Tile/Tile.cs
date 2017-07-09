using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public GameObject GameObject;
    private SpriteRenderer _spriteRenderer;

    public Tile(string tileName, TileType TileSprite, Vector2 startPosition, GameObject parent = null, float opacity = 1f)
    {
        GameObject = new GameObject(tileName);
        _spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        _spriteRenderer.sprite = TileSprite.Sprite;
        _spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        if (parent != null)
        {
            GameObject.transform.parent = parent.transform;
        }
        SetPosition(startPosition);
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
        GameObject.transform.position = position;
    }

    public void ChangeTileSprite(TileType TileSprite)
    {
        _spriteRenderer.sprite = TileSprite.Sprite;
    }
}
