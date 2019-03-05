using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    public int monsterID;       //怪物ID
    public int HP;              //总血量
    public int currentHP;       //当前血量
    public float moveSpeed;     //当前移动速度
    public float initMoveSpeed; //初始移动速度
    public int prize;           //奖励

    //引用
    private Animator m_animator;
    //public RuntimeAnimatorController runtimeAnimatorController;
    private Slider m_slider;
    public GameObject shitGo;
    private GameController m_gameController;
    private List<Vector3> m_monsterPathPosList;

    //用于计数的属性或开关
    private int m_roadPointIndex = 1;     //路点索引
    private bool m_reachCarrot;           //到达终点
    private bool m_isDecreaseSpeed;       //是否减速

    private float m_decreaseSpeedTimeVal; //减速计时器
    private float m_decreaseTime;         //减速持续的时间

    //资源
    public AudioClip dieAudioClip;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_slider = transform.Find("MonsterCanvas").Find("HPSlider").GetComponent<Slider>();
        m_slider.gameObject.SetActive(false);
        m_gameController = GameController.Instance;
        m_monsterPathPosList = m_gameController.mapMaker.monsterPathPos;
    }

    private void OnEnable()
    {
        InitMonsterInfo();
        TurnMonster(true);
    }

    private void InitMonsterInfo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        moveSpeed = 0;
        m_roadPointIndex = 1;
        dieAudioClip = null;
        m_reachCarrot = false;
        m_slider.value = 1;
        m_slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);
        m_decreaseSpeedTimeVal = 0;
        m_decreaseTime = 0;
        CancelDecreaseDebuff();
    }

    private void TakeDamage(int attackValue)
    {
        m_slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            //播放死亡音效
            DestroyMonster();
            return;
        }
        m_slider.value = (float)currentHP / HP;
    }

    //取消减速buff
    private void CancelDecreaseDebuff()
    {

    }

    private void Update()
    {
        if (m_gameController.isPause || m_gameController.IsGameOver)
        {
            return;
        }
        if (m_reachCarrot)
        {
            DestroyMonster();
            //萝卜减血
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_monsterPathPosList[m_roadPointIndex],
                1 / (Vector3.Distance(transform.position, m_monsterPathPosList[m_roadPointIndex])) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            if (Vector3.Distance(transform.position, m_monsterPathPosList[m_roadPointIndex]) < 0.01f)
            {
                TurnMonster();
                m_roadPointIndex++;
                if (m_roadPointIndex >= m_monsterPathPosList.Count)
                {
                    m_reachCarrot = true;
                }
            }
        }
    }

    private void TurnMonster(bool isBegin = false)
    {
        int roadIndex = isBegin ? 0 : m_roadPointIndex;
        if (roadIndex + 1 < m_monsterPathPosList.Count)
        {
            float xOffset = m_monsterPathPosList[roadIndex].x - m_monsterPathPosList[roadIndex + 1].x;
            if (xOffset < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        m_slider.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void DestroyMonster()
    {
        if (!m_reachCarrot) //被杀死
        {
            //生成奖励、金币
            GameObject coin = m_gameController.GetGameObject("CoinCanvas");
            coin.transform.Find("Emp_Coin").GetComponent<Coin>().prize = prize;
            coin.transform.position = transform.position;
            coin.transform.SetParent(transform);

            m_gameController.ChangeCoin(prize);
        }
        GameObject go = m_gameController.GetGameObject("DestoryEffect");
        go.transform.position = transform.position;
        go.transform.SetParent(transform);

        InitMonsterInfo();
        m_gameController.PushGameObjectToFactory("MonsterPrefab", gameObject);
        m_gameController.killMonsterNum++;
        m_gameController.killMonsterTotalNum++;
    }

    public void GetMonsterProperty()
    {
        m_animator.runtimeAnimatorController = GameController.Instance.controllers[monsterID - 1];
    }

#if Game
    private void OnMouseDown()
    {
        if (m_gameController.targetTrans == null)
        {
            m_gameController.targetTrans = transform;
            m_gameController.ShowSignal();
        }
        else if (m_gameController.targetTrans != transform)
        {
            m_gameController.HideSignal();
            m_gameController.targetTrans = transform;
            m_gameController.ShowSignal();
        }
        else
        {
            m_gameController.HideSignal();
        }
    }
#endif
}
