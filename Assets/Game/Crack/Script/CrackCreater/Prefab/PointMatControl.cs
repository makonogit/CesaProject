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

    float Wait = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // �ʏ�}�e���A����ݒ�
        NormalMat = GetComponent<SpriteRenderer>().material;
        //FirstCheck = true;
    }

    private void Update()
    {
        if (Wait < 0.1f)
        {
            Wait += Time.deltaTime;
        }
    }

    // �Ђт𔭌�������֐�
    public void FlashCrack()
    {
        if(Wait > 0.1f)
        thisrenderer.material = FlashMat;
    }

    public void NomalCrack()
    {
        thisrenderer.material = NormalMat;
    }

}
