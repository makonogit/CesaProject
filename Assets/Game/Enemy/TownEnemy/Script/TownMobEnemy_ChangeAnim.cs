//----------------------------------------------------------
// 担当者：二宮怜
// 内容  ：敵のアニメーション制御(町モブ)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMobEnemy_ChangeAnim : MonoBehaviour
{
    // 変数宣言


    // 外部取得
    private EnemyMove enemyMove;
    private Animator anim; // 町の雑魚敵のアニメーター取得

    // Start is called before the first frame update
    void Start()
    {
        // アニメーター取得
        anim = GetComponent<Animator>();

        // EnemyMove取得
        enemyMove = GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {

        // 自身のAIの状態によってアニメーションを変更する
        switch (enemyMove.EnemyAI)
        {
            // 敵の行動が移動のみ
            case EnemyMove.AIState.INIT_PATROL:
            case EnemyMove.AIState.PATROL:
            case EnemyMove.AIState.INIT_TRACKING:
            case EnemyMove.AIState.TRACKING:

                // アニメーション用にパラメーターセット
                anim.SetBool("attack", false);
                anim.SetBool("walk", true);
                break;

            // 攻撃状態時
            case EnemyMove.AIState.ATTACK:

                // アニメーション用にパラメーターセット
                anim.SetBool("attack", true);
                break;

            case EnemyMove.AIState.ATTACK_WAIT:

                // アニメーション用にパラメーターセット
                anim.SetBool("wait", true);
                break;

            case EnemyMove.AIState.DEATH:

                // アニメーション用にパラメーターセット
                anim.SetBool("death", true);
                break;
        }
    }
}
