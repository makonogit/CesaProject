//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StagesManager : MonoBehaviour
{
    [SerializeField]
    private bool _areaClear = false;
    [SerializeField]
    private GameObject _crackObj;
    [SerializeField]
    private List<GameObject> _stages;
    [SerializeField]
    private List<bool>  _stageClear;

    private EdgeCollider2D _edgeCollider2D;

    [SerializeField]
    private List<GameObject> _cracks;

    [SerializeField]
    private GameObject _playerObj;
    //-----------------------------------------------------------------
    //―初期化処理―
    void Awake()// インスタンス直後(Startより先に呼ばれる)
    {
        
        
    }
    //-----------------------------------------------------------------
    //―設定処理―
    void OnValidate()// Inspectorを触った時の処理
    {
        // リセット
        for (; 0 < _stageClear.Count;) _stageClear.RemoveAt(0);
        // セット
        for (int i = 0; i < _stages.Count; i++) _stageClear.Add(false);
        

       
    }
    // Use this for initialization
    void Start()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        for (; 0 < _cracks.Count;) _cracks.RemoveAt(0);
        for (int i = 0; i < _edgeCollider2D.edgeCount; i++)
        {
            _cracks.Add(Instantiate(_crackObj));
            _cracks[i].name = (name + "Crack" + i);
            Vector3 _pos = _edgeCollider2D.points[i] + _edgeCollider2D.points[i + 1];
            _pos = new Vector3(_pos.x * transform.localScale.x, _pos.y * transform.localScale.y, _pos.z);
            _cracks[i].transform.position =this.transform.position + _pos / 2;
            // 二つの釘から垂直な角度を求める
            Vector2 _vec = new Vector2(_edgeCollider2D.points[i].x * transform.localScale.x, _edgeCollider2D.points[i].y * transform.localScale.y);
            _vec -= new Vector2(_edgeCollider2D.points[i + 1].x * transform.localScale.x, _edgeCollider2D.points[i + 1].y * transform.localScale.y);
            float _angle = Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg ;
            _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle);
            _cracks[i].transform.localScale = new Vector3(_vec.magnitude ,_crackObj.transform.localScale.y, _crackObj.transform.localScale.z);
        }
        SelectMove _player;
        _player = _playerObj.GetComponent<SelectMove>();
        _player._ec2d.Add(this._edgeCollider2D);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AreaClear
    {
        get 
        {
            return _areaClear;
        }
    }
}