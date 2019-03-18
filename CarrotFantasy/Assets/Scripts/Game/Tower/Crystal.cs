using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : TowerPersonalProperty {

    private float m_distance;
    private float m_bulletWidth;
    private float m_bulletHeight;
    private AudioSource m_audioSource;

    private void OnEnable()
    {
        if (animator == null)
        {
            return;
        }
        bulletGo = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString());
        bulletGo.SetActive(false);
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        bulletGo = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString());
        bulletGo.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = GameController.Instance.GetAudioClip("NormalModel/Tower/Attack/" + tower.towerID.ToString());
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.IsGameOver)
        {
            if (targetTrans == null)
            {
                bulletGo.SetActive(false);
            }
            return;
        }
        Attack();
	}

    protected override void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }

        if (!m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }
        
        animator.Play("Attack");
        if (targetTrans.tag.Equals("Item"))
        {
            m_distance = Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 4));
        }
        else
        {
            m_distance = Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 1));
        }
        m_bulletWidth = 3 / m_distance;
        m_bulletHeight = m_distance / 2;
        if (m_bulletWidth <= 0.5f)
        {
            m_bulletWidth = 0.5f;
        }
        else if (m_bulletWidth >= 1f)
        {
            m_bulletWidth = 1f;
        }
        bulletGo.transform.position = new Vector3((targetTrans.position.x + transform.position.x) / 2, (targetTrans.position.y + transform.position.y) / 2, 0);
        bulletGo.transform.localScale = new Vector3(1, m_bulletWidth, m_bulletHeight);
        bulletGo.SetActive(true);
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans;
    }

    protected override void DestroyTower()
    {
        bulletGo.SetActive(false);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString(), bulletGo);
        bulletGo = null;
        base.DestroyTower();
    }
}
