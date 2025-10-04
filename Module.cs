using System;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using MinimalCompass.Controls;
using MinimalCompass.Models;
using MinimalCompass.Services;
using MinimalCompass.Views;

namespace MinimalCompass;

[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static IServiceProvider? Services;
    
    private CompassControl? _compassControl;
    //TODO: Add settings
    private static SettingsDataModel _settingsDataModel;
    private IView SettingsView { get; set; }

    private static readonly Logger Logger = Logger.GetLogger<Module>();

// #region Service Managers
//     internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
//     internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
//     internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
//     internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;
// #endregion

    [ImportingConstructor]
    public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { }

    protected override void DefineSettings(SettingCollection settings)
    {
        //TODO: Add settings
        _settingsDataModel = new SettingsDataModel(settings);
    }

    /// <inheritdoc />
    protected override Task LoadAsync()
    {
        _compassControl = new CompassControl
        {
            LocationFactorX = _settingsDataModel.CompassLocationX.Value,
            LocationFactorY = _settingsDataModel.CompassLocationY.Value,
        };
        
        _settingsDataModel.CompassLocationChanged += CompassLocationChanged;
        GameService.Graphics.SpriteScreen.Resized += SpriteScreenOnResized;
        _compassControl.RecalculateLayout();
        
        return Task.CompletedTask;
    }

    private void SpriteScreenOnResized(object sender, ResizedEventArgs e)
    {
        _compassControl?.RecalculateLayout();
        Logger.Info("Screen resized, recalculated compass layout.");
    }

    protected override void Initialize()
    {
        RegisterServices();
        
        SettingsView = new CompassSettingsView(_settingsDataModel, Services!.GetRequiredService<DialogService>());
    }

    private void RegisterServices()
    {
        Services = new ServiceCollection()
            .AddSingleton(new SettingsManagerService(_settingsDataModel))
            .AddSingleton(new ManagerService(ModuleParameters))
            .AddSingleton<DialogService>()
            .AddSingleton<TextureService>()
            .BuildServiceProvider();
    }

    private void CompassLocationChanged(object sender, ValueChangedEventArgs<float> e)
    {
        if (_compassControl == null) return;
        _compassControl.LocationFactorX = _settingsDataModel.CompassLocationX.Value;
        _compassControl.LocationFactorY = _settingsDataModel.CompassLocationY.Value;
        _compassControl.RecalculateLayout();
    }

    protected override void Update(GameTime gameTime)
    {
        if (!GameService.Gw2Mumble.IsAvailable) return;
        if (!GameService.GameIntegration.Gw2Instance.IsInGame) return;
        _compassControl?.Update(gameTime);
    }

    /// <inheritdoc />
    public override IView GetSettingsView() => SettingsView;

    /// <inheritdoc />
    protected override void Unload()
    {
        // Unload here
        if (_compassControl != null)
        {
            _compassControl.Parent = null;
            _compassControl?.Dispose();
            _compassControl = null;
        }
        // _SettingsDataModel = null;
        // All static members must be manually unset
    }

    /// <inheritdoc />
    protected override void OnModuleLoaded(EventArgs e)
    {
        _compassControl?.RecalculateLayout();
        
        base.OnModuleLoaded(e);
    }
}