using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000041 RID: 65
	public class SpeedBonusEffect : MPCombatPerkEffect
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000A55C File Offset: 0x0000875C
		protected SpeedBonusEffect()
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000A564 File Offset: 0x00008764
		protected override void Deserialize(XmlNode node)
		{
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
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
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
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !float.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\SpeedBonusEffect.cs", "Deserialize", 24);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000A608 File Offset: 0x00008808
		public override float GetSpeedBonusEffectiveness(Agent attacker, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			attacker = ((attacker != null && attacker.IsMount) ? attacker.RiderAgent : attacker);
			if (attacker == null)
			{
				return 0f;
			}
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x040000B3 RID: 179
		protected static string StringType = "SpeedBonus";

		// Token: 0x040000B4 RID: 180
		private float _value;
	}
}
