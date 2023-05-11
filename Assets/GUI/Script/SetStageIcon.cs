//---------------------------------------
//担当者：二宮
//内容　：ステージアイコンセット
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStageIcon : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    [SerializeField] private List<AreaIcon> areaList;
    [SerializeField] private Image stageIcon;

    // Start is called before the first frame update
    void Start()
    {
        SetStage setstage = new SetStage();

        int areanum = setstage.GetAreaNum();
        int stagenum = setstage.GetStageNum();

        // ステージアイコン取得
        stageIcon.sprite = areaList[areanum].stageList[stagenum]._sprite;
    }
}

[System.Serializable]
public class AreaIcon
{
    public List<StageIcon> stageList;

    public AreaIcon(List<StageIcon> _stagelist)
    {
        stageList = _stagelist;
    }
}

[System.Serializable]
public class StageIcon
{
    public Sprite _sprite; // スプライト情報持つ

    public StageIcon(Sprite sp)
    {
        _sprite = sp;
    }
}