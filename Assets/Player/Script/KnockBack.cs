//---------------------------------
//�S���F��{��
//���e�F��_���[�W���Ƀm�b�N�o�b�N
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // �ϐ��錾

    // �f�o�b�O�p
    public bool knockback = false; // true�ɂ�����m�b�N�o�b�N����
    [Header("�m�b�N�o�b�N����")]
    public Vector2 KnockBackPower = new Vector2(1f, 0.5f); // �m�b�N�o�b�N����
    [Header("�R���[�`���̌J��Ԃ���")]
    public int CoroutineNum = 10; // �R���[�`�����J��Ԃ���
    private float direction = 1f; // �v���C���[�̌����ۑ��p�ϐ�

    private Rigidbody2D rigid2D; // rigidbody�p�ϐ�
    private Transform thisTransform; // Transform�p�ϐ�
    private GameObject thisObj; // ���̃X�N���v�g���R���|�[�l���g���ꂽ�I�u�W�F�N�g�p�ϐ�

    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        thisTransform = GetComponent<Transform>();

        thisObj = gameObject;
    }

    private void Update()
    {
        if(knockback == true)
        {
            KnockBack_Func();

            knockback = false;
        }
    }

    public void KnockBack_Func()
    {
         // �I�u�W�F�N�g�̌���������o��
         direction = thisTransform.localScale.x / Mathf.Abs(thisTransform.localScale.x); // ���݂̎����̃X�P�[�������̒l�̐�Βl�Ŋ����ĕ��������

        StartCoroutine(KnockBack_Coroutine());
    }

    private IEnumerator KnockBack_Coroutine()
    {
        int i = 0;
        while (i < 10)
        {
            // ��~���Ԍ���
            float WaitTime = 0.02f;

            // �w�莞�ԏ�����~
            yield return new WaitForSeconds(WaitTime);

            // �w�莞�Ԍo�߂����̂ŏ����ĊJ
            // �m�b�N�o�b�N����
            // �v���C���[�̌����Ă�������Ƌt�����Ƀm�b�N�o�b�N
            thisTransform.Translate(-direction * KnockBackPower.x * WaitTime, KnockBackPower.y * WaitTime,0f);

            //�J�E���g
            i++;

        }
    }
}
