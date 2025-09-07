//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MazeGenerator : MonoBehaviour
//{
//    [SerializeField] private int rows = 11;
//    [SerializeField] private int cols = 11;
//    [SerializeField] private GameObject CellPrefab;


//    public MazeCell[,] mMazeCells;
//    public Maze maze
//    {
//        get;
//        private set;
//    }
//    Stack<Cell> _stack = new Stack<Cell>();
//    public bool MazeGenerationCompleted
//    {
//        get;
//        private set;
//    } = false;
//    void Start()
//    {
//        int START_X = -cols / 2;
//        int START_Y = -rows / 2;
//        maze = new Maze(rows, cols);
//        mMazeCells = new MazeCell[cols, rows];
//        for (int i = 0; i < cols; ++i)
//        {
//            for (int j = 0; j < rows; ++j)
//            {
//                GameObject obj = Instantiate(CellPrefab);
//                obj.transform.parent = transform;
//                Cell cell = maze.GetCell(i, j);
//                cell.onSetDirFlag = OnCellSetDirFlag;
//                obj.transform.position = new Vector3(
//                  START_X + cell.x,
//                  START_Y + cell.y,
//                  1.0f);

//                mMazeCells[i, j] = obj.GetComponent<MazeCell>();
//                mMazeCells[i, j].Cell = cell;
//            }
//        }
//        CreateNewMaze();
//    }

//    public void CreateNewMaze()
//    {
//        maze.RemoveCellWall(0, 0, Directions.LEFT);
//        maze.RemoveCellWall(cols - 1, rows - 1, Directions.RIGHT);
//        _stack.Push(maze.GetCell(0, 0));
//        StartCoroutine(Coroutine_Generate());
//    }


//    public void HighlightCell(int i, int j, bool flag)
//    {
//        mMazeCells[i, j].SetHighlight(flag);
//    }
//    public void RemoveAllHightlights()
//    {

//        for (int i = 0; i < cols; ++i)
//        {
//            for (int j = 0; j < rows; ++j)
//            {
//                mMazeCells[i, j].SetHighlight(false);
//            }
//        }
//    }

//    public void OnCellSetDirFlag(int x, int y, Directions dir, bool f)
//    {
//        mMazeCells[x, y].SetActive(dir, f);
//    }

//    bool GenerateStep()
//    {
//        if (_stack.Count == 0) return true;

//        Cell c = _stack.Peek();
//        var neighbours = maze.GetNeighboursNotVisited(c.x, c.y);

//        if (neighbours.Count != 0)
//        {
//            var index = 0;
//            if (neighbours.Count > 1)
//            {
//                index = Random.Range(0, neighbours.Count);
//            }
//            var item = neighbours[index];
//            Cell neighbour = item.Item2;
//            neighbour.visited = true;
//            maze.RemoveCellWall(c.x, c.y, item.Item1);

//            mMazeCells[c.x, c.y].SetHighlight(true);

//            _stack.Push(neighbour);
//        }
//        else
//        {
//            _stack.Pop();
//            mMazeCells[c.x, c.y].SetHighlight(false);
//        }
//        return false;
//    }

//    IEnumerator Coroutine_Generate()
//    {
//        bool flag = false;
//        while (!flag)
//        {
//            flag = GenerateStep();
//            //yield return null;
//            yield return new WaitForSeconds(0.01f);
//        }
//        MazeGenerationCompleted = true;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
