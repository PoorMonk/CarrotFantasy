using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPet : MonoBehaviour {

    public MonsterPetData monsterPetData;
    public MonsterNestPanel monsterNestPanel;

    private GameObject[] m_monsterLevelGo;

    public Sprite[] buttonSprites;

    //Egg
    private GameObject m_instructionImgGo;
    //Baby
    private GameObject m_feedGo;
    private Text txt_milk;
    private Text txt_cookie;
    private Button btn_milk;
    private Button btn_cookie;
    private Image img_milk;
    private Image img_cookie;
    //Normal
    private GameObject m_talkRightGo;
    private GameObject m_talkLeftGo;

    private void Awake()
    {
        m_monsterLevelGo = new GameObject[3];
        m_monsterLevelGo[0] = transform.Find("EmpEgg").gameObject;
        m_monsterLevelGo[1] = transform.Find("EmpBaby").gameObject;
        m_monsterLevelGo[2] = transform.Find("EmpNormal").gameObject;

        m_instructionImgGo = m_monsterLevelGo[0].transform.Find("ImgInstruction").gameObject;
        m_instructionImgGo.SetActive(false);

        m_feedGo = m_monsterLevelGo[1].transform.Find("EmpFeed").gameObject;
        m_feedGo.SetActive(false);
        btn_milk = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnMilk").GetComponent<Button>();
        btn_cookie = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnCookie").GetComponent<Button>();
        img_milk = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnMilk").GetComponent<Image>();
        img_cookie = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnCookie").GetComponent<Image>();
        txt_cookie = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnCookie").Find("Text").GetComponent<Text>();
        txt_milk = m_monsterLevelGo[1].transform.Find("EmpFeed").Find("BtnMilk").Find("Text").GetComponent<Text>();

        m_talkLeftGo = transform.Find("ImgTalkLeft").gameObject;
        m_talkRightGo = transform.Find("ImgTalkRight").gameObject;
    }

    private void OnEnable()
    {
        InitMonsterPet();
    }

    private void Start()
    {
        InitMonsterPet();
    }

    public void ClickPet()
    {
        GameManager.Instance.m_audioSourceManager.PlayEffectMusic(GameManager.Instance.m_factoryManager.m_audioClipFactory.GetSingleResource("MonsterNest/PetSound" + monsterPetData.monsterLevel.ToString()));
        switch (monsterPetData.monsterLevel)
        {
            case 1:
                if (GameManager.Instance.m_playerManager.nest >= 1)
                {
                    GameManager.Instance.m_playerManager.nest--;
                    ToNormal();
                    monsterPetData.monsterLevel++;
                    ShowMonster();
                    monsterNestPanel.UpdateText();
                }
                else
                {
                    m_instructionImgGo.SetActive(true);
                    Invoke("CloseTalkUI", 2f);
                }
                break;
            case 2:
                if (m_feedGo.activeSelf)
                {
                    m_feedGo.SetActive(false);
                }
                else
                {
                    m_feedGo.SetActive(true);
                    if (GameManager.Instance.m_playerManager.milk == 0)
                    {
                        img_milk.sprite = buttonSprites[1];
                        btn_milk.interactable = false;
                    }
                    else
                    {
                        img_milk.sprite = buttonSprites[0];
                        btn_milk.interactable = true;
                    }
                    if (GameManager.Instance.m_playerManager.cookies == 0)
                    {
                        img_cookie.sprite = buttonSprites[3];
                        btn_cookie.interactable = false;
                    }
                    else
                    {
                        img_cookie.sprite = buttonSprites[2];
                        btn_cookie.interactable = true;
                    }
                    if (monsterPetData.remainMilk == 0)
                    {
                        btn_milk.gameObject.SetActive(false);
                    }
                    else
                    {
                        txt_milk.text = monsterPetData.remainMilk.ToString();
                        btn_milk.gameObject.SetActive(true);
                    }
                    if (monsterPetData.remainCookies == 0)
                    {
                        btn_cookie.gameObject.SetActive(false);
                    }
                    else
                    {
                        txt_cookie.text = monsterPetData.remainCookies.ToString();
                        btn_cookie.gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                int randomNum = Random.Range(0, 2);
                if (randomNum == 1)
                {
                    m_talkLeftGo.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                else
                {
                    m_talkRightGo.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                break;
            default:
                break;
        }
    }

    //关闭对话框
    private void CloseTalkUI()
    {
        m_instructionImgGo.SetActive(false);
        m_talkLeftGo.SetActive(false);
        m_talkRightGo.SetActive(false);
    }

    private void ToNormal()
    {
        if (monsterPetData.remainMilk == 0 && monsterPetData.remainCookies == 0)
        {
            GameManager.Instance.m_audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/PetChange"));
            monsterPetData.monsterLevel++;
            if (monsterPetData.monsterLevel >=3)
            {
                GameManager.Instance.m_playerManager.unLockedNormalModelLevelList[monsterPetData.monsterID * 5 - 1].m_unLocked = true;
                GameManager.Instance.m_playerManager.burriedLevelNum++;
                ShowMonster();
            }
            else
            {
                ShowMonster();
            }
        }
        SaveMonsterData();
    }

    public void InitMonsterPet()
    {
        if (monsterPetData.remainMilk == 0)
        {
            monsterPetData.remainMilk = monsterPetData.monsterID * 60;

        }
        if (monsterPetData.remainCookies == 0)
        {
            monsterPetData.remainCookies = monsterPetData.monsterID * 30;
        }
        ShowMonster();
    }

    public void ShowMonster()
    {
        for (int i = 0; i < m_monsterLevelGo.Length; i++)
        {
            m_monsterLevelGo[i].SetActive(false);
            if ((i + 1) == monsterPetData.monsterLevel)
            {
                m_monsterLevelGo[i].SetActive(true);
                Sprite petSprite = null;
                switch (monsterPetData.monsterLevel)
                {
                    case 1:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Egg/" + monsterPetData.monsterID.ToString());
                        break;
                    case 2:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Baby/" + monsterPetData.monsterID.ToString());
                        break;
                    case 3:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Normal/" + monsterPetData.monsterID.ToString());
                        break;
                    default:
                        break;
                }
                Image monsterImg = m_monsterLevelGo[i].transform.Find("ImgPet").GetComponent<Image>();
                monsterImg.sprite = petSprite;
                monsterImg.SetNativeSize();
                float imgScale = 0;
                if (monsterPetData.monsterLevel == 1)
                {
                    imgScale = 2;
                }
                else
                {
                    imgScale = 1 + (monsterPetData.monsterLevel - 1) * 0.5f;
                }
                monsterImg.transform.localScale = new Vector3(imgScale, imgScale, 1);
            }
        }
    }

    //喂牛奶
    public void FeedMilk()
    {
        //播放喂养动画与音效
        GameManager.Instance.m_audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/Feed01"));
        GameObject heartGo = GameManager.Instance.GetGameObjectResource(FactoryType.UIFactory, "ImgHeart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.m_playerManager.milk >= monsterPetData.remainMilk)
        {
            GameManager.Instance.m_playerManager.milk -= monsterPetData.remainMilk;
            monsterPetData.remainMilk = 0;
            //更新文本
            monsterNestPanel.UpdateText();
        }
        else
        {
            monsterPetData.remainMilk -= GameManager.Instance.m_playerManager.milk;
            GameManager.Instance.m_playerManager.milk = 0;
            btn_milk.gameObject.SetActive(false);
        }
        m_feedGo.SetActive(false);
        Invoke("ToNormal", 0.433f);
    }

    //喂饼干
    public void FeedCookie()
    {
        //播放喂养动画与音效
        GameManager.Instance.m_audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/Feed02"));
        GameObject heartGo = GameManager.Instance.GetGameObjectResource(FactoryType.UIFactory, "ImgHeart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.m_playerManager.cookies >= monsterPetData.remainCookies)
        {

            GameManager.Instance.m_playerManager.cookies -= monsterPetData.remainCookies;
            monsterPetData.remainCookies = 0;
            //更新文本
            monsterNestPanel.UpdateText();

        }
        else
        {
            monsterPetData.remainCookies -= GameManager.Instance.m_playerManager.cookies;
            GameManager.Instance.m_playerManager.cookies = 0;
            btn_cookie.gameObject.SetActive(false);
        }
        m_feedGo.SetActive(false);
        Invoke("ToNormal", 0.433f);
    }

    //保存宠物数据
    private void SaveMonsterData()
    {
        for (int i = 0; i < GameManager.Instance.m_playerManager.monsterPetDatas.Count; i++)
        {
            if (GameManager.Instance.m_playerManager.monsterPetDatas[i].monsterID == monsterPetData.monsterID)
            {
                GameManager.Instance.m_playerManager.monsterPetDatas[i] = monsterPetData;
            }
        }
    }
}

[System.Serializable]
public struct MonsterPetData
{
    public int monsterLevel;
    public int remainCookies;
    public int remainMilk;
    public int monsterID;
}
