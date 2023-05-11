//----------------------------------------------------------
// 担当者：二宮怜
// 内容  ：敵のアニメーション制御(プラント場モブ)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy_ChangeAnim : MonoBehaviour
{
    // 外部取得
    [SerializeField] private PlantEnemyMove _plantEnemyMove; // プラント場のモブ
    [SerializeField] private Animator anim; // プラント場モブのアニメーター取得

    //Idle,         // アイドル
    //Pre_Attack,   // 攻撃準備
    //Attack,       // 攻撃
    //Attack_Wait,  // 攻撃後の待機
    //Firing,       // 飛び出す
    //Confusion,    // 混乱
    //Death,        // 死亡

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("idle",_plantEnemyMove.EnemyAI == PlantEnemyMove.AIState.Idle);
        anim.SetBool("walk",_plantEnemyMove.EnemyAI == PlantEnemyMove.AIState.Attack);
        anim.SetBool("death",_plantEnemyMove.EnemyAI == PlantEnemyMove.AIState.Death);
        anim.SetBool("rage", _plantEnemyMove.EnemyAI == PlantEnemyMove.AIState.Rage);
    }
}
