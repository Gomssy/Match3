using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Piece[] piecePrefab;
    public int[] initialPiece = { 4, 3, 2, 6, 5, 5, 0, 6, 2, 2, 6, 3, 6, 5, 4, 4, 2, 1, 6, 4, 6, 3, 1, 6, 5, 5, 6, 4, 3, 2 };

    [SerializeField]
    private Piece selected;
    [SerializeField]
    private List<MatchedPiece> matchedPieces = new();
    [SerializeField]
    private bool isDragging = true;

    private void Update()
    {
        if (!isDragging) return;
        if (selected != null) return;
        if(Input.GetMouseButtonDown(0))
        {
            var mp = GetMouseWorldPos();
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero);

            if(hit.collider != null && hit.collider.gameObject.GetComponent<Piece>() != null )
            {
                selected = hit.collider.gameObject.GetComponent<Piece>();
                StartCoroutine(Drag(selected.transform.position));
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
                PieceManager.Inst.SwapPiece(selected, target);
                var selectInfo = Match.Inst.CheckPiece(selected);
                var targetInfo = Match.Inst.CheckPiece(target);
                matchedPieces = selectInfo.Concat(targetInfo).Distinct().ToList();

                if(matchedPieces.Count == 0 )
                {
                    PieceManager.Inst.UndoSwap();
                    break;
                }
                break;
            }

            yield return null;
        }

        selected = null;
        isDragging = true;
    }

    public static Vector2 GetMouseWorldPos()
    {
        var mp = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mp);
    }
}
