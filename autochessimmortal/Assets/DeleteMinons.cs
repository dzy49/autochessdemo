using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMinons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool overSprite = false;
        Vector2 mousePosition1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        overSprite = GetComponent<BoxCollider2D>().bounds.Contains(mousePosition1);
        if (overSprite)
        {
            //If we've pressed down on the mouse (or touched on the iphone)
            if (Input.GetButton("Fire1"))
            {
                removeMinons();
            }
        }
    }

    public void removeMinons()
    {
        print("test11");
        Destroy(transform.parent.gameObject);
    }
}
