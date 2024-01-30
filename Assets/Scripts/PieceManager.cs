using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PieceManager : Singleton<PieceManager>
{
    public List<Piece> pieces = new List<Piece>();
    private Piece[] tempPieces = new Piece[2]; 

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
    public Piece FindPiece(Vector2Int coord)
    {
        return pieces.Find(x => x.coord == coord);
    }    
    public Piece GetAdjacentPiece(Piece piece, Direction dir)
    {
        return pieces.Find(x => x.coord == BoardManager.Inst.GetAdjacentBoard(piece.coord.x, piece.coord.y, dir));
    }
    public IEnumerable<Piece> GetAdjacentPieceAll(Piece piece)
    {
        // 인접한 null이 아닌 모든 piece 탐색
        return BoardManager.Inst.GetAllAdjacent(piece.coord.x, piece.coord.y).Select(x => FindPiece(x)).Where(x => x != null);
    }

    private void SpawnInitialPiece(int i, int x, int y)
    {
        var piece = Instantiate(GameManager.Inst.piecePrefab[i]);
        piece.coord = new Vector2Int(x, y);
        piece.transform.SetParent(transform, false);
        piece.transform.position = BoardManager.Inst.MoveBoardPos(x, y);
        pieces.Add(piece);
    }
    public void SwapPiece(Piece piece1, Piece piece2)
    {
        if (piece1.coord == piece2.coord)
            throw new Exception("Same Piece");
        Debug.Log("Swap: " + piece1.coord + " " + piece2.coord);
        tempPieces[0] = piece1;
        tempPieces[1] = piece2;
        Vector2Int temp = piece1.coord;
        piece1.coord = piece2.coord;
        piece1.transform.position = BoardManager.Inst.MoveBoardPos(piece1.coord.x, piece1.coord.y);

        piece2.coord = temp;
        piece2.transform.position = BoardManager.Inst.MoveBoardPos(piece2.coord.x, piece2.coord.y);

    }
    public void UndoSwap()
    {
        Debug.Log("Undo Swap");
        if (tempPieces[0] == null || tempPieces[1] == null) return;
        SwapPiece(tempPieces[0], tempPieces[1]);
        tempPieces[0] = null;
        tempPieces[1] = null;
    }


}
