using UnityEngine;
using System.Collections;



public class TitleLogo : MonoBehaviour {

	// メンバ変数
    [SerializeField]
    private float       fadeTime = 1.0f;    // フェード時間

    [SerializeField]
    private float       showTime = 3.0f;    // 表示時間

    private double      time;               // タイマ
    private bool        switchFlg;          // フェード切り替えフラグ

    private UISprite    sprite;             // スプライト
    private Color       color;              // カラー



	// Use this for initialization
	void Start () {

        sprite = GetComponent<UISprite>();

        // フェード時間を調整
        fadeTime = Mathf.Clamp( fadeTime, 0, 1000 );

        // タイマを初期化
        time = 0.0;

        // スプライトのアルファ値を0にする
        color = Color.white;
        color.a = 0;
        if ( sprite ) { sprite.color = color; }
	}
	


	// Update is called once per frame
	void Update () {

        if ( !sprite ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



        if ( IsFinished() ) {

            GameObject root = GameObject.Find( "UI Root" );
            if ( root ) {
        
                Transform[] children = new Transform[ root.transform.childCount ];
                for ( int i = 0; i < children.Length; i++ ) { 

                    children[i] = root.transform.GetChild(i); 
                }
                foreach ( var c in children ) { if ( c ) c.gameObject.SetActive( true ); }
            }

            Destroy( gameObject );
        }





        // フェード切り替えフラグが立っていなくて、
        // スプライトのアルファ値が最大以上ならタイマをカウントアップする
        //
        // アルファ値の増加が終了したら一定期間表示し、
        // 時間が経ったらアルファ値の減少に切り替える。
        if ( !switchFlg && 1.0f <= sprite.color.a ) {

            // タイムカウント
            time += Time.deltaTime;

            // 一定時間経過したらフラグOFF
            if ( showTime <= time ) { 

                color.a = Mathf.Clamp( color.a, 0.0f, 1.0f );
                switchFlg = true; 
            }



            return;
        }



        // スプライトのアルファ値を加算する
        float addAlphaValue =  (float)( 1.0f / (fadeTime * FPS.fps * 1.0f) );
        color.a             += ( !switchFlg ) ? addAlphaValue : -addAlphaValue;
        sprite.color        =  color;
	}



    public bool IsFinished() { return switchFlg && sprite && 0.0f >= sprite.color.a; }
}
