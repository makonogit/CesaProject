//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̖��G���Ԃ��Ǘ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public float NoDamageTime = 2f; //���G����
    [SerializeField]public float HitTime = 0.0f; // �O��_���[�W���󂯂�������̌o�ߎ���

    private void Start()
    {
        // �n�܂��������G���ԂȂ̂�h�����ߏ�����
        HitTime = NoDamageTime;
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }
}
