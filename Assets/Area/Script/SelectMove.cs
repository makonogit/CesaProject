//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SelectMove : MonoBehaviour
{

    public enum SelectPlayerState 
    {
        MOVE,
        STOP,
        FREE_MOVE,
        AREA_CHANGE,
    }

    private Rigidbody2D _rb;
    private Vector2 _move;
    [Header("移動スピード")]
    [SerializeField]
    private float _speed;
    private SelectPlayerState _state;
    [SerializeField]
    private Transform _camPos;
    
    public List<EdgeCollider2D> _ec2d;

    int _nowArea = 0;
    int _nextArea = 0;
    int _nextPoint = 0;
    int _oldPoint = 0;

    private string _selectScene;

    // Use this for initialization
    void Start()
    {
        //--------------------------------------
        // のコンポーネント取得
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2Dコンポーネントを取得できませんでした。");


        State = SelectPlayerState.FREE_MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        if(State == SelectPlayerState.FREE_MOVE) 
        {
            _rb.velocity = _move * _speed * Time.deltaTime;
        }
        if(State == SelectPlayerState.STOP)
        {
            _rb.velocity = new Vector2(0,0);

            Vector2 _vec = new Vector2(0,0);
            if (_oldPoint != _ec2d[_nowArea].edgeCount - 1 && _move.x > 0) 
            {
                _vec = (_ec2d[_nowArea].points[_oldPoint] - _ec2d[_nowArea].points[_oldPoint + 1]);
                _nextPoint = _oldPoint + 1;
            }
            if(_oldPoint <= 0 && _move.x < 0) 
            {
                _vec = (_ec2d[_nowArea].points[_oldPoint] - _ec2d[_nowArea].points[_oldPoint - 1]);
                _nextPoint = _oldPoint - 1;
            }
            _vec = _vec.normalized - _move;
            if (_vec.magnitude < 1) 
            {
                State = SelectPlayerState.MOVE;
            }

        }
        if(State == SelectPlayerState.MOVE) 
        {
            Vector2 _vec2 = new Vector2(transform.position.x , transform.position.y) - _ec2d[_nowArea].points[_nextPoint];
            if (_vec2.magnitude == 0) 
            {
                _vec2 = _ec2d[_nowArea].points[_oldPoint] - _ec2d[_nowArea].points[_nextPoint];
                transform.position += new Vector3(_vec2.normalized.x, _vec2.normalized.y, 0);
            }
            

        }
        if(State == SelectPlayerState.AREA_CHANGE) 
        {

        }
    }
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
}