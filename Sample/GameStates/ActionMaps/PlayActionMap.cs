using System;
using System.Collections.Generic;
using AtomicGames.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Sample;

public class ActionMapPlay : IActionMap
{
    public Dictionary<Keys, Action<InputState>> KeyboardInputs { get; private set;}
    public Dictionary<Buttons, Action<InputState>> GamepadButtonInputs { get; private set;}
    public Action<Vector2> GamepadThumbstickLeftInput => GetGamePadLeftThumbstickPosition;
    public Action<Vector2> GamepadThumbstickRightInput => GetGamePadRightThumbstickPosition;
    public Dictionary<MouseButtons, Action> MouseButtonInputs { get; private set; }
    public Action<Vector2> MousePosition => GetMousePosition;

    public ActionMapPlay()
    {
        KeyboardInputs = new Dictionary<Keys, Action<InputState>> {
            {Keys.Escape, QuitInput},
            {Keys.OemTilde, ToggleDebugInput},
            {Keys.Tab, PrintDebugToConsole},
            {Keys.OemPlus, CameraZoomIn},
            {Keys.OemMinus, CameraZoomOut},
            {Keys.Right, CameraPanRight},
            {Keys.Left, CameraPanLeft},
            {Keys.Up, CameraPanUp},
            {Keys.Down, CameraPanDown},
            {Keys.R, CameraReset},
            {Keys.W, DirectionInputUp},
            {Keys.D, DirectionInputRight},
            {Keys.S, DirectionInputDown},
            {Keys.A, DirectionInputLeft},
        };

        GamepadButtonInputs = new Dictionary<Buttons, Action<InputState>> {
            {Buttons.A, DebugInputPress},
            {Buttons.B,  DebugInputPress},
            {Buttons.Back,  DebugInputPress},
            {Buttons.BigButton,  DebugInputPress},
            {Buttons.DPadDown,  DebugInputPress},
            {Buttons.DPadLeft,  DebugInputPress},
            {Buttons.DPadRight,  DebugInputPress},
            {Buttons.DPadUp,  DebugInputPress},
            {Buttons.LeftShoulder,  DebugInputPress},
            {Buttons.LeftStick,  DebugInputPress},
            {Buttons.RightShoulder,  DebugInputPress},
            {Buttons.RightStick,  DebugInputPress},
            {Buttons.Start,  DebugInputPress},
            {Buttons.X,  DebugInputPress},
            {Buttons.Y,  DebugInputPress},
        };

        MouseButtonInputs = new Dictionary<MouseButtons, Action> {
            //{MouseButtons.LeftButton, ThrustInput},
        };
    }

    // Action Events --------------------------------------------------------------------------------
    public event Action OnResetCameraPressed;
    public event Action<Vector2> OnCameraPan;
    public event Action<float> OnCameraZoom;
    public event Action<Vector2> OnDirectionInput;
    public event Action<Vector2> OnMousePositionInput;
    public event Action<Vector2> OnGamePadLeftStickPositionInput;
    public event Action<Vector2> OnGamePadRightStickPositionInput;
    public event Action OnQuitPressed;
        //Debug
        public event Action<InputState> OnDebugInputPressed;
        public event Action OnToggleDebugPressed;
        public event Action OnPrintDebugPressed;

    
    //Input Mappings --------------------------------------------------------------------------------
    private void GetMousePosition(Vector2 mousePosition) =>
        OnMousePositionInput?.Invoke(mousePosition);

    private void GetGamePadLeftThumbstickPosition(Vector2 leftStickPosition) =>
        OnGamePadLeftStickPositionInput?.Invoke(leftStickPosition);

    private void GetGamePadRightThumbstickPosition(Vector2 rightStickPosition) =>
        OnGamePadRightStickPositionInput?.Invoke(rightStickPosition);

    private void DirectionInputRight(InputState state) => OnDirectionInput?.Invoke(Vector2.UnitX);
    private void DirectionInputLeft(InputState state) => OnDirectionInput?.Invoke(-Vector2.UnitX);
    private void DirectionInputUp(InputState state) => OnDirectionInput?.Invoke(-Vector2.UnitY);
    private void DirectionInputDown(InputState state) => OnDirectionInput?.Invoke(Vector2.UnitY);

    private void CameraPanRight(InputState state) => OnCameraPan?.Invoke(Vector2.UnitX);
    private void CameraPanLeft(InputState state) => OnCameraPan?.Invoke(-Vector2.UnitX);
    private void CameraPanUp(InputState state) => OnCameraPan?.Invoke(-Vector2.UnitY);
    private void CameraPanDown(InputState state) => OnCameraPan?.Invoke(Vector2.UnitY);
    
    private void CameraZoomIn(InputState state) => OnCameraZoom?.Invoke(1f);
    private void CameraZoomOut(InputState state) => OnCameraZoom?.Invoke(-1f);

    private void QuitInput(InputState state) => OnQuitPressed?.Invoke();

    private void CameraReset(InputState state) => OnResetCameraPressed?.Invoke();


        //Debug
        private void ToggleDebugInput(InputState state)
        {
            if (state.Pressed && !state.Held) OnToggleDebugPressed?.Invoke();
        }

        private void PrintDebugToConsole(InputState state)
        {
            if (state.Pressed && !state.Held) OnPrintDebugPressed?.Invoke();
        }

        private void DebugInputPress(InputState state) =>
            OnDebugInputPressed?.Invoke(state);

}