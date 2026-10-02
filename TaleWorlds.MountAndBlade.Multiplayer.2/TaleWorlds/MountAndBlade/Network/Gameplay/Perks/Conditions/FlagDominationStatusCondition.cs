using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000048 RID: 72
	public class FlagDominationStatusCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000AB52 File Offset: 0x00008D52
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000AB55 File Offset: 0x00008D55
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000AB58 File Offset: 0x00008D58
		protected FlagDominationStatusCondition()
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000AB60 File Offset: 0x00008D60
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
					XmlAttribute xmlAttribute = attributes["status"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._status = FlagDominationStatusCondition.Status.Tie;
			if (text2 != null && !Enum.TryParse<FlagDominationStatusCondition.Status>(text2, true, out this._status))
			{
				this._status = FlagDominationStatusCondition.Status.Tie;
				Debug.FailedAssert("provided 'status' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\FlagDominationStatusCondition.cs", "Deserialize", 39);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000ABCD File Offset: 0x00008DCD
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent == null)
			{
				return false;
			}
			MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
			int num = 0;
			int num2 = 0;
			foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
			{
				if (!flagCapturePoint.IsDeactivated)
				{
					Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
					if (flagOwnerTeam == agent.Team)
					{
						num++;
					}
					else if (flagOwnerTeam != null)
					{
						num2++;
					}
				}
			}
			if (this._status == FlagDominationStatusCondition.Status.Winning)
			{
				return num > num2;
			}
			if (this._status != FlagDominationStatusCondition.Status.Losing)
			{
				return num == num2;
			}
			return num2 > num;
		}

		// Token: 0x040000C1 RID: 193
		protected static string StringType = "FlagDominationStatus";

		// Token: 0x040000C2 RID: 194
		private FlagDominationStatusCondition.Status _status;

		// Token: 0x020000AC RID: 172
		private enum Status
		{
			// Token: 0x040001CB RID: 459
			Winning,
			// Token: 0x040001CC RID: 460
			Losing,
			// Token: 0x040001CD RID: 461
			Tie
		}
	}
}
