using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// イベントクラス
/// 
/// </summary>
public class Event : MonoBehaviour {

    // メンバ変数
    private List<Speach>    m_SpeachList = new List<Speach>();  // セリフリスト

    private int             m_iCurrentSpeachIndex;              // 現在のセリフのインデックス
    private bool            m_bSkipIventFlg;                    // イベントスキップフラグ



	// Use this for initialization
	void Start () {
	
//        if ( null != m_SpeachList ) { m_SpeachList.Clear(); }

        m_iCurrentSpeachIndex = -1;
        m_bSkipIventFlg = false;
	}


	
	// Update is called once per frame
	void Update () {

        // セリフ発生の更新処理
        Speaking();
	}



    /// <summary>
    /// トリガー検知
    /// </summary>
    public void OnTriggerEnter( Collider col ) {

        // タグ別に処理
        string tag = col.transform.tag;
        switch ( tag ) {

            // プレイヤー
            case "Player":


                if ( -1 == m_iCurrentSpeachIndex ) {

                    // 現在のセリフのインデックスを最初に合わせる
                    m_iCurrentSpeachIndex = 0;




col.GetComponent<AnimatorController>().AnimationChange( 0 );
Pauser.Pause();
                }

                break;
        }
    }



    /// <summary>
    /// セリフの追加
    /// </summary>
    /// <param name="speach"></param>
    /// <returns></returns>
    public bool AddSpeach( Speach speach ) { 
    
        if ( null == m_SpeachList ) { return MyUtil.ErrorLog( "null値の変数です。" ); }
        if ( null == speach ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // セリフリストにセリフを追加する
        m_SpeachList.Add( speach );

        return true;
    }



    /// <summary>
    /// セリフ発生の更新処理
    /// </summary>
    private void Speaking() { 
    
        if ( null == m_SpeachList ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        if ( 0 > m_iCurrentSpeachIndex || m_SpeachList.Count <= m_iCurrentSpeachIndex ) { return; }

        Speach currentSpeach = m_SpeachList[ m_iCurrentSpeachIndex ];
        if ( null == currentSpeach ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        // ポーズ中なら処理しない
        if ( PauseScreen.IsPause() ) { return; }

        


        // セリフのトリガーON
        currentSpeach.TriggerOn();

        // セリフのトリガーOFF
        currentSpeach.TriggerOff();

        // イベントをスキップするキーが押された場合
        if ( Input.GetKeyDown( KeyCode.Tab ) ) {

            GameObject commentObj = currentSpeach.Comment;
            if ( !commentObj ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

            Comment comment = commentObj.GetComponent<Comment>();
            if ( !comment ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



            // 現在のコメントを破棄
            comment.DestroyComment();

            // イベントスキップフラグを立てる
            m_bSkipIventFlg = true;
        }


        // 現在のセリフが終了したら次のセリフに切り替える
        if ( currentSpeach.IsFinished ) {

            // イベントスキップフラグが立っていれば
            // 現在のセリフのインデックスをリストの要素数以上にして
            // イベントが終了する数値にする
            if ( m_bSkipIventFlg ) {  m_iCurrentSpeachIndex = m_SpeachList.Count; }

            // 現在のセリフのインデックスがセリフリストの要素数以上ならポーズ解除
            if ( m_SpeachList.Count <= ++m_iCurrentSpeachIndex ) {

Pauser.Resume();
            }
        }
    }
}
