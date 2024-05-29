using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator sniperAnimator;
 public GameObject clip;
 public GameObject clipSolo;

public void TriggerSniperAniamtion()
    {
     
        sniperAnimator.SetTrigger("Sniper");
    }
public void TriggerSmgAniamtion()
    {
     
        sniperAnimator.SetTrigger("Smg");
    }

    public void StartAnimationReload()
    {
        clip.SetActive(false);
        clipSolo.SetActive(true);

    }
 public void StopAnimationReload()
    {
        clip.SetActive(true);
        clipSolo.SetActive(false);

    }

}
