using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000047 RID: 71
	public class ControllerCondition : MPPerkCondition
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.PeerControlledAgentChange;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000AAAC File Offset: 0x00008CAC
		protected ControllerCondition()
		{
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000AAB4 File Offset: 0x00008CB4
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
					XmlAttribute xmlAttribute = attributes["is_player_controlled"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._isPlayerControlled = ((text2 != null) ? text2.ToLower() : null) == "true";
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000AB07 File Offset: 0x00008D07
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000AB1B File Offset: 0x00008D1B
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			return agent != null && agent.IsPlayerControlled == this._isPlayerControlled;
		}

		// Token: 0x040000BF RID: 191
		protected static string StringType = "Controller";

		// Token: 0x040000C0 RID: 192
		private bool _isPlayerControlled;
	}
}
