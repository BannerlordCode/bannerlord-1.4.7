using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000166 RID: 358
	public class BadgeCondition
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0000F40E File Offset: 0x0000D60E
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0000F416 File Offset: 0x0000D616
		public ConditionType Type { get; private set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0000F41F File Offset: 0x0000D61F
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0000F427 File Offset: 0x0000D627
		public ConditionGroupType GroupType { get; private set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0000F430 File Offset: 0x0000D630
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0000F438 File Offset: 0x0000D638
		public TextObject Description { get; private set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0000F441 File Offset: 0x0000D641
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x0000F449 File Offset: 0x0000D649
		public string StringId { get; private set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0000F452 File Offset: 0x0000D652
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0000F45A File Offset: 0x0000D65A
		public IReadOnlyDictionary<string, string> Parameters { get; private set; }

		// Token: 0x060009EF RID: 2543 RVA: 0x0000F464 File Offset: 0x0000D664
		public BadgeCondition(int index, XmlNode node)
		{
			XmlAttributeCollection attributes = node.Attributes;
			ConditionType conditionType;
			if (!Enum.TryParse<ConditionType>((attributes != null) ? attributes["type"].Value : null, true, out conditionType))
			{
				conditionType = ConditionType.Custom;
				Debug.FailedAssert("No 'type' was provided for a condition", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", ".ctor", 47);
			}
			this.Type = conditionType;
			ConditionGroupType conditionGroupType = ConditionGroupType.Any;
			XmlAttributeCollection attributes2 = node.Attributes;
			bool flag;
			if (attributes2 == null)
			{
				flag = null != null;
			}
			else
			{
				XmlAttribute xmlAttribute = attributes2["group_type"];
				flag = ((xmlAttribute != null) ? xmlAttribute.Value : null) != null;
			}
			if (flag && !Enum.TryParse<ConditionGroupType>(node.Attributes["group_type"].Value, true, out conditionGroupType))
			{
				Debug.FailedAssert("Provided 'group_type' was wrong for a condition", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", ".ctor", 54);
			}
			this.GroupType = conditionGroupType;
			XmlAttributeCollection attributes3 = node.Attributes;
			this.Description = new TextObject((attributes3 != null) ? attributes3["description"].Value : null, null);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Parameter")
				{
					string text = xmlNode.Attributes["name"].Value.Trim();
					string text2 = xmlNode.Attributes["value"].Value.Trim();
					dictionary[text] = text2;
				}
			}
			this.Parameters = dictionary;
			XmlAttributeCollection attributes4 = node.Attributes;
			string text3;
			if (attributes4 == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttribute xmlAttribute2 = attributes4["id"];
				text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			}
			this.StringId = text3;
			string text4;
			if (this.StringId == null && this.Parameters.TryGetValue("property", out text4))
			{
				this.StringId = text4 + ((this.GroupType == ConditionGroupType.Party) ? ".Party" : ((this.GroupType == ConditionGroupType.Solo) ? ".Solo" : ""));
			}
			if (this.StringId == null)
			{
				this.StringId = "condition." + index;
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0000F68C File Offset: 0x0000D88C
		public bool Check(string value)
		{
			ConditionType type = this.Type;
			if (type != ConditionType.PlayerData)
			{
				return false;
			}
			string text;
			if (!this.Parameters.TryGetValue("value", out text))
			{
				Debug.FailedAssert("Given condition doesn't have a value parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 94);
				return false;
			}
			return value == text;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0000F6DC File Offset: 0x0000D8DC
		public bool Check(int value)
		{
			ConditionType type = this.Type;
			if (type - ConditionType.PlayerDataNumeric > 2)
			{
				return false;
			}
			string text;
			if (this.Parameters.TryGetValue("value", out text))
			{
				int num;
				if (!int.TryParse(text, out num))
				{
					Debug.FailedAssert("Given condition value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 115);
					return false;
				}
				return value == num;
			}
			else
			{
				string text2;
				bool flag = this.Parameters.TryGetValue("min_value", out text2);
				string text3;
				bool flag2 = this.Parameters.TryGetValue("max_value", out text3);
				int minValue = int.MinValue;
				int maxValue = int.MaxValue;
				if (flag && !int.TryParse(text2, out minValue))
				{
					Debug.FailedAssert("Given condition min_value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 129);
					return false;
				}
				if (flag2 && !int.TryParse(text3, out maxValue))
				{
					Debug.FailedAssert("Given condition max_value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 134);
					return false;
				}
				return (flag || flag2) && value >= minValue && value <= maxValue;
			}
		}
	}
}
