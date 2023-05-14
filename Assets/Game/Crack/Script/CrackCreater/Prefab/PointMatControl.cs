//---------------------------------
// �S���F��
// ���e�F�}�e���A���̊Ǘ�
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMatControl : MonoBehaviour
{
    public Material NormalMat; //�@�ʏ��Material
    public Material FlashMat;  //  ����Material

    [SerializeField, Header("SpeiteRenderer")]
    private SpriteRenderer thisrenderer;

    // Start is called before the first frame update
    void Start()
    {
        // �ʏ�}�e���A����ݒ�
        //NormalMat = GetComponent<SpriteRenderer>().material;
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

}
