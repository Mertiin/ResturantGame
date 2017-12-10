using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class Character
{
    public GameObject GameObject;
    protected SpriteRenderer spriteRenderer;
    public TileSprite TileSprite;
    public TileEngine TileEngine;
    public Vector2 Position
    {
        get
        {
            return GameObject.transform.position;
        }
    }

    public Vector2 TargetPosition;
    public Vector2 NextPosition;
    private Queue<Vector2> Path = new Queue<Vector2>();
    private List<Tile> checkedPoints = new List<Tile>();

    public float MovmentSpeed = 0.01f;

    public Character(Vector2 startPosition, TileSprite tileSprite, TileEngine tileEngine)
    {
        TileEngine = tileEngine;
        TileSprite = tileSprite;
        GameObject = new GameObject("character");
        spriteRenderer = GameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.sprite = TileSprite.Sprite;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sortingOrder = 1;

        GameObject.transform.position = startPosition;
        TargetPosition = startPosition;
        NextPosition = startPosition;
    }

    public void FindPath(Vector2 target)
    {
        var goal = TileEngine.GetTile(target);
        var position = Position;
        AStar breadth = new AStar(TileEngine.GetTile((int)position.x, (int)position.y), goal);
        var path = breadth.GetPath(goal);
        if (path != null)
        {
            foreach (var node in path)
            {
                Path.Enqueue(node.Position);
            }
        }
    }

    public void ClearPath()
    {
        bool reCalculate = true;
        foreach (var node in Path)
        {
            if (TileEngine.GetTile((int)node.x, (int)node.y).WalkSpeed == 0)
            {
                reCalculate = true;
            }
        }

        if (reCalculate)
        {
            checkedPoints.Clear();
            Path.Clear();
        }
    }

    public void Update()
    {
        if (TargetPosition != Position)
        {
            if (Position != NextPosition)
            {
                GameObject.transform.position = Vector3.MoveTowards(Position, NextPosition, MovmentSpeed);
            }
            else
            {
                if (Path != null && Path.Count > 0)
                {
                    NextPosition = Path.Dequeue();
                }
                else
                {
                    FindPath(TargetPosition);
                }
            }
        }
    }
}