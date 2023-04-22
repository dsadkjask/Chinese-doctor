using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //游戏中的 秒，分钟，小时，天，月，年
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;//初始的季节是春天
    private int monthInSeason = 3;//3个月一个季节

    public bool gameClockPause;//控制游戏时间的暂停
    private float tikTime;//计时

    void Awake()
    {
        NewGameTime();//游戏一开始的时候初始化一下（临时测试一下）
    }

    private void Start()
    {

        
         EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
         EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameDay, gameSeason);
        
    }

   private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;//计时

            if (tikTime >= Settings.secondThreshold)//秒数大于阈值就可以变化了
            {
                tikTime -= Settings.secondThreshold;//减去阈值
                UpdateGameTime();
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }

        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     gameDay++;
        //     EventHandler.CallGameDayEvent(gameDay, gameSeason);
        //     EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        // }
    }

    private void NewGameTime()//用于初始化
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 1911;
        gameSeason = Season.春天;
    }

   private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 12)
                            gameMonth = 1;

                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;
                         //年可写可不写
                            if (gameYear > 9999)
                            {
                                gameYear = 1911;//调整年份
                            }
                        }
                        //用来刷新地图和农作物生长
                        //EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameDay, gameSeason);
            //切换灯光
            //EventHandler.CallLightShiftChangeEvent(gameSeason, GetCurrentLightShift(), timeDifference);
        }

         //Debug.Log("Second: " + gameSecond + " Minute: " + gameMinute);
    }
}
