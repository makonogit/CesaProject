//----------------------------------------------------------
// �S���ҁF��{��
// ���e  �F�{�X�̓ːi���Ƀv���C���[�ɓ���������~�܂�
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingStop : MonoBehaviour
{
    private string PlayerTag = "Player";

    // �O���擾
    private TownBossMove townBossMove;

    // Start is called before the first frame update
    void Start()
    {
        townBossMove = transform.parent.gameObject.GetComponent<TownBossMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�Ɠ���������
        if(collision.gameObject.tag == PlayerTag)
        {
            // �����������𑗂�
            townBossMove.SetHitPlayer(true);
        }
    }
}
