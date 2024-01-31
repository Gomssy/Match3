using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField]
    private TextMeshProUGUI topCount;
    [SerializeField]
    private TextMeshProUGUI moveCount;
    [SerializeField]
    private Image background;

    private void Start()
    {
        SetTopCountText();
        SetMoveCountText();

    }

    public void SetTopCountText()
    {
        topCount.SetText("Tops\n" + GameManager.Inst.topCount.ToString());
    }

    public void SetMoveCountText()
    {
        moveCount.SetText("Moves\n" + GameManager.Inst.moveCount.ToString());
    }
}
