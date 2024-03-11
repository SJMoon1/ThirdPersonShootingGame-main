using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgOnOff : MonoBehaviour
{
    private Image miniimg;
    private float timePrev;
    void Start()
    {
        miniimg = GetComponent<Image>();
        timePrev = Time.time;
    }
    void Update()
    {
        if (Time.time - timePrev > 0.5f)
        {
            miniimg.enabled = !miniimg.enabled;
            timePrev = Time.time;
        }
    }
}
