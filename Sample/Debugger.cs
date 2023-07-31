using System;
using System.Collections.Generic;
using System.Diagnostics;
using AtomicGames.Engine;
using AtomicGames.Engine.Graphics;
using AtomicGames.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Sample;

public class Debugger : GameObject
{
    private readonly SpriteFont font;
    private readonly int verticalSpacing;
    private Dictionary<string, string> messages;
    private Dictionary<string, string> consoleMessages;

    public Debugger(SpriteFont font) : base()
    {
        this.font = font;
        verticalSpacing = (int)System.Math.Ceiling((font.MeasureString("A")).Y * 1.2);
        IsActive = false;
        IsVisible = false;
        MoveTo(new Vector2(1f, 1f));
        messages = new Dictionary<string, string>();
        consoleMessages = new Dictionary<string, string>();
    }

    public void AddDebugMessage(string title, string message)
    {
        message = message.ToLower();
        if (!messages.TryAdd(title, message))
            messages[title] = message;
    }

    public void AddDebugConsoleMessage(string title, string message)
    {
        if (!consoleMessages.TryAdd(title, message))
            consoleMessages[title] = message;
    }

    public void ClearDebugMessages()
    {
        messages?.Clear();
        consoleMessages?.Clear();
    }

    public string ConvertPositionToDebugMessage(Vector2 pos) =>
        $"x: {pos.X}, y: {pos.Y}";

    public void PrintDebugMessagesToConsole()
    {
        Debug.WriteLine("=========================================================");
        Debug.WriteLine(DateTime.Now);
        Debug.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - -");

        foreach (var message in consoleMessages)
        {
            string textString = $"{message.Key}: {message.Value}";
            Debug.WriteLine(textString);
        }
    }

    public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        int i = 0;
        foreach (var message in messages)
        {
            string textString = $"{message.Key}: {message.Value}";
            spriteBatch.DrawString(font, textString, Position + new Vector2(0, i * verticalSpacing), Color.LimeGreen);
            i++;
        }
    }
}