using UnityEngine;

public class Exit : MonoBehaviour
{
    private ExitInput input;

    private void Awake()
    {
        input = new ExitInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        input.Exit.Exited.performed += Exited_performed;
    }

    private void Exited_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Application.Quit();
        Debug.Log("exited");
    }
}
