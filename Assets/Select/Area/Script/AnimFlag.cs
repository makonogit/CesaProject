using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFlag : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("animatorコンポーネントを取得できませんでした。");
    }
    public void offFlag() 
    {
        animator.SetBool("isPush", false);
    }
}
