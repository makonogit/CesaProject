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
    [SerializeField] PlantBossMove move;        //�s���X�N���v�g


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�@�Ђт��������������
        if (collision.gameObject.tag == "Crack")
        {
            if (collision.gameObject.GetComponent<CrackCreater>() && (collision.gameObject.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.CREATING ||
            collision.gameObject.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                if (light != null)
                {
                    if (move.GetState() == PlantBossMove.PlantBossMoveState.DETH)
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
                    if (move.GetState() == PlantBossMove.PlantBossMoveState.DETH)
                    {
                        light.Flash();
                    }
                    Destroy(collision.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
