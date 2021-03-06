﻿using UnityEngine;
using System.Collections;



/// <summary>
/// コントラクトエフェクトクラス
/// </summary>
public class Contract : MonoBehaviour {

    // メンバ変数
    public  Vector3     startScale      = new Vector3( 1, 1, 1 ) * -1;  // エフェクト開始時のスケール(負数の成分はゲームオブジェクトのスケールから取得する。)
    public  Vector3     goalScale       = new Vector3( 0, 0, 0 );       // エフェクト終了時のスケール(負数の成分は0にする。)
    public  float       scalingSpeed    = 1.0f;                         // スケーリングの速さ

    private bool        finishFlg;          // エフェクト終了フラグ(true : 終了した | false : 終了してない)
    private Vector3     formerScale;        // 元々のスケール
    private Vector3     vec3ScalingSpeed;   // スケーリングの速さ(毎フレームのスケール値の加算量)



	// Use this for initialization
	void Start () {

        // エフェクト開始の初期化処理
        Initialize();
    }
	


	// Update is called once per frame
	void Update () {

        // エフェクトが終了したら処理しない
        if ( finishFlg ) { return; }



        // スケールの成分の値がエフェクト終了時のスケール以下なら
        // スケーリングの速さのその成分を0にする
        if ( transform.localScale.x <= goalScale.x ) { vec3ScalingSpeed.x = 0; }
        if ( transform.localScale.y <= goalScale.y ) { vec3ScalingSpeed.y = 0; }
        if ( transform.localScale.z <= goalScale.z ) { vec3ScalingSpeed.z = 0; }

        // 現在のスケールにスケーリングの速さを加算してスケーリング
        transform.localScale += vec3ScalingSpeed;



        // エフェクトの終了処理
        Finish();
	}



    /// <summary>
    /// エフェクト開始の初期化処理
    /// </summary>
    public void Initialize() { 
    
        finishFlg = false;
	
        // 元々のスケールを保存
        formerScale = transform.localScale;



        // エフェクト開始時のスケールを調整
        //
        // 指定されたスケールで値が0未満の成分は
        // 元々のスケールの成分と同じにして
        // その方向のスケーリングを無効にする。
        if ( 0 > startScale.x ) { startScale.x = formerScale.x; }
        if ( 0 > startScale.y ) { startScale.y = formerScale.y; }
        if ( 0 > startScale.z ) { startScale.z = formerScale.z; }

        // スケールをエフェクト開始時のスケールにする
        transform.localScale = startScale;



        // エフェクト終了時のスケールを調整
        //
        // 指定されたスケールで値が0未満の成分は0にする。
        if ( 0 > goalScale.x ) { goalScale.x = 0; }
        if ( 0 > goalScale.y ) { goalScale.y = 0; }
        if ( 0 > goalScale.z ) { goalScale.z = 0; }



        // スケーリングの速さ(毎フレームのスケール値の加算量)を求める
        Vector3 goalToStartScaleVector  = startScale - goalScale;
        float   maxElement              = Mathf.Abs( Mathf.Max( goalToStartScaleVector.x, goalToStartScaleVector.y, goalToStartScaleVector.z ) );
        vec3ScalingSpeed                = -( goalToStartScaleVector / maxElement ) * Mathf.Abs( scalingSpeed );
    }



    /// <summary>
    /// エフェクトの終了情報(true : 終了した | false : 終了してない)
    /// </summary>
    /// <returns></returns>
    public bool IsFinished() { return finishFlg; }



    /// <summary>
    /// エフェクトの終了処理
    /// </summary>
    /// <returns></returns>
    private bool Finish() { 

        // エフェクト終了フラグが立っていれば処理しない
        if ( finishFlg ) { return true; }


    
        // スケールの全ての成分の値がエフェクト終了時のスケール以下なら終了
        if ( 
            transform.localScale.x <= goalScale.x &&
            transform.localScale.y <= goalScale.y &&
            transform.localScale.z <= goalScale.z 
        ) {

            // エフェクト終了フラグを立てる
            finishFlg               = true;

            // スケールをエフェクト終了時のスケールにする
            transform.localScale    = goalScale;



            return true;
        }



        return false;
    }
}
