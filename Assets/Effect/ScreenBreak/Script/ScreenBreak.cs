//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F��ʂ����ꂽ���o
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �j�Њ֘A
    [Header("�j�Ђ̃I�u�W�F�N�g")]
    public GameObject[] debris = new GameObject[3];// �j�Зp�I�u�W�F�N�g
    [Header("�j�Ђ̐�����")]
    public int amount = 50;                        // �j�Ђ̐�����

    // ���֘A
    [Header("���ʉ�")]
    public AudioClip sound1;// �����t�@�C��
    AudioSource audioSource;// �擾����AudioSource�R���|�[�l���g

    //============================================================
    // - ���������� -

    void Start()
    {
        // AudioSource�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();

        Break();
    }

    //============================================================
    // - ��ʂ����鉉�K�����鏈�� -
    
    void Break()
    {
        //--------------------------------------------------------
        // �����t�@�C�����Đ�����
        audioSource.PlayOneShot(sound1);

        //--------------------------------------------------------
        // �j�Ђ𐶐�����

        for (int i = 0; i < amount; i++)
        {
            // �����_���Ɍ`�A���W�A�傫���A��]�������肷��
            int rndDebris = Random.Range(0, 3);
            int rndX = Random.Range(1, 20);
            int rndY = Random.Range(1, 20);
            int rndSizeX = Random.Range(1, 10);
            int rndSizeY = Random.Range(1, 10);
            int rndRot = Random.Range(1, 360);

            // �j�Ђ𐶐�
            GameObject obj = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            Transform objTransform = obj.transform;

            // ���W��ύX
            Vector3 pos = objTransform.position;
            pos.x = -8.0f + 0.8f * rndX;
            pos.y = -8.0f + 0.8f * rndY;
            pos.z = 0.0f;

            // �傫����ύX
            Vector3 scale;
            scale.x = 0.2f * rndSizeX;
            scale.y = 0.2f * rndSizeY;
            scale.z = 0.2f;

            // ��]��ύX
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // �ύX��G�p����
            objTransform.position = pos;    // ���W
            objTransform.localScale = scale;// �傫��
            objTransform.eulerAngles = rot; // ��]
        }
    }
    //============================================================
}
