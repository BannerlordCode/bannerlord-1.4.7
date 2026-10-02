using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000030 RID: 48
	public class GoldGainOnAssistEffect : MPPerkEffect
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00008FFE File Offset: 0x000071FE
		protected GoldGainOnAssistEffect()
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009008 File Offset: 0x00007208
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
			if (text4 == null || !int.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldGainOnAssistEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000090AC File Offset: 0x000072AC
		public override int GetGoldOnAssist()
		{
			return this._value;
		}

		// Token: 0x04000089 RID: 137
		protected static string StringType = "GoldGainOnAssist";

		// Token: 0x0400008A RID: 138
		private int _value;
	}
}
