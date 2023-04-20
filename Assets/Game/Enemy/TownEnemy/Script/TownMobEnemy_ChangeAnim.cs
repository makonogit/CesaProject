//----------------------------------------------------------
// �S���ҁF��{��
// ���e  �F�G�̃A�j���[�V��������(�����u)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMobEnemy_ChangeAnim : MonoBehaviour
{
    // �ϐ��錾


    // �O���擾
    private EnemyMove enemyMove;
    private Animator anim; // ���̎G���G�̃A�j���[�^�[�擾

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�^�[�擾
        anim = GetComponent<Animator>();

        // EnemyMove�擾
        enemyMove = GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {

        // ���g��AI�̏�Ԃɂ���ăA�j���[�V������ύX����
        switch (enemyMove.EnemyAI)
        {
            // �G�̍s�����ړ��̂�
            case EnemyMove.AIState.INIT_PATROL:
            case EnemyMove.AIState.PATROL:
            case EnemyMove.AIState.INIT_TRACKING:
            case EnemyMove.AIState.TRACKING:

                // �A�j���[�V�����p�Ƀp�����[�^�[�Z�b�g
                anim.SetBool("attack", false);
                anim.SetBool("walk", true);
                break;

            // �U����Ԏ�
            case EnemyMove.AIState.ATTACK:

                // �A�j���[�V�����p�Ƀp�����[�^�[�Z�b�g
                anim.SetBool("attack", true);
                break;

            case EnemyMove.AIState.ATTACK_WAIT:

                // �A�j���[�V�����p�Ƀp�����[�^�[�Z�b�g
                anim.SetBool("wait", true);
                break;

            case EnemyMove.AIState.DEATH:

                // �A�j���[�V�����p�Ƀp�����[�^�[�Z�b�g
                anim.SetBool("death", true);
                break;
        }
    }
}
