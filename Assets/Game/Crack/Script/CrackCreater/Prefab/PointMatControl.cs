//---------------------------------
// �S���F��
// ���e�F�}�e���A���̊Ǘ�
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMatControl : MonoBehaviour
{
    public Material NormalMat;      //�@�ʏ��Material
    public Material NormalEndMat;   //�@�ʏ�̐�[Material
    public Material FlashMat;       //  ����Material
    public Material FlashEndMat;    //  ������[Material

    [SerializeField, Header("SpeiteRenderer")]
    private SpriteRenderer thisrenderer;

    //float Wait = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // �ʏ�}�e���A����ݒ�
        NormalMat = GetComponent<SpriteRenderer>().material;
        //FirstCheck = true;
    }

    private void Update()
    {
    }

    // �Ђт𔭌�������֐�
    public void FlashCrack()
    {

        thisrenderer.material = FlashMat;
    }

    public void NomalCrack()
    {
        thisrenderer.material = NormalMat;
    }

    public void NormalCrackEnd()
    {
        thisrenderer.material = NormalEndMat;
    }

    public void FlashCrackEnd()
    {
        thisrenderer.material = FlashEndMat;
    }

}
