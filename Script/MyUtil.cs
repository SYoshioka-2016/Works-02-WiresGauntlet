using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MyUtil : MonoBehaviour {

    // メンバ変数
    private static readonly MyUtil instance = new MyUtil();



    /// <summary>
    /// コンストラクタ
    /// </summary>
    private MyUtil() { }
    private MyUtil( MyUtil inst ) { }



    /// <summary>
    /// インスタンスゲッタ
    /// </summary>
    public static MyUtil Instance { get { return instance; } }



    public static bool ErrorLog( string message ) {

        System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame( 1, true );
        Debug.Log( "file name :: " + sf.GetFileName() + "\nmethod name :: " + sf.GetMethod().ToString() + "\nline :: " + sf.GetFileLineNumber() + " >>> \n\n" + message + "\n" );
        return false;
    }



    /// <summary>
    /// ゲームオブジェクトの子を取得(名前で検索)
    /// </summary>
    public static GameObject SearchChildGameObject( GameObject go, string name ) {

        // 子の数だけループ
        for ( int i = 0; i < go.transform.childCount; i++ ) {

            // 子を取得
            GameObject child = go.transform.GetChild(i).gameObject;

            // 子の名前が検索する名前と一致したら発見
            if ( child && child.name.Equals(name) ) { 
            
                return child;
            }



            // 発見出来なければ、更に子の下の子を検索する
            child = SearchChildGameObject( child.gameObject, name );
            if ( child ) return child;
        }
        


        return null;
    }
}
