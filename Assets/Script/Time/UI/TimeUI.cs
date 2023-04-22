using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public Image dayNightImage;//日夜的图像
    public Sprite[] dayNightSprites;//日夜的图像数组
    //public RectTransform clockParent;//时间块
    public Image seasonImage;//季节图片
    public TextMeshProUGUI dateText;//日期
    public TextMeshProUGUI timeText;//时间的字

    public Sprite[] seasonSprites;//四个季节图片的数组

     private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    private void OnGameMinuteEvent(int minute, int hour, int day, Season season)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonImage.sprite = seasonSprites[(int)season];

        DayNightImage(hour);
    }

    private void DayNightImage(int hour)//日夜更替
    {
        if(hour >= 0 && hour < 6)
        {
             dayNightImage.sprite = dayNightSprites[0];
        }
        else if(hour >= 6 && hour < 12)
        {
             dayNightImage.sprite = dayNightSprites[1];
        }
        else if(hour >= 12 && hour < 18)
        {
             dayNightImage.sprite = dayNightSprites[2];
        }
        else if(hour >= 18 && hour < 24)
        {
             dayNightImage.sprite = dayNightSprites[3];
        }
    }

    

    

}
