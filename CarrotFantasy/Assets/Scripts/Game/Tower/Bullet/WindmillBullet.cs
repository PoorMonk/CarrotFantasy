using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillBullet : Bullet {

    private float m_timeVal;
    private bool m_hasTarget;

    private void OnEnable()
    {
        m_timeVal = 0f;
        m_hasTarget = false;
    }

    // Use this for initialization
    void Start () {
		
	}

    private void InitTarget()
    {
        if (targetTrans.gameObject.tag.Equals("Item"))
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 4));
        }
        else
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 1));
        }
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (GameController.Instance.IsGameOver || m_timeVal >= 2.5f)
        {
            DestroyBullet();
            return;
        }
        if (m_timeVal < 2.5f)
        {
            m_timeVal += Time.deltaTime;
        }
        if (m_hasTarget)
        {          
            transform.Translate(transform.forward * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed, Space.World);           
        }
        else
        {
            if (targetTrans != null && targetTrans.gameObject.activeSelf)
            {
                InitTarget();
                m_hasTarget = true;
            }
        }
	}

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Monster") || collider.tag.Equals("Item"))
        {
            if (collider.gameObject.activeSelf)
            {
                if (targetTrans == null || (collider.tag.Equals("Item") && /*GameController.Instance.targetTrans*/targetTrans == null))
                {
                    return;
                }
                if (collider.tag.Equals("Monster") || (collider.tag.Equals("Item") && /*GameController.Instance.targetTrans*/targetTrans == collider.transform))
                {
                    collider.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }
            }
        }
    }
}
