using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFlag : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("animator�R���|�[�l���g���擾�ł��܂���ł����B");
    }
    public void offFlag() 
    {
        animator.SetBool("isPush", false);
    }
}
