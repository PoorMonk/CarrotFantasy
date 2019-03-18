using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	private RectTransform m_contentTrans;   //Content对象
	private float m_beginMousePositionX;
	private float m_endMousepositionX;
	private ScrollRect m_scrollRect;

	public int m_cellLength;
	public int m_spaceing;
	public int m_leftOffset;
	private float m_moveOneItemLength;

	private Vector3 m_currentContentLocalPos;   //content的位置
    private Vector3 m_contentInitPos;           //保存content的初始位置

	public int m_totalItemNum;
	private int m_currentIndex;

    public Text pageText;

    private Vector2 m_contentSize;

    public bool isNeedSendMessage = false;

    public int CurrentIndex
    {
        get
        {
            return m_currentIndex;
        }
    }

    void Awake()
	{
		m_scrollRect = GetComponent<ScrollRect>();
		m_contentTrans = m_scrollRect.content;
		m_moveOneItemLength = m_cellLength + m_spaceing;
		m_currentContentLocalPos = m_contentTrans.localPosition;
        m_contentInitPos = m_currentContentLocalPos;
		m_currentIndex = 1;
        m_contentSize = m_contentTrans.sizeDelta;
	}

    public void Init()
    {
        if (m_contentTrans != null)
        {
            m_currentIndex = 1;
            m_contentTrans.localPosition = m_contentInitPos;
            m_currentContentLocalPos = m_contentInitPos;
        }
    }

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_beginMousePositionX = Input.mousePosition.x;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_endMousepositionX = Input.mousePosition.x;
		float offsetX = 0;
		offsetX = m_beginMousePositionX - m_endMousepositionX;

		if (offsetX > 0) // 右滑
		{           
            Move(false);
		}
		else // 左滑
		{
            Move(true);
		}
	}

    public void OnLeftPageBtnClicked()
    {
        Move(true);
    }

    public void OnRightPageBtnClicked()
    {
        Move(false);
    }

    //手指向右滑动表示左移一个关卡
    private void Move(bool isRight)
    {
        float moveDistance = 0;
        if ((!isRight && m_currentIndex >= m_totalItemNum) || (isRight && m_currentIndex <= 1))
        {
            return;
        }
        if (isNeedSendMessage)
        {
            UpdatePanel(!isRight);
        }
        moveDistance = isRight ? m_moveOneItemLength : -m_moveOneItemLength;
        if (isRight)
        {
            m_currentIndex--;
        }
        else
        {
            m_currentIndex++;
        }
        if (pageText != null)
        {
            pageText.text = m_currentIndex.ToString() + "/" + m_totalItemNum;
        }
        DOTween.To(() => m_contentTrans.localPosition, lerpValue => m_contentTrans.localPosition = lerpValue, m_currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.3f).SetEase(Ease.Linear);
        m_currentContentLocalPos += new Vector3(moveDistance, 0, 0);
        GameManager.Instance.m_audioSourceManager.PlayPagingAudioClip();
    }

    //设置Content的大小
    public void SetContent(int itemNum)
    {
        m_contentTrans.sizeDelta = new Vector2(m_contentTrans.sizeDelta.x + (m_cellLength + m_spaceing) * (itemNum - 1),
            m_contentTrans.sizeDelta.y);
        m_totalItemNum = itemNum;
    }

    //初始化Content的大小
    public void InitContent()
    {
        m_contentTrans.sizeDelta = m_contentSize;
    }

    //发送翻页信息
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextStage");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastStage");
        }
    }
}
