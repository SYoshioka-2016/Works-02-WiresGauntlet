using UnityEngine;
using System.Collections;

public class SystemMessage : Comment {

	// メンバ変数



	// Use this for initialization
	protected override void Start () {

        InitializeEffect();
	}
	


	// Update is called once per frame
	protected override void Update () {

        // コントラクトエフェクト終了によるゲームオブジェクト破棄
        DestroyByFinishContractEffect();
	}
}
