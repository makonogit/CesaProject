//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：エリアオブジェ中のひび、ステージとその他状態を管理する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StagesManager : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    public bool _Start = false;// このエリアをプレイできるか
    public int _clearCount = 0;// クリアしたステージの数

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    [SerializeField]
    private bool _areaClear = false;// このエリアをクリアしたか
    [SerializeField]
    private GameObject _crackObj;// ひびオブジェ
    [SerializeField]
    private List<GameObject> _stages;// ステージオブジェリスト
    
    private EdgeCollider2D _edgeCollider2D;// エッジコライダー2D

    [SerializeField]
    private List<GameObject> _cracks;// ひびのオブジェリスト

    [SerializeField]
    private List<int> _stagePointNum;// ステージの位置リスト

    private int _crackActiveNum = 0;// ひびのアクティブ数
    [SerializeField]
    private float _creatTime = 0.5f;// ひびの生成速度
    private float _nowTime = 0.0f;// 前回の生成からの経過時間
    [SerializeField]
    private GameObject _crystal;// 結晶オブジェ

    //-----------------------------------------------------------------
    //―設定処理―
    void OnValidate()// Inspectorを触った時の処理
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        if (_edgeCollider2D == null) Debug.LogError("EdgeCollider2Dのコンポーネントを取得できませんでした。");
        //--------------------------------------
        // 各ステージ位置の更新処理
        for (int i = 0; i < _stagePointNum.Count; i++) 
        {
            // 各ステージの位置を更新
            _stages[i].transform.position = new Vector3(_edgeCollider2D.points[_stagePointNum[i]].x * transform.localScale.x, _edgeCollider2D.points[_stagePointNum[i]].y * transform.localScale.y, 0) + transform.position;
        }
    }
    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        if (_edgeCollider2D == null) Debug.LogError("EdgeCollider2Dのコンポーネントを取得できませんでした。");
        // 初期化
        _nowTime = 0.0f;
        //--------------------------------------
        // ひびのオブジェクトリストの初期化
        for (; 0 < _cracks.Count;) _cracks.RemoveAt(0);

        //--------------------------------------
        // ひびオブジェの設定
        for (int i = 0; i < _edgeCollider2D.edgeCount; i++)
        {
            //--------------------------------------
            // ひびオブジェの追加
            _cracks.Add(Instantiate(_crackObj));
            
            // 名前設定
            _cracks[i].name = (name + "Crack" + i);

            //--------------------------------------
            // 位置の設定
            // 計算
            Vector3 _pos = _edgeCollider2D.points[i] + _edgeCollider2D.points[i + 1];// 今と次の位置を足す
            _pos = new Vector3(_pos.x * transform.localScale.x, _pos.y * transform.localScale.y, _pos.z);// ローカル座標なので自身のサイズをかけて距離を求める
            // 設定
            _cracks[i].transform.position =this.transform.position + _pos / 2;// 求めた座標を半分にして、ローカル座標なので自身の座標を足す

            //--------------------------------------
            // 角度とサイズの設定

            // 今と次の位置を引いて角度と長さを求める
            Vector2 _vec = new Vector2(_edgeCollider2D.points[i].x * transform.localScale.x, _edgeCollider2D.points[i].y * transform.localScale.y);
            _vec -= new Vector2(_edgeCollider2D.points[i + 1].x * transform.localScale.x, _edgeCollider2D.points[i + 1].y * transform.localScale.y);
            // ベクトルを角度に変換する
            float _angle = Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;// アークタンジェントでラジアン角を求め、その後に角度に変換
            // 角度設定
            _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle);
            // サイズ設定
            _cracks[i].transform.localScale = new Vector3(_vec.magnitude ,_crackObj.transform.localScale.y, _crackObj.transform.localScale.z);
            //--------------------------------------
            // 非表示設定
            _cracks[i].SetActive(false);
        }

    }

    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //--------------------------------------
        // ステージの間のひびの生成
        CrackBetweenStages();
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //―エリアの状態取得関数―(公)
    public bool AreaClear
    {
        get 
        {
            return _areaClear;
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //―ひびを生成するかどうか判断する関数―(私)
    private bool IsCrackActivate() 
    {
        // ラストステージではなく且つ次のステージまでひびができていないなら
        bool result_1 = _clearCount < _stagePointNum.Count && _crackActiveNum < _stagePointNum[_clearCount];
        // クリアした数がステージ数以上且つエッジの数より表示したひびの数が小さいとき
        bool result_2 = _stagePointNum.Count <= _clearCount && _crackActiveNum < _edgeCollider2D.edgeCount;

        return result_1 || result_2;
    }

    //―ステージの間のひびの生成関数―(私)
    private void CrackBetweenStages() 
    {
        // 経過時間の処理
        _nowTime += Time.deltaTime;
        // ひびの生成時間を越えて且つエリアをクリアしてなく且つこのエリアをプレイできるか
        if (_nowTime >= _creatTime && _crystal.activeSelf && _Start)
        {
            // ひびの表示処理
            if (IsCrackActivate())// ひびの生成をするべきか
            {
                _cracks[_crackActiveNum].SetActive(true);
                _crackActiveNum++;
            }
            // 全ステージクリアして且つ結晶が表示されていれば
            if (_areaClear == true && _crystal.activeSelf)
            {
                _crystal.SetActive(false);

                // このエリアのすべてのひびを非表示
                for (int i = 0; i < _cracks.Count; i++)
                {
                    _cracks[i].SetActive(false);
                }
            }
            // 経過時間のリセット
            _nowTime = 0.0f;
        }
        // ひびがすべて表示出来たら
        if (_crackActiveNum == _edgeCollider2D.edgeCount)
        {
            _areaClear = true;
        }
    }
}