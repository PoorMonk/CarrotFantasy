using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour {

    public int towerID;
    public int price;
    private Button m_towerBtn;
    private Sprite m_canBuildBlueSp;
    private Sprite m_cantBuildGraySp;
    private Image m_towerImg;
    private GameController m_gameController;

    private void OnEnable()
    {
        if (price != 0)
        {
            UpdateIcon();
        }
        m_gameController = GameController.Instance;
    }

    private void Start()
    {
        m_gameController = GameController.Instance;
        m_towerBtn = GetComponent<Button>();
        m_towerImg = GetComponent<Image>();
        m_canBuildBlueSp = m_gameController.GetSprite("NormalModel/Game/Tower/" + towerID.ToString() + "/CanClick1");
        m_cantBuildGraySp = m_gameController.GetSprite("NormalModel/Game/Tower/" + towerID.ToString() + "/CanClick0");
        UpdateIcon();
        price = m_gameController.towerPriceDict[towerID];
        m_towerBtn.onClick.AddListener(BuildTower);
    }

    private void BuildTower()
    {
        m_gameController.PlayEffectMusic("NormalModel/Tower/TowerBulid");

        m_gameController.towerBuild.towerID = towerID;
        m_gameController.towerBuild.towerLevel = 1;

        GameObject towerGo = m_gameController.towerBuild.GetProduct();
        towerGo.transform.SetParent(m_gameController.selectedGrid.transform);
        towerGo.transform.position = m_gameController.selectedGrid.transform.position + new Vector3(0, 0, 1);

        GameObject buildEffect = m_gameController.GetGameObject("BuildEffect");
        buildEffect.transform.SetParent(m_gameController.transform);
        buildEffect.transform.position = m_gameController.selectedGrid.transform.position;
#if Game
        m_gameController.selectedGrid.AfterBuild();
        m_gameController.selectedGrid.HideGrid();
#endif
        m_gameController.selectedGrid.isHasTower = true;
        m_gameController.ChangeCoin(-price);

        m_gameController.selectedGrid = null;

        m_gameController.handleTowerCanvas.SetActive(false);
    }

    private void UpdateIcon()
    {
        if (m_gameController.coin >= price)
        {
            m_towerImg.sprite = m_canBuildBlueSp;
        }
        else
        {
            m_towerImg.sprite = m_cantBuildGraySp;
        }
    }
}
