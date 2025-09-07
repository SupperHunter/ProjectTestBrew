//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public enum Directions
//{
//    UP,
//    RIGHT,
//    DOWN,
//    LEFT,
//    NONE,
//}

//public class Cell : Node<Vector2Int>
//{
//    public bool visited = false;
//    public bool[] flag =
//    {
//      true,
//      true,
//      true,
//      true
//    };
//    public int x { get { return Value.x; } }
//    public int y { get { return Value.y; } }

//    public delegate void DelegateSetDirFlag(int x, int y, Directions dir, bool f);

//    public DelegateSetDirFlag onSetDirFlag;

//    private Maze mMaze;

//    public Cell(int c, int r, Maze maze): base(new Vector2Int(c, r))
//    {
//        mMaze = maze;
//    }

//    public void SetDirFlag(Directions dir,bool f)
//    {
//        flag[(int)dir] = f;
//        onSetDirFlag?.Invoke(Value.x, Value.y, dir, f);
//    }

//    public override List<Node<Vector2Int>> GetNeighbours()
//    {
//        List<Node<Vector2Int>> neighbours = new List<Node<Vector2Int>>();
//        foreach (Directions dir in Enum.GetValues(typeof(Directions)))
//        {
//            int x = Value.x;
//            int y = Value.y;

//            switch (dir)
//            {
//                case Directions.UP:
//                    if (y < mMaze.NumRows - 1)
//                    {
//                        ++y;
//                        if (!flag[(int)dir])
//                        {
//                            neighbours.Add(mMaze.GetCell(x, y));
//                        }
//                    }
//                    break;
//                case Directions.RIGHT:
//                    if (x < mMaze.NumCols - 1)
//                    {
//                        ++x;
//                        if (!flag[(int)dir])
//                        {
//                            neighbours.Add(mMaze.GetCell(x, y));
//                        }
//                    }
//                    break;
//                case Directions.DOWN:
//                    if (y > 0)
//                    {
//                        --y;
//                        if (!flag[(int)dir])
//                        {
//                            neighbours.Add(mMaze.GetCell(x, y));
//                        }
//                    }
//                    break;
//                case Directions.LEFT:
//                    if (x > 0)
//                    {
//                        --x;
//                        if (!flag[(int)dir])
//                        {
//                            neighbours.Add(mMaze.GetCell(x, y));
//                        }
//                    }
//                    break;
//                default:
//                    break;
//            }
//        }
//        return neighbours;
//    }
//}
