//-----------------------------------
//  担当：菅眞心
//　内容：タイトルの演出
//-----------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
public class TitleDirection : MonoBehaviour
{
    Transform thistrans;    // このオブジェクトのTransform

    [SerializeField, Header("目的地")]
    private Vector3 TargetPos;

    [SerializeField,Header("待ち時間")]
    private float WaitTime;     // 目的地に到着したら一定時間待つ

    // 発光用
    private Bloom bloom;
    private Volume volume;

    [SerializeField, Header("クリスタルパネル")]
    private GameObject CrystalPanel;

    [SerializeField, Header("タイトルの演出用")]
    private TitleMove _titlemove;

    [SerializeField, Header("TITLELOGO")]
    private Animator Titleanim;             //TitleLogoのアニメーション

    [SerializeField, Header("PushA_Renderer")]
    private SpriteRenderer PushA;               //PushA
    [SerializeField, Header("PushA_Script")]
    private Button PushA_Script;

    private Animator anim;          //アニメーション用

    private bool FlashFlg = false;  //フラッシュ用フラグ
    private int FlashNum = 0;       //フラッシュ回数

    private float TimeMasure;   // 時間計測用

    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        thistrans = transform;
        // Volume関係取得
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out bloom);

        anim = GetComponent<Animator>();    //Animatorを取得
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            start = true;
        }
        if (start)
        {
            // 敵を移動させる
            if (thistrans.position.x > TargetPos.x && TimeMasure < WaitTime)
            {
                thistrans.position = Vector3.MoveTowards(thistrans.position, TargetPos, 2.0f * Time.deltaTime);
            }
            else
            {

                TimeMasure += Time.deltaTime;   //　時間計測用

                // 一定時間待ったらフラッシュさせる
                if (TimeMasure > WaitTime)
                {
                    // フラッシュが終わったら
                    if (!Flash())
                    {
                        if (FlashNum < 1)
                        {
                            FlashNum++;
                            FlashFlg = false;

                            // 演出用の設定
                            CrystalPanel.SetActive(true);
                            _titlemove.enabled = true;
                            thistrans.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            anim.enabled = true;
                            PushA.color = new Color(1.0f, 1.0f, 1.0f);
                            PushA_Script.enabled = true;
                        }
                        else
                        {
                            Titleanim.enabled = true;
                            thistrans.position = Vector3.MoveTowards(thistrans.position, new Vector3(9.78f, -3.1f, 0.0f), 2.0f * Time.deltaTime);
                        }

                    }
                }
                else
                {
                    anim.enabled = false;
                }
            }
        }
        
    }

    //------------------------------
    //　一瞬光る演出用関数
    private bool Flash()
    {
        if (!FlashFlg && bloom.intensity.value < 40.0f)
        {
            bloom.intensity.value += 50.0f * Time.deltaTime;
        }
        else
        {
            FlashFlg = true;
        }

        if (FlashFlg)
        {
            if (bloom.intensity.value <= 0.0f)
            {
               return false;
            }

            bloom.intensity.value -= 50.0f * Time.deltaTime;
        }

        return true;
    }
}
