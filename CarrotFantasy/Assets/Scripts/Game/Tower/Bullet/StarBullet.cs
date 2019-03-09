using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBullet : MonoBehaviour {

    public int attackValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf)
        {
            if (collision.tag.Equals("Monster") || collision.tag.Equals("Item"))
            {
                collision.SendMessage("TakeDamage", attackValue);
            }
        }
    }
}
