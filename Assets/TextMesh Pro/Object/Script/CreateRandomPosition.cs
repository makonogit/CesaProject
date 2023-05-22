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

    [Header("��������"),SerializeField] private int CreateNumAll = 20;
    private int CreateNumNow = 0;

    // �������ꂽ�I�u�W�F�N�g
    private GameObject Obj;

    // �O���擾
    [SerializeField]
    [Tooltip("��������GameObject")]
    public GameObject createPrefab;

    // ��̃Q�[���I�u�W�F�N�g��Transform
    [SerializeField] private Transform rangeA;
    [SerializeField] private Transform rangeB;

    [SerializeField] private Transform _thisTransform;

    private void Start()
    {
        // �I�u�W�F�N�g�T��
        //Area_A = GameObject.Find("CreateArea_A");
        //Area_B = GameObject.Find("CreateArea_B");

        //rangeA = Area_A.GetComponent<Transform>();
        //rangeB = Area_B.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // �����\�葍����菭�Ȃ���Βǉ�����
        if (CreateNumNow < CreateNumAll)
        {
            // �O�t���[������̎��Ԃ����Z���Ă���
            time = time + Time.deltaTime;

            // �w�莞�Ԓu���Ƀ����_���ɐ��������悤�ɂ���B
            if (time > CreateTime)
            {
                // ����͈͓̔��̍��W�������_���Ŏ擾
                var pos = GetSpawnPos();

                // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
                Obj = Instantiate(createPrefab, new Vector3(pos.x, pos.y, 0), createPrefab.transform.rotation);
                // Stars�̎q�I�u�W�F�N�g�Ƃ��Đ���
                Obj.transform.parent = _thisTransform;

                // �o�ߎ��ԃ��Z�b�g
                time = 0f;

                // �������J�E���g
                CreateNumNow++;
            }
        }
    }

    public Vector2 GetSpawnPos()
    {
        // �߂�l�p�ϐ�
        Vector2 pos;

        // rangeA��rangeB��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
        pos.x = Random.Range(rangeA.position.x, rangeB.position.x);
        // rangeA��rangeB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
        pos.y = Random.Range(rangeA.position.y, rangeB.position.y);

        return pos;
    }
}
