using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShitBullet : Bullet
{
    public BulletProperty bulletProperty;
	// Use this for initialization
	void Start () {
        bulletProperty = new BulletProperty
        {
            debuffTime = towerLevel * 0.4f,
            debuffValue = towerLevel * 0.3f
        };
	}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf)
        {
            if (collision.tag.Equals("Monster"))
            {
                collision.SendMessage("DecreaseDebuff", bulletProperty);
            }
        }
        base.OnTriggerEnter2D(collision);
    }
}

public struct BulletProperty
{
    public float debuffTime;
    public float debuffValue;
}
