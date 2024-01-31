using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Piece[] piecePrefab;
    public int[] initialPiece = { 4,3,2,6,5,5,6,6,2,0,3,6,6,5,6,0,3,4,6,0,3,4,6,6,5,5,6,4,3,2 };
    public int topCount = 10;
    public int moveCount = 20;
    public int totalScore = 0;

    [SerializeField]
    private Piece selected;
    [SerializeField]
    private List<MatchedPiece> matchedPieces = new();
    [SerializeField]
    private bool isDragging = true;

    private bool isGameEnd = false;
    private bool cleared = false;

    private void Update()
    {
        if(!isGameEnd)
        {
            if (!isDragging) return;
            if (selected != null) return;
            if (Input.GetMouseButtonDown(0))
            {
                var mp = GetMouseWorldPos();
                RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.GetComponent<Piece>() != null)
                {
                    selected = hit.collider.gameObject.GetComponent<Piece>();
                    StartCoroutine(Drag(selected.transform.position));
                }
            }
        }        
    }

    IEnumerator Drag(Vector3 pos)
    {
        isDragging = false;
        while(true)
        {
            if (Input.GetMouseButtonUp(0)) break;
            var mp = GetMouseWorldPos();
            float dist = Vector3.Distance(pos, mp);
            if(dist > 1.5f)
            {
                Direction dir = BoardManager.Inst.FindDirection(pos, mp);
                Piece target = PieceManager.Inst.GetAdjacentPiece(selected, dir);
                if (target == null) break;
                yield return StartCoroutine(PieceManager.Inst.SwapPiece(selected, target));
                var selectInfo = Match.Inst.CheckPiece(selected);
                var targetInfo = Match.Inst.CheckPiece(target);
                matchedPieces = selectInfo.Concat(targetInfo).Distinct().ToList();
                if(matchedPieces.Count == 0 )
                {
                    yield return StartCoroutine(PieceManager.Inst.UndoSwap());
                    break;
                }
                moveCount--;
                CanvasManager.Inst.SetMoveCountText();
                while (true)
                {
                    if (matchedPieces.Count == 0) break;
                    PieceManager.Inst.DestroyPiece(matchedPieces.SelectMany(x=>x.coords).ToList());
                    yield return StartCoroutine(PieceManager.Inst.SpawnNewPiece());
                    matchedPieces = Match.Inst.CheckPieceAll();
                }
                break;
            }

            yield return null;
        }
        if(topCount == 0 || moveCount == 0)
        {
            isGameEnd = true;
            if(topCount == 0) cleared = true;
            EndGame();
        }

        selected = null;
        isDragging = true;
    }

    public static Vector2 GetMouseWorldPos()
    {
        var mp = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mp);
    }

    public void EndGame()
    {
        Debug.Log("Game End");
        CanvasManager.Inst.ActivateWinPanel();
    }
}
