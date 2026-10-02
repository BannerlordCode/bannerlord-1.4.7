using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000129 RID: 297
	public class BehaviorSergeantMPLastFlagLastStand : BehaviorComponent
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x000237FA File Offset: 0x000219FA
		public BehaviorSergeantMPLastFlagLastStand(Formation formation)
			: base(formation)
		{
			this._flagpositions = Mission.Current.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00023834 File Offset: 0x00021A34
		protected override void CalculateCurrentOrder()
		{
			base.CurrentOrder = ((this._flagpositions.Count > 0) ? MovementOrder.MovementOrderMove(new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._flagpositions[0].Position, false)) : MovementOrder.MovementOrderStop);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00023888 File Offset: 0x00021A88
		public override void TickOccasionally()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000238E8 File Offset: 0x00021AE8
		protected override void OnBehaviorActivatedAux()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00023978 File Offset: 0x00021B78
		protected override float GetAiWeight()
		{
			if (this._lastEffort)
			{
				return 10f;
			}
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			FlagCapturePoint flagCapturePoint = this._flagpositions.FirstOrDefault<FlagCapturePoint>();
			if (this._flagpositions.Count != 1 || this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint) == null || !this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint).IsEnemyOf(base.Formation.Team))
			{
				return 0f;
			}
			float timeUntilBattleSideVictory = this._flagDominationGameMode.GetTimeUntilBattleSideVictory(this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint).Side);
			if (timeUntilBattleSideVictory <= 60f)
			{
				return 10f;
			}
			float num = base.Formation.CachedAveragePosition.Distance(flagCapturePoint.Position.AsVec2);
			float movementSpeedMaximum = base.Formation.QuerySystem.MovementSpeedMaximum;
			if (num / movementSpeedMaximum * 8f > timeUntilBattleSideVictory)
			{
				this._lastEffort = true;
				return 10f;
			}
			return 0f;
		}

		// Token: 0x0400037F RID: 895
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x04000380 RID: 896
		private bool _lastEffort;

		// Token: 0x04000381 RID: 897
		private MissionMultiplayerFlagDomination _flagDominationGameMode;
	}
}
