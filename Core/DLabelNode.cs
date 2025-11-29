namespace Dragon;

public class DLabelNode : DNode
{
    public static Dictionary<Language, SpriteFont> defaultSpriteFontDictionary = new();
    public static Dictionary<Language, List<SpriteFont>> spriteFontDictionary = new();
    public static Dictionary<SpriteFont, SamplerState> samplerStateDictionary = new();



    public BlendState blendState;
    public SamplerState samplerState;
    public Color color;
    SpriteEffects spriteEffects;
    float layerDepth;

    Vector2 _origin;
    Vector2 origin => _origin;

    float _spriteFontScale;
    float spriteFontScale => _spriteFontScale;

    public Vector2 textureSize => spriteFont.MeasureString(text);

    string _text;
    public string text
    {
        get => _text;
        set
        {
            _text = value;
            updateOrigin();
        }
    }

    SpriteFont _spriteFont;
    public SpriteFont spriteFont
    {
        get => _spriteFont;
        set
        {
            _spriteFont = value;
            updateSpriteFontScale();
            updateOrigin();
        }
    }

    float _fontSize;
    public float fontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            updateSpriteFontScale();
        }
    }

    public Vector2 size
    {
        get => textureSize * spriteFontScale;
    }

    Vector2 _anchorPoint;
    public Vector2 anchorPoint
    {
        get => _anchorPoint;
        set
        {
            _anchorPoint = value;
            updateOrigin();
        }
    }

    public bool translateText;

    public DLabelNode(string text, SpriteFont font, Color? color = null, float? fontSize = null, bool translateText = true)
    {
        this.translateText = translateText;
        load(font, fontSize, color, text, translateText);
    }

    public DLabelNode(string text, string assetName, Color? color = null, float? fontSize = null, bool translateText = true)
    {
        this.translateText = translateText;
        SpriteFont font = loadSpriteFont(assetName);
        load(font, fontSize, color, text, translateText);
    }

    public DLabelNode(string text, SpriteFont font, float fontSize, bool translateText = true)
    {
        this.translateText = translateText;
        load(font, fontSize, null, text, translateText);
    }

    public DLabelNode(string text, string assetName, float fontSize, bool translateText = true)
    {
        this.translateText = translateText;
        SpriteFont font = loadSpriteFont(assetName);
        load(font, fontSize, null, text, translateText);
    }

    public DLabelNode(string text, Color? color = null, float? fontSize = null, bool translateText = true)
    {
        this.translateText = translateText;
        SpriteFont font = defaultSpriteFontDictionary[currentLanguage] ?? loadSpriteFont();
        load(font, fontSize, color, text, translateText);
    }

    public DLabelNode(string text, float fontSize, bool translateText = true)
    {
        this.translateText = translateText;
        SpriteFont font = defaultSpriteFontDictionary[currentLanguage] ?? loadSpriteFont();
        load(font, fontSize, null, text, translateText);
    }

    void load(SpriteFont font, float? fontSize, Color? color, string text, bool translateText)
    {

        _spriteFont = font;

        if (translateText && !spriteFontDictionary[currentLanguage].Contains(_spriteFont))
        {
            _spriteFont = defaultSpriteFontDictionary[currentLanguage];
        }

        samplerState = samplerStateDictionary[_spriteFont];

        _text = translateText ? text.translate() : text;
        position = Vector2.Zero;
        this.color = color ?? Color.White;
        rotation = 0;
        _anchorPoint = new Vector2(0.5f, 0.5f);
        scale = Vector2.One;
        spriteEffects = SpriteEffects.None;
        layerDepth = 0;
        _fontSize = fontSize ?? spriteFont.LineSpacing;
        updateSpriteFontScale();
        updateOrigin();
    }

    public override void beforeDraw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        base.beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);

        DGame game = DGame.current;

        if (blendState != game.blendState || samplerState != game.samplerState)
        {
            game.blendState = blendState;
            game.samplerState = samplerState;
            game.spriteBatch.End();
            game.spriteBatch.Begin(game.sortMode, game.blendState, game.samplerState, game.depthStencilState, game.rasterizerState, game.effect, game.transformMatrix);
        }
    }

    public override void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);
        DGame.current.spriteBatch.DrawString(spriteFont, text, drawPosition, color * drawAlpha, drawRotation, origin, drawScale * spriteFontScale, spriteEffects, layerDepth);
        drawChildren();
    }

    void updateSpriteFontScale()
    {
        _spriteFontScale = _fontSize / (_spriteFont != null ? _spriteFont.LineSpacing : 1);
    }

    void updateOrigin()
    {
        _origin = textureSize * _anchorPoint;
    }

    public Vector2 calculateScaleToFit(Vector2 size)
    {
        float scale = Math.Min(size.X / this.size.X, size.Y / this.size.Y);
        return new Vector2(scale);
    }

    public Vector2 calculateScaleToFill(Vector2 size)
    {
        float scale = Math.Max(size.X / this.size.X, size.Y / this.size.Y);
        return new Vector2(scale);
    }

    public void setScaleToFit(Vector2 size)
    {
        scale = calculateScaleToFit(size);
    }

    public void setScaleToFill(Vector2 size)
    {
        scale = calculateScaleToFill(size);
    }

    public static SpriteFont loadSpriteFont(string assetName = "missingSpriteFont", bool handleException = true)
    {
        SpriteFont spriteFont = null;

        try
        {
            spriteFont = DGame.current.Content.Load<SpriteFont>($"SpriteFont/{assetName}");
        }
        catch (Exception e)
        {
            if (handleException)
            {
                spriteFont = loadSpriteFont(null, false);
            }
        }

        return spriteFont;
    }

    public void lineBreak(int maxWidth)
    {
        bool isAsianLanguage = currentLanguage == Language.Japanese || currentLanguage == Language.SimplifiedChinese || currentLanguage == Language.TraditionalChinese || currentLanguage == Language.Korean;

        if (isAsianLanguage)
        {
            string result = "";
            float currentWidth = 0;

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    result += c;
                    currentWidth = 0;
                    continue;
                }

                float charWidth = spriteFont.MeasureString(c.ToString()).X * spriteFontScale;

                if (currentWidth + charWidth < maxWidth)
                {
                    result += c;
                    currentWidth += charWidth;
                }
                else
                {
                    result += "\n" + c;
                    currentWidth = charWidth;
                }
            }

            text = result;
        }
        else
        {
            string[] words = text.Split(' ');
            string result = "";
            float currentWidth = 0;
            float spaceWidth = spriteFont.MeasureString(" ").X * spriteFontScale;

            foreach (string word in words)
            {
                float wordWidth = spriteFont.MeasureString(word).X * spriteFontScale;

                if (currentWidth + wordWidth < maxWidth)
                {
                    result += word + " ";
                    currentWidth += wordWidth + spaceWidth;
                }
                else
                {
                    result = result.TrimEnd() + "\n" + word + " ";
                    currentWidth = wordWidth + spaceWidth;
                }
            }

            text = result.TrimEnd();
        }
    }

    public DLabelNode withShadow(Color color, Vector2 position)
    {
        DLabelNode label = new(text, spriteFont, color, fontSize, false);
        label.addChild(this, -position);
        return label;
    }

    public static Language currentLanguage { get; set; } = Language.English;
    public static Dictionary<Language, Dictionary<string, string>> dictionary = new();

    public static void load(Dictionary<string, string> texts, Language language)
    {
        if (dictionary.ContainsKey(language))
        {
            foreach (KeyValuePair<string, string> text in texts)
            {
                dictionary[language][text.Key] = text.Value;
            }
        }
        else
        {
            dictionary[language] = new Dictionary<string, string>(texts);
        }
    }

    public static string translate(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        if (key.All(char.IsDigit))
        {
            return key;
        }

        if (currentLanguage == Language.English)
        {
            return key;
        }

        if (!dictionary.ContainsKey(currentLanguage))
        {
            string languageCode = getLanguageCode(currentLanguage);
            string languageFolder = $"Content/json/{languageCode}";
            dictionary[currentLanguage] = loadLanguageFiles(languageFolder);
        }

        if (dictionary.TryGetValue(currentLanguage, out Dictionary<string, string> langDict) && langDict.TryGetValue(key, out string translation))
        {
            return translation;
        }
        else
        {
            bool hasDigit = key.Any(char.IsDigit);
            bool hasCurly = key.Contains('{');

            if (!hasDigit || (hasDigit && hasCurly))
            {
                Dictionary<string, string> nullLangDict = dictionary[currentLanguage];

                try
                {
                    string languageCode = getLanguageCode(currentLanguage);
                    string jsonPath = $"Content/json/{languageCode}.json";

                    Dictionary<string, string> existingDict = new();

                    if (File.Exists(jsonPath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(jsonPath);

                            if (!string.IsNullOrEmpty(existingJson))
                            {
                                existingDict = JsonSerializer.Deserialize<Dictionary<string, string>>(existingJson) ?? new Dictionary<string, string>();
                            }
                        }
                        catch
                        {
                        }
                    }

                    if (!existingDict.ContainsKey(key))
                    {
                        existingDict[key] = "";

                        string engJsonOut = JsonSerializer.Serialize(existingDict, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });

                        File.WriteAllText(jsonPath, engJsonOut);
                    }
                }
                catch (Exception e)
                {
                    ;
                }
            }
            else
            {
                ;
            }
        }

        return key;
    }

    public static string getDisplayName(Language language)
    {
        switch (language)
        {
            case Language.English: return "English";
            case Language.Spanish: return "Español";
            case Language.Portuguese: return "Português";
            case Language.French: return "Français";
            case Language.German: return "Deutsch";
            case Language.Italian: return "Italiano";
            case Language.Russian: return "Русский";
            case Language.Japanese: return "日本語";
            case Language.SimplifiedChinese: return "简体中文";
            case Language.TraditionalChinese: return "繁體中文";
            case Language.Korean: return "한국어";
            case Language.Polish: return "Polski";
            case Language.Turkish: return "Türkçe";
            default: return language.ToString();
        }
    }

    public static string getLanguageCode(Language language)
    {
        switch (language)
        {
            case Language.English: return "en";
            case Language.Spanish: return "es";
            case Language.Portuguese: return "pt";
            case Language.French: return "fr";
            case Language.German: return "de";
            case Language.Italian: return "it";
            case Language.Russian: return "ru";
            case Language.Japanese: return "ja";
            case Language.SimplifiedChinese: return "zh-CN";
            case Language.TraditionalChinese: return "zh-TW";
            case Language.Korean: return "ko";
            case Language.Polish: return "pl";
            case Language.Turkish: return "tr";
            default: return language.ToString();
        }
    }

    private static Dictionary<string, string> loadLanguageFiles(string languageFolder)
    {
        Dictionary<string, string> result = new();

        if (!Directory.Exists(languageFolder))
        {
            return result;
        }

        try
        {
            string[] jsonFiles = Directory.GetFiles(languageFolder, "*.json");

            foreach (string jsonFile in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(jsonFile);

                    if (!string.IsNullOrEmpty(json))
                    {
                        Dictionary<string, string> fileDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                        if (fileDict != null)
                        {
                            foreach (KeyValuePair<string, string> kvp in fileDict)
                            {
                                result[kvp.Key] = kvp.Value;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ;
                }
            }
        }
        catch (Exception e)
        {
            ;
        }

        return result;
    }

    public static void setSampler(string fontName, SamplerState state)
    {
        samplerStateDictionary[loadSpriteFont(fontName)] = state;
    }

    public static void setDefault(Language lang, string fontName)
    {
        defaultSpriteFontDictionary[lang] = loadSpriteFont(fontName);
    }
}