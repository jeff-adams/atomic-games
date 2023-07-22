namespace AtomicGames.Engine.Input;

public struct InputState
{
    public string Name { get; }
    public bool Pressed { get; }
    public bool Held { get; }

    public InputState(string inputName, bool pressed, bool held)
    {
        Name = inputName;
        Pressed = pressed;
        Held = held;
    }
}