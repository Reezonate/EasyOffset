using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using EasyOffset.Configuration;
using JetBrains.Annotations;

namespace EasyOffset.UI
{
    public class SettingsUI : NotifiableSingleton<SettingsUI>
    {
		[UIValue("enabled-bool")]
		internal bool EnabledValue
		{
			get => PluginConfig.Enabled;
			set => PluginConfig.Enabled = value;
		}
	}
}
