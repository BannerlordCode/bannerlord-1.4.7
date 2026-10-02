using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A4 RID: 420
	[ScriptingInterfaceBase]
	internal interface IMBActionSet
	{
		// Token: 0x06001668 RID: 5736
		[EngineMethod("get_index_with_id", false, null, false)]
		int GetIndexWithID(string id);

		// Token: 0x06001669 RID: 5737
		[EngineMethod("get_name_with_index", false, null, false)]
		string GetNameWithIndex(int index);

		// Token: 0x0600166A RID: 5738
		[EngineMethod("get_skeleton_name", false, null, false)]
		string GetSkeletonName(int index);

		// Token: 0x0600166B RID: 5739
		[EngineMethod("get_number_of_action_sets", false, null, false)]
		int GetNumberOfActionSets();

		// Token: 0x0600166C RID: 5740
		[EngineMethod("get_number_of_monster_usage_sets", false, null, false)]
		int GetNumberOfMonsterUsageSets();

		// Token: 0x0600166D RID: 5741
		[EngineMethod("get_animation_name", false, null, false)]
		string GetAnimationName(int index, int actionNo);

		// Token: 0x0600166E RID: 5742
		[EngineMethod("are_actions_alternatives", false, null, false)]
		bool AreActionsAlternatives(int index, int actionNo1, int actionNo2);

		// Token: 0x0600166F RID: 5743
		[EngineMethod("get_bone_index_with_id", false, null, false)]
		sbyte GetBoneIndexWithId(string actionSetId, string boneId);

		// Token: 0x06001670 RID: 5744
		[EngineMethod("get_bone_has_parent_bone", false, null, false)]
		bool GetBoneHasParentBone(string actionSetId, sbyte boneIndex);
	}
}
