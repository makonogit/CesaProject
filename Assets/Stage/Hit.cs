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
   
    //---------------------------------------------------------
    //* 初期化処理 *
    //---------------------------------------------------------
    private void Start()
    {

    }


    //---------------------------------------------------------
    //* 更新処理 *
    //---------------------------------------------------------
    void Update()
    {

    }


    //----------------------------------------------------------
    // * 当たり判定の処理 *
    //----------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collider)
    {
      
        if (collider.gameObject.CompareTag("Goal"))
        {
            collider.gameObject.SetActive(false);

            // ゴールオブジェクトに当たったらクリア画面を描画する
            SceneManager.LoadScene("ClearScene");
        }
    }
}
