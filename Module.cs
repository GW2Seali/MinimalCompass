using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using System.ComponentModel.Composition;
using MinimalCompass.Controls;
using MinimalCompass.Models;

namespace MinimalCompass;

[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    private CompassControl _compassControl;
    //TODO: Add settings
    //private static SettingsDataModel _SettingsDataModel;

    private static readonly Logger Logger = Logger.GetLogger<Module>();

#region Service Managers
    internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
    internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
    internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
    internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;
#endregion

    [ImportingConstructor]
    public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { }

    protected override void DefineSettings(SettingCollection settings)
    {
        //TODO: Add settings
        // _SettingsDataModel = new SettingsDataModel(settings);
    }

    protected override void Initialize()
    {
        _compassControl = new CompassControl();
    }

    protected override void Update(GameTime gameTime)
    {
            
        if (!GameService.Gw2Mumble.IsAvailable) return;
        if (!GameService.GameIntegration.Gw2Instance.IsInGame) return;
        _compassControl.Parent = GameService.Graphics.SpriteScreen;
    }

    /// <inheritdoc />
    protected override void Unload()
    {
        // Unload here
        _compassControl.Parent = null;
        _compassControl?.Dispose();
        _compassControl = null;
        // _SettingsDataModel = null;
        // All static members must be manually unset
    }
        
}