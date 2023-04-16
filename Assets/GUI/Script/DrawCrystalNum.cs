//---------------------------------------
//�S���ҁF��{
//���e�@�F�N���X�^���l������\���A���A���^�C���X�V
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCrystalNum : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    [SerializeField, Header("�����̃t�H���g")]
    List<Sprite> Number;

    // �O���擾
    private GameObject player; // �v���C���[�������ĕێ�����
    private PlayerStatas status;

    private Image Number_1;    // 1�̈�
    private Image Number_2;    // 10�̈�


    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �v���C���[������
        player = GameObject.Find("player");

        // HaveCrystal���擾
        //crystal = player.GetComponent<HaveCrystal>();
        status = player.GetComponent<PlayerStatas>();

        Number_1 = transform.GetChild(0).GetComponent<Image>();
        Number_2 = transform.GetChild(1).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        Number_1.sprite = Number[status.GetCrystal() % 10];
        Number_2.sprite = Number[status.GetCrystal() / 10];
    }
}
