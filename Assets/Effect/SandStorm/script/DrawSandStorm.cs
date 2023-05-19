//------------------------------------
//担当：二宮怜
//内容：砂嵐を表示するかしないか
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSandStorm : MonoBehaviour
{
    // 変数宣言

    // 現在のステージ、エリアを取得できるクラス
    SetStage setstage = new SetStage();

    [SerializeField] private SpriteRenderer _spriteRenderer_l; // 砂嵐オブジェのレンダラー（左）
    [SerializeField] private SpriteRenderer _spriteRenderer_r; // 砂嵐オブジェのレンダラー（右） 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(setstage.GetAreaNum());

        // 挑戦エリアが砂漠なら砂嵐表示
        if(setstage.GetAreaNum() == 2)
        {
            _spriteRenderer_l.enabled = true;
            _spriteRenderer_r.enabled = true;
        }
        // 砂漠以外なら砂嵐非表示
        else
        {
            _spriteRenderer_l.enabled = false;
            _spriteRenderer_r.enabled = false;
        }
    }
}
