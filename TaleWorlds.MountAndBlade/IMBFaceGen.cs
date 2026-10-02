using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B9 RID: 441
	[ScriptingInterfaceBase]
	internal interface IMBFaceGen
	{
		// Token: 0x060018D1 RID: 6353
		[EngineMethod("get_num_editable_deform_keys", false, null, false)]
		int GetNumEditableDeformKeys(int race, bool initialGender, float age);

		// Token: 0x060018D2 RID: 6354
		[EngineMethod("get_params_from_key", false, null, false)]
		void GetParamsFromKey(ref FaceGenerationParams faceGenerationParams, ref BodyProperties bodyProperties, bool earsAreHidden, bool mouthHidden);

		// Token: 0x060018D3 RID: 6355
		[EngineMethod("get_params_max", false, null, false)]
		void GetParamsMax(int race, int curGender, float curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale);

		// Token: 0x060018D4 RID: 6356
		[EngineMethod("get_zero_probabilities", false, null, false)]
		void GetZeroProbabilities(int race, int curGender, float curAge, ref float tattooZeroProbability);

		// Token: 0x060018D5 RID: 6357
		[EngineMethod("produce_numeric_key_with_params", false, null, false)]
		void ProduceNumericKeyWithParams(ref FaceGenerationParams faceGenerationParams, bool earsAreHidden, bool mouthIsHidden, ref BodyProperties bodyProperties);

		// Token: 0x060018D6 RID: 6358
		[EngineMethod("produce_numeric_key_with_default_values", false, null, false)]
		void ProduceNumericKeyWithDefaultValues(ref BodyProperties initialBodyProperties, bool earsAreHidden, bool mouthIsHidden, int race, int gender, float age);

		// Token: 0x060018D7 RID: 6359
		[EngineMethod("transform_face_keys_to_default_face", false, null, false)]
		void TransformFaceKeysToDefaultFace(ref FaceGenerationParams faceGenerationParams);

		// Token: 0x060018D8 RID: 6360
		[EngineMethod("get_random_body_properties", false, null, false)]
		void GetRandomBodyProperties(int race, int gender, ref BodyProperties bodyPropertiesMin, ref BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount, ref BodyProperties outBodyProperties);

		// Token: 0x060018D9 RID: 6361
		[EngineMethod("enforce_constraints", false, null, false)]
		bool EnforceConstraints(ref FaceGenerationParams faceGenerationParams);

		// Token: 0x060018DA RID: 6362
		[EngineMethod("get_deform_key_data", false, null, false)]
		void GetDeformKeyData(int keyNo, ref DeformKeyData deformKeyData, int race, int gender, float age);

		// Token: 0x060018DB RID: 6363
		[EngineMethod("get_face_gen_instances_length", false, null, false)]
		int GetFaceGenInstancesLength(int race, int gender, float age);

		// Token: 0x060018DC RID: 6364
		[EngineMethod("get_scale", false, null, false)]
		float GetScaleFromKey(int race, int gender, ref BodyProperties initialBodyProperties);

		// Token: 0x060018DD RID: 6365
		[EngineMethod("get_voice_records_count", false, null, false)]
		int GetVoiceRecordsCount(int race, int curGender, float age);

		// Token: 0x060018DE RID: 6366
		[EngineMethod("get_hair_color_count", false, null, false)]
		int GetHairColorCount(int race, int curGender, float age);

		// Token: 0x060018DF RID: 6367
		[EngineMethod("get_hair_color_gradient_points", false, null, false)]
		void GetHairColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x060018E0 RID: 6368
		[EngineMethod("get_tatoo_color_count", false, null, false)]
		int GetTatooColorCount(int race, int curGender, float age);

		// Token: 0x060018E1 RID: 6369
		[EngineMethod("get_tatoo_color_gradient_points", false, null, false)]
		void GetTatooColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x060018E2 RID: 6370
		[EngineMethod("get_skin_color_count", false, null, false)]
		int GetSkinColorCount(int race, int curGender, float age);

		// Token: 0x060018E3 RID: 6371
		[EngineMethod("get_maturity_type", false, null, false)]
		int GetMaturityType(float age);

		// Token: 0x060018E4 RID: 6372
		[EngineMethod("flush_face_cache", false, null, false)]
		void FlushFaceCache();

		// Token: 0x060018E5 RID: 6373
		[EngineMethod("get_voice_type_usable_for_player_data", false, null, false)]
		void GetVoiceTypeUsableForPlayerData(int race, int curGender, float age, bool[] aiArray);

		// Token: 0x060018E6 RID: 6374
		[EngineMethod("get_skin_color_gradient_points", false, null, false)]
		void GetSkinColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x060018E7 RID: 6375
		[EngineMethod("get_race_ids", false, null, false)]
		string GetRaceIds();

		// Token: 0x060018E8 RID: 6376
		[EngineMethod("get_hair_indices_by_tag", false, null, false)]
		string GetHairIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x060018E9 RID: 6377
		[EngineMethod("get_facial_indices_by_tag", false, null, false)]
		string GetFacialIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x060018EA RID: 6378
		[EngineMethod("get_tattoo_indices_by_tag", false, null, false)]
		string GetTattooIndicesByTag(int race, int curGender, float age, string tag);
	}
}
