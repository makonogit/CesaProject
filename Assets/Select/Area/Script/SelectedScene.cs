//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：シーン移動
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SelectedScene : MonoBehaviour
{
    [SerializeField,Header("確認用です。")]
    private string _selectScene = null;
    private SceneChange sceneChange;
    private bool _isChanging;

    // 二宮追加
    private GameObject se;
    private AudioSource Audio;

    // 菅追加
    private SetStage setmanager;
    private StageManager stagemanager;
    private SelectZoom zoom;
    private Animator anim;

    [SerializeField,Header("ひびのオブジェクト")]
    private GameObject Crack;   // ひびのオブジェクト

    [SerializeField, Header("ひびの破片")]
    private GameObject CrackParticle;
    private bool Create = false;    // 生成フラグ
    

    // プレイヤーのパーティクル関連
    private GameObject PlayerParticle; // プレイヤーのパーティクル
    private ParticleSystem PlayerParticleSystem;
    private float ParticleLifetime = 5.0f;


    //　選択アニメーション用
    private GameObject CrackObj;
    private float WaitTime = 0.0f;
    private float Line = 1.0f;  //Materialアニメーション用
    public Transform AreaPos;   //選択しているステージのTransform

    private void Start()
    {
        setmanager = new SetStage();  //ステージマネージャーの取得
        stagemanager = GetComponent<StageManager>();
        zoom = GameObject.Find("CameraControl").GetComponent<SelectZoom>();
        anim = GetComponent<Animator>();    // Animatorを取得
        PlayerParticle = transform.GetChild(0).gameObject;
        PlayerParticleSystem = PlayerParticle.GetComponent<ParticleSystem>();
        PlayerParticle.SetActive(false);    //パーティクルを非表示に


        transform.position = stagemanager.stage[setmanager.GetAreaNum()].stage[setmanager.GetStageNum()].StageObj.transform.position;

        _selectScene = null;
        sceneChange = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (sceneChange == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
        _isChanging = false;

        se = GameObject.Find("SE");
        Audio = se.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (zoom.ZoomEnd)
        {
            anim.SetBool("accumulate", false);
            anim.SetBool("crack", true);

            //------------------------------------
            // ヒビに入るアニメーション
            if (!Create)
            {
                // ひびを生成する
                CrackObj = Instantiate(Crack,
                    AreaPos.position, Quaternion.identity);
                CrackObj.transform.localScale = new Vector3(0.08f, 0.08f, 1.0f);
                CrackObj.GetComponent<SpriteRenderer>().sortingOrder = 12;
                se.GetComponent<SEManager_Select>().PlayHammer();
                
                //　パーティクルを生成
                GameObject Particle = Instantiate(CrackParticle, transform.position, Quaternion.identity);
                Particle.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().sortingOrder = 13;
                Create = true;

                GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            }
            else
            {
                WaitTime += Time.deltaTime;

                //　打ち込んでから0.3秒待機
                if (WaitTime > 0.3f)
                {
                    PlayerParticle.SetActive(true);

                    Line -= 2.0f * Time.deltaTime;
                    GetComponent<SpriteRenderer>().material.SetFloat("_Border",Line);

                    //　Materialアニメーションが終了していたら
                    if (Line < 0.0f)
                    {

                        //　ヒビに入る
                        transform.position = Vector3.MoveTowards(transform.position, CrackObj.transform.position, 5.0f * Time.deltaTime);


                        //　パーティクルを消していく
                        ParticleLifetime -= 3.0f * Time.deltaTime;

                        var Particlemain = PlayerParticleSystem.main;
                        Particlemain.startLifetime = ParticleLifetime;

                        if (ParticleLifetime < 0)
                        {
                            sceneChange.LoadScene(_selectScene);
                            _isChanging = true;
                        }
                    }
                }
            }
            //Audio.Play();
        }
        
    }

    public void SelectScene(string value)
    {
        _selectScene = value;
    }

    public void SelectedStage(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            //Debug.Log(_selectScene);
            if (_selectScene != null && !_isChanging)
            {
                anim.SetBool("accumulate", true);
                zoom.Select = true; // 選択状態にする
             
            }
            else
            {
                //Debug.Log("しーんがないです");
            }

        }
    }
}