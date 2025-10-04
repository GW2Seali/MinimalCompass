using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MinimalCompass.Extensions;
using MinimalCompass.Models;
using MinimalCompass.Services;

namespace MinimalCompass.Views;

public sealed class CompassSettingsView(SettingsDataModel settingsDataModel, 
	DialogService dialogService)
	: View
{
	/// <inheritdoc />
	protected override void Build(Container buildPanel)
	{
		buildPanel.AddControl(new StandardButton
		{
			Text = "Show settings",
			Width = 200,
			Height = 40,
			Location = Point.Zero
		}).Click += OnClick;
	}

	private void OnClick(object sender, MouseEventArgs e)
	{
		dialogService.SettingsWindow.Show();
	}
}