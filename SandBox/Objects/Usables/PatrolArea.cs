using System;
using SandBox.AI;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x0200004E RID: 78
	public class PatrolArea : UsableMachine
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002DB RID: 731 RVA: 0x000102CC File Offset: 0x0000E4CC
		// (set) Token: 0x060002DC RID: 732 RVA: 0x000102D4 File Offset: 0x0000E4D4
		private int ActiveIndex
		{
			get
			{
				return this._activeIndex;
			}
			set
			{
				if (this._activeIndex != value)
				{
					base.StandingPoints[value].IsDeactivated = false;
					base.StandingPoints[this._activeIndex].IsDeactivated = true;
					this._activeIndex = value;
				}
				if (base.StandingPoints.Count == 1 && this._activeIndex == 0 && base.StandingPoints[this._activeIndex].IsDeactivated)
				{
					base.StandingPoints[this._activeIndex].IsDeactivated = false;
				}
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0001035F File Offset: 0x0000E55F
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			if (pilotStandingPoint == null)
			{
				return null;
			}
			return pilotStandingPoint.ActionMessage;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00010372 File Offset: 0x0000E572
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			return ((pilotStandingPoint != null) ? pilotStandingPoint.DescriptionMessage : null) ?? TextObject.GetEmpty();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001038F File Offset: 0x0000E58F
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new UsablePlaceAI(this);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010398 File Offset: 0x0000E598
		protected override void OnInit()
		{
			base.OnInit();
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.IsDeactivated = true;
			}
			this.ActiveIndex = base.StandingPoints.Count - 1;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00010410 File Offset: 0x0000E610
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001041C File Offset: 0x0000E61C
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (base.StandingPoints[this.ActiveIndex].HasAIUser)
			{
				this.ActiveIndex = ((this.ActiveIndex == 0) ? (base.StandingPoints.Count - 1) : (this.ActiveIndex - 1));
			}
		}

		// Token: 0x0400013D RID: 317
		public int AreaIndex;

		// Token: 0x0400013E RID: 318
		private int _activeIndex;
	}
}
