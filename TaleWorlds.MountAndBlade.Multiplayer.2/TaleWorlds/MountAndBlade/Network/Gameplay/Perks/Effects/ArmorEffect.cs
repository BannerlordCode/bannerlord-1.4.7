using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000029 RID: 41
	public class ArmorEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x0000891A File Offset: 0x00006B1A
		protected ArmorEffect()
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008924 File Offset: 0x00006B24
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
					XmlAttribute xmlAttribute = attributes["value"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null || !float.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ArmorEffect.cs", "Deserialize", 21);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000898C File Offset: 0x00006B8C
		public override float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			if ((drivenProperty == DrivenProperty.ArmorHead || drivenProperty == DrivenProperty.ArmorTorso || drivenProperty == DrivenProperty.ArmorLegs || drivenProperty == DrivenProperty.ArmorArms) && (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Any || (isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops))))
			{
				return this._value;
			}
			return 0f;
		}

		// Token: 0x04000076 RID: 118
		protected static string StringType = "ArmorOnSpawn";

		// Token: 0x04000077 RID: 119
		private float _value;
	}
}
