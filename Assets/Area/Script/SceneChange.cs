//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{

    public void LoadScene(string _str) 
    {
        Debug.Log(_str+"シーンをロードします。");
        SceneManager.LoadScene(_str);
    }
}