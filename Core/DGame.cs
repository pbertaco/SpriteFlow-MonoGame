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
global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Runtime.InteropServices;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;

#if Steam
global using Steamworks;
#endif

#if Windows
global using GameAnalyticsSDK.Net;
#endif

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

    public static float currentTime;
    public static float elapsedTime;

    DInputManager inputManager;
    Texture2D texture;
    Vector2 sizeLastClientBounds;

    Vector2 sizeClientBounds => new(Window.ClientBounds.Width, Window.ClientBounds.Height);
    Vector2 sizeCurrentDisplayMode => new(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

    public DGame(DScene scene, string title)
    {
        this.scene = scene;
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        graphicsDeviceManager.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        inputManager = new DInputManager();
        Window.Title = title;
        current = this;
    }

    protected override void Initialize()
    {
        initializeWindowEventHandlers();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

#if iOS || Android
        graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#else
        float maxSize = 1;

        while (scene.size.X < sizeCurrentDisplayMode.X / maxSize && scene.size.Y < sizeCurrentDisplayMode.Y / maxSize)
        {
            updatePreferredBackBuffer(scene.size * maxSize);
            maxSize += 1;
        }
#endif

        presentScene(scene);
        texture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
        texture.SetData([Color.White]);
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

        inputManager.update();

        if (inputManager.keyPress(Keys.F12))
        {
            printScreen();
        }

        if (inputManager.keyPress(Keys.F11))
        {
            toggleFullScreen();
        }

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
            updatePreferredBackBuffer(sizeClientBounds);
            scene.updateSize(this, sizeClientBounds);
        };
    }

    void updatePreferredBackBuffer(Vector2 size)
    {
        graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
        graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;
        graphicsDeviceManager.ApplyChanges();
    }

    void toggleFullScreen()
    {
        graphicsDeviceManager.IsFullScreen = !graphicsDeviceManager.IsFullScreen;

        if (graphicsDeviceManager.IsFullScreen)
        {
            sizeLastClientBounds = sizeClientBounds;
            updatePreferredBackBuffer(sizeCurrentDisplayMode);
        }
        else
        {
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
