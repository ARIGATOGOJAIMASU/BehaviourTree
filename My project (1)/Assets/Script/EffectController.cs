
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] public float posFoward;
    [SerializeField] public Vector3 WantPosition;

    public virtual void StartEffect(Vector3 start_Point, Vector3 forward)
    {
        gameObject.SetActive(true);
        transform.position = start_Point + (forward * posFoward) + forward * WantPosition.z + new Vector3(0, WantPosition.y, 0);
        transform.LookAt(transform.position + forward);
        StartCoroutine(LifeTimer());
    }

    public void ReStart()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    protected IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        gameObject.SetActive(false);
    }
}
