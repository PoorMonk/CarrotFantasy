﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShit : TowerPersonalProperty {

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.IsGameOver)
        {
            return;
        }
        if (!targetTrans.gameObject.activeSelf)
        {
            targetTrans = null;
            return;
        }
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            Attack();
            timeVal = 0;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
	}
}
