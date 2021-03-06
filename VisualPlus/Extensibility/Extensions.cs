﻿#region Namespace

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using VisualPlus.Managers;

#endregion

namespace VisualPlus.Extensibility
{
    public static class Extensions
    {
        #region Methods

        /// <summary>Gets a boolean determining whether the object holds any value or is empty/null.</summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            return (source != null) && source.Any();
        }

        /// <summary>Determines whether the object has the method.</summary>
        /// <param name="source">The object source.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool HasMethod(this object source, string methodName)
        {
            return ObjectManagement.HasMethod(source, methodName);
        }

        /// <summary>Check if the value is in range.</summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool IsInRange(this int value, int minimum, int maximum)
        {
            return (value >= minimum) && (value <= maximum);
        }

        /// <summary>Indicates whether the specified <see cref="Array" /> is null or an empty <see cref="Array" />.</summary>
        /// <param name="array">The array.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool IsNullOrEmpty(this Array array)
        {
            return (array == null) || (array.Length == 0);
        }

        /// <summary>Limits the number exclusively to only what is in range.</summary>
        /// <param name="value">The value.</param>
        /// <param name="inclusiveMinimum">The minimum.</param>
        /// <param name="inclusiveMaximum">The maximum.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int LimitToRange(this int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum)
            {
                return inclusiveMinimum;
            }

            if (value > inclusiveMaximum)
            {
                return inclusiveMaximum;
            }

            return value;
        }

        /// <summary>Scroll down the panel.</summary>
        /// <param name="panel">The panel.</param>
        /// <param name="position">The position.</param>
        public static void ScrollDown(this Panel panel, int position)
        {
            // position passed in should be positive
            using (Control c = new Control { Parent = panel, Height = 1, Top = panel.ClientSize.Height + position })
            {
                panel.ScrollControlIntoView(c);
            }
        }

        /// <summary>Scroll to the bottom of the panel.</summary>
        /// <param name="panel">The panel.</param>
        public static void ScrollToBottom(this Panel panel)
        {
            using (Control c = new Control { Parent = panel, Dock = DockStyle.Bottom })
            {
                panel.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        /// <summary>Scroll up the panel.</summary>
        /// <param name="panel">The panel.</param>
        /// <param name="position">The position.</param>
        public static void ScrollUp(this Panel panel, int position)
        {
            // position passed in should be negative
            using (Control c = new Control { Parent = panel, Height = 1, Top = position })
            {
                panel.ScrollControlIntoView(c);
            }
        }

        /// <summary>Returns the size of the structure.</summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="uint" />.</returns>
        public static uint SizeOf<T>(this T value) where T : struct
        {
            return (uint)Marshal.SizeOf(typeof(T));
        }

        #endregion
    }

    public class CustomNumberTypeConverter : TypeConverter
    {
        #region Overrides

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var s = (string)value;
                return int.Parse(s, NumberStyles.AllowThousands, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((int)value).ToString("N0", culture);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}