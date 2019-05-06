using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAdapter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float standardWidth = 1024f;
        float standardHeight = 768f;
        float deviceWidth = 0f;
        float deviceHeight = 0f;
        float adjustor = 0f;

        //获取设备宽高
        deviceWidth = Screen.width;
        deviceHeight = Screen.height;

        //计算宽高比例
        float standardAspect = standardWidth / standardHeight;
        float deviceAspect = deviceWidth / deviceHeight;

        if (deviceAspect < standardAspect)
        {
            adjustor = standardAspect / deviceAspect;
        }

        CanvasScaler canvasScaler = transform.GetComponent<CanvasScaler>();
        if (adjustor == 0)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
	}
	
}
