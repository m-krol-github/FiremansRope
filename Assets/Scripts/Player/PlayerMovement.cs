using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClass
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Range(2, 50)]
        public float moveSpeed;
        [Range(2, 50)]
        public float rotateSpeed;

        //[SerializeField] private Vector2 dir; 

        [SerializeField, Space] private Rigidbody rb;

        private void Start()
        {

        }

        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            Vector3 dir = new Vector3(y, 0, -x);

            rb.AddForce(dir * moveSpeed, ForceMode.Force);
        }
    }
}