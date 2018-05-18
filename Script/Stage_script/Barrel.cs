using UnityEngine;
using System.Collections;



/// <summary>
/// 樽クラス
/// </summary>
public class Barrel : MonoBehaviour {

    // メンバ変数
    public  float   power = 10.0f;      // 吹っ飛ぶ力量
    private bool    isHit;              // 衝突したか? (true : 衝突した | false : 衝突してない)



	// Use this for initialization
	void Start () {

        isHit = false;
	}


	
	// Update is called once per frame
	void Update () {
	
	}



    void OnCollisionEnter( Collision col ) {

        // タグ別に処理
        string tag = col.gameObject.tag;
        switch ( tag ) {

            // プレイヤー
            case "Player":

                // 衝突した
                isHit = true;

                // RigitbodyのFreezePositionとFreezeRotationを解除する
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;



                // プレイヤーから樽へのベクトルを力のベクトルとする
                Vector3 force = gameObject.transform.position - col.transform.position;

                // 力のベクトルに上ベクトルを加えて補正
//                force += Vector3.up * force.magnitude * power;
                force.Normalize();



                // 樽に力を加える
                GetComponent<Rigidbody>().AddForce( force * power, ForceMode.Impulse );

                break;
        }
    }



    /// <summary>
    /// 衝突したか?
    /// </summary>
    /// <returns></returns>
    public bool IsHit() { return isHit; }
}
