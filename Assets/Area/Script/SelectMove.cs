//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：SelectSceneのプレイヤーの移動処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SelectMove : MonoBehaviour
{

    //-----------------------------------------------------------------
    //―公開変数―(公)

    public enum SelectPlayerState 
    {
        MOVE,
        STOP,
        FREE_MOVE,
        AREA_CHANGE,
    }
    
    public int _nextPoint = 0;
    public int _oldPoint = 0;

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    private Rigidbody2D _rb;
    private Vector2 _move;
    [Header("移動スピード")]
    [SerializeField]
    private float _speed;
    [Header("ひびの移動スピード")]
    [SerializeField]
    private float _crackSpeed =5;
    private SelectPlayerState _state;
    [SerializeField]
    private Transform _camPos;

    private int _stageNum;

    [SerializeField]
    private List<GameObject> _areas;
    [SerializeField]
    private List<StagesManager> _stageManagers;
    [SerializeField]
    private List<EdgeCollider2D> _ec2d;

    private string _selectScene;

    [SerializeField]
    private GameObject _mainCam;
    private SelectArea _selectArea;

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        //--------------------------------------
        // Rigidbody2Dのコンポーネント取得
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2Dのコンポーネントを取得できませんでした。");
        //--------------------------------------
        // 各エリアのコライダーとステージマネージャーのコンポーネント取得
        for (int i =0;i< _areas.Count; i++) 
        {
            _stageManagers.Add(_areas[i].GetComponent<StagesManager>());
            if (_stageManagers[i] == null) Debug.LogError(_areas[i].name +"StagesManagerのコンポーネントを取得できませんでした。");
            _ec2d.Add(_areas[i].GetComponent<EdgeCollider2D>());
            if (_ec2d[i] == null) Debug.LogError(_areas[i].name + "EdgeCollider2Dのコンポーネントを取得できませんでした。");
        }
        //--------------------------------------
        // 状態の設定
        State = SelectPlayerState.STOP;

        //--------------------------------------
        // SelectAreaのコンポーネント取得
        if (_mainCam == null) Debug.LogError("_mainCamのオブジェクトが設定されていません");
        _selectArea = _mainCam.GetComponent<SelectArea>();
        if (_selectArea == null) Debug.LogError("SelectAreaのコンポーネントを取得できませんでした。");
        _stageManagers[_selectArea._nextArea]._Start = true;
    }


    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {

        if(State == SelectPlayerState.FREE_MOVE) 
        {
            _rb.velocity = _move * _speed * Time.deltaTime;
        }
        if(State == SelectPlayerState.STOP)
        {
            _rb.velocity = new Vector2(0,0);

            if (_stageManagers[_selectArea._nowArea]._Start) 
            {
                NextPoint();
            }

            if (_stageManagers[_selectArea._nowArea].AreaClear) 
            {
                State = SelectPlayerState.FREE_MOVE;
            }

        }
        if(State == SelectPlayerState.MOVE) 
        {
            Vector2 _vec2 = new Vector2(transform.position.x, transform.position.y) - TransPos(_ec2d[_selectArea._nowArea].points[_nextPoint], _areas[_selectArea._nowArea].transform);
            if (_vec2.magnitude >= 0.125) 
            {
                _vec2 = TransPos(_ec2d[_selectArea._nowArea].points[_nextPoint], _areas[_selectArea._nowArea].transform);
                _vec2 -= new Vector2(transform.position.x, transform.position.y);

                transform.position += new Vector3(_vec2.normalized.x, _vec2.normalized.y, 0) * _crackSpeed * Time.deltaTime;
            }
            else 
            {
                int old = _nextPoint;
                _nextPoint += _nextPoint - _oldPoint;
                if (_nextPoint < 0 || _ec2d[_selectArea._nowArea].edgeCount <=  _nextPoint) 
                {
                    State = SelectPlayerState.STOP;
                }
                
                _oldPoint = old;
                _oldPoint = Mathf.Clamp(_oldPoint, 0, _ec2d[_selectArea._nowArea].edgeCount - 1);
                _nextPoint = Mathf.Clamp(_nextPoint, 0, _ec2d[_selectArea._nowArea].edgeCount - 1);
                
            }
            

        }
        if(State == SelectPlayerState.AREA_CHANGE) 
        {

        }

        if(_selectArea._nowArea != _selectArea._nextArea) 
        {
            // クリアしていたら
            if (_stageManagers[_selectArea._nextArea].AreaClear)
            {
                State = SelectPlayerState.FREE_MOVE;
            }
            else 
            {

                if (_stageManagers[_selectArea._nowArea].AreaClear && !_stageManagers[_selectArea._nextArea]._Start) 
                {
                    _stageManagers[_selectArea._nextArea]._Start = true;
                }
                State = SelectPlayerState.STOP;
            }

            _oldPoint = 0;
            _nextPoint = 0 ;
            transform.position = new Vector3(-8.0f, 2.0f);
            transform.position += new Vector3(1.0f,0.0f) * 18 * _selectArea._nextArea;
        }
        //Debug.Log(State);
    }
    //-----------------------------------------------------------------
    //★★公開関数★★(公)
    public SelectPlayerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }
    public void OnMove(InputAction.CallbackContext _context)
    {
        _move = _context.ReadValue<Vector2>();
    }

    public void SelectScene (string value)
    {
        _selectScene = value;
    }
    
    public int StageNumber 
    {
        set 
        {
            _stageNum = value;
        }
    }

    public void SelectedStage(InputAction.CallbackContext _context) 
    {
        if (_context.phase == InputActionPhase.Started)
        {
            if(_selectScene != null) 
            {
                SceneManager.LoadScene(_selectScene);
            }
            else 
            {
                Debug.Log("しーんがないです");
            }
            
        }
    }
    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)
    private void NextPoint()
    {
        Vector2 _chack = transform.position;
        _chack -= TransPos(_ec2d[_selectArea._nowArea].points[_nextPoint], _areas[_selectArea._nowArea].transform);
        Vector2 vector2 = TransPos(_ec2d[_selectArea._nowArea].points[_nextPoint], _areas[_selectArea._nowArea].transform);
        vector2 -= TransPos(_ec2d[_selectArea._nowArea].points[_oldPoint], _areas[_selectArea._nowArea].transform);
        if (_chack.magnitude < vector2.magnitude / 2)
        {
            transform.position = TransPos(_ec2d[_selectArea._nowArea].points[_nextPoint], _areas[_selectArea._nowArea].transform);
            _oldPoint = _nextPoint;
            _oldPoint = Mathf.Clamp(_oldPoint, 0, _ec2d[_selectArea._nowArea].edgeCount - 1);
        }

        bool _flg = false;
        Vector2 _vec = new Vector2(100, 100);

        if (_oldPoint != _ec2d[_selectArea._nowArea].edgeCount - 1 && _move.x > 0 && _stageNum <= _stageManagers[_selectArea._nowArea]._clearCount)
        {
            _vec = TransPos(_ec2d[_selectArea._nowArea].points[_oldPoint + 1], _areas[_selectArea._nowArea].transform);
            _vec -= TransPos(_ec2d[_selectArea._nowArea].points[_oldPoint], _areas[_selectArea._nowArea].transform);
            _nextPoint = _oldPoint + 1;
            _flg = true;

        }
        if (_oldPoint != 0 && _move.x < 0)
        {
            _vec = TransPos(_ec2d[_selectArea._nowArea].points[_oldPoint - 1], _areas[_selectArea._nowArea].transform);
            _vec -= TransPos(_ec2d[_selectArea._nowArea].points[_oldPoint], _areas[_selectArea._nowArea].transform);
            _nextPoint = _oldPoint - 1;
            _flg = true;

        }

        _vec = _vec.normalized - _move;

        if (_vec.magnitude < 0.25f && _flg == true)
        {
            State = SelectPlayerState.MOVE;
        }
    }

    private Vector2 TransPos(Vector2 _Edge, Transform _Trans)
    {
        Vector2 _vec = new Vector2(_Edge.x * _Trans.localScale.x, _Edge.y * _Trans.localScale.y) + new Vector2(_Trans.position.x, _Trans.position.y);
        return _vec;
    }

}
