using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int coord;
    public PieceType pieceType;
    public static List<PieceType> obstacleType = new List<PieceType>() { PieceType.Top};
    
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
    Empty,
}
