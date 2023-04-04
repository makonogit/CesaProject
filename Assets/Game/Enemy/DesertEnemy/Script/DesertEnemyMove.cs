//-------------------------------
//�@�S���F�����S
//�@���e�F�����̂��ѓG�̈ړ�
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertEnemyMove : MonoBehaviour
{
    //-------------------------
    //�@�ϐ��錾

    //---------------------------
    // �O���擾
    GameObject Player;          // �v���C���[�̃I�u�W�F�N�g
    Transform PlayerTrans;      // �v���C���[��Transform


    [SerializeField, Header("��������j")]
    private GameObject Needle;
    [SerializeField, Header("�v���C���[�����m���鋗��")]
    private float PlayerDistance = 2.0f;
    [SerializeField, Header("�͂𗭂߂鎞��")]
    private float PowerMaxTime = 1.5f;


    private float PowerTime = 0.0f; // ���ߎ��Ԍv���p

    Transform ThisTrans;        // ���g��Transform
    Animator ThisAnim;          // ���g��Animator
    CapsuleCollider2D ThisCol;  // ���g��Collider

   
    public enum DesertEnemyState
    {
        NONE,       // �ҋ@���
        FACE,       // ����o��
        ATTACK,     // �U�����
        ATTACKEND   // �U���I��
    }

    public DesertEnemyState EnemyState; //��ԊǗ��p�ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------------
        // Player�̏����擾
        Player = GameObject.Find("player");
        if (Player == null) Debug.Log("Player�̃I�u�W�F�N�g���擾�ł��܂���ł���");
        PlayerTrans = Player.transform;

        ThisTrans = transform;                          //���g��Transform��ϐ���
        ThisAnim = GetComponent<Animator>();            //���g��Animator���擾
        ThisCol = GetComponent<CapsuleCollider2D>();    // ���g��Collider���擾

        EnemyState = DesertEnemyState.NONE;     //�������Ă��Ȃ���Ԃɐݒ�

    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------------------
        //�@�v���C���[�Ǝ��g�̋��������߂�
        float Distance = Vector3.Magnitude(PlayerTrans.position - ThisTrans.position);
        
        //-----------------------------------------
        //�@�v���C���[�����m���鋗���ɗ�����s��
        if(Distance < PlayerDistance)
        {
            //------------------------------------------
            //�@�܂�����o���Ă��Ȃ�������
            if (EnemyState == DesertEnemyState.NONE)
            {
                OpenFace();     // ����o��
                EnemyState = DesertEnemyState.FACE;
            }

            //------------------------------------------
            // ����o���Ă�����
            if (EnemyState == DesertEnemyState.FACE)
            {
                Debug.Log(EnemyState);
                PowerTime += Time.deltaTime;    // ���Ԍv��

                //--------------------------------
                // ���Ԍo�߂�����U���J�n
                if (PowerTime > PowerMaxTime)
                {
                    EnemyState = DesertEnemyState.ATTACK;
                    PowerTime = 0.0f;
                }
            }

            if(EnemyState == DesertEnemyState.ATTACK)
            {
                Attack();
            }

        }


    }

    //-------------------------
    //�@����o������
    private void OpenFace()
    {
        // ����o���A�j���[�V�������Đ�
        ThisAnim.SetBool("OpenFace", true);
        ThisCol.size = new Vector2(ThisCol.size.x, 10.0f);
        ThisTrans.position = new Vector3(ThisTrans.position.x, ThisTrans.position.y + 0.14f);

    }

    //--------------------------------
    //�@�U������
    private void Attack()
    {
        //------------------------------------------------
        //�@�v���C���[�Ƃ̊p�x�����߂�
        Vector2 Distance = PlayerTrans.position - ThisTrans.position;
        float Radian = Mathf.Atan2(Distance.y, Distance.x);
        float Angle = Radian * Mathf.Rad2Deg;

        //�@�p�x�𐳋K��
        if(Angle < 0)
        {
            Angle += 360;
        }

        Vector3 CreatePos = new Vector3(ThisTrans.position.x + (0.5f * Mathf.Cos(Angle * (Mathf.PI / 180))), ThisTrans.position.y + (0.5f * Mathf.Sin(Angle * (Mathf.PI / 180))),0.0f);
        Instantiate(Needle, CreatePos, Quaternion.Euler(0, Angle, 0));

        EnemyState = DesertEnemyState.FACE;


    }

}
