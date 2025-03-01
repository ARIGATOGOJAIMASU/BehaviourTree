
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] Vector3 wantPosition;
    [SerializeField] bool changeRotation = true;

    public virtual void StartEffect(Vector3 start_Point, Vector3 forward)
    {
        gameObject.SetActive(true);
        transform.position = start_Point + forward * wantPosition.z + new Vector3(0, wantPosition.y, 0);        
        if(changeRotation) transform.LookAt(transform.position + forward);
        StartCoroutine(LifeTimer());
    }

    public void ReStart()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        StopCoroutine("LifeTimer");
        StartCoroutine(LifeTimer());
    }

    protected IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        gameObject.SetActive(false);
    }
}
