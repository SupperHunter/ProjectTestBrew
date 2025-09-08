using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding
{
    private List<List<Cell>> maze;

    public Pathfinding(List<List<Cell>> mazeGrid)
    {
        maze = mazeGrid;
    }

    public List<Vector3> FindPath(Vector2Int start, Vector2Int target)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        queue.Enqueue(start);
        cameFrom[start] = start;

        int h = maze.Count;
        int w = maze[0].Count;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == target) break;

            foreach (var next in GetNeighbors(current))
            {
                if (!cameFrom.ContainsKey(next))
                {
                    queue.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }
        List<Vector3> path = new List<Vector3>();
        if (!cameFrom.ContainsKey(target)) return path;

        var cur = target;
        while (cur != start)
        {
            path.Add(maze[cur.y][cur.x].position);
            cur = cameFrom[cur];
        }
        path.Add(maze[start.y][start.x].position);
        path.Reverse();

        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int h = maze.Count;
        int w = maze[0].Count;
        Cell cell = maze[pos.y][pos.x];

        // Up
        if (pos.y > 0 && !cell.Borders.Contains(Direction.Up) && !maze[pos.y - 1][pos.x].Borders.Contains(Direction.Down))
            result.Add(new Vector2Int(pos.x, pos.y - 1));

        // Down
        if (pos.y < h - 1 && !cell.Borders.Contains(Direction.Down) && !maze[pos.y + 1][pos.x].Borders.Contains(Direction.Up))
            result.Add(new Vector2Int(pos.x, pos.y + 1));

        // Left
        if (pos.x > 0 && !cell.Borders.Contains(Direction.Left) && !maze[pos.y][pos.x - 1].Borders.Contains(Direction.Right))
            result.Add(new Vector2Int(pos.x - 1, pos.y));

        // Right
        if (pos.x < w - 1 && !cell.Borders.Contains(Direction.Right) && !maze[pos.y][pos.x + 1].Borders.Contains(Direction.Left))
            result.Add(new Vector2Int(pos.x + 1, pos.y));

        return result;
    }
}
