using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000045 RID: 69
	public class AgentStatusCondition : MPPerkCondition
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000A881 File Offset: 0x00008A81
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.MountChange;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000A888 File Offset: 0x00008A88
		protected AgentStatusCondition()
		{
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000A890 File Offset: 0x00008A90
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
					XmlAttribute xmlAttribute = attributes["agent_status"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			if (!Enum.TryParse<AgentStatusCondition.AgentStatus>(text, true, out this._status))
			{
				Debug.FailedAssert("provided 'agent_status' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\AgentStatusCondition.cs", "Deserialize", 31);
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000A8EA File Offset: 0x00008AEA
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000A8FE File Offset: 0x00008AFE
		public override bool Check(Agent agent)
		{
			if (agent == null)
			{
				return false;
			}
			if (agent.MountAgent == null)
			{
				return this._status == AgentStatusCondition.AgentStatus.OnFoot;
			}
			return this._status == AgentStatusCondition.AgentStatus.OnMount;
		}

		// Token: 0x040000BB RID: 187
		protected static string StringType = "AgentStatus";

		// Token: 0x040000BC RID: 188
		private AgentStatusCondition.AgentStatus _status;

		// Token: 0x020000AA RID: 170
		private enum AgentStatus
		{
			// Token: 0x040001C3 RID: 451
			OnFoot,
			// Token: 0x040001C4 RID: 452
			OnMount
		}
	}
}
