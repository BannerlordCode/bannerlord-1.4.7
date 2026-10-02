using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class BadgeDataEntry
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00005270 File Offset: 0x00003470
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x00005278 File Offset: 0x00003478
		[JsonProperty]
		public PlayerId PlayerId { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00005281 File Offset: 0x00003481
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x00005289 File Offset: 0x00003489
		[JsonProperty]
		public string BadgeId { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00005292 File Offset: 0x00003492
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0000529A File Offset: 0x0000349A
		[JsonProperty]
		public string ConditionId { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000052A3 File Offset: 0x000034A3
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x000052AB File Offset: 0x000034AB
		[JsonProperty]
		public int Count { get; set; }

		// Token: 0x0600048F RID: 1167 RVA: 0x000052BC File Offset: 0x000034BC
		public static Dictionary<ValueTuple<PlayerId, string, string>, int> ToDictionary(List<BadgeDataEntry> entries)
		{
			Dictionary<ValueTuple<PlayerId, string, string>, int> dictionary = new Dictionary<ValueTuple<PlayerId, string, string>, int>();
			if (entries != null)
			{
				foreach (BadgeDataEntry badgeDataEntry in entries)
				{
					dictionary.Add(new ValueTuple<PlayerId, string, string>(badgeDataEntry.PlayerId, badgeDataEntry.BadgeId, badgeDataEntry.ConditionId), badgeDataEntry.Count);
				}
			}
			return dictionary;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00005330 File Offset: 0x00003530
		public static List<BadgeDataEntry> ToList(Dictionary<ValueTuple<PlayerId, string, string>, int> dictionary)
		{
			List<BadgeDataEntry> list = new List<BadgeDataEntry>();
			if (dictionary != null)
			{
				foreach (KeyValuePair<ValueTuple<PlayerId, string, string>, int> keyValuePair in dictionary)
				{
					list.Add(new BadgeDataEntry
					{
						PlayerId = keyValuePair.Key.Item1,
						BadgeId = keyValuePair.Key.Item2,
						ConditionId = keyValuePair.Key.Item3,
						Count = keyValuePair.Value
					});
				}
			}
			return list;
		}
	}
}
