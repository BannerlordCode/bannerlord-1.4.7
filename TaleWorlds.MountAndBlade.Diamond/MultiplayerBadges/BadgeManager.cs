using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000167 RID: 359
	public static class BadgeManager
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0000F7D3 File Offset: 0x0000D9D3
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0000F7DA File Offset: 0x0000D9DA
		public static List<Badge> Badges { get; private set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0000F7E2 File Offset: 0x0000D9E2
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0000F7E9 File Offset: 0x0000D9E9
		public static bool IsInitialized { get; private set; }

		// Token: 0x060009F6 RID: 2550 RVA: 0x0000F7F1 File Offset: 0x0000D9F1
		public static void InitializeWithXML(string xmlPath)
		{
			Debug.Print("BadgeManager::InitializeWithXML", 0, Debug.DebugColor.White, 17592186044416UL);
			if (BadgeManager.IsInitialized)
			{
				return;
			}
			BadgeManager.LoadFromXml(xmlPath);
			BadgeManager.IsInitialized = true;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0000F820 File Offset: 0x0000DA20
		public static void OnFinalize()
		{
			Debug.Print("BadgeManager::OnFinalize", 0, Debug.DebugColor.White, 17592186044416UL);
			if (!BadgeManager.IsInitialized)
			{
				return;
			}
			BadgeManager._badgesById.Clear();
			BadgeManager._badgesByType.Clear();
			BadgeManager.Badges.Clear();
			BadgeManager._badgesById = null;
			BadgeManager._badgesByType = null;
			BadgeManager.Badges = null;
			BadgeManager.IsInitialized = false;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0000F884 File Offset: 0x0000DA84
		private static void LoadFromXml(string path)
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (StreamReader streamReader = new StreamReader(path))
			{
				string text = streamReader.ReadToEnd();
				xmlDocument.LoadXml(text);
				streamReader.Close();
			}
			BadgeManager._badgesById = new Dictionary<string, Badge>();
			BadgeManager._badgesByType = new Dictionary<BadgeType, List<Badge>>();
			BadgeManager.Badges = new List<Badge>();
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Badges")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Badge")
						{
							BadgeType badgeType = BadgeType.Custom;
							if (!Enum.TryParse<BadgeType>(xmlNode2.Attributes["type"].Value, true, out badgeType))
							{
								Debug.FailedAssert("No 'type' was provided for a badge", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "LoadFromXml", 82);
							}
							Badge badge = null;
							if (badgeType > BadgeType.OnLogin)
							{
								if (badgeType == BadgeType.Conditional)
								{
									badge = new ConditionalBadge(BadgeManager.Badges.Count, badgeType);
								}
							}
							else
							{
								badge = new Badge(BadgeManager.Badges.Count, badgeType);
							}
							badge.Deserialize(xmlNode2);
							BadgeManager._badgesById[badge.StringId] = badge;
							BadgeManager.Badges.Add(badge);
							List<Badge> list;
							if (!BadgeManager._badgesByType.TryGetValue(badgeType, out list))
							{
								list = new List<Badge>();
								BadgeManager._badgesByType.Add(badgeType, list);
							}
							list.Add(badge);
						}
					}
				}
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		public static Badge GetByIndex(int index)
		{
			if (index == -1 || BadgeManager.Badges == null || BadgeManager.Badges.Count <= index || index < 0)
			{
				return null;
			}
			return BadgeManager.Badges[index];
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0000FACC File Offset: 0x0000DCCC
		public static Badge GetById(string id)
		{
			Badge badge;
			if (id == null || !BadgeManager._badgesById.TryGetValue(id, out badge))
			{
				return null;
			}
			return badge;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		public static List<Badge> GetByType(BadgeType type)
		{
			List<Badge> list;
			if (!BadgeManager._badgesByType.TryGetValue(type, out list))
			{
				list = new List<Badge>();
				BadgeManager._badgesByType.Add(type, list);
			}
			return list;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0000FB20 File Offset: 0x0000DD20
		public static string GetBadgeConditionValue(this PlayerData playerData, BadgeCondition condition)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData is null on get value", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionValue", 143);
				return "";
			}
			string text;
			if (!condition.Parameters.TryGetValue("property", out text))
			{
				Debug.FailedAssert("Condition with type PlayerData does not have Property parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionValue", 150);
				return "";
			}
			if (text == "ShownBadgeId")
			{
				return playerData.ShownBadgeId;
			}
			return "";
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public static int GetBadgeConditionNumericValue(this PlayerData playerData, BadgeCondition condition)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData is null on get value", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionNumericValue", 167);
				return 0;
			}
			string text;
			if (!condition.Parameters.TryGetValue("property", out text))
			{
				Debug.FailedAssert("Condition with type PlayerDataNumeric does not have Property parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionNumericValue", 174);
				return 0;
			}
			int num = 0;
			string[] array = text.Split(new char[] { '.' });
			string text2 = array[0];
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num2 <= 1096112509U)
			{
				if (num2 <= 267161228U)
				{
					if (num2 != 192547213U)
					{
						if (num2 == 267161228U)
						{
							if (text2 == "Stats")
							{
								if (array.Length == 3 && playerData.Stats != null)
								{
									string text3 = array[1].Trim().ToLower();
									PlayerStatsBase[] stats = playerData.Stats;
									int i = 0;
									while (i < stats.Length)
									{
										PlayerStatsBase playerStatsBase = stats[i];
										if (playerStatsBase.GameType.Trim().ToLower() == text3)
										{
											text2 = array[2];
											if (text2 == "KillCount")
											{
												num = playerStatsBase.KillCount;
												break;
											}
											if (text2 == "DeathCount")
											{
												num = playerStatsBase.DeathCount;
												break;
											}
											if (text2 == "AssistCount")
											{
												num = playerStatsBase.AssistCount;
												break;
											}
											if (text2 == "WinCount")
											{
												num = playerStatsBase.WinCount;
												break;
											}
											if (!(text2 == "LoseCount"))
											{
												break;
											}
											num = playerStatsBase.LoseCount;
											break;
										}
										else
										{
											i++;
										}
									}
								}
							}
						}
					}
					else if (text2 == "AssistCount")
					{
						num = playerData.AssistCount;
					}
				}
				else if (num2 != 1093842208U)
				{
					if (num2 == 1096112509U)
					{
						if (text2 == "Level")
						{
							num = playerData.Level;
						}
					}
				}
				else if (text2 == "WinCount")
				{
					num = playerData.WinCount;
				}
			}
			else if (num2 <= 2667250970U)
			{
				if (num2 != 1128891543U)
				{
					if (num2 == 2667250970U)
					{
						if (text2 == "Playtime")
						{
							num = playerData.Playtime;
						}
					}
				}
				else if (text2 == "LoseCount")
				{
					num = playerData.LoseCount;
				}
			}
			else if (num2 != 3945868512U)
			{
				if (num2 == 4058818476U)
				{
					if (text2 == "DeathCount")
					{
						num = playerData.DeathCount;
					}
				}
			}
			else if (text2 == "KillCount")
			{
				num = playerData.KillCount;
			}
			return num;
		}

		// Token: 0x040004DD RID: 1245
		public const string PropertyParameterName = "property";

		// Token: 0x040004DE RID: 1246
		public const string ValueParameterName = "value";

		// Token: 0x040004DF RID: 1247
		public const string MinValueParameterName = "min_value";

		// Token: 0x040004E0 RID: 1248
		public const string MaxValueParameterName = "max_value";

		// Token: 0x040004E1 RID: 1249
		public const string IsBestParameterName = "is_best";

		// Token: 0x040004E4 RID: 1252
		private static Dictionary<string, Badge> _badgesById;

		// Token: 0x040004E5 RID: 1253
		private static Dictionary<BadgeType, List<Badge>> _badgesByType;
	}
}
