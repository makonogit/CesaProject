//----------------------------------------------------------
// �S���ҁF��{��
// ���e  �F�^�C�g���V�[���ɑJ��
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitleScene : MonoBehaviour
{
    public void GoTitle()
    {
        // �^�C�g���ɍs��
        SceneManager.LoadScene("TitleScene");
    }
}
