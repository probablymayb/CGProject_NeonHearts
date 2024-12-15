using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChainsawOrbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;
    private Vector3 offset;
    private bool isActive = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        offset = transform.position - target.position;
    }

    public void Activate(Vector3 position)
    {
        transform.position = target.position + position;
        offset = position;
        isActive = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void ResetPosition()
    {
        transform.position = target.position + offset;
    }
}