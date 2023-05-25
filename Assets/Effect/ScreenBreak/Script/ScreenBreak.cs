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
    [SerializeField]
    private GameObject BreakManager;        //�j�ЊǗ��I�u�W�F�N�g
    [Header("�j�Ђ̃I�u�W�F�N�g")]
    public GameObject[] debris = new GameObject[3];// �j�Зp�I�u�W�F�N�g
    [Header("�j�Ђ̐�����")]
    public int amount = 50;                        // �j�Ђ̐�����
    
    [SerializeField,Header("��ʒ[���W")]
    private float screenedge;

    // ���֘A
    [Header("���ʉ�")]
    public AudioClip sound1;// �����t�@�C��
    AudioSource audioSource;// �擾����AudioSource�R���|�[�l���g

    //============================================================
    // - ���������� -

    void Start()
    {
        BreakManager = GameObject.Find("BreakCrystal");
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
      // audioSource.PlayOneShot(sound1);

        //--------------------------------------------------------
        // �j�Ђ𐶐�����

        for (int i = 0; i < amount; i++)
        {
            // �����_���Ɍ`�A���W�A�傫���A��]�������肷��
            int rndDebris = Random.Range(1, 101) % 5;
            int rndX = Random.Range(1, 20);
            int rndY = Random.Range(1, 20);
            float rndSizeX = Random.Range(0.1f, 2.0f);
            float rndSizeY = Random.Range(0.1f, 2.0f);
            float rndSize = Random.Range(0.1f, 2.0f);
            int rndRot = Random.Range(1, 360);

            // �j�Ђ𐶐�
            GameObject obj = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            Transform objTransform = obj.transform;

            // ���W��ύX
            Vector3 pos = objTransform.localPosition;
            pos.x = screenedge + 0.8f * rndX;
            pos.y = -8.0f + 0.8f * rndY;
            pos.z = 0.0f;

            // �傫����ύX
            Vector3 scale;
            scale.x = 0.6f * rndSize;
            scale.y = 0.6f * rndSize;
            scale.z = 1.0f;

            // ��]��ύX
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // �ύX��G�p����
            objTransform.localPosition = pos;    // ���W
            objTransform.localScale = scale;// �傫��
            objTransform.eulerAngles = rot; // ��]

            obj.transform.parent = BreakManager.transform;
        }
    }
    //============================================================
}
