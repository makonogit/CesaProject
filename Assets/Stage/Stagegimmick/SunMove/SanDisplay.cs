//--------------------------------
// �S���F��
// �G���A3�̎��������z��\��
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetStage set = new SetStage();
        if(set.GetAreaNum() == 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
