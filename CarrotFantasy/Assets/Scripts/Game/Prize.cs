using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour {

    public void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GiftGot");
        GameController.Instance.isPause = true;
        GameController.Instance.ShowPrizePage();
        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
    }
}
