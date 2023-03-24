//---------------------------------------
//�S���ҁF��{
//���e�@�F�H��̏�ԊǗ�
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBoxManager : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public bool isPassedNails = false; // �B���v���C���[�ɓn������
    private bool Finished = false; // ���̃X�N���v�g�̎d�����I�������

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃X�N���v�g�ł�肽�����Ƃ��I����ĂȂ����
        if (!Finished)
        {
            // ��x�B��n���Ă����
            if (isPassedNails)
            {
                // �������ɂ���
                sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Finished = true;
            }
        }
    }
}
