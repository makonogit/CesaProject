//----------------------------------------------------------
// 担当者：二宮怜
// 内容  ：タイトルシーンに遷移
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitleScene : MonoBehaviour
{
    public void GoTitle()
    {
        // タイトルに行く
        SceneManager.LoadScene("TitleScene");
    }
}
