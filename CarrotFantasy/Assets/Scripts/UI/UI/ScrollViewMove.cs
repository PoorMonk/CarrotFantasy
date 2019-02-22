using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollViewMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    public int m_cellLength;                // 单元格长度
    public int m_spacing;                   // 间隔
    public int m_leftOffset;                // 左偏移
    private float m_upperLimit;             // 上限
    private float m_lowerLimit;             // 下限
    private ScrollRect m_scrollRect;
    private float m_beginPosX;              // 开始位置
    private float m_endPosX;                // 结束位置
    private float m_lastProportion;         // 上一个位置的比例
    private float m_oneItemLength;          // 滑动一个单元格所需要的距离
    private float m_contentLength;          // 容器长度
    private int m_currentIndex;             // 当前单元格索引
    public int m_totalItemNum;              // 共有几个单元格
    private float m_firstItemLength;        // 移动第一个单元格的距离
    private float m_oneItemProportion;      // 移动一个单元格所占比例

    public int CurrentIndex
    {
        get
        {
            return m_currentIndex;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_beginPosX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float offsetX = 0;
        m_endPosX = Input.mousePosition.x;
        offsetX = (m_beginPosX - m_endPosX) * 2;
        //Debug.Log("offsetX:" + offsetX);
        //Debug.Log("m_firstItemLength:" + m_firstItemLength);
        if (Mathf.Abs(offsetX) > m_firstItemLength)
        {
            if (0 < offsetX) // 右滑
            {
                if (m_currentIndex >= m_totalItemNum)
                {
                    return;
                }
                int moveCount = (int)((offsetX - m_firstItemLength) / m_oneItemLength) + 1;
                m_currentIndex += moveCount;
                if (m_currentIndex >= m_totalItemNum)  // 因为是加的moveCount，可能会越界，所以需要再加一次判断
                {
                    m_currentIndex = m_totalItemNum;
                }
                m_lastProportion += m_oneItemProportion * moveCount;
                if (m_lastProportion >= m_upperLimit)
                {
                    m_lastProportion = 1;
                }
            }
            else  // 左滑
            {
                if (m_currentIndex <= 1)
                {
                    return;
                }
                int moveCount = (int)((offsetX + m_firstItemLength) / m_oneItemLength) - 1;
                m_currentIndex += moveCount;
                if (m_currentIndex <= 1)  // 因为是加的moveCount，可能会越界，所以需要再加一次判断
                {
                    m_currentIndex = 1;
                }
                m_lastProportion += m_oneItemProportion * moveCount;
                if (m_lastProportion <= m_lowerLimit)
                {
                    m_lastProportion = 0; // 不能是m_lowerLimit 
                }
            }
        }

        DOTween.To(()=>m_scrollRect.horizontalNormalizedPosition, lerpValue=>m_scrollRect.horizontalNormalizedPosition=lerpValue, m_lastProportion, 0.3f).SetEase(Ease.Linear);
    }

    public void Init()
    {
        if (m_scrollRect != null)
        {
            m_scrollRect.horizontalNormalizedPosition = 0;
            m_lastProportion = 0;
            m_currentIndex = 1;
        }
    }

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_contentLength = m_scrollRect.content.rect.xMax - 2 * m_leftOffset - m_cellLength;
        m_firstItemLength = m_cellLength / 2 + m_leftOffset;
        m_oneItemLength = m_cellLength + m_spacing;
        m_oneItemProportion = m_oneItemLength / m_contentLength;
        m_lowerLimit = m_firstItemLength / m_contentLength;
        m_upperLimit = 1 - m_lowerLimit;
        m_currentIndex = 1;
        m_scrollRect.horizontalNormalizedPosition = 0;

        // Debug.Log("right:" + m_scrollRect.content.rect.xMax);
        // Debug.Log("contentLength:" + m_contentLength);
        // Debug.Log("firstItemLength:" + m_firstItemLength);
        // Debug.Log("oneItemLength:" + m_oneItemLength);
        // Debug.Log("oneItemProportion:" + m_oneItemProportion);
        // Debug.Log("m_lowerLimit:" + m_lowerLimit);
        // Debug.Log("m_upperLimit:" + m_upperLimit);
    }
}
