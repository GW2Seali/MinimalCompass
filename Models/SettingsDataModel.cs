using Blish_HUD.Settings;

namespace MinimalCompass.Models;

//TODO: Add more settings
// Such as color, position, etc.
public sealed class SettingsDataModel
{
	public SettingsDataModel(SettingCollection settingCollection)
	{
		SettingCollection = settingCollection;
	}
	
	public SettingCollection SettingCollection { get; }
}