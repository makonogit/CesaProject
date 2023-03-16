//---------------------------------------------
//担当者：尾花真理子
//内容：当たり判定（ゴール）
//---------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hit : MonoBehaviour
{
    //―追加担当者：中川直登―

    // 入力関係
    private PlayerInputManager _playerInputManager;
    private InputTrigger _inputTrigger;
    [SerializeField]
    private bool _isHitGoal = false;
    //――――――――――――

    //---------------------------------------------------------
    //* 初期化処理 *
    //---------------------------------------------------------
    private void Start()
    {
        //―追加担当者：中川直登―

        // 初期化
        _isHitGoal = false;
        //--------------------------------------
        // PlayerInputManagerの取得

        // PlayerInputManagerを探す
        GameObject _inputManager = GameObject.Find("PlayerInputManager");
        // エラー文
        if (_inputManager == null) Debug.LogError("PlayerInputManagerを見つけることが出来ませんでした。");

        // コンポーネントの取得
        _playerInputManager = _inputManager.GetComponent<PlayerInputManager>();
        // エラー文
        if (_playerInputManager == null) Debug.LogError("PlayerInputManagerのコンポーネントを取得できませんでした。");

        // コンポーネントの取得
        _inputTrigger = _inputManager.GetComponent<InputTrigger>();
        // エラー文
        if (_inputTrigger == null) Debug.LogError("InputTriggerのコンポーネントを取得できませんでした。");
        //――――――――――――
    }


    //---------------------------------------------------------
    //* 更新処理 *
    //---------------------------------------------------------
    void Update()
    {
        //―追加担当者：中川直登―
        if(_isHitGoal == true) 
        {
            // GetHammerTriggerが今使えないのでGetHammerで代用しています
            if (_playerInputManager.GetHammer() )
            {
                //Debug.Log("押しました");
                // ゴールオブジェクトに当たったらクリア画面を描画する
                SceneManager.LoadScene("SelectScene");
            }
        }
        //――――――――――――
    }


    //----------------------------------------------------------
    // * 当たり判定の処理 *
    //----------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collider)
    {
      
        if (collider.gameObject.CompareTag("Goal"))
        {
            //collider.gameObject.SetActive(false);

            // ゴールオブジェクトに当たったらクリア画面を描画する
            //SceneManager.LoadScene("ClearScene");
            //―追加担当者：中川直登―
            _isHitGoal = true;
        }
    }

}
