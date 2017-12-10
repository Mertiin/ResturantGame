using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject
{
    public string TileName;
    public GameObject GameObject;
    protected SpriteRenderer spriteRenderer;
    private Vector2 position;
    public Vector2 Position
    {
        get
        {
            if (position == null)
            {
                position = GameObject.transform.position;
            }
            return position;
        }
    }

    protected TileSprite tileSprite;
    public ObjectType Type
    {
        get { return tileSprite.Type; }
    }
    public float WalkSpeed
    {
        get { return tileSprite.Walkspeed; }
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
        this.position = position;
        GameObject.transform.position = position;
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ChangeTileSprite(TileSprite tileSprite)
    {
        this.tileSprite = tileSprite;
        spriteRenderer.sprite = tileSprite.Sprite;
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }
}