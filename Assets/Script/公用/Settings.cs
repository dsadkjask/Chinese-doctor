using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    //时间
    public const float secondThreshold = 0.00001f;    //数值越小时间越快
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //场景转换
    public const float fadeDuration = 0.8f;

    public const float itemFadeDuration = 0.35f;//物品过渡时间
    public const float targetAlpha = 0.45f;//目标透明度
}
