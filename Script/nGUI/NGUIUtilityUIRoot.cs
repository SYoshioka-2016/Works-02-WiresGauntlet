/*using UnityEngine;
using System.Collections;



public class NGUIUtilityUIRoot : MonoBehaviour {

    // メンバ変数
    private int Width = Screen.width;
    private int Height = Screen.height;

    UIRoot uiRoot;



	void Awake () {

        uiRoot = GetComponent<UIRoot>();
	}


	
	void Start () {
Debug.Log( Screen.width + " " + Screen.height );
        if ( !uiRoot || 0 >= Width || 0 >= Height ) return;



        int h = Height;
        float r = (float)( Screen.height * Width ) / ( Screen.width * Height );
        if ( r > 1.0f ) { h = (int)( h * r ); }

        if ( uiRoot.scalingStyle != UIRoot.Scaling.FixedSize ) { uiRoot.scalingStyle = UIRoot.Scaling.FixedSize; }
        if ( uiRoot.manualHeight != h ) { uiRoot.manualHeight = h; }
        if ( uiRoot.minimumHeight > 1 ) { uiRoot.minimumHeight = 1; }
        if ( uiRoot.maximumHeight < System.Int32.MaxValue ) { uiRoot.maximumHeight = System.Int32.MaxValue; }
	}
}
*/