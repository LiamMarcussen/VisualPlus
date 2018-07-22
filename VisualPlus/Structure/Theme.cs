﻿#region Namespace

using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using VisualPlus.Enumerators;
using VisualPlus.Managers;
using VisualPlus.Toolkit.Dialogs;
using VisualPlus.TypeConverters;

#endregion

namespace VisualPlus.Structure
{
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The theme structure.")]
    [DesignerCategory("code")]
    [ToolboxItem(false)]
    [TypeConverter(typeof(ThemeTypeConverter))]
    public class Theme
    {
        #region Variables

        private ColorPalette _colorPalette;
        private ThemeInformation _informationSettings;
        private string _rawTheme;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="themeInformation">The theme information.</param>
        /// <param name="colorPalette">The color Palette.</param>
        public Theme(ThemeInformation themeInformation, ColorPalette colorPalette)
        {
            UpdateTheme(themeInformation, colorPalette);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="theme">The theme.</param>
        public Theme(Theme theme)
        {
            UpdateTheme(theme.InformationSettings, theme.ColorPalette);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="themes">The internal themes.</param>
        public Theme(Themes themes)
        {
            LoadThemeFromResources(themes);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="filePath">The file.</param>
        public Theme(string filePath) : this()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NoNullAllowedException(ExceptionMessenger.IsNullOrEmpty(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(ExceptionMessenger.FileNotFound(filePath));
            }

            Load(filePath);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        public Theme()
        {
            _rawTheme = string.Empty;
            _informationSettings = new ThemeInformation();
            _colorPalette = new ColorPalette
                {
                    HelpButtonBack = new ControlColorState(),
                    HelpButtonFore = new ControlColorState(),
                    MinimizeButtonBack = new ControlColorState(),
                    MinimizeButtonFore = new ControlColorState(),
                    MaximizeButtonBack = new ControlColorState(),
                    MaximizeButtonFore = new ControlColorState(),
                    CloseButtonBack = new ControlColorState(),
                    CloseButtonFore = new ControlColorState(),
                    ScrollBar = new ColorState(),
                    ScrollButton = new ControlColorState(),
                    ScrollThumb = new ControlColorState()
                };
        }

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public ColorPalette ColorPalette
        {
            get
            {
                return _colorPalette;
            }

            set
            {
                if (_colorPalette != value)
                {
                    _colorPalette = value;
                    UpdateTheme(_informationSettings, _colorPalette);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public ThemeInformation InformationSettings
        {
            get
            {
                return _informationSettings;
            }

            set
            {
                if (_informationSettings != value)
                {
                    _informationSettings = value;
                    UpdateTheme(_informationSettings, _colorPalette);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string RawTheme
        {
            get
            {
                return _rawTheme;
            }

            private set
            {
                if (_rawTheme != value)
                {
                    _rawTheme = value;

                    // TODO: Verify raw theme is compatible theme then update contents.
                }
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return _rawTheme;
        }

        #endregion

        #region Methods

        /// <summary>Loads the <see cref="Theme" /> from the file path.</summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NoNullAllowedException(ExceptionMessenger.IsNullOrEmpty(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(ExceptionMessenger.FileNotFound(filePath));
            }

            try
            {
                if (File.Exists(filePath))
                {
                    Theme theme = ThemeSerialization.Deserialize(filePath);
                    UpdateTheme(theme.InformationSettings, theme.ColorPalette);
                }
                else
                {
                    VisualExceptionDialog.Show(new FileNotFoundException(ExceptionMessenger.FileNotFound(filePath)));
                }
            }
            catch (Exception e)
            {
                ConsoleEx.WriteDebug(e);
            }
        }

        /// <summary>Saves the theme to a file.</summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NoNullAllowedException(ExceptionMessenger.IsNullOrEmpty(filePath));
            }

            _rawTheme = ThemeSerialization.Serialize(_informationSettings, _colorPalette);

            if (string.IsNullOrEmpty(_rawTheme))
            {
                throw new ArgumentNullException(nameof(_rawTheme));
            }

            XDocument _theme = XDocument.Parse(_rawTheme);
            _theme.Save(filePath);
        }

        /// <summary>Update the theme contents.</summary>
        /// <param name="themeInformation">The theme Information.</param>
        /// <param name="colorPalette">The color Palette.</param>
        public void UpdateTheme(ThemeInformation themeInformation, ColorPalette colorPalette)
        {
            _informationSettings = themeInformation;
            _colorPalette = colorPalette;

            _rawTheme = ThemeSerialization.Serialize(_informationSettings, _colorPalette);
        }

        /// <summary>Loads a <see cref="Theme" /> from resources.</summary>
        /// <param name="themes">The theme.</param>
        private void LoadThemeFromResources(Themes themes)
        {
            try
            {
                Theme theme = ThemeSerialization.Deserialize(themes);
                UpdateTheme(theme.InformationSettings, theme.ColorPalette);
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }
        }

        #endregion
    }
}