//---------------------------------------------------------
//担当者：二宮怜
//内容　：ひびをいれた時の反動ダメージをくらう
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    [SerializeField, Unity.Collections.ReadOnly]private bool CrackFlg; // ひびが作られるときのフラグ
    [SerializeField, Unity.Collections.ReadOnly]private float CrackPower; // ひびが作られるときのフラグ

    // 外部取得
    private Crack crack; // CrackPowerを取得するための変数
    private GameObject GUI;  // GUIオブジェクトを探して格納するための変数
    private GameOver gameOver; // GameOverスクリプトを格納
    private GameObject Health; // Alpha値減らすために必要なスクリプトを持つゲームオブジェクトを
    private DownAlpha alpha; // UIのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // Crackを取得
        crack = GetComponent<Crack>();

        //---------------------------------------------------------
        // GUIを探す
        GUI = GameObject.Find("GUI");

        //---------------------------------------------------------
        // GameOverスクリプトを格納
        gameOver = GUI.GetComponent<GameOver>();

        //---------------------------------------------------------
        // Healthを探す
        Health = GameObject.Find("Health");

        //---------------------------------------------------------
        // DownAlphaスクリプトを格納
        //alpha = Health.GetComponent<DownAlpha>();
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // ひびが作られる瞬間のみtrueになるフラグを見てダメージを与える
        if (CrackFlg == true)
        {
            //---------------------------------------------------------
            // HPにダメージを与える
            gameOver.DecreaseHP(CrackPower);
            // アルファ値変更
            alpha.SetAlpha(gameOver.HP, gameOver.maxHp);

            CrackFlg = false;
        }
    }

    public void SetCrackInfo(float _power)
    {
        CrackFlg = true;
        CrackPower = _power;
    }
}
