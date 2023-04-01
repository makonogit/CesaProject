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

    private void Start()
    {
        _selectScene = null;
        sceneChange = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (sceneChange == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
        _isChanging = false;

        se = GameObject.Find("SE");
        Audio = se.GetComponent<AudioSource>();
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
                Audio.Play();
                
                sceneChange.LoadScene(_selectScene);
                _isChanging = true;
            }
            else
            {
                //Debug.Log("しーんがないです");
            }

        }
    }
}