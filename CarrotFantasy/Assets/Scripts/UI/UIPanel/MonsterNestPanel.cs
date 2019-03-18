using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterNestPanel : BasePanel
{
    private GameObject m_shopPanelGo;
    private Text txt_cookies;
    private Text txt_milk;
    private Text txt_nest;
    private Text txt_diamandsNum;
    private List<GameObject> m_monsterPetList;
    private Transform m_monsterGroupTrans;

    protected override void Awake()
    {
        base.Awake();
        m_shopPanelGo = transform.Find("ShopPage").gameObject;
        txt_cookies = transform.Find("Img_BG").Find("TextCookies").GetComponent<Text>();
        txt_milk = transform.Find("Img_BG").Find("TextMilk").GetComponent<Text>();
        txt_nest = transform.Find("Img_BG").Find("TextNest").GetComponent<Text>();
        m_monsterGroupTrans = transform.Find("EmpMonsterGroup");
        txt_diamandsNum = transform.Find("ShopPage").Find("ImgDiamands").Find("TextDiamands").GetComponent<Text>();
        for (int i = 1; i < 4; i++)
        {
            m_uiFacade.GetSprite("MonsterNest/Monster/Baby/" + i.ToString());
            m_uiFacade.GetSprite("MonsterNest/Monster/Egg/" + i.ToString());
            m_uiFacade.GetSprite("MonsterNest/Monster/Normal/" + i.ToString());
        }
        m_monsterPetList = new List<GameObject>();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        for (int i = 0; i < m_monsterPetList.Count; i++)
        {
            m_uiFacade.PushGameObjectToFactory(FactoryType.UIFactory, "EmpMonsters", m_monsterPetList[i]);
        }
        m_monsterPetList.Clear();
        for (int i = 0; i < GameManager.Instance.m_playerManager.monsterPetDatas.Count; i++)
        {
            GameObject monsterGo = m_uiFacade.GetGameObjectResource(FactoryType.UIFactory, "EmpMonsters");
            monsterGo.GetComponent<MonsterPet>().monsterPetData = m_uiFacade.m_playerManager.monsterPetDatas[i];
            monsterGo.GetComponent<MonsterPet>().monsterNestPanel = this;
            monsterGo.GetComponent<MonsterPet>().InitMonsterPet();
            monsterGo.transform.SetParent(m_monsterGroupTrans);
            monsterGo.transform.localScale = Vector3.one;
            m_monsterPetList.Add(monsterGo);
        }
        UpdateText();
    }

    public void ShowShopPanel()
    {
        m_shopPanelGo.SetActive(true);
    }

    public void CloseShopPanel()
    {
        m_shopPanelGo.SetActive(false);
    }

    public void ReturnToMain()
    {
        m_uiFacade.ChangeSceneState(new MainSceneState(m_uiFacade));
    }

    public void UpdateText()
    {
        txt_cookies.text = GameManager.Instance.m_playerManager.cookies.ToString();
        txt_milk.text = GameManager.Instance.m_playerManager.milk.ToString();
        txt_nest.text = GameManager.Instance.m_playerManager.nest.ToString();
        txt_diamandsNum.text = GameManager.Instance.m_playerManager.diamands.ToString();
    }

    public void BuyNest()
    {
        if (GameManager.Instance.m_playerManager.diamands >= 60)
        {
            GameManager.Instance.m_playerManager.diamands -= 60;
        }
        GameManager.Instance.m_playerManager.nest++;
        UpdateText();
    }

    public void BuyCookies()
    {
        if (GameManager.Instance.m_playerManager.diamands >= 10)
        {
            GameManager.Instance.m_playerManager.diamands -= 10;
            GameManager.Instance.m_playerManager.cookies += 15;
        }
        UpdateText();
    }

    public void BuyMilk()
    {
        if (GameManager.Instance.m_playerManager.diamands >= 1)
        {
            GameManager.Instance.m_playerManager.diamands -= 1;
            GameManager.Instance.m_playerManager.milk += 10;
        }
        UpdateText();
    }

    public void SetCanvasTrans(Transform uiTrans)
    {
        uiTrans.SetParent(m_uiFacade.m_canvasTransform);
    }
}
