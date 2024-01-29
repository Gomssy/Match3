using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PieceManager : Singleton<PieceManager>
{
    public List<Piece> pieces = new List<Piece>();
    private void Start()
    {
        int idx = 0;
        foreach(var i in BoardManager.Inst.boards)
        {
            if (idx < GameManager.Inst.initialPiece.Length)
            {
                SpawnInitialPiece(GameManager.Inst.initialPiece[idx], i._x, i._y);
                idx++;
            }
        }
        
    }
    private void SpawnInitialPiece(int i, int x, int y)
    {
        var piece = Instantiate(GameManager.Inst.piecePrefab[i]);
        piece._x = x; piece._y = y;
        piece.transform.SetParent(transform, false);
        piece.transform.position = BoardManager.Inst.MoveBoardPos(x, y);
        pieces.Add(piece);
    }

    public void SwapPiece(Piece piece1, Piece piece2)
    {
        if (piece1._x == piece2._x && piece1._y == piece2._y)
            throw new Exception("Same Piece");
        Debug.Log(piece1._x + "," + piece1._y + " " + piece2._x + "," + piece2._y);

        int temp_x = piece1._x;
        int temp_y = piece1._y;

        piece1._x = piece2._x;
        piece1._y = piece2._y;
        piece1.transform.position = BoardManager.Inst.MoveBoardPos(piece1._x, piece1._y);

        piece2._x = temp_x;
        piece2._y = temp_y;
        piece2.transform.position = BoardManager.Inst.MoveBoardPos(piece2._x, piece2._y);
    }


}
