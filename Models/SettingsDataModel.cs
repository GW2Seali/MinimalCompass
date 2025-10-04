using System;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Extensions.DependencyInjection;
using MinimalCompass.Services;
using MinimalCompass.Views;

namespace MinimalCompass.Models;

//TODO: Add more settings
// Such as color, position, etc.
public sealed class SettingsDataModel
{
	public SettingsDataModel(SettingCollection settingCollection)
	{
		SettingCollection = settingCollection;
		{
			CompassLocationX = settingCollection.DefineSetting("CompassLocationX", 0.5f, 
				() => "Compass Location X", 
				() => "The horizontal location of the compass on the screen, as a percentage of screen width.");
			CompassLocationX.SetRange(0f, 1f);
				
			CompassLocationY = settingCollection.DefineSetting("CompassLocationY", 0.1f, 
				() => "Compass Location Y",
				() => "The vertical location of the compass on the screen, as a percentage of screen height.");
			CompassLocationY.SetRange(0f, 1f);
		}
	}

	// public SettingCollection CompassSettings { get; }

	public SettingCollection SettingCollection { get; }
	
	
	public SettingEntry<float> CompassLocationX { get; }
	
	public SettingEntry<float> CompassLocationY { get; }
	
	public event EventHandler<ValueChangedEventArgs<float>> CompassLocationChanged
	{
		add
		{
			CompassLocationX.SettingChanged += value;
			CompassLocationY.SettingChanged += value;
		}
		remove
		{
			CompassLocationX.SettingChanged -= value;
			CompassLocationY.SettingChanged -= value;
			
		}
	}
}