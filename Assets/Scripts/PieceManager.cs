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
        piece.transform.position = BoardManager.Inst.GetWorldPos(x, y);
        pieces.Add(piece);
    }


    public IEnumerator SpawnNewPiece()
    {
        while(true)
        {
            var emptyBoards = BoardManager.Inst.boards.FindAll(x => FindPiece(new Vector2Int(x._x,x._y)) is null);
            if (emptyBoards.Count == 0) break;
            bool filling = false;

            foreach(var emptyBoard in emptyBoards)
            {
                var fillPiece = emptyBoard.FindFillPiece();
                if(fillPiece != null)
                {
                    filling = true;
                    fillPiece.Drop(new Vector2Int(emptyBoard._x, emptyBoard._y));
                }
            }
            if (!filling)
                break;
        }

        while(true)
        {
            var spawnPiece = SpawnRandomPiece();
            if (pieces.Count == 30) break;
            var emptyCoord = FindEmpty();

            spawnPiece.Drop(emptyCoord);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.7f);
    }

    private Piece SpawnRandomPiece()
    {
        var randomPiece = GameManager.Inst.piecePrefab[UnityEngine.Random.Range(0, 5)];

        Vector2Int spawnCoord = new Vector2Int(BoardManager.max_x / 2, BoardManager.max_y -1);
        if (pieces.Exists(x => x.coord == spawnCoord)) throw new Exception("piece exists in same coord");

        var piece = Instantiate(randomPiece);
        piece.coord = spawnCoord;
        piece.transform.SetParent(transform, false);
        piece.transform.position = BoardManager.Inst.GetWorldPos(spawnCoord.x, spawnCoord.y) + new Vector3(0f, 2f, 0f);
        piece.Move(BoardManager.Inst.GetWorldPos(spawnCoord.x, spawnCoord.y));
        pieces.Add(piece);

        return piece;
    }

    /// <summary>
    /// y가 작은 순, x가 중앙에 가까운 순으로 정렬
    /// </summary>
    /// <returns></returns>
    private Vector2Int FindEmpty()
    {
        var center_x = BoardManager.max_x / 2;
        var coords = BoardManager.Inst.boards
            .Where(x => FindPiece(new Vector2Int(x._x, x._y)) == null)
            .Select(x => new Vector2Int(x._x, x._y))
            .OrderBy(x => x.y)
            .ThenBy(x => Mathf.Abs(x.x - center_x))
            .ToList();

        return coords[0];
    }

    public IEnumerator SwapPiece(Piece piece1, Piece piece2)
    {
        if (piece1.coord == piece2.coord)
            throw new Exception("Same Piece");
        tempPieces[0] = piece1;
        tempPieces[1] = piece2;
        Vector2Int temp = piece1.coord;
        piece1.Move(BoardManager.Inst.GetWorldPos(piece2.coord.x, piece2.coord.y));
        piece2.Move(BoardManager.Inst.GetWorldPos(piece1.coord.x, piece1.coord.y));
        yield return new WaitUntil(() => !piece1.IsMoving() && !piece2.IsMoving());
        piece1.coord = piece2.coord;
        piece2.coord = temp;

    }
    public IEnumerator UndoSwap()
    {
        Debug.Log("Undo Swap");
        if (tempPieces[0] == null || tempPieces[1] == null) yield break;
        yield return StartCoroutine(SwapPiece(tempPieces[0], tempPieces[1]));
        tempPieces[0] = null;
        tempPieces[1] = null;
    }

    public void DestroyPiece(List<Vector2Int> targets)
    {
        var obstacles = FindObstacles(targets);
        foreach (var obstacle in obstacles)
        {
            var obstaclePiece = FindPiece(obstacle);
            if (obstaclePiece != null)
            {
                obstaclePiece.DestroyThis();
            }
        }

        foreach (var target in targets)
        {
            var targetPiece = FindPiece(target);
            if (targetPiece == null) continue;
            targetPiece.DestroyThis();
        }
        CanvasManager.Inst.SetScoreText();
    }

    private List<Vector2Int> FindObstacles(List<Vector2Int> targets)
    {
        var obstacles = new List<Vector2Int>();
        foreach (var target in targets)
        {
            foreach (var coord in BoardManager.Inst.GetAllAdjacent(target.x, target.y))
            {
                var piece = FindPiece(coord);
                if (piece != null && piece.pieceType == PieceType.Top && !obstacles.Contains(piece.coord))
                {
                    obstacles.Add(piece.coord);
                }
            }
        }
        return obstacles;
    }


}
