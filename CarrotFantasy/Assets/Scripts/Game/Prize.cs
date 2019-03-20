using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour {

    public void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GiftGot");
        GameController.Instance.isPause = true;
#if Game
        GameController.Instance.ShowPrizePage();
#endif
        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
    }

    private void Update()
    {
        if (GameController.Instance.IsGameOver)
        {
            GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
        }
    }
}
