//---------------------------------------------------------
//担当者：中川直登
//内容　：ゲームオーバー時のカメラの動き
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameOverCameraEvent : MonoBehaviour
{
    private Camera _camera;
    private GameOver _playerGO;
    private GameObject _player;
    //private float _nowSize;
    private float _endSize;
    //private float _normalSize;// 通常サイズ

    [SerializeField, Header("アニメーション時間")]
    private float _maxTime = 1.0f;
    [SerializeField, Header("１処理の時間")]
    private float _processTime = 0.1f;
    [SerializeField, Header("アニメーションカーブ")]
    private AnimationCurve Speed;
    private float n;
    private float _posZ;
    private bool _started;
    private bool _end;

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null) Debug.LogError("カメラに追加しましたか？");

        string _name = "player";
        _player = GameObject.Find(_name);
        if (_player == null) Debug.LogError(_name+"が見つかりませんでした。");
        _playerGO = _player.GetComponent<GameOver>();
        if (_playerGO == null) Debug.LogError("GameOverのコンポーネントを取得できませんでした。");

        _started = false;
        _end = false;
        n = 0.0f;
        _posZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_playerGO.IsGameOver&&!_started) 
        {
            StartCoroutine("ZoomCamera");
            _started = true;
        }
        if (_end) 
        {
            _camera.orthographicSize = _endSize;
        }
    }
    
    // 非同期処理
    IEnumerator ZoomCamera() 
    {
        while (true) 
        {
            _camera.orthographicSize = Speed.Evaluate(n);
            _camera.transform.position = _camera.transform.position * (1 - n) + _player.transform.position * n;
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _posZ);// z座標を戻す。
            // 秒待つ
            n += _processTime;
            yield return new WaitForSeconds (_processTime);

            // 1まで行くとループを抜ける
            if (n > _maxTime) 
            {
                _endSize = Speed.Evaluate(n);
                _end = true;
                yield break;
            }
            
        }
    }

}