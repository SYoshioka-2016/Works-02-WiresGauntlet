using UnityEngine;
using System.Collections;



/// <summary>
/// ライト制御クラス
/// </summary>
public class LightController : MonoBehaviour {

    // メンバ変数
    private const float  intensityMin    = 3.7f;             // ライトの強さの最小
    private const float  intensityMax    = 4.0f;             // ライトの強さの最大
    private const float  flashingRateMin = 0.0f;             // ライトの点滅のレートの最小
    private const float  flashingRateMax = 100.0f;           // ライトの点滅のレートの最大

    public  float        intensity       = intensityMax;     // ライトの強さ
    public  float        flashingRate    = 20.0f;            // ライトの点滅のレート

    private Light        light;    // ライト



	// Use this for initialization
	void Start () {

        // ライトの設定
        SetLight();

        // ライトの点灯
        EnableLight();
    }
	


	// Update is called once per frame
	void Update () {

        // ライトの強さの設定
        SettingIndensity();

        // ライトの点滅
        Flashing();
	}



    void OnTriggerEnter( Collider col ) { 
    
        // タグ別に処理
        if (col.GetComponent<Collider>().tag == "Wire"){
            DisableLight();
			AudioSEManager.Instance.SEPlay("Lamp");
		}
    }



    /// <summary>
    /// ライトの点灯
    /// </summary>
    public void EnableLight() {
    
        // ライトが無ければ処理しない
        if ( !light ) return ;


        // ライトを有効にする
        light.enabled = true;
    }



    /// <summary>
    /// ライトの消灯
    /// </summary>
    public void DisableLight() {
    
        // ライトが無ければ処理しない
        if ( !light ) return ;


        // ライトを無効にする
        light.enabled = false;
    }



    /// <summary>
    /// カスタムエディタ用
    /// </summary>
    public float IntensityMin() { return intensityMin; }
    public float IntensityMax() { return intensityMax; }
    public float FlashingRateMin() { return flashingRateMin; }
    public float FlashingRateMax() { return flashingRateMax; }



    /// <summary>
    /// ライトの設定
    /// </summary>
    private Light SetLight() { 
    
        // 自身のオブジェクトからライトコンポーネントを取得
        Light check = gameObject.GetComponent<Light>();

        // ライトコンポーネントを取得できればライトを設定
        light       = ( check ) ? gameObject.GetComponent<Light>() : null;



        return light;
    }



    /// <summary>
    /// ライトの強さの設定
    /// </summary>
    private void SettingIndensity() {

        // ライトが無ければ処理しない
        if ( !light ) return;



        // ライトの強さが範囲を超えたら調整
        if ( intensityMin > intensity ) intensity = intensityMin;
        if ( intensityMax < intensity ) intensity = intensityMax;

        // ライトの強さを設定
        light.intensity = intensity;
    }



    /// <summary>
    /// ライトの点滅
    /// </summary>
    private void Flashing() {

        // ライトが無ければ処理しない
        if ( !light ) return;

        // ライトが点灯していなければ処理しない
        if ( !light.enabled ) return;



        // 乱数値
        float rand = (int)Random.Range( flashingRateMin, flashingRateMax );

        // レートを調整
        if ( flashingRateMin > flashingRate ) flashingRate = flashingRateMin;
        if ( flashingRateMax < flashingRate ) flashingRate = flashingRateMax;

        // レートが乱数値を超えたら点滅
        if ( flashingRate > rand ) {

            // ライトの強さの中間値
            float pivot = ( intensityMax + intensityMin ) / 2;

            // 現在のライトの強さと中間値を比較して、最大と最小でスイッチング
            intensity = ( intensity > pivot ) ? intensityMin : intensityMax;
        }
    }
}
