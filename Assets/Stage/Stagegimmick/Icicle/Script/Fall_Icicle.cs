//-----------------------------
//�S���F��{��
//���e�F�ЂтƐڐG����Ɨ�������
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Icicle : MonoBehaviour
{
    // �ϐ��錾

    // �^�O����
    private string GroundTag = "Ground"; // �n�ʂ̃^�O��
    private string CrackTag = "Crack"; // �ЂуI�u�W�F�N�g�̃^�O��
    private string EnemyTag = "Enemy"; // �G�I�u�W�F�N�g�̃^�O��
    private string IceTag = "Ice"; // �X�u���b�N�̃^�O��

    // �����Ă��邩
    public bool isFall = false;
    // �Ђт�����������
    private bool CrackHit = false;
    // �U��������
    private bool Vibration = false;

    private Rigidbody2D rigid2D; // rigidbody
    private Vector3 initTransform; // �������W

    // �O���擾
    private BreakBlock breakBlock = null;
    private VibrationObject vibration; 

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody�擾
        rigid2D = GetComponent<Rigidbody2D>();

        // �������W�ۑ�
        initTransform = transform.position;

        // �U���p�X�N���v�g�擾
        vibration = GetComponent<VibrationObject>();
    }

    private void Update()
    {
        // isFall��true���d�͂̒l��0�̎�
        if (isFall && rigid2D.gravityScale == 0f)
        {
            rigid2D.gravityScale = 1.0f;
        }

        // �V��ɒ���t���Ă����ԂȂ�
        if(isFall == false && !Vibration)
        {
            // ���W�Œ�
            transform.position = initTransform;
        }

        // �Ђтɓ��������t���O���o���Ă���
        if (CrackHit)
        {
            if (Vibration == false)
            {
                // �U��������
                vibration.SetVibration(0.7f);
                CrackHit = false;
                Vibration = true;
            }
        }

        // �U�������Ȃ�
        if (Vibration)
        {
            if(vibration.GetVibration() == false)
            {
                // ����
                rigid2D.gravityScale = 1.0f;
                isFall = true;
            }
        }

    }

    // �Ђтɓ����� �Ђ�(isTrriger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // �ЂтɐڐG�����t���O���Ă�
            CrackHit = true;

            // �Ђт�����
            //Destroy(collision.gameObject);
        }
    }

    // �n�ʂɓ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������Ԃ̎���
        if (isFall)
        {
            // �n�ʂɂ���������
            if (collision.gameObject.tag == GroundTag)
            {
                // ����
                Destroy(this.gameObject);
            }

            // �G�ɓ�����
            if(collision.gameObject.tag == EnemyTag)
            {
                // ������
                Destroy(this.gameObject);

                // �G����
                Destroy(collision.gameObject);
            }

            // �X�ɓ�����
            if(collision.gameObject.tag == IceTag)
            {
                // ������
                Destroy(this.gameObject);

                // �ڐG�����I�u�W�F�N�g�����X����X�N���v�g�擾
                breakBlock = collision.gameObject.GetComponent<BreakBlock>();

                // �X����
                breakBlock.Func_BreakBlock();
            }
        }
    }
}
