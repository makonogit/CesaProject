//---------------------------------
//�@�S���F�����S
//�@�v�����g��{�X�̃_���[�W����
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCore : MonoBehaviour
{
    [SerializeField] Directing_BossLight light; //���U�p���C�g

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�@�Ђт��������������
        if(collision.tag == "Crack")
        {
            if (collision.GetComponent<CrackCreater>() && (collision.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.CREATING ||
            collision.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                if (light != null)
                {
                    if (transform.parent.GetComponent<PlantBossMove>().GetState() == PlantBossMove.PlantBossMoveState.DETH)
                    {
                        light.Flash();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
