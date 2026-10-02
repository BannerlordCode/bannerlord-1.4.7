using System;
using System.Globalization;
using System.Xml;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000162 RID: 354
	public class Badge
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0000F1D9 File Offset: 0x0000D3D9
		public int Index { get; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0000F1E1 File Offset: 0x0000D3E1
		public BadgeType Type { get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0000F1E9 File Offset: 0x0000D3E9
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x0000F1F1 File Offset: 0x0000D3F1
		public string StringId { get; private set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0000F1FA File Offset: 0x0000D3FA
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0000F202 File Offset: 0x0000D402
		public string GroupId { get; private set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0000F20B File Offset: 0x0000D40B
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0000F213 File Offset: 0x0000D413
		public TextObject Name { get; private set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0000F21C File Offset: 0x0000D41C
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0000F224 File Offset: 0x0000D424
		public TextObject Description { get; private set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0000F22D File Offset: 0x0000D42D
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x0000F235 File Offset: 0x0000D435
		public bool IsVisibleOnlyWhenEarned { get; private set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0000F23E File Offset: 0x0000D43E
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0000F246 File Offset: 0x0000D446
		public DateTime PeriodStart { get; private set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0000F24F File Offset: 0x0000D44F
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0000F257 File Offset: 0x0000D457
		public DateTime PeriodEnd { get; private set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0000F260 File Offset: 0x0000D460
		public bool IsActive
		{
			get
			{
				return DateTime.UtcNow >= this.PeriodStart && DateTime.UtcNow <= this.PeriodEnd;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0000F286 File Offset: 0x0000D486
		public bool IsTimed
		{
			get
			{
				return this.PeriodStart > DateTime.MinValue || this.PeriodEnd < DateTime.MaxValue;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0000F2AC File Offset: 0x0000D4AC
		public Badge(int index, BadgeType badgeType)
		{
			this.Index = index;
			this.Type = badgeType;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
		public virtual void Deserialize(XmlNode node)
		{
			this.StringId = node.Attributes["id"].Value;
			XmlAttributeCollection attributes = node.Attributes;
			string text;
			if (attributes == null)
			{
				text = null;
			}
			else
			{
				XmlAttribute xmlAttribute = attributes["group_id"];
				text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
			}
			string text2 = text;
			this.GroupId = (string.IsNullOrWhiteSpace(text2) ? null : text2);
			string value = node.Attributes["name"].Value;
			string value2 = node.Attributes["description"].Value;
			XmlAttribute xmlAttribute2 = node.Attributes["is_visible_only_when_earned"];
			this.IsVisibleOnlyWhenEarned = Convert.ToBoolean((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			XmlAttribute xmlAttribute3 = node.Attributes["period_start"];
			DateTime dateTime;
			this.PeriodStart = (DateTime.TryParse((xmlAttribute3 != null) ? xmlAttribute3.Value : null, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) : DateTime.MinValue);
			XmlAttribute xmlAttribute4 = node.Attributes["period_end"];
			DateTime dateTime2;
			this.PeriodEnd = (DateTime.TryParse((xmlAttribute4 != null) ? xmlAttribute4.Value : null, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime2) ? DateTime.SpecifyKind(dateTime2, DateTimeKind.Utc) : DateTime.MaxValue);
			this.Name = new TextObject(value, null);
			this.Description = new TextObject(value2, null);
		}
	}
}
