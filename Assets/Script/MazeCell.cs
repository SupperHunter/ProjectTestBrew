using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Vector2Int CellPos;   // tọa độ trong grid
    public Cell Data;            // tham chiếu logic cell

    public void Init(Vector2Int pos, Cell cellData)
    {
        CellPos = pos;
        Data = cellData;
    }
}
