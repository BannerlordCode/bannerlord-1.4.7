using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000038 RID: 56
	public class MountManeuverEffect : MPPerkEffect
	{
		// Token: 0x06000218 RID: 536 RVA: 0x00009866 File Offset: 0x00007A66
		protected MountManeuverEffect()
		{
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00009870 File Offset: 0x00007A70
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountManeuverEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009914 File Offset: 0x00007B14
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && !agent.IsMount) ? agent.MountAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009935 File Offset: 0x00007B35
		public override float GetMountManeuver()
		{
			return this._value;
		}

		// Token: 0x0400009D RID: 157
		protected static string StringType = "MountManeuver";

		// Token: 0x0400009E RID: 158
		private float _value;
	}
}
