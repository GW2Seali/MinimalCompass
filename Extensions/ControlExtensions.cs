using System;
using Blish_HUD.Controls;

namespace MinimalCompass.Extensions;

// Credit:
// Thanks to @Soeed for the idea of this extension method
public static class ControlExtensions
{
	public static T AddControl<T>(this Container parent, T control) where T : Control
	{
		control.Parent = parent;
		parent.AddChild(control);
		
		return control;
	}
}