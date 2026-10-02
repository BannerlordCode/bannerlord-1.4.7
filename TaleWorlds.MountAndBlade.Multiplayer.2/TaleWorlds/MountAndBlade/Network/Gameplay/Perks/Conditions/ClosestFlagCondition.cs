using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000046 RID: 70
	public class ClosestFlagCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000A92C File Offset: 0x00008B2C
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000A92F File Offset: 0x00008B2F
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000A932 File Offset: 0x00008B32
		protected ClosestFlagCondition()
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000A93C File Offset: 0x00008B3C
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
					XmlAttribute xmlAttribute = attributes["owner"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._owner = ClosestFlagCondition.FlagOwner.Any;
			if (text2 != null && !Enum.TryParse<ClosestFlagCondition.FlagOwner>(text2, true, out this._owner))
			{
				this._owner = ClosestFlagCondition.FlagOwner.Any;
				Debug.FailedAssert("provided 'owner' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\ClosestFlagCondition.cs", "Deserialize", 40);
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000A9A9 File Offset: 0x00008BA9
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				ClosestFlagCondition.FlagOwner flagOwner = ClosestFlagCondition.FlagOwner.None;
				float num = float.MaxValue;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated)
					{
						float num2 = agent.Position.DistanceSquared(flagCapturePoint.Position);
						if (num2 < num)
						{
							num = num2;
							Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
							if (flagOwnerTeam == null)
							{
								flagOwner = ClosestFlagCondition.FlagOwner.None;
							}
							else if (flagOwnerTeam == agent.Team)
							{
								flagOwner = ClosestFlagCondition.FlagOwner.Ally;
							}
							else
							{
								flagOwner = ClosestFlagCondition.FlagOwner.Enemy;
							}
						}
					}
				}
				return this._owner == ClosestFlagCondition.FlagOwner.Any || this._owner == flagOwner;
			}
			return false;
		}

		// Token: 0x040000BD RID: 189
		protected static string StringType = "FlagDominationClosestFlag";

		// Token: 0x040000BE RID: 190
		private ClosestFlagCondition.FlagOwner _owner;

		// Token: 0x020000AB RID: 171
		private enum FlagOwner
		{
			// Token: 0x040001C6 RID: 454
			Ally,
			// Token: 0x040001C7 RID: 455
			Enemy,
			// Token: 0x040001C8 RID: 456
			None,
			// Token: 0x040001C9 RID: 457
			Any
		}
	}
}
