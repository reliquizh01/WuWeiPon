using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FloatingSpiritBehavior : MonoBehaviour
{
    public SpriteRenderer blinkingSprite;
    public Vector2 targetPosition, direction;
    public float speed = 1.5f;

    private Action<WeaponStatEnum, float,FloatingSpiritBehavior> callBackOnClick;

    internal bool isMoving = false;

    private WeaponStatEnum statTarget;
    private float statAmount;

    private float maxlifeSpan = 3.0f;
    private float curlifeSpan = 0.0f;
    internal int uniqId;

    public void Update()
    {
        if (isMoving)
        {
            curlifeSpan += Time.deltaTime;
            if(curlifeSpan >= maxlifeSpan)
            {
                SpiritCondensationContainer.Instance.RemoveFloatingSpiritNoIncreaseStats(this);
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
    public void InitializeSprit(WeaponStatEnum spiritStats,float amount, Action<WeaponStatEnum,float, FloatingSpiritBehavior> callBackOnClick = null)
    {
        this.callBackOnClick = callBackOnClick;

        statTarget = spiritStats;
        statAmount = amount;

        speed = UnityEngine.Random.Range(3.0f, 8.0f);

        var radius = 10f;
        var angle = UnityEngine.Random.value * (3f * Mathf.PI);
        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        targetPosition = direction * radius;

        // When raycasting
        direction = (targetPosition - (Vector2)gameObject.transform.position).normalized;

        isMoving = true;
    }

    public void OnMouseDown()
    {
        if(callBackOnClick != null)
        {
            callBackOnClick.Invoke(statTarget, statAmount, this);
            blinkingSprite.gameObject.SetActive(true);
            isMoving = false;
        }
    }

    public void OnDestroy()
    {
        
    }
}
