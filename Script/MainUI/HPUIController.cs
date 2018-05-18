using UnityEngine;
using System.Collections;



public class HPUIController : MonoBehaviour {
    
    // メンバ変数
    public  GameObject[]        HPUIs;                          // 体力を表す単位当たりのUIの配列     
    public  GameObject          damageEffectPrefab;             // ダメージエフェクト用のゲームオブジェクト(スプライト)
    public  ParticleSystem      fireSplashParticle;             // ダメージエフェクト用のパーティクル

    private ParticleSystem[]    hpParticles;                    // 体力をUIとして表す炎パーティクルの配列

    private const int           lifeCountPerUnitHPUI_MAX = 2;                       // HPUI1つ当たりのカウント数の最大
    private const float         HP_PARTICLE_START_LIFETIME_LARGE        = 0.18f;
    private const float         HP_PARTICLE_START_LIFETIME_SMALL        = 0.10f;
    private const float         FIRE_SPLASH_PARTICLE_START_SPEED_MAX    = 3.0f;
    private const float         FIRE_SPLASH_PARTICLE_START_SPEED_MIN    = 1.5f;
    private const float         FIRE_SPLASH_PARTICLE_START_SIZE_MAX     = 0.2f;
    private const float         FIRE_SPLASH_PARTICLE_START_SIZE_MIN     = 0.07f;
    
    private int                 prevHP;                         // 前回の体力
    private CharacterStatus     characterStatus;                // キャラクタステータス



	// Use this for initialization
	void Start () {

        characterStatus = GetComponent<CharacterStatus>();



        // HPUIs配列からデータを取得
        GetArrayFromHPUIs();



        prevHP = 0;
	}


	
	// Update is called once per frame
	void Update () {

        // HPによるUIの操作
        ControlHPUI();
        


        for ( int i = 0; i < hpParticles.Length; i++ ) {

            hpParticles[i].time = Time.realtimeSinceStartup;
        }

        // 体力を保存
        prevHP = characterStatus.HP;
	}



    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage() { 
//CharacterStatusクラスのDamage()が先に呼ばれる為、
//前回のHPを参照した方が良い?
//        if ( !damageEffectPrefab || !characterStatus || 0 >= characterStatus.HP ) return;
        if ( !damageEffectPrefab || !characterStatus || 0 >= prevHP ) return;



//        int HP = characterStatus.HP;
        int HP = prevHP;

//1度のダメージが3以上だと、演出面で不自然になるかも?

        // 体力の炎パーティクル配列のインデックスを算出
        //現在のHPをもとに、有効な範囲で右端の炎パーティクルがある
        //インデックスを求める。
        int idx = ( HP / lifeCountPerUnitHPUI_MAX ) - 1;
        idx += ( (HP % lifeCountPerUnitHPUI_MAX ) >= 1 ) ? 1 : 0;


        // インデックス0番以外用の炎パーティクル設定
        if ( 1 <= idx ) {

            fireSplashParticle.startSpeed = FIRE_SPLASH_PARTICLE_START_SPEED_MIN;
            fireSplashParticle.startSize  = FIRE_SPLASH_PARTICLE_START_SIZE_MIN;
        }

        // インデックス0番用の炎パーティクル設定
        else {

            fireSplashParticle.startSpeed = FIRE_SPLASH_PARTICLE_START_SPEED_MAX;
            fireSplashParticle.startSize  = FIRE_SPLASH_PARTICLE_START_SIZE_MAX;
        }

        // ダメージエフェクト用のパーティクルを
        // 対象の炎パーティクルの位置にして発生させる
        fireSplashParticle.transform.position = hpParticles[ idx ].transform.position;
        fireSplashParticle.Play();



        // ダメージエフェクト用のゲームオブジェクト(スプライト)を生成
        WidgetController.AddPrefab( 
            GameObject.Find( "UI Root" ),
            damageEffectPrefab
        );
    }



    /// <summary>
    /// HPUIs配列からデータを取得
    /// </summary>
    private bool GetArrayFromHPUIs() { 
    
        if ( 0 >= HPUIs.Length ) { return MyUtil.ErrorLog( HPUIs.ToString() + "の要素数は0です。" ); }



        // 体力をUIとして表す炎パーティクルの配列を取得
        hpParticles = new ParticleSystem[ HPUIs.Length ];
        for ( int i = 0; i < hpParticles.Length; i++ ) { 
            
            hpParticles[i] = HPUIs[i].transform.GetChild(2).GetComponent<ParticleSystem>(); 
            if ( !hpParticles[i] ) { return MyUtil.ErrorLog( "null値の変数です。" ); }
        }



        return true;
    }



    /// <summary>
    /// HPによるUIの操作
    /// </summary>
    private void ControlHPUI() {

        if ( characterStatus.HP == prevHP ) return;



        int HP = characterStatus.HP;



        // 体力を保存
        int hp = HP;



        // HPUIの数だけ左から右へパーティクルを設定する
        for ( int i = 0; i < hpParticles.Length; i++ ) {

            // HP1ポイント当たりの寿命の大きさ
            //0番は大きく、それ以降は小さい。
            float startLifeTimePerUnitHP = 
                ( i == 0 ) ? HP_PARTICLE_START_LIFETIME_LARGE / lifeCountPerUnitHPUI_MAX : HP_PARTICLE_START_LIFETIME_SMALL / lifeCountPerUnitHPUI_MAX;

            // 体力をHPUI1つ当たりのカウント数の最大で割った商と余り
            int q = hp / lifeCountPerUnitHPUI_MAX;
            int r = hp % lifeCountPerUnitHPUI_MAX;



            // 商が1以上ならそのHPUIの寿命は最大
            if ( 1 <= q ) {

                // HP1ポイント当たりの寿命の大きさ × HPUI1つ当たりのカウント数の最大で最大値
                hpParticles[i].startLifetime = startLifeTimePerUnitHP * lifeCountPerUnitHPUI_MAX;

                // 計算用の体力を更新
                hp -= lifeCountPerUnitHPUI_MAX;

                // パーティクルを再生
                hpParticles[i].Play();



                // UI画像を明るくする
                UISprite hpSprite = HPUIs[i].transform.GetChild(0).GetComponent<UISprite>();
                hpSprite.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
            }
            // 商が0で余りが1以上ならそのHPUI(寿命が有るHPUIの中で右端)の寿命は最大値より小さい
            else if ( 1 <= r ) {

                // 寿命は、HP1ポイント当たりの寿命の大きさ × 余り(HPUI1つ当たりのカウント数の最大未満)
                hpParticles[i].startLifetime = startLifeTimePerUnitHP * r;

                // 計算用の体力を更新
                hp -= r;

                // パーティクルを再生
                hpParticles[i].Play();



                // UI画像を明るくする
                UISprite hpSprite = HPUIs[i].transform.GetChild(0).GetComponent<UISprite>();
                hpSprite.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
            }
            // 商も余りも0ならそのHPUI(寿命が有るHPUIより右)のパーティクルを停止
            else {

                // パーティクルを停止
                hpParticles[i].Stop();



                // UI画像を暗くする
                UISprite hpSprite = HPUIs[i].transform.GetChild(0).GetComponent<UISprite>();
                hpSprite.color = new Color( 0.3f, 0.3f, 0.3f, 1.0f );
            }
        }
    }
}