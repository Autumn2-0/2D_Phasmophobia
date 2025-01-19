using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /*  // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }*/

    private Rigidbody2D rigidbody_;
    private Vector2 movementInput;

    private void Start()
    {
        rigidbody_ = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody_.velocity = movementInput;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

}
