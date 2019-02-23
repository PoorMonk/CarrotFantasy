using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTest : MonoBehaviour {

    private GameObject one;
    private GameObject two;
    private Button btn;

    private void Start()
    {
        btn = GameObject.Find("BT").GetComponent<Button>();
        two = transform.parent.Find("Two").gameObject;
        Debug.Log(two);
        btn.onClick.AddListener(ShowHide);
    }

    public void ShowHide()
    {
        gameObject.SetActive(false);
        two.SetActive(true);
    }
}
