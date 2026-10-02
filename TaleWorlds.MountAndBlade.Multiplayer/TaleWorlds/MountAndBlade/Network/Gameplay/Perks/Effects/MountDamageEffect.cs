using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000035 RID: 53
	public class MountDamageEffect : MPCombatPerkEffect
	{
		// Token: 0x0600020B RID: 523 RVA: 0x000095D1 File Offset: 0x000077D1
		protected MountDamageEffect()
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000095DC File Offset: 0x000077DC
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountDamageEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009641 File Offset: 0x00007841
		public override float GetMountDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x04000096 RID: 150
		protected static string StringType = "MountDamage";

		// Token: 0x04000097 RID: 151
		private float _value;
	}
}
