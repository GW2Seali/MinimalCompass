using MinimalCompass.Models;

namespace MinimalCompass.Services;

public sealed class SettingsManagerService(SettingsDataModel settingsDataModel)
{
	public SettingsDataModel SettingsDataModel { get; } = settingsDataModel;
}