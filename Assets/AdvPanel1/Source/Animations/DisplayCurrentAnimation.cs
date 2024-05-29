using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayCurrentAnimation : MonoBehaviour {


    public Text textAnimationName;
    public Animator anim;
   
    AnimatorClipInfo[] m_CurrentClipInfo;
    float m_CurrentClipLength;

    // Update is called once per frame
    void Update () {


        m_CurrentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        textAnimationName.text = m_CurrentClipInfo[0].clip.name;

    }
}
