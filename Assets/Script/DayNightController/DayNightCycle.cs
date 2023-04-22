using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float secondsPerDay = 120f;  // 一天的秒数
    public float degreesPerSecond = -360f / 120f;  // 太阳在一天内旋转的角度

    private Light sun;  // 方向光组件

    private void Start()
    {
        sun = GetComponent<Light>();
    }

    private void Update()
    {
        // 计算当前时间是一天中的哪个时间段
        float currentSeconds = Time.time % secondsPerDay;
        float normalizedTime = currentSeconds / secondsPerDay;  // 当前时间的百分比

        // 根据当前时间计算太阳的旋转角度
        float rotationDegrees = normalizedTime * 360f;

        // 让太阳朝着当前角度旋转
        sun.transform.rotation = Quaternion.Euler(new Vector3(rotationDegrees, 0f, 0f));
    }
}
