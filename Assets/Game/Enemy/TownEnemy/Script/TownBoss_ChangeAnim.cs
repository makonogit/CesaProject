//----------------------------------------------------------
// 担当者：二宮怜
// 内容  ：敵のアニメーション制御(町ボス)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBoss_ChangeAnim : MonoBehaviour
{
    // 外部取得
    private TownBossMove townBossMove;
    private Animator anim; // 町のボスのアニメーター取得

    // Start is called before the first frame update
    void Start()
    {
        // アニメーター取得
        anim = GetComponent<Animator>();

        // EnemyMove取得
        townBossMove = GetComponent<TownBossMove>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("walk", townBossMove.EnemyAI == TownBossMove.AIState.Walk);
        anim.SetBool("charge", townBossMove.EnemyAI == TownBossMove.AIState.ThrowShardsInit);
        anim.SetBool("ramming", townBossMove.EnemyAI == TownBossMove.AIState.Ramming);
        anim.SetBool("ramminginit", townBossMove.EnemyAI == TownBossMove.AIState.RammingInit);
        anim.SetBool("death", townBossMove.EnemyAI == TownBossMove.AIState.Death);
    }
}
