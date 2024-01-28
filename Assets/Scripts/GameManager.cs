using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static int max_x { get; set; } = 7;
    public static int max_y { get; set; } = 11;

    public Board boardPrefab;
    public List<Board> boards = new List<Board>();
    public GameObject[] piecePrefab;
    private int[] initialPiece = { 4, 3, 2, 6, 5, 5, 0, 6, 2, 2, 6, 3, 6, 5, 4, 4, 2, 1, 6, 4, 6, 3, 1, 6, 5, 5, 6, 4, 3, 2 };


    [SerializeField]
    private GameObject boardGO;

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
        int idx = 0;
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

                if(idx < initialPiece.Length)
                {
                    SpawnInitialPiece(initialPiece[idx], x, y);
                    idx++;
                }

            }
        }
    }

    private Board CreateBoard(int x, int y)
    {
        var board = Instantiate(boardPrefab);
        board._x = x; board._y = y;
        board.transform.SetParent(boardGO.transform, false);
        board.transform.position = MoveBoardPos(x, y);

        return board;

    }

    private void SpawnInitialPiece(int i, int x, int y)
    {
        GameObject piece = Instantiate(piecePrefab[i]);
        piece.transform.SetParent(boardGO.transform, false);
        piece.transform.position = MoveBoardPos(x, y);
    }

    public Vector3 MoveBoardPos(int x, int y)
    {
        return new Vector3((x - max_x / 2) * 1.75f, (y - max_y / 2) * 0.96f, 0f);
    }
}
