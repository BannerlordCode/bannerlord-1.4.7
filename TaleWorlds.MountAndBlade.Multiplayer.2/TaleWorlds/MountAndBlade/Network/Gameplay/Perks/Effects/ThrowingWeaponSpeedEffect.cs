using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000043 RID: 67
	public class ThrowingWeaponSpeedEffect : MPPerkEffect
	{
		// Token: 0x06000249 RID: 585 RVA: 0x0000A727 File Offset: 0x00008927
		protected ThrowingWeaponSpeedEffect()
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000A730 File Offset: 0x00008930
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ThrowingWeaponSpeedEffect.cs", "Deserialize", 24);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A7D4 File Offset: 0x000089D4
		public override float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
		{
			if (attackerWeapon == null || WeaponComponentData.GetItemTypeFromWeaponClass(attackerWeapon.WeaponClass) != ItemObject.ItemTypeEnum.Thrown)
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x040000B7 RID: 183
		protected static string StringType = "ThrowingWeaponSpeed";

		// Token: 0x040000B8 RID: 184
		private float _value;
	}
}
