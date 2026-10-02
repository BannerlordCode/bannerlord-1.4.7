using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A0 RID: 416
	public struct MBAnimation
	{
		// Token: 0x06001643 RID: 5699 RVA: 0x00052A5A File Offset: 0x00050C5A
		public MBAnimation(MBAnimation animation)
		{
			this._index = animation._index;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00052A68 File Offset: 0x00050C68
		internal MBAnimation(int i)
		{
			this._index = i;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00052A71 File Offset: 0x00050C71
		public bool Equals(MBAnimation a)
		{
			return this._index == a._index;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00052A81 File Offset: 0x00050C81
		public override int GetHashCode()
		{
			return this._index;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00052A89 File Offset: 0x00050C89
		public static int GetAnimationIndexWithName(string animationName)
		{
			if (string.IsNullOrEmpty(animationName))
			{
				return -1;
			}
			return MBAPI.IMBAnimation.GetIndexWithID(animationName);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00052AA0 File Offset: 0x00050CA0
		public static Agent.ActionCodeType GetActionType(ActionIndexCache actionIndex)
		{
			if (!(actionIndex == ActionIndexCache.act_none))
			{
				return MBAPI.IMBAnimation.GetActionType(actionIndex.Index);
			}
			return Agent.ActionCodeType.Other;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00052AC2 File Offset: 0x00050CC2
		public static void PrefetchAnimationClip(MBActionSet actionSet, ActionIndexCache actionIndexCache)
		{
			MBAPI.IMBAnimation.PrefetchAnimationClip(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00052ADC File Offset: 0x00050CDC
		public static float GetAnimationDuration(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationDuration(indexWithID);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00052B00 File Offset: 0x00050D00
		public static float GetAnimationDuration(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationDuration(animationIndex);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00052B10 File Offset: 0x00050D10
		public static float GetAnimationParameter1(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter1(indexWithID);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00052B34 File Offset: 0x00050D34
		public static float GetAnimationParameter1(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationParameter1(animationIndex);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00052B44 File Offset: 0x00050D44
		public static float GetAnimationParameter2(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter2(indexWithID);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00052B68 File Offset: 0x00050D68
		public static float GetAnimationParameter2(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationParameter2(animationIndex);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00052B78 File Offset: 0x00050D78
		public static float GetAnimationParameter3(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter3(indexWithID);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00052B9C File Offset: 0x00050D9C
		public static float GetAnimationBlendInPeriod(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationBlendInPeriod(indexWithID);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00052BC0 File Offset: 0x00050DC0
		public static float GetAnimationBlendInPeriod(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationBlendInPeriod(animationIndex);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00052BD0 File Offset: 0x00050DD0
		public static ActionIndexCache GetAnimationBlendsWithActionIndex(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationBlendsWithActionIndex(indexWithID));
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00052BF9 File Offset: 0x00050DF9
		public static ActionIndexCache GetAnimationBlendsWithActionIndex(int animationIndex)
		{
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationBlendsWithActionIndex(animationIndex));
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00052C0C File Offset: 0x00050E0C
		public static Vec3 GetAnimationDisplacementAtProgress(string animationName, float progress)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationDisplacementAtProgress(indexWithID, progress);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00052C31 File Offset: 0x00050E31
		public static Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress)
		{
			return MBAPI.IMBAnimation.GetAnimationDisplacementAtProgress(animationIndex, progress);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00052C3F File Offset: 0x00050E3F
		public static int GetActionCodeWithName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return MBAPI.IMBAnimation.GetActionCodeWithName(name);
			}
			return ActionIndexCache.act_none.Index;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00052C5F File Offset: 0x00050E5F
		public static int GetNumActionCodes()
		{
			return MBAPI.IMBAnimation.GetNumActionCodes();
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00052C6B File Offset: 0x00050E6B
		public static int GetNumAnimations()
		{
			return MBAPI.IMBAnimation.GetNumAnimations();
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00052C77 File Offset: 0x00050E77
		public static bool IsAnyAnimationLoadingFromDisk()
		{
			return MBAPI.IMBAnimation.IsAnyAnimationLoadingFromDisk();
		}

		// Token: 0x04000854 RID: 2132
		private readonly int _index;
	}
}
