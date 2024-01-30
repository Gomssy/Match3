using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int _x;
    public int _y;

    public Piece FindFillPiece()
    {
        var nextCoord = new Vector2Int(_x, _y);
        var maxUpCoord = new Vector2Int(_x, _y);

        while(true)
        {
            nextCoord = BoardManager.Inst.GetAdjacentBoard(nextCoord.x, nextCoord.y, Direction.Up);
            if(BoardManager.Inst.boards.Find(x => (x._x == nextCoord.x && x._y == nextCoord.y)) == null)
            {
                maxUpCoord = BoardManager.Inst.GetAdjacentBoard(nextCoord.x, nextCoord.y, Direction.Down);
                break;
            }

            var nextPiece = PieceManager.Inst.FindPiece(nextCoord);
            if (nextPiece != null)
                return nextPiece;

        }
        int center_x = BoardManager.max_x / 2;
        if (_x == center_x)
            return null;
        if(_x < center_x)
        {
            nextCoord = maxUpCoord;
            for(int i = 0; i < center_x - _x; i++)
            {
                nextCoord = BoardManager.Inst.GetAdjacentBoard(nextCoord.x, nextCoord.y, Direction.RightUp);
                var nextPiece = PieceManager.Inst.FindPiece(nextCoord);
                if(nextPiece != null) return nextPiece;
            }
            return null;
        }
        else
        {
            nextCoord = maxUpCoord;
            for(int i = 0; i < _x - center_x; i++)
            {
                nextCoord = BoardManager.Inst.GetAdjacentBoard(nextCoord.x, nextCoord.y, Direction.LeftUp);
                var nextPiece = PieceManager.Inst.FindPiece(nextCoord);
                if (nextPiece != null) return nextPiece;
            }
            return null;
        }
    }
   
}