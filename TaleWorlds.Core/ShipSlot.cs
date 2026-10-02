using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000C2 RID: 194
	public class ShipSlot : MBObjectBase
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00022E68 File Offset: 0x00021068
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00022E70 File Offset: 0x00021070
		public string TypeId { get; private set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00022E79 File Offset: 0x00021079
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00022E81 File Offset: 0x00021081
		public string MainPrefabId { get; private set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00022E8A File Offset: 0x0002108A
		public MBReadOnlyList<ShipUpgradePiece> MatchingPieces
		{
			get
			{
				return this._matchingPieces;
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00022E92 File Offset: 0x00021092
		public ShipSlot()
		{
			this._matchingPieces = new MBList<ShipUpgradePiece>();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00022EA5 File Offset: 0x000210A5
		public override void AfterRegister()
		{
			base.AfterRegister();
			this.Initialize();
			base.IsReady = true;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00022EBA File Offset: 0x000210BA
		public void AddMatchingPiece(ShipUpgradePiece upgradePiece)
		{
			if (!this._matchingPieces.Contains(upgradePiece))
			{
				this._matchingPieces.Add(upgradePiece);
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00022ED6 File Offset: 0x000210D6
		public TextObject GetSlotTypeName()
		{
			return GameTexts.FindText("str_ship_slot_type", this.TypeId);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00022EE8 File Offset: 0x000210E8
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			XmlAttribute xmlAttribute = node.Attributes["prefab_id"];
			this.MainPrefabId = ((xmlAttribute != null) ? xmlAttribute.Value : null) ?? base.StringId;
			string typeId = this.TypeId;
			XmlAttribute xmlAttribute2 = node.Attributes["type_id"];
			this.TypeId = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null) ?? base.StringId;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name.Equals("ShipUpgradePieces"))
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name.Equals("ShipUpgradePiece"))
						{
							string value = xmlNode2.Attributes["id"].Value;
							ShipUpgradePiece @object = MBObjectManager.Instance.GetObject<ShipUpgradePiece>(value);
							this.AddMatchingPiece(@object);
							@object.AddTargetSlot(this);
						}
					}
				}
			}
		}

		// Token: 0x040005FC RID: 1532
		private MBList<ShipUpgradePiece> _matchingPieces;
	}
}
