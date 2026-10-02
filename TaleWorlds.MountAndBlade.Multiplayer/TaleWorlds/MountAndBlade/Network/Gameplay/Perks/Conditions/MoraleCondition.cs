using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004C RID: 76
	public class MoraleCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000B068 File Offset: 0x00009268
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.MoraleChange;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000B06B File Offset: 0x0000926B
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B06E File Offset: 0x0000926E
		protected MoraleCondition()
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B078 File Offset: 0x00009278
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
				this._min = -1f;
			}
			else if (!float.TryParse(text2, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MoraleCondition.cs", "Deserialize", 35);
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
				this._max = 1f;
				return;
			}
			if (!float.TryParse(text4, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MoraleCondition.cs", "Deserialize", 45);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B140 File Offset: 0x00009340
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B154 File Offset: 0x00009354
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			Team team = ((agent != null) ? agent.Team : null);
			if (team != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				float num = ((team.Side == BattleSideEnum.Attacker) ? gameModeInstance.MoraleRounded : (-gameModeInstance.MoraleRounded));
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000CA RID: 202
		protected static string StringType = "FlagDominationMorale";

		// Token: 0x040000CB RID: 203
		private float _min;

		// Token: 0x040000CC RID: 204
		private float _max;
	}
}
