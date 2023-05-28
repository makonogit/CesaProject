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
    private string PlayerTag = "Player"; // �v���C���[�̃^�O��

    // �����Ă��邩
    public bool isFall = false;
    // �Ђт�����������
    private bool CrackHit = false;
    // �U��������
    private bool Vibration = false;
    // �U����̃A�j���[�V�������Đ����ꂽ��
    private bool fallAnimFinish = false;

    [SerializeField] private Rigidbody2D _rigid2D; // rigidbody
    private Vector3 initTransform; // �������W

    // �O���擾
    private BreakBlock breakBlock = null;
    [SerializeField] private VibrationObject vibration;
    private GameObject player;
    private HitEnemy _hitEnemy;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PolygonCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        // �������W�ۑ�
        initTransform = transform.position;

        // �U���p�X�N���v�g�擾
        vibration = GetComponent<VibrationObject>();

        // �v���C���[�擾
        player = GameObject.Find("player");

        // �v���C���[�Ƀ_���[�W��^���邽��
        _hitEnemy = player.GetComponent<HitEnemy>();
    }

    private void Update()
    {
        // isFall��true���d�͂̒l��0�̎�
        if (isFall && _rigid2D.gravityScale == 0f)
        {
            _rigid2D.gravityScale = 1.0f;
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
            // ��番���A�j���[�V�������I����Ă����
            if (fallAnimFinish == true)
            {
                // �U�����I����Ă�����
                if (vibration.GetVibration() == false)
                {
                    // ����
                    _rigid2D.gravityScale = 1.0f;
                    isFall = true;

                    // ���C���[�ύX
                    this.gameObject.layer = 11;

                }
            }else
                {
                    // ��番���A�j���[�V�����Đ��J�n
                    _anim.SetBool("FallIcicle", true);
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
                //// ����
                //Destroy(this.gameObject);

                // �����A�j���[�V����
                _anim.SetBool("BreakIcicle", true);
                //Debug.Log("����");
            }

            // �G�ɓ�����
            if(collision.gameObject.tag == EnemyTag)
            {
                //// ������
                //Destroy(this.gameObject);

                //// �����A�j���[�V����
                //_anim.SetBool("BreakIcicle", true);

                // �G����
                //Destroy(collision.gameObject);

                EnemyMove enemyMove = collision.gameObject.GetComponent<EnemyMove>();
                //Debug.Log(enemyMove);
                if (enemyMove != null)
                {
                    // �G���j��Ԃɂ���
                    enemyMove.EnemyAI = EnemyMove.AIState.DEATH; // ���j���A�p�[�e�B�N���A�f�X�g���C
                }

            }

            // �X�ɓ�����
            if(collision.gameObject.tag == IceTag)
            {
                //// ������
                //Destroy(this.gameObject);

                // �����A�j���[�V����
                //_anim.SetBool("BreakIcicle", true);

                // �ڐG�����I�u�W�F�N�g�����X����X�N���v�g�擾
                breakBlock = collision.gameObject.GetComponent<BreakBlock>();

                // �X����
                breakBlock.Func_BreakBlock();
            }

            // �v���C���[�ɓ�����
            if(collision.gameObject.tag == PlayerTag)
            {
                //// ������
                //Destroy(this.gameObject);

                // �����A�j���[�V����
                _anim.SetBool("BreakIcicle", true);

                _hitEnemy.HitPlayer(transform);
            }
        }
    }

    private void AnimFinish()
    {
        fallAnimFinish = true;
    }
    private void Invisible()
    {
        // �s����
        _spriteRenderer.enabled = false;
        // �����蔻�薳����
        _collider.enabled = false;
    }
}
