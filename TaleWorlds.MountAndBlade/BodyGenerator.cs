using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E9 RID: 489
	public class BodyGenerator
	{
		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x00061880 File Offset: 0x0005FA80
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x00061888 File Offset: 0x0005FA88
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x06001C7E RID: 7294 RVA: 0x00061894 File Offset: 0x0005FA94
		public BodyGenerator(BasicCharacterObject troop)
		{
			this.Character = troop;
			MBDebug.Print("FaceGen set character> character face key: " + troop.GetBodyProperties(troop.Equipment, -1), 0, Debug.DebugColor.White, 17592186044416UL);
			this.Race = this.Character.Race;
			this.IsFemale = this.Character.IsFemale;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00061900 File Offset: 0x0005FB00
		public FaceGenerationParams InitBodyGenerator(bool isDressed)
		{
			this.CurrentBodyProperties = this.Character.GetBodyProperties(this.Character.Equipment, -1);
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			faceGenerationParams.CurrentRace = this.Character.Race;
			faceGenerationParams.CurrentGender = (this.Character.IsFemale ? 1 : 0);
			faceGenerationParams.CurrentAge = this.Character.Age;
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, this.CurrentBodyProperties, isDressed && this.Character.Equipment.EarsAreHidden, isDressed && this.Character.Equipment.MouthIsHidden);
			faceGenerationParams.SetRaceGenderAndAdjustParams(faceGenerationParams.CurrentRace, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
			return faceGenerationParams;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000619C0 File Offset: 0x0005FBC0
		public void RefreshFace(FaceGenerationParams faceGenerationParams, bool hasEquipment)
		{
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, hasEquipment && this.Character.Equipment.EarsAreHidden, hasEquipment && this.Character.Equipment.MouthIsHidden, ref this.CurrentBodyProperties);
			this.Race = faceGenerationParams.CurrentRace;
			this.IsFemale = faceGenerationParams.CurrentGender == 1;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00061A20 File Offset: 0x0005FC20
		public void SaveCurrentCharacter()
		{
			this.Character.UpdatePlayerCharacterBodyProperties(this.CurrentBodyProperties, this.Race, this.IsFemale);
		}

		// Token: 0x040009B6 RID: 2486
		public const string FaceGenTeethAnimationName = "facegen_teeth";

		// Token: 0x040009B7 RID: 2487
		public BodyProperties CurrentBodyProperties;

		// Token: 0x040009B8 RID: 2488
		public BodyProperties BodyPropertiesMin;

		// Token: 0x040009B9 RID: 2489
		public BodyProperties BodyPropertiesMax;

		// Token: 0x040009BA RID: 2490
		public int Race;

		// Token: 0x040009BB RID: 2491
		public bool IsFemale;
	}
}
