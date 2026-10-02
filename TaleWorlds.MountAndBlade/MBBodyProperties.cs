using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C0 RID: 448
	public static class MBBodyProperties
	{
		// Token: 0x0600190B RID: 6411 RVA: 0x00052E0B File Offset: 0x0005100B
		public static int GetNumEditableDeformKeys(int race, bool initialGender, int age)
		{
			return MBAPI.IMBFaceGen.GetNumEditableDeformKeys(race, initialGender, (float)age);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00052E1B File Offset: 0x0005101B
		public static void GetParamsFromKey(ref FaceGenerationParams faceGenerationParams, BodyProperties bodyProperties, bool earsAreHidden, bool mouthHidden)
		{
			MBAPI.IMBFaceGen.GetParamsFromKey(ref faceGenerationParams, ref bodyProperties, earsAreHidden, mouthHidden);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00052E2C File Offset: 0x0005102C
		public static void GetParamsMax(int race, int curGender, int curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale)
		{
			MBAPI.IMBFaceGen.GetParamsMax(race, curGender, (float)curAge, ref hairNum, ref beardNum, ref faceTextureNum, ref mouthTextureNum, ref faceTattooNum, ref soundNum, ref eyebrowNum, ref scale);
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00052E56 File Offset: 0x00051056
		public static void GetZeroProbabilities(int race, int curGender, float curAge, ref float tattooZeroProbability)
		{
			MBAPI.IMBFaceGen.GetZeroProbabilities(race, curGender, curAge, ref tattooZeroProbability);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00052E66 File Offset: 0x00051066
		public static void ProduceNumericKeyWithParams(FaceGenerationParams faceGenerationParams, bool earsAreHidden, bool mouthIsHidden, ref BodyProperties bodyProperties)
		{
			MBAPI.IMBFaceGen.ProduceNumericKeyWithParams(ref faceGenerationParams, earsAreHidden, mouthIsHidden, ref bodyProperties);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00052E77 File Offset: 0x00051077
		public static void TransformFaceKeysToDefaultFace(ref FaceGenerationParams faceGenerationParams)
		{
			MBAPI.IMBFaceGen.TransformFaceKeysToDefaultFace(ref faceGenerationParams);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00052E84 File Offset: 0x00051084
		public static void ProduceNumericKeyWithDefaultValues(ref BodyProperties initialBodyProperties, bool earsAreHidden, bool mouthIsHidden, int race, int gender, int age)
		{
			MBAPI.IMBFaceGen.ProduceNumericKeyWithDefaultValues(ref initialBodyProperties, earsAreHidden, mouthIsHidden, race, gender, (float)age);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00052E9C File Offset: 0x0005109C
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount)
		{
			BodyProperties bodyProperties = default(BodyProperties);
			MBAPI.IMBFaceGen.GetRandomBodyProperties(race, isFemale ? 1 : 0, ref bodyPropertiesMin, ref bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tatooTags, variationAmount, ref bodyProperties);
			return bodyProperties;
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00052ED8 File Offset: 0x000510D8
		public static DeformKeyData GetDeformKeyData(int keyNo, int race, int gender, int age)
		{
			DeformKeyData deformKeyData = default(DeformKeyData);
			MBAPI.IMBFaceGen.GetDeformKeyData(keyNo, ref deformKeyData, race, gender, (float)age);
			return deformKeyData;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00052EFF File Offset: 0x000510FF
		public static int GetFaceGenInstancesLength(int race, int gender, int age)
		{
			return MBAPI.IMBFaceGen.GetFaceGenInstancesLength(race, gender, (float)age);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00052F0F File Offset: 0x0005110F
		public static bool EnforceConstraints(ref FaceGenerationParams faceGenerationParams)
		{
			return MBAPI.IMBFaceGen.EnforceConstraints(ref faceGenerationParams);
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00052F1C File Offset: 0x0005111C
		public static float GetScaleFromKey(int race, int gender, BodyProperties bodyProperties)
		{
			return MBAPI.IMBFaceGen.GetScaleFromKey(race, gender, ref bodyProperties);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00052F2C File Offset: 0x0005112C
		public static int GetHairColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetHairColorCount(race, curGender, (float)age);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x00052F3C File Offset: 0x0005113C
		public static List<uint> GetHairColorGradientPoints(int race, int curGender, int age)
		{
			int hairColorCount = MBBodyProperties.GetHairColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[hairColorCount];
			MBAPI.IMBFaceGen.GetHairColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x00052FAB File Offset: 0x000511AB
		public static int GetTatooColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetTatooColorCount(race, curGender, (float)age);
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x00052FBC File Offset: 0x000511BC
		public static List<uint> GetTatooColorGradientPoints(int race, int curGender, int age)
		{
			int tatooColorCount = MBBodyProperties.GetTatooColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[tatooColorCount];
			MBAPI.IMBFaceGen.GetTatooColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0005302B File Offset: 0x0005122B
		public static int GetSkinColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetSkinColorCount(race, curGender, (float)age);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0005303B File Offset: 0x0005123B
		public static BodyMeshMaturityType GetMaturityType(float age)
		{
			return (BodyMeshMaturityType)MBAPI.IMBFaceGen.GetMaturityType(age);
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x00053048 File Offset: 0x00051248
		public static void FlushFaceCache()
		{
			MBAPI.IMBFaceGen.FlushFaceCache();
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x00053054 File Offset: 0x00051254
		public static string[] GetRaceIds()
		{
			return MBAPI.IMBFaceGen.GetRaceIds().Split(new char[] { ';' });
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x00053070 File Offset: 0x00051270
		public static int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetHairIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000530D8 File Offset: 0x000512D8
		public static int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetFacialIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x00053140 File Offset: 0x00051340
		public static int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetTattooIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x000531A8 File Offset: 0x000513A8
		public static List<uint> GetSkinColorGradientPoints(int race, int curGender, int age)
		{
			int skinColorCount = MBBodyProperties.GetSkinColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[skinColorCount];
			MBAPI.IMBFaceGen.GetSkinColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x00053218 File Offset: 0x00051418
		public static List<bool> GetVoiceTypeUsableForPlayerData(int race, int curGender, float age, int voiceTypeCount)
		{
			bool[] array = new bool[voiceTypeCount];
			MBAPI.IMBFaceGen.GetVoiceTypeUsableForPlayerData(race, curGender, age, array);
			return new List<bool>(array);
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x00053240 File Offset: 0x00051440
		public static void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			if (hair > -1)
			{
				faceGenerationParams.CurrentHair = hair;
			}
			if (beard > -1)
			{
				faceGenerationParams.CurrentBeard = beard;
			}
			if (tattoo > -1)
			{
				faceGenerationParams.CurrentFaceTattoo = tattoo;
			}
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x00053290 File Offset: 0x00051490
		public static void SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x000532C4 File Offset: 0x000514C4
		public static void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x000532FC File Offset: 0x000514FC
		public static void GenerateParentKey(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			FaceGenerationParams faceGenerationParams2 = FaceGenerationParams.Create();
			FaceGenerationParams faceGenerationParams3 = FaceGenerationParams.Create();
			MBBodyProperties.GenerationType[] array = new MBBodyProperties.GenerationType[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (MBBodyProperties.GenerationType)MBRandom.RandomInt(2);
			}
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, childBodyProperties, false, false);
			int faceGenInstancesLength = MBBodyProperties.GetFaceGenInstancesLength(race, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
			for (int j = 0; j < faceGenInstancesLength; j++)
			{
				DeformKeyData deformKeyData = MBBodyProperties.GetDeformKeyData(j, race, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
				if (deformKeyData.GroupId >= 0 && deformKeyData.GroupId != 0 && deformKeyData.GroupId != 5 && deformKeyData.GroupId != 6)
				{
					float num = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.KeyWeights[j], 1f - faceGenerationParams.KeyWeights[j]);
					if (array[deformKeyData.GroupId - 1] == MBBodyProperties.GenerationType.FromMother)
					{
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j];
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
					}
					else if (array[deformKeyData.GroupId - 1] == MBBodyProperties.GenerationType.FromFather)
					{
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j];
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
					}
					else
					{
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j] - num;
					}
				}
			}
			faceGenerationParams2.CurrentAge = faceGenerationParams.CurrentAge + (float)MBRandom.RandomInt(18, 25);
			float num2;
			faceGenerationParams2.SetRandomParamsExceptKeys(race, 0, (int)faceGenerationParams2.CurrentAge, out num2);
			faceGenerationParams2.CurrentFaceTattoo = 0;
			faceGenerationParams3.CurrentAge = faceGenerationParams.CurrentAge + (float)MBRandom.RandomInt(18, 22);
			float num3;
			faceGenerationParams3.SetRandomParamsExceptKeys(race, 1, (int)faceGenerationParams3.CurrentAge, out num3);
			faceGenerationParams3.CurrentFaceTattoo = 0;
			faceGenerationParams3.HeightMultiplier = faceGenerationParams2.HeightMultiplier * MBRandom.RandomFloatRanged(0.7f, 0.9f);
			if (faceGenerationParams3.CurrentHair == 0)
			{
				faceGenerationParams3.CurrentHair = 1;
			}
			float num4 = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.CurrentSkinColorOffset, 1f - faceGenerationParams.CurrentSkinColorOffset);
			float num5 = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.CurrentHairColorOffset, 1f - faceGenerationParams.CurrentHairColorOffset);
			int num6 = MBRandom.RandomInt(2);
			if (num6 == 1)
			{
				faceGenerationParams2.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset + num4;
				faceGenerationParams3.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset - num4;
			}
			else
			{
				faceGenerationParams2.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset - num4;
				faceGenerationParams3.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset + num4;
			}
			if (num6 == 1)
			{
				faceGenerationParams2.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset + num5;
				faceGenerationParams3.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset - num5;
			}
			else
			{
				faceGenerationParams2.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset - num5;
				faceGenerationParams3.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset + num5;
			}
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams3, false, false, ref motherBodyProperties);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams2, false, false, ref fatherBodyProperties);
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x000535EC File Offset: 0x000517EC
		public static BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age)
		{
			FaceGenerationParams faceGenerationParams = default(FaceGenerationParams);
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			faceGenerationParams.CurrentAge = age;
			BodyProperties bodyProperties2 = default(BodyProperties);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties2);
			return bodyProperties2;
		}

		// Token: 0x020004E9 RID: 1257
		public enum GenerationType
		{
			// Token: 0x04001C4E RID: 7246
			FromMother,
			// Token: 0x04001C4F RID: 7247
			FromFather,
			// Token: 0x04001C50 RID: 7248
			Count
		}
	}
}
