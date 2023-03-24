//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：釘に乗っているとひびが伸びる処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NailAddCrack : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    // ナシ

    //-----------------------------------------------------------------
    //―秘匿変数―(私)
    [SerializeField]
    private float _createTime;
    [Header("ひびのobj")]
    [SerializeField]
    private GameObject _crackObject;
    [Header("分割する数の範囲(X～Y)")]
    [SerializeField]
    private Vector2Int _divisionNum;
    [Header("ギザギザの範囲(0.0f～)")]
    [SerializeField]
    private Vector2 _rangeNum;
    private bool _onPlayer;
    [SerializeField]
    private List<Vector2> _edgePoints;// 辺の座標リスト
    [SerializeField]
    private List<GameObject> _cracks;// ひびのオブジェクトリスト
    private Vector2Int _addCrackNow;
    private bool _createCarck;
    private float _nowTime;
    private int _addCrackCount;

    private NailStateManager _stateManager;
    private NailStateManager.NailState _state;
    private NailStateManager.NailState _oldState;
    private BoxCollider2D _boxCollider2D;
    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        // 初期化
        _onPlayer = false;
        _nowTime = 0.0f;
        _addCrackNow = new Vector2Int(0, 0);

        //--------------------------------------
        //NailStateManagerを取得  
        _stateManager = GetComponentInParent<NailStateManager>();
        if (_stateManager == null) Debug.LogError("NailStateManagerのコンポーネントを取得できませんでした。");
        //--------------------------------------
        //BoxCollider2Dを取得  
        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null) Debug.LogError("BoxCollider2Dのコンポーネントを取得できませんでした。");
        _boxCollider2D.enabled = false;
    }

    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        _state = _stateManager.GetState();
        _boxCollider2D.enabled = !(_state == NailStateManager.NailState.THROW);
        if (_state != _oldState && _oldState == NailStateManager.NailState.THROW)
        {
            _edgePoints.Add(this.transform.position);
        }
        if (_onPlayer && _state != NailStateManager.NailState.THROW) 
        {
            if (!_createCarck) 
            {
                AddCrack();
                AddBack();
                _nowTime = 0.0f;
                _createCarck = true;
                
            }
            if (_nowTime >= _createTime && _createCarck && _addCrackCount > 0) 
            {
                // 前表示
                if (_addCrackNow.x >= 0)
                {
                    _cracks[_addCrackNow.x].SetActive(true);// 表示
                    _addCrackNow = new Vector2Int(_addCrackNow.x - 1, _addCrackNow.y);// カウントを減らす
                    _addCrackCount--;
                }
                // 後ろ表示
                if (_addCrackNow.y < _cracks.Count)
                {
                    _cracks[_addCrackNow.y].SetActive(true);// 表示
                    _addCrackNow = new Vector2Int(_addCrackNow.x, _addCrackNow.y + 1);// カウントを減らす
                    _addCrackCount--;
                }
                // リセット
                _nowTime = 0.0f;
                if (_addCrackCount <= 0)
                {
                    _addCrackCount = 0;
                    _createCarck = false;
                }
            }

            _nowTime += Time.deltaTime;
        }
        _oldState = _state;
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "Player")
        {
            _onPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _onPlayer = false;
        }
    }

    //-------------------------------------------------------
    //―ひびの前方追加関数―(私)
    private void AddCrack()
    {
        // 方向決定(仮想釘の設定)
        Vector2 _vNailVec = new Vector2(1, 0);
        // 方向と距離でレイであたり判定
        RaycastHit2D hit = Physics2D.Raycast(_edgePoints[0], _vNailVec.normalized, 1.5f, 3);
        //if (hit) Debug.Log("前" + hit.collider.gameObject.name);
        if (hit && _edgePoints.Count >1 )
        {
            //Debug.Log("前"+ hit.collider.tag);
            _vNailVec = hit.point;
        }
        else
        {
            _vNailVec = _edgePoints[0] + (_vNailVec.normalized * 1.5f);
            // 設定
            _edgePoints.Insert(0, _vNailVec);
            // 二つの釘から垂直な角度を求める
            //Vector2 _vec = _edgePoints[1] - _edgePoints[0];
            //float _angle = Mathf.Atan2(_vec.y, _vec.x);
            //_angle += (90 * Mathf.Deg2Rad);
            // 方向ベクトル
            Vector2 _verticalVec = new Vector2(0, 1);

            // 分割数を決定する
            int _division = Random.Range(_divisionNum.x, _divisionNum.y);
            //--------------------------------------
            // 分割する頂点分繰り返す
            for (int j = 1; j < _division; j++)
            {
                //奇数なら－偶数なら＋
                float _odd = (j % 2 != 0 ? -1.0f : 1.0f);
                // 割合を求める
                float _percent = (float)j / ((float)_division);
                // 間の座標を求める
                Vector2 _pos = _edgePoints[0] * (1.0f - _percent);
                _pos += _edgePoints[j] * _percent;
                _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
                // 間を追加
                _edgePoints.Insert(j, _pos);
                //Debug.Log("TRUE");
            }
            //--------------------------------------
            // 2つ頂点の間にポイントを置く// 0～_division - 1
            for (int i = 0; i < _division; i++)
            {
                // 中間座標を求める
                Vector2 _center = (_edgePoints[i] + _edgePoints[i + 1]) / 2;
                Vector3 _point = new Vector3(_center.x, _center.y, 0);

                // リストに追加
                // 呼び出し
                _cracks.Insert(i, Instantiate(_crackObject, _point, Quaternion.identity));
                // 非表示
                _cracks[i].SetActive(false);

                // 二つの釘から垂直な角度を求める
                Vector2 _vector = _edgePoints[i] - _edgePoints[i + 1];
                float _angle2 = Mathf.Atan2(_vector.y, _vector.x) * Mathf.Rad2Deg;
                // 角度設定
                _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle2);
                // サイズ設定
                _cracks[i].transform.localScale = new Vector3(_vector.magnitude, _cracks[i].transform.localScale.y, _cracks[i].transform.localScale.z);
                _addCrackCount++;
            }
            // 非表示の場所を記録
            _addCrackNow = new Vector2Int(_division - 1, 0);
        }

    }
    //-------------------------------------------------------
    //―ひびの後方追加関数―(私)
    private void AddBack()
    {
        int Last = _edgePoints.Count - 1;
        // 方向決定(仮想釘の設定)
        Vector2 _vNailVec = new Vector2(-1, 0);
        // 方向と距離でレイであたり判定
        RaycastHit2D hit = Physics2D.Raycast(_edgePoints[Last], _vNailVec.normalized, 1.5f, 3);
        //if (hit) Debug.Log("後" + hit.collider.gameObject.tag);
        if (hit && hit.collider.name != this.name)
        {
            //Debug.Log("後" + hit.transform.name);
            _vNailVec = hit.point;
        }
        else
        {
            _vNailVec = _edgePoints[_edgePoints.Count - 1] + (_vNailVec.normalized * 1.5f);

            // 設定
            _edgePoints.Add(_vNailVec);
            // 二つの釘から垂直な角度を求める
            //Vector2 _vec = _edgePoints[1] - _edgePoints[0];
            //float _angle = Mathf.Atan2(_vec.y, _vec.x);
            //_angle += (90 * Mathf.Deg2Rad);
            // 方向ベクトル
            Vector2 _verticalVec = new Vector2( 0, 1);

            // 分割数を決定する
            int _division = Random.Range(_divisionNum.x, _divisionNum.y);
            //--------------------------------------
            // 分割する頂点分繰り返す
            for (int j = 1; j < _division; j++)
            {
                //奇数なら－偶数なら＋
                float _odd = (j % 2 != 0 ? -1.0f : 1.0f);
                // 割合を求める
                float _percent = (float)j / ((float)_division);
                // 間の座標を求める
                Vector2 _pos = _edgePoints[Last] * (1.0f - _percent);
                _pos += _edgePoints[_edgePoints.Count - 1] * _percent;
                _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
                // 間を追加
                _edgePoints.Insert(_edgePoints.Count - 1, _pos);
                //Debug.Log("TRUE");
            }
            //--------------------------------------
            // 2つ頂点の間にポイントを置く// Last～Cont - 1
            for (int i = Last; i < _edgePoints.Count - 1; i++)
            {
                // 中間座標を求める
                Vector2 _center = (_edgePoints[i] + _edgePoints[i + 1]) / 2;
                Vector3 _point = new Vector3(_center.x, _center.y, 0);

                // リストに追加
                // 呼び出し
                _cracks.Add(Instantiate(_crackObject, _point, Quaternion.identity));
                // 非表示
                _cracks[i].SetActive(false);

                // 二つの釘から垂直な角度を求める
                Vector2 _vector = _edgePoints[i] - _edgePoints[i + 1];
                float _angle2 = Mathf.Atan2(_vector.y, _vector.x) * Mathf.Rad2Deg;
                // 角度設定
                _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle2);
                // サイズ設定
                _cracks[i].transform.localScale = new Vector3(_vector.magnitude , _cracks[i].transform.localScale.y, _cracks[i].transform.localScale.z);
                _addCrackCount++;
            }
            // 非表示の場所を記録
            _addCrackNow = new Vector2Int(_addCrackNow.x, Last);
        }


    }
}