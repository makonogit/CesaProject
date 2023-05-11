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

    public enum Rank
    {
        One,
        Ten,
    }

    [SerializeField] private Rank rank;

    // �O���擾
    [SerializeField] private PlayerStatas status;

    [SerializeField] private Image _number;

    [SerializeField] private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        ////---------------------------------------------------------
        //// �v���C���[������
        //player = GameObject.Find("player");

        //// HaveCrystal���擾
        ////crystal = player.GetComponent<HaveCrystal>();
        //status = player.GetComponent<PlayerStatas>();

        //Number_1 = transform.GetChild(0).GetComponent<Image>();
        //Number_2 = transform.GetChild(1).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (rank) {
            case Rank.One:
                _number.sprite = Number[status.GetCrystal() % 10];
                break;

            case Rank.Ten:
                _number.sprite = Number[status.GetCrystal() / 10];
                break;
        }
    }

    public void Get()
    {
        anim.SetBool("get", false);
    }
}
