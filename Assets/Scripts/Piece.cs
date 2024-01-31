using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int coord;
    public PieceType pieceType;
    public static List<PieceType> obstacleType = new List<PieceType>() { PieceType.Top};
    public static List<PieceType> itemType = new List<PieceType>() { PieceType.Line, PieceType.TNT, PieceType.Color };
    private Move move;
    public int score;

    private void Awake()
    {
        move = GetComponent<Move>();
    }

    public virtual void DestroyThis()
    {
        GameManager.Inst.totalScore += this.score;
        PieceManager.Inst.pieces.Remove(this);
        Destroy(this.gameObject);
    }

    public void Drop(Vector2Int target)
    {
        //아래로 쭉 떨어지는 경우
        if(coord.x == target.x)
        {
            var pos = BoardManager.Inst.GetWorldPos(target.x, target.y);
            Move(pos);
        }
        //대각선 아래로 떨어지는 경우
        else
        {
            Vector3 via = GetVia(coord, target);
            var dest = new List<Vector3>();

            if(move.isMoving)
            {
                dest.Add(move.dest);
            }
            dest.Add(via);
            dest.Add(BoardManager.Inst.GetWorldPos(target.x, target.y));
            move.MovePiece(dest);
        }
        coord = target;
    }

    public void Move(Vector3 pos)
    {
        move.MovePiece(pos);
    }

    public bool IsMoving()
    {
        return move.isMoving;
    }

    /// <summary>
    /// 대각선 이동 경로 설정
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 GetVia(Vector2Int coord,  Vector2Int target)
    {
        var coords = coord;
        int count = Mathf.Abs(target.x - coord.x);
        Direction dir = Direction.LeftDown;
        if (coord.x < target.x)
            dir = Direction.RightDown;

        for(int i = 0; i < count; i++)
        {
            coords = BoardManager.Inst.GetAdjacentBoard(coords.x, coords.y, dir);
        }
        return BoardManager.Inst.GetWorldPos(coords.x, coords.y);
    }
}
public enum PieceType
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Top,
    Line,
    TNT,
    Color,
    Empty,
}
