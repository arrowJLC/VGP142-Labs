using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public Player controller;
    ThirdPersonInputs inputs;

    protected override void Awake()
    {
        base.Awake();
        inputs = new ThirdPersonInputs();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Overworld.SetCallbacks(controller);
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Overworld.RemoveCallbacks(controller);
    }
}