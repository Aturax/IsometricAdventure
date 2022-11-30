using UnityEngine;

public class InputController
{
    private InputHandler input = null;

    public InputController()
    {
        input = new InputHandler();
        input.Enable();
    }

    public Vector2 Move()
    {
        return input.Gameplay.Move.ReadValue<Vector2>();
    }

    public float Jump()
    {
        return input.Gameplay.Jump.ReadValue<float>();
    }

    public bool IceBall()
    {
        return input.Gameplay.IceBall.IsPressed();
    }

    public bool HasteSpell()
    {
        return input.Gameplay.HasteSpell.IsPressed();        
    }
}
