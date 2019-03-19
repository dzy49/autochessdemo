using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public int TotalTime = 60;
    public board gameboard;
    public void StartCountDown()
    {

        StartCoroutine(CountDownTimer());

    }

    IEnumerator CountDownTimer()
    {
        while (TotalTime >= 0)
        {
            this.GetComponent<Text>().text = TotalTime.ToString();
            yield return new WaitForSeconds(1);
            TotalTime--;

        }
        if (TotalTime == 0)
        {
            gameboard.SendBattleListRequest();
        }
        
    }

}
