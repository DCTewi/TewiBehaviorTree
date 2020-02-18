using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float speed;
    [SerializeField] private Vector2 move;

    private PlayerInput input;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        speed = 5f;

        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        move = input.actions["Move"].ReadValue<Vector2>();

        transform.Translate(move * speed * Time.deltaTime);
    }
}
