using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021D RID: 541
	[EngineStruct("Face_generation_params", false, null)]
	public struct FaceGenerationParams
	{
		// Token: 0x06001F66 RID: 8038 RVA: 0x0006C9C8 File Offset: 0x0006ABC8
		public static FaceGenerationParams Create()
		{
			FaceGenerationParams faceGenerationParams;
			faceGenerationParams.Seed = 0;
			faceGenerationParams.CurrentBeard = 0;
			faceGenerationParams.CurrentHair = 0;
			faceGenerationParams.CurrentEyebrow = 0;
			faceGenerationParams.IsHairFlipped = false;
			faceGenerationParams.CurrentRace = 0;
			faceGenerationParams.CurrentGender = 0;
			faceGenerationParams.CurrentFaceTexture = 0;
			faceGenerationParams.CurrentMouthTexture = 0;
			faceGenerationParams.CurrentFaceTattoo = 0;
			faceGenerationParams.CurrentVoice = 0;
			faceGenerationParams.HairFilter = 0;
			faceGenerationParams.BeardFilter = 0;
			faceGenerationParams.TattooFilter = 0;
			faceGenerationParams.FaceTextureFilter = 0;
			faceGenerationParams.TattooZeroProbability = 0f;
			faceGenerationParams.KeyWeights = new float[320];
			faceGenerationParams.CurrentAge = 0f;
			faceGenerationParams.CurrentWeight = 0f;
			faceGenerationParams.CurrentBuild = 0f;
			faceGenerationParams.CurrentSkinColorOffset = 0f;
			faceGenerationParams.CurrentHairColorOffset = 0f;
			faceGenerationParams.CurrentEyeColorOffset = 0f;
			faceGenerationParams.FaceDirtAmount = 0f;
			faceGenerationParams.CurrentFaceTattooColorOffset1 = 0f;
			faceGenerationParams.HeightMultiplier = 0f;
			faceGenerationParams.VoicePitch = 0f;
			faceGenerationParams.UseCache = false;
			faceGenerationParams.UseGpuMorph = false;
			faceGenerationParams.Padding2 = false;
			faceGenerationParams.FaceCacheId = 0;
			return faceGenerationParams;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0006CB04 File Offset: 0x0006AD04
		public void SetRaceGenderAndAdjustParams(int race, int gender, int curAge)
		{
			this.CurrentGender = gender;
			this.CurrentRace = race;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			float num8 = 0f;
			MBBodyProperties.GetParamsMax(race, gender, curAge, ref num, ref num2, ref num3, ref num4, ref num7, ref num6, ref num5, ref num8);
			this.CurrentHair = MBMath.ClampInt(this.CurrentHair, 0, num - 1);
			this.CurrentBeard = MBMath.ClampInt(this.CurrentBeard, 0, num2 - 1);
			this.CurrentFaceTexture = MBMath.ClampInt(this.CurrentFaceTexture, 0, num3 - 1);
			this.CurrentMouthTexture = MBMath.ClampInt(this.CurrentMouthTexture, 0, num4 - 1);
			this.CurrentFaceTattoo = MBMath.ClampInt(this.CurrentFaceTattoo, 0, num7 - 1);
			this.CurrentVoice = MBMath.ClampInt(this.CurrentVoice, 0, num6 - 1);
			this.VoicePitch = MBMath.ClampFloat(this.VoicePitch, 0f, 1f);
			this.CurrentEyebrow = MBMath.ClampInt(this.CurrentEyebrow, 0, num5 - 1);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0006CC00 File Offset: 0x0006AE00
		public void SetRandomParamsExceptKeys(int race, int gender, int minAge, out float scale)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			scale = 0f;
			MBBodyProperties.GetParamsMax(race, gender, minAge, ref num, ref num2, ref num3, ref num4, ref num7, ref num6, ref num5, ref scale);
			this.CurrentHair = MBRandom.RandomInt(num);
			this.CurrentBeard = MBRandom.RandomInt(num2);
			this.CurrentFaceTexture = MBRandom.RandomInt(num3);
			this.CurrentMouthTexture = MBRandom.RandomInt(num4);
			this.CurrentFaceTattoo = MBRandom.RandomInt(num7);
			this.CurrentVoice = MBRandom.RandomInt(num6);
			this.VoicePitch = MBRandom.RandomFloat;
			this.CurrentEyebrow = MBRandom.RandomInt(num5);
			this.CurrentSkinColorOffset = MBRandom.RandomFloat;
			this.CurrentHairColorOffset = MBRandom.RandomFloat;
			this.CurrentEyeColorOffset = MBRandom.RandomFloat;
			this.CurrentFaceTattooColorOffset1 = MBRandom.RandomFloat;
			this.HeightMultiplier = MBRandom.RandomFloat;
		}

		// Token: 0x04000AB9 RID: 2745
		public int Seed;

		// Token: 0x04000ABA RID: 2746
		public int CurrentBeard;

		// Token: 0x04000ABB RID: 2747
		public int CurrentHair;

		// Token: 0x04000ABC RID: 2748
		public int CurrentEyebrow;

		// Token: 0x04000ABD RID: 2749
		public int CurrentRace;

		// Token: 0x04000ABE RID: 2750
		public int CurrentGender;

		// Token: 0x04000ABF RID: 2751
		public int CurrentFaceTexture;

		// Token: 0x04000AC0 RID: 2752
		public int CurrentMouthTexture;

		// Token: 0x04000AC1 RID: 2753
		public int CurrentFaceTattoo;

		// Token: 0x04000AC2 RID: 2754
		public int CurrentVoice;

		// Token: 0x04000AC3 RID: 2755
		public int HairFilter;

		// Token: 0x04000AC4 RID: 2756
		public int BeardFilter;

		// Token: 0x04000AC5 RID: 2757
		public int TattooFilter;

		// Token: 0x04000AC6 RID: 2758
		public int FaceTextureFilter;

		// Token: 0x04000AC7 RID: 2759
		public float TattooZeroProbability;

		// Token: 0x04000AC8 RID: 2760
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 320)]
		public float[] KeyWeights;

		// Token: 0x04000AC9 RID: 2761
		public float CurrentAge;

		// Token: 0x04000ACA RID: 2762
		public float CurrentWeight;

		// Token: 0x04000ACB RID: 2763
		public float CurrentBuild;

		// Token: 0x04000ACC RID: 2764
		public float CurrentSkinColorOffset;

		// Token: 0x04000ACD RID: 2765
		public float CurrentHairColorOffset;

		// Token: 0x04000ACE RID: 2766
		public float CurrentEyeColorOffset;

		// Token: 0x04000ACF RID: 2767
		public float FaceDirtAmount;

		// Token: 0x04000AD0 RID: 2768
		public float CurrentFaceTattooColorOffset1;

		// Token: 0x04000AD1 RID: 2769
		public float HeightMultiplier;

		// Token: 0x04000AD2 RID: 2770
		public float VoicePitch;

		// Token: 0x04000AD3 RID: 2771
		[MarshalAs(UnmanagedType.U1)]
		public bool IsHairFlipped;

		// Token: 0x04000AD4 RID: 2772
		[MarshalAs(UnmanagedType.U1)]
		public bool UseCache;

		// Token: 0x04000AD5 RID: 2773
		[MarshalAs(UnmanagedType.U1)]
		public bool UseGpuMorph;

		// Token: 0x04000AD6 RID: 2774
		[MarshalAs(UnmanagedType.U1)]
		public bool Padding2;

		// Token: 0x04000AD7 RID: 2775
		public int FaceCacheId;
	}
}
