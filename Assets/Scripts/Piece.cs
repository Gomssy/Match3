using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int _x;
    public int _y;
    

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
}
