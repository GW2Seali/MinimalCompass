using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using MinimalCompass.Services;

namespace MinimalCompass.Views.Subviews;

public sealed class CompassSubSettingsView : SettingView<SettingCollection>
{
  
  private FlowPanel _settingFlowPanel;
  private readonly SettingCollection _settings;
  private bool _lockBounds = true;
  private ViewContainer _lastSettingContainer;

  public CompassSubSettingsView(SettingEntry<SettingCollection> setting, int definedWidth = -1)
    : base(setting, definedWidth)
  {
    _settings = setting.Value;
  }

  public CompassSubSettingsView(ManagerService managerService, int definedWidth = -1)
    : this(new SettingEntry<SettingCollection>()
    {
      Value = managerService.SettingsManager.ModuleSettings
    }, definedWidth)
  {
  }

  public bool LockBounds
  {
    get => _lockBounds;
    set
    {
      if (_lockBounds == value)
        return;
      _lockBounds = value;
      UpdateBoundsLocking(_lockBounds);
    }
  }

  private void UpdateBoundsLocking(bool locked)
  {
    _settingFlowPanel.ShowBorder = !locked;
    _settingFlowPanel.CanCollapse = !locked;
  }

  protected override void BuildSetting(Container buildPanel)
  {
    FlowPanel flowPanel = new();
    flowPanel.Size = buildPanel.Size;
    flowPanel.FlowDirection = ControlFlowDirection.SingleTopToBottom;
    flowPanel.ControlPadding = new Vector2(5f, 2f);
    flowPanel.OuterControlPadding = new Vector2(10f, 15f);
    flowPanel.WidthSizingMode = SizingMode.Fill;
    flowPanel.HeightSizingMode = SizingMode.AutoSize;
    flowPanel.AutoSizePadding = new Point(0, 15);
    flowPanel.Parent = buildPanel;
    _settingFlowPanel = flowPanel;
    foreach (SettingEntry setting in _settings.Where<SettingEntry>(s => s.SessionDefined))
    {
      IView newView;
      if ((newView = SettingView.FromType(setting, _settingFlowPanel.Width)) != null)
      {
        ViewContainer viewContainer = new();
        viewContainer.WidthSizingMode = SizingMode.Fill;
        viewContainer.HeightSizingMode = SizingMode.AutoSize;
        viewContainer.Parent = _settingFlowPanel;
        _lastSettingContainer = viewContainer;
        _lastSettingContainer.Show(newView);
        
        if (newView is SettingsView settingsView)
          settingsView.LockBounds = false;
      }
    }
    UpdateBoundsLocking(_lockBounds);
  }

  protected override void RefreshDisplayName(string displayName)
  {
    _settingFlowPanel.Title = displayName;
  }

  protected override void RefreshDescription(string description)
  {
    _settingFlowPanel.BasicTooltipText = description;
  }

  protected override void RefreshValue(SettingCollection value)
  {
  }
}