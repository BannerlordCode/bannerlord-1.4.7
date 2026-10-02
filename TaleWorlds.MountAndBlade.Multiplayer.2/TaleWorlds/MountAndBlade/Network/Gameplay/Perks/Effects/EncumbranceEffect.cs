using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002F RID: 47
	public class EncumbranceEffect : MPPerkEffect
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x00008EC5 File Offset: 0x000070C5
		protected EncumbranceEffect()
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008ED0 File Offset: 0x000070D0
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\EncumbranceEffect.cs", "Deserialize", 23);
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
					XmlAttribute xmlAttribute3 = attributes3["is_on_body"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			this._isOnBody = ((text6 != null) ? text6.ToLower() : null) == "true";
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008FBA File Offset: 0x000071BA
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008FDB File Offset: 0x000071DB
		public override float GetEncumbrance(bool isOnBody)
		{
			if (isOnBody != this._isOnBody)
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x04000086 RID: 134
		protected static string StringType = "Encumbrance";

		// Token: 0x04000087 RID: 135
		private bool _isOnBody;

		// Token: 0x04000088 RID: 136
		private float _value;
	}
}
