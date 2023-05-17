//---------------------------------------------------------
//担当者：二宮怜
//内容　：ステージによってポストエフェクトによる画面効果を変化させる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectManager : MonoBehaviour
{
    // 変数宣言

    // ダメージを受けたときの画面効果を乗せる関数に入るためのフラグ
    public bool damageFlg = false;
    private float Timer = 0f;
    [SerializeField] private float FilterTime = 1f; // 画面効果が開始してから終了するまでの時間
    [SerializeField] private float ChangeVolume = 0.1f; // vignetteの値を増やす量



    // ポストエフェクトの調整したいステータス
    [SerializeField] private List<PostEffectStatus> _postESta;

    // globsl Volume
    [SerializeField] private Volume PostFXvolume;

    // effect↓↓

    // vignette
    private Vignette vignette;

    SetStage setStage = new SetStage();

    // Start is called before the first frame update
    void Start()
    {
        // ポストエフェクトプロセッサーはコンポーネントではないためGetComponentを使わない
        // かわりに各エフェクトの構成情報を持つprofile変数のTryGetメソッドで各プロセッサーを取得

        //------------------------------------------
        //vignette

        // エフェクト取得
        PostFXvolume.profile.TryGet(out vignette);
        // エラーチェック
        if(vignette == null)
        {
            Debug.Log("No Vignette");
        }
        // 強さ設定
        vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        // カラー設定
        vignette.color.value = _postESta[setStage.GetAreaNum()]._vinette_color;


        //-------------------------------------------

        //Debug.Log(setStage.GetAreaNum());

    }

    private void Update()
    {
        //// 強さ設定
        //vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        //// カラー設定
        //vignette.color.value = _postESta[setStage.GetAreaNum()]._vinette_color;

        if(damageFlg == true)
        {
            DamageFilter();
        }
    }

    public void Damage()
    {
        // ダメージをくらった時の処理を実行するためにtrue
        damageFlg = true;
    }

    // 被ダメージ時に画面の周りに色がつくやつ
    private void DamageFilter()
    {
        // 経過時間に応じた変化量
        float value;

        // 演出時間の半分以下なら上昇
        if (Timer <= FilterTime / 2f)
        {
            value = ChangeVolume * Timer;
        }
        else
        {
            value = ChangeVolume * (FilterTime - Timer);
        }

        //Debug.Log(value);

        vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity + value;

        if (Timer > FilterTime)
        {
            damageFlg = false;

            // 初期化
            Timer = 0f;
            vignette.intensity.value = _postESta[setStage.GetAreaNum()]._vignette_intensity;
        }

        Timer += Time.deltaTime;

        Debug.Log(vignette.intensity.value);
    }
}

[System.Serializable]
public class PostEffectStatus
{
    //ステージごとに変更したい ポストエフェクトの数値を持つ変数
    public float _vignette_intensity; // ヴィネットの効果をどれくらい掛けるか
    public Color _vinette_color; // ヴィネットの色

    public PostEffectStatus(float _VineInten,Color _VineCol)
    {
        _vignette_intensity = _VineInten;
        _vinette_color = _VineCol;
    }
}
