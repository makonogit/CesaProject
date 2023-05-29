//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：ひびが入る
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AreaCrack : MonoBehaviour
{
    [SerializeField, Header("ひびのオブジェクトを入れてください")]
    private GameObject _crackObj;
    [SerializeField,Header("ひびの表示する所")]
    private int[] _point;
    private EdgeCollider2D _eddgeC2D;
    [SerializeField]
    private List<GameObject> _cracks;
    private int _displayedNum = 0;
    private int _startNum;
    private int _nextNum = 0;
    [SerializeField, Header("クリスタル")]
    private GameObject Crystal;
    private SpriteRenderer _crys;
    private SpriteRenderer _crys2;
    [SerializeField, Header("ひびの生成時間")]
    private AnimationCurve _creatTime;
    //private float _creatTime = 0.1f;
    [SerializeField, Header("壊れる時間")]
    private float _breakTime = 2.0f;

    private float _nowTime;

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private ScreenBreak Break;

    private bool _isAnimation;

    [SerializeField]
    private List<GiveScene>_stages;

    private bool _isBreaked;

    private bool _isAreClear;

    // 二宮追加
    private  bool _savedataClearArea; // セーブデータを取得したときに既にエリアをクリアしていた時、背景が壊れる演出をしないようにする

    // Use this for initialization
    void Start()
    {
        // 確認
        Check();

        // コンポーネントを取得
        SetComponents();

        
        // 最後の位置設定
        _point[_point.Length - 1] = _eddgeC2D.edgeCount - 1;
        // 初期化
        _isAnimation = false;
        _isBreaked = false;
        _isAreClear = false;
        _displayedNum = 0;
        _nowTime = 0.0f;
        // ひびオブジェの設置
        SetCrackObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAnimation) CrackAnimetion();
        if (isBreak && _savedataClearArea == false) BreakCrystal();      
    }

    //=====================================================
    // プライベート関数

    //
    // 関数：Check() 
    // 
    // 内容：変数が設定されているか確認する
    //
    private void Check() 
    {
        if (_crackObj == null) Debug.LogError("ひびのオブジェクトが入ってません" + this.name);
        if (_point.Length < 5) Debug.LogError("ポイント設定しましたか？");
        if (Crystal == null) Debug.LogError("Crystalがありません");
    }
    //
    // 関数：SetComponents() 
    // 
    // 内容：コンポーネントを取得する
    //
    private void SetComponents() 
    {
        _crys = Crystal.GetComponent<SpriteRenderer>();
        if (_crys == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");

        _crys2 = Crystal.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (_crys2 == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");

        _eddgeC2D = GetComponent<EdgeCollider2D>();
        if (_eddgeC2D == null) Debug.LogError("EdgeCollider2Dのコンポーネントを取得できませんでした。");

    }

    //
    // 関数：SetCrackObject()
    // 
    // 内容：ひびオブジェの設置
    //
    private void SetCrackObject()
    {
        for (int i = 0; i < _eddgeC2D.edgeCount; i++)
        {
            Vector2 _pos = (_eddgeC2D.points[i] + _eddgeC2D.points[i + 1]) / 2;
            _pos = new Vector2(_pos.x * this.transform.localScale.x, _pos.y * this.transform.localScale.y);
            _pos += new Vector2(this.transform.position.x, this.transform.position.y);
            _cracks.Add(Instantiate(_crackObj, new Vector3(_pos.x, _pos.y, 0), Quaternion.identity));

            _cracks[i].name = (transform.name + "crack" + i);
            //--------------------------------------
            // 角度とサイズの設定

            // 今と次の位置を引いて角度と長さを求める
            Vector2 _vec = new Vector2(_eddgeC2D.points[i].x * this.transform.localScale.x, _eddgeC2D.points[i].y * this.transform.localScale.y);
            _vec -= new Vector2(_eddgeC2D.points[i + 1].x * this.transform.localScale.x, _eddgeC2D.points[i + 1].y * this.transform.localScale.y);
            // ベクトルを角度に変換する
            float _angle = Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;// アークタンジェントでラジアン角を求め、その後に角度に変換
            // 角度設定
            _cracks[i].transform.eulerAngles = new Vector3(0, 0, _angle);
            // サイズ設定
            _cracks[i].transform.localScale = new Vector3(_vec.magnitude, _crackObj.transform.localScale.y, _crackObj.transform.localScale.z);
            _cracks[i].SetActive(false);
            //_cracks[i].transform.SetParent(this.transform);
        }
    }

    //
    // 関数： CrackAnimetion() 
    // 
    // 内容：ひびのアニメーション
    //
    private void CrackAnimetion() 
    {
        // 時間経過
        _nowTime += Time.deltaTime;
        if (isCrackActive)
        {
            _cracks[_displayedNum].SetActive(true);
            _displayedNum++;
            _nowTime = 0.0f;
        }
        if(_displayedNum >= _point[_nextNum]) 
        {
            _isAnimation = false;
        }
    }

    //
    // 関数：BreakCrystal() 
    // 
    // 内容：結晶が壊れる関数
    //
    private void BreakCrystal() 
    {
        // 結晶の非表示
        AreaClear();

        // パーティクルの設定
        Quaternion quat = Quaternion.Euler(-90, 0.0f, 0.0f);// 角度
        // パーティクル生成
        for (int i = 0; i < 2; i++) Instantiate(particle, this.transform.position, quat);
        for (int i = 0; i < 2; i++) Instantiate(particle, this.transform.position + new Vector3(transform.position.x + 19.2f,transform.position.y), quat);

        //nstantiate(Break, this.transform.position, Quaternion.identity);
        Break.enabled = true;

        _isBreaked = true;
    }

    //
    // 関数：isBreak 
    // 
    // 内容：結晶が壊れるフラグ
    //
    private bool isBreak 
    {
        get 
        {
            // 壊れているか
            if (_isBreaked) return false;
            // すべて表示したか
            if (_displayedNum < _point[_point.Length - 1]) return false;
            return true;
        }
    }
    //
    // 関数：isCrackActive 
    // 
    // 内容：ひびの表示をするか
    //
    private bool isCrackActive 
    {
        get
        {
            float numerator = (_displayedNum - _startNum);
            numerator = (numerator == 0 ? 1 : numerator);
            float  ratio = numerator / (float)(_point[_nextNum] - _startNum);
            if (_nowTime < _creatTime.Evaluate(ratio)) return false;
            return true;
        }
    }


    //=====================================================
    // パブリック関数

    //
    // 関数： AreaClear()
    // 
    // 内容：エリアをクリアしていた時
    //
    public void AreaClear()
    {
        // 結晶非表示
        _crys.enabled = false;
        _crys2.enabled = false;
        // 全ひびの非表示
        for (int i =0; i < _cracks.Count; i++) _cracks[i].SetActive(false);
        
        // 全ステージの状態をクリアにする
        for (int i = 0; i < _stages.Count; i++) _stages[i].State = GiveScene.StateID.CLEAR;

        // エリアクリアの割れる演出をゲームスタート時にしなくていいように
        _isBreaked = true;
        _isAreClear = true;
    }

    //
    // 関数：LoadStage(int _clearNum) 
    // 
    // 内容：エリアのステージ状態を設定する
    //
    public void LoadStage(int _clearNum) 
    {
        //Debug.Log("クリアした数"+_clearNum);
        // 配列に合わせるため値を一個ずらす
        int Num = _clearNum - 1;

        //Debug.Log(Num);

        if(Num >= 0)// ステージ１以上クリアしているなら
        {
            // ひびの表示
            for (; _displayedNum <= _point[Num]; _displayedNum++) _cracks[_displayedNum].SetActive(true);
            // ステージの状態をクリアにする
            for (int i = 0; i <= Num; i++) _stages[i].State = GiveScene.StateID.CLEAR;
            //次のステージをプレイ可能にする
            _stages[Num+1].State = GiveScene.StateID.PLAYABLE;
        }
        else if (Num < 0) // まだステージをクリアしてないなら
        {
            //はじめのステージをプレイ可能にする
            _stages[0].State = GiveScene.StateID.PLAYABLE;
        }    
        
    }

    //
    // 関数：ClearStage(int _stageNum) 
    // 
    // 内容：ステージをクリアしたならひびのアニメーションを開始する
    //
    public int ClearStage(int _stageNum) 
    {
        //Debug.Log("ステージステータス"+_stages[_stageNum].State);
        // プレイ可能でクリアしていない時
        if (_stages[_stageNum].State == GiveScene.StateID.PLAYABLE) 
        {
            _isAnimation = true;
            _startNum = _displayedNum;
            _nextNum = _stageNum;
            _stages[_stageNum].State = GiveScene.StateID.CLEAR;
            int next = _stageNum + 1;
            if(next < 5) 
            {
                _stages[next].State = GiveScene.StateID.PLAYABLE;
            }
            else 
            {
                _isAreClear = true;
            }
            return 1;
        }
        return 0;
    }

    //
    // 関数：AreaStart()
    // 
    // 内容：エリアの解放
    //
    public void AreaStart() 
    {
        //はじめのステージをプレイ可能にする
        _stages[0].State = GiveScene.StateID.PLAYABLE;
    }
    //
    // 関数：IsAreaClear 
    // 
    // 内容：エリアがクリアしたか
    //
    public bool IsAreaClear 
    { get 
        {
            return _isAreClear;
        } 
    }

    public void SetSaveDataClearArea()
    {
        _savedataClearArea = true;
    }
}