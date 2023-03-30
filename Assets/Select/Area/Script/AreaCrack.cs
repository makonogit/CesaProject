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
    private bool _init = false;
    private int _nextNum = 0;
    [SerializeField, Header("クリスタル")]
    private GameObject Crystal;
    private SpriteRenderer _crys;
    [SerializeField, Header("ひびの生成時間")]
    private float _creatTime = 0.1f;
    [SerializeField, Header("壊れる時間")]
    private float _breakTime = 2.0f;

    private float _nowTime;

    [SerializeField]
    private ParticleSystem particle;
    // Use this for initialization
    void Start()
    {
        if (!_init) Init();
        if(Crystal == null) Debug.LogError("Crystalがありません");
        _crys = Crystal.GetComponent<SpriteRenderer>();
        if (_crys == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");
    }

    // Update is called once per frame
    void Update()
    {
        if ((_displayedNum - _nextNum) < 0)
        {
            CracksGenerating();
            Debug.Log("表示した数" + _displayedNum+"最大"+ _eddgeC2D.edgeCount);
        }
        if(_displayedNum == _eddgeC2D.edgeCount-1) 
        {
            _nowTime += Time.deltaTime;
            _crys.color = new Color(1, 1, 1, _nowTime / _breakTime);// 結晶の透明度を変えてる
            if (_nowTime >= _breakTime) 
            {
                if (Crystal.activeSelf) 
                {
                    Instantiate(particle,transform.position,Quaternion.identity);
                }
                CrystalDisp(false);
            }
            
        }
    }
    public void CrystalDisp(bool _onOff) 
    {
        Crystal.SetActive(_onOff);
        if (!_onOff) 
        {
            for (int i = 0; i < _cracks.Count; i++)
            {
                _cracks[i].SetActive(false);
            }
        }
    }
    public void Set(int _oldnNm,int _nowNum) 
    {
        if (!_init) Init();
        if(_oldnNm > 0) 
        {
            for (int i = 0; i <= _point[_oldnNm - 1]; i++)
            {
                _cracks[i].SetActive(true);
            }
            _displayedNum = _point[_oldnNm - 1];
        }
        if (_nowNum>0) 
        {
            _nextNum = _point[_nowNum - 1];
        }
    }
    private void Init()
    {
        _eddgeC2D = GetComponent<EdgeCollider2D>();
        if (_eddgeC2D == null) Debug.LogError("EdgeCollider2Dのコンポーネントを取得できませんでした。");
        if (_crackObj == null) Debug.LogError("ひびのオブジェクトが入ってません" + this.name);
        if (_point.Length < 5) Debug.LogError("ポイント設定しましたか？");
        
        // 最後の位置設定
        _point[_point.Length - 1] = _eddgeC2D.edgeCount-1;
        _displayedNum = 0;
        _nextNum = 0;
        for (int i = 0; i < _eddgeC2D.edgeCount; i++) 
        {
            Vector2 _pos = (_eddgeC2D.points[i] + _eddgeC2D.points[i + 1])/2;
            _pos = new Vector2(_pos.x * this.transform.localScale.x, _pos.y * this.transform.localScale.y);
            _pos += new Vector2(this.transform.position.x, this.transform.position.y);
            _cracks.Add(Instantiate(_crackObj,new Vector3(_pos.x, _pos.y,0),Quaternion.identity));

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
        _init = true;
    }

    private void CracksGenerating() 
    {
        _nowTime += Time.deltaTime;
        if (_nowTime >= _creatTime&& _displayedNum < _eddgeC2D.edgeCount) 
        {
            _displayedNum++;
            _cracks[_displayedNum].SetActive(true);

            _nowTime = 0;
        }
    }
}