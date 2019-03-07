﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSellTower : MonoBehaviour {

    private int m_price;
    private Button m_button;
    private Text m_text;
    private GameController m_gameController;

    private void OnEnable()
    {
        if (m_text == null)
        {
            return;
        }
        m_price = m_gameController.selectedGrid.towerPersonalProperty.sellPrice;
        m_text.text = m_price.ToString();
    }

    // Use this for initialization
    void Start () {
        m_gameController = GameController.Instance;
        m_button.GetComponent<Button>();
        m_button.onClick.AddListener(OnBtnSellTowerClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnBtnSellTowerClicked()
    {
        m_gameController.selectedGrid.towerPersonalProperty.SellTower();
        m_gameController.selectedGrid.InitGrid();
        m_gameController.selectedGrid.m_handleTowerCanvas.SetActive(false);
        m_gameController.selectedGrid.HideGrid();
        m_gameController.selectedGrid = null;
    }
}
