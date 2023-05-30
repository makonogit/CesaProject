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

    [SerializeField]
    private Sprite DethBoss;    //���S���̃X�v���C�g

    private void Start()
    {
        LightEffect = GameObject.Find("Directing_BossLight").GetComponent<Directing_BossLight>();  //�����p

        _BGMfadeMana = GameObject.Find("Main Camera").GetComponent<BGMFadeManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crack")
        {
            CrackCreater creater = collision.GetComponent<CrackCreater>();
            if (creater != null)
            {
                if ((creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
                    creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
                {
                    GameObject boss = GameObject.Find("BossEnemy").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
                    boss.GetComponent<Animator>().enabled = false;
                    boss.GetComponent<SpriteRenderer>().sprite = DethBoss;

                    LightEffect.Flash();
                    _BGMfadeMana.SmallBossBGM();
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }

}
