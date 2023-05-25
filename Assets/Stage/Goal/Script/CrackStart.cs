//-----------------------------------
//　担当：菅
//　内容：ひび入れ開始
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackStart : MonoBehaviour
{
    [SerializeField,Header("ひび入りクリスタルのリスト")]
    private List<ClearCrack> _clearacks;

    [SerializeField, Header("最後のひび")]
    private SpriteRenderer _endcrack;

    public bool StartFlg = false; //破壊開始アニメーション

    public bool EndFlg = false;　//終了

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlg)
        {
            // ひび割れスクリプトを有効化
            for (int i = 0;i< _clearacks.Count;i++)
            {
                _clearacks[i].enabled = true;
            }
            StartFlg = false;
        }

        // 最後のひびが表示されたら終了する
        if (_endcrack.enabled)
        {
            EndFlg = true;
        }

    }

    //----------------------
    // ひび割れ開始関数
    //----------------------
    public void StartCrack()
    {
        StartFlg = true;
    }

}
