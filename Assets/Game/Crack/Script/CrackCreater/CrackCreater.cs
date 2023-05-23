﻿//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：与えらた位置からひびを作る
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackCreater : MonoBehaviour
{
    UnityEngine.Rendering.Universal.Light2D _light2D;
    
    //-----------------------------------------------------------------
    //―変数―(公)Accessible variables

    // 状態
    public enum CrackCreaterState
    {
        NONE,       // 何もない
        START,      // 作成開始
        CREATING,   // 作成中
        CRAETED,    // 作成完了
        ADD_CREATE, // 追加作成開始
        ADD_CREATING,// 追加作成中
        //-------追加　菅----------
        ADD_CREATEFORWARD,      // 前方追加
        ADD_CREATEBACK,         // 後方追加
    }
    [System.NonSerialized]// 非表示
    public CrackCreaterState State; // 外部閲覧用

    [System.NonSerialized]// 非表示
    public EdgeCollider2D Edge2D;
    private EdgeCollider2D SandEdge;    //砂との当たり判定用

    //-----------------------------------------------------------------
    //―変数―(私)Inaccessible variables
    [SerializeField]
    private CrackCreaterState _nowState;// 今の状態を入れる変数

    [Header("ひびのobj")]
    [SerializeField]
    private GameObject _crackObject;

    [Header("分割する数の範囲(X～Y)")]
    [SerializeField]
    private Vector2Int _divisionNum;

    [Header("ギザギザの範囲(0.0f～)")]
    [SerializeField]
    private Vector2 _rangeNum;

    [Header("生成時間")]
    [SerializeField]
    private float _createTime;
    private float _nowTime;
    private int _createCount;
    private int _addCrackCount;
    private Vector2Int _addCrackNow;
    private int HitPoint;   //生成中に衝突したpoint

    [SerializeField]
    private List<Vector2> _nailPoints;// 釘の座標リスト
    [SerializeField]
    private List<Vector2> _edgePoints;// 辺の座標リスト
    [SerializeField]
    private List<int> _nailPointCount;// 辺リスト中の釘の番号
    [SerializeField]
    private List<GameObject> _cracks;// ひびのオブジェクトリスト

    [SerializeField]private List<Vector2> _nowEdgePoints = new List<Vector2>(); // 更新中のエッジリスト

    //private Wall_HP_System_Script _WHPSS;

    int layerMask;      //Rayのレイヤーマスク

    [SerializeField, Header("追加するひびの長さ")]
    private float AddLength;

    SetStage stage; //ステージ

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        //--------------------------------------
        // エッジコライダー2Dが入っているか
        Edge2D = GetComponent<EdgeCollider2D>();
        if (Edge2D == null)
        {
            Debug.LogError("EdgeCollider2Dがコンポーネントされてません。");
        }

        //　砂用コライダー　担当：菅
        if (transform.childCount > 0)
        {
            SandEdge = transform.GetChild(0).GetComponent<EdgeCollider2D>();
        }
        //if(SandEdge == null) Debug.LogError("Sand用EdgeCollider2Dがコンポーネントされてません。");


        //--------------------------------------
        // 釘の座標リストに値が入っているか
        if (_nailPoints == null)
        {
            Debug.LogError("釘の座標リストが渡されてません。");
            _nowState = CrackCreaterState.NONE;
        }
        else
        {
            _nowState = CrackCreaterState.START;
        }
        // 状態を共有する
        State = _nowState;

        // WHPSS　-追加
        //GameObject _whpg =GameObject.Find("Wall_Hp_Gauge");
        //_WHPSS = _whpg.GetComponent<Wall_HP_System_Script>();
        //if (_WHPSS == null) 
        //{
        //    Debug.LogError("null");
        //}

        //---------------------------------
        //　layermaskでGroundだけ判定する
        layerMask = 1 << 10 | 1 << 18 | 1 << 8;
        //layerMask = ~layerMask;

        //---------------------------------
        // Light2Dのコンポーネントを取得
        _light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (_light2D == null) Debug.LogError("Light2Dのコンポーネントを取得できませんでした。");
        // 非表示
        _light2D.enabled = false;

        stage = new SetStage();

    }


    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //--------------------------------------
        // 状態が作成開始
        if (_nowState == CrackCreaterState.START)
        {
            EdgeSetting();//エッジの設定
            _createCount = 0;
            
            //_nowState = CrackCreaterState.CREATING;// 確認用
        }

        //--------------------------------------
        // 状態が作成中(演出部分)
        if (_nowState == CrackCreaterState.CREATING)
        {
            CreatingCrack();
        }

        //--------------------------------------
        // 状態が作成開始
        if (_nowState == CrackCreaterState.ADD_CREATEFORWARD)
        {
            AddCreateForward();
            _createCount = 0;
            _nowState = CrackCreaterState.ADD_CREATING;
        }


        //--------------------------------------
        // 状態が作成開始
        if (_nowState == CrackCreaterState.ADD_CREATEBACK)
        {
            AddCreateBack();
            _createCount = 0;
            _nowState = CrackCreaterState.ADD_CREATING;
        }


        //--------------------------------------
        // 状態が作成開始
        if (_nowState == CrackCreaterState.ADD_CREATE)
        {
            AddCreate();
            _createCount = 0;
            _nowState = CrackCreaterState.ADD_CREATING;
        }


        //--------------------------------------
        // 状態が追加作成中(演出部分)
        if (_nowState == CrackCreaterState.ADD_CREATING)
        {
            AddCreating();
        }
        // 状態を共有する
        State = _nowState;
    }


    //-------------------------------------------------------
    //―釘座標リスト設定関数―(公)
    public void SetPointList(List<Vector2> _pointList)
    {
        _nailPoints = _pointList;
    }

    //-------------------------------------------------------
    //―状態設定関数―(公)
    public void SetState(CrackCreaterState _state)
    {
        _nowState = _state;
        State = _nowState;
    }

    //-------------------------------------------------------
    //―状態獲得関数―(公) 追加担当：菅眞心
    public CrackCreaterState　GetState()
    {
        return _nowState;
    }

    //-------------------------------------------------------
    //―エッジ設定関数―(私)
    private void EdgeSetting()
    {
        //--------------------------------------
        // 辺の頂点を設定する
        for (int i = 0; i < _nailPoints.Count; i++)// 釘の数繰り返す
        {
            // 釘の座標を追加
            _edgePoints.Add(_nailPoints[i]);
            
            // 最後の座標でなければ
            if (i != _nailPoints.Count - 1)
            {
                // 方向決定
                Vector2 _vNailVec = _nailPoints[i] - _nailPoints[i + 1];

                // 方向と距離でレイであたり判定
                RaycastHit2D hit = Physics2D.Raycast(_nailPoints[i] + new Vector2(0.01f, 0.01f), _vNailVec.normalized * -1, _vNailVec.magnitude, layerMask);
                //Debug.DrawRay(_nailPoints[i], _vNailVec.normalized * -1, Color.red,  _vNailVec.magnitude, false);
                //Debug.Log("当たりました" +hit.collider.gameObject.name);

                // ステージに当たったら終了する
                if (hit) 
                {
                    break;
                }

                // 分割処理
                DivisionPositionSetting(i);
            }
        }

        //--------------------------------------
        // 2つ頂点の間にポイントを置く
        for (int i = 0; i < _edgePoints.Count - 1; i++)
        {
            // 中間座標を求める
            Vector2 _center = (_edgePoints[i] + _edgePoints[i + 1]) / 2;
            Vector3 _point = new Vector3(_center.x, _center.y, 0);

            // 最後の座標でなければ
            //if (i != _edgePoints.Count - 2)
            //{
            //    // 方向決定
            //    Vector2 _vNailVec = _edgePoints[i] - _edgePoints[i + 1];

            //    // 方向と距離でレイであたり判定
            //    RaycastHit2D hit = Physics2D.Raycast(_edgePoints[i], _vNailVec.normalized * -1, _vNailVec.magnitude, layerMask);
            //    Debug.DrawRay(_edgePoints[i], _vNailVec.normalized * -1, Color.red,  _vNailVec.magnitude, false);


            //    // ステージに当たったら終了する
            //    if (hit)
            //    {
            //        HitPoint = i;
            //        break;
            //    }

            //    //Debug.Log("当たりました" +hit.collider.gameObject.name);

            //}

            // リストに追加
            // 呼び出し
            _cracks.Add(Instantiate(_crackObject, _point, Quaternion.identity, transform));

            // 非表示
            _cracks[i].SetActive(false);

            // 二つの釘から垂直な角度を求める
            Vector2 _vec = _edgePoints[i] - _edgePoints[i + 1];
            float _angle = Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;
            // 角度設定
            _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle);
            // サイズ設定
            _cracks[i].transform.localScale = new Vector3(_vec.magnitude, _cracks[i].transform.localScale.y, _cracks[i].transform.localScale.z);

        }

        // 頂点を設定する
        //Edge2D.SetPoints(_edgePoints);
        //if(SandEdge != null) SandEdge.SetPoints(_edgePoints);

        _nowState = CrackCreaterState.CREATING;
    }

    //-------------------------------------------------------
    //―分割位置設定関数―(私)
    private void DivisionPositionSetting(int _num)
    {
        if (_num == 0)
        {
            _nailPointCount.Add(_num);
        }
        _nailPointCount.Add(_nailPointCount[_num] + 1);

        // 二つの釘から垂直な角度を求める
        Vector2 _vec = _nailPoints[_num + 1] - _nailPoints[_num];
        float _angle = Mathf.Atan2(_vec.y, _vec.x);
        _angle += (90 * Mathf.Deg2Rad);
        // 方向ベクトル
        Vector2 _verticalVec = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));

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
            Vector2 _pos = _nailPoints[_num] * (1.0f - _percent);
            _pos += _nailPoints[_num + 1] * _percent;
            if (j != 1 && j != _division - 1)// 最初と最後以外
            {
                _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
            }
            // 間を追加
            _edgePoints.Add(_pos);
            // _edgePoinsの位置を計算する
            _nailPointCount[_num + 1] += 1;
        }
    }

    //-------------------------------------------------------
    //―ひび演出関数―(私)
    private void CreatingCrack()
    {
        // 時間計算
        _nowTime += Time.deltaTime;
        // 生成時間を越えたら
        if (_nowTime >= _createTime && _createCount < _cracks.Count)
        {
            // 表示
            _cracks[_createCount].SetActive(true);

            // エッジ設定
            _nowEdgePoints.Add(_edgePoints[_createCount]);
            Edge2D.SetPoints(_nowEdgePoints);
            if (SandEdge != null) SandEdge.SetPoints(_nowEdgePoints);

            // WHPSSのHPを減らす-追加
            //_WHPSS.SubHp(_cracks[_createCount].transform.localScale.x);
            // 次へ
            _createCount++;
            // リセット
            _nowTime = 0.0f;
        }
        // 全て表示したら
        if (_createCount == _cracks.Count)
        {
            if (stage.GetAreaNum() == 3) SetLight();
            //_nowState = CrackCreaterState.ADD_CREATE;
            _nowState = CrackCreaterState.CRAETED;
        }
    }

    //-------------------------------------------------------
    //―ひびの追加関数―(私)
    private void AddCreate()
    {
        _addCrackCount = 0;
        //--------------------------------------
        // リストの0以下を追加 
        AddForward();
        //--------------------------------------
        // リストの後ろを追加　
        AddBack();
        // 頂点を再設定する
        //Edge2D.SetPoints(_edgePoints);
        if (SandEdge != null) SandEdge.SetPoints(_edgePoints);
    }

    //-------------------------------------------------------------------
    //　ひびの前方追加関数　追加　菅
    private void AddCreateForward()
    {
        _addCrackCount = 0;

        //--------------------------------------
        // リストの0以下を追加 
        AddForward();

        // 頂点を再設定する
        //Edge2D.SetPoints(_edgePoints);
        SandEdge.SetPoints(_edgePoints);

    }

    //-------------------------------------------------------------------
    //　ひびの後方追加関数　追加　菅
    private void AddCreateBack()
    {
        _addCrackCount = 0;

        //--------------------------------------
        // リストの後ろを追加　
        AddBack();

        // 頂点を再設定する
        //Edge2D.SetPoints(_edgePoints);
        if (SandEdge != null)  SandEdge.SetPoints(_edgePoints);

    }


    //-------------------------------------------------------
    //―ひびの前方追加関数―(私)
    private void AddForward()
    {

        // 方向決定(仮想釘の設定)
        Vector2 _vNailVec = _edgePoints[0] - _edgePoints[1];
        // 方向と距離でレイであたり判定
        RaycastHit2D hit = Physics2D.Raycast(_edgePoints[0], _vNailVec.normalized, AddLength, layerMask);
        //if (hit) Debug.Log("前" + hit.collider.gameObject.name);
        if (hit) 
        {
            //Debug.Log("前"+ hit.collider.tag);
            _vNailVec = hit.point;
        }
        else 
        {
            _vNailVec = _edgePoints[0] + (_vNailVec.normalized * AddLength);
            // 設定
            _edgePoints.Insert(0, _vNailVec);
            _nailPointCount[0]++;
            // 二つの釘から垂直な角度を求める
            Vector2 _vec = _edgePoints[1] - _edgePoints[0];
            float _angle = Mathf.Atan2(_vec.y, _vec.x);
            _angle += (90 * Mathf.Deg2Rad);
            // 方向ベクトル
            Vector2 _verticalVec = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));

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
                if (j != 1 && j != _division - 1)// 最初と最後以外
                {
                    _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
                }
                // 間を追加
                _edgePoints.Insert(j, _pos);
                // _edgePoinsの位置を計算する
                _nailPointCount[0]++;
            }
            // 前に増えた分を足す
            for (int i = 1; i < _nailPointCount.Count; i++)
            {
                _nailPointCount[i] += _nailPointCount[0];
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
                _cracks.Insert(i, Instantiate(_crackObject, _point, Quaternion.identity, transform));

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
        Vector2 _vNailVec = _edgePoints[Last] - _edgePoints[_edgePoints.Count - 2];
        Debug.DrawRay(_edgePoints[Last] + new Vector2(0.01f, 0.01f), _vNailVec.normalized, Color.red, AddLength - 1.0f, false);
        // 方向と距離でレイであたり判定
        RaycastHit2D hit = Physics2D.Raycast(_edgePoints[Last]/* + new Vector2(0.01f,0.01f)*/, _vNailVec.normalized , AddLength - 1.0f, layerMask);
        //if (hit) Debug.Log("後" + hit.collider.gameObject.tag);
        if (hit)
        {
            //Debug.Log("後" + hit.transform.name);
            _vNailVec = hit.point;
        }
        else
        {
            _vNailVec = _edgePoints[_edgePoints.Count - 1] + (_vNailVec.normalized * AddLength);

            // 設定
            _edgePoints.Add(_vNailVec);
            // 二つの釘から垂直な角度を求める
            Vector2 _vec = _edgePoints[1] - _edgePoints[0];
            float _angle = Mathf.Atan2(_vec.y, _vec.x);
            _angle += (90 * Mathf.Deg2Rad);
            // 方向ベクトル
            Vector2 _verticalVec = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));

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
                if (j != 1 && j != _division - 1)// 最初と最後以外
                {
                    _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
                }
                // 間を追加
                _edgePoints.Insert(_edgePoints.Count - 1, _pos);
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
                _cracks.Add(Instantiate(_crackObject, _point, Quaternion.identity, transform));

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
            _addCrackNow = new Vector2Int(_addCrackNow.x, Last);
        }
    }
    //-------------------------------------------------------
    //―ひびの追加作成関数―(私)
    private void AddCreating()
    {
        // 時間計算
        _nowTime += Time.deltaTime;
        // 生成時間を越えたら
        if (_nowTime >= _createTime && _addCrackCount>0)
        {
            // 前表示
            if (_addCrackNow.x > 0) 
            {
                _cracks[_addCrackNow.x].SetActive(true);// 表示
                _addCrackCount --;// カウントを減らす
                _addCrackNow = new Vector2Int(_addCrackNow.x -1 , _addCrackNow.y);// カウントを減らす
            }
            // 後ろ表示
            if (_addCrackNow.y < _cracks.Count) 
            {
                _cracks[_addCrackNow.y].SetActive(true);// 表示
                _nowEdgePoints.Add(_edgePoints[_addCrackNow.y]);
                Edge2D.SetPoints(_nowEdgePoints);
                _addCrackCount--;// カウントを減らす
                _addCrackNow = new Vector2Int(_addCrackNow.x , _addCrackNow.y + 1);// カウントを減らす
            }
            // リセット
            _nowTime = 0.0f;
        }
        // 全て表示したら
        if (_addCrackCount <= 0)
        {
            if (stage.GetAreaNum() == 3)
            {
                SetLight();
            }
            // 状態変更
            _nowState = CrackCreaterState.CRAETED;
        }
    }

    //
    // 関数：SetLight ()
    //
    // 目的：光の形をセットする
    // 
    // コメント：座標は反時計回りにしてください
    // 
    private void SetLight ()
    {
        // 表示
        _light2D.enabled = true;

        // 方向設定
        Vector2 _direction = new Vector2(0.125f, -1);
        
        // 距離設定
        float _distance = 10000f;

        // 右向きか
        if (_isRight)
        {
            RightSideShape(_direction, _distance);
        }
        else // 左向きなら
        {
            LeftSideShape(_direction, _distance);
        }
    }

    //
    // 関数：SetLightShape(Vector2 _pos,Vector2 _direction,float _distance) 
    //
    // 目的：レイを飛ばして当たったとこの位置を返す   
    // 
    private Vector3 SetLightShape(Vector2 _pos,Vector2 _direction,float _distance) 
    {
        // レイを飛ばす
        RaycastHit2D _hit = Physics2D.Raycast(_pos, _direction.normalized, _distance, layerMask);
        // 当たったのがステージの床なら
        if (_hit && _hit.transform.tag == "Ground") return new Vector3(_hit.point.x, _hit.point.y, 0);
        // 当たらなかったとき
        Vector2 resut = _pos + (_direction.normalized * _distance);

        return new Vector3(resut.x, resut.y, 0);
    }

    //
    // 関数：_isRight 
    //
    // 目的：ひびが右向きかを判断する
    // 
    private bool _isRight 
    {
        get 
        {
            if (_edgePoints[0].x < _edgePoints[_edgePoints.Count - 1].x) return true;
            return false;
        }
    }

    //
    // 関数：RightSideShape(Vector2 _direction,float _distance) 
    //
    // 目的：ひびが右向きの方法で光の形を設定する
    // 
    private void RightSideShape(Vector2 _direction,float _distance) 
    {
        // 頂点数
        int pointNum = _edgePoints.Count;
        // 変数宣言
        Vector3[] _shape;
        // サイズ指定
        _shape = new Vector3[pointNum * 2];

        // 頂点をひびの形に合わせる
        for (int i = 1; i <= pointNum; i++)
        {
            _shape[i-1] = new Vector3(_edgePoints[pointNum - i].x, _edgePoints[pointNum - i].y, 0.0f);
            _shape[pointNum * 2 - i] = SetLightShape(_edgePoints[pointNum - i], _direction, _distance);
        }

        // 形を設定
        _light2D.SetShapePath(_shape);
        _light2D.intensity = 0.3f;
    }

    //
    // 関数：LeftSideShape(Vector2 _direction, float _distance) 
    //
    // 目的：ひびが左向きの方法で、光の形を設定する
    // 
    private void LeftSideShape(Vector2 _direction, float _distance) 
    {
        // 頂点数
        int pointNum = _edgePoints.Count;
        // 変数宣言
        Vector3[] _shape;
        // サイズ指定
        _shape = new Vector3[pointNum * 2];
        
        // 頂点をひびの形に合わせる
        for (int i = 0; i < pointNum; i++)
        {
            _shape[i] = new Vector3(_edgePoints[i].x, _edgePoints[i].y, 0.0f);
            // 頂点設定
            _shape[pointNum * 2 - (i + 1)] = SetLightShape(_edgePoints[i], _direction, _distance);
        }

        // 形を設定
        _light2D.SetShapePath(_shape);
        _light2D.intensity = 0.3f;
    }

    //-------------------------------------
    // ひびの分岐関数
    // 引数：なし
    // 戻り値なし
    private void CrackRandomBranch()
    {

    }

}
