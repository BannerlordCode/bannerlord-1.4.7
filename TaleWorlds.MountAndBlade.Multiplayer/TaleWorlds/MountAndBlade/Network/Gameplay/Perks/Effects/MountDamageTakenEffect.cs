using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000036 RID: 54
	public class MountDamageTakenEffect : MPCombatPerkEffect
	{
		// Token: 0x0600020F RID: 527 RVA: 0x00009665 File Offset: 0x00007865
		protected MountDamageTakenEffect()
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009670 File Offset: 0x00007870
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountDamageTakenEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000096D5 File Offset: 0x000078D5
		public override float GetMountDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x04000098 RID: 152
		protected static string StringType = "MountDamageTaken";

		// Token: 0x04000099 RID: 153
		private float _value;
	}
}
