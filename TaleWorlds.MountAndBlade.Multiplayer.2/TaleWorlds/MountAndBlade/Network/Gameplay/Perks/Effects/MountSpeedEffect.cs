using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000039 RID: 57
	public class MountSpeedEffect : MPPerkEffect
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00009949 File Offset: 0x00007B49
		protected MountSpeedEffect()
		{
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009954 File Offset: 0x00007B54
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountSpeedEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000099F8 File Offset: 0x00007BF8
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && !agent.IsMount) ? agent.MountAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009A19 File Offset: 0x00007C19
		public override float GetMountSpeed()
		{
			return this._value;
		}

		// Token: 0x0400009F RID: 159
		protected static string StringType = "MountSpeed";

		// Token: 0x040000A0 RID: 160
		private float _value;
	}
}
