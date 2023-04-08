//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスの行動を制御
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // *** Ray関連 ***

    [SerializeField] private LayerMask rayLayer;// Rayのレンダー

    //-------------------------------------
    // *** 行動制御関連 ***

    enum MainStateID   // メイン状態ID
    {
        NULL,          // 状態なし
        MOVE,          // 移動状態
        ATTACK,        // 攻撃状態
    }
    MainStateID oldMainState = MainStateID.NULL; // 前の状態
    MainStateID nowMainState = MainStateID.MOVE; // 現在の状態
    MainStateID nextMainState = MainStateID.NULL;// 次の状態

    enum AttackStateID// 攻撃状態ID
    {
        NULL,         // 状態なし
        ENEMY_DROP,   // 敵を降らせる
        GRIP_PLAYER   // プレイヤーを捕まえる
    }
    AttackStateID oldAttackState = AttackStateID.NULL;       // 前の状態
    AttackStateID nowAttackState = AttackStateID.GRIP_PLAYER;// 現在の状態
    AttackStateID nextAttackState = AttackStateID.NULL;      // 次の状態

    //-------------------------------------
    // *** 行動決定関連 ***

    int mainStateDelay;                  // 行動間の間隔
    bool isEndState;                     // 行動終了フラグ

    //=====================================
    // *** 更新処理 ***
    //=====================================

    void Update()
    {
        //---------------------------------------
        // *** 現在の状態によって処理を分岐 ***

        if (nextMainState != MainStateID.NULL)
        {
            oldMainState = nowMainState;
            nowMainState = nextMainState;
            nextMainState = MainStateID.NULL;
        }

        switch (nowMainState)
        {
            // 移動状態
            case MainStateID.MOVE:
                Move_CaveBoss.instance.Move();
                RandomMainState();
                break;
            // 攻撃状態
            case MainStateID.ATTACK:
                Attack();
                break;
        }
    }

    //=====================================
    // *** 攻撃処理 ***
    //=====================================

    void Attack()
    {
        //---------------------------------------
        // *** 現在の状態によって処理を分岐 ***

        if (nextAttackState != AttackStateID.NULL)
        {
            oldAttackState = nowAttackState;
            nowAttackState = nextAttackState;
            nextAttackState = AttackStateID.NULL;
        }

        switch (nowAttackState)
        {
            // 敵を降らせる
            case AttackStateID.ENEMY_DROP:
                isEndState = EnemyDrop_CaveBoss.instance.EnemyDrop();
                break;
            // プレイヤーを掴む
            case AttackStateID.GRIP_PLAYER:
                isEndState = GripPlayer_CaveBoss.instance.GripPlayer();
                break;
        }

        //---------------------------------------
        // *** 攻撃が終了したら通常状態に戻す ***

        if (isEndState == true)
        {
            nextMainState = MainStateID.MOVE;
            isEndState = false;
        }
    }

    //=============================================
    // *** ランダムにメイン行動を決定する処理 ***
    //=============================================

    void RandomMainState()
    {
        //---------------------------------
        // *** ランダムに次の行動を決定 ***

        mainStateDelay++;

        if (mainStateDelay >= 500)
        {
            //---------------------------------
            // 敵を降らせる

            int rnd = Random.Range(1, 100 + 1);

            if (rnd > 50)
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.ENEMY_DROP;
            }

            //---------------------------------
            // プレイヤーを捕まえる

            else
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.GRIP_PLAYER;
            }

            mainStateDelay = 0;
        }
    }
}