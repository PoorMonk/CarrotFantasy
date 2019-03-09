using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnUpLevel : MonoBehaviour {

    public int price;
    private Button m_button;
    private Text m_text;
    private Image m_image;
    private Sprite m_canUpLevelSp;
    private Sprite m_cantUpLevelSp;
    private Sprite m_reachHighestLevelSp;
    private GameController m_gameController;

    private void OnEnable()
    {
        if (m_text == null)
        {
            return;
        }
        UpdateUIView();
    }

    // Use this for initialization
    void Start () {
        m_gameController = GameController.Instance;
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_canUpLevelSp = m_gameController.GetSprite("NormalModel/Game/Tower/Btn_CanUpLevel");
        m_cantUpLevelSp = m_gameController.GetSprite("NormalModel/Game/Tower/Btn_CantUpLevel");
        m_reachHighestLevelSp = m_gameController.GetSprite("NormalModel/Game/Tower/Btn_ReachHighestLevel");
        m_button.onClick.AddListener(OnBtnUpLevelClicked);
        m_text = transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void UpdateUIView()
    {
        if (m_gameController.selectedGrid.towerLevel >= 3)
        {
            m_image.sprite = m_reachHighestLevelSp;
            m_button.interactable = false;
            m_text.enabled = false;
        }
        else
        {
            m_text.enabled = true;
            price = m_gameController.selectedGrid.towerPersonalProperty.upLevelPrice;
            m_text.text = price.ToString();
            if (m_gameController.coin >= price)
            {
                m_image.sprite = m_canUpLevelSp;
                m_button.interactable = true;
            }
            else
            {
                m_image.sprite = m_cantUpLevelSp;
                m_button.interactable = false;
            }
        }
    }

    private void OnBtnUpLevelClicked()
    {
        m_gameController.towerBuild.towerID = m_gameController.selectedGrid.tower.towerID;
        m_gameController.towerBuild.towerLevel = m_gameController.selectedGrid.towerLevel + 1;

        m_gameController.selectedGrid.towerPersonalProperty.UpLevelTower();

        GameObject towerGo = m_gameController.towerBuild.GetProduct();
        towerGo.transform.SetParent(m_gameController.selectedGrid.transform);
        towerGo.transform.position = m_gameController.selectedGrid.transform.position + new Vector3(0, 0, 1);//z不加1会无法响应鼠标点击事件
        m_gameController.selectedGrid.AfterBuild();
        m_gameController.selectedGrid.HideGrid();
        m_gameController.selectedGrid = null;
    }
}
