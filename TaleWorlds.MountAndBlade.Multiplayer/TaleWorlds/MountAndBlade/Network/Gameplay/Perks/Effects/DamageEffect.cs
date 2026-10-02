using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002A RID: 42
	public class DamageEffect : MPCombatPerkEffect
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000089E7 File Offset: 0x00006BE7
		protected DamageEffect()
		{
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000089F0 File Offset: 0x00006BF0
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008A55 File Offset: 0x00006C55
		public override float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x04000078 RID: 120
		protected static string StringType = "DamageDealt";

		// Token: 0x04000079 RID: 121
		private float _value;
	}
}
