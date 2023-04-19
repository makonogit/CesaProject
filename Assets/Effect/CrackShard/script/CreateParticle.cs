//---------------------------------
//�S���F��{��
//���e�F�Ђт𐶐������炽�������Ƃ��납�痱���o�Ă���
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticle : MonoBehaviour
{
    // �ϐ��錾

    // ��������p�[�e�B�N���̃v���n�u
    public GameObject particle;

    // ��񂽂��������x��������
    private bool Create = false;

    // �O���擾
    private Hammer hammer;

    private void Start()
    {
        // �v���C���[�̎���Hammer�X�N���v�g�擾
        hammer = GetComponent<Hammer>();
    }

    // Update is called once per frame
    void Update()
    {
        // �Ђт𐶐������^�C�~���O
        if(hammer.hammerstate == Hammer.HammerState.HAMMER && Create == false)
        {
            // �R���[�`���Ăяo��
            StartCoroutine(CreateObject());

            Create = true;
        } 


        if(hammer.hammerstate == Hammer.HammerState.NONE && Create == true)
        {
            Create = false;
        }
    }

    IEnumerator CreateObject()
    {
        // �A�j���[�V������������~
        yield return new WaitForSeconds(0.25f);

        Vector2 worldPosition = hammer.CrackPointList[0];

        Instantiate(particle, new Vector3(worldPosition.x, worldPosition.y, 0f), Quaternion.identity);
    }
}
