using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000123 RID: 291
	internal class ItemInnerData
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x000086AD File Offset: 0x000068AD
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x000086B5 File Offset: 0x000068B5
		internal string TypeId { get; private set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x000086BE File Offset: 0x000068BE
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x000086C6 File Offset: 0x000068C6
		internal ItemType Type { get; private set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x000086CF File Offset: 0x000068CF
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x000086D7 File Offset: 0x000068D7
		internal int Price { get; private set; }

		// Token: 0x0600068E RID: 1678 RVA: 0x000086E0 File Offset: 0x000068E0
		internal void Deserialize(XmlNode node)
		{
			this.TypeId = node.Attributes["id"].Value;
			this.Price = ((node.Attributes["value"] != null) ? int.Parse(node.Attributes["value"].Value) : 0);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "flags")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "flag" && xmlNode2.Attributes["name"].Value == "type")
						{
							string value = xmlNode2.Attributes["value"].Value;
							this.Type = (ItemType)Enum.Parse(typeof(ItemType), value, true);
						}
					}
				}
			}
		}
	}
}
