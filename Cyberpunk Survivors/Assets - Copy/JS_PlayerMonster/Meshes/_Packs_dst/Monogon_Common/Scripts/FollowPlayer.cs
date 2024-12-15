using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float MouseSpeed_X = 3;
    [SerializeField] float MouseSpeed_Y = 3;
    [SerializeField] float OrbitDamping = 10;
    [SerializeField] float PitchMin = -45;
    [SerializeField] float PitchMax = 89;
    Vector3 localRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position;

        localRot.x += Input.GetAxis("Mouse X") * MouseSpeed_X;
        localRot.y -= Input.GetAxis("Mouse Y") * MouseSpeed_Y;

        localRot.y = Mathf.Clamp(localRot.y, PitchMin, PitchMax);

        Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, QT,Time.deltaTime*OrbitDamping);
    }
}
