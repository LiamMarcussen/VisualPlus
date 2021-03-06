﻿#region Namespace

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VisualPlus.Toolkit.VisualBase;

#endregion

namespace VisualPlus.Toolkit.Controls.DataVisualization
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ProgressBar))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual Progress Stepper")]
    public abstract class VisualProgressStepper : ProgressBase
    {
    }
}