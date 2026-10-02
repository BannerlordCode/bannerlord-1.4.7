using System;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000081 RID: 129
	public abstract class MissionView : MissionBehavior
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000249EB File Offset: 0x00022BEB
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x000249F3 File Offset: 0x00022BF3
		public MissionScreen MissionScreen { get; internal set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000249FC File Offset: 0x00022BFC
		public IInputContext Input
		{
			get
			{
				return this.MissionScreen.SceneLayer.Input;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00024A0E File Offset: 0x00022C0E
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00024A16 File Offset: 0x00022C16
		private protected bool IsViewSuspended { protected get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00024A1F File Offset: 0x00022C1F
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Other;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00024A22 File Offset: 0x00022C22
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x00024A2A File Offset: 0x00022C2A
		public bool IsFinalized { get; internal set; }

		// Token: 0x060004D0 RID: 1232 RVA: 0x00024A33 File Offset: 0x00022C33
		public virtual void OnMissionScreenTick(float dt)
		{
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00024A35 File Offset: 0x00022C35
		public virtual bool OnEscape()
		{
			return false;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00024A38 File Offset: 0x00022C38
		public virtual bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00024A3B File Offset: 0x00022C3B
		public virtual bool IsPhotoModeAllowed()
		{
			return true;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00024A3E File Offset: 0x00022C3E
		public virtual void OnFocusChangeOnGameWindow(bool focusGained)
		{
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00024A40 File Offset: 0x00022C40
		public virtual void OnSceneRenderingStarted()
		{
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00024A42 File Offset: 0x00022C42
		public virtual void OnMissionScreenInitialize()
		{
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00024A44 File Offset: 0x00022C44
		public virtual void OnMissionScreenFinalize()
		{
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00024A46 File Offset: 0x00022C46
		public virtual void OnMissionScreenActivate()
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00024A48 File Offset: 0x00022C48
		public virtual void OnMissionScreenDeactivate()
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00024A4A File Offset: 0x00022C4A
		public virtual bool UpdateOverridenCamera(float dt)
		{
			return false;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00024A4D File Offset: 0x00022C4D
		public virtual bool IsReady()
		{
			return true;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00024A50 File Offset: 0x00022C50
		public virtual void OnPhotoModeActivated()
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00024A52 File Offset: 0x00022C52
		public virtual void OnPhotoModeDeactivated()
		{
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00024A54 File Offset: 0x00022C54
		public virtual void OnConversationBegin()
		{
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00024A56 File Offset: 0x00022C56
		public virtual void OnConversationEnd()
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00024A58 File Offset: 0x00022C58
		protected virtual void OnSuspendView()
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00024A5A File Offset: 0x00022C5A
		protected virtual void OnResumeView()
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00024A5C File Offset: 0x00022C5C
		public virtual void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00024A5E File Offset: 0x00022C5E
		public void SuspendView()
		{
			this.OnSuspendView();
			this.IsViewSuspended = true;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00024A6D File Offset: 0x00022C6D
		public void ResumeView()
		{
			this.OnResumeView();
			this.IsViewSuspended = false;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00024A7C File Offset: 0x00022C7C
		public sealed override void OnEndMissionInternal()
		{
			this.OnEndMission();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00024A84 File Offset: 0x00022C84
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
		}

		// Token: 0x040002C1 RID: 705
		public int ViewOrderPriority;
	}
}
