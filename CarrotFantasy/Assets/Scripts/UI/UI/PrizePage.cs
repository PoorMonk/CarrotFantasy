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
        m_prizeNameText = transform.Find("Text_PrizeName").GetComponent<Text>();
        m_normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }
}
