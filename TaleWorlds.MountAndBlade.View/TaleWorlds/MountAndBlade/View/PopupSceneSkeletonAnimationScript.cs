using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000020 RID: 32
	public class PopupSceneSkeletonAnimationScript : ScriptComponentBehavior
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00006AD4 File Offset: 0x00004CD4
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006AE8 File Offset: 0x00004CE8
		public void Initialize()
		{
			if (this.SkeletonName != "" && (base.GameEntity.Skeleton == null || base.GameEntity.Skeleton.GetName() != this.SkeletonName))
			{
				base.GameEntity.CreateSimpleSkeleton(this.SkeletonName);
				return;
			}
			if (this.SkeletonName == "" && base.GameEntity.Skeleton != null)
			{
				base.GameEntity.RemoveSkeleton();
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006B85 File Offset: 0x00004D85
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006B88 File Offset: 0x00004D88
		protected override void OnTick(float dt)
		{
			if (base.GameEntity.Skeleton.GetAnimationAtChannel(0) == this.InitialAnimationClip && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) >= 1f)
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(this.InitialAnimationContinueClip, 0, 1f, -1f, 0f);
			}
			if (base.GameEntity.Skeleton.GetAnimationAtChannel(0) == this.PositiveAnimationClip && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) >= 1f)
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(this.PositiveAnimationContinueClip, 0, 1f, -1f, 0f);
			}
			if (base.GameEntity.Skeleton.GetAnimationAtChannel(0) == this.NegativeAnimationClip && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) >= 1f)
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(this.NegativeAnimationContinueClip, 0, 1f, -1f, 0f);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006CC4 File Offset: 0x00004EC4
		public void SetState(int state)
		{
			string text = ((state == 1) ? this.PositiveAnimationClip : ((state == 0) ? this.InitialAnimationClip : this.NegativeAnimationClip));
			if (base.GameEntity.IsValid && base.GameEntity.Skeleton != null && !string.IsNullOrEmpty(text))
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(text, 0, 1f, -1f, 0f);
			}
			this._currentState = state;
		}

		// Token: 0x04000027 RID: 39
		public string SkeletonName = "";

		// Token: 0x04000028 RID: 40
		public int BoneIndex;

		// Token: 0x04000029 RID: 41
		public Vec3 AttachmentOffset = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x0400002A RID: 42
		public string InitialAnimationClip = "";

		// Token: 0x0400002B RID: 43
		public string PositiveAnimationClip = "";

		// Token: 0x0400002C RID: 44
		public string NegativeAnimationClip = "";

		// Token: 0x0400002D RID: 45
		public string InitialAnimationContinueClip = "";

		// Token: 0x0400002E RID: 46
		public string PositiveAnimationContinueClip = "";

		// Token: 0x0400002F RID: 47
		public string NegativeAnimationContinueClip = "";

		// Token: 0x04000030 RID: 48
		private int _currentState;
	}
}
