using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match : MonoBehaviour
{
    public List<Piece> pieces => PieceManager.Inst.pieces;
    public List<Direction[]> clusterDir = new();

    /// <summary>
    /// cluster match üũ�� ���� 3���� �ǽ��� ���� �پ� �ִ��� üũ
    /// </summary>
    private void Awake()
    {
        foreach(var dir in BoardManager.Inst.directions)
        {
            Direction[] clusterDirArr = new Direction[3];
            for(int i = 0; i < 3; i++)
            {
                clusterDirArr[i] = (Direction)(((int)dir + i) % BoardManager.Inst.directions.Length + 1);
            }
            clusterDir.Add(clusterDirArr);
        }
    }
    /// <summary>
    /// Line Match
    /// Ž�� �ǽ��κ��� �� �������� ������ Ž�� �� �ű⸦ �������� ���������� �� Ž��
    /// </summary>
    private List<MatchedPiece> FindLineMatchAll(Piece piece)
    {
        var matchedPieces = new List<MatchedPiece>();
        PieceType pieceType = piece.pieceType;

        foreach(var dir in Enum.GetValues(typeof(MatchDir)))
        {
            MatchDir matchDir = (MatchDir)dir;
            if (matchDir == MatchDir.None) continue;
            var matchedPiece = FindLineMatch(piece, matchDir);
            if(matchedPiece != null)
            {
                matchedPieces.Add(matchedPiece);
            }
        }
        return matchedPieces;
    }

    private MatchedPiece FindLineMatch(Piece piece, MatchDir matchDir)
    {
        var pieceType = piece.pieceType;

        Piece startPiece = piece;
        Piece nextPiece = piece;

        Direction dir;
        switch (matchDir)
        {
            case MatchDir.Vertical:
                dir = Direction.Down;
                break;
            case MatchDir.DiagonalUp:
                dir = Direction.LeftDown;
                break;
            case MatchDir.DiagonalDown:
                dir = Direction.LeftUp;
                break;
            default:
                dir = Direction.No;
                break;
        }

        while(true)
        {
            nextPiece = PieceManager.Inst.GetAdjacentPiece(nextPiece, dir);
            if (nextPiece == null || nextPiece.pieceType != piece.pieceType)
                break;
            startPiece = nextPiece;
        }

        switch(matchDir)
        {
            case MatchDir.Vertical:
                dir = Direction.Up;
                break;
            case MatchDir.DiagonalUp:
                dir = Direction.RightUp;
                break;
            case MatchDir.DiagonalDown:
                dir = Direction.RightDown;
                break;
            default:
                dir = Direction.No;
                break;
        }

        var result = CheckLine(startPiece, dir);
        if (result.Count == 0) return null;

        var matchedPiece = new MatchedPiece(pieceType, MatchType.Line, matchDir, result);
        return matchedPiece;
    }
    /// <summary>
    /// �������� 3�� �̻� ����Ǿ� �ִ��� üũ
    /// </summary>
    private List<Vector2Int> CheckLine(Piece piece, Direction dir)
    {
        var matched = new List<Vector2Int>();

        int matchCount = 1;
        var nextPiece = piece;
        matched.Add(piece.coord);

        while(true)
        {
            nextPiece = PieceManager.Inst.GetAdjacentPiece(nextPiece, dir);
            if (nextPiece == null || nextPiece.pieceType != piece.pieceType)
                break;

            matchCount++;
            matched.Add(nextPiece.coord);
        }
        bool isMatched = matchCount >= 3;
        if (!isMatched)
            matched.Clear();

        return matched;
    }

    /// <summary>
    /// Ž�� ���� �ǽ� �ֺ����� ����� �ǽ��� 3�� �̻����� üũ
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    private List<MatchedPiece> FindClusterMatchAll(Piece piece)
    {
        var matchedPieces = new List<MatchedPiece>();
        PieceType pieceType = piece.pieceType;

        var adjacentPieces = PieceManager.Inst.GetAdjacentPieceAll(piece);
        foreach(var _piece in adjacentPieces)
        {
            foreach (var dirs in clusterDir)
            {
                var matched = FindClusterMatch(_piece, pieceType, dirs);
                if(matched.Count != 0)
                {
                    var matchedPiece = new MatchedPiece(pieceType, MatchType.Cluster, MatchDir.None, matched);
                    matchedPieces.Add(matchedPiece);
                }
            }
        }
        return matchedPieces;
    }
    private List<Vector2Int> FindClusterMatch(Piece piece, PieceType pieceType, Direction[] dirs)
    {
        var matched = new List<Vector2Int>();
        var nextPiece = piece;
        if(piece.pieceType != pieceType) return matched;

        matched.Add(piece.coord);
        foreach(var dir in dirs)
        {
            nextPiece = PieceManager.Inst.GetAdjacentPiece(piece, dir);
            if(nextPiece == null || nextPiece.pieceType != pieceType)
            {
                matched.Clear();
                return matched;
            }
            matched.Add(piece.coord);
        }
        return matched;
    }
    public List<MatchedPiece> CheckPieceAll()
    {
        var res = new List<MatchedPiece>();
        foreach (var piece in pieces)
        {
            var checkResult = CheckPiece(piece);
            if (checkResult != null)
            {
                res = res.Concat(checkResult).Distinct().ToList();
            }
        }
        return res;
    }

    public List<MatchedPiece> CheckPiece(Piece piece)
    {
        var pieceType = piece.pieceType;

        var lineMatch = FindLineMatchAll(piece);
        var clusterMatch = FindClusterMatchAll(piece);

        var matchedPieces = lineMatch.Concat(clusterMatch).Distinct().ToList();
        return matchedPieces;
    }
}
public enum MatchType
{
    Empty,
    Line,
    Cluster,
}

public enum MatchDir
{
    None,
    Vertical,
    DiagonalUp,
    DiagonalDown,
}
