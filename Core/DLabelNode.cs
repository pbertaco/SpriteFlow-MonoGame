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

        if (!spriteFontDictionary[currentLanguage].Contains(_spriteFont))
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
        string[] words = text.Split(' ');
        string result = "";
        float currentWidth = 0;

        foreach (string word in words)
        {
            float wordWidth = spriteFont.MeasureString(word).X * spriteFontScale;

            if (currentWidth + wordWidth < maxWidth)
            {
                result += word + " ";
                currentWidth += wordWidth;
            }
            else
            {
                result = result.TrimEnd() + "\n" + word + " ";
                currentWidth = wordWidth;
            }
        }

        text = result.TrimEnd();
    }

    public DLabelNode withShadow(Color color, Vector2 position)
    {
        DLabelNode label = new(text, color, fontSize, false);
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

        bool allDigits = key.All(char.IsDigit);
        if (allDigits)
        {
            return key;
        }

        if (currentLanguage == Language.English)
        {
            return key;
        }

        if (!dictionary.ContainsKey(currentLanguage))
        {
            string languageCode = getLanguageFileName(currentLanguage);
            string fileName = $"Content/json/{languageCode}";
            Dictionary<string, string> jsonDict = null;

            try
            {
                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName);
                    if (!string.IsNullOrEmpty(json))
                    {
                        jsonDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    }
                }
            }
            catch (Exception)
            {
            }

            dictionary[currentLanguage] = jsonDict ?? new Dictionary<string, string>();
        }

        if (dictionary.TryGetValue(currentLanguage, out Dictionary<string, string> langDict) &&
            langDict.TryGetValue(key, out string translation))
        {
            return translation;
        }
        else
        {
            bool hasDigit = key.Any(char.IsDigit);
            bool hasCurly = key.Contains('{');

            if (!hasDigit || (hasDigit && hasCurly))
            {
                if (!dictionary.ContainsKey(Language.English))
                {
                    string englishFileName = "Content/json/en.json";
                    Dictionary<string, string> englishDict = null;

                    try
                    {
                        if (File.Exists(englishFileName))
                        {
                            string engJson = File.ReadAllText(englishFileName);

                            if (!string.IsNullOrEmpty(engJson))
                            {
                                englishDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(engJson, new System.Text.Json.JsonSerializerOptions
                                {
                                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    dictionary[Language.English] = englishDict ?? new Dictionary<string, string>();
                }

                Dictionary<string, string> englishLangDict = dictionary[Language.English];

                if (!englishLangDict.ContainsKey(key))
                {
                    englishLangDict[key] = key;

                    try
                    {
                        string engJsonOut = System.Text.Json.JsonSerializer.Serialize(englishLangDict, new System.Text.Json.JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });

                        File.WriteAllText("Content/json/en.json", engJsonOut);
                    }
                    catch (Exception)
                    {
                    }
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

    public static string getLanguageFileName(Language language)
    {
        switch (language)
        {
            case Language.English: return "en.json";
            case Language.Spanish: return "es.json";
            case Language.Portuguese: return "pt.json";
            case Language.French: return "fr.json";
            case Language.German: return "de.json";
            case Language.Italian: return "it.json";
            case Language.Russian: return "ru.json";
            case Language.Japanese: return "ja.json";
            case Language.SimplifiedChinese: return "zh-CN.json";
            case Language.TraditionalChinese: return "zh-TW.json";
            case Language.Korean: return "ko.json";
            case Language.Polish: return "pl.json";
            case Language.Turkish: return "tr.json";
            default: return language.ToString() + ".json";
        }
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