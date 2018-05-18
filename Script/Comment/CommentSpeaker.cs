using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// コメント発生クラス
/// </summary>
public class CommentSpeaker : MonoBehaviour {

    // メンバ変数
    [SerializeField] private TextAsset          speachTextFile;         // セリフデータのファイル
    
    [SerializeField] private GameObject         ivents;                 // イベントをまとめたオブジェクト

    private CommentCreator                      commentCreator;         // コメント生成クラス
    private GameObject                          player;                 // プレイヤー
    private List<string>                        textData;               // テキストファイル読み込み先
    private List<Speach>                        speachList;             // セリフリスト



	// Use this for initialization
	void Start () {

        commentCreator  = GetComponent<CommentCreator>();
        player          = GameObject.FindGameObjectWithTag( "Player" );
        speachList      = new List<Speach>();
        speachList.Clear();



        // イベントの初期化
        InitializeIvent();
    }


	
	// Update is called once per frame
	void Update () {

	}



    /// <summary>
    /// イベントの初期化
    /// </summary>
    private void InitializeIvent() { 
        
        if ( null == speachList ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( !commentCreator ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        if ( !ivents ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }


    
        // テキストの読み込み
        if ( speachTextFile && null != speachTextFile.name ) { textData = commentCreator.LoadText( speachTextFile.name ); }



        // セリフを生成
        for ( int i = 0; i < textData.Count; i++ ) { 
        
            speachList.Add( new Speach( textData[i] ) );
        }



        // イベントをまとめたオブジェクトからコンポーネントを取得
        Ivent[] iventArray = ivents.transform.GetComponentsInChildren<Ivent>();


        // セリフのイベント番号で各イベントに振り分ける
        foreach ( var s in speachList ){ 
            
            iventArray[ s.IventNumber ].AddSpeach( s );
        }
    }
}
