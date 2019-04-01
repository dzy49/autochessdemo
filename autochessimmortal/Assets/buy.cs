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
        if (this.gameObject.GetComponent<Text>().text == "warrior"&&GetGold()>0)
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
            a.GetComponent<Minons>().state = Minons.States.wait;
            a.GetComponent<Minons>().player = board.id;
            a.name = board.count.ToString();
            Instantiate(a, new Vector3(i * 2.0F,-5, 0), Quaternion.identity);
          
            print("minons ID:" + board.id);
            a.transform.localPosition = new Vector2(i,-5);
            board.count++;

            gb.gold -= 1;

            this.gameObject.GetComponent<Text>().text = "";


        }
    }

    public int GetGold()
    {
        return gb.gold;
    }
}
