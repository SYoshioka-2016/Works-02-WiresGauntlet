using UnityEngine;
using System.Collections;


//http://westhillapps.blog.jp/archives/38436612.html
public class RenderFrontThanNGUI : MonoBehaviour {

    // メンバ変数
    [SerializeField]
    private int rQueue = 4000;



	// Use this for initialization
	void Start () {

        GetComponent<Renderer>().material.renderQueue = rQueue;

        Transform trans = gameObject.transform;
        for ( int i = 0; i < trans.childCount; i++ ) {

            trans.GetChild(i).gameObject.GetComponent<Renderer>().material.renderQueue = rQueue;
        }
	}
}
