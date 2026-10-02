using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003D RID: 61
	public class RewardGoldOnAssistEffect : MPPerkEffect
	{
		// Token: 0x0600022F RID: 559 RVA: 0x00009E70 File Offset: 0x00008070
		protected RewardGoldOnAssistEffect()
		{
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009E78 File Offset: 0x00008078
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnAssistEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009F1C File Offset: 0x0000811C
		public override int GetRewardedGoldOnAssist()
		{
			return this._value;
		}

		// Token: 0x040000A7 RID: 167
		protected static string StringType = "RewardGoldOnAssist";

		// Token: 0x040000A8 RID: 168
		private int _value;
	}
}
