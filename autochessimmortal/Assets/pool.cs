using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pool : MonoBehaviour {
    public board gb;
    public List<int> minonPool;
    public GameObject text1;
    public GameObject text2;
    // Use this for initialization
    void Awake()
    {
        gb = GameObject.Find("mboard").GetComponent<board>();
        text1 = GameObject.Find("Canvas/Text1");
    }
    void Start () {
        minonPool = new List<int>();
        minonPool.Add(1);
        minonPool.Add(1);
        minonPool.Add(1);
        minonPool.Add(1);
        minonPool.Add(2);
        minonPool.Add(2);
        minonPool.Add(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Refreshstore()
    {
        minonPool.AddRange(gb.storepool);
        gb.storepool = new int[5];
        for (int i = 0; i < 5; i++)
        {
            Draw(i);
        }

        for (int j = 0; j < 5;j++) {
            string textname = "Canvas/Panel/Text" + j.ToString();
            if (gb.storepool[j] == 1)
            {
            GameObject.Find(textname).GetComponent<Text>().text = "warrior";
            }
            else
            {
                GameObject.Find(textname).GetComponent<Text>().text = "OTHER";

            }
        }

    }

    void Draw(int index)
    {
        int randindex = Random.Range(0, minonPool.Count);
        gb.storepool[index]=minonPool[randindex];
        minonPool.RemoveAt(randindex);
    }
}
