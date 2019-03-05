using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Coin : MonoBehaviour {
    private Image m_coinImg;
    private Text m_coinText;
    private Sprite[] m_coinSprites;
    [HideInInspector]
    public int prize;

    private void Awake()
    {
        m_coinImg = transform.Find("Img_Coin").GetComponent<Image>();
        m_coinText = transform.Find("Text_Coin").GetComponent<Text>();
        m_coinSprites[0] = GameController.Instance.GetSprite("NormalModel/Game/Coin");
        m_coinSprites[1] = GameController.Instance.GetSprite("NormalModel/Game/ManyCoin");
    }

    private void OnEnable()
    {
        ShowCoin();
    }

    private void ShowCoin()
    {
        m_coinText.text = prize.ToString();
        if (prize <= 500)
        {
            m_coinImg.sprite = m_coinSprites[0];
        }
        else
        {
            m_coinImg.sprite = m_coinSprites[1];
        }
        transform.DOLocalMoveY(60, 0.5f);
        DOTween.To(() => m_coinImg.color, toColor => m_coinImg.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        Tween tween = DOTween.To(() => m_coinText.color, toColor => m_coinText.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        tween.OnComplete(DestroyCoin);
    }

    private void DestroyCoin()
    {
        transform.localPosition = Vector3.zero;
        m_coinImg.color = m_coinText.color = new Color(1, 1, 1, 1);
        GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
    }
}
