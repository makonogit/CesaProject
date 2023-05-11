//----------------------------------------------------------
// �S���ҁF��{��
// ���e  �F�G�̃A�j���[�V��������(�v�����g�ꃂ�u)
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy_ChangeAnim : MonoBehaviour
{
    // �O���擾
    [SerializeField] private PlantEnemyMove _plantEnemyMove; // �v�����g��̃��u
    [SerializeField] private Animator anim; // �v�����g�ꃂ�u�̃A�j���[�^�[�擾

    //Idle,         // �A�C�h��
    //Pre_Attack,   // �U������
    //Attack,       // �U��
    //Attack_Wait,  // �U����̑ҋ@
    //Firing,       // ��яo��
    //Confusion,    // ����
    //Death,        // ���S

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
