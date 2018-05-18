using UnityEngine;
using System.Collections;
using System.Collections.Generic;



using System.Reflection;



public class LabelPartsManager : MonoBehaviour {

    // メンバ変数
    [SerializeField] private GameObject     labelPartsPrefab;           // ラベルパーツプレハブ
    [SerializeField] private int            labelPartsCount     = 2;    // ラベルパーツ数
    
    [SerializeField] private float          startY              = -400; // スタート位置のY座標
    [SerializeField] private float          goalY               = 200;  // ゴール位置のY座標
    [SerializeField] private float          moveSpeed           = 4;    // 移動の速さ

    [SerializeField] private string[]       textTable =                 // ラベルのテキストテーブル
    {
        "-----------------",
        "** **",
        "*** ***",
        "****** *****",
        "-----------------",
        "*** **",
        "* ***",
        "** ****",
        "-----------------",

        "- END -",
    };



    [SerializeField] private LabelParts[]   labelPartsArray;            // ラベルパーツ配列



	// Use this for initialization
	void Start () {

        if ( 0 >= labelPartsCount ) { return; }
        
        GameObject uiRoot = GameObject.Find( "UI Root" );
        if ( !uiRoot ) { return; }



        // ラベルパーツ配列を生成
        labelPartsArray = new LabelParts[ labelPartsCount ];

        // UIルートにラベルパーツを追加
        for ( int i = 0; i < labelPartsCount; i++ ) {

            LabelParts labelParts = NGUITools.AddChild( uiRoot, labelPartsPrefab ).GetComponent<LabelParts>();
            if ( labelParts ) { labelPartsArray[i] = labelParts; }
        }



        // ラベルパーツ毎にテキストテーブルを生成
        List<string>[] textTableListArray = new List<string>[ labelPartsCount ];
        for ( int i = 0; i < labelPartsCount; i++ ) { 
            
            textTableListArray[i] = new List<string>(); 
        }

        // テキストを振り分ける
        for ( int i = 0; i < textTable.Length; i++ ) {
            
            textTableListArray[ i % labelPartsCount ].Add( textTable[i] );
        }



        // ラベルパーツを初期化
        for ( int i = 0; i < labelPartsCount; i++ ) {

            labelPartsArray[i].textTable    = textTableListArray[i].ToArray();
            labelPartsArray[i].goal         = new Vector3( 0, goalY, 0 );
            labelPartsArray[i].start        = new Vector3( 0, startY - (i * (400 / labelPartsCount)), 0 );
            labelPartsArray[i].moveSpeed    = moveSpeed;

            labelPartsArray[i].SendMessage( "Start" );
        }
	}
	


	// Update is called once per frame
	void Update () {
        
        this.Skip();
	}



    private void Skip()
    {
        if ( !Input.GetKeyDown( KeyCode.Return ) ) { return; }



        Application.LoadLevel( "Title" );
    }
}
