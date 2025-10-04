using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinimalCompass.Services;
using MinimalCompass.Views.Subviews;

namespace MinimalCompass.Dialogs;

public sealed class SettingsWindow : TabbedWindow2
{
	
// Credit:
// Thanks to @Soeed for these constants
// as a test for a good window size
#region Static Fields
	
	private static Rectangle SettingPanelRegion => new()
	{
		Location = new Point(38, 25),
		Size = new Point(1100, 705),
	};
    
	private static Rectangle SettingPanelContentRegion => new()
	{
		Location = SettingPanelRegion.Location + new Point(52, 0),
		Size = SettingPanelRegion.Size - SettingPanelRegion.Location,
	};
	private static Point SettingPanelWindowSize => new(800, 600);
#endregion
	
	
	/// <param name="managerService"></param>
	/// <inheritdoc />
	public SettingsWindow(ManagerService managerService)
		: base(managerService.TextureService.WindowBackground, 
			SettingPanelRegion, SettingPanelContentRegion, SettingPanelWindowSize)
	{
		Parent = Graphics.SpriteScreen;
		Title = "Minimal Compass";
		Subtitle = "Settings";
		SavesPosition = true;
		Id = $"{nameof(Module)}_3b4ec580-0e49-447d-ba54-399bdf8da44c";
		
		Tabs.Add(new Tab(GameService.Content.DatAssetCache.GetTextureFromAssetId(155052),
			() => new CompassSubSettingsView(managerService), 
			"Compass"
		));
	}
}