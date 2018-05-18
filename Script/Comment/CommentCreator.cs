using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// コメント生成クラス
/// </summary>
public class CommentCreator : MonoBehaviour {

	// メンバ変数
    private static CommentCreator instance;
    public static CommentCreator Instance { get { return instance; } }



    [SerializeField] private GameObject      uiRoot;                 // UIルート
    [SerializeField] private GameObject      charaSpeachPrefab;      // キャラクタメッセージのプレハブ
    [SerializeField] private GameObject      systemMessagePrefab;    // システムメッセージのプレハブ

    public static int                        currentDepth;           // Widgetの深度
    public static uint                       commentNumber;          // キャラクタメッセージ番号
    public static uint                       messageNumber;          // システムメッセージ番号



    void Awake() {

        instance = GetComponent<CommentCreator>();
    }



	// Use this for initialization
	void Start () {

        currentDepth    = 0;    // Widgetの深度
        commentNumber   = 0;    // キャラクタメッセージ番号
        messageNumber   = 0;    // システムメッセージ番号
	}
	


	// Update is called once per frame
	void Update () {

    }



    /// <summary>
    /// UIルート
    /// </summary>
    private GameObject UIRoot() { 

        if ( !uiRoot ) { MyUtil.ErrorLog( "null値の変数です。" ); return null; }

        return uiRoot;
    }



    /// <summary>
    /// テキストの読み込み
    /// </summary>
    public List<string> LoadText( string filename ) { 

        // テキストアセットの生成
        TextAsset   textAsset   = Resources.Load( filename ) as TextAsset;

        // バイナリデータの読み込み
        byte[]      rawData     = textAsset.bytes;

        // バイナリデータをUTF-8で文字列に変換
        string      data         = System.Text.Encoding.UTF8.GetString( rawData );



        // 文字列を行単位に分割
//        textData = tmp.Split( new [] {"\r\n"}, System.StringSplitOptions.RemoveEmptyEntries );
/*        string[] sArray = data.Split( new [] {"\r\n"}, System.StringSplitOptions.RemoveEmptyEntries );
        foreach ( string s in sArray ) textData.Add( s );*/

        // データの格納先
        List<string> retTextData = new List<string>();



        // 文字列を改行が連続する箇所で分割

        // 文字列の末尾にnull文字を追加
        data += "\0";

        char[]  c       = data.ToCharArray();   // 文字列データの文字配列
        string  tmp     = "";                   // 文字列データの一時保存用バッファ
        bool    flg     = false;                // フラグ

        // 文字数分ループ
        for ( int i = 0; i < data.Length; i++ ) {
        
            // 改行の場合
            //\n、\r\nを改行とする。
            if ( c[i].Equals( '\r' ) || c[i].Equals( '\n' ) ) {

                // \r\nを想定して1つ進める
                if ( c[i].Equals('\r') ) i++;



                // 次の文字が改行ならフラグON
                //c[i]が改行でc[i + 1]も改行なら、それはデータの区切りである。
                //このスコープ内で一時保存しているデータの追加を行う。
                if ( i < data.Length && (c[i + 1].Equals('\r') || c[i + 1].Equals('\n')) ) {

                    // フラグOFFならデータを追加
                    //この時点でフラグOFFならここを区切りとして、
                    //現在まで一時保存していたデータを追加しておき、
                    //一時保存用のバッファを初期化する。
                    if ( !flg ) {

                        // 一時保存したデータを追加
                        retTextData.Add( tmp );

                        // バッファを初期化
                        tmp = "";
                    }

                    // フラグON
                    //次が改行で続いている事を意味するチェックを付ける。
                    //一時保存用のバッファが初期化されていて、
                    //データの保存が行われていない事も意味している。
                    flg = true;
                }

                

                // 改行でなければフラグOFF
                //c[i]が改行でc[i + 1]が改行でなければ、まだ次の行にデータが続いている。
                else {

                    // 既にフラグOFFなら改行を加える
                    if ( !flg ) tmp += "\n";

                    // フラグOFF
                    //次がデータで続いている事を意味するチェックを付ける。
                    //一時保存用のバッファにまだデータを保存している事も意味している。
                    flg = false;
                }
            }

            // null文字の場合
            //null文字ならそれはデータの終わりであり、
            //その時点の一時保存していたデータを追加する。
            else if ( c[i].Equals('\0') ) {
                
                // 一時保存したデータを追加
                retTextData.Add( tmp );

                // バッファを初期化
                tmp = "";
            }

            // それ以外の場合
            //改行とnull文字以外の文字をデータとする。
            //空白やオクターブなどもデータとする。
            else {

                // データを追加
                //検査した文字を連結する。
                tmp += c[i];
            }
        }



        return retTextData;
    }



    /// <summary>
    /// コメントの生成
    /// </summary>
    public GameObject CreateComment( Vector3 position, string speakerName, string text, Color color, double lifeTime ) {

        if ( !charaSpeachPrefab ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }



        GameObject charaSpeach = WidgetController.AddPrefab( UIRoot(), charaSpeachPrefab );
        if ( !charaSpeach ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        charaSpeach.name                    = "Comment_" + commentNumber++;
        charaSpeach.transform.localPosition = position;



        CharaSpeach comment = charaSpeach.GetComponent<CharaSpeach>();
        if ( !comment ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        comment.lifeTime = lifeTime;
        comment.InitializeColor( color );
        if ( !comment.speakerNameLabel || null == comment.speakerNameLabel.text ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        comment.speakerNameLabel.text = speakerName;



        UILabel label = comment.label;
        if ( !label ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        label.text = text;



        return charaSpeach;
    }



    /// <summary>
    /// コメントの生成
    /// </summary>
    public GameObject CreateComment( Vector3 position, string speakerName, string showStr, double lifeTime ) {

        return CreateComment( position, speakerName, showStr, Color.white, lifeTime );
    }



    /// <summary>
    /// コメントの生成
    /// </summary>
    public GameObject CreateComment( Vector3 position, string speakerName, string showStr ) { 

        return CreateComment( position, speakerName, showStr, Color.white, -1 );
    }



    /// <summary>
    /// システムメッセージの生成
    /// </summary>
    public GameObject CreateSystemMessage( string text ) {

        if ( !systemMessagePrefab ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }



        GameObject systemMessage = WidgetController.AddPrefab( UIRoot(), systemMessagePrefab );
        if ( !systemMessage ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        systemMessage.name = "Message_" + messageNumber++;



        SystemMessage message = systemMessage.GetComponent<SystemMessage>();
        if ( !message ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }



        UILabel label = message.label;
        if ( !label ) {

            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }
        label.text = text;



        return systemMessage;
    }



    /// <summary>
    /// コメントの破棄
    /// </summary>
    public bool DestroyComment( string objName ) {

        GameObject commentObj = MyUtil.SearchChildGameObject( UIRoot(), objName );
        if ( !commentObj ) { return false; }

        Comment comment = commentObj.GetComponent<Comment>();
        if ( !comment ) { return false; }

        comment.DestroyComment();



        return true;
    }
}
