//----------------------------------------------------------
// �S���ҁF��{��
// ���e  �F�G�̃A�j���[�V��������(���{�X)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBoss_ChangeAnim : MonoBehaviour
{
    // �O���擾
    private TownBossMove townBossMove;
    private Animator anim; // ���̃{�X�̃A�j���[�^�[�擾

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�^�[�擾
        anim = GetComponent<Animator>();

        // EnemyMove�擾
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
