//---------------------------------
//担当：二宮怜
//内容：SpriteRendererのオンオフを切り替えて
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOnOff : MonoBehaviour
{
    // 変数宣言

    // 点滅周期
    [SerializeField] private float cycle = 1f;

    // 経過時間
    private double time;

    // 点滅するかどうか
    public bool isFlashing = false;

    // 何秒間点滅させるか
    private float flashTime = 2f;

    // 明滅のデューティ比（1でレンダラーon,0でoff）
    [SerializeField, Range(0, 1)] private float dutyRate = 0.5f;

    private SpriteRenderer spriteRenderer; // 自身のスプライトレンダラー

    // Start is called before the first frame update
    void Start()
    {
        // 自身のスプライトレンダラー取得
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 点滅フラグがたったら
        if(isFlashing == true)
        {
            // 時間経過
            time += Time.deltaTime;

            // 周期cycleで繰り返す値の取得
            // 0～cycleの範囲で値が得られる
            var repeatRange = Mathf.Repeat((float)time, cycle);

            // 内部時刻timeにおける明滅状態を反映
            // デューティ比でon/offの割合を変更している
            spriteRenderer.enabled = repeatRange >= cycle * (1 - dutyRate);

            // 点滅時間が経過したら
            if(time >= flashTime)
            {
                // 点滅しない
                isFlashing = false;

                // 初期化
                time = 0f;

                // 消えた状態で終わらないように
                spriteRenderer.enabled = true;
            }
        }
    }

    // 点滅の長さを変えながら状態をフラグをセットしたい
    public void SetFlash(bool _flash,float _time)
    {
        isFlashing = _flash;
        flashTime = _time;
    }

    // 点滅時間はそのままでフラグをセット
    public void SetFlash(bool _flash)
    {
        isFlashing = _flash;
    }
}
