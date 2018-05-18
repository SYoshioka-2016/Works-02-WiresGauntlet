using UnityEngine;
using System;
using System.Collections;



public class Watch : MonoBehaviour {

    // メンバ変数
    public UILabel[] label;
    private DateTime dateTime;



	// Use this for initialization
	void Start () {

	}


	
	// Update is called once per frame
	void Update () {

        dateTime = DateTime.Now;

        int hour = dateTime.Hour;
        label[0].text = ( hour / 10 ).ToString();
        label[1].text = ( hour % 10 ).ToString();

        int minute = dateTime.Minute;
        label[3].text = ( minute / 10 ).ToString();
        label[4].text = ( minute % 10 ).ToString();
	}
}
