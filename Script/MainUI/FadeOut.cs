using UnityEngine;
using System.Collections;



public class FadeOut : MonoBehaviour {

    // メンバ変数
    public  UISprite        uiSprite;
    public  double          fadeTime;

    private int             timeCounter;
    private float           tmpAlpha;



	// Use this for initialization
	void Start () {

        timeCounter = 0;
	}
	


	// Update is called once per frame
	void Update () {

        if ( !uiSprite ) return;

        if ( timeCounter >= fadeTime * FPS.fps ) {
            
            Destroy( gameObject );
            return;
        }



        timeCounter += 1;



        Color color = uiSprite.color;
        color.a     = ( 0 >= color.a ) ? tmpAlpha : color.a;



        // 点滅無し
        color.a -= (float)( 255 / (fadeTime * FPS.fps * 255) );
        tmpAlpha = color.a;
/*
        // 点滅有り
        if ( 2 >= timeCounter % 4 ) {

            color.a -= (float)( 255 / (fadeTime * FPS.fps * 255) );
            tmpAlpha = color.a;
        }
        else {

            tmpAlpha -= (float)( 255 / (fadeTime * FPS.fps * 255) );
            color.a = 0;
        }
*/


        uiSprite.color = color;
	}
}
