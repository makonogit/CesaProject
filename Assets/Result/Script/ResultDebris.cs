//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F���U���g�̔j��
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDebris : MonoBehaviour
{
    //============================================================
    // *** �ϐ��錾 ***
    //============================================================
   
    //-------------------------------------
    // �ړ��֘A
    //-------------------------------------

    // ��]���x
    Vector3 rot_speed = new Vector3(0.0f, 0.0f,0.05f);

    // �ړ����x
    public Vector2 move_speed;

    public float acceleration;

    // �ړ������i�P�ʃx�N�g���j
    Vector2 direction;

    //�ڕW�n�_
    Vector2 destination; 

    GameObject obj;
    ResultManager resultManager;

    //============================================================
    // *** ���������� ***
    //============================================================

    void Start()
    {
        //-------------------------------------
        // �e�L�X�g�̎��ӂ�ړI�n�ɐݒ�
        //-------------------------------------

        // �ڕW�n�_��������
        obj = GameObject.Find("Result_StageClear");
        resultManager = obj.GetComponent<ResultManager>();
        destination = obj.transform.position;

        //-------------------------------------
        // �����_���Ɉړ����������肷��
        //-------------------------------------

        // �����𐶐�
        int rndX = Random.Range(-1, 1 + 1);
        int rndY = Random.Range(-1, 1 + 1);
        // �����x�N�g����������
        direction.x = rndX;
        direction.y = rndY;
    }

    //============================================================
    // *** �X�V���� ***
    //============================================================

    void Update()
    {

        //-------------------------------------
        // �j�Ђ���]������
        //-------------------------------------

        // ���݂̊p�x���擾
        Vector3 rot = this.transform.eulerAngles;
        // ���݂̊p�x�ɉ�]���x�����Z
        rot += rot_speed;
        // ���݂̊p�x���X�V
        this.transform.eulerAngles = rot;

        //-------------------------------------
        // �j�Ђ��ړ�������
        //-------------------------------------

        // ���݂̍��W���擾
        Vector2 position = this.transform.position;
        // �x�N�g���̐��������߂�
        Vector2 components;
        components.x = destination.x - this.transform.position.x;
        components.y = destination.y - this.transform.position.y;
        // �x�N�g���̑傫�������߂�
        float magnitude = (float)Mathf.Sqrt(components.x * components.x + components.y * components.y);
        if (resultManager.GetMoveFlg() == true)
        {
            // �x�N�g���𐳋K��
            direction.x = components.x / magnitude;
            direction.y = components.y / magnitude;
            move_speed.x += acceleration;
            move_speed.y += acceleration;

            // �x�N�g���̑傫����1�����Ȃ炱�̃I�u�W�F�N�g������
            if (magnitude < 1)
            {
                Destroy(gameObject);
            }
        }
        // ���݂̍��W�Ɉړ��x�N�g�������Z
        position += move_speed * direction;
        // ���݂̍��W���X�V
        this.transform.position = position;   
    }
}


