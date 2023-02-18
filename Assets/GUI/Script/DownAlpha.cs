//---------------------------------------------------------
//担当者：二宮怜
//内容　：UIのalpha値変更
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAlpha : MonoBehaviour
{
    public CanvasRenderer canvas;

    public void SetAlpha(int _hp,int _maxhp)
    {
        this.canvas.SetAlpha((float)_hp / (float)_maxhp);
    }

    //private void Start()
    //{
    //    col = GetComponent<SpriteRenderer>().color;
    //}

    //void Update()
    //{
    //    alpha = gameOver.HP / gameOver.maxHp;

    //    //col = new Color(col.r, col.g, col.b, alpha);
    //    col = new Color(1.0f, 0.0f, 0.0f, alpha);
    //}
}
