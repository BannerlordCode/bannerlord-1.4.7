using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000212 RID: 530
	public class NarrativeMenuCharacter
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x00090F44 File Offset: 0x0008F144
		// (set) Token: 0x06002028 RID: 8232 RVA: 0x00090F4C File Offset: 0x0008F14C
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x00090F55 File Offset: 0x0008F155
		// (set) Token: 0x0600202A RID: 8234 RVA: 0x00090F5D File Offset: 0x0008F15D
		public int Race { get; private set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00090F66 File Offset: 0x0008F166
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x00090F6E File Offset: 0x0008F16E
		public bool IsFemale { get; set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x00090F77 File Offset: 0x0008F177
		// (set) Token: 0x0600202E RID: 8238 RVA: 0x00090F7F File Offset: 0x0008F17F
		public MBEquipmentRoster Equipment { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x00090F88 File Offset: 0x0008F188
		// (set) Token: 0x06002030 RID: 8240 RVA: 0x00090F90 File Offset: 0x0008F190
		public string AnimationId { get; private set; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x00090F99 File Offset: 0x0008F199
		// (set) Token: 0x06002032 RID: 8242 RVA: 0x00090FA1 File Offset: 0x0008F1A1
		public MountCreationKey MountCreationKey { get; private set; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00090FAA File Offset: 0x0008F1AA
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x00090FB2 File Offset: 0x0008F1B2
		public string Item1Id { get; private set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x00090FBB File Offset: 0x0008F1BB
		// (set) Token: 0x06002036 RID: 8246 RVA: 0x00090FC3 File Offset: 0x0008F1C3
		public string Item2Id { get; private set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002037 RID: 8247 RVA: 0x00090FCC File Offset: 0x0008F1CC
		// (set) Token: 0x06002038 RID: 8248 RVA: 0x00090FD4 File Offset: 0x0008F1D4
		public EquipmentIndex RightHandEquipmentIndex { get; private set; }

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x00090FDD File Offset: 0x0008F1DD
		// (set) Token: 0x0600203A RID: 8250 RVA: 0x00090FE5 File Offset: 0x0008F1E5
		public EquipmentIndex LeftHandEquipmentIndex { get; private set; }

		// Token: 0x0600203B RID: 8251 RVA: 0x00090FF0 File Offset: 0x0008F1F0
		public NarrativeMenuCharacter(string stringId, BodyProperties bodyProperties, int race, bool isFemale)
		{
			this.StringId = stringId;
			this.BodyProperties = bodyProperties;
			this.Race = race;
			this.IsFemale = isFemale;
			this.IsHuman = true;
			this.SpawnPointEntityId = "spawnpoint_player_1";
			this.AnimationId = "act_inventory_idle_start";
			this.Equipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00091057 File Offset: 0x0008F257
		public NarrativeMenuCharacter(string stringId)
		{
			this.StringId = stringId;
			this.IsHuman = false;
			this.SpawnPointEntityId = "spawnpoint_mount_1";
			this.AnimationId = "act_inventory_idle_start";
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00091083 File Offset: 0x0008F283
		public void UpdateBodyProperties(BodyProperties bodyProperties, int race, bool isFemale)
		{
			this.BodyProperties = bodyProperties;
			this.Race = race;
			this.IsFemale = isFemale;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0009109A File Offset: 0x0008F29A
		public void SetEquipment(MBEquipmentRoster equipment)
		{
			this.Equipment = equipment;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000910A3 File Offset: 0x0008F2A3
		public void SetAnimationId(string animationId)
		{
			this.AnimationId = animationId;
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000910AC File Offset: 0x0008F2AC
		public void SetRightHandItem(string itemId)
		{
			this.Item1Id = itemId;
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000910B5 File Offset: 0x0008F2B5
		public void SetLeftHandItem(string itemId)
		{
			this.Item2Id = itemId;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000910BE File Offset: 0x0008F2BE
		public void EquipRightHandItemWithEquipmentIndex(EquipmentIndex item)
		{
			this.RightHandEquipmentIndex = item;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000910C7 File Offset: 0x0008F2C7
		public void EquipLeftHandItemWithEquipmentIndex(EquipmentIndex item)
		{
			this.LeftHandEquipmentIndex = item;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000910D0 File Offset: 0x0008F2D0
		public void SetSpawnPointEntityId(string spawnPointEntityId)
		{
			this.SpawnPointEntityId = spawnPointEntityId;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000910DC File Offset: 0x0008F2DC
		public void ChangeAge(float age)
		{
			BodyProperties bodyProperties = this.BodyProperties;
			this.BodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000910FE File Offset: 0x0008F2FE
		public void SetMountCreationKey(MountCreationKey mountCreationKey)
		{
			this.MountCreationKey = mountCreationKey;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00091107 File Offset: 0x0008F307
		public void SetHorseItemId(string itemId)
		{
			this.Item1Id = itemId;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00091110 File Offset: 0x0008F310
		public void SetHarnessItemId(string itemId)
		{
			this.Item2Id = itemId;
		}

		// Token: 0x04000974 RID: 2420
		public readonly string StringId;

		// Token: 0x04000975 RID: 2421
		public readonly bool IsHuman;

		// Token: 0x04000976 RID: 2422
		public string SpawnPointEntityId;
	}
}
