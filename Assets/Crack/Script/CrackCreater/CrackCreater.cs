//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：与えらた位置からひびを作る
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackCreater : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―変数―(公)Accessible variables

    // 状態
    public enum CrackCreaterState
    {
        NONE,       // 何もない
        START,      // 作成開始
        CREATING,   // 作成中
        CRAETED,    // 作成完了
    }
    [System.NonSerialized]// 非表示
    public CrackCreaterState State; // 外部閲覧用

    [System.NonSerialized]// 非表示
    public EdgeCollider2D Edge2D;

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

    [SerializeField]
    private List<Vector2> _nailPoints;// 釘の座標リスト
    [SerializeField]
    private List<Vector2> _edgePoints;// 辺の座標リスト
    [SerializeField]
    private List<int> _nailPointCount;
    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        //--------------------------------------
        // エッジコライダー2Dが入っているか
        Edge2D = GetComponent<EdgeCollider2D>();
        if(Edge2D == null) 
        {
            Debug.LogError("EdgeCollider2Dがコンポーネントされてません。");
        }

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
    }


    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //--------------------------------------
        // 状態が作成開始
        if (_nowState == CrackCreaterState.START )
        {
            EdgeSetting();//エッジの設定
            _nowState = CrackCreaterState.CREATING;
        }

        //--------------------------------------
        // 状態が作成中(演出部分)
        if (_nowState == CrackCreaterState.CREATING)
        {

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
            if (i != _nailPoints.Count-1) 
            {
                // 分割処理
                DivisionPositionSetting(i);
            }
        }

        //--------------------------------------
        // 2つ頂点の間にポイントを置く
        for (int i = 0; i < _edgePoints.Count-1; i++)
        {
            // 中間座標を求める
            Vector2 _center = (_edgePoints[i] + _edgePoints[i + 1])/2;
            Vector3 _point = new Vector3(_center.x, _center.y,0);
            
            // 呼び出し
            GameObject obj = Instantiate(_crackObject,_point,Quaternion.identity,transform);
            
            // 二つの釘から垂直な角度を求める
            Vector2 _vec = _edgePoints[i] - _edgePoints[i+1];
            float _angle = Mathf.Atan2(_vec.y, _vec.x)*Mathf.Rad2Deg;
            // 角度設定
            obj.transform.eulerAngles = new Vector3(0, 0, _angle);
            // サイズ設定
            obj.transform.localScale = new Vector3( _vec.magnitude, obj.transform.localScale.y, obj.transform.localScale.z);
        }
        // 頂点を設定する
        Edge2D.SetPoints(_edgePoints);
    }

    //-------------------------------------------------------
    //―分割位置設定関数―(私)
    private void DivisionPositionSetting(int _num) 
    {
        if(_num == 0) 
        {
            _nailPointCount.Add(_num);
        }
        _nailPointCount.Add(_nailPointCount[_num] + 1);

        // 二つの釘から垂直な角度を求める
        Vector2 _vec = _nailPoints[_num + 1] - _nailPoints[_num];
        float _angle = Mathf.Atan2(_vec.y, _vec.x) ;
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
            _pos += _nailPoints[_num + 1] *  _percent;
            if (j != 1 && j != _division - 1)// 最初と最後以外
            {
                _pos += _verticalVec * _odd * Random.Range(_rangeNum.x, _rangeNum.y);
            }
            // 間を追加
            _edgePoints.Add(_pos);
            // _edgePoinsの位置を計算する
            _nailPointCount[_num +1] += 1;
        }
    }
}
