//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�w��͈͓��Ƀ����_���ɐ��𐶐�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomPosition : MonoBehaviour
{
    // �ϐ��錾

    // ��������Ԋu
    public float CreateTime = 2.0f;
    // �o�ߎ���
    private float time = 0.0f;
    // �������ꂽ�I�u�W�F�N�g
    private GameObject Obj;

    // �O���擾
    [SerializeField]
    [Tooltip("��������GameObject")]
    public GameObject createPrefab;

    // �e�I�u�W�F�N�g
    private GameObject parent;

    // �Q�[���I�u�W�F�N�g
    private GameObject Area_A;
    private GameObject Area_B;

    // ��̃Q�[���I�u�W�F�N�g��Transform
    private Transform rangeA;
    private Transform rangeB;

    private void Start()
    {
        // �I�u�W�F�N�g�T��
        Area_A = GameObject.Find("CreateArea_A");
        Area_B = GameObject.Find("CreateArea_B");

        rangeA = Area_A.GetComponent<Transform>();
        rangeB = Area_B.GetComponent<Transform>();

        parent = GameObject.Find("Stars");
    }

    // Update is called once per frame
    void Update()
    {
        // �O�t���[������̎��Ԃ����Z���Ă���
        time = time + Time.deltaTime;

        // �w�莞�Ԓu���Ƀ����_���ɐ��������悤�ɂ���B
        if (time > CreateTime)
        {
            // rangeA��rangeB��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float x = Random.Range(rangeA.position.x, rangeB.position.x);
            // rangeA��rangeB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float y = Random.Range(rangeA.position.y, rangeB.position.y);

            // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
            Obj = Instantiate(createPrefab, new Vector3(x, y, 0), createPrefab.transform.rotation);
            // Stars�̎q�I�u�W�F�N�g�Ƃ��Đ���
            Obj.transform.parent = parent.transform;

            // �o�ߎ��ԃ��Z�b�g
            time = 0f;
        }
    }
}
