//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�w�i�I�u�W�F�N�g�̉Ԃ���]����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFlower : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    [Header("��]���x")]
    public float RotateSpeed = 5.0f; // �Ԃ̉�]���x

    public enum PATTERN
    {
        LEFT,
        RIGHT,
        MIX
    }
    [Header("��]���@���ς��")]
    public PATTERN pattern = PATTERN.LEFT;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g��Transform���擾����

    // Start is called before the first frame update
    void Start()
    {
        // Transform���擾����
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��b�Ԃ�RotateSpeed�̐��l��������]����
        switch (pattern)
        {
            case PATTERN.LEFT:

                // �����v���
                thisTransform.Rotate(new Vector3(0, 0, 10 * RotateSpeed * Time.deltaTime));
                break;

            case PATTERN.RIGHT:

                // ���v���
                thisTransform.Rotate(new Vector3(0, 0, -10 * RotateSpeed * Time.deltaTime));
                break;

            case PATTERN.MIX:

                // ���v��聨�����v��聨���v����...�J��Ԃ�
                thisTransform.Rotate(new Vector3(0, 0, 10 * Mathf.Cos(Time.time) * RotateSpeed * Time.deltaTime));
                break;
        }
    }
}
