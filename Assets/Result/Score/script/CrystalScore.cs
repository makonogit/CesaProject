//----------------------------------------------------------------
// �S���ҁF�����V�S
// ���e�@�F�N���X�^���̃X�R�A��\������
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrystalScore : MonoBehaviour
{
    //=================================================
    // - �ϐ� -

    [Header("�N���X�^���̌��Ђ̐���\������e�L�X�g")]
    [SerializeField] TextMeshProUGUI crystalText;// �\������e�L�X�g

    int score; // �X�R�A

    //=================================================
    // - �X�V���� -

    void Update()
    {
        //---------------------------------------------
        // �\������e�L�X�g���X�V����
        crystalText.text = "crystal:" + score;
        //---------------------------------------------
    }

    //=================================================
}
