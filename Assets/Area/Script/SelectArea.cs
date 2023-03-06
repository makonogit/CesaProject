//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class SelectArea : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    // ナシ

    //-----------------------------------------------------------------
    //―秘匿変数―(私)
    [SerializeField]
    public int _nowArea;
    [SerializeField]
    public int _nextArea;

    private int _min = 0;
    private int _max = 4;

    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _prevButton;

    [SerializeField]
    private List<Transform> _positions;


    // Use this for initialization
    void Start()
    {
        _nowArea = 0;
        _nowArea = Mathf.Clamp(_nowArea, _min, _max);
        _nextArea = 0;
        _nextArea = Mathf.Clamp(_nextArea, _min, _max);
        _max = _positions.Count - 1;
        
    }
     void Update()
    {
        Vector3 aaa = new Vector3 (10,10,-10);
        if (_nowArea != _nextArea)
        {
            Vector3 _pos = transform.position * 0.95f;
            _pos += new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z) * (1.0f - 0.95f);
            this.transform.position = _pos;
            aaa = _pos - new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z);
        }
        
        if (aaa.magnitude <= 0.01f) 
        {
            Vector3 _pos =  new Vector3(_positions[_nextArea].position.x, _positions[_nextArea].position.y, transform.position.z);
            this.transform.position = _pos;
            _nowArea = _nextArea;
        }
        
    }

    public void NextArea()
    {
        if (_nowArea == _nextArea)
        {
            _nextArea++;
            _nextArea = Mathf.Clamp(_nextArea, _min, _max);
        }
    }
    public void PrevArea()
    {
        if (_nowArea == _nextArea)
        {
            _nextArea--;
            _nextArea = Mathf.Clamp(_nextArea, _min, _max);
        }
    }

    public void OnNext(InputAction.CallbackContext _context) 
    {
        if(_context.phase == InputActionPhase.Started) 
        {
            _nextButton.onClick.Invoke();
        }
    }

    public void OnPrev(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            _prevButton.onClick.Invoke();
        }
    }
}
