using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003B RID: 59
	public class RangedAccuracyEffect : MPPerkEffect
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00009CCC File Offset: 0x00007ECC
		protected RangedAccuracyEffect()
		{
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009CD4 File Offset: 0x00007ED4
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RangedAccuracyEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009D78 File Offset: 0x00007F78
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00009D99 File Offset: 0x00007F99
		public override float GetRangedAccuracy()
		{
			return this._value;
		}

		// Token: 0x040000A3 RID: 163
		protected static string StringType = "RangedAccuracy";

		// Token: 0x040000A4 RID: 164
		private float _value;
	}
}
