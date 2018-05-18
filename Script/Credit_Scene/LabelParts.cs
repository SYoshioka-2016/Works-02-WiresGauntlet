using UnityEngine;
using System.Collections;



/// <summary>
/// ラベルパーツクラス
/// </summary>
public class LabelParts : MonoBehaviour {

    // メンバ変数
    private UILabel     label;              // ラベル
    private Vector3     moveVector;         // 移動ベクトル

    private int         index;              // テキストテーブル配列のインデックス

    private readonly Vector3 restart = new Vector3( 0, -200, 0 );
    
    public float        moveSpeed;          // 移動の速さ
    public string[]     textTable;          // テキストテーブル
    
    public Vector3      start;              // スタート位置
    public Vector3      goal;               // ゴール位置



	// Use this for initialization
	void Start () {

        // コンポーネントを取得
        label           = GetComponent<UILabel>();

        // 座標をスタート位置にする
        transform.localPosition = start;



        moveSpeed   = Mathf.Abs( moveSpeed );
        moveVector  = (goal - start).normalized * moveSpeed;
        index       = 0;



        // ラベルのテキストの変更
        ChangeText( 0 );
	}



	// Update is called once per frame
	void Update () {

        // テキストテーブル配列のインデックスが最大で
        // ゴールに達したら終了
        if ( 
            null != textTable &&
            textTable.Length - 1 <= index && 
            IsReachTheGoal()
        ) {
            if ( label ) { label.text = ""; }

            Application.LoadLevel( "Title" );
            return;
        }



        // リセット
        Reset();


        // 移動
        for ( int i = 0; i < 10; i++ ) {

            if ( !IsReachTheGoal() ) { 
                
                transform.localPosition += moveVector / 10;
            }
        }
	}



    /// <summary>
    /// ゴールに達したか
    /// </summary>
    /// <returns></returns>
    public bool IsReachTheGoal() { 

        // 現在の座標とゴールの距離が縮まったらゴールに達する
        return Vector3.Distance( transform.localPosition, goal ) <= 1.0f;
    }



    /// <summary>
    /// ラベルのテキストの変更
    /// </summary>
    /// <param name="index"></param>
    private void ChangeText( int index ) { 
    
        // ラベルが無ければ処理しない
        if ( !label ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }
        
        // テキストテーブルが無ければ処理しない
        if ( null == textTable ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }

        // テキストテーブルの要素が無ければ処理しない
        if ( 0 >= textTable.Length ) { return; }



        // 配列のインデックスを調整
        this.index = Mathf.Clamp( index, 0, textTable.Length - 1 );

        // ラベルのテキストを変更
        label.text = textTable[ this.index ];
    }



    /// <summary>
    /// リセット
    /// </summary>
    private void Reset() {

        // ゴールに達したら設定をリセットする
        if ( IsReachTheGoal() ) {

            
start = restart;

            // 座標をスタート位置にする
            transform.localPosition = start;

            // ラベルのテキストの変更
            ChangeText( index + 1 );
        }
    }
}
