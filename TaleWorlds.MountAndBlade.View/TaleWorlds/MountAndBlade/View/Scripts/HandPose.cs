using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000066 RID: 102
	public class HandPose : ScriptComponentBehavior
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x0001D9E6 File Offset: 0x0001BBE6
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001DA00 File Offset: 0x0001BC00
		protected override void OnEditorTick(float dt)
		{
			if (!this._isFinished && this._editorGameManager != null)
			{
				this._isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && !this._initiliazed)
			{
				AnimationSystemData animationSystemData = Game.Current.DefaultMonster.FillAnimationSystemData(MBActionSet.GetActionSet(Game.Current.DefaultMonster.ActionSetCode), 1f, false);
				base.GameEntity.CreateSkeletonWithActionSet(ref animationSystemData);
				base.GameEntity.CopyComponentsToSkeleton();
				base.GameEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_tableau_hand_armor_pose, 0f, -0.2f, true, 0f);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.01f, base.GameEntity.GetGlobalFrame(), true);
				base.GameEntity.Skeleton.Freeze(false);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), false);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(0f, 0f, 1f));
				base.GameEntity.Skeleton.SetUptoDate(false);
				base.GameEntity.Skeleton.Freeze(true);
				this._initiliazed = true;
			}
			if (this._initiliazed)
			{
				base.GameEntity.Skeleton.Freeze(false);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), false);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(0f, 0f, 1f));
				base.GameEntity.Skeleton.SetUptoDate(false);
				base.GameEntity.Skeleton.Freeze(true);
			}
		}

		// Token: 0x0400024A RID: 586
		private MBGameManager _editorGameManager;

		// Token: 0x0400024B RID: 587
		private bool _initiliazed;

		// Token: 0x0400024C RID: 588
		private bool _isFinished;
	}
}
