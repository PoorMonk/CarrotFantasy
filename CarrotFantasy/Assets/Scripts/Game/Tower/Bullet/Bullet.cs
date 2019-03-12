using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [HideInInspector]
    public Transform targetTrans;

    public int towerID;
    public int towerLevel;
    public int moveSpeed;
    public int attackValue;

    protected virtual void Update()
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
            transform.position = Vector3.Lerp(transform.position, targetTrans.position,
                1 / Vector3.Distance(transform.position, targetTrans.position) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position);
        }

        if (targetTrans.tag.Equals("Item"))
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3),
                1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        //Debug.Log("targetTrans.z: " + targetTrans.position.z);
        //Debug.Log("transform.z: " + transform.position.z);
    }

    protected virtual void DestroyBullet()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullet/" + towerLevel.ToString(), gameObject);
    }

    protected virtual void CreateEffect()
    {
        GameObject towerEffect = GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        towerEffect.transform.position = transform.position;
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
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
                    DestroyBullet();
                    CreateEffect();
                }
            }
        }
    }
}
