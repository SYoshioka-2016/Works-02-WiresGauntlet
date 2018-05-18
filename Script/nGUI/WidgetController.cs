using UnityEngine;
using System.Collections;



/// <summary>
/// NGUIウィジェット制御クラス
/// シングルトン
/// </summary>
public class WidgetController : MonoBehaviour {

    // メンバ変数
    private static readonly WidgetController instance = new WidgetController();



    /// <summary>
    /// コンストラクタ
    /// </summary>
    private WidgetController() { }
    private WidgetController( WidgetController widgetController ) { }



    /// <summary>
    /// インスタンスゲッタ
    /// </summary>
    public static WidgetController Instance { get { return instance; } }



    public static Camera GetCamera( GameObject UIRoot ) {
    
        if ( !UIRoot ) return null;



        Camera camera = MyUtil.SearchChildGameObject( UIRoot, "Camera" ).GetComponent<Camera>();
        return camera;
    }



    /// <summary>
    /// ウィジェットの設定
    /// </summary>
    public static UIWidget SetUIWidget(
        UIWidget        widget,
        Color           color, 
        UIWidget.Pivot  pivot, 
        int             depth, 
        Vector2         dimensions 
    ) { 
        if ( !widget ) {
            
            MyUtil.ErrorLog( "null値の変数です。" );
            return null;
        }



        widget.color    = color;
        widget.pivot    = pivot;
        widget.depth    = depth;
        widget.width    = (int)dimensions.x;
        widget.height   = (int)dimensions.y;



        return widget;
    }



    /// <summary>
    /// スプライトウィジェットの追加
    /// </summary>
    public static UISprite AddUISprite( 
        GameObject      go, 
        Vector3         position, 
        UIAtlas         atlas, 
        string          spriteName, 
        UISprite.Type   spriteType,
        Color           color,
        UIWidget.Pivot  pivot,
        int             depth,
        Vector2         dimensions
    ) {

        // パネルにスプライトを追加
        UISprite sprite = NGUITools.AddWidget<UISprite>( go );



        // ウィジェットの設定
        SetUIWidget( 
            sprite, 
            color, 
            pivot, 
            depth,
            dimensions
        );

        sprite.transform.position = position;

        sprite.atlas        = atlas;
        sprite.spriteName   = spriteName;
        sprite.type         = spriteType;



        return sprite;
    }



    /// <summary>
    /// スプライトウィジェットの追加
    /// </summary>
    public static UISprite AddUISprite( GameObject go ) {

        // パネルにスプライトを追加
        return NGUITools.AddWidget<UISprite>( go );
    }



    /// <summary>
    /// ラベルウィジェットの追加
    /// </summary>
    public static UILabel AddUILabel( 
        GameObject      go, 
        Vector3         position, 
        Vector3         scale, 
        UIFont          font, 
        string          text,
        Color           color,
        UIWidget.Pivot  pivot,
        int             depth
    ) {

        // パネルにラベルを追加
        UILabel label = NGUITools.AddWidget<UILabel>( go );



        // ウィジェットの設定
        SetUIWidget( 
            label, 
            color, 
            pivot, 
            depth,
            Vector2.zero
        );

        label.transform.localScale  = scale;
        label.transform.position    = position;

        label.font = font;
        label.text = text;



        return label;
    }



    /// <summary>
    /// ラベルウィジェットの追加
    /// </summary>
    public static UILabel AddUILabel( GameObject go ) {

        // パネルにラベルを追加
        return NGUITools.AddWidget<UILabel>( go );
    }



    public static GameObject AddPrefab( GameObject go, GameObject prefab ) {
        
        if ( !go || !prefab ) return null;

        

        Transform   tran            = prefab.transform;
        GameObject  child           = NGUITools.AddChild( go, prefab );
        child.transform.localScale  = tran.localScale;



        return child.gameObject;
    }



    /// <summary>
    /// UIラベルの検索
    /// </summary>
    public static UILabel SearchLabel( GameObject go ) {

        // UIパネルの子の数だけループ
        for ( int i = 0; i < go.transform.childCount; i++ ) {

            // 子を取得
            Transform child = go.transform.GetChild(i);

            // 子がラベルを持っていたら発見
            UILabel label = (UILabel)child.GetComponent( "UILabel" );
            if ( label ) { return label; }

            // 発見出来なければ、子を検索する
            label = SearchLabel( child.gameObject );
            if ( label ) return label;
        }



        return null;
    }



    /// <summary>
    /// UIラベルの検索
    /// (UIラベルのテキストで検索)
//上の関数を使って改良出来るかも?
    /// </summary>
    public static UILabel SearchLabel( GameObject go, string text ) {

        // UIパネルの子の数だけループ
        for ( int i = 0; i < go.transform.childCount; i++ ) {

            // 子を取得
            Transform child = go.transform.GetChild(i);

            // 子がラベルを持っていたらUIラベルとして認識
            UILabel label = (UILabel)child.GetComponent( "UILabel" );
            if ( label ) { 
            
                // ラベルのテキストと一致したら発見
                if ( string.Equals(label.text, text) ) return label;
            }

            // 発見出来なければ、子を検索する
            label = SearchLabel( child.gameObject, text );
            if ( label ) return label;
        }



        return null;
    }
}
