//-----------------------------------
//�@�S���F��
//�@���e�F�Ђѓ���J�n
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackStart : MonoBehaviour
{
    [SerializeField,Header("�Ђѓ���N���X�^���̃��X�g")]
    private List<ClearCrack> _clearacks;

    [SerializeField, Header("�Ō�̂Ђ�")]
    private SpriteRenderer _endcrack;

    public bool StartFlg = false; //�j��J�n�A�j���[�V����

    public bool EndFlg = false;�@//�I��

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlg)
        {
            // �Ђъ���X�N���v�g��L����
            for (int i = 0;i< _clearacks.Count;i++)
            {
                _clearacks[i].enabled = true;
            }
            StartFlg = false;
        }

        // �Ō�̂Ђт��\�����ꂽ��I������
        if (_endcrack.enabled)
        {
            EndFlg = true;
        }

    }

    //----------------------
    // �Ђъ���J�n�֐�
    //----------------------
    public void StartCrack()
    {
        StartFlg = true;
    }

}
