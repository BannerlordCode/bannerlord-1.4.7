using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000025 RID: 37
	public static class ConfigurationManager
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000053D8 File Offset: 0x000035D8
		public static void SetConfigurationManager(IConfigurationManager configurationManager)
		{
			ConfigurationManager._configurationManager = configurationManager;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000053E0 File Offset: 0x000035E0
		public static string GetAppSettings(string name)
		{
			if (ConfigurationManager._configurationManager != null)
			{
				return ConfigurationManager._configurationManager.GetAppSettings(name);
			}
			return null;
		}

		// Token: 0x04000077 RID: 119
		private static IConfigurationManager _configurationManager;
	}
}
