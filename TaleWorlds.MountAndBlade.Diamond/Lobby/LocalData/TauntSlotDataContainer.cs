using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000178 RID: 376
	public class TauntSlotDataContainer : MultiplayerLocalDataContainer<TauntSlotData>
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x00011336 File Offset: 0x0000F536
		protected override string GetSaveDirectoryName()
		{
			return "Data";
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001133D File Offset: 0x0000F53D
		protected override string GetSaveFileName()
		{
			return "TauntSlots.json";
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00011344 File Offset: 0x0000F544
		protected override PlatformFilePath GetCompatibilityFilePath()
		{
			return new PlatformFilePath(new PlatformDirectoryPath(PlatformFileType.User, "Data"), "Taunts.json");
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001135C File Offset: 0x0000F55C
		protected override List<TauntSlotData> DeserializeInCompatibilityMode(string serializedJson)
		{
			List<TauntSlotData> list = new List<TauntSlotData>();
			try
			{
				Dictionary<string, List<ValueTuple<string, int>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<ValueTuple<string, int>>>>(serializedJson);
				if (dictionary != null)
				{
					foreach (KeyValuePair<string, List<ValueTuple<string, int>>> keyValuePair in dictionary)
					{
						string key = keyValuePair.Key;
						List<TauntIndexData> list2 = new List<TauntIndexData>();
						if (keyValuePair.Value != null)
						{
							foreach (ValueTuple<string, int> valueTuple in keyValuePair.Value)
							{
								if (string.IsNullOrEmpty(valueTuple.Item1))
								{
									Debug.FailedAssert("Taunt id is null when trying to load in compatibility mode", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\LocalData\\TauntSlotDataContainer.cs", "DeserializeInCompatibilityMode", 120);
								}
								else
								{
									for (int i = 0; i < list2.Count; i++)
									{
										if (list2[i].TauntIndex == valueTuple.Item2)
										{
											Debug.FailedAssert("Taunt index used for multiple taunts", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\LocalData\\TauntSlotDataContainer.cs", "DeserializeInCompatibilityMode", 128);
										}
									}
									list2.Add(new TauntIndexData(valueTuple.Item1, valueTuple.Item2));
								}
							}
						}
						list.Add(new TauntSlotData(key)
						{
							TauntIndices = list2
						});
					}
				}
			}
			catch
			{
				Debug.FailedAssert("Failed to resolve taunt slot data in compatibility mode. Resetting local data.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\LocalData\\TauntSlotDataContainer.cs", "DeserializeInCompatibilityMode", 145);
			}
			return list;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00011510 File Offset: 0x0000F710
		public MBReadOnlyList<TauntIndexData> GetTauntIndicesForPlayer(string playerId)
		{
			MBReadOnlyList<TauntSlotData> entries = base.GetEntries();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].PlayerId == playerId)
				{
					return new MBReadOnlyList<TauntIndexData>(entries[i].TauntIndices);
				}
			}
			return null;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001155C File Offset: 0x0000F75C
		public void SetTauntIndicesForPlayer(string playerId, List<TauntIndexData> tauntIndices)
		{
			MBReadOnlyList<TauntSlotData> entries = base.GetEntries();
			TauntSlotData tauntSlotData = null;
			int num = -1;
			for (int i = 0; i < entries.Count; i++)
			{
				TauntSlotData tauntSlotData2 = entries[i];
				if (tauntSlotData2.PlayerId == playerId)
				{
					tauntSlotData = tauntSlotData2;
					num = i;
					break;
				}
			}
			TauntSlotData tauntSlotData3 = new TauntSlotData(playerId);
			tauntSlotData3.TauntIndices = tauntIndices.ToList<TauntIndexData>();
			if (tauntSlotData != null)
			{
				base.RemoveEntry(tauntSlotData);
				base.InsertEntry(tauntSlotData3, num);
				return;
			}
			base.AddEntry(tauntSlotData3);
		}
	}
}
