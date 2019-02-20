using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buy : MonoBehaviour {
    public board gb;

    void Awake()
    {
        gb = GameObject.Find("mboard").GetComponent<board>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void makePurchase()
    {
        int i;
        if (this.gameObject.GetComponent<Text>().text == "warrior")
        {
            for (i=0; i < 5; i++)
            {
                if (gb.waitpool[i] == -1)
                {
                    gb.waitpool[i] = 1;
                    break;
                }
            }
            print(this.gameObject.name[4]);
            gb.storepool[Int32.Parse(this.gameObject.name[4].ToString())] = -1;
            GameObject a = (Resources.Load("warrior") as GameObject);
            Instantiate(a, new Vector3(i * 2.0F,11, 0), Quaternion.identity);
            a.GetComponent<minons>().state = minons.States.wait;
            a.transform.localPosition = new Vector2(i,-5);
            board.count++;
            a.name = board.count.ToString();
            GameObject.Find("GoldText").GetComponent<Text>().text = (Int32.Parse(GameObject.Find("GoldText").GetComponent<Text>().text) - 1).ToString();


            this.gameObject.GetComponent<Text>().text = "";

        }
    }
}
