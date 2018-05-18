using UnityEngine;
using System.Collections;



/// <summary>
/// トロッコ制御クラス
/// </summary>
public class TruckController : MonoBehaviour {

    // メンバ変数
    [SerializeField] private WheelCollider      wheelFL;                        // 前輪左
    [SerializeField] private WheelCollider      wheelFR;                        // 前輪右
    [SerializeField] private WheelCollider      wheelRL;                        // 後輪左
    [SerializeField] private WheelCollider      wheelRR;                        // 後輪右
    [SerializeField] private GameObject         standPoint;                     // プレイヤーの立ち位置

    public float                                speed       = 10.0f;            // 速さ
    public float                                breaking    = 20.0f;            // ブレーキ
    
    private GameObject                          player;                         // プレイヤー
    private RigidbodyConstraints                playerRigidbodyConstraints;     // プレイヤーのConstraints
    private bool                                accelerateFlg;                  // 加速フラグ( true : 加速している | false : 加速してない )

    private Vector3[]                           oldPositionArray;               // トロッコの前回の座標データ
    private int                                 oldPosArrayIndex;               // 前回の座標配列のインデックス



	// Use this for initialization
	void Start () {

        player                      = null;
        playerRigidbodyConstraints  = RigidbodyConstraints.None;
        accelerateFlg                     = false;

        oldPosArrayIndex            = 0;
        oldPositionArray            = new Vector3[10];
        for ( int i = 0; i < oldPositionArray.Length; i++ ) { oldPositionArray[i] = new Vector3( -1, -1, -1 ); }



        GetComponent<Rigidbody>().constraints =
            GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY;




        wheelRL.motorTorque = 0;
        wheelRR.motorTorque = 0;
        wheelFL.motorTorque = 0;
        wheelFR.motorTorque = 0;
	}


	
	// Update is called once per frame
	void Update () {

        // 移動
        Moving();

        // プレイヤーの制御
        ControllPlayer();



        // 1フレームに1要素ずつトロッコの座標を保存
        if ( player ) {

            oldPositionArray[ oldPosArrayIndex ] = transform.position;
            if ( ++oldPosArrayIndex >= oldPositionArray.Length ) { oldPosArrayIndex = 0; }
        }
	}



    void OnTriggerEnter( Collider col ) { 
    
        // タグ別に処理
        string tag = col.gameObject.tag;
        switch ( tag ) {

            // プレイヤー
            case "Player":

                // 加速フラグが降りていれば移動開始
                if ( !accelerateFlg ) {

                    // プレイヤーを取得
                    player = col.gameObject;

                    // プレイヤーのConstraintsを保存
                    playerRigidbodyConstraints = player.GetComponent<Rigidbody>().constraints;

                    // プレイヤーのConstraintsを全て固定する
                    player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    // 移動開始
                    MoveStart();



                    
player.GetComponent<AnimatorController>().AnimationChange( 0 );
                }

                break;
        }



        // 名前別に処理
        string name = col.gameObject.name;
        switch ( name ) {

            case "Point_To_Stop_The_Truck":

                // 加速フラグが立っていれば停止する
                if ( accelerateFlg ) {

                    // フラグを降ろす
                    accelerateFlg = false;

                    // 停止
                    Stop();
                }

                break;
        }
    }



    void OnTriggerExit( Collider col ) { 
    
        // タグ別に処理
        string tag = col.gameObject.tag;
        switch ( tag ) {

            // プレイヤー
            case "Player":

                // プレイヤーをクリア
                player = null;

                // スクリプトのアクティブを切る
//                enabled = false;
Destroy( this );
                break;
        }
    }



    /// <summary>
    /// 移動開始の処理
    /// </summary>
    private void MoveStart() {

        // フラグを立てる
        accelerateFlg = true;
		AudioSEManager.Instance.SEPlay("Trolley");

        // フリーズを限定する
GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY;
    }



    /// <summary>
    /// 停止
    /// </summary>
    private void Stop() { 
    
        // 加速フラグが降りていればブレーキを掛ける
        if ( !accelerateFlg ) {

            wheelFL.brakeTorque = breaking;
            wheelFR.brakeTorque = breaking;
            wheelRL.brakeTorque = breaking;
            wheelRR.brakeTorque = breaking;
        }
    }
  


    /// <summary>
    /// 移動
    /// </summary>
    private void Moving() { 
    
        // 加速フラグが降りていれば処理しない
        if ( !accelerateFlg ) { return; }



        // トルクを設定(回転方向はspeedによる)
        wheelRL.motorTorque = speed;
        wheelRR.motorTorque = speed;
    }
    


    /// <summary>
    /// プレイヤーの制御
    /// </summary>
    private void ControllPlayer() { 
    
        if ( !player ) { return; }
        if ( !standPoint ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



if ( IsFinish() ) {
    player.GetComponent<Rigidbody>().constraints = playerRigidbodyConstraints; return;
}
   //     if ( moveFlg ) {Debug.Log((double)Vector3.Distance( transform.position, oldPositionArray[0] ));

            if ( flg ) {

                player.transform.position = standPoint.transform.position;
                return;
            }
            if ( Vector3.Distance(player.transform.position, standPoint.transform.position) <= 0.1f ) {
  //              player.transform.position = standPoint.transform.position;
                flg = true;
                return;
            }
            Vector3 moveVector = standPoint.transform.position - player.transform.position;
            moveVector.Normalize();
            moveVector *= 0.3f;
            player.transform.position += moveVector;
  //      }
    }
    private bool flg = false;



    /// <summary>
    /// 終了フラグ
    /// </summary>
    /// <returns></returns>
    public bool IsFinish() {

        // 停止していて、且つ加速フラグが降りていれば終了
        if ( IsStay() && !accelerateFlg ) { return true; }

        return false;
    }



    /// <summary>
    /// 停止しているか( true : 止まっている | false : 動いている )
    /// </summary>
    /// <returns></returns>
    private bool IsStay() { 
        
        // 座標データの数だけループ
        foreach ( var oldPosition in oldPositionArray ) {
        
            // 現在の位置との距離が少しでも有れば停止していない
            if ( Vector3.Distance( transform.position, oldPosition ) > 0.0001f ) { return false; }
        }



        return true;
    }
}
