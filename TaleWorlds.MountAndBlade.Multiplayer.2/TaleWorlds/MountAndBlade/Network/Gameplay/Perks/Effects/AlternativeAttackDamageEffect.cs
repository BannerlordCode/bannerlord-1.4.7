using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000027 RID: 39
	public class AlternativeAttackDamageEffect : MPPerkEffect
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000868D File Offset: 0x0000688D
		protected AlternativeAttackDamageEffect()
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008698 File Offset: 0x00006898
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
					XmlAttribute xmlAttribute2 = attributes2["attack_type"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			this._attackType = AlternativeAttackDamageEffect.AttackType.Any;
			if (text4 != null && !Enum.TryParse<AlternativeAttackDamageEffect.AttackType>(text4, true, out this._attackType))
			{
				this._attackType = AlternativeAttackDamageEffect.AttackType.Any;
				Debug.FailedAssert("provided 'attack_type' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\AlternativeAttackDamageEffect.cs", "Deserialize", 34);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["value"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null || !float.TryParse(text6, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\AlternativeAttackDamageEffect.cs", "Deserialize", 40);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000879C File Offset: 0x0000699C
		public override float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (isAlternativeAttack)
			{
				switch (this._attackType)
				{
				case AlternativeAttackDamageEffect.AttackType.Any:
					return this._value;
				case AlternativeAttackDamageEffect.AttackType.Kick:
					if (attackerWeapon != null)
					{
						return 0f;
					}
					return this._value;
				case AlternativeAttackDamageEffect.AttackType.Bash:
					if (attackerWeapon == null)
					{
						return 0f;
					}
					return this._value;
				}
			}
			return 0f;
		}

		// Token: 0x04000070 RID: 112
		protected static string StringType = "AlternativeAttackDamage";

		// Token: 0x04000071 RID: 113
		private AlternativeAttackDamageEffect.AttackType _attackType;

		// Token: 0x04000072 RID: 114
		private float _value;

		// Token: 0x020000A3 RID: 163
		private enum AttackType
		{
			// Token: 0x040001A7 RID: 423
			Any,
			// Token: 0x040001A8 RID: 424
			Kick,
			// Token: 0x040001A9 RID: 425
			Bash
		}
	}
}
