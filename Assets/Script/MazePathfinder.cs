using System.Collections.Generic;
using UnityEngine;

public class MazePathfinder : MonoBehaviour
{
    private List<List<Cell>> maze; // CellGrid từ MazeGenerator

    public void SetGrid(List<List<Cell>> mazeGrid)
    {
        maze = mazeGrid;
        Debug.Log("Total rows: " + maze.Count);

        for (int y = 0; y < maze.Count; y++)
        {
            Debug.Log($"Row {y} has {maze[y].Count} cells");
            for (int x = 0; x < maze[y].Count; x++)
            {
                Cell cell = maze[y][x];
                string borders = string.Join(",", cell.Borders); // danh sách các hướng
                Debug.Log($"Cell[{x},{y}] Pos: {cell.position} Borders: {borders}");
            }
        }
    }

    public List<Cell> FindPath(Vector2Int start, Vector2Int end)
    {
        int width = maze[0].Count;
        int height = maze.Count;

        if (start == end)
            return new List<Cell> { maze[start.y][start.x] };

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

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            Cell currCell = maze[current.y][current.x];

            foreach (Vector2Int dir in directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                if (visited[ny, nx]) continue;

                // Kiểm tra tường
                if ((dir.y == 1 && currCell.Borders.Contains(Direction.Up)) ||
                    (dir.y == -1 && currCell.Borders.Contains(Direction.Down)) ||
                    (dir.x == 1 && currCell.Borders.Contains(Direction.Right)) ||
                    (dir.x == -1 && currCell.Borders.Contains(Direction.Left)))
                    continue;

                queue.Enqueue(new Vector2Int(nx, ny));
                visited[ny, nx] = true;
                parent[new Vector2Int(nx, ny)] = current;

                if (nx == end.x && ny == end.y)
                    break; // tìm thấy end
            }
        }

        // Dựng path
        List<Cell> path = new List<Cell>();
        Vector2Int cur = end;

        if (!parent.ContainsKey(cur))
        {
            Debug.LogWarning("Không tìm thấy đường từ start → end!");
            return null;
        }

        while (cur != start)
        {
            path.Add(maze[cur.y][cur.x]);
            cur = parent[cur];
        }
        path.Add(maze[start.y][start.x]);
        path.Reverse();

        return path;
    }
}
