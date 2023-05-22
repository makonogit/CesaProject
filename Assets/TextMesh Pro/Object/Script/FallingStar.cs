//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�w�i�I�u�W�F�N�g�̐����~��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStar : MonoBehaviour
{
    //-�ϐ��錾-

    [Header("�����~�鑬�x")]
    [SerializeField] private float FallSpeed = 10.0f;
    [Header("���X�|�[���܂ł̎���")]
    [SerializeField] private float RespawnTime = 3.0f;
    // �o�ߎ���
    private float time = 0.0f;

    // �O���擾
    private Transform thisTransform;
    private GameObject parent;
    private CreateRandomPosition _createRandomPos;

    private void Start()
    {
        // Transform�擾
        thisTransform = GetComponent<Transform>();

        parent = transform.parent.gameObject;
        _createRandomPos = parent.GetComponent<CreateRandomPosition>();

        // ���̍~�鑬�x�������_����
        // 10�`20
        FallSpeed = Random.Range(10.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ԍo��
        time += Time.deltaTime;

        // ���܂�Ă����莞�Ԍo�߂�������W�����߂Ȃ����ă��X�|�[��
        if(time > RespawnTime || thisTransform.position.y < -6f)
        {
            // ����͈͂��烉���_���ȍ��W���擾���Ă��̏�Ɉړ�������
            var pos = _createRandomPos.GetSpawnPos();

            thisTransform.position = new Vector3(pos.x, pos.y, 0f);

            // ���x�Đݒ�
            FallSpeed = Random.Range(10.0f, 20.0f);

            // ������
            time = 0f;
        }

        // ���������
        Vector3 fall = new Vector3(-0.5f, -1.0f, 0.0f);

        // ����������Ɍ��ݒn����ړ�������
        thisTransform.Translate(fall * FallSpeed * Time.deltaTime);
    }
}
