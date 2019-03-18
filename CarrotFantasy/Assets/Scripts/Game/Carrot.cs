using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrot : MonoBehaviour {

    private Sprite[] m_sprites;
    private SpriteRenderer m_spriteRenderer;
    private float m_timeVal;
    private Animator m_animator;
    private Text m_hpText;

    private const int SPRITE_NUM = 7;

	// Use this for initialization
	void Start () {
        m_sprites = new Sprite[SPRITE_NUM];
        for (int i = 0; i < m_sprites.Length; i++)
        {
            m_sprites[i] = GameController.Instance.GetSprite("NormalModel/Game/Carrot/" + i.ToString());
        }
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_hpText = transform.Find("HPCanvas").Find("Tex_HP").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Instance.carrotHP < 10)
        {
            m_animator.enabled = false;
        }

        if (m_timeVal >= 20)
        {
            m_animator.Play("Idle");
            m_timeVal = 0;
        }
        else
        {
            m_timeVal += Time.deltaTime;
        }
	}

    private void OnMouseDown()
    {
        if (GameController.Instance.carrotHP >= 10)
        {
            m_animator.Play("Touch");
            int randomNum = Random.Range(1, 4);
            GameController.Instance.PlayEffectMusic("NormalModel/Carrot/" + randomNum.ToString());
        }
    }

    public void UpdateCarrotUI()
    {
        int hp = GameController.Instance.carrotHP;
        m_hpText.text = hp.ToString();
        if (7 <= hp && hp <= 10)
        {
            m_spriteRenderer.sprite = m_sprites[6];
        }
        else if (1 <= hp && hp <= 6)
        {
            m_spriteRenderer.sprite = m_sprites[hp - 1];
        }
        else
        {
            //游戏结束
            GameController.Instance.normalModelPanel.ShowGameOverPage();
            GameController.Instance.IsGameOver = true;
        }
    }
}
