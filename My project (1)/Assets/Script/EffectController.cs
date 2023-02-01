using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] public float posFoward;

    public void StartEffect(Vector3 start_Point, Vector3 forward)
    {
        gameObject.SetActive(true);
        transform.position = start_Point + (forward * posFoward) + new Vector3(0, 0, 0);
        StartCoroutine(LifeTimer());
    }

    public void ReStart()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        gameObject.SetActive(false);
    }
}
