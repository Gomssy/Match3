using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MatchedPiece
{
    public PieceType pieceType;
    public MatchType matchType;
    public MatchDir matchDir;
    public List<Vector2Int> coords = new();

    public MatchedPiece(PieceType _pieceType,  MatchType _matchType, MatchDir _matchDir, List<Vector2Int> _coords)
    {
        this.pieceType = _pieceType;
        this.matchType = _matchType;
        this.matchDir = _matchDir;
        this.coords = _coords;
    }

    public override bool Equals(object obj)
    {
        var other = obj as MatchedPiece;
        if (other == null) return false;

        return coords.SequenceEqual(other.coords);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + coords.Aggregate(0, (acc, coord) => acc ^ coord.GetHashCode());
        return hash;
    }
}
