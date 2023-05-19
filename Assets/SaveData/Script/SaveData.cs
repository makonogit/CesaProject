//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：セーブとロード
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    [HideInInspector]
    public StatusData _data;                          // json変換するデータのクラス
    private string filepath;                                        // ファイルパス
    private string fileName = "Data.json";                          // ファイル名

    public SaveData() 
    {
        // パス名取得
        filepath = Application.persistentDataPath + "/" + fileName;

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
    //-----------------------------------------------------------------
    //―開始時
    private void Start()
    {
        // パス名取得
        filepath = Application.persistentDataPath + "/" + fileName;

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

    public void Save(StatusData data)
    {
        // パス名取得
        filepath = Application.persistentDataPath + "/" + fileName;

        var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        string json = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(fs);
        wr.WriteLine(json);
        wr.Flush();
        wr.Close();
        fs.Close();
    }

    private StatusData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();
        return JsonUtility.FromJson<StatusData>(json);
    }

    private void OnDestroy()
    {
        Save(_data);
    }
    private void Init()
    {
        _data = new StatusData();
    }

}


[System.Serializable]
public class StatusData
{ 
    public string Name;
    public int ClearStages;
    public StatusData() 
    { 
        Name = "テストデータ";
        ClearStages = 0;
    }
}