//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GiveScene : MonoBehaviour
{
    private GameObject _playerObj;
    private SelectMove _player;
    [SerializeField]
    private Object _scene;

    //private void Start()
    //{
    //    //_playerObj = GameObject.Find("Player(Select)");
    //    //_player = _playerObj.GetComponent<SelectMove>();
    //}

    public string GetScene
    {
        get
        {
            return _scene.name;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _player = collision.gameObject.GetComponent<SelectMove>();
            if (_player.State != SelectMove.SelectPlayerState.AREA_CHANGE)
            {
                //_player.State = SelectMove.SelectPlayerState.STOP;
                _player.SelectScene(GetScene);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _player = collision.gameObject.GetComponent<SelectMove>();
            _player.SelectScene(null);
        }
    }
}