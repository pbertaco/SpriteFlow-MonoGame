namespace Dragon;

public class DGame : Game
{
    internal static DGame current;

    internal GraphicsDeviceManager graphicsDeviceManager;
    internal SpriteBatch spriteBatch;
    internal DScene scene;
    internal SpriteSortMode sortMode = SpriteSortMode.Deferred;
    internal BlendState blendState = null;
    internal SamplerState samplerState = null;
    internal DepthStencilState depthStencilState = null;
    internal RasterizerState rasterizerState = new RasterizerState();
    internal Effect effect = null;
    internal Matrix? transformMatrix = null;

    internal static float currentTime;
    internal static float elapsedTime;

    DInputManager inputManager;
    Texture2D texture;
    Vector2 sizeLastClientBounds;

    Vector2 sizeClientBounds => new(Window.ClientBounds.Width, Window.ClientBounds.Height);
    Vector2 sizeCurrentDisplayMode => new(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

    internal DGame(DScene scene)
    {
        this.scene = scene;
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        inputManager = new DInputManager();
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
        base.LoadContent();

#if iOS || Android || __IOS__
        graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        toggleFullScreen();
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
        texture.SetData(new[] { Color.White });
        Window.Title = "Window.Title";
        Window.AllowUserResizing = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        texture.Dispose();
    }

    internal void presentScene(DScene scene)
    {
        DConsole.WriteLine(this, $"presentScene: {scene}");
        this.scene = scene;
        scene.load();

        if (rasterizerState.ScissorTestEnable != scene.scissorTestEnable)
        {
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.ScissorTestEnable = scene.scissorTestEnable;
            this.rasterizerState = rasterizerState;
        }

        updateSceneSize();
    }

    internal void updateSceneSize()
    {
        scene.updateSize(this, sizeClientBounds);
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        inputManager.update();

        if (inputManager.keyPress(Keys.PrintScreen))
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
            updateSceneSize();
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
