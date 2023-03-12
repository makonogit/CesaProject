//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：エリア移動とその他ボタンの処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectArea : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    public int _nowArea;// 現在のエリア
    public int _nextArea;// 次に移動するエリア

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    private int _min = 0;// エリアの最小値
    private int _max = 4;// エリアの最大値

    [SerializeField]
    public Object _returnScene;// シーンチェンジ用のオブジェ

    [SerializeField]
    private List<Transform> _positions;// 各エリアの位置

    private SceneChange scene;// ロードシーン関数を使うため

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        //--------------------------------------
        //初期化
        _nowArea = 0;// ※おそらくデータを読み込む処理に変わる
        _nextArea = 0;// ※おそらくデータを読み込む処理に変わる
        _max = _positions.Count - 1;
        //--------------------------------------
        //SceneChangeの取得
        scene = GetComponent<SceneChange>();
        if (scene == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
    }

    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //--------------------------------------
        // エリア移動の処理
        ChangeArea();
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //―次のエリアを設定する関数―(公)
    public void NextArea()
    {
        if (_nowArea == _nextArea)// エリア移動中ではないなら
        {
            _nextArea++;
            // 予期しない値にならないよう制限する
            _nextArea = Mathf.Clamp(_nextArea, _min, _max);
        }
    }

    //―前のエリアを設定する関数―(公)
    public void PrevArea()
    {
        if (_nowArea == _nextArea)// エリア移動中ではないなら
        {
            _nextArea--;
            // 予期しない値にならないよう制限する
            _nextArea = Mathf.Clamp(_nextArea, _min, _max);
        }
    }

    //―次エリアのボタン関数―(公)
    public void OnNext(InputAction.CallbackContext _context) 
    {
        // 押された瞬間
        if(_context.phase == InputActionPhase.Started) 
        {
            NextArea();
        }
    }

    //―前エリアのボタン関数―(公)
    public void OnPrev(InputAction.CallbackContext _context)
    {
        // 押された瞬間
        if (_context.phase == InputActionPhase.Started)
        {
            PrevArea();
        }
    }

    //―タイトルに戻るのボタン関数―(公)
    public void OnReturn(InputAction.CallbackContext _context)
    {
        // 押された瞬間
        if (_context.phase == InputActionPhase.Started)
        {
            scene.LoadScene(_returnScene.name);
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //―エリア移動関数―(私)
    private void ChangeArea() 
    {
        // 現在地と目標地の距離
        Vector3 Distance = new Vector3(10, 10, -10);

        // エリアを移動す時
        if (_nowArea != _nextArea)// 現在と次のエリアが違うとき
        {
            // 現在地と目標地の間の座標を求める。
            Vector3 _pos = transform.position * 0.95f;
            _pos += new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z) * (1.0f - 0.95f);
            // 現在地の更新
            this.transform.position = _pos;
            // 現在地と目標地の距離を入れる。
            Distance = _pos - new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z);
        }
        
        // エリア移動完了
        if (Distance.magnitude <= 0.01f) // 距離が近かったら
        {
            // 現在地の更新
            Vector3 _pos = new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z);
            this.transform.position = _pos;
            // 現在のエリアと次のエリアを同じにする。
            _nowArea = _nextArea;
        }
    }
}
