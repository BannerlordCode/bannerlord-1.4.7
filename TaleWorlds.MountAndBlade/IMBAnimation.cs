using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A7 RID: 423
	[ScriptingInterfaceBase]
	internal interface IMBAnimation
	{
		// Token: 0x060017AE RID: 6062
		[EngineMethod("get_id_with_index", false, null, false)]
		string GetIDWithIndex(int index);

		// Token: 0x060017AF RID: 6063
		[EngineMethod("get_index_with_id", false, null, false)]
		int GetIndexWithID(string id);

		// Token: 0x060017B0 RID: 6064
		[EngineMethod("get_displacement_vector", false, null, false)]
		Vec3 GetDisplacementVector(int actionSetNo, int actionIndex);

		// Token: 0x060017B1 RID: 6065
		[EngineMethod("check_animation_clip_exists", false, null, false)]
		bool CheckAnimationClipExists(int actionSetNo, int actionIndex);

		// Token: 0x060017B2 RID: 6066
		[EngineMethod("prefetch_animation_clip", false, null, false)]
		void PrefetchAnimationClip(int actionSetNo, int actionIndex);

		// Token: 0x060017B3 RID: 6067
		[EngineMethod("get_animation_index_of_action_code", false, null, true)]
		int AnimationIndexOfActionCode(int actionSetNo, int actionIndex);

		// Token: 0x060017B4 RID: 6068
		[EngineMethod("get_animation_flags", false, null, false)]
		AnimFlags GetAnimationFlags(int actionSetNo, int actionIndex);

		// Token: 0x060017B5 RID: 6069
		[EngineMethod("get_action_type", false, null, true)]
		Agent.ActionCodeType GetActionType(int actionIndex);

		// Token: 0x060017B6 RID: 6070
		[EngineMethod("get_animation_duration", false, null, false)]
		float GetAnimationDuration(int animationIndex);

		// Token: 0x060017B7 RID: 6071
		[EngineMethod("get_animation_parameter1", false, null, false)]
		float GetAnimationParameter1(int animationIndex);

		// Token: 0x060017B8 RID: 6072
		[EngineMethod("get_animation_parameter2", false, null, false)]
		float GetAnimationParameter2(int animationIndex);

		// Token: 0x060017B9 RID: 6073
		[EngineMethod("get_animation_parameter3", false, null, false)]
		float GetAnimationParameter3(int animationIndex);

		// Token: 0x060017BA RID: 6074
		[EngineMethod("get_action_animation_duration", false, null, false)]
		float GetActionAnimationDuration(int actionSetNo, int actionIndex);

		// Token: 0x060017BB RID: 6075
		[EngineMethod("get_animation_name", false, null, false)]
		string GetAnimationName(int actionSetNo, int actionIndex);

		// Token: 0x060017BC RID: 6076
		[EngineMethod("get_animation_continue_to_action", false, null, false)]
		int GetAnimationContinueToAction(int actionSetNo, int actionIndex);

		// Token: 0x060017BD RID: 6077
		[EngineMethod("get_animation_blend_in_period", false, null, false)]
		float GetAnimationBlendInPeriod(int animationIndex);

		// Token: 0x060017BE RID: 6078
		[EngineMethod("get_action_blend_out_start_progress", false, null, false)]
		float GetActionBlendOutStartProgress(int actionSetNo, int actionIndex);

		// Token: 0x060017BF RID: 6079
		[EngineMethod("get_animation_blends_with_action_index", false, null, false)]
		int GetAnimationBlendsWithActionIndex(int animationIndex);

		// Token: 0x060017C0 RID: 6080
		[EngineMethod("get_animation_displacement_at_progress", false, null, true)]
		Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress);

		// Token: 0x060017C1 RID: 6081
		[EngineMethod("get_action_code_with_name", false, null, false)]
		int GetActionCodeWithName(string name);

		// Token: 0x060017C2 RID: 6082
		[EngineMethod("get_action_name_with_code", false, null, false)]
		string GetActionNameWithCode(int index);

		// Token: 0x060017C3 RID: 6083
		[EngineMethod("get_num_action_codes", false, null, false)]
		int GetNumActionCodes();

		// Token: 0x060017C4 RID: 6084
		[EngineMethod("get_num_animations", false, null, false)]
		int GetNumAnimations();

		// Token: 0x060017C5 RID: 6085
		[EngineMethod("is_any_animation_loading_from_disk", false, null, false)]
		bool IsAnyAnimationLoadingFromDisk();
	}
}
