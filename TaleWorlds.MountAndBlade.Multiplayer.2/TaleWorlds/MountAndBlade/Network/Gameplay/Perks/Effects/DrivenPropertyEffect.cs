using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002D RID: 45
	public class DrivenPropertyEffect : MPPerkEffect
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00008BCD File Offset: 0x00006DCD
		protected DrivenPropertyEffect()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008BD8 File Offset: 0x00006DD8
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
					XmlAttribute xmlAttribute2 = attributes2["is_ratio"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			this._isRatio = ((text4 != null) ? text4.ToLower() : null) == "true";
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
					XmlAttribute xmlAttribute3 = attributes3["driven_property"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			if (!Enum.TryParse<DrivenProperty>(text5, true, out this._drivenProperty))
			{
				Debug.FailedAssert("provided 'driven_property' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DrivenPropertyEffect.cs", "Deserialize", 28);
			}
			string text6;
			if (node == null)
			{
				text6 = null;
			}
			else
			{
				XmlAttributeCollection attributes4 = node.Attributes;
				if (attributes4 == null)
				{
					text6 = null;
				}
				else
				{
					XmlAttribute xmlAttribute4 = attributes4["value"];
					text6 = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
				}
			}
			string text7 = text6;
			if (text7 == null || !float.TryParse(text7, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DrivenPropertyEffect.cs", "Deserialize", 35);
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008D0F File Offset: 0x00006F0F
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008D30 File Offset: 0x00006F30
		public override float GetDrivenPropertyBonus(DrivenProperty drivenProperty, float baseValue)
		{
			if (drivenProperty != this._drivenProperty)
			{
				return 0f;
			}
			if (!this._isRatio)
			{
				return this._value;
			}
			return baseValue * this._value;
		}

		// Token: 0x0400007E RID: 126
		protected static string StringType = "DrivenProperty";

		// Token: 0x0400007F RID: 127
		private DrivenProperty _drivenProperty;

		// Token: 0x04000080 RID: 128
		private float _value;

		// Token: 0x04000081 RID: 129
		private bool _isRatio;
	}
}
