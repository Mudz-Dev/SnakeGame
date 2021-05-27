using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum FoodTypes {
        Normal = 0,
        SuperFood = 1
    }

    public FoodTypes foodType;
    public int points {
        get {
            switch(foodType) {
                case FoodTypes.Normal:
                    return 1;
                case FoodTypes.SuperFood:
                    return 5;
                default:
                    return 0;
            }
        }
    }
    
    public float targetScale;
    public float timeToLerp = 0.25f;
    public float rotateSpeed = 50f;
    float scaleModifier = 1;
    SphereCollider sc;

    public System.Action OnEat;
    public System.Action OnSuperFoodEat;

    void Start() {
       sc = GetComponent<SphereCollider>(); 
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Player player = other.gameObject.GetComponent<Player>();
                player.PlayEatAnimation();
                break;
        }
        }

    public void Eat(Vector3 moveLocation)
    {
        if(foodType == FoodTypes.Normal) {
            OnEat();
        }
        else {
            OnSuperFoodEat();
        }
        
        StartCoroutine(EatPositionLerp(moveLocation, timeToLerp));
        StartCoroutine(EatScaleLerp(targetScale, timeToLerp));
    }

    IEnumerator EatScaleLerp(float endValue, float duration)
    {
        float time = 0;
        float startValue = scaleModifier;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            transform.localScale = startScale * scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = startScale * targetScale;
        scaleModifier = targetScale;
        GameObject.Destroy(gameObject);
    }

    IEnumerator EatPositionLerp(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
