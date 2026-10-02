using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TroopSuppliers
{
	// Token: 0x020000B0 RID: 176
	public class PartyGroupTroopSupplier : IMissionTroopSupplier
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x0005AE1D File Offset: 0x0005901D
		// (set) Token: 0x06001385 RID: 4997 RVA: 0x0005AE25 File Offset: 0x00059025
		internal MapEventSide PartyGroup { get; private set; }

		// Token: 0x06001386 RID: 4998 RVA: 0x0005AE30 File Offset: 0x00059030
		public PartyGroupTroopSupplier(MapEvent mapEvent, BattleSideEnum side, FlattenedTroopRoster priorTroops = null, Func<UniqueTroopDescriptor, MapEventParty, bool> customAllocationConditions = null)
		{
			this._customAllocationConditions = customAllocationConditions;
			this.PartyGroup = mapEvent.GetMapEventSide(side);
			this._isPlayerSide = mapEvent.PlayerSide == side;
			this._initialTroopCount = this.PartyGroup.TroopCount;
			this.PartyGroup.MakeReadyForMission(priorTroops);
			this._nextTroopRank = 0;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005AE94 File Offset: 0x00059094
		public IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate)
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.AllocateTroops(ref list, numberToAllocate, this._customAllocationConditions);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			this._numAllocated += list.Count;
			for (int i = 0; i < array.Length; i++)
			{
				PartyGroupAgentOrigin[] array2 = array;
				int num = i;
				UniqueTroopDescriptor uniqueTroopDescriptor = list[i];
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				array2[num] = new PartyGroupAgentOrigin(this, uniqueTroopDescriptor, nextTroopRank);
			}
			if (array.Length < numberToAllocate)
			{
				this._anyTroopRemainsToBeSupplied = false;
			}
			return array;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0005AF14 File Offset: 0x00059114
		public IAgentOriginBase SupplyOneTroop()
		{
			UniqueTroopDescriptor uniqueTroopDescriptor;
			if (this.PartyGroup.AllocateTroop(this._customAllocationConditions, out uniqueTroopDescriptor))
			{
				UniqueTroopDescriptor uniqueTroopDescriptor2 = uniqueTroopDescriptor;
				int nextTroopRank = this._nextTroopRank;
				this._nextTroopRank = nextTroopRank + 1;
				IAgentOriginBase agentOriginBase = new PartyGroupAgentOrigin(this, uniqueTroopDescriptor2, nextTroopRank);
				this._anyTroopRemainsToBeSupplied = this._anyTroopRemainsToBeSupplied && this.PartyGroup.HasReadyTroops;
				return agentOriginBase;
			}
			this._anyTroopRemainsToBeSupplied = false;
			return null;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005AF74 File Offset: 0x00059174
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.GetAllTroops(ref list);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PartyGroupAgentOrigin(this, list[i], i);
			}
			return array;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005AFBC File Offset: 0x000591BC
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this.PartyGroup.LeaderParty.General;
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0005AFCE File Offset: 0x000591CE
		public int NumRemovedTroops
		{
			get
			{
				return this._numWounded + this._numKilled + this._numRouted;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x0005AFE4 File Offset: 0x000591E4
		public int NumTroopsNotSupplied
		{
			get
			{
				return this._initialTroopCount - this._numAllocated;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0005AFF3 File Offset: 0x000591F3
		public bool AnyTroopRemainsToBeSupplied
		{
			get
			{
				return this._anyTroopRemainsToBeSupplied;
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0005AFFC File Offset: 0x000591FC
		public int GetNumberOfPlayerControllableTroops()
		{
			int num = 0;
			foreach (MapEventParty mapEventParty in this.PartyGroup.Parties)
			{
				PartyBase party = mapEventParty.Party;
				if (PartyBase.IsPartyUnderPlayerCommand(party) || (party.Side == PartyBase.MainParty.Side && this.PartyGroup.MapEvent.IsPlayerSergeant()))
				{
					num += party.NumberOfHealthyMembers;
				}
			}
			return num;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0005B08C File Offset: 0x0005928C
		public void OnTroopWounded(UniqueTroopDescriptor troopDescriptor)
		{
			this._numWounded++;
			this.PartyGroup.OnTroopWounded(troopDescriptor);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0005B0A8 File Offset: 0x000592A8
		public void OnTroopKilled(UniqueTroopDescriptor troopDescriptor)
		{
			this._numKilled++;
			this.PartyGroup.OnTroopKilled(troopDescriptor);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0005B0C4 File Offset: 0x000592C4
		public void OnTroopRouted(UniqueTroopDescriptor troopDescriptor, bool isOrderRetreat)
		{
			this._numRouted++;
			this.PartyGroup.OnTroopRouted(troopDescriptor, isOrderRetreat);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0005B0E1 File Offset: 0x000592E1
		internal CharacterObject GetTroop(UniqueTroopDescriptor troopDescriptor)
		{
			return this.PartyGroup.GetAllocatedTroop(troopDescriptor) ?? this.PartyGroup.GetReadyTroop(troopDescriptor);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0005B100 File Offset: 0x00059300
		public PartyBase GetParty(UniqueTroopDescriptor troopDescriptor)
		{
			PartyBase partyBase = this.PartyGroup.GetAllocatedTroopParty(troopDescriptor);
			if (partyBase == null)
			{
				partyBase = this.PartyGroup.GetReadyTroopParty(troopDescriptor);
			}
			return partyBase;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0005B12B File Offset: 0x0005932B
		public void OnTroopScoreHit(UniqueTroopDescriptor descriptor, BasicCharacterObject attackedCharacter, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			this.PartyGroup.OnTroopScoreHit(descriptor, (CharacterObject)attackedCharacter, damage, isFatal, isTeamKill, attackerWeapon, false);
		}

		// Token: 0x04000666 RID: 1638
		private readonly int _initialTroopCount;

		// Token: 0x04000667 RID: 1639
		private int _numAllocated;

		// Token: 0x04000668 RID: 1640
		private int _numWounded;

		// Token: 0x04000669 RID: 1641
		private int _numKilled;

		// Token: 0x0400066A RID: 1642
		private int _numRouted;

		// Token: 0x0400066B RID: 1643
		private bool _isPlayerSide;

		// Token: 0x0400066C RID: 1644
		private Func<UniqueTroopDescriptor, MapEventParty, bool> _customAllocationConditions;

		// Token: 0x0400066D RID: 1645
		private bool _anyTroopRemainsToBeSupplied = true;

		// Token: 0x0400066E RID: 1646
		private int _nextTroopRank;
	}
}
