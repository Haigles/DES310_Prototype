using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Increase : MonoBehaviour
{
    public Animator animator = null;
    public TMP_Text text = null;

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            Destroy(this.gameObject);
        }
    }
}
