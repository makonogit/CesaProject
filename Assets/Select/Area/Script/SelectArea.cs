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
    private string _returnName;

    [SerializeField]
    private List<Transform> _positions;// 各エリアの位置

    private SceneChange scene;// ロードシーン関数を使うため

    [SerializeField]
    private Animator _next;
    //private float _nextUiTime = 0.3f;
    //private float _nowNextUiTime;
    [SerializeField]
    private Animator _prev;
    //private float _nowPrevUiTime;
    [SerializeField, Header("アイコン")]
    private List<Transform> _stageIcon;
  
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float _diplayTime;
    //[SerializeField]
    private float _diplayNowTime;
    //[SerializeField]
    private float _ratio;
    //[SerializeField]
    private bool _start;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private SetStage setmanager;
    [SerializeField] StageManager stageManager;

    private EdgeCollider2D HorizonLimit;
    private Vector2 OldLimitpoint;
    
    public bool LeftMove = false;
    
    public bool RightMove = false;

    public bool PlayerLeftMove = false;

    // Playerの移動
    [SerializeField] private Transform Playertrans;

    [SerializeField] private CrossFadeBGM _crossFadeBGM;

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        //--------------------------------------
        //初期化
        setmanager = new SetStage();
        _nowArea = setmanager.GetAreaNum();// ※おそらくデータを読み込む処理に変わる
        _nextArea = setmanager.GetAreaNum();// ※おそらくデータを読み込む処理に変わる
        _max = _positions.Count - 1;
        _startPos =  new Vector3(-11, 4.2f, 0);
        _endPos = new Vector3(-6.5f, 4.2f, 0);
        _diplayNowTime = 0.0f;
        _start = true;
        _ratio = 0.0f;
        //_nowNextUiTime = _nextUiTime;
        //_nowPrevUiTime = _nextUiTime;
        //--------------------------------------
        //SceneChangeの取得
        scene = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (scene == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
        this.transform.position = new Vector3(_positions[_nowArea].position.x, _positions[_nowArea].position.y, transform.position.z);

        HorizonLimit = GameObject.Find("HorizonLimit").GetComponent<EdgeCollider2D>();
        List<Vector2> point = new List<Vector2>(2);
        point.Add(new Vector2(-9.0f + (36.0f * _nowArea), HorizonLimit.points[0].y));
        point.Add(new Vector2(27.0f + (36.0f * _nowArea), HorizonLimit.points[1].y));
        HorizonLimit.SetPoints(point);

    }

    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //--------------------------------------
        // エリア移動の処理
        PlayerSelectMove();

        ChangeArea();

        DisplayIcon();
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
            _next.SetBool("isPush", true);
            //_nowNextUiTime = 0;

            
            //プレイヤーの座標変更
            GameObject.Find("player").transform.position = stageManager.stage[_nextArea].stage[0].StageObj.transform.position;
            
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
            _prev.SetBool("isPush", true);
            //_nowPrevUiTime = 0;

            if (Playertrans.position.x < HorizonLimit.points[0].x)
            {
                //プレイヤーの座標変更
                GameObject.Find("player").transform.position = stageManager.stage[_nextArea].stage[4].StageObj.transform.position;
            }
            else
            {
                //プレイヤーの座標変更
                GameObject.Find("player").transform.position = stageManager.stage[_nextArea].stage[0].StageObj.transform.position;
            }
        }
    }

    //―次エリアのボタン関数―(公)
    public void OnNext(InputAction.CallbackContext _context) 
    {
        // 押された瞬間
        if(_context.phase == InputActionPhase.Started) 
        {
            NextArea();
            if (_nowArea != _nextArea && !RightMove && !LeftMove && !PlayerLeftMove)
            {
                RightMove = true;
                OldLimitpoint = HorizonLimit.points[1];
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2(HorizonLimit.points[0].x, HorizonLimit.points[0].y));
                point.Add(new Vector2(27.0f + (36.0f * _nextArea), HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
            }
        }
    }

    //―前エリアのボタン関数―(公)
    public void OnPrev(InputAction.CallbackContext _context)
    {
        // 押された瞬間
        if (_context.phase == InputActionPhase.Started)
        {
            PrevArea();
            if (_nowArea != _nextArea && !LeftMove && !RightMove && !PlayerLeftMove)
            {
                LeftMove = true;
                OldLimitpoint = HorizonLimit.points[0];
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2((36.0f * _nextArea) - 9.0f, HorizonLimit.points[0].y));
                point.Add(new Vector2(HorizonLimit.points[1].x, HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
            }
        }
    }

    //―タイトルに戻るのボタン関数―(公)
    public void OnReturn(InputAction.CallbackContext _context)
    {
        // 押された瞬間
        if (_context.phase == InputActionPhase.Started)
        {
            scene.LoadScene(_returnName);
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //―エリア移動関数―(私)
    private void ChangeArea() 
    {
        // 現在地と目標地の距離
        Vector3 Distance = new Vector3(10, 10, -10);

        //_next.SetBool("isPush", (_nowNextUiTime <= _nextUiTime));
        //_prev.SetBool("isPush", (_nowPrevUiTime <= _nextUiTime));

        // エリアを移動す時
        if (_nowArea != _nextArea)// 現在と次のエリアが違うとき
        {
            if (!PlayerLeftMove)
            {
                // 現在地と目標地の間の座標を求める。
                Vector3 _pos = transform.position * 0.95f;
                _pos += new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z) * (1.0f - 0.95f);

                // 現在地の更新
                this.transform.position = _pos;
                // 現在地と目標地の距離を入れる。
                Distance = _pos - new Vector3(_positions[_nextArea].position.x,_positions[_nextArea].position.y, transform.position.z);
            }
            else
            {
                // 現在地と目標地の間の座標を求める。
                Vector3 _pos = transform.position * 0.95f;
                _pos += new Vector3(_positions[_nextArea].position.x + 18.0f, _positions[_nextArea].position.y, transform.position.z) * (1.0f - 0.95f);

                // 現在地の更新
                this.transform.position = _pos;
                // 現在地と目標地の距離を入れる。
                Distance = _pos - new Vector3(_positions[_nextArea].position.x + 18.0f, _positions[_nextArea].position.y, transform.position.z);
            }
        }
        
        // エリア移動完了
        if (Distance.magnitude <= 0.01f) // 距離が近かったら
        {
            _next.SetBool("isPush", false);
            _prev.SetBool("isPush", false);
            // 現在地の更新
            Vector3 _pos = new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z);
            this.transform.position = _pos;
            // 現在のエリアと次のエリアを同じにする。
            _nowArea = _nextArea;
            _start = true;

            // 二宮追加
            // クロスフェードの準備
            _crossFadeBGM.PreXFadeBGM();

            //　カメラの端を調整
            if (LeftMove)
            {
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2(HorizonLimit.points[0].x, HorizonLimit.points[0].y));
                point.Add(new Vector2(OldLimitpoint.x, HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);

                LeftMove = false;
            }
            
            if (PlayerLeftMove)
            {
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2(HorizonLimit.points[0].x, HorizonLimit.points[0].y));
                point.Add(new Vector2(OldLimitpoint.x, HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
                PlayerLeftMove = false;
            }

            if (RightMove)
            {
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2(OldLimitpoint.x, HorizonLimit.points[0].y));
                point.Add(new Vector2(HorizonLimit.points[1].x, HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
                OldLimitpoint = Vector2.zero;

                RightMove = false;
            }
        
        }
        //_nowNextUiTime += Time.deltaTime;        _nowPrevUiTime += Time.deltaTime;
    }

    private void DisplayIcon() 
    {
        if (_start && _nowArea == _nextArea) 
        {
            _stageIcon[_nextArea].localPosition = (_startPos * (1 - _ratio)) + _endPos * _ratio;
            _ratio += _speed * Time.deltaTime;
        }
        if (_ratio >= 1) 
        {
            _start = false;
            _diplayNowTime += Time.deltaTime;
        }else if (_ratio <= 0) 
        {
            _start = false;
            _ratio = 0;
            _diplayNowTime = 0;
        }


        if (_diplayNowTime >= _diplayTime) 
        {
            _stageIcon[_nextArea].localPosition = (_startPos * (1 - _ratio)) + _endPos * _ratio;
            _ratio -= _speed * Time.deltaTime;
        }

        if (_nowArea != _nextArea && (_start || _diplayNowTime > 0)) 
        {
            _stageIcon[_nowArea].localPosition = (_startPos * (1 - _ratio)) + _endPos * _ratio;
            _ratio -= _speed * Time.deltaTime * 2f;
        }
    }

    // プレイヤーの移動によって画面移動
    private void PlayerSelectMove()
    {
        // 右移動
        if (Playertrans.position.x > HorizonLimit.points[1].x)
        {
            NextArea();
            if (_nowArea != _nextArea && !RightMove && !LeftMove && !PlayerLeftMove)
            {
                RightMove = true;
                OldLimitpoint = HorizonLimit.points[1];
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2(HorizonLimit.points[0].x, HorizonLimit.points[0].y));
                point.Add(new Vector2(27.0f + (36.0f * _nextArea), HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
            }
        }

        // 左移動
        if(Playertrans.position.x < HorizonLimit.points[0].x)
        {
            PrevArea();
            if (_nowArea != _nextArea && !LeftMove && !RightMove && !PlayerLeftMove)
            {
                //LeftMove = true;
                PlayerLeftMove = true;
                OldLimitpoint = HorizonLimit.points[0];
                List<Vector2> point = new List<Vector2>(2);
                point.Add(new Vector2((36.0f * _nextArea) - 9.0f, HorizonLimit.points[0].y));
                point.Add(new Vector2(HorizonLimit.points[1].x, HorizonLimit.points[1].y));
                HorizonLimit.SetPoints(point);
            }
        }

    } 

}
