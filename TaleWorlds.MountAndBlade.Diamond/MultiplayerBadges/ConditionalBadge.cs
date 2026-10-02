using System;
using System.Collections.Generic;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000169 RID: 361
	public class ConditionalBadge : Badge
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0000FFF2 File Offset: 0x0000E1F2
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0000FFFA File Offset: 0x0000E1FA
		public IReadOnlyList<BadgeCondition> BadgeConditions { get; private set; }

		// Token: 0x06000A03 RID: 2563 RVA: 0x00010003 File Offset: 0x0000E203
		public ConditionalBadge(int index, BadgeType badgeType)
			: base(index, badgeType)
		{
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00010010 File Offset: 0x0000E210
		public override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
			List<BadgeCondition> list = new List<BadgeCondition>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Condition")
				{
					BadgeCondition badgeCondition = new BadgeCondition(list.Count, xmlNode);
					list.Add(badgeCondition);
				}
			}
			this.BadgeConditions = list;
		}
	}
}
