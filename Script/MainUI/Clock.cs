using UnityEngine;
using System.Collections;



public class Clock : MonoBehaviour {

    // メンバ変数
	private const double    TIME_INTERVAL   = 15.0;     // 分
    private const double    ANGLE_INTERVAL  = ( (360.0 / 12) / 60 ) * TIME_INTERVAL;// * (Mathf.PI / 180.0f);
    private const double    ROTATE_ANGLE    = 0.2;

    private double angle;
    private double prevAngle;
    private double time;
    private double lostTime;

    private double rotateSpeed;

    public static double playTime = 0;      // プレイ時間



	// Use this for initialization
	void Start () {

//        angle = transform.rotation.z;
        angle       = 0;
        prevAngle   = 0;
        time        = 0;
        lostTime    = 0;

        rotateSpeed = ROTATE_ANGLE;




GameOverSceneManager.backSceneName = Application.loadedLevelName;
	}



    void Update () {

        // 一定時間経過したら回転
        if ( TIME_INTERVAL * 60 < time ) {

            lostTime += Time.deltaTime;



            if ( 0 <= rotateSpeed && ANGLE_INTERVAL <= angle - prevAngle - 1 ) {

                rotateSpeed = -ROTATE_ANGLE;
            }

            if ( 0 > rotateSpeed && ANGLE_INTERVAL >= angle - prevAngle ) {
            
                // 回転量を設定
                angle       = ( ANGLE_INTERVAL + prevAngle );
                
                // 回転量を保存
                prevAngle   = angle;

                // タイムカウンタを初期化
//                time        = 0;
                time        = lostTime;
                lostTime    = 0;



                rotateSpeed = ROTATE_ANGLE;
            }

            // Z軸回転
            angle += rotateSpeed;
            transform.rotation = Quaternion.AngleAxis( (float)angle, Vector3.forward );
        }
        else {
            
            // タイムカウント
            time += Time.deltaTime * 600;
        }



        playTime += Time.deltaTime;
//Debug.Log( (int)playTime + "s" );
	}
}