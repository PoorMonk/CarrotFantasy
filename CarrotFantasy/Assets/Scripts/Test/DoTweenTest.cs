using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DoTweenTest : MonoBehaviour {
    public Image m_img;
	
	void Start () {
        m_img = GetComponent<Image>();
        DOTween.To(() => m_img.color, toColor => m_img.color = toColor, new Color(0, 0, 0, 0), 2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
