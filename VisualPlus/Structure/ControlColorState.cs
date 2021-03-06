#region Namespace

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using VisualPlus.Delegates;
using VisualPlus.Enumerators;
using VisualPlus.Events;
using VisualPlus.Localization;
using VisualPlus.TypeConverters;

#endregion

namespace VisualPlus.Structure
{
    [TypeConverter(typeof(VisualSettingsTypeConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The control color state of a component.")]
    [Category(PropertyCategory.Appearance)]
    public class ControlColorState : HoverColorState
    {
        #region Variables

        private Color _pressed;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ControlColorState" /> class.</summary>
        /// <param name="disabled">The disabled color</param>
        /// <param name="enabled">The enabled color.</param>
        /// <param name="hover">The hover color.</param>
        /// <param name="pressed">The pressed color.</param>
        public ControlColorState(Color disabled, Color enabled, Color hover, Color pressed)
        {
            Disabled = disabled;
            Enabled = enabled;
            Hover = hover;
            _pressed = pressed;
        }

        /// <summary>Initializes a new instance of the <see cref="ControlColorState" /> class.</summary>
        public ControlColorState()
        {
            Disabled = Color.Empty;
            Enabled = Color.Empty;
            Hover = Color.Empty;
            _pressed = Color.Empty;
        }

        #endregion

        #region Events

        [Category(EventCategory.PropertyChanged)]
        [Description(EventDescription.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler PressedColorChanged;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether this <see cref="ControlColorState" /> is empty.</summary>
        [Browsable(false)]
        public new bool IsEmpty
        {
            get
            {
                return Hover.IsEmpty && _pressed.IsEmpty && Disabled.IsEmpty && Enabled.IsEmpty;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color Pressed
        {
            get
            {
                return _pressed;
            }

            set
            {
                _pressed = value;
                OnDisabledColorChanged(new ColorEventArgs(_pressed));
            }
        }

        #endregion

        #region Overrides

        protected virtual void OnPressedColorChanged(ColorEventArgs e)
        {
            PressedColorChanged?.Invoke(e);
        }

        public override string ToString()
        {
            StringBuilder _stringBuilder = new StringBuilder();
            _stringBuilder.Append(GetType().Name);
            _stringBuilder.Append(" [");

            if (IsEmpty)
            {
                _stringBuilder.Append("IsEmpty");
            }
            else
            {
                _stringBuilder.Append("Disabled=");
                _stringBuilder.Append(Disabled);
                _stringBuilder.Append("Hover=");
                _stringBuilder.Append(Hover);
                _stringBuilder.Append("Normal=");
                _stringBuilder.Append(Enabled);
                _stringBuilder.Append("Pressed=");
                _stringBuilder.Append(Pressed);
            }

            _stringBuilder.Append("]");

            return _stringBuilder.ToString();
        }

        #endregion

        #region Methods

        /// <summary>Get the control back color state.</summary>
        /// <param name="controlColorState">The control color state.</param>
        /// <param name="enabled">The enabled toggle.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color BackColorState(ControlColorState controlColorState, bool enabled, MouseStates mouseState)
        {
            Color _color;

            if (enabled)
            {
                switch (mouseState)
                {
                    case MouseStates.Normal:
                        {
                            _color = controlColorState.Enabled;
                            break;
                        }

                    case MouseStates.Hover:
                        {
                            _color = controlColorState.Hover;
                            break;
                        }

                    case MouseStates.Pressed:
                        {
                            _color = controlColorState.Pressed;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(mouseState), mouseState, null);
                        }
                }
            }
            else
            {
                _color = controlColorState.Disabled;
            }

            return _color;
        }

        #endregion
    }
}