//---------------------------------
// �S���F��
// ���e�F�{�X�̃R�A����ꂽ����
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBossCore : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɓ���������{�X���Ə���
        if (collision.tag == "Player")
        {
            Destroy(GameObject.Find("BossEnemy").transform.GetChild(0).gameObject);
            Destroy(gameObject);
        }
    }

}
