using UnityEngine;
using System;
using System.Collections;



/// <summary>
/// セリフクラス
/// </summary>
public class Speach {

    // セリフのテキストデータの形式について
    //
    // ex. "1000敵はすぐそこまで追ってきている。"
    // 左から
    // ① 1桁目 … 発声者タイプ識別子。誰のコメントか識別する。
    // ② 2桁目 … 発生タイプ識別子。どの様に発生するのか識別する。
    // ③ 3桁目 … 発生条件識別子。どのコメントの発生条件か識別する。(イベント番号)
    // ④ 4桁目 … 消滅条件識別子。どのコメントの消滅条件か識別する。
    //
    // ① 0 … プレイヤー、1 … 指令キャラ、2 … 敵、3 … システムメッセージ
    // ② 0 … トリガー検知で発生、1 … コメントに続けて発生
    // ③ トリガー検知で発生してから次のトリガー検知の直前までのコメント群を1単位とするイベント番号
    // ④ CommentConditionsList_BaseのCheckTriggerOff関数で指定する消滅条件配列のインデックス(システムメッセージのみ適用、キャラのセリフは常に0番とし、消滅条件の判定は行わない)
    //
    // ～変更～
    //
    // ex. "10敵はすぐそこまで追ってきている。"
    // 左から
    // ① 1桁目 … 発声者タイプ識別子。誰のコメントか識別する。
    // ② 2桁目 … イベント番号。どのイベントに属しているか識別する。順番は読み込み順とする。
    //
    // ① 0 … プレイヤー、1 … 指令キャラ、2 … 敵、3 … システムメッセージ
    // ② トリガー検知で発生してから後続のコメントが終了するまでのコメント群を1単位とするイベント番号。


    /// <summary>
    /// 発声者タイプ列挙
    /// </summary>
    public enum SPEAKER_TYPE {

        PLAYER,                 // プレイヤー
        COMMANDER,              // 指令キャラ
        ENEMY,                  // 敵
        SYSTEM,                 // システム

        UNKNWON_SPEAKER_TYPE,
    }



    /// <summary>
    /// コメントタイプ列挙
    /// </summary>
    public enum COMMENT_TYPE {

        CHARA_SPEACH,           // キャラのセリフ
        SYSTEM_MESSAYGE,        // システムメッセージ
    }



    /// <summary>
    /// 発生タイプ列挙
    /// </summary>
    public enum BORN_TYPE {

        IVENT_START,            // イベントのトリガーONで発生
        AFTER_THE_COMMENT,      // 1つ前のコメントの後に続けて発生
    }



    // メンバ変数
    private string                      speakerName;                // 発声者名

    private string                      text;                       // コメントのテキストデータ
    private double                      lifeTime;                   // コメントの寿命

    private GameObject                  comment;                    // コメント

    private bool                        triggerFlg;                 // 発生トリガー
    private bool                        isFinished;                 // コメントが破棄されて終了したか?(true : 終了した | false : 終了してない)
    private double                      differentialTimeCounter;    // 発生遅延時間カウントタイマ

    private SPEAKER_TYPE                speakerType;                // 発声者タイプ
    private COMMENT_TYPE                commentType;                // コメントタイプ

    private int                         iventNumber;                // イベント番号

    private readonly Vector2[]          showPositionTable =         // 発声者タイプ別のコメント表示位置
    {
        new Vector2( -300, 150 ),   // プレイヤー 
        new Vector2( -300, 250 ),   // 指令キャラ
        new Vector2( 300, 150 ),    // 敵
        new Vector2( 0, 0 ),        // システム
    };


    
    public bool TriggerFlg { get { return this.triggerFlg; } }
    public bool IsFinished { get { return this.isFinished && !comment; } }
    public GameObject Comment { get { return this.comment; } }

    public int IventNumber { get { return this.iventNumber; } }



    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Speach( 
        string text // セリフのテキストデータ
    ) {
        // セリフのテキストデータ
        this.text = text;

        // 発声者タイプの読み込み
        this.speakerType        = (SPEAKER_TYPE)ReadType( typeof(SPEAKER_TYPE) );

        // コメントタイプの設定
        switch ( this.speakerType ) {

            case SPEAKER_TYPE.SYSTEM: 
                this.commentType = COMMENT_TYPE.SYSTEM_MESSAYGE; break;

            default: 
                this.commentType = COMMENT_TYPE.CHARA_SPEACH; break;
        }

        // 発声者名の設定
        switch ( this.speakerType ) {

            case SPEAKER_TYPE.PLAYER:       this.speakerName = "プレイヤー"; break;
            case SPEAKER_TYPE.COMMANDER:    this.speakerName = "指令キャラ"; break;
            case SPEAKER_TYPE.ENEMY:        this.speakerName = "敵";         break;

            default: this.speakerName = ""; break;
        }

        // イベント番号の読み込み
        this.iventNumber = ReadNumber();

        // 寿命の設定
        //
        // キャラのセリフは数秒で自動消滅、システムメッセージは無制限。
//        if ( (int)SPEAKER_TYPE.SYSTEM > (int)this.speakerType ) this.lifeTime = 2.0f;
//        else this.lifeTime = -1;
        this.lifeTime = -1;



        triggerFlg              = false;
        isFinished              = false;
        differentialTimeCounter = 0;
    }



    /// <summary>
    /// トリガーON
    /// 
    /// コメントを生成する。
    /// </summary>
    public bool TriggerOn() {

        // 既に発生していたら処理しない
        if ( triggerFlg ) {

            // コメントの存在で終了を判定する
            isFinished = !comment;
            return false;
        }



 /*       
GameObject player =  GameObject.Find( "PlayerModel" );
player.GetComponent<AnimatorController>().AnimationChange( 0 );
Pauser.Pause();
*/



        // 設定した発生遅延時間がまだ経過していなければ処理しない
        const double differentialTime = 0.5;
        if ( UpdateDifferentialTimeCounter() <= differentialTime ) return false;



        // フラグON
        triggerFlg = true;



        // コメントタイプ別にコメントの生成
        switch ( commentType ) {

            


            // キャラのセリフ
            case COMMENT_TYPE.CHARA_SPEACH:

                comment = 
                    CommentCreator.Instance.CreateComment( 
                        showPositionTable[ (int)speakerType ], 
                        speakerName,
                        text, 
                        Color.white,
                        lifeTime 
                    );
                break;



            // システムメッセージ
            case COMMENT_TYPE.SYSTEM_MESSAYGE:

                comment = 
                    CommentCreator.Instance.CreateSystemMessage( 
                        text
                    );
                break;
        }



        return true;
    }



    /// <summary>
    /// トリガーOFF
    /// 
    /// コメントを破棄する。
    /// </summary>
    public void TriggerOff() {

        // まだ発生していない、又はすでに消滅しているなら処理しない
        if ( !triggerFlg || isFinished ) { return; }

        // システムメッセージでないなら処理しない
//        if ( SPEAKER_TYPE.SYSTEM != speakerType ) { return; }

        // コメントが無ければ処理しない
        if ( !comment ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        // ポーズ中なら処理しない
        if ( PauseScreen.IsPause() ) { return; }



        // 条件を達成したコメント(システムメッセージ)は消滅
        //
        // キャラのセリフは自動消滅。
//        if ( commentConditionsList && commentConditionsList.CheckTriggerOff(checkTriggerOffNumber) ) { 
if ( Input.GetKeyDown(KeyCode.Return) ) { 
                
            // コメントの破棄
            CommentCreator.Instance.DestroyComment( comment.name );
        }
    }



    /// <summary>
    /// 発生遅延時間カウントタイマの更新
    /// </summary>
    private double UpdateDifferentialTimeCounter() {

        differentialTimeCounter += Time.deltaTime;

        return differentialTimeCounter; 
    }



    /// <summary>
    /// テキストデータからタイプの読み込み
    /// </summary>
    /// <returns></returns>
    private Enum ReadType( Type enumType ) {

        // セリフのテキストデータの先頭の数字を数値に変換
        double d = 0;
        if ( double.TryParse( this.text.Substring(0, 1), out d ) ) {
        
            // 数値を調整
            d = 
                Mathf.Clamp( 
                    (int)d, 
                    0, 
                    Enum.GetNames( enumType ).Length - 1
                );

            // セリフのテキストデータから先頭の文字を削除
            this.text          = this.text.Remove( 0, 1 );



            // 数値に対応するタイプの列挙子を返す
            return (Enum)Enum.ToObject( enumType, (int)d );
        }



        MyUtil.ErrorLog( "データを正しく読み込めませんでした。" );



        return null;
    }



    /// <summary>
    /// テキストデータから数値の読み込み
    /// </summary>
    /// <returns></returns>
    private int ReadNumber() {

        // セリフのテキストデータの先頭の数字を数値に変換
        double d = 0;
        if ( double.TryParse( this.text.Substring(0, 1), out d ) ) {
        
            // 数値を調整
            d = 
                Mathf.Clamp( 
                    (int)d, 
                    0, 
                    9
                );

            // セリフのテキストデータから先頭の文字を削除
            this.text          = this.text.Remove( 0, 1 );



            // 数値を返す
            return (int)d;
        }



        MyUtil.ErrorLog( "データを正しく読み込めませんでした。" );



        return -1;
    }
}
