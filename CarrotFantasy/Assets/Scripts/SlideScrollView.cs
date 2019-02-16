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

	public int m_totalItemNum;
	private int m_currentIndex;

	void Awake()
	{
		m_scrollRect = GetComponent<ScrollRect>();
		m_contentTrans = m_scrollRect.content;
		m_moveOneItemLength = m_cellLength + m_spaceing;
		m_currentContentLocalPos = m_contentTrans.localPosition;
		m_currentIndex = 1;
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
			if (m_currentIndex >= m_totalItemNum)
			{
				return;
			}
			moveDistance = -m_moveOneItemLength;
			m_currentIndex++;
		}
		else // 左滑
		{
			if (m_currentIndex <= 1)
			{
				return;
			}
			moveDistance = m_moveOneItemLength;
			m_currentIndex--;
		}

		DOTween.To(()=>m_contentTrans.localPosition, lerpValue=>m_contentTrans.localPosition=lerpValue, m_currentContentLocalPos + new Vector3(moveDistance,0,0), 0.3f).SetEase(Ease.Linear);
		m_currentContentLocalPos += new Vector3(moveDistance, 0, 0);
	}
}
