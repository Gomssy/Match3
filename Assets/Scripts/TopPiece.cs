using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPiece : Piece
{
    private bool spinning = false;
    private Animator animator;

    
    public override void DestroyThis()
    {
        if(!spinning)
        {
            spinning = true;
            animator = GetComponent<Animator>();
            animator.SetBool("IsSpinning", spinning);

            return;
        }
        GameManager.Inst.topCount--;
        CanvasManager.Inst.SetTopCountText();
        PieceManager.Inst.pieces.Remove(this);
        Destroy(this.gameObject);
    }
}
