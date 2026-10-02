using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002E RID: 46
	public class DrivenPropertyOnSpawnEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x060001EC RID: 492 RVA: 0x00008D64 File Offset: 0x00006F64
		protected DrivenPropertyOnSpawnEffect()
		{
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008D6C File Offset: 0x00006F6C
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_ratio"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._isRatio = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["driven_property"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			if (!Enum.TryParse<DrivenProperty>(text3, true, out this._drivenProperty))
			{
				Debug.FailedAssert("provided 'driven_property' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DrivenPropertyOnSpawnEffect.cs", "Deserialize", 26);
			}
			string text4;
			if (node == null)
			{
				text4 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text4 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["value"];
					text4 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text5 = text4;
			if (text5 == null || !float.TryParse(text5, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DrivenPropertyOnSpawnEffect.cs", "Deserialize", 33);
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008E64 File Offset: 0x00007064
		public override float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			if (drivenProperty != this._drivenProperty || (this.EffectTarget != MPOnSpawnPerkEffectBase.Target.Any && !(isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops))))
			{
				return 0f;
			}
			if (!this._isRatio)
			{
				return this._value;
			}
			return baseValue * this._value;
		}

		// Token: 0x04000082 RID: 130
		protected static string StringType = "DrivenPropertyOnSpawn";

		// Token: 0x04000083 RID: 131
		private DrivenProperty _drivenProperty;

		// Token: 0x04000084 RID: 132
		private float _value;

		// Token: 0x04000085 RID: 133
		private bool _isRatio;
	}
}
