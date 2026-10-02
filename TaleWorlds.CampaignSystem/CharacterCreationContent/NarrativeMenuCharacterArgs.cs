using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000213 RID: 531
	public readonly struct NarrativeMenuCharacterArgs
	{
		// Token: 0x06002049 RID: 8265 RVA: 0x0009111C File Offset: 0x0008F31C
		public NarrativeMenuCharacterArgs(string characterId, int age, string equipmentId, string animationId, string spawnPointEntityId, string leftHandItemId = "", string rightHandItemId = "", MountCreationKey mountCreationKey = null, bool isHuman = true, bool isFemale = false)
		{
			this.CharacterId = characterId;
			this.Age = age;
			this.EquipmentId = equipmentId;
			this.AnimationId = animationId;
			this.SpawnPointEntityId = spawnPointEntityId;
			this.LeftHandItemId = leftHandItemId;
			this.RightHandItemId = rightHandItemId;
			this.MountCreationKey = mountCreationKey;
			this.IsHuman = isHuman;
			this.IsFemale = isFemale;
		}

		// Token: 0x04000981 RID: 2433
		public readonly string CharacterId;

		// Token: 0x04000982 RID: 2434
		public readonly int Age;

		// Token: 0x04000983 RID: 2435
		public readonly string EquipmentId;

		// Token: 0x04000984 RID: 2436
		public readonly string AnimationId;

		// Token: 0x04000985 RID: 2437
		public readonly string SpawnPointEntityId;

		// Token: 0x04000986 RID: 2438
		public readonly string LeftHandItemId;

		// Token: 0x04000987 RID: 2439
		public readonly string RightHandItemId;

		// Token: 0x04000988 RID: 2440
		public readonly MountCreationKey MountCreationKey;

		// Token: 0x04000989 RID: 2441
		public readonly bool IsHuman;

		// Token: 0x0400098A RID: 2442
		public readonly bool IsFemale;
	}
}
