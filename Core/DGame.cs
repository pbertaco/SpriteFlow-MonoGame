global using Dragon;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Audio;
global using Microsoft.Xna.Framework.Content;
global using Microsoft.Xna.Framework.Design;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Graphics.PackedVector;
global using Microsoft.Xna.Framework.Input;
global using Microsoft.Xna.Framework.Input.Touch;
global using Microsoft.Xna.Framework.Media;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;

namespace Dragon;

public class DGame : Game
{
    public static DGame current;
    public static string version = "1.0.0";

    public GraphicsDeviceManager graphicsDeviceManager;
    public SpriteBatch spriteBatch;
    public DScene scene;
    public SpriteSortMode sortMode = SpriteSortMode.Deferred;
    public BlendState blendState = null;
    public SamplerState samplerState = null;
    public DepthStencilState depthStencilState = null;
    public RasterizerState rasterizerState = new();
    public Effect effect = null;
    public Matrix? transformMatrix = null;
    readonly bool _startFullScreen;
    bool _pendingFullScreenToggle;
    bool _applyingBackBuffer;

    public static float currentTime;
    public static float elapsedTime;

    DInputManager inputManager;
    Texture2D texture;
    Vector2 sizeLastClientBounds;

    Vector2 sizeClientBounds => new(Window.ClientBounds.Width, Window.ClientBounds.Height);
    Vector2 sizeCurrentDisplayMode => new(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

    public DGame(DScene scene, string title, bool startFullScreen = false)
    {
        this.scene = scene;
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        inputManager = new DInputManager();
        Window.Title = title;
        current = this;
        _startFullScreen = startFullScreen;
        _pendingFullScreenToggle = _startFullScreen;
    }

    protected override void Initialize()
    {
        initializeWindowEventHandlers();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        base.LoadContent();

#if iOS || Android
        graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#else
        float scaleX = sizeCurrentDisplayMode.X / scene.size.X;
        float scaleY = sizeCurrentDisplayMode.Y / scene.size.Y;
        float targetScale = Math.Min(scaleX, scaleY);

        targetScale = Math.Min(targetScale, 3.0f);
        targetScale = Math.Max(targetScale, 1.0f);
        
        Vector2 initialSize = scene.size * targetScale;
        updatePreferredBackBuffer(initialSize);
#endif

        presentScene(scene);
        texture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
        texture.SetData(new Color[] { Color.White });
        Window.AllowUserResizing = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        texture.Dispose();
    }

    public void presentScene(DScene scene)
    {
        DConsole.WriteLine(this, $"presentScene: {scene}");

        scene.load();

        float duration = 0.25f;

        if (this.scene != scene)
        {
            scene.alpha = 0;
            scene.run(DAction.fadeAlphaTo(1, duration));
            this.scene.run(DAction.afterDelay(duration, () => this.scene = scene));
            this.scene.run(DAction.fadeAlphaTo(0, duration));
        }

        if (rasterizerState.ScissorTestEnable != scene.scissorTestEnable)
        {
            RasterizerState rasterizerState = new();
            rasterizerState.ScissorTestEnable = scene.scissorTestEnable;
            this.rasterizerState = rasterizerState;
        }

        scene.updateSize(this, sizeClientBounds);
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        if (_pendingFullScreenToggle && !graphicsDeviceManager.IsFullScreen)
        {
            try
            {
                toggleFullScreen();
            }
            catch { }
            finally { _pendingFullScreenToggle = false; }
        }

#if Windows || macOS || Linux
        // Check for special keys BEFORE inputManager.update() so they're not consumed by scene
        KeyboardState currentKeyboard = Keyboard.GetState();
        KeyboardState previousKeyboard = inputManager.getPreviousKeyboardState();
        
        if (currentKeyboard.IsKeyDown(Keys.F12) && previousKeyboard.IsKeyUp(Keys.F12))
        {
            printScreen();
        }

        if (currentKeyboard.IsKeyDown(Keys.F11) && previousKeyboard.IsKeyUp(Keys.F11))
        {
            toggleFullScreen();
        }

        // Exit fullscreen on Escape (only when currently fullscreen)
        if (currentKeyboard.IsKeyDown(Keys.Escape) && previousKeyboard.IsKeyUp(Keys.Escape) && graphicsDeviceManager.IsFullScreen)
        {
            toggleFullScreen();
        }
#endif

        inputManager.update();

        scene.update();
        scene.evaluateActions(elapsedTime);
        base.Update(gameTime);

        float totalSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
        elapsedTime = totalSeconds - currentTime;
        currentTime = totalSeconds;
        int fps = (int)Math.Round(1 / elapsedTime);

        if (fps < 60 && fps > 0)
        {
            DConsole.WriteLine(this, $"{totalSeconds}\n{fps}\n");
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        if (rasterizerState.ScissorTestEnable)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            spriteBatch.Draw(texture, graphicsDeviceManager.GraphicsDevice.ScissorRectangle, scene.backgroundColor);
        }
        else
        {
            GraphicsDevice.Clear(scene.backgroundColor);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        scene.draw();
        spriteBatch.End();
        base.Draw(gameTime);
    }

    void initializeWindowEventHandlers()
    {
        Window.ClientSizeChanged += (sender, e) =>
        {
            scene.updateSize(this, sizeClientBounds);
        };
    }

    void updatePreferredBackBuffer(Vector2 size)
    {
        int targetW = (int)size.X;
        int targetH = (int)size.Y;
        if (graphicsDeviceManager.PreferredBackBufferWidth == targetW && graphicsDeviceManager.PreferredBackBufferHeight == targetH)
        {
            return;
        }

        graphicsDeviceManager.PreferredBackBufferWidth = targetW;
        graphicsDeviceManager.PreferredBackBufferHeight = targetH;
        graphicsDeviceManager.ApplyChanges();
    }

    void toggleFullScreen()
    {
        bool enable = !graphicsDeviceManager.IsFullScreen;
        if (enable)
        {
            sizeLastClientBounds = sizeClientBounds;
            graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.ApplyChanges();
        }
        else
        {
            graphicsDeviceManager.IsFullScreen = false;
            updatePreferredBackBuffer(sizeLastClientBounds);
        }
    }

    void printScreen()
    {
        int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int height = GraphicsDevice.PresentationParameters.BackBufferHeight;
        int[] backBuffer = new int[width * height];
        GraphicsDevice.GetBackBufferData(backBuffer);
        Texture2D texture = new(GraphicsDevice, width, height);
        texture.SetData(backBuffer);
        string dateString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        using FileStream stream = File.OpenWrite($"screenshot-{dateString}.png");
        texture.SaveAsPng(stream, width, height);
    }
}
