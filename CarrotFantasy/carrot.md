Assets中新建一个SteamingAssets文件夹，用来存放发布时不会被Unity压缩的文件。（和Resources文件夹的区别如下图）

![](https://raw.githubusercontent.com/PoorMonk/MarkDownPhotos/master/5.png)

**Canvas中ScrollView的使用：**

如何确定ScrollView本身的大小（Width和Height属性）？子对象Content中，在添加了Grid Layout Group之后，如何设置Padding中的值以及Cell Size和Space的值？

设置好要显示的物体的合适大小，然后再加上Padding中的边框间隔就是ScrollView的大小。

----

UI宽度=原来的宽度+（每一个单元格长度+间隙）*（单元格数量-1）

一 我们实现的是要翻一页书或者多页书，使用的方法是让ScrollView用它自身长度的单位化比例来实现

实现这个需求首先要知道：

1. content的总长，间隔值，单元格长度，左偏移量
2. 玩家鼠标的开始位置与结束位置：
	开始位置-结束位置>0，则右滑；
	开始位置-结束位置<0，则左滑；
	差值来决定滑动几个单元格
3. 移动一个单元格玩家鼠标需要滑动的距离：一个单元格长度/2+左偏移量
4. 移动多个单元格玩家鼠标需要滑动的距离：第一个的单元格长度+左偏移量，之后的每一个滑动都是单元格长度/2+间隔

``` C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollViewMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler 
{
	private ScrollRect m_scrollRect;
	public int m_cellLength;                // 单元格长度
	public int m_spacing;                   // 间隔
	public int m_leftOffset;                // 左偏移
	public int m_totalItemNum;              // 共有几个单元格
	private float m_upperLimit;             // 上限
	private float m_lowerLimit;             // 下限	    
	private float m_beginPosX;              // 开始位置
	private float m_endPosX;                // 结束位置
	private float m_lastProportion;         // 上一个位置的比例
	private float m_oneItemLength;          // 滑动一个单元格所需要的距离
	private float m_contentLength;          // 容器长度
	private int m_currentIndex;             // 当前单元格索引		    
	private float m_firstItemLength;        // 移动第一个单元格的距离
	private float m_oneItemProportion;      // 移动一个单元格所占比例

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
```
		


二 第二张方法来实现滑动效果是通过修改content的localPosition。

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

Unity宏定义

Build Setting --> Player Settings --> Scripting Define Symbols下定义

**UI显示问题：**

1 将UIPanel做成预制体，通过工厂方法创建后，先是会生成在Hierarchy面板下，设置parent后才会显示在Canvas下，但是这样操作会有问题，UI显示**大小比例不对**。

**解决方法**：先将prefab拖到Canvas下，将缩放锚点由Stretch改为Center，然后点击Apply；再通过工厂创建设置到Canvas下后，修改其LocalPosition(Vector3.Zero)和LocalScale(Vector3.One)。

2 UGUI中由于不依赖摄像机渲染，无法通过修改Z值来改变物体间的层级关系，可以用如下函数：

	transform.SetSillingIndex(int); // 参数越大越后渲染

3 Scroll View的报空问题（DOTWEEN :: An error inside a tween callback was silently taken care of > Object reference not set to an instance of an object）：

父物体先初始化的时候，挂有Scroll View的子物体还没初始化（也就是Awake）没调用，所有用到Scroll View脚本的方法时会有问题，需要加判断

4 MainPanel面板的Button无法点击

原因：事件交互依赖EventSystem，所以不能只让Canvas不销毁，EventSystem也需要添加不销毁的脚本。

5 由大关卡进入小关卡时，关卡界面没显示出来，小关卡所能使用的炮塔UI也不显示

原因：在创建Sprite对象时，需要设置父物体；由于代码中错把这一步（transform.SetParent(parentTrans)）写成了SetActive，所以造成了上述问题。（就这么简单的问题花了好几个小时来查找原因，所以写代码一定要细心）

6 Scroll View滑动的时候，不会立即停在当前页面

解决方案：去掉Scroll Rect组件中的Inertia属性。

7 生成的格子Grid不响应OnMouseDown事件

解决方案：将Grid Prefab的BoxCollider改为BoxCollider 2D类型

8 获取屏幕及格子大小

	public void CalculateSize()
    {
        // 视口中左下右上的位置
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightTop = new Vector3(1, 1);

        Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown); //将左下角位置转换为Vector3
        Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightTop); //将右上角位置转换为Vector3

        m_mapWidth = posTwo.x - posOne.x;	//获取到屏幕宽度
        m_mapHeight = posTwo.y - posOne.y;	//获取到屏幕高度

        m_gridWidth = m_mapWidth / xCol;	//屏宽除以列数就是单个格子的宽度
        m_gridHeight = m_mapHeight / yRow;	//屏高除以行数就是单个格子的高度
    }

9 Canvas的摄像机与游戏场景的摄像机是分别渲染再融合输出到屏幕上的，如何配合使用？

首先，将Canvas组件的Render Mode由原来的Screen Space - Overlay改为World Space，然后拖到Sprite图片下；Reset Canvas的Transform，同时将Scale缩小100倍（由1改为0.01），Order in Layer与Sprite的保持一致，这样就能显示到一起同时使用UGUI的属性方法。


**Json读取问题：**

1 数据读取成功，但是一直报空。

原因：数据保存时是以一维数组保存的，所以如果要读取的类信息中有二维数组，要转成一维数组（List之类的），然后自己转换成对应的下标。

2 游戏开始的时候，怪物ID列表一直报空

原因：Json文件中保存的ID列表和类中定义的名字不一样，所以读取不到

3 Json文件在读取自定义类信息时，类中不能包含有构造方法。

