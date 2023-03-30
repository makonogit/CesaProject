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

    private void Start()
    {
        _selectScene = null;
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
            if (_selectScene != null)
            {
                SceneManager.LoadScene(_selectScene);
            }
            else
            {
                //Debug.Log("しーんがないです");
            }

        }
    }
}