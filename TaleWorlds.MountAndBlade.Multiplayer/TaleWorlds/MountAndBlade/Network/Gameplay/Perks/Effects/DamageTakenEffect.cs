using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002C RID: 44
	public class DamageTakenEffect : MPCombatPerkEffect
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00008B3C File Offset: 0x00006D3C
		protected DamageTakenEffect()
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008B44 File Offset: 0x00006D44
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageTakenEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008BA9 File Offset: 0x00006DA9
		public override float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x0400007C RID: 124
		protected static string StringType = "DamageTaken";

		// Token: 0x0400007D RID: 125
		private float _value;
	}
}
