using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000028 RID: 40
	public class AlternativeEquipmentEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x000087FF File Offset: 0x000069FF
		protected AlternativeEquipmentEffect()
		{
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008808 File Offset: 0x00006A08
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
			this._item = default(EquipmentElement);
			this._index = EquipmentIndex.None;
			XmlNode xmlNode;
			if (node == null)
			{
				xmlNode = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				xmlNode = ((attributes != null) ? attributes["item"] : null);
			}
			XmlNode xmlNode2 = xmlNode;
			if (xmlNode2 != null)
			{
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(xmlNode2.Value);
				this._item = new EquipmentElement(@object, null, null, false);
			}
			XmlNode xmlNode3;
			if (node == null)
			{
				xmlNode3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				xmlNode3 = ((attributes2 != null) ? attributes2["slot"] : null);
			}
			XmlNode xmlNode4 = xmlNode3;
			if (xmlNode4 != null)
			{
				this._index = Equipment.GetEquipmentIndexFromOldEquipmentIndexName(xmlNode4.Value);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000088A4 File Offset: 0x00006AA4
		public override List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAll)
		{
			if (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Any || (isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops)))
			{
				if (alternativeEquipments == null)
				{
					alternativeEquipments = new List<ValueTuple<EquipmentIndex, EquipmentElement>>
					{
						new ValueTuple<EquipmentIndex, EquipmentElement>(this._index, this._item)
					};
				}
				else
				{
					alternativeEquipments.Add(new ValueTuple<EquipmentIndex, EquipmentElement>(this._index, this._item));
				}
			}
			return alternativeEquipments;
		}

		// Token: 0x04000073 RID: 115
		protected static string StringType = "AlternativeEquipmentOnSpawn";

		// Token: 0x04000074 RID: 116
		private EquipmentElement _item;

		// Token: 0x04000075 RID: 117
		private EquipmentIndex _index;
	}
}
