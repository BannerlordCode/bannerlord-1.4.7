using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000137 RID: 311
	public class BehaviorUseSiegeMachines : BehaviorComponent
	{
		// Token: 0x06000F00 RID: 3840 RVA: 0x00027C64 File Offset: 0x00025E64
		public BehaviorUseSiegeMachines(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._primarySiegeWeapons = new List<SiegeWeapon>();
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				if ((primarySiegeWeapon = missionObject as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this._behaviorSide)
				{
					this._primarySiegeWeapons.Add(missionObject as SiegeWeapon);
				}
			}
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			base.BehaviorCoherence = 0f;
			this._stopOrder = MovementOrder.MovementOrderStop;
			this.RecreateFollowEntityOrder();
			if (this._followEntityOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid)
			{
				this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Follow;
				base.CurrentOrder = this._followEntityOrder;
				return;
			}
			this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Stop;
			base.CurrentOrder = this._stopOrder;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00027D68 File Offset: 0x00025F68
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00027DC4 File Offset: 0x00025FC4
		private void RecreateFollowEntityOrder()
		{
			this._followEntityOrder = MovementOrder.MovementOrderStop;
			SiegeWeapon siegeWeapon = this._primarySiegeWeapons.FirstOrDefault<SiegeWeapon>(delegate(SiegeWeapon psw)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				return !psw.IsDeactivated && (primarySiegeWeapon = psw as IPrimarySiegeWeapon) != null && !primarySiegeWeapon.HasCompletedAction();
			});
			this._followedEntity = ((siegeWeapon != null) ? siegeWeapon.WaitEntity : null);
			if (this._followedEntity != null)
			{
				this._followEntityOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00027E38 File Offset: 0x00026038
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._primarySiegeWeapons.Clear();
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				if ((primarySiegeWeapon = missionObject as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this._behaviorSide && !((SiegeWeapon)missionObject).IsDeactivated)
				{
					this._primarySiegeWeapons.Add(missionObject as SiegeWeapon);
				}
			}
			this.RecreateFollowEntityOrder();
			this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Unset;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00027EDC File Offset: 0x000260DC
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			bool flag = false;
			for (int i = this._primarySiegeWeapons.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon = this._primarySiegeWeapons[i];
				if (siegeWeapon.IsDestroyed || siegeWeapon.IsDeactivated)
				{
					this._primarySiegeWeapons.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				this.RecreateFollowEntityOrder();
			}
			int num = 0;
			SiegeTower siegeTower = null;
			foreach (SiegeWeapon siegeWeapon2 in this._primarySiegeWeapons)
			{
				if (!((IPrimarySiegeWeapon)siegeWeapon2).HasCompletedAction())
				{
					num++;
					SiegeTower siegeTower2;
					if ((siegeTower2 = siegeWeapon2 as SiegeTower) != null)
					{
						siegeTower = siegeTower2;
					}
				}
			}
			if (num == 0)
			{
				base.CurrentOrder = this._stopOrder;
				return;
			}
			if (this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow)
			{
				if (this._followEntityOrder.OrderEnum == MovementOrder.MovementOrderEnum.Stop)
				{
					this.RecreateFollowEntityOrder();
				}
				base.CurrentOrder = this._followEntityOrder;
			}
			BehaviorUseSiegeMachines.BehaviorState behaviorState = ((siegeTower != null && siegeTower.HasArrivedAtTarget) ? BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower : ((this._followEntityOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid) ? BehaviorUseSiegeMachines.BehaviorState.Follow : BehaviorUseSiegeMachines.BehaviorState.Stop));
			if (behaviorState != this._behaviorState)
			{
				if (behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow)
				{
					base.CurrentOrder = this._followEntityOrder;
				}
				else if (behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower)
				{
					this.RecreateFollowEntityOrder();
					base.CurrentOrder = this._followEntityOrder;
				}
				else
				{
					base.CurrentOrder = this._stopOrder;
				}
				this._behaviorState = behaviorState;
				bool flag2 = this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower;
				if (!flag2)
				{
					using (List<SiegeWeapon>.Enumerator enumerator = this._primarySiegeWeapons.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SiegeLadder siegeLadder;
							if ((siegeLadder = enumerator.Current as SiegeLadder) != null && !siegeLadder.IsDisabled)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
				if (flag2)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
				}
				else if (base.Formation.QuerySystem.IsRangedFormation)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
				}
				else
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				}
			}
			if (this._followedEntity != null && (this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow || this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower))
			{
				base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(this._followedEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized()));
			}
			else
			{
				base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			}
			if (base.Formation.AI.ActiveBehavior == this)
			{
				foreach (SiegeWeapon siegeWeapon3 in this._primarySiegeWeapons)
				{
					if (!((IPrimarySiegeWeapon)siegeWeapon3).HasCompletedAction())
					{
						if (!siegeWeapon3.IsUsedByFormation(base.Formation))
						{
							base.Formation.StartUsingMachine(siegeWeapon3, false);
						}
						for (int j = siegeWeapon3.UserFormations.Count - 1; j >= 0; j--)
						{
							Formation formation = siegeWeapon3.UserFormations[j];
							if (formation != base.Formation && formation.IsAIControlled && (formation.AI.Side != this._behaviorSide || !(formation.AI.ActiveBehavior is BehaviorUseSiegeMachines)) && formation.Team == base.Formation.Team)
							{
								formation.StopUsingMachine(siegeWeapon3, false);
							}
						}
					}
				}
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00028284 File Offset: 0x00026484
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.IsRangedFormation ? ArrangementOrder.ArrangementOrderScatter : ArrangementOrder.ArrangementOrderShieldWall);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x000282EB File Offset: 0x000264EB
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000282F4 File Offset: 0x000264F4
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._teamAISiegeComponent != null && this._primarySiegeWeapons.Count > 0)
			{
				if (this._primarySiegeWeapons.All<SiegeWeapon>((SiegeWeapon psw) => !(psw as IPrimarySiegeWeapon).HasCompletedAction()))
				{
					num = ((!this._teamAISiegeComponent.IsCastleBreached()) ? 0.75f : 0.25f);
				}
			}
			return num;
		}

		// Token: 0x040003A7 RID: 935
		private List<SiegeWeapon> _primarySiegeWeapons;

		// Token: 0x040003A8 RID: 936
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x040003A9 RID: 937
		private MovementOrder _followEntityOrder;

		// Token: 0x040003AA RID: 938
		private GameEntity _followedEntity;

		// Token: 0x040003AB RID: 939
		private MovementOrder _stopOrder;

		// Token: 0x040003AC RID: 940
		private BehaviorUseSiegeMachines.BehaviorState _behaviorState;

		// Token: 0x0200044E RID: 1102
		private enum BehaviorState
		{
			// Token: 0x040019A4 RID: 6564
			Unset,
			// Token: 0x040019A5 RID: 6565
			Follow,
			// Token: 0x040019A6 RID: 6566
			ClimbSiegeTower,
			// Token: 0x040019A7 RID: 6567
			Stop
		}
	}
}
