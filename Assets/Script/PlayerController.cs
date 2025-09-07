using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMover mover;    // component PlayerMover
    [SerializeField] private MazeGenerator mazeGen; // tham chiếu MazeGenerator
    [SerializeField] private MazePathfinder pathfinder;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var (endIndex, targetCell) = FindNearestCellWithIndex(hit.point);
                if (targetCell != null)
                {
                    Vector2Int startIndex = CellPositionFromWorld(transform.position);
                    pathfinder.SetGrid(mazeGen.CellGrid);
                    List<Cell> path = pathfinder.FindPath(startIndex, endIndex);

                    if (path != null)
                        mover.SetPath(path);
                    else
                        Debug.Log("Không tìm thấy đường đi!");
                }
            }
        }
    }

    (Vector2Int, Cell) FindNearestCellWithIndex(Vector3 worldPos)
    {
        float minDist = float.MaxValue;
        Cell nearest = null;
        Vector2Int index = Vector2Int.zero;

        for (int y = 0; y < mazeGen.CellGrid.Count; y++)
        {
            for (int x = 0; x < mazeGen.CellGrid[y].Count; x++)
            {
                float dist = Vector3.Distance(worldPos, mazeGen.CellGrid[y][x].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = mazeGen.CellGrid[y][x];
                    index = new Vector2Int(x, y);
                }
            }
        }

        return (index, nearest);
    }

    // Tìm cell gần nhất với vị trí world
    Cell FindNearestCell(Vector3 worldPos)
    {
        float minDist = float.MaxValue;
        Cell nearest = null;

        foreach (var row in mazeGen.CellGrid)
        {
            foreach (var cell in row)
            {
                float dist = Vector3.Distance(worldPos, cell.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = cell;
                }
            }
        }

        return nearest;
    }

    // Chuyển world position -> index grid
    Vector2Int CellPositionFromWorld(Vector3 pos)
    {
        float minDist = float.MaxValue;
        Vector2Int closest = Vector2Int.zero;

        for (int y = 0; y < mazeGen.CellGrid.Count; y++)
        {
            for (int x = 0; x < mazeGen.CellGrid[y].Count; x++)
            {
                float dist = Vector3.Distance(pos, mazeGen.CellGrid[y][x].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = new Vector2Int(x, y);
                }
            }
        }

        return closest;
    }
}
