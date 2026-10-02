using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004E RID: 78
	public class OwnedFlagCountCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000B379 File Offset: 0x00009579
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000B37C File Offset: 0x0000957C
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B37F File Offset: 0x0000957F
		protected OwnedFlagCountCondition()
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B388 File Offset: 0x00009588
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
					XmlAttribute xmlAttribute = attributes["min"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null)
			{
				this._min = 0;
			}
			else if (!int.TryParse(text2, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\OwnedFlagCountCondition.cs", "Deserialize", 35);
			}
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
					XmlAttribute xmlAttribute2 = attributes2["max"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null)
			{
				this._max = int.MaxValue;
				return;
			}
			if (!int.TryParse(text4, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\OwnedFlagCountCondition.cs", "Deserialize", 45);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000B44C File Offset: 0x0000964C
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B460 File Offset: 0x00009660
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				int num = 0;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated && gameModeInstance.GetFlagOwnerTeam(flagCapturePoint) == agent.Team)
					{
						num++;
					}
				}
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000D1 RID: 209
		protected static string StringType = "FlagDominationOwnedFlagCount";

		// Token: 0x040000D2 RID: 210
		private int _min;

		// Token: 0x040000D3 RID: 211
		private int _max;
	}
}
