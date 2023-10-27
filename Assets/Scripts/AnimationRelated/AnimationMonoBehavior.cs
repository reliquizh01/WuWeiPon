using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationMonoBehavior : MonoBehaviour
{
    public Animation myAnim;
    public void Play(string name, Action callback = null)
    {
        myAnim.Play(name);

        if(callback != null)
        {
            StartCoroutine(doActionAfter(myAnim.GetClip(name).length, callback));
        }
    }

    IEnumerator doActionAfter(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback.Invoke();
    }
}
