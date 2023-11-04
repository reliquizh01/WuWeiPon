using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpriteRendererNotificationBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    RectTransform myRectTransform;

    private bool isPlaying = false;
    [SerializeField] private float directionSpeed = 5.0f;
    [SerializeField] private float maxDuration = 2.0f;
    private float curDuration = 0.0f;
    private void Start()
    {
        myRectTransform = spriteRend.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (isPlaying)
        {
            myRectTransform.localPosition += Vector3.up * directionSpeed * Time.deltaTime;
            curDuration += Time.deltaTime;

            float alpha = 0;
            alpha = maxDuration - curDuration;

            spriteRend.color = new Color(1, 1, 1, alpha);

            if(curDuration > maxDuration)
            {
                Reset();
            }
        } 
    }

    public void Play()
    {
        Reset();
        isPlaying = true;
        spriteRend.color = new Color(1, 1, 1, 1);
    }

    public void Reset()
    {
        spriteRend.transform.localPosition = Vector3.zero;
        spriteRend.color = new Color(1, 1, 1, 0);       
        isPlaying = false;
        curDuration = 0;
    }
}
