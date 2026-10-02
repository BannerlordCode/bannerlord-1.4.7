using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AF RID: 431
	[ScriptingInterfaceBase]
	internal interface IMBSkeletonExtensions
	{
		// Token: 0x0600189A RID: 6298
		[EngineMethod("create_agent_skeleton", false, null, false)]
		Skeleton CreateAgentSkeleton(string skeletonName, bool isHumanoid, int actionSetIndex, string monsterUsageSetName, ref AnimationSystemData animationSystemData);

		// Token: 0x0600189B RID: 6299
		[EngineMethod("create_simple_skeleton", false, null, false)]
		Skeleton CreateSimpleSkeleton(string skeletonName);

		// Token: 0x0600189C RID: 6300
		[EngineMethod("create_with_action_set", false, null, false)]
		Skeleton CreateWithActionSet(ref AnimationSystemData animationSystemData);

		// Token: 0x0600189D RID: 6301
		[EngineMethod("get_skeleton_face_animation_time", false, null, false)]
		float GetSkeletonFaceAnimationTime(UIntPtr entityId);

		// Token: 0x0600189E RID: 6302
		[EngineMethod("set_skeleton_face_animation_time", false, null, false)]
		void SetSkeletonFaceAnimationTime(UIntPtr entityId, float time);

		// Token: 0x0600189F RID: 6303
		[EngineMethod("get_skeleton_face_animation_name", false, null, false)]
		string GetSkeletonFaceAnimationName(UIntPtr entityId);

		// Token: 0x060018A0 RID: 6304
		[EngineMethod("get_bone_entitial_frame_at_animation_progress", false, null, false)]
		void GetBoneEntitialFrameAtAnimationProgress(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame);

		// Token: 0x060018A1 RID: 6305
		[EngineMethod("get_bone_entitial_frame", false, null, false)]
		void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte bone, bool useBoneMapping, bool forceToUpdate, ref MatrixFrame outFrame);

		// Token: 0x060018A2 RID: 6306
		[EngineMethod("set_animation_at_channel", false, null, false)]
		void SetAnimationAtChannel(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress);

		// Token: 0x060018A3 RID: 6307
		[EngineMethod("get_action_at_channel", false, null, false)]
		int GetActionAtChannel(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x060018A4 RID: 6308
		[EngineMethod("set_facial_animation_of_channel", false, null, false)]
		void SetFacialAnimationOfChannel(UIntPtr skeletonPointer, int channel, string facialAnimationName, bool playSound, bool loop);

		// Token: 0x060018A5 RID: 6309
		[EngineMethod("set_agent_action_channel", false, null, false)]
		void SetAgentActionChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, bool forceFaceMorphRestart, float blendWithNextActionFactor);

		// Token: 0x060018A6 RID: 6310
		[EngineMethod("does_action_continue_with_current_action_at_channel", false, null, false)]
		bool DoesActionContinueWithCurrentActionAtChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex);

		// Token: 0x060018A7 RID: 6311
		[EngineMethod("tick_action_channels", false, null, false)]
		void TickActionChannels(UIntPtr skeletonPointer);
	}
}
