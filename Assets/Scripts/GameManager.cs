using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Piece[] piecePrefab;
    public int[] initialPiece = { 4, 3, 2, 6, 5, 5, 0, 6, 2, 2, 6, 3, 6, 5, 4, 4, 2, 1, 6, 4, 6, 3, 1, 6, 5, 5, 6, 4, 3, 2 };

    private Piece selected;
    private bool isDragging;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mp = GetMouseWorldPos();
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero);

            if(hit.collider != null && hit.collider.gameObject.GetComponent<Piece>() != null )
            {
                selected = hit.collider.gameObject.GetComponent<Piece>();
                isDragging = true;
                Debug.Log(selected.gameObject.name);
            }
        }

        if(Input.GetMouseButtonUp(0) && isDragging)
        {
            var mp = GetMouseWorldPos();
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Piece>() != null)
            {
                Piece target = hit.collider.gameObject.GetComponent<Piece>();
                PieceManager.Inst.SwapPiece(selected, target);
            }

            isDragging = false;
            selected = null;
        }
    }
    public static Vector2 GetMouseWorldPos()
    {
        var mp = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mp);
    }
}
