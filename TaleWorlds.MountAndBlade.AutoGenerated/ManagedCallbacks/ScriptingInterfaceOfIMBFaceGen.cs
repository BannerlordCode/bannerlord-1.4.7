using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000013 RID: 19
	internal class ScriptingInterfaceOfIMBFaceGen : IMBFaceGen
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0000B516 File Offset: 0x00009716
		public bool EnforceConstraints(ref FaceGenerationParams faceGenerationParams)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_EnforceConstraintsDelegate(ref faceGenerationParams);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B523 File Offset: 0x00009723
		public void FlushFaceCache()
		{
			ScriptingInterfaceOfIMBFaceGen.call_FlushFaceCacheDelegate();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000B52F File Offset: 0x0000972F
		public void GetDeformKeyData(int keyNo, ref DeformKeyData deformKeyData, int race, int gender, float age)
		{
			ScriptingInterfaceOfIMBFaceGen.call_GetDeformKeyDataDelegate(keyNo, ref deformKeyData, race, gender, age);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000B542 File Offset: 0x00009742
		public int GetFaceGenInstancesLength(int race, int gender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetFaceGenInstancesLengthDelegate(race, gender, age);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B554 File Offset: 0x00009754
		public string GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIMBFaceGen.call_GetFacialIndicesByTagDelegate(race, curGender, age, array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B5BF File Offset: 0x000097BF
		public int GetHairColorCount(int race, int curGender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetHairColorCountDelegate(race, curGender, age);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B5D0 File Offset: 0x000097D0
		public void GetHairColorGradientPoints(int race, int curGender, float age, Vec3[] colors)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(colors, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBFaceGen.call_GetHairColorGradientPointsDelegate(race, curGender, age, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B604 File Offset: 0x00009804
		public string GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIMBFaceGen.call_GetHairIndicesByTagDelegate(race, curGender, age, array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B66F File Offset: 0x0000986F
		public int GetMaturityType(float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetMaturityTypeDelegate(age);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B67C File Offset: 0x0000987C
		public int GetNumEditableDeformKeys(int race, bool initialGender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetNumEditableDeformKeysDelegate(race, initialGender, age);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B68B File Offset: 0x0000988B
		public void GetParamsFromKey(ref FaceGenerationParams faceGenerationParams, ref BodyProperties bodyProperties, bool earsAreHidden, bool mouthHidden)
		{
			ScriptingInterfaceOfIMBFaceGen.call_GetParamsFromKeyDelegate(ref faceGenerationParams, ref bodyProperties, earsAreHidden, mouthHidden);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B69C File Offset: 0x0000989C
		public void GetParamsMax(int race, int curGender, float curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale)
		{
			ScriptingInterfaceOfIMBFaceGen.call_GetParamsMaxDelegate(race, curGender, curAge, ref hairNum, ref beardNum, ref faceTextureNum, ref mouthTextureNum, ref faceTattooNum, ref soundNum, ref eyebrowNum, ref scale);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B6C6 File Offset: 0x000098C6
		public string GetRaceIds()
		{
			if (ScriptingInterfaceOfIMBFaceGen.call_GetRaceIdsDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000B6DC File Offset: 0x000098DC
		public void GetRandomBodyProperties(int race, int gender, ref BodyProperties bodyPropertiesMin, ref BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount, ref BodyProperties outBodyProperties)
		{
			byte[] array = null;
			if (hairTags != null)
			{
				int byteCount = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(hairTags);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(hairTags, 0, hairTags.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (beardTags != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(beardTags);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(beardTags, 0, beardTags.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (tatooTags != null)
			{
				int byteCount3 = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(tatooTags);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(tatooTags, 0, tatooTags.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			ScriptingInterfaceOfIMBFaceGen.call_GetRandomBodyPropertiesDelegate(race, gender, ref bodyPropertiesMin, ref bodyPropertiesMax, hairCoverType, seed, array, array2, array3, variationAmount, ref outBodyProperties);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B7DD File Offset: 0x000099DD
		public float GetScaleFromKey(int race, int gender, ref BodyProperties initialBodyProperties)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetScaleFromKeyDelegate(race, gender, ref initialBodyProperties);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B7EC File Offset: 0x000099EC
		public int GetSkinColorCount(int race, int curGender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetSkinColorCountDelegate(race, curGender, age);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B7FC File Offset: 0x000099FC
		public void GetSkinColorGradientPoints(int race, int curGender, float age, Vec3[] colors)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(colors, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBFaceGen.call_GetSkinColorGradientPointsDelegate(race, curGender, age, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B830 File Offset: 0x00009A30
		public int GetTatooColorCount(int race, int curGender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetTatooColorCountDelegate(race, curGender, age);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000B840 File Offset: 0x00009A40
		public void GetTatooColorGradientPoints(int race, int curGender, float age, Vec3[] colors)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(colors, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBFaceGen.call_GetTatooColorGradientPointsDelegate(race, curGender, age, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000B874 File Offset: 0x00009A74
		public string GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMBFaceGen._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBFaceGen._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIMBFaceGen.call_GetTattooIndicesByTagDelegate(race, curGender, age, array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B8DF File Offset: 0x00009ADF
		public int GetVoiceRecordsCount(int race, int curGender, float age)
		{
			return ScriptingInterfaceOfIMBFaceGen.call_GetVoiceRecordsCountDelegate(race, curGender, age);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		public void GetVoiceTypeUsableForPlayerData(int race, int curGender, float age, bool[] aiArray)
		{
			PinnedArrayData<bool> pinnedArrayData = new PinnedArrayData<bool>(aiArray, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBFaceGen.call_GetVoiceTypeUsableForPlayerDataDelegate(race, curGender, age, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000B924 File Offset: 0x00009B24
		public void GetZeroProbabilities(int race, int curGender, float curAge, ref float tattooZeroProbability)
		{
			ScriptingInterfaceOfIMBFaceGen.call_GetZeroProbabilitiesDelegate(race, curGender, curAge, ref tattooZeroProbability);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B935 File Offset: 0x00009B35
		public void ProduceNumericKeyWithDefaultValues(ref BodyProperties initialBodyProperties, bool earsAreHidden, bool mouthIsHidden, int race, int gender, float age)
		{
			ScriptingInterfaceOfIMBFaceGen.call_ProduceNumericKeyWithDefaultValuesDelegate(ref initialBodyProperties, earsAreHidden, mouthIsHidden, race, gender, age);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B94A File Offset: 0x00009B4A
		public void ProduceNumericKeyWithParams(ref FaceGenerationParams faceGenerationParams, bool earsAreHidden, bool mouthIsHidden, ref BodyProperties bodyProperties)
		{
			ScriptingInterfaceOfIMBFaceGen.call_ProduceNumericKeyWithParamsDelegate(ref faceGenerationParams, earsAreHidden, mouthIsHidden, ref bodyProperties);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B95B File Offset: 0x00009B5B
		public void TransformFaceKeysToDefaultFace(ref FaceGenerationParams faceGenerationParams)
		{
			ScriptingInterfaceOfIMBFaceGen.call_TransformFaceKeysToDefaultFaceDelegate(ref faceGenerationParams);
		}

		// Token: 0x040001B4 RID: 436
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001B5 RID: 437
		public static ScriptingInterfaceOfIMBFaceGen.EnforceConstraintsDelegate call_EnforceConstraintsDelegate;

		// Token: 0x040001B6 RID: 438
		public static ScriptingInterfaceOfIMBFaceGen.FlushFaceCacheDelegate call_FlushFaceCacheDelegate;

		// Token: 0x040001B7 RID: 439
		public static ScriptingInterfaceOfIMBFaceGen.GetDeformKeyDataDelegate call_GetDeformKeyDataDelegate;

		// Token: 0x040001B8 RID: 440
		public static ScriptingInterfaceOfIMBFaceGen.GetFaceGenInstancesLengthDelegate call_GetFaceGenInstancesLengthDelegate;

		// Token: 0x040001B9 RID: 441
		public static ScriptingInterfaceOfIMBFaceGen.GetFacialIndicesByTagDelegate call_GetFacialIndicesByTagDelegate;

		// Token: 0x040001BA RID: 442
		public static ScriptingInterfaceOfIMBFaceGen.GetHairColorCountDelegate call_GetHairColorCountDelegate;

		// Token: 0x040001BB RID: 443
		public static ScriptingInterfaceOfIMBFaceGen.GetHairColorGradientPointsDelegate call_GetHairColorGradientPointsDelegate;

		// Token: 0x040001BC RID: 444
		public static ScriptingInterfaceOfIMBFaceGen.GetHairIndicesByTagDelegate call_GetHairIndicesByTagDelegate;

		// Token: 0x040001BD RID: 445
		public static ScriptingInterfaceOfIMBFaceGen.GetMaturityTypeDelegate call_GetMaturityTypeDelegate;

		// Token: 0x040001BE RID: 446
		public static ScriptingInterfaceOfIMBFaceGen.GetNumEditableDeformKeysDelegate call_GetNumEditableDeformKeysDelegate;

		// Token: 0x040001BF RID: 447
		public static ScriptingInterfaceOfIMBFaceGen.GetParamsFromKeyDelegate call_GetParamsFromKeyDelegate;

		// Token: 0x040001C0 RID: 448
		public static ScriptingInterfaceOfIMBFaceGen.GetParamsMaxDelegate call_GetParamsMaxDelegate;

		// Token: 0x040001C1 RID: 449
		public static ScriptingInterfaceOfIMBFaceGen.GetRaceIdsDelegate call_GetRaceIdsDelegate;

		// Token: 0x040001C2 RID: 450
		public static ScriptingInterfaceOfIMBFaceGen.GetRandomBodyPropertiesDelegate call_GetRandomBodyPropertiesDelegate;

		// Token: 0x040001C3 RID: 451
		public static ScriptingInterfaceOfIMBFaceGen.GetScaleFromKeyDelegate call_GetScaleFromKeyDelegate;

		// Token: 0x040001C4 RID: 452
		public static ScriptingInterfaceOfIMBFaceGen.GetSkinColorCountDelegate call_GetSkinColorCountDelegate;

		// Token: 0x040001C5 RID: 453
		public static ScriptingInterfaceOfIMBFaceGen.GetSkinColorGradientPointsDelegate call_GetSkinColorGradientPointsDelegate;

		// Token: 0x040001C6 RID: 454
		public static ScriptingInterfaceOfIMBFaceGen.GetTatooColorCountDelegate call_GetTatooColorCountDelegate;

		// Token: 0x040001C7 RID: 455
		public static ScriptingInterfaceOfIMBFaceGen.GetTatooColorGradientPointsDelegate call_GetTatooColorGradientPointsDelegate;

		// Token: 0x040001C8 RID: 456
		public static ScriptingInterfaceOfIMBFaceGen.GetTattooIndicesByTagDelegate call_GetTattooIndicesByTagDelegate;

		// Token: 0x040001C9 RID: 457
		public static ScriptingInterfaceOfIMBFaceGen.GetVoiceRecordsCountDelegate call_GetVoiceRecordsCountDelegate;

		// Token: 0x040001CA RID: 458
		public static ScriptingInterfaceOfIMBFaceGen.GetVoiceTypeUsableForPlayerDataDelegate call_GetVoiceTypeUsableForPlayerDataDelegate;

		// Token: 0x040001CB RID: 459
		public static ScriptingInterfaceOfIMBFaceGen.GetZeroProbabilitiesDelegate call_GetZeroProbabilitiesDelegate;

		// Token: 0x040001CC RID: 460
		public static ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithDefaultValuesDelegate call_ProduceNumericKeyWithDefaultValuesDelegate;

		// Token: 0x040001CD RID: 461
		public static ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithParamsDelegate call_ProduceNumericKeyWithParamsDelegate;

		// Token: 0x040001CE RID: 462
		public static ScriptingInterfaceOfIMBFaceGen.TransformFaceKeysToDefaultFaceDelegate call_TransformFaceKeysToDefaultFaceDelegate;

		// Token: 0x0200021B RID: 539
		// (Invoke) Token: 0x06000B5A RID: 2906
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool EnforceConstraintsDelegate(ref FaceGenerationParams faceGenerationParams);

		// Token: 0x0200021C RID: 540
		// (Invoke) Token: 0x06000B5E RID: 2910
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FlushFaceCacheDelegate();

		// Token: 0x0200021D RID: 541
		// (Invoke) Token: 0x06000B62 RID: 2914
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetDeformKeyDataDelegate(int keyNo, ref DeformKeyData deformKeyData, int race, int gender, float age);

		// Token: 0x0200021E RID: 542
		// (Invoke) Token: 0x06000B66 RID: 2918
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFaceGenInstancesLengthDelegate(int race, int gender, float age);

		// Token: 0x0200021F RID: 543
		// (Invoke) Token: 0x06000B6A RID: 2922
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFacialIndicesByTagDelegate(int race, int curGender, float age, byte[] tag);

		// Token: 0x02000220 RID: 544
		// (Invoke) Token: 0x06000B6E RID: 2926
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetHairColorCountDelegate(int race, int curGender, float age);

		// Token: 0x02000221 RID: 545
		// (Invoke) Token: 0x06000B72 RID: 2930
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetHairColorGradientPointsDelegate(int race, int curGender, float age, IntPtr colors);

		// Token: 0x02000222 RID: 546
		// (Invoke) Token: 0x06000B76 RID: 2934
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetHairIndicesByTagDelegate(int race, int curGender, float age, byte[] tag);

		// Token: 0x02000223 RID: 547
		// (Invoke) Token: 0x06000B7A RID: 2938
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMaturityTypeDelegate(float age);

		// Token: 0x02000224 RID: 548
		// (Invoke) Token: 0x06000B7E RID: 2942
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumEditableDeformKeysDelegate(int race, [MarshalAs(UnmanagedType.U1)] bool initialGender, float age);

		// Token: 0x02000225 RID: 549
		// (Invoke) Token: 0x06000B82 RID: 2946
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetParamsFromKeyDelegate(ref FaceGenerationParams faceGenerationParams, ref BodyProperties bodyProperties, [MarshalAs(UnmanagedType.U1)] bool earsAreHidden, [MarshalAs(UnmanagedType.U1)] bool mouthHidden);

		// Token: 0x02000226 RID: 550
		// (Invoke) Token: 0x06000B86 RID: 2950
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetParamsMaxDelegate(int race, int curGender, float curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale);

		// Token: 0x02000227 RID: 551
		// (Invoke) Token: 0x06000B8A RID: 2954
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRaceIdsDelegate();

		// Token: 0x02000228 RID: 552
		// (Invoke) Token: 0x06000B8E RID: 2958
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetRandomBodyPropertiesDelegate(int race, int gender, ref BodyProperties bodyPropertiesMin, ref BodyProperties bodyPropertiesMax, int hairCoverType, int seed, byte[] hairTags, byte[] beardTags, byte[] tatooTags, float variationAmount, ref BodyProperties outBodyProperties);

		// Token: 0x02000229 RID: 553
		// (Invoke) Token: 0x06000B92 RID: 2962
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetScaleFromKeyDelegate(int race, int gender, ref BodyProperties initialBodyProperties);

		// Token: 0x0200022A RID: 554
		// (Invoke) Token: 0x06000B96 RID: 2966
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSkinColorCountDelegate(int race, int curGender, float age);

		// Token: 0x0200022B RID: 555
		// (Invoke) Token: 0x06000B9A RID: 2970
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSkinColorGradientPointsDelegate(int race, int curGender, float age, IntPtr colors);

		// Token: 0x0200022C RID: 556
		// (Invoke) Token: 0x06000B9E RID: 2974
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetTatooColorCountDelegate(int race, int curGender, float age);

		// Token: 0x0200022D RID: 557
		// (Invoke) Token: 0x06000BA2 RID: 2978
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetTatooColorGradientPointsDelegate(int race, int curGender, float age, IntPtr colors);

		// Token: 0x0200022E RID: 558
		// (Invoke) Token: 0x06000BA6 RID: 2982
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetTattooIndicesByTagDelegate(int race, int curGender, float age, byte[] tag);

		// Token: 0x0200022F RID: 559
		// (Invoke) Token: 0x06000BAA RID: 2986
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVoiceRecordsCountDelegate(int race, int curGender, float age);

		// Token: 0x02000230 RID: 560
		// (Invoke) Token: 0x06000BAE RID: 2990
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetVoiceTypeUsableForPlayerDataDelegate(int race, int curGender, float age, IntPtr aiArray);

		// Token: 0x02000231 RID: 561
		// (Invoke) Token: 0x06000BB2 RID: 2994
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetZeroProbabilitiesDelegate(int race, int curGender, float curAge, ref float tattooZeroProbability);

		// Token: 0x02000232 RID: 562
		// (Invoke) Token: 0x06000BB6 RID: 2998
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProduceNumericKeyWithDefaultValuesDelegate(ref BodyProperties initialBodyProperties, [MarshalAs(UnmanagedType.U1)] bool earsAreHidden, [MarshalAs(UnmanagedType.U1)] bool mouthIsHidden, int race, int gender, float age);

		// Token: 0x02000233 RID: 563
		// (Invoke) Token: 0x06000BBA RID: 3002
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProduceNumericKeyWithParamsDelegate(ref FaceGenerationParams faceGenerationParams, [MarshalAs(UnmanagedType.U1)] bool earsAreHidden, [MarshalAs(UnmanagedType.U1)] bool mouthIsHidden, ref BodyProperties bodyProperties);

		// Token: 0x02000234 RID: 564
		// (Invoke) Token: 0x06000BBE RID: 3006
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformFaceKeysToDefaultFaceDelegate(ref FaceGenerationParams faceGenerationParams);
	}
}
