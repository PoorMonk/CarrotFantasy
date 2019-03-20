using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBullet : Bullet
{
    private float m_attackTimeVal;
    private bool m_canTakeDamage;
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (GameController.Instance.IsGameOver)
        {
            DestroyBullet();
            return;
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestroyBullet();
            return;
        }

        //子弹的移动和转向
        if (targetTrans.tag.Equals("Monster"))
        {
            transform.LookAt(targetTrans.position);
        }

        if (targetTrans.tag.Equals("Item"))
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        if (!m_canTakeDamage)
        {
            m_attackTimeVal += Time.deltaTime;
            if (m_attackTimeVal >= 0.5f - towerLevel * 0.15f)
            {
                m_canTakeDamage = true;
                m_attackTimeVal = 0;
                DecreaseHP();
            }
        }
	}

    private void DecreaseHP()
    {
        if (!m_canTakeDamage || targetTrans == null) return;
        if (targetTrans.gameObject.activeSelf)
        {
            if (targetTrans == null || (targetTrans.tag.Equals("Item") && GameController.Instance.targetTrans == null))
            {
                return;
            }
            if (targetTrans.tag.Equals("Monster") || (targetTrans.tag.Equals("Item") && GameController.Instance.targetTrans.gameObject.tag == "Item"))
            {
                targetTrans.SendMessage("TakeDamage", attackValue);
                CreateEffect();
                m_canTakeDamage = false;
            }
        }
    }

    protected override void CreateEffect()
    {
        if (targetTrans == null)
        {
            return;
        }
        GameObject bulletEffect = GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        bulletEffect.transform.position = targetTrans.position;
    }
}
