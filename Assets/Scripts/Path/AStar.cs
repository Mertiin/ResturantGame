using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class AStar
{
    SimplePriorityQueue<Tile> frontier = new SimplePriorityQueue<Tile>();
    Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
    Dictionary<Tile, float> costSoFar = new Dictionary<Tile, float>();
    Tile start;

    public AStar(Tile start, Tile goal = null)
    {
        this.start = start;
        frontier.Enqueue(start, 0);
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (goal != null && goal == current)
                break;

            foreach (var next in current.GetNeighbors())
            {
                if (next.WalkSpeed > 0)
                {
                    float newCost = costSoFar[current] + current.WalkSpeed + next.WalkSpeed;
                    if (!costSoFar.ContainsKey(next) || costSoFar[next] > newCost)
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost + PathHelper.Heuristic(goal.Position, next.Position);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
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