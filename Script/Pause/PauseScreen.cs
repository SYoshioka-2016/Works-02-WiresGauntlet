using UnityEngine;
using System.Collections;



/// <summary>
/// ポーズ画面クラス
/// </summary>
public class PauseScreen : MonoBehaviour {

    // メンバ変数
    [SerializeField] private UISprite       pauseBackSprite;        // 背景スプライト
    [SerializeField] private UISprite       pauseLogoSprite;        // ロゴスプライト
    [SerializeField] private UISprite       pauseOperationSprite;   // 操作説明スプライト
    [SerializeField] private UISprite[]     pauseMenuSpriteArray;   // メニュースプライト配列
	public GameObject coin;

    private static bool                     isPause;                // ポーズ中か?( true : ポーズしている | false : ポーズしてない )
    private static bool                     wasPause;               // 前回ポーズしていたか?( true : ポーズしていた | false : ポーズしてなかった )
    private int                             currentMenuIndex;       // 現在のメニューインデックス
    private float                           prevTimeScale;          // 前回のタイムスケール



    /// <summary>
    /// ポーズ中(前回ポーズしていた)か?
    /// </summary>
    /// <returns></returns>
    public static bool IsPause() { return wasPause; }



	// Use this for initialization
	void Start () {

        // ウィジェットを無効にする
        InitializeSpriteEnable( false );



        isPause             = false;
        wasPause            = false;
        currentMenuIndex    = 0;
        prevTimeScale       = 0;
	}
	


	// Update is called once per frame
	void Update () {

        // ポーズのフラグを保存
        wasPause = isPause;



        // ポーズ処理
        Pause();
        
        // ポーズメニューの実行
        ExecutePauseMenu();

        // 現在のメニューインデックスの切り替え
        SwitchCurrentMenuIndex();
	}



    /// <summary>
    /// ウィジェットのアクティブ設定
    /// </summary>
    private void InitializeSpriteEnable( bool enable ) { 
    
        // 各種ウィジェットのアクティブを設定する

        if ( pauseBackSprite )      { pauseBackSprite.enabled       = enable; }
        if ( pauseLogoSprite )      { pauseLogoSprite.enabled       = enable; }
        if ( pauseOperationSprite ) { pauseOperationSprite.enabled  = enable; }

        if ( null != pauseMenuSpriteArray ) {

            for ( int i = 0; i < pauseMenuSpriteArray.Length; i++ ) {
        
                if ( pauseMenuSpriteArray[i] ) { pauseMenuSpriteArray[i].enabled = enable; }
            }
        }
    }



    /// <summary>
    /// 現在のメニューインデックスの切り替え
    /// </summary>
    private void SwitchCurrentMenuIndex() { 
    
        // ポーズ中でなければ処理しない
        if ( !isPause ) { return; }



        // 現在のメニューインデックスを保存
        int prevIndex = currentMenuIndex;



        // 現在のメニューインデックスを前方に戻す
        if ( Input.GetKeyDown(KeyCode.A) ) { currentMenuIndex--; }

        // 現在のメニューインデックスを後方に進める
        if ( Input.GetKeyDown(KeyCode.D) ) { currentMenuIndex++; }

        // 現在のメニューインデックスを調整
        currentMenuIndex = Mathf.Clamp( currentMenuIndex, 0, pauseMenuSpriteArray.Length - 1 );



        // メニューインデックスが変化したら(一致しなかったら)メニュースプライトの初期化
        if ( !prevIndex.Equals(currentMenuIndex) ) { InitializePauseMenuSpriteByCurrentIndex(); }
    }



    /// <summary>
    /// ポーズ処理
    /// </summary>
    private void Pause() { 
    
        // ポーズするキーが押されたらポーズ
        if ( Input.GetKeyDown(KeyCode.Escape) ) {

            // ポーズしてなければポーズする
            if ( !isPause ) {

				coin.SetActive(true);

                // ポーズ中にする
                isPause = true;

                // ウィジェットを有効にする
                InitializeSpriteEnable( true );

                // 現在のメニューインデックスを初期化
                currentMenuIndex = 0;

                // メニュースプライトの初期化
                InitializePauseMenuSpriteByCurrentIndex();

                // ポーズ直前のタイムスケールを保存
                prevTimeScale   = Time.timeScale;

                // タイムスケールを0にする
                Time.timeScale  = 0;
            }
        }
    }



    /// <summary>
    /// ポーズから再生
    /// </summary>
    private void Play() {

        // ポーズをやめる
        isPause = false;

		coin.SetActive(false);

        // ウィジェットを無効にする
        InitializeSpriteEnable( false );

        // タイムスケールをポーズ直前に戻す
        Time.timeScale = prevTimeScale;
    }



    /// <summary>
    /// メニュースプライトの初期化
    /// </summary>
    private void InitializePauseMenuSpriteByCurrentIndex() {

        // ポーズ中でなければ処理しない
        if ( !isPause ) { return; }

        if ( null == pauseMenuSpriteArray ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



        // 非選択メニューのカラー
        float colorElement = ( 1.0f / 255 ) * 70;
        Color grayColor = new Color( colorElement, colorElement, colorElement, 1.0f );

        // メニュースプライトのカラー設定
        for ( int i = 0; i < pauseMenuSpriteArray.Length; i++ ) {

            // 現在のメニューインデックスでなければ非選択メニューのカラーに設定
            if ( !currentMenuIndex.Equals(i) ) { 

                pauseMenuSpriteArray[i].color = grayColor; 
            }

            // 現在のメニューインデックスなら選択メニューのカラーに設定
            else { 
                
                pauseMenuSpriteArray[i].color = Color.white; 
            }
        }
    }



    /// <summary>
    /// ポーズメニューの実行
    /// </summary>
    private void ExecutePauseMenu() {

        // ポーズ中でなければ処理しない
        if ( !isPause ) { return; }

        // ポーズメニューを実行するキーが押されなければ処理しない
        if ( !Input.GetKeyDown(KeyCode.Return) ) { return; }
    
        UISprite pauseMenuSprite = pauseMenuSpriteArray[ currentMenuIndex ];
        if ( !pauseMenuSprite ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        

        // ポーズメニュースプライトの名前別に処理
        string name = pauseMenuSprite.name;
        switch ( name ) {

            case "Pause Return To Game Text Sprite": 
                
                // ポーズから再生
                Play(); 
                break;



            case "Pause Title Text Sprite":

                // ポーズから再生
                Play();

                // タイトルシーンに切り替える
				FadeScript.fadeScript.FadeStart( "Title" );
        //        Application.LoadLevel( "Title" ); 

                break;



            case "Pause Restart Text Sprite":

                // ポーズから再生
                Play();

                // 現在のシーン名を取得
                string currentSceneName = Application.loadedLevelName;

                // 現在のシーンを再読み込みする
				FadeScript.fadeScript.FadeStart( currentSceneName );

                break;



            // 該当無し
            default: 
                
                // ポーズから再生
                Play(); 
                break;
        }
    }
}
