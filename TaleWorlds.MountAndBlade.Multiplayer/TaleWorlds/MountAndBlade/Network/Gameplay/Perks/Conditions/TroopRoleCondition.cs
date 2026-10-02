using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000050 RID: 80
	public class TroopRoleCondition : MPPerkCondition
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000B70E File Offset: 0x0000990E
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.PeerControlledAgentChange;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000B712 File Offset: 0x00009912
		protected TroopRoleCondition()
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000B71C File Offset: 0x0000991C
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
					XmlAttribute xmlAttribute = attributes["role"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._role = TroopRoleCondition.Role.Sergeant;
			if (text2 != null && !Enum.TryParse<TroopRoleCondition.Role>(text2, true, out this._role))
			{
				this._role = TroopRoleCondition.Role.Sergeant;
				Debug.FailedAssert("provided 'role' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\TroopRoleCondition.cs", "Deserialize", 35);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B789 File Offset: 0x00009989
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null && MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				switch (this._role)
				{
				case TroopRoleCondition.Role.Sergeant:
					return this.IsAgentSergeant(agent);
				case TroopRoleCondition.Role.Troop:
					return !this.IsAgentBannerBearer(agent) && !this.IsAgentSergeant(agent);
				case TroopRoleCondition.Role.BannerBearer:
					return this.IsAgentBannerBearer(agent) && !this.IsAgentSergeant(agent);
				}
			}
			return false;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B821 File Offset: 0x00009A21
		private bool IsAgentSergeant(Agent agent)
		{
			return agent.Character == MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character).HeroCharacter;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B83C File Offset: 0x00009A3C
		private bool IsAgentBannerBearer(Agent agent)
		{
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			Formation formation = ((missionPeer != null) ? missionPeer.ControlledFormation : null);
			if (formation != null)
			{
				MissionWeapon missionWeapon = agent.Equipment[EquipmentIndex.ExtraWeaponSlot];
				if (!missionWeapon.IsEmpty && missionWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Banner && new Banner(formation.BannerCode, missionPeer.Team.Color, missionPeer.Team.Color2).Serialize() == missionWeapon.Banner.Serialize())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000D8 RID: 216
		protected static string StringType = "TroopRole";

		// Token: 0x040000D9 RID: 217
		private TroopRoleCondition.Role _role;

		// Token: 0x020000AE RID: 174
		private enum Role
		{
			// Token: 0x040001D4 RID: 468
			Sergeant,
			// Token: 0x040001D5 RID: 469
			Troop,
			// Token: 0x040001D6 RID: 470
			BannerBearer
		}
	}
}
