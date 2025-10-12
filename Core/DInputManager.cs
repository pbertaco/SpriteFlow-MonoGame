namespace Dragon;

public class DInputManager
{
    Dictionary<int, DTouch> touches;

    MouseState lastMouseState;

    public DInputManager()
    {
        touches = new Dictionary<int, DTouch>();
    }

#if Windows || macOS
    KeyboardState keyboardState;
    KeyboardState lastKeyboardState;
    public void update()
    {
        updateKeyboard();
        updateMouse();
    }

    public bool keyPress(Keys key)
    {
        return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
    }

    public bool keyRelease(Keys key)
    {
        return lastKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }

    void updateKeyboard()
    {
        KeyboardState prev = keyboardState;
        lastKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();

        Keys[] prevKeys = prev.GetPressedKeys();
        Keys[] currKeys = keyboardState.GetPressedKeys();

        HashSet<Keys> prevSet = new(prevKeys);
        HashSet<Keys> currSet = new(currKeys);

        foreach (Keys key in currKeys)
        {
            if (!prevSet.Contains(key))
            {
                DGame.current.scene.keyDown(key);
            }
        }

        foreach (Keys key in prevKeys)
        {
            if (!currSet.Contains(key))
            {
                DGame.current.scene.keyUp(key);
            }
        }
    }

    void updateMouse()
    {
        MouseState mouseState = Mouse.GetState();

        Vector2 lastPosition = new(lastMouseState.X, lastMouseState.Y);
        Vector2 position = new(mouseState.X, mouseState.Y);

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (position != lastPosition)
                {
                    DTouch touch = touches[0];
                    touch.moved(position);
                    DGame.current.scene.touchMoved(touch);
                }
            }
            else
            {
                DTouch touch = new(position);
                DGame.current.scene.touchDown(touch);
                touches.Add(0, touch);
            }
        }
        else
        {
            if (lastMouseState.LeftButton == ButtonState.Pressed)
            {
                DTouch touch = touches[0];
                touch.up(position);
                DGame.current.scene.touchUp(touch);
                DConsole.WriteLine(this, $"\n");
                touches.Remove(0);
            }
        }

        lastMouseState = mouseState;
    }
#endif

#if iOS || Android
    public void update()
    {
        updateTouchPanel();
    }

    void updateTouchPanel()
    {
        TouchCollection touchCollection = TouchPanel.GetState();

        foreach (TouchLocation touchLocation in touchCollection)
        {
            Vector2 position = touchLocation.Position;

            switch (touchLocation.State)
            {
                case TouchLocationState.Pressed:
                    {
                        DTouch touch = new(position);
                        DGame.current.scene.touchDown(touch);
                        touches.Add(touchLocation.Id, touch);
                    }
                    break;
                case TouchLocationState.Moved:
                    {
                        DTouch touch = touches[touchLocation.Id];
                        if (position != touch.lastPosition)
                        {
                            touch.moved(position);
                            DGame.current.scene.touchMoved(touch);
                        }
                    }
                    break;
                case TouchLocationState.Released:
                    {
                        DTouch touch = touches[touchLocation.Id];
                        touch.up(position);
                        DGame.current.scene.touchUp(touch);
                        touches.Remove(touchLocation.Id);
                    }
                    break;
                case TouchLocationState.Invalid:
                    touches = new Dictionary<int, DTouch>();
                    break;
            }
        }
    }

    public bool keyPress(Keys keys)
    {
        return false;
    }
#endif
}
