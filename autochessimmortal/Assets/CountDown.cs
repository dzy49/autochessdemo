using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public int TotalTime =  15;
    float time = 15f;
    public board gameboard;
    bool started=false;
    public void StartCountDown()
    {
        TotalTime = 15;
        time = 15f;
        StartCoroutine(CountDownTimer());
        started = true;
    }
    void Update()
    {
        //时间减少
        if (started == true)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                gameboard.SendBattleListRequest();
                started = false;
            }
        }
    }
    IEnumerator CountDownTimer()
    {
       
        while (TotalTime >= 0)
        {
            this.GetComponent<Text>().text = TotalTime.ToString();
            yield return new WaitForSeconds(1);
            TotalTime--;

        }
        
        
    }

}
