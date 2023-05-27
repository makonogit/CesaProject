//---------------------------------
//�S���F��{��
//���e�FSpriteRenderer�̃I���I�t��؂�ւ���
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOnOff : MonoBehaviour
{
    // �ϐ��錾

    // �_�Ŏ���
    [SerializeField] private float cycle = 1f; // �_�Ŏ���
    private float initCycle; // �����_�Ŏ���

    // �o�ߎ���
    private double time;

    // �_�ł��邩�ǂ���
    public bool isFlashing = false;

    // ���b�ԓ_�ł����邩
    private float flashTime = 2f;
    private float ratio = 0.6f;

    private bool Init = false;

    // ���ł̃f���[�e�B��i1�Ń����_���[on,0��off�j
    [SerializeField, Range(0, 1)] private float dutyRate = 0.5f;

    [SerializeField] private SpriteRenderer spriteRenderer; // ���g�̃X�v���C�g�����_���[

    [SerializeField] private HitEnemy _hitEnemy; // �_�Ŏ��ԂƖ��G���Ԃ����т���

    // Start is called before the first frame update
    void Start()
    {
        //// ���g�̃X�v���C�g�����_���[�擾
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Init == false)
        {
            // �v���C���[�ȊO�̃I�u�W�F�N�g�ɂ����p�ł���悤��
            if (_hitEnemy != null)
            {
                // ���G���ԂƓ_�Ŏ��Ԃ����т�
                flashTime = _hitEnemy.NoDamageTime;
            }

            // �����T�C�N����ێ�
            initCycle = cycle;

            Init = true;
        }

        // �_�Ńt���O����������
        if(isFlashing == true)
        {
            // ���Ԍo��
            time += Time.deltaTime;

            // ����cycle�ŌJ��Ԃ��l�̎擾
            // 0�`cycle�͈̔͂Œl��������
            var repeatRange = Mathf.Repeat((float)time, cycle);

            // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
            // �f���[�e�B���on/off�̊�����ύX���Ă���
            spriteRenderer.enabled = repeatRange >= cycle * (1 - dutyRate);

            // �_�Ŏ��Ԃ�ratio�{�ɂȂ�����
            if (time >= flashTime * ratio)
            {
                // �_�ŃX�s�[�h���߂�
                cycle = initCycle / 2f;
            }

             //�_�Ŏ��Ԃ��o�߂�����
            if(time >= flashTime)
            {
                // �_�ł��Ȃ�
                isFlashing = false;

                // ������
                time = 0f;

                // �_�Ŏ���������
                cycle = initCycle;

                // ��������ԂŏI���Ȃ��悤��
                spriteRenderer.enabled = true;
            }
        }
    }

    // �_�ł̒�����ς��Ȃ����Ԃ��t���O���Z�b�g������
    public void SetFlash(bool _flash,float _time)
    {
        isFlashing = _flash;
        flashTime = _time;
    }

    // �_�Ŏ��Ԃ͂��̂܂܂Ńt���O���Z�b�g
    public void SetFlash(bool _flash)
    {
        isFlashing = _flash;
    }
}
