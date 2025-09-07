using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{

    [SerializeField] private GameObject BorderParent;
    public List<List<Vector3>> Grid;
    public List<List<Cell>> CellGrid;
    private ObjectPoints PointsOnThePlane;
    private int MazeWidth = 11;
    private int MazeHeight = 11;
    private readonly int planeHeight = 11;
    private readonly int planeWidth = 11;
    private RecursiveBacktracker RB;

    void Start()
    {
        PointsOnThePlane = this.GetComponent<ObjectPoints>();
        RB = new RecursiveBacktracker();
        CreateNewMaze();
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

    void CreateTheGrid()  //creates grid with given width and length from the points on the plane
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

    void ShowMaze()  //Creates maz with borders as cubes
    {
        GameObject VerticalBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);  //vertically scaled cube for up and down borders
        VerticalBorder.transform.localScale = new Vector3(1.3f, 1, 0.3f);

        GameObject HorizontalBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);//horizontally scaled cube for left and right borders
        HorizontalBorder.transform.localScale = new Vector3(0.3f, 1, 1.3f);
        int counttt = 0;

        for (int x = 0; x < MazeWidth; x++)
        {
            for (int y = 0; y < MazeHeight; y++)
            {
                counttt++;
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
