using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static int max_x { get; set; } = 7;
    public static int max_y { get; set; } = 11;

    public Board boardPrefab;
    public List<Board> boards = new List<Board>();

    private bool[,] boardExist = new bool[max_x, max_y];

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
        board.transform.position = new Vector3(x,y,0);

        return board;

    }
}
