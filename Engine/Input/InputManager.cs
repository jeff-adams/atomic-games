using System;
using AtomicGames.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine;

public class InputManager : IDisposable
{
    private readonly IKeyboardBroadcaster keyboardBroadcaster;
    private readonly IMouseBroadcaster mouseBroadcaster;
    private readonly IGamePadBroadcaster gamePadBroadcaster;

    private IActionMap actionMap;

    public InputManager(IBroadcaster[] broadcasters)
    {
        foreach (var broadcaster in broadcasters)
        {
            if(broadcaster is IKeyboardBroadcaster kb) 
            {
                this.keyboardBroadcaster = kb;
                keyboardBroadcaster.OnKeyPressed += KeyboardPressedReciever;
            }
            if(broadcaster is IMouseBroadcaster mb) 
            {
                this.mouseBroadcaster = mb;
                mouseBroadcaster.MousePosition += MousePositionReciever;
                mouseBroadcaster.OnMouseButtonPressed += MouseButtonReceiver;
            }
            if(broadcaster is IGamePadBroadcaster gpb) 
            {
                this.gamePadBroadcaster = gpb;
                gamePadBroadcaster.OnButtonPressed += GamePadButtonReceiver;
                gamePadBroadcaster.LeftAnalogStickMovement += GamePadLeftStickReceiver;
                gamePadBroadcaster.RightAnalogStickMovement += GamePadRightStickReceiver;
            }
        }

    }

    public void SetActionMap(IActionMap actionMap) =>
        this.actionMap = actionMap;

    private void KeyboardPressedReciever(Keys key, InputState state)
    {
        if (actionMap.KeyboardInputs.ContainsKey(key))
        {
            actionMap.KeyboardInputs[key](state);
        }
    }

    private void MousePositionReciever(Vector2 mousePosition) =>
        actionMap.MousePosition(mousePosition);

    private void MouseButtonReceiver(MouseButtons mouseButton)
    {
        if (actionMap.MouseButtonInputs.ContainsKey(mouseButton))
        {
            actionMap.MouseButtonInputs[mouseButton]();
        }
    }

    private void GamePadButtonReceiver(Buttons button, InputState state)
    {
        if (actionMap.GamepadButtonInputs.ContainsKey(button))
        {
            actionMap.GamepadButtonInputs[button](state);
        }
    }

    private void GamePadLeftStickReceiver(Vector2 leftMovement) =>
        actionMap.GamepadThumbstickLeftInput(leftMovement);

    private void GamePadRightStickReceiver(Vector2 rightMovement) =>
        actionMap.GamepadThumbstickRightInput(rightMovement);

    public void Dispose()
    {
        keyboardBroadcaster.OnKeyPressed -= KeyboardPressedReciever;
        mouseBroadcaster.MousePosition -= MousePositionReciever;
        mouseBroadcaster.OnMouseButtonPressed -= MouseButtonReceiver;
        gamePadBroadcaster.OnButtonPressed -= GamePadButtonReceiver;
        gamePadBroadcaster.LeftAnalogStickMovement -= GamePadLeftStickReceiver;
        gamePadBroadcaster.RightAnalogStickMovement -= GamePadRightStickReceiver;
    }
}