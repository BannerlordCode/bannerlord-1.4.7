using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002B RID: 43
	public class DamageInterruptionThresholdEffect : MPPerkEffect
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00008A79 File Offset: 0x00006C79
		protected DamageInterruptionThresholdEffect()
		{
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008A84 File Offset: 0x00006C84
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageInterruptionThresholdEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008B28 File Offset: 0x00006D28
		public override float GetDamageInterruptionThreshold()
		{
			return this._value;
		}

		// Token: 0x0400007A RID: 122
		protected static string StringType = "DamageInterruptionThreshold";

		// Token: 0x0400007B RID: 123
		private float _value;
	}
}
