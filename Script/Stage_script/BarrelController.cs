using UnityEngine;
using System.Collections;



/// <summary>
/// 樽制御クラス
/// </summary>
public class BarrelController : MonoBehaviour {

    public  GameObject  BarrelFolder;   // 樽オブジェクトを束ねるオブジェクト
    private bool        isCrumbled;     // 崩れたか? (true : 崩れた | false : 崩れてない)



	// Use this for initialization
	void Start () {

        isCrumbled = false;

        // 樽オブジェクトのコンポーネントのセット
        SetComponent();
	}


	
	// Update is called once per frame
	void Update () {

        // 樽が崩れたかチェック
        Check();
	}



    /// <summary>
    /// 樽オブジェクトのコンポーネントのセット
    /// </summary>
    private void SetComponent() {
 
        // 樽管理オブジェクトが無ければ処理しない
        if ( !BarrelFolder ) return;


	
        for ( int i = 0; i < BarrelFolder.transform.childCount; i++ ) {

            GameObject child = BarrelFolder.transform.GetChild(i).gameObject;

            if ( "Barrel" == child.name ) {

                child.AddComponent<BoxCollider>();
                child.AddComponent<Rigidbody>();
                child.AddComponent<Barrel>();

                child.GetComponent<Rigidbody>().mass = 1.0f;
//                child.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
/*                child.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                child.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                child.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;*/
            }
        }
    }



    /// <summary>
    /// 樽が崩れたかチェック
    /// </summary>
    private void Check() {
 
        // 崩れていたら処理しない
        if ( isCrumbled ) return;



        // 樽管理オブジェクトの子数分ループ
        for ( int i = 0; i < BarrelFolder.transform.childCount; i++ ) {

            // 子を取得
            GameObject child = BarrelFolder.transform.GetChild(i).gameObject;

            // 子が樽なら
            if ( "Barrel" == child.name ) {

                // 樽がプレイヤーと衝突したら樽山を崩す
                Barrel barrel = child.GetComponent<Barrel>();
                if ( barrel.IsHit() ) {

                    // 崩れる処理
                    Crumble();
					AudioSEManager.Instance.SEPlay("Barrel");
                    break;
                }
            }
        }
    }



    /// <summary>
    /// 崩れる処理
    /// </summary>
    private void Crumble() {

        // 崩れていたら処理しない
        if ( isCrumbled ) return;



        // 樽管理オブジェクトの子数分ループ
        for ( int i = 0; i < BarrelFolder.transform.childCount; i++ ) {

            // 子を取得
            GameObject child = BarrelFolder.transform.GetChild(i).gameObject;
/*
            // 子が樽なら
            if ( "Barrel" == child.name ) {

                // Rigitbodyのフリーズを解除する
                child.rigidbody.constraints = RigidbodyConstraints.None;
            }*/
        }

        // 「崩れた」に設定
        isCrumbled = true;
    }
}
