using UnityEngine;
using System.Collections;



/// <summary>
/// コメントクラス
/// </summary>
public abstract class Comment : MonoBehaviour {

	// メンバ変数
    public      UILabel         label;          // ラベル
    public      UISprite[]      sprites;        // スプライト

    protected   Expand          expand;         // エクスパンドエフェクト
    protected   Contract        contract;       // コントラクトエフェクト



	// Use this for initialization
    protected abstract void Start();
	


	// Update is called once per frame
	protected abstract void Update ();



    protected void InitializeEffect() {

        // エクスパンドエフェクトを取得
        expand = GetComponent<Expand>();
        if ( expand ) { expand.enabled = true; }

        // コントラクトエフェクトを取得
        contract = GetComponent<Contract>();
        if ( contract ) { contract.enabled = false; }
    }



    /// <summary>
    /// コメントの破棄
    /// </summary>
    /// <returns></returns>
    public bool DestroyComment() { 
    
        // コントラクトエフェクトが無いなら処理しない
        if ( !contract ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // コントラクトエフェクト終了によるゲームオブジェクト破棄
        DestroyByFinishContractEffect();



        // コントラクトエフェクトのアクティブを設定
        if ( !contract.enabled ) { 

            // エクスパンドエフェクトを破棄する
            if ( expand ) { Destroy( expand ); }

            // コントラクトエフェクトのアクティブを有効にする
            contract.enabled = true;



            return true;
        }



        return false;
    }



    /// <summary>
    /// コントラクトエフェクト終了によるゲームオブジェクト破棄
    /// </summary>
    /// <returns></returns>
    protected bool DestroyByFinishContractEffect() { 
    
        // コントラクトエフェクトが無いなら処理しない
        if ( !contract ) { return MyUtil.ErrorLog( "null値の変数です。" ); }



        // コントラクトエフェクトが終了したら破棄する
        if ( contract.enabled && contract.IsFinished() ) {

            CommentCreator.currentDepth = gameObject.transform.GetChild(0).GetComponent<UIWidget>().depth;
            Destroy( gameObject );



            return true;
        }



        return false;
    }
}
