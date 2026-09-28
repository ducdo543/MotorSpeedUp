using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinUIController : MonoBehaviour
{
    private bool isWin = false;
    [SerializeField] float fadeOutTime = 0.3f;
    private CanvasGroup winCanvasGroup;
    [SerializeField] RectTransform winTextRect;
    [SerializeField] private GameObject blurOverlay;

    private void Awake()
    {
        winCanvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        winCanvasGroup.alpha = 0f;
        winTextRect.localScale = Vector3.zero;


    }

    
    void Update()
    {
        if (!isWin)
        {
            return;
        }

        if (!winTextRect.gameObject.activeSelf)
        {
            winTextRect.gameObject.SetActive(true);
        }
        if (!blurOverlay.activeSelf)
        {
            blurOverlay.SetActive(true);
        }

        winCanvasGroup.DOFade(1f, fadeOutTime).SetUpdate(true);
        winTextRect.DOScale(Vector3.one, fadeOutTime).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() =>
            {
                LoopWinText();
            });
        isWin = false;
    }

    public void SetIsWin(bool isWin)
    {
        this.isWin = isWin;
    }    

    private void LoopWinText()
    {
        winTextRect.DOScale(0.8f, 0.6f).SetEase(Ease.InOutSine).SetLoops(-1, DG.Tweening.LoopType.Yoyo).SetUpdate(true);
    }    
}
