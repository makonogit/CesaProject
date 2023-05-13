//---------------------------------
// �S���F��
// ���e�F�{�X�̃R�A����ꂽ����
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBossCore : MonoBehaviour
{
    [SerializeField, Header("Boss")]
    private GameObject Boss;

    private void Start()
    {
        Boss = GameObject.Find("BossEnemy").transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɓ���������{�X���Ə���
        if (collision.tag == "Player")
        {
            Destroy(Boss);
            Destroy(gameObject);
        }
    }

}
