using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ShakeDuration;
    public float magnitude;
    public Vector3 StartPos;

    public void CameraShacking()
    {
        StartPos = transform.position;
        StartCoroutine(Shaking());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            CameraShacking();
        }
    }

    IEnumerator Shaking()
    {
        float timer = 0;

        while (timer <= ShakeDuration)
        {
            transform.localPosition = (Vector3)Random.insideUnitSphere * magnitude + StartPos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = StartPos;
        yield break;
    }
}
