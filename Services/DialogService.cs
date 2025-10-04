using MinimalCompass.Dialogs;

namespace MinimalCompass.Services;

public sealed class DialogService
{
	private readonly ManagerService _managerService;
	
	public DialogService(ManagerService managerService)
	{
		_managerService = managerService;
		SettingsWindow = new SettingsWindow(managerService);
	}
	
	public SettingsWindow SettingsWindow { get; }
}