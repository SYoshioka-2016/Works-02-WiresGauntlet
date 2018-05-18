using UnityEngine;
using System.Collections;



/// <summary>
/// ウィジェットカラー点滅クラス
/// </summary>
public class FlashingWidget : MonoBehaviour {

    // メンバ変数
    [SerializeField] private float      TIME_INTERVAL               = 1.0f;     // アルファ値折り返しの時間間隔(秒)
    [SerializeField] private bool       playFlash                   = true;     // 点滅再生フラグ

    private UIWidget                    m_widget;                               // ウィジェット
    private float                       m_fCurrentAlpha;                        // アルファ値
    private bool                        m_bTurnCurrentAlphaFlg;                 // アルファ値の折り返しフラグ



	// Use this for initialization
	void Start () {

        TIME_INTERVAL           = Mathf.Abs( TIME_INTERVAL );

        if ( !m_widget ) { m_widget = GetComponent<UIWidget>(); }



        SetWidgetColorAlpha( 0 );

        m_bTurnCurrentAlphaFlg = false;
	}


	
	// Update is called once per frame
	void Update () {
	
        // 点滅
        Flashing();
	}



    public void PlayFlash( Color color ) {

        playFlash = true;

        SetWidgetColor( color );
    }



    public void StopFlash() {

        playFlash = false;
    }



    /// <summary>
    /// 点滅
    /// </summary>
    private void Flashing() { 

        if ( !m_widget ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( null == m_widget.color ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



if ( !playFlash ) { return; }
    


        // 点滅
        if ( !m_bTurnCurrentAlphaFlg ) {

            // アルファ値を増加
            m_fCurrentAlpha += Time.deltaTime / TIME_INTERVAL;

            // アルファ値が最大以上ならアルファ値の折り返しフラグを立てる
            if ( 1.0f <= m_fCurrentAlpha ) { m_bTurnCurrentAlphaFlg = true; }
        }
        else {

            // アルファ値を減少
            m_fCurrentAlpha -= Time.deltaTime / TIME_INTERVAL;

            // アルファ値が最小以下ならアルファ値の折り返しフラグを降ろす
            if ( 0.0f >= m_fCurrentAlpha ) { m_bTurnCurrentAlphaFlg = false; }
        }

        // ウィジェットカラーのアルファ値を変更
        SetWidgetColorAlpha( m_fCurrentAlpha );
    }



    /// <summary>
    /// ウィジェットカラーの設定
    /// </summary>
    /// <param name="color"></param>
    private void SetWidgetColor( Color color ) { 
    
        if ( null == color ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( !m_widget ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( null == m_widget.color ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



        // ウィジェットのカラーを設定
        m_widget.color  = color;

        // 現在のアルファ値を取得
        m_fCurrentAlpha = m_widget.color.a;
    }



    /// <summary>
    /// ウィジェットカラーのアルファ値の設定
    /// </summary>
    /// <param name="alpha"></param>
    private void SetWidgetColorAlpha( float alpha ) { 
    
        // アルファ値を調整
        alpha = Mathf.Clamp( alpha, 0.0f, 1.0f );

        // ウィジェットのカラーを設定
        Color color     = m_widget.color;
        color.a         = alpha;
        SetWidgetColor( color );
    }
}
