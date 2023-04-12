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

    public bool isFall = false;
    private Rigidbody2D rigid2D;
    private Vector3 initTransform;

    // �O���擾
    private BreakBlock breakBlock = null;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody�擾
        rigid2D = GetComponent<Rigidbody2D>();

        // �������W�ۑ�
        initTransform = transform.position;
    }

    private void Update()
    {
        // isFall��true���d�͂̒l��0�̎�
        if (isFall && rigid2D.gravityScale == 0f)
        {
            rigid2D.gravityScale = 1f;
        }

        // �V��ɒ���t���Ă����ԂȂ�
        if(isFall == false)
        {
            // ���W�Œ�
            transform.position = initTransform;
        }

    }

    // �Ђтɓ����� �Ђ�(isTrriger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ����
            rigid2D.gravityScale = 1.0f;
            isFall = true;

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
