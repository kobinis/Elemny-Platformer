using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using XnaUtils.XnaUtils.RichText;
using XnaUtils.XnaUtils.RichText.Commands;

namespace XnaUtils {
    /// <remarks>
    /// BUG: parse this test string, note that the middle image isn't displayed:
    /// "#image{FederationLogo} 1\n#image{FederationLogo} 2\n #image{FederationLogo} 3"
    /// </remarks>
    [Serializable]
    public class RichTextParser //TODO: add size
    {
        private const char COMMAND_CHAR = '#';
        /// <summary>Dictionary from command tags to the corresponding classes</summary>        
        private static Dictionary<string, Type> _commandTypes;

        static RichTextParser() {
            _commandTypes = new Dictionary<string, Type>();
            var commandTypeList = ReflectionUtils.GetTypesUnderNamespace(Assembly.GetExecutingAssembly(), "XnaUtils.XnaUtils.RichText.Commands");
            foreach (var type in commandTypeList) {
                FieldInfo field = type.GetField("Command", BindingFlags.Public | BindingFlags.Static);
                string key = (string)field.GetValue(null);
                _commandTypes.Add(key.ToLower(), type);
            }
        }

        public static void AddCommand(Type type) {
            Debug.Assert(type.GetInterfaces().Contains(typeof(ITextElement)), "RichTextParser command doesn't implement ITextElement");
            FieldInfo field = type.GetField("Command", BindingFlags.Public | BindingFlags.Static);
            string key = (string)field.GetValue(null);
            _commandTypes.Add(key, type);
        }
        
        public float Scale { get; set; }
        public int LineSpacesing { get; set; }
        public Color DefaultColor { get; set; }
        public SpriteFont DefaultFont { get { return _defaultFont; } set { _defaultFont = value; } }
        [NonSerialized] // TEMP
        SpriteFont _defaultFont;
        public Color CurrentColor { get; set; }
        public SpriteFont CurrentFont { get { return _currentFont; } set { _currentFont = value; } }
        [NonSerialized] // TEMP
        SpriteFont _currentFont;
        public Vector2 CurrentPosition;
        public float LineHeight;
        public float CurrentHeight;
        public float MaxWidth;
        public bool IsTextBorder;
        //public const int LineHeight = 20;

        private enum ParsingState { Text, Command, Params }
        private ParsingState _parsingState;
        private string _rawText;
        private bool _isNewText;
        private List<ITextElement> _elements;

        public String Text {
            get { return _rawText; }
            set {
                _isNewText = value != _rawText;
                _rawText = value;
            }
        }

        float _maxLineWidth;
        public float MaxLineWidth {
            get {
                return _maxLineWidth;
            }
            set {
                _isNewText = value != _maxLineWidth;
                _maxLineWidth = value;
            }
        }

        public RichTextParser(SpriteFont font) {
            _currentFont = font;
            DefaultFont = font;
            _elements = new List<ITextElement>();
            LineSpacesing = 0;
            DefaultColor = Color.White;
            _maxLineWidth = 1500;
        }

        public void Parse() {
            if (Scale == 0)
                Scale = 1;
            if (_isNewText) {
                int startingIndex = 0;
                string command = null;
                string parameters = null;
                _parsingState = ParsingState.Text;
                _elements.Clear();
                int i;
                for (i = 0; i < _rawText.Length; i++) {
                    switch (_parsingState) {
                        case ParsingState.Text:
                            switch (_rawText[i]) {
                                case '\n':
                                    if (i - startingIndex > 0) {
                                        _elements.Add(new TextCommand(_rawText.Substring(startingIndex, i - startingIndex)));
                                    }
                                    _elements.Add(new NewLineCommand());
                                    startingIndex = i + 1;
                                    break;
                                case COMMAND_CHAR:
                                    if (i - startingIndex > 0) {
                                        _elements.Add(new TextCommand(_rawText.Substring(startingIndex, i - startingIndex)));
                                    }
                                    startingIndex = i + 1;
                                    _parsingState = ParsingState.Command;
                                    break;
                            }
                            break;
                        case ParsingState.Command:
                            if (_rawText[i] == '{') {
                                command = _rawText.Substring(startingIndex, i - startingIndex);
                                startingIndex = i + 1;
                                _parsingState = ParsingState.Params;
                            }
                            break;
                        case ParsingState.Params:
                            if (_rawText[i] == '}') //TODO: maybe add case of multiple ")"
                            {
                                parameters = _rawText.Substring(startingIndex, i - startingIndex);
                                startingIndex = i + 1;
                                ITextElement element = GetElementFromCommand(command);
                                if (element == null)
                                    element = new CTextCommand($"Error: unrecognized command {command}", Color.Red);
                                else
                                    element.ParseParameters(parameters);
                                _elements.Add(element);
                                _parsingState = ParsingState.Text;
                            }
                            break;
                    }
                }
                if (i - startingIndex > 0) {
                    _elements.Add(new TextCommand(_rawText.Substring(startingIndex, i - startingIndex)));
                }

                if (_maxLineWidth > 0)
                    // Wrap lines
                    _elements = WrapLines().ToList();

                _isNewText = false;
            }
        }

        private ITextElement GetElementFromCommand(string command) {
            command = command.ToLower();
            if (_commandTypes.ContainsKey(command))
                return Activator.CreateInstance(_commandTypes[command]) as ITextElement;
            return null;        
        }

        public Vector2 MeasureText() {
            Parse();
            Vector2 size = Vector2.Zero;
            CurrentFont = DefaultFont;
            CurrentPosition = Vector2.Zero;
            foreach (var element in _elements) {
                element.GetSize(this);
                size.X = Math.Max(size.X, CurrentPosition.X);
                size.Y = Math.Max(size.Y, CurrentPosition.Y + CurrentHeight);
                MaxWidth = size.X;
            }
            return size;
        }

        /// <remarks>Note that elements returned by this method aren't guaranteed new; method may return a mix of new ITextElements and existing members of _elements</remarks>
        IEnumerable<ITextElement> WrapLines() {
            var currentWidth = 0f;

            foreach (var element in _elements) {
                var elementWidth = element.GetSize(this).X;

                if (currentWidth + elementWidth <= _maxLineWidth) {
                    // Element fits on current line                
                    yield return element;
                } else {
                    // Element doesn't fit. Can we split it so that it does?
                    var split = (element as ISplittableTextElement)?.Split(this, _maxLineWidth - currentWidth, _maxLineWidth);

                    if (split == null) {
                        // Nope. Move the whole element to a new line (or leave it where it is, if it's already at the start of one)
                        if (currentWidth == 0) {
                            yield return element;
                        } else {
                            yield return new NewLineCommand();

                            // Try splitting it on this new line, if necessary
                            currentWidth = elementWidth;
                            if (elementWidth > _maxLineWidth) {
                                split = (element as ISplittableTextElement)?.Split(this, _maxLineWidth, _maxLineWidth);
                                if (split != null) {
                                    // Successfully split on new line
                                    foreach (var e in split.Take(split.Length - 1)) {
                                        yield return e;
                                        yield return new NewLineCommand();
                                    }
                                    yield return split.Last();
                                    currentWidth = split.Last().GetSize(this).X;
                                } else
                                    // Can't split
                                    yield return element;
                            } else {
                                // No need to split on new line
                                yield return element;
                            }
                        }
                    } else {
                        // Successfully split
                        foreach (var e in split.Take(split.Length - 1)) {
                            yield return e;
                            yield return new NewLineCommand();
                        }
                        yield return split.Last();
                        currentWidth = split.Last().GetSize(this).X;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color? color = null) {
            Parse();
            CurrentFont = DefaultFont;
            CurrentColor = DefaultColor;
            CurrentPosition = Vector2.Zero;


            foreach (var element in _elements)
                element.Draw(spriteBatch, this, position, color);
        }

        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context) {
            _defaultFont = FontBank.DefaultFont;
            _currentFont = FontBank.DefaultFont;
        }
    }
}
