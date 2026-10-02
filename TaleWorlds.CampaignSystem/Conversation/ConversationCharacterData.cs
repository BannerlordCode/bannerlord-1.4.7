using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000236 RID: 566
	public struct ConversationCharacterData : ISerializableObject
	{
		// Token: 0x0600224E RID: 8782 RVA: 0x0009842C File Offset: 0x0009662C
		public ConversationCharacterData(CharacterObject character, PartyBase party = null, bool noHorse = false, bool noWeapon = false, bool spawnAfterFight = false, bool isCivilianEquipmentRequiredForLeader = false, bool isCivilianEquipmentRequiredForBodyGuardCharacters = false, bool noBodyguards = false)
		{
			this.Character = character;
			this.Party = party;
			this.NoHorse = noHorse;
			this.NoWeapon = noWeapon;
			this.NoBodyguards = noBodyguards;
			this.SpawnedAfterFight = spawnAfterFight;
			this.IsCivilianEquipmentRequiredForLeader = isCivilianEquipmentRequiredForLeader;
			this.IsCivilianEquipmentRequiredForBodyGuardCharacters = isCivilianEquipmentRequiredForBodyGuardCharacters;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0009846C File Offset: 0x0009666C
		void ISerializableObject.DeserializeFrom(IReader reader)
		{
			MBGUID mbguid = new MBGUID(reader.ReadUInt());
			this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(mbguid);
			int num = reader.ReadInt();
			this.Party = ConversationCharacterData.FindParty(num);
			this.NoHorse = reader.ReadBool();
			this.NoWeapon = reader.ReadBool();
			this.SpawnedAfterFight = reader.ReadBool();
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000984D4 File Offset: 0x000966D4
		void ISerializableObject.SerializeTo(IWriter writer)
		{
			writer.WriteUInt(this.Character.Id.InternalValue);
			writer.WriteInt((this.Party == null) ? (-1) : this.Party.Index);
			writer.WriteBool(this.NoHorse);
			writer.WriteBool(this.NoWeapon);
			writer.WriteBool(this.SpawnedAfterFight);
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0009853C File Offset: 0x0009673C
		private static PartyBase FindParty(int index)
		{
			MobileParty mobileParty = Campaign.Current.CampaignObjectManager.FindFirst<MobileParty>((MobileParty x) => x.Party.Index == index);
			if (mobileParty != null)
			{
				return mobileParty.Party;
			}
			Settlement settlement = Settlement.All.FirstOrDefaultQ<Settlement>((Settlement x) => x.Party.Index == index);
			if (settlement != null)
			{
				return settlement.Party;
			}
			return null;
		}

		// Token: 0x04000A1A RID: 2586
		public CharacterObject Character;

		// Token: 0x04000A1B RID: 2587
		public PartyBase Party;

		// Token: 0x04000A1C RID: 2588
		public bool NoHorse;

		// Token: 0x04000A1D RID: 2589
		public bool NoWeapon;

		// Token: 0x04000A1E RID: 2590
		public bool NoBodyguards;

		// Token: 0x04000A1F RID: 2591
		public bool SpawnedAfterFight;

		// Token: 0x04000A20 RID: 2592
		public bool IsCivilianEquipmentRequiredForLeader;

		// Token: 0x04000A21 RID: 2593
		public bool IsCivilianEquipmentRequiredForBodyGuardCharacters;
	}
}
