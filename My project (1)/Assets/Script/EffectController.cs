
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] Vector3 WantPosition;
    [SerializeField] bool ChangeRotation = true;
    IEnumerator EffectIenumrator;

    private void Start()
    {
        EffectIenumrator = LifeTimer();
    }

    public virtual void StartEffect(Vector3 start_Point, Vector3 forward)
    {
        gameObject.SetActive(true);
        transform.position = start_Point + forward * WantPosition.z + new Vector3(0, WantPosition.y, 0);        
        if(ChangeRotation) transform.LookAt(transform.position + forward);
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
