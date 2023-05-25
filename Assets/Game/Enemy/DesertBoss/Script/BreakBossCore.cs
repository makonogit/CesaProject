//---------------------------------
// �S���F��
// ���e�F�{�X�̃R�A����ꂽ����
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBossCore : MonoBehaviour
{
    private Directing_BossLight LightEffect;  // �����G�t�F�N�g�p

    private BGMFadeManager _BGMfadeMana;


    private void Start()
    {
        LightEffect = GameObject.Find("Directing_BossLight").GetComponent<Directing_BossLight>();  //�����p

        _BGMfadeMana = GameObject.Find("Main Camera").GetComponent<BGMFadeManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrackCreater creater = collision.GetComponent<CrackCreater>();
        if (collision.tag == "Crack" &&
            (creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
            creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
        {
            //Destroy(GameObject.Find("BossEnemy").transform.GetChild(0).gameObject);
            LightEffect.Flash();
            _BGMfadeMana.SmallBossBGM();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
