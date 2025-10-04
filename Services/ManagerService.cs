using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;

namespace MinimalCompass.Services;

public sealed class ManagerService(ModuleParameters moduleParameters)
{
	public ManagerService(ModuleParameters moduleParameters,
		SettingsManagerService settingsManagerService)
		: this(moduleParameters)
	{
		SettingsManagerService = settingsManagerService;
	}
	
	internal SettingsManager SettingsManager = moduleParameters.SettingsManager;
	internal ContentsManager ContentsManager = moduleParameters.ContentsManager;
	internal DirectoriesManager DirectoriesManager = moduleParameters.DirectoriesManager;
	internal Gw2ApiManager Gw2ApiManager = moduleParameters.Gw2ApiManager;
	internal SettingsManagerService SettingsManagerService { get; } = null!;
	
	internal readonly TextureService TextureService = new(moduleParameters.ContentsManager);
}