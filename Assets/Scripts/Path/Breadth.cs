using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breadth
{
    Queue<Tile> frontier = new Queue<Tile>();
    Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
    Tile start;

    public Breadth(Tile start, Tile goal = null)
    {
        this.start = start;
        frontier.Enqueue(start);
        cameFrom.Add(start, null);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (goal != null && goal == current)
                break;

            foreach (var next in current.GetNeighbors())
            {
                if (!cameFrom.ContainsKey(next) && next.WalkSpeed > 0)
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }
    }

    public Queue<Tile> GetPath(Tile target)
    {
        List<Tile> path = new List<Tile>();

        Tile current = target;

        while (current != start)
        {
            if (cameFrom.ContainsKey(current))
            {
                var last = current;
                current = cameFrom[current];
                path.Add(current);
            }
            else
            {
                break;
            }
        }

        path.Reverse();
        return new Queue<Tile>(path);
    }
}