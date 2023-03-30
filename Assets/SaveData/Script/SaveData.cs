//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：セーブとロード
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SaveData : MonoBehaviour
{
    [HideInInspector]
    public StatusData _data;                          // json変換するデータのクラス
    string filepath;                                        // ファイルパス
    string fileName = "Data.json";                          // ファイル名

    private int _maxArea = 5;
    private int _maxStage = 5;

    //-----------------------------------------------------------------
    //―開始時
    private void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            // 初期化
            Init();

            Save(_data);
        }

        // ファイルを読み込んでdataに格納
        _data = Load(filepath);
    }

    public StatusData Data 
    {
        get 
        {
            return _data;
        }
        set 
        {
            _data = value;
        }
    }

    public void Save(StatusData _data)
    {
        string json = JsonUtility.ToJson(_data);
        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }

    private StatusData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        return JsonUtility.FromJson<StatusData>(json);
    }
    private void OnDestroy()
    {
        Save(_data);
    }
    private void Init()
    {

        // 初期化
        _data.Name = "テストデータ";
        _data.ClearStages = 0;
        //宣言
        _data.Stage =new bool[_maxArea][];
        for (int i = 0; i < _maxArea; i++) 
        {
            _data.Stage[i] = new bool[_maxStage];
        }
        // 初期化
        for(int i = 0;i<_maxArea; i++) 
        {
            for(int j = 0; j < _maxStage; j++) 
            {
                _data.Stage[i][j] = false;
            }
        }
        

    }

}


[System.Serializable]
public class StatusData
{ 
    public string Name;
    public bool[][] Stage;
    public int ClearStages;
}