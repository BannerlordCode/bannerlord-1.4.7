using System;
using TaleWorlds.SaveSystem;

namespace StoryMode.Extensions
{
	// Token: 0x02000059 RID: 89
	public static class MetaDataExtensions
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
		public static bool HasStoryMode(this MetaData metaData)
		{
			bool flag = false;
			string text;
			if (metaData != null && metaData.TryGetValue("Modules", out text))
			{
				string[] array = text.Split(new char[] { ';' });
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Equals(array[i], "StoryMode", StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001FE2C File Offset: 0x0001E02C
		public static bool AreAchievementsDisabled(this MetaData metaData)
		{
			string text;
			int num;
			return metaData != null && metaData.TryGetValue("AchievementsDisabled", out text) && int.TryParse(text, out num) && num == 1;
		}
	}
}
