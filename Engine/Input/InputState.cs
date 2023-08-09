namespace AtomicGames.Engine.Input;

public readonly struct InputState
{
    public string Name { get; init; }
    public bool Pressed { get; init; }
    public bool Held { get; init; }
    public bool Released { get; init; }

    public InputState(string inputName, bool pressed, bool held, bool released)
    {
        Name = inputName;
        Pressed = pressed;
        Held = held;
        Released = released;
    }
}