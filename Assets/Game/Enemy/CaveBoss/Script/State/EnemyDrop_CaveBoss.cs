//=========================================
// �S���F�����V�S
// ���e�F���A�̃{�X�̓G���~�点��U��
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // *** �C���X�^���X ***

    public static EnemyDrop_CaveBoss instance;// ���̃N���X�̃C���X�^���X

    //-------------------------------------
    // *** �U���֘A ***

    [Header("�~�点��G")]
    public GameObject needleEnemy;// �~�点��G
    [Header("�~�点��G�͈̔�")]
    public float dropRange = 15.0f;

    //-------------------------------------
    // *** ���W�֘A ***

    Vector2 startPos;// �����ʒu

    //=====================================
    // *** ���������� ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** �ϐ��̏����� ***

        // �����ʒu��ۑ�
        startPos = transform.position;

        // ���̃N���X�̃C���X�^���X�𐶐�
        if (instance == null)
        {
            instance = this;
        }
    }

    //=====================================
    // *** �G���~�点�鏈�� ***
    //
    // �����@�F����
    // �߂�l�F�U�����I���������itrue�F�I���Afalse�F�U�����j
    //=====================================

    public bool EnemyDrop()
    {
        //---------------------------------
        // *** �G�𐶐� ***

        for (int i = 0; i < 3; i++)
        {
            // �G�𐶐�
            GameObject obj = Instantiate(needleEnemy, transform.position, Quaternion.identity);

            int rndX = Random.Range(1, 20);

            // ���W��ύX
            Transform objTransform = obj.transform;
            Vector3 pos = objTransform.position;
            pos.x = startPos.x + -dropRange + (dropRange * 0.1f) * rndX;
            pos.y = transform.position.y;

            // ���W�̕ύX��G�p����
            objTransform.position = pos;
        }

        return true;
    }
}
