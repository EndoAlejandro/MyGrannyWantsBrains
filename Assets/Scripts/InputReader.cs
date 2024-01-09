using UnityEngine;

public class InputReader
{
    private readonly MainInput _input;
    public Vector2 Movement => _input != null ? _input.Main.Move.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 Aim => _input != null ? Aiming() : Vector2.zero;
    public bool Grab => _input != null && _input.Main.Grab.WasPerformedThisFrame();

    public InputReader()
    {
        _input = new MainInput();
        _input.Main.Enable();
    }
    
    private Vector2 Aiming()
    {
        var mouseScreen = _input.Main.Aim.ReadValue<Vector2>();
        return new Vector2(mouseScreen.x / Screen.width, mouseScreen.y / Screen.height); //- Vector2.one / 2;
    }
}