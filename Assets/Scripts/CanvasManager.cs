using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField]
    private TextMeshProUGUI topCount;
    [SerializeField]
    private TextMeshProUGUI moveCount;
    [SerializeField]
    private TextMeshProUGUI currentScore;
    [SerializeField]
    private Image background;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private ClickUI retryButton;
    [SerializeField]
    private ClickUI exitButton;
    [SerializeField]
    private TextMeshProUGUI endScore;

    private void Start()
    {
        Panel.SetActive(false);
        retryButton.AddListenerOnly(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
        exitButton.AddListenerOnly(() =>
        {
            Application.Quit();
        });
        SetTopCountText();
        SetMoveCountText();
        SetScoreText();

    }

    public void SetTopCountText()
    {
        topCount.SetText("Tops\n" + GameManager.Inst.topCount.ToString());
    }

    public void SetMoveCountText()
    {
        moveCount.SetText("Moves\n" + GameManager.Inst.moveCount.ToString());
    }

    public void SetScoreText()
    {
        currentScore.SetText("Score\n" + GameManager.Inst.totalScore.ToString());
        endScore.SetText("Score : " + GameManager.Inst.totalScore.ToString());
    }
    public void ActivateWinPanel()
    {
        Panel.SetActive(true);
    }
}
