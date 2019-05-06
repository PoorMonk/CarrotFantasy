using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleTest : MonoBehaviour {

    private float m_beginTime;
    private float m_endTime;
    private bool m_ub = false;
    private bool m_lub = false;
    private bool m_fub = false;

    private void Start()
    {
        m_beginTime = Time.unscaledTime;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!m_ub)
        {
            for (int i = 0; i < 10000000; i++)
            {

            }
            Debug.Log("Update: " + (Time.unscaledTime - m_beginTime));
            m_ub = true;
        }
    }

    private void LateUpdate()
    {
        if (!m_lub)
        {
            for (int i = 0; i < 10000000; i++)
            {

            }
            Debug.Log("LateUpdate: " + (Time.unscaledTime - m_beginTime));
            m_lub = true;
        }
    }

    private void FixedUpdate()
    {
        if (!m_fub)
        {
            for (int i = 0; i < 10000000; i++) 
            {

            }
            Debug.Log("FixedUpdate: " + (Time.unscaledTime - m_beginTime));
            m_fub = true;
        }
    }
}
