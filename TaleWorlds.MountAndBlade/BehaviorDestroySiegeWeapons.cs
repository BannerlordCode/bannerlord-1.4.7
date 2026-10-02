using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000115 RID: 277
	public class BehaviorDestroySiegeWeapons : BehaviorComponent
	{
		// Token: 0x06000E05 RID: 3589 RVA: 0x0001E88C File Offset: 0x0001CA8C
		private void DetermineTargetWeapons()
		{
			this._targetWeapons = this._allWeapons.Where<SiegeWeapon>((SiegeWeapon w) => w is IPrimarySiegeWeapon && (w as IPrimarySiegeWeapon).WeaponSide == this._behaviorSide && w.IsDestructible && !w.IsDestroyed && !w.IsDisabled).ToList<SiegeWeapon>();
			if (this._targetWeapons.IsEmpty<SiegeWeapon>())
			{
				this._targetWeapons = this._allWeapons.Where<SiegeWeapon>((SiegeWeapon w) => !(w is IPrimarySiegeWeapon) && w.IsDestructible && !w.IsDestroyed && !w.IsDisabled).ToList<SiegeWeapon>();
				this._isTargetPrimaryWeapon = false;
				return;
			}
			this._isTargetPrimaryWeapon = true;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0001E90C File Offset: 0x0001CB0C
		public BehaviorDestroySiegeWeapons(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0.2f;
			this._behaviorSide = formation.AI.Side;
			this._allWeapons = (from sw in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeWeapon>()
				where sw.Side != formation.Team.Side
				select sw).ToList<SiegeWeapon>();
			this.DetermineTargetWeapons();
			base.CurrentOrder = MovementOrder.MovementOrderCharge;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0001E990 File Offset: 0x0001CB90
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0001E9EA File Offset: 0x0001CBEA
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.DetermineTargetWeapons();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0001E9F8 File Offset: 0x0001CBF8
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			this._targetWeapons.RemoveAll((SiegeWeapon tw) => tw.IsDestroyed);
			if (this._targetWeapons.Count == 0)
			{
				this.DetermineTargetWeapons();
			}
			if (base.Formation.AI.ActiveBehavior == this)
			{
				if (this._targetWeapons.Count == 0)
				{
					MovementOrder currentOrder = base.CurrentOrder;
					if ((in currentOrder) != MovementOrder.MovementOrderCharge)
					{
						base.CurrentOrder = MovementOrder.MovementOrderCharge;
					}
					this._isTargetPrimaryWeapon = false;
				}
				else
				{
					SiegeWeapon siegeWeapon = this._targetWeapons.MinBy<SiegeWeapon, float>((SiegeWeapon tw) => base.Formation.CachedAveragePosition.DistanceSquared(tw.GameEntity.GlobalPosition.AsVec2));
					if (base.CurrentOrder.OrderEnum != MovementOrder.MovementOrderEnum.AttackEntity || this.LastTargetWeapon != siegeWeapon)
					{
						this.LastTargetWeapon = siegeWeapon;
						base.CurrentOrder = MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this.LastTargetWeapon.GameEntity), true);
					}
				}
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0001EAF8 File Offset: 0x0001CCF8
		protected override void OnBehaviorActivatedAux()
		{
			this.DetermineTargetWeapons();
			base.Formation.SetArrangementOrder((base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation) ? ArrangementOrder.ArrangementOrderSkein : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0001EB77 File Offset: 0x0001CD77
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0001EB7E File Offset: 0x0001CD7E
		protected override float GetAiWeight()
		{
			if (this._targetWeapons.IsEmpty<SiegeWeapon>())
			{
				return 0f;
			}
			if (!this._isTargetPrimaryWeapon)
			{
				return 0.7f;
			}
			return 1f;
		}

		// Token: 0x04000355 RID: 853
		private readonly List<SiegeWeapon> _allWeapons;

		// Token: 0x04000356 RID: 854
		private List<SiegeWeapon> _targetWeapons;

		// Token: 0x04000357 RID: 855
		public SiegeWeapon LastTargetWeapon;

		// Token: 0x04000358 RID: 856
		private bool _isTargetPrimaryWeapon;
	}
}
