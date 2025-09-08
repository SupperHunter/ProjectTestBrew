using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : Singleton<MazeGenerator>
{

    [SerializeField] private GameObject BorderParent;
    [SerializeField] private GameObject MazeCellPrefab;
    [SerializeField] private Transform CellParent;
    [SerializeField] private Material m_simpleLit;
    public List<List<Vector3>> Grid;
    public List<List<Cell>> CellGrid;
    public int MazeWidth = 11;
    public int MazeHeight = 11;
    private ObjectPoints PointsOnThePlane;
    private readonly int planeHeight = 11;
    private readonly int planeWidth = 11;
    private RecursiveBacktracker RB;

    void Start()
    {
        PointsOnThePlane = this.GetComponent<ObjectPoints>();
        RB = new RecursiveBacktracker();
    }

    public void CreateNewMaze()
    {
        Destroy(BorderParent);
        BorderParent = new GameObject("BorderParent");
        MazeWidth = 10;
        MazeHeight = 10;
        CreateTheGrid();
        CellGrid = RB.GetNewMaze(Grid);
        ShowMaze();
    }

    void CreateTheGrid() 
    {
        Grid = new List<List<Vector3>>();
        List<Vector3> OneRow = new List<Vector3>();

        for (int y = 0; y < MazeHeight; y++)
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                OneRow.Add(PointsOnThePlane.GetObjectGlobalVertices()[y + x * planeHeight]); //getting the points on the plane from the upper left corner
            }
            Grid.Add(OneRow);
            OneRow = new List<Vector3>();
        }
    }

    [PunRPC]
    void RPC_CreateCell(int x, int y, Vector3 pos, byte[] borderBytes)
    {
        Direction[] borders = new Direction[borderBytes.Length];
        for (int i = 0; i < borderBytes.Length; i++)
            borders[i] = (Direction)borderBytes[i];
        GameObject cellObj = Instantiate(MazeCellPrefab, pos, Quaternion.identity, CellParent);
        cellObj.name = $"Cell_{x}_{y}";

        MazeCell mazeCell = cellObj.GetComponent<MazeCell>();
        mazeCell.Init(
            new Vector2Int(x, y),
            new Cell(pos: pos, borders: new List<Direction>(borders))
        );
        foreach (Direction d in borders)
        {
            if (d == Direction.Up || d == Direction.Down)
                PlaceHorizontalBorderSimple(pos, x, y, d);
            else
                PlaceVerticalBorderSimple(pos, x, y, d);
        }
    }
    public void SyncMazeWithOthers()
    {
        PhotonView pv = this.GetComponent<PhotonView>();

        for (int y = 0; y < MazeHeight; y++)
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                Direction[] borders = CellGrid[y][x].Borders.ToArray();
                byte[] borderBytes = new byte[borders.Length];

                for (int i = 0; i < borders.Length; i++)
                    borderBytes[i] = (byte)borders[i];
                pv.RPC("RPC_CreateCell", RpcTarget.OthersBuffered, x, y, CellGrid[y][x].position, borderBytes);
            }
        }
    }
    void PlaceHorizontalBorderSimple(Vector3 pos, int x, int y, Direction d)
    {
        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
        border.transform.localScale = new Vector3(0.3f, 1, 1.3f);
        border.GetComponent<MeshRenderer>().material = m_simpleLit;
        Vector3 offset = Vector3.right * 0.5f * ((d == Direction.Up) ? 1 : -1);
        Instantiate(border, pos + offset, Quaternion.identity, BorderParent.transform)
            .name = $"Border_H_{x}_{y}_{d}";
    }

    void PlaceVerticalBorderSimple(Vector3 pos, int x, int y, Direction d)
    {
        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
        border.transform.localScale = new Vector3(1.3f, 1, 0.3f);
        border.GetComponent<MeshRenderer>().material = m_simpleLit;
        Vector3 offset = Vector3.forward * 0.5f * ((d == Direction.Left) ? 1 : -1);
        Instantiate(border, pos + offset, Quaternion.identity, BorderParent.transform)
            .name = $"Border_V_{x}_{y}_{d}";
    }

    void ShowMaze()
    {
        GameObject VerticalBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);  //vertically scaled cube for up and down borders
        VerticalBorder.transform.localScale = new Vector3(1.3f, 1, 0.3f);

        GameObject HorizontalBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);//horizontally scaled cube for left and right borders
        HorizontalBorder.transform.localScale = new Vector3(0.3f, 1, 1.3f);

        VerticalBorder.GetComponent<MeshRenderer>().material = m_simpleLit;
        HorizontalBorder.GetComponent<MeshRenderer>().material = m_simpleLit;
        for (int x = 0; x < MazeWidth; x++)
        {
            for (int y = 0; y < MazeHeight; y++)
            {
                GameObject cellObj = Instantiate(MazeCellPrefab, CellGrid[y][x].position, Quaternion.identity, CellParent);
                cellObj.SetActive(true);
                cellObj.name = $"Cell_{x}_{y}";
                MazeCell mazeCell = cellObj.GetComponent<MazeCell>();
                mazeCell.Init(new Vector2Int(x, y), CellGrid[y][x]);
                if ((x == 0 && y == 0) || (x == MazeWidth - 1 && y == MazeWidth - 1)) continue;
                if (CellGrid[y][x].Borders.Contains(Direction.Up))
                {
                    PlaceHorizontalBorder(HorizontalBorder, CellGrid[y][x], Direction.Up , x , y);
                }
                if (CellGrid[y][x].Borders.Contains(Direction.Down))
                {
                    PlaceHorizontalBorder(HorizontalBorder, CellGrid[y][x], Direction.Down, x, y);
                }

                if (CellGrid[y][x].Borders.Contains(Direction.Right))
                {
                    PlaceVerticalBorder(VerticalBorder, CellGrid[y][x], Direction.Right, x, y);
                }
                if (CellGrid[y][x].Borders.Contains(Direction.Left))
                {
                    PlaceVerticalBorder(VerticalBorder, CellGrid[y][x], Direction.Left, x, y);
                }
            }
        }
        GameObject.Destroy(VerticalBorder); //destroy source objects
        GameObject.Destroy(HorizontalBorder);
    }

    void PlaceHorizontalBorder(GameObject border, Cell c, Direction d, int x, int y)
    {
        Vector3 offset = Vector3.right * 0.5f * ((d == Direction.Up) ? 1 : -1);
        GameObject obj = Instantiate(border, c.position + offset, Quaternion.identity, BorderParent.transform);
        obj.name = $"Border_H_{x}_{y}_{d}"; // gán tên theo x, y và hướng
    }

    void PlaceVerticalBorder(GameObject border, Cell c, Direction d, int x, int y)
    {
        Vector3 offset = Vector3.forward * 0.5f * ((d == Direction.Left) ? 1 : -1);
        GameObject obj = Instantiate(border, c.position + offset, Quaternion.identity, BorderParent.transform);
        obj.name = $"Border_V_{x}_{y}_{d}"; // gán tên theo x, y và hướngc
    }
}
