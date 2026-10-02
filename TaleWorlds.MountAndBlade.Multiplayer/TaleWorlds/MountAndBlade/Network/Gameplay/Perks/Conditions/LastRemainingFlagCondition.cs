using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004B RID: 75
	public class LastRemainingFlagCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000AF0A File Offset: 0x0000910A
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000AF0D File Offset: 0x0000910D
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000AF10 File Offset: 0x00009110
		protected LastRemainingFlagCondition()
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000AF18 File Offset: 0x00009118
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
			this._owner = LastRemainingFlagCondition.FlagOwner.Any;
			if (text2 != null && !Enum.TryParse<LastRemainingFlagCondition.FlagOwner>(text2, true, out this._owner))
			{
				this._owner = LastRemainingFlagCondition.FlagOwner.Any;
				Debug.FailedAssert("provided 'owner' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\LastRemainingFlagCondition.cs", "Deserialize", 40);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000AF85 File Offset: 0x00009185
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000AF9C File Offset: 0x0000919C
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				LastRemainingFlagCondition.FlagOwner flagOwner = LastRemainingFlagCondition.FlagOwner.None;
				int num = 0;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated)
					{
						num++;
						Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
						if (flagOwnerTeam == null)
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.None;
						}
						else if (flagOwnerTeam == agent.Team)
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.Ally;
						}
						else
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.Enemy;
						}
					}
				}
				return num == 1 && (this._owner == LastRemainingFlagCondition.FlagOwner.Any || this._owner == flagOwner);
			}
			return false;
		}

		// Token: 0x040000C8 RID: 200
		protected static string StringType = "FlagDominationLastRemainingFlag";

		// Token: 0x040000C9 RID: 201
		private LastRemainingFlagCondition.FlagOwner _owner;

		// Token: 0x020000AD RID: 173
		private enum FlagOwner
		{
			// Token: 0x040001CF RID: 463
			Ally,
			// Token: 0x040001D0 RID: 464
			Enemy,
			// Token: 0x040001D1 RID: 465
			None,
			// Token: 0x040001D2 RID: 466
			Any
		}
	}
}
