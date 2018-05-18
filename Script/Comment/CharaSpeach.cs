using UnityEngine;
using System.Collections;



public class CharaSpeach : Comment {

	// メンバ変数


    [SerializeField] private UIWidget       enterKeySprite;             // Enterキーのスプライト
    [SerializeField] private GameObject     speakerNameObj;             // 発声者の名前のオブジェクト
    public UILabel                          speakerNameLabel;           // 発声者の名前のラベル

    private Vector3                         enterKeySpritePosition;     // Enterキーのスプライトの座標

    private bool                            finishExpandFlg;            // エクスパンド終了フラグ

    public  double          lifeTime = 2.0;         // 寿命
    private double          time;                   // 経過時間(秒)



	// Use this for initialization
	protected override void Start () {

        enterKeySpritePosition = Vector3.zero;

        finishExpandFlg = false;



        time = 0.0f;



        // スプライトの初期化
        InitializeSprite();



        // 発生時はスケールを0にしておく
        //
        // 最初の描画前にこの処理が必要。
        // スプライトの初期化で吹き出し画像の配置が終わってから
        // スケーリングする。
        transform.localScale = Vector3.zero;



        InitializeEffect();
	}
	


	// Update is called once per frame
	protected override void Update () {

        // エクスパンドが既に終了していればEnterキーのスプライトを有効にする
        if ( expand.IsFinished() && !finishExpandFlg ) {

            if ( enterKeySprite ) {

                // 座標を設定
                enterKeySprite.transform.localPosition = enterKeySpritePosition;

                // アクティブを有効にする
                enterKeySprite.enabled = true;

                // フラグを立てる
                finishExpandFlg = true;
            }
        }

        // キーが押されたらEnterキーのスプライトのアクティブを切る
        if ( Input.GetKeyDown( KeyCode.Return ) && finishExpandFlg ) {

            enterKeySprite.enabled = false;
        }



        // コントラクトエフェクト終了によるゲームオブジェクト破棄
        DestroyByFinishContractEffect();

        // コメントの自動破棄
        AutoDestroy();
	}



    /// <summary>
    /// カラーの初期化
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool InitializeColor( Color color ) {

        // スプライトが無ければ処理しない
        if ( null == sprites ) { return MyUtil.ErrorLog( "null値の変数です。" );}



        // スプライトのカラーを設定
        foreach ( var s in sprites ) {
        
            if ( null != color && s ) { s.color = color; }
            else return false;
        }



        return true;
    }



    /// <summary>
    /// スプライトの初期化
    /// </summary>
    /// <returns></returns>
    private bool InitializeSprite() { 
    
        if ( !label ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // 吹き出し画像スプライトの初期化
        InitialazeCommentFrame();



        return true;
    }



    /// <summary>
    /// 吹き出し画像スプライトの初期化
    /// </summary>
    /// <returns></returns>
    private bool InitialazeCommentFrame() {
 
        // ラベルが無ければ処理しない
        if ( !label ) {
            
            return MyUtil.ErrorLog( "null値の変数です。" );
        }



        // スプライトピクセルサイズテーブル
        int fontSize = label.bitmapFont.defaultSize;
        Vector2 centerBoardDimensions   = new Vector2( label.localSize.x, label.localSize.y );  // 中央の矩形
        Vector2 horizonBarDimensions    = new Vector2( centerBoardDimensions.x, fontSize );     // 横長の上下の矩形
        Vector2 varticalBarDimensions   = new Vector2( fontSize, centerBoardDimensions.y );     // 縦長の左右の矩形
        Vector2 cornerDimensions        = new Vector2( fontSize, fontSize );                    // 四隅の矩形
        Vector2[] dimensionsTable = 
        {
            centerBoardDimensions,          // 中央

            horizonBarDimensions,           // 上
            horizonBarDimensions,           // 下
            varticalBarDimensions,          // 右
            varticalBarDimensions,          // 左

            cornerDimensions,               // 右上
            cornerDimensions,               // 右下
            cornerDimensions,               // 左上
            cornerDimensions,               // 左下
        };

        // 座標テーブル
        //
        // 中央をピボットとする。
        float units = 360.0f;
        Vector3[] positionTable = 
        {
            label.transform.position,                                                                                         // 中央

            label.transform.position + ( new Vector3( 0,  horizonBarDimensions.y )  + new Vector3( 0,  centerBoardDimensions.y ) ) / units / 2,     // 上
            label.transform.position + ( new Vector3( 0, -horizonBarDimensions.y )  + new Vector3( 0, -centerBoardDimensions.y ) ) / units / 2,     // 下
            label.transform.position + ( new Vector3(  varticalBarDimensions.x, 0 ) + new Vector3(  centerBoardDimensions.x, 0 ) ) / units / 2,     // 右
            label.transform.position + ( new Vector3( -varticalBarDimensions.x, 0 ) + new Vector3( -centerBoardDimensions.x, 0 ) ) / units / 2,     // 左
            
            label.transform.position + ( new Vector3(  cornerDimensions.x,  cornerDimensions.y ) + new Vector3(  centerBoardDimensions.x,  centerBoardDimensions.y ) ) / units / 2,     // 右上
            label.transform.position + ( new Vector3(  cornerDimensions.x, -cornerDimensions.y ) + new Vector3(  centerBoardDimensions.x, -centerBoardDimensions.y ) ) / units / 2,     // 右下
            label.transform.position + ( new Vector3( -cornerDimensions.x,  cornerDimensions.y ) + new Vector3( -centerBoardDimensions.x,  centerBoardDimensions.y ) ) / units / 2,     // 左上
            label.transform.position + ( new Vector3( -cornerDimensions.x, -cornerDimensions.y ) + new Vector3( -centerBoardDimensions.x, -centerBoardDimensions.y ) ) / units / 2,     // 左下
        };



        // Enterキーのスプライトの座標を設定
        enterKeySpritePosition = 
            label.transform.position + 
            ( new Vector3( cornerDimensions.x, -cornerDimensions.y ) + new Vector3( centerBoardDimensions.x, -centerBoardDimensions.y ) ) / 2 + 
            new Vector3( -100, 0 ) - 
            NGUITools.GetRoot( this.gameObject ).transform.position;



        // スプライトの設定
        for ( int i = 0; i < sprites.Length; i++ ) {

            // スプライトが無ければ処理しない
            if ( !sprites[i] ) {
            
                return MyUtil.ErrorLog( "null値の変数です。" );
            }



            // スプライトサイズと座標を設定
            sprites[i].width                = (int)dimensionsTable[i].x;
            sprites[i].height               = (int)dimensionsTable[i].y;
            sprites[i].transform.position   = positionTable[i];
        }



        // 発声者の名前のオブジェクトの座標を設定
        if ( speakerNameObj ) {

            speakerNameObj.transform.localPosition = 
                label.transform.position + 
                ( new Vector3( -cornerDimensions.x,  cornerDimensions.y ) + new Vector3( -centerBoardDimensions.x,  centerBoardDimensions.y ) ) / 2 + 
                new Vector3( 20, 40 ) - 
                NGUITools.GetRoot( this.gameObject ).transform.position;
        }



        return true;
    }



    /// <summary>
    /// コメントの自動破棄
    /// </summary>
    private void AutoDestroy() { 
    
        // 寿命が負数なら処理しない
        if ( 0 > lifeTime ) { return; }

        // エクスパンドエフェクトが終わってないなら処理しない
        if ( expand && !expand.IsFinished() ) { return; }



        // 経過時間を更新
        time += Time.deltaTime;

        // 経過時間が寿命を超えたら破棄
        if ( lifeTime < time ) {

            // コメントの破棄
            DestroyComment();
        }
    }
}
