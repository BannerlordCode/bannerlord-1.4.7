using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000199 RID: 409
	[EngineStruct("int", false, null)]
	public struct MBActionSet
	{
		// Token: 0x060015D5 RID: 5589 RVA: 0x0005139E File Offset: 0x0004F59E
		internal MBActionSet(int i)
		{
			this.Index = i;
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x000513A7 File Offset: 0x0004F5A7
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x000513B5 File Offset: 0x0004F5B5
		public bool Equals(MBActionSet a)
		{
			return this.Index == a.Index;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000513C5 File Offset: 0x0004F5C5
		public bool Equals(int index)
		{
			return this.Index == index;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000513D0 File Offset: 0x0004F5D0
		public override int GetHashCode()
		{
			return this.Index;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000513D8 File Offset: 0x0004F5D8
		public string GetName()
		{
			if (!this.IsValid)
			{
				return "Invalid";
			}
			return MBAPI.IMBActionSet.GetNameWithIndex(this.Index);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000513F8 File Offset: 0x0004F5F8
		public string GetSkeletonName()
		{
			return MBAPI.IMBActionSet.GetSkeletonName(this.Index);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0005140A File Offset: 0x0004F60A
		public string GetAnimationName(in ActionIndexCache actionCode)
		{
			return MBAPI.IMBActionSet.GetAnimationName(this.Index, actionCode.Index);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00051422 File Offset: 0x0004F622
		public bool AreActionsAlternatives(in ActionIndexCache actionCode1, in ActionIndexCache actionCode2)
		{
			return MBAPI.IMBActionSet.AreActionsAlternatives(this.Index, actionCode1.Index, actionCode2.Index);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00051440 File Offset: 0x0004F640
		public static int GetNumberOfActionSets()
		{
			return MBAPI.IMBActionSet.GetNumberOfActionSets();
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0005144C File Offset: 0x0004F64C
		public static int GetNumberOfMonsterUsageSets()
		{
			return MBAPI.IMBActionSet.GetNumberOfMonsterUsageSets();
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00051458 File Offset: 0x0004F658
		public static MBActionSet GetActionSet(string objectID)
		{
			return MBActionSet.GetActionSetWithIndex(MBAPI.IMBActionSet.GetIndexWithID(objectID));
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0005146A File Offset: 0x0004F66A
		public static MBActionSet GetActionSetWithIndex(int index)
		{
			return new MBActionSet(index);
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00051472 File Offset: 0x0004F672
		public static sbyte GetBoneIndexWithId(string actionSetId, string boneId)
		{
			return MBAPI.IMBActionSet.GetBoneIndexWithId(actionSetId, boneId);
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00051480 File Offset: 0x0004F680
		public static bool GetBoneHasParentBone(string actionSetId, sbyte boneIndex)
		{
			return MBAPI.IMBActionSet.GetBoneHasParentBone(actionSetId, boneIndex);
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0005148E File Offset: 0x0004F68E
		public static Vec3 GetActionDisplacementVector(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetDisplacementVector(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x000514A6 File Offset: 0x0004F6A6
		public static AnimFlags GetActionAnimationFlags(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetAnimationFlags(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x000514BE File Offset: 0x0004F6BE
		public static bool CheckActionAnimationClipExists(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.CheckAnimationClipExists(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x000514D6 File Offset: 0x0004F6D6
		public static int GetAnimationIndexOfAction(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.AnimationIndexOfActionCode(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000514EE File Offset: 0x0004F6EE
		public static string GetActionAnimationName(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetAnimationName(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00051506 File Offset: 0x0004F706
		public static float GetActionAnimationDuration(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetActionAnimationDuration(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0005151E File Offset: 0x0004F71E
		public static ActionIndexCache GetActionAnimationContinueToAction(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationContinueToAction(actionSet.Index, actionIndexCache.Index));
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0005153C File Offset: 0x0004F73C
		public static float GetTotalAnimationDurationWithContinueToAction(MBActionSet actionSet, ActionIndexCache actionIndexCache)
		{
			float num = 0f;
			while (actionIndexCache != ActionIndexCache.act_none)
			{
				num += MBActionSet.GetActionAnimationDuration(actionSet, in actionIndexCache);
				actionIndexCache = MBActionSet.GetActionAnimationContinueToAction(actionSet, in actionIndexCache);
			}
			return num;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00051574 File Offset: 0x0004F774
		public static float GetActionBlendOutStartProgress(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetActionBlendOutStartProgress(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x04000715 RID: 1813
		[CustomEngineStructMemberData("ignoredMember", true)]
		internal readonly int Index;

		// Token: 0x04000716 RID: 1814
		public static readonly MBActionSet InvalidActionSet = new MBActionSet(-1);
	}
}
