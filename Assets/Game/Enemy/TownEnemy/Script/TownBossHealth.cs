//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F1�ʃ{�X�̗̑͊Ǘ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossHealth : MonoBehaviour
{
    // �ϐ��錾
    private string CrackTag = "Crack";

    // �{�X�̗̑�
    [System.NonSerialized]public int BossHealth;
    public int MaxBossHealth = 3;

    private CrackCreater order = null;

    // �O���擾
    private GameObject Boss;
    private TownBossMove bossMove;

    private void Start()
    {
        // �{�X�T��
        Boss = GameObject.Find("TownBoss");
        // �{�X�̍s���X�N���v�g�擾
        bossMove = Boss.GetComponent<TownBossMove>();

        BossHealth = MaxBossHealth;
        //BossHealth = 2;
        //BossHealth = 1;
    }

    private void Update()
    {
        //Debug.Log(BossHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

            //�������Ȃ�
            if (order != null)
            {
                if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                {
                    // ���G��Ԃ���Ȃ����
                    if (bossMove.invincibility == false)
                    {
                        // �̗�-1
                        BossHealth--;

                        // �Ђя���
                        Destroy(collision.gameObject);

                        // �{�X�̗̑͂�0�ȉ��ɂȂ�����
                        if (BossHealth <= 0)
                        {
                            // AI�̏�Ԃ�ω�
                            bossMove.EnemyAI = TownBossMove.AIState.Death; // ���j���
                        }

                        // �A����e��h�����ߎ��̍s���܂Ŗ��G
                        bossMove.invincibility = true;
                    }
                }
            }
        }
    }
}
