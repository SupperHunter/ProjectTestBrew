using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding
{
    private List<List<Cell>> maze;
    public Pathfinding(List<List<Cell>> cellGrid)
    {
        maze = cellGrid;
    }

    public List<Cell> FindFath(Vector2Int start, Vector2Int end)
    {
        int width = maze[0].Count;
        int height = maze.Count;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();
        bool[,] visited = new bool[height, width];

        queue.Enqueue(start);
        visited[start.y, start.x] = true;

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0,1),   // Up
            new Vector2Int(0,-1),  // Down
            new Vector2Int(1,0),   // Right
            new Vector2Int(-1,0)   // Left
        };

        Debug.Log("Start: " + start + "  end: " + end);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == end) break;

            foreach (Vector2Int dir in directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                if (visited[ny, nx]) continue;
                Cell currCell = maze[current.y][current.x];
                if ((dir.y == 1 && currCell.Borders.Contains(Direction.Up)) ||
                    (dir.y == -1 && currCell.Borders.Contains(Direction.Down)) ||
                    (dir.x == 1 && currCell.Borders.Contains(Direction.Right)) ||
                    (dir.x == -1 && currCell.Borders.Contains(Direction.Left)))
                    continue;

                queue.Enqueue(new Vector2Int(nx, ny));
                visited[ny, nx] = true;
                parent[new Vector2Int(nx, ny)] = current;
            }
        }
        List<Cell> path = new List<Cell>();
        Vector2Int cur = end;
        if (start == end)
        {
            path.Add(maze[start.y][start.x]);
            return path;
        }
        while (cur != start)
        {
            if (!parent.ContainsKey(cur))
            {
                Debug.Log("Không tìm thấy đường! cur=" + cur);
                return null; // fallback
            }
            path.Add(maze[cur.y][cur.x]);
            cur = parent[cur];
        }
        path.Add(maze[start.y][start.x]);
        path.Reverse();

        Debug.Log("Countttt: "+ path.Count);
        return path;

    }
}
