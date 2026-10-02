using System;
using System.Collections;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200004A RID: 74
	public class WeaponDescription : MBObjectBase
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x00015996 File Offset: 0x00013B96
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x0001599E File Offset: 0x00013B9E
		public WeaponClass WeaponClass { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x000159A7 File Offset: 0x00013BA7
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x000159AF File Offset: 0x00013BAF
		public WeaponFlags WeaponFlags { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x000159B8 File Offset: 0x00013BB8
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x000159C0 File Offset: 0x00013BC0
		public string ItemUsageFeatures { get; private set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000159C9 File Offset: 0x00013BC9
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x000159D1 File Offset: 0x00013BD1
		public bool RotatedInHand { get; private set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x000159DA File Offset: 0x00013BDA
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x000159E2 File Offset: 0x00013BE2
		public bool IsHiddenFromUI { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000159EB File Offset: 0x00013BEB
		public MBReadOnlyList<CraftingPiece> AvailablePieces
		{
			get
			{
				return this._availablePieces;
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000159F4 File Offset: 0x00013BF4
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.WeaponClass = ((node.Attributes["weapon_class"] != null) ? ((WeaponClass)Enum.Parse(typeof(WeaponClass), node.Attributes["weapon_class"].Value)) : WeaponClass.Undefined);
			this.ItemUsageFeatures = ((node.Attributes["item_usage_features"] != null) ? node.Attributes["item_usage_features"].Value : "");
			this.RotatedInHand = XmlHelper.ReadBool(node, "rotated_in_hand");
			this.UseCenterOfMassAsHandBase = XmlHelper.ReadBool(node, "use_center_of_mass_as_hand_base");
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "WeaponFlags")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							this.WeaponFlags |= (WeaponFlags)Enum.Parse(typeof(WeaponFlags), xmlNode2.Attributes["value"].Value);
						}
						continue;
					}
				}
				if (xmlNode.Name == "AvailablePieces")
				{
					this._availablePieces = new MBList<CraftingPiece>();
					foreach (object obj3 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj3;
						if (xmlNode3.NodeType == XmlNodeType.Element)
						{
							string value = xmlNode3.Attributes["id"].Value;
							CraftingPiece @object = MBObjectManager.Instance.GetObject<CraftingPiece>(value);
							if (@object != null)
							{
								this._availablePieces.Add(@object);
							}
						}
					}
				}
			}
		}

		// Token: 0x040002EB RID: 747
		public bool UseCenterOfMassAsHandBase;

		// Token: 0x040002ED RID: 749
		private MBList<CraftingPiece> _availablePieces;
	}
}
