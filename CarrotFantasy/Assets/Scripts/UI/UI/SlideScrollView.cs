using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	private RectTransform m_contentTrans;
	private float m_beginMousePositionX;
	private float m_endMousepositionX;
	private ScrollRect m_scrollRect;

	public int m_cellLength;
	public int m_spaceing;
	public int m_leftOffset;
	private float m_moveOneItemLength;

	private Vector3 m_currentContentLocalPos;
    private Vector3 m_contentInitPos;

	public int m_totalItemNum;
	private int m_currentIndex;

    public Text pageText;

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
		float moveDistance = 0; // 当次需要滑动的距离
		offsetX = m_beginMousePositionX - m_endMousepositionX;

		if (offsetX > 0) // 右滑
		{
            //if (CurrentIndex >= m_totalItemNum)
            //{
            //	return;
            //}
            //moveDistance = -m_moveOneItemLength;
            //         m_currentIndex++;
            Move(true);
		}
		else // 左滑
		{
            //if (CurrentIndex <= 1)
            //{
            //	return;
            //}
            //moveDistance = m_moveOneItemLength;
            //         m_currentIndex--;
            Move(false);
		}

		//DOTween.To(()=>m_contentTrans.localPosition, lerpValue=>m_contentTrans.localPosition=lerpValue, m_currentContentLocalPos + new Vector3(moveDistance,0,0), 0.3f).SetEase(Ease.Linear);
		//m_currentContentLocalPos += new Vector3(moveDistance, 0, 0);
	}

    public void OnLeftPageBtnClicked()
    {
        Move(false);
    }

    public void OnRightPageBtnClicked()
    {
        Move(true);
    }

    private void Move(bool isRight)
    {
        float moveDistance = 0;
        if ((isRight && m_currentIndex >= m_totalItemNum) || (!isRight && m_currentIndex <= 0))
        {
            return;
        }
        moveDistance = isRight ? m_moveOneItemLength : -m_moveOneItemLength;
        if (isRight)
        {
            m_currentIndex++;
        }
        else
        {
            m_currentIndex--;
        }
        if (pageText != null)
        {
            pageText.text = m_currentIndex.ToString() + "/" + m_totalItemNum;
        }
        DOTween.To(() => m_contentTrans.localPosition, lerpValue => m_contentTrans.localPosition = lerpValue, m_currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.3f).SetEase(Ease.Linear);
        m_currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }
}
