using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizePage : MonoBehaviour {

    private Image m_prizeImg;
    private Image m_instructionImg;
    private Text m_prizeNameText;
    private Animator m_animator;
    private NormalModelPanel m_normalModelPanel;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_prizeImg = transform.Find("Img_Prize").GetComponent<Image>();
        m_instructionImg = transform.Find("Img_Instruction").GetComponent<Image>();
        m_prizeNameText = transform.Find("Img_Prize").Find("Text_PrizeName").GetComponent<Text>(); 
        m_normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }

    private void OnEnable()
    {
        int randomNum = Random.Range(1, 4);
        string prizeName = "";
        if (randomNum >= 4 && GameManager.Instance.m_playerManager.monsterPetDatas.Count < 3)
        {
            int randomEggNum = 0;
            do
            {
                randomEggNum = Random.Range(1, 4);
            } while (HasPetEgg(randomEggNum));
            MonsterPetData monsterPetData = new MonsterPetData
            {
                monsterLevel = 1,
                remainCookies = 0,
                remainMilk = 0,
                monsterID = randomEggNum
            };
            GameManager.Instance.m_playerManager.monsterPetDatas.Add(monsterPetData);
            prizeName = "宠物蛋";
        }
        else
        {
            switch (randomNum)
            {
                case 1:
                    prizeName = "牛奶";
                    GameManager.Instance.m_playerManager.milk += 20;
                    break;
                case 2:
                    prizeName = "饼干";
                    GameManager.Instance.m_playerManager.cookies += 20;
                    break;
                case 3:
                    prizeName = "窝";
                    GameManager.Instance.m_playerManager.nest += 1;
                    break;
                default:
                    break;
            }
        }
        m_prizeNameText.text = prizeName;
        m_prizeImg.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Prize" + randomNum.ToString());
        m_instructionImg.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Instruction" + randomNum.ToString());
        m_animator.Play("PrizePage");
    }

    private bool HasPetEgg(int eggID)
    {
        for (int i = 0; i < GameManager.Instance.m_playerManager.monsterPetDatas.Count; i++)
        {
            if (GameManager.Instance.m_playerManager.monsterPetDatas[i].monsterID == eggID)
            {
                return true;
            }
        }
        return false;
    }

    public void ClosePrizePage()
    {
        m_normalModelPanel.ClosePrizePage();
        GameController.Instance.isPause = false;
    }
}
