using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int coord;
    public PieceType pieceType;
    
    
}
public enum PieceType
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Spin,
    Empty,
}
