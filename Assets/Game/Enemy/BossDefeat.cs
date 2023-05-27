//---------------------------------------
//�@�S���F�����S
//  ���e�F�{�X���|�ꂽ��̏���
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    [SerializeField, Header("��������Ђ�")]
    private GameObject CoreCrack;

    [SerializeField, Header("�Ђт�Transform")]
    private Transform CrackTrans;

    private GameObject CoreManager;  // �Ђт��Ǘ����Ă���I�u�W�F�N�g

    private StageStatas statas;     //�@�X�e�[�W�X�e�[�^�X�p

    private bool Create = false;    //�����t���O 

    // Start is called before the first frame update
    void Start()
    {
        CoreManager = GameObject.Find("Core");
        statas = transform.root.transform.GetChild(0).GetComponent<StageStatas>();
    }

    // Update is called once per frame
    void Update()
    {
        // �{�X����������
        if(transform.childCount == 0 && CoreManager.transform.childCount == 0 && !Create)
        {
            //�@�Ђѐ���
            GameObject obj = Instantiate(CoreCrack, CrackTrans);
            obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            obj.transform.parent = CoreManager.transform;   // Core�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɂ���
            statas.SetStageCrystal(1);
            Create = true;
        }
        
    }
}
