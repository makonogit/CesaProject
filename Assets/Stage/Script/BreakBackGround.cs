//----------------------------------
// �S���F�����S
// ���e�F����Ă����w�i�̕`��
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBackGround : MonoBehaviour
{
    //-------------------------
    // �ϐ��錾

    private List<GameObject> BackCrystal = new List<GameObject>();   // �w�i�̃N���X�^��
    private float Alpha;                    // �w�i�N���X�^���̃��l

    // Start is called before the first frame update
    void Start()
    {

        if(BackCrystal.Count > 0)
        {
            BackCrystal.Clear();
        }

        // �w�i�N���X�^�����擾
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "Crystal")
            {
                BackCrystal.Add(transform.GetChild(i).gameObject);
            }
        }

        // �������l��ێ�
        Alpha = BackCrystal[0].GetComponent<SpriteRenderer>().color.a;

    }

    //--------------------
    //�@�w�i������֐�
    //  �����F�Ȃ�
    //  �߂�l�F�Ȃ�
    public void BreakBack()
    {
        Alpha += (1.0f / 6);

        for (int i = 0; i < BackCrystal.Count; i++)
        {
            BackCrystal[i].GetComponent<SpriteRenderer>().color =
                new Color(1.0f, 1.0f, 1.0f, Alpha);
        }
    }
}
