using UnityEngine;
using System.Collections;



public class MenuExplanation : MonoBehaviour {

    // メンバ変数
    public  UISprite            backSprite;     // 背景スプライト
    public  UISprite            textSprite;     // テキストスプライト

    public  GearScript          gearScript;     // 

    private bool                switchFlg;      // メニュー切り替えフラグ

    [SerializeField]
    private GearScript.Mode     mode;           // 現在選択しているメニュー
    private GearScript.Mode     nextMode;       // 次に変更するメニュー

    private Expand              expand;         // エクスパンドエフェクト
    private Contract            contract;       // コントラクトエフェクト



	// Use this for initialization
	void Start () {

        // メニューの初期設定
        mode        = GearScript.Mode.Credit;
        nextMode    = GearScript.Mode.Credit;



        // エクスパンドエフェクトを取得
        expand = GetComponent<Expand>();

        // コントラクトエフェクトを取得
        contract = GetComponent<Contract>();



        // フェードイン
        FadeIn();
	}
	


	// Update is called once per frame
	void Update () {
	
        // 次に変更するメニューを取得
        if ( gearScript ) { nextMode = gearScript.GetMode(); }



        // メニュー変更
        ChangeMode();


/*
if ( gearScript && !gearScript.IsPauseRotation() && Input.GetKeyDown(KeyCode.Space) ) {

            switch ( mode ) {

                case GearScript.Mode.GameStart :

                    Application.LoadLevel( "ExplodeBarrel" ); break;
            }
        }*/
	}



    /// <summary>
    /// フェードイン
    /// </summary>
    private bool FadeIn() { 
    
        if ( !expand )   { return MyUtil.ErrorLog( "null値の変数です。" ); }
        if ( !contract ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // エクスパンドエフェクトを開始する
        if ( !expand.enabled ) { 

            // コントラクトエフェクトを無効にする
            contract.enabled    = false;

            // エクスパンドエフェクトを有効にする
            expand.enabled      = true;

            // エクスパンドエフェクトを設定
            expand.startScale   = new Vector3( 1, transform.localScale.y, transform.localScale.z );
            expand.Initialize();
        }



        // エフェクトの終了情報を返す
        //
        // true : 終了 | false : 終了してない(処理しない)
        return expand.IsFinished();
    }



    /// <summary>
    /// フェードアウト
    /// </summary>
    private bool FadeOut() { 
    
        if ( !expand )   { return MyUtil.ErrorLog( "null値の変数です。" ); }
        if ( !contract ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // コントラクトエフェクトを開始する
        if ( !contract.enabled ) {

            // エクスパンドエフェクトを無効にする
            expand.enabled      = false;

            // コントラクトエフェクトを有効にする
            contract.enabled    = true;

            // コントラクトエフェクトを設定
            contract.startScale = new Vector3( 1, transform.localScale.y, transform.localScale.z );
            contract.Initialize();
        }



        // エフェクトの終了情報を返す
        //
        // true : 終了 | false : 終了してない(処理しない)
        return contract.IsFinished();
    }



    /// <summary>
    /// メニュー変更
    /// </summary>
    private void ChangeMode() { 

        if ( !backSprite ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( !textSprite ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        // 現在と次のメニューが同じなら処理しない
        if ( mode == nextMode ) { return; }



        // メニュー切り替えフラグがOFFならフェードアウト
        //
        // フェードアウト中はメニュー変更の処理はしない。
        // フェードアウトが終了したらフラグON。
        if ( !switchFlg ) { switchFlg = FadeOut(); return; }

        // メニュー切り替えフラグがONならフェードイン
        //
        // フェードインの直後に現在のメニューを変更してフラグON。
        else { 

            FadeIn();
            mode        = nextMode;
            switchFlg   = false;
        }


        
        // 現在のメニュー別に処理
        switch ( mode ) {
        
            // クレジット
            case GearScript.Mode.Credit:

                backSprite.spriteName = "title_credit_frame";
                textSprite.spriteName = "title_text_credit_frame";
                break;

            // ゲームスタート
            case GearScript.Mode.GameStart:

//                backSprite.spriteName = "title_gamestart_frame";
                backSprite.spriteName = "title_text_frame";
                textSprite.spriteName = "title_text_gamestart_frame";
                break;
        }
    }
}
