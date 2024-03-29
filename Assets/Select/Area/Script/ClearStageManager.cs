﻿//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：クリアしたステージを管理するnewSelectScene
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClearStageManager : MonoBehaviour
{
    private SaveData _data;
    [SerializeField,Header("エリア")]
    private List<AreaCrack> _areas;

    [SerializeField, Header("ステージマネージャー")]
    private StageManager _stagemanager;

    //必要のもの
    // クリアしたか
    private Clear clear;
    // どこのステージか
    private SetStage setmanager;
    [SerializeField]
    private bool flag;
    // Use this for initialization
    void Start()
    {
        clear = new Clear();
        setmanager = new SetStage();
        //-------------------------------------------------------
        _data = new SaveData();

        //-------------------------------------------------------

        // クリアしたステージの設定
        SetClearStageNum();
        flag = clear.GetFlag;
        // クリアしたなら　クリアしたエリアにステージ番号を送る
        if (clear.GetFlag) 
        {
            int areaNum = setmanager.GetAreaNum();
            int stageNum = setmanager.GetStageNum();

            setmanager.SetClearFlg(areaNum, stageNum);

            //Debug.Log("エリア"+areaNum+"ステージ"+stageNum);
            _data._data.ClearStages += _areas[areaNum].ClearStage(stageNum);
            //Debug.Log(_data._data.ClearStages);
            _data.Save(_data._data);
            // エリアをクリアしたか
            if (_areas[areaNum]._isAreClear == true && areaNum + 1 < 5)
            {
                _areas[areaNum+1].AreaStart();//新エリア解放
                    
            }
        }
        
    }
    private void OnDestroy()
    {
        // メインゲーム開始
        clear.StartGame();
    }

    // 二宮追加
    // ゲームを開始してシーンのロードを挟んでも最初の一回しか呼び出さない
    // 関数のフラグを持つクラス
    public static class TheVeryFirst
    {
        public static bool Init = false;
    };

    //
    // 関数；SetClearStageNum()
    //
    // 内容：クリアしたステージの設定
    //
    private void SetClearStageNum()
    {
        // エリア番号：ゼロの除算をする可能性があるため三項演算でやっています。
        int _areaNum = (_data._data.ClearStages == 0 ? 0 : _data._data.ClearStages / 5);

        // ステージ番号
        int _stageNum = _data._data.ClearStages - (_areaNum * 5);

        //Debug.Log(_data._data.ClearStages);
        //Debug.Log(_areaNum);
        //Debug.Log(_stageNum);


        // 二宮追加
        // セーブデータを取得してきて、ステージをクリア済みと未クリアで分ける
        // 最初の一回のみ実行
        if (TheVeryFirst.Init == false)
        {
            // 一番進んでいるエリアのひとつ前のエリアまでのクリアフラグを立てる
            for (int i = 0; i < _areaNum; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    setmanager.SetClearFlg(i, j);
                }

                // セーブデータによるエリアのクリア
                _areas[i].SetSaveDataClearArea();
                _areas[i]._isAreClear = true;
            }

            // 一番進んでいるエリアのステージのクリアフラグを立てる
            for (int i = 0; i < _stageNum; i++)
            {
                setmanager.SetClearFlg(_areaNum, i);
            }

            TheVeryFirst.Init = true;
        }

        if (_areaNum < 5)
        {
            // クリアしたステージの設定
            _areas[_areaNum].LoadStage(_stageNum);
        }

        // クリアしたエリアの設定
        for (int i = 0; i < _areaNum; i++) _areas[i].AreaClear();
    }


}
public class Clear 
{
    private static bool _isClear = false;
    public bool GetFlag
    {
        get 
        {
            return _isClear;
        }
    }

    public void GameClear() 
    {
        _isClear = true;
    }
    public void StartGame()
    {
        _isClear = false;
    }
}