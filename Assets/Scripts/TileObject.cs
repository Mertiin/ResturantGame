using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject
{
    public string TileName;
    public GameObject GameObject;
    protected SpriteRenderer spriteRenderer;
    public Vector2 Position;
    public TileSprite TileSprite;

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

    public void ChangeTileSprite(TileSprite tileSprite)
    {
        TileSprite = tileSprite;
        spriteRenderer.sprite = tileSprite.Sprite;
    }
    
    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }
}