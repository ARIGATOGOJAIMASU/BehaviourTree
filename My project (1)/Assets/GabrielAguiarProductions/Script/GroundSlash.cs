using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    public float speed = 30;
    public float slowDownRate = 0.01f;
    public float distance;
    Vector3 targetPoint;

    private Rigidbody rb;
    private bool stopped;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        else
            Debug.Log("No Rigidbody");
    }

    private void OnEnable()
    {
        targetPoint = transform.position * distance;
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            transform.position = Vector3.MoveTowards(transform.position ,targetPoint, Time.fixedDeltaTime * speed);
        }
    }

    IEnumerator SlowDown ()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }
}
