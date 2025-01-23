////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Utilities;

////public class NewBehaviourScript : MonoBehaviour
////{
////    [SerializeField] Transform Play;
////    [SerializeField] float MouseSpeed = 5.0f;
////    [SerializeField] float orbitCam = 10;

////    Vector3 localRot;
////    // Start is called before the first frame update
////    void Start()
////    {

////    }

////    // Update is called once per frame
////    void Update()
////    {
////        transform.position = Play.position;

////        localRot.x += Input.GetAxis("Mouse X") * MouseSpeed;
////        localRot.y -= Input.GetAxis("Mouse Y") * MouseSpeed;

////        localRot.y = Mathf.Clamp(localRot.y, 0f, 80f);

////        Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0f);
////        transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitCam);
////    }
////}
