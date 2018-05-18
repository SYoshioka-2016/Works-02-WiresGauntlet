using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class Pauser : MonoBehaviour {

    // メンバ変数
    private static List<Pauser>     targets     = new List<Pauser>();   // ポーズ対象のスクリプト
    private Behaviour[]             pauseBehavs = null;                 // ポーズ対象のコンポーネント



	// Use this for initialization
	void Start () {

        // 自身をポーズ対象に追加する
        targets.Add( this );
	}



    /// <summary>
    /// ポーズ対象から除外
    /// </summary>
    private void OnDestroy() { targets.Remove( this ); }



    /// <summary>
    /// ポーズ設定
    /// </summary>
    private void OnPause() { 
    
        if ( null != pauseBehavs ) { return; }



        // 有効なBehaviourを取得
        pauseBehavs = 
            Array.FindAll<Behaviour>(
                GetComponents<Behaviour>(), 
                (obj) => { return obj.enabled; } 
            );

/*
        pauseBehavs = 
            Array.FindAll(
                GetComponentsInChildren<Behaviour>(), 
                (obj) => { return obj.enabled; } 
            );
*/

        // enableを切る
        foreach ( var com in pauseBehavs ) { 
            
if ( 
        !( com is Animator ) && 
        !( com is Animation ) &&
        !( com is UIWidget ) 
    ) {

                com.enabled = false; 
            }
        }
    }



    /// <summary>
    /// ポーズ解除
    /// </summary>
    private void OnResume() { 
    
        if ( null == pauseBehavs ) { MyUtil.ErrorLog( "null値の変数です。" ); return; }



        // enableを有効に戻す
        foreach ( var com in pauseBehavs ) { com.enabled = true; }

        // ポーズ対象のコンポーネントをnullクリア
        pauseBehavs = null;
    }



    public static void Pause() {

        foreach ( var obj in targets ) { obj.OnPause(); }
    }



    public static void Resume() { 
    
        foreach ( var obj in targets ) { obj.OnResume(); }
    }
}
