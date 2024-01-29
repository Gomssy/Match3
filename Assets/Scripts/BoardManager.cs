using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public static int max_x { get; set; } = 7;
    public static int max_y { get; set; } = 11;

    public Board boardPrefab;
    public List<Board> boards = new List<Board>();

    private bool[,] boardExist = new bool[max_x, max_y];
    public Direction[] directions = { Direction.Up, Direction.RightUp, Direction.RightDown, Direction.Down, Direction.LeftDown, Direction.LeftUp };
    private void Awake()
    {
        InitBoard();
    }

    /// <summary>
    /// 0,6: 3,5,7
    /// 1,5: 2,4,6,8
    /// 2,4: 1,3,5,7,9
    /// 3:   0,2,4,6,8,10
    /// </summary>
    private void InitBoard()
    {
        for (int x = 0; x < max_x; x++)
        {
            for (int y = 0; y < max_y; y++)
            {
                if ((x + y) % 2 == 0)
                    continue;
                if ((x == 0 && (y == 1 || y == max_y - 2)) || (x == 1 && (y == 0 || y == max_y - 1)))
                    continue;
                if ((x == max_x - 1 && (y == 1 || y == max_y - 2)) || (x == max_x - 2 && (y == 0 || y == max_y - 1)))
                    continue;

                boardExist[x, y] = true;

                if (!boardExist[x, y]) continue;
                var board = CreateBoard(x,y);
                boards.Add(board);

            }
        }
    }

    private Board CreateBoard(int x, int y)
    {
        var board = Instantiate(boardPrefab);
        board._x = x; board._y = y;
        board.transform.SetParent(transform, false);
        board.transform.position = MoveBoardPos(x, y);

        return board;

    }

    public Vector3 MoveBoardPos(int x, int y)
    {
        return new Vector3((x - max_x / 2) * 1.75f, (y - max_y / 2) * 0.96f, 0f);
    }

    public Vector2Int GetAdjacentBoard(int x, int y, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector2Int(x, y + 2);
            case Direction.RightUp:
                return new Vector2Int(x + 1, y + 1);
            case Direction.RightDown:
                return new Vector2Int(x + 1, y - 1);
            case Direction.Down:
                return new Vector2Int(x, y - 2);
            case Direction.LeftDown:
                return new Vector2Int(x - 1, y - 1);
            case Direction.LeftUp:
                return new Vector2Int(x - 1, y + 1);
        }
        return Vector2Int.zero;
    }

    public IEnumerable<Vector2Int> GetAllAdjacent(int x, int y)
    {
        return directions.Select(dir => GetAdjacentBoard(x, y, dir));
    }

    public bool isAdjacent(Vector2Int coord1, Vector2Int coord2)
    {
        int diff_x = Mathf.Abs(coord1.x - coord2.x);
        int diff_y = Mathf.Abs(coord1.y - coord2.y);
        return diff_x <= 1 && diff_y <= 2;
    }
}
public enum Direction
{
    No,
    Up,
    RightUp,
    RightDown,
    Down,
    LeftDown,
    LeftUp,
}