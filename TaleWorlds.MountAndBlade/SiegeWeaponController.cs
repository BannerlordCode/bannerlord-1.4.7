using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000164 RID: 356
	public class SiegeWeaponController
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0003A28E File Offset: 0x0003848E
		public MBReadOnlyList<SiegeWeapon> SelectedWeapons
		{
			get
			{
				return this._selectedWeapons;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06001263 RID: 4707 RVA: 0x0003A298 File Offset: 0x00038498
		// (remove) Token: 0x06001264 RID: 4708 RVA: 0x0003A2D0 File Offset: 0x000384D0
		public event Action<SiegeWeaponOrderType, IEnumerable<SiegeWeapon>> OnOrderIssued;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001265 RID: 4709 RVA: 0x0003A308 File Offset: 0x00038508
		// (remove) Token: 0x06001266 RID: 4710 RVA: 0x0003A340 File Offset: 0x00038540
		public event Action OnSelectedSiegeWeaponsChanged;

		// Token: 0x06001267 RID: 4711 RVA: 0x0003A375 File Offset: 0x00038575
		public SiegeWeaponController(Mission mission, Team team)
		{
			this._mission = mission;
			this._team = team;
			this._selectedWeapons = new MBList<SiegeWeapon>();
			this.InitializeWeaponsForDeployment();
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0003A39C File Offset: 0x0003859C
		private void InitializeWeaponsForDeployment()
		{
			IEnumerable<SiegeWeapon> enumerable = from w in (from dp in this._mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>()
					where dp.Side == this._team.Side
					select dp).SelectMany<DeploymentPoint, SynchedMissionObject>((DeploymentPoint dp) => dp.DeployableWeapons)
				select w as SiegeWeapon;
			this._availableWeapons = enumerable.ToList<SiegeWeapon>();
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0003A420 File Offset: 0x00038620
		private void InitializeWeapons()
		{
			this._availableWeapons = new List<SiegeWeapon>();
			this._availableWeapons.AddRange(from w in this._mission.ActiveMissionObjects.FindAllWithType<RangedSiegeWeapon>()
				where w.Side == this._team.Side
				select w);
			if (this._team.Side == BattleSideEnum.Attacker)
			{
				this._availableWeapons.AddRange(from w in this._mission.ActiveMissionObjects.FindAllWithType<SiegeWeapon>()
					where w is IPrimarySiegeWeapon && !(w is RangedSiegeWeapon)
					select w);
			}
			this._availableWeapons.Sort((SiegeWeapon w1, SiegeWeapon w2) => this.GetShortcutIndexOf(w1).CompareTo(this.GetShortcutIndexOf(w2)));
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0003A4C8 File Offset: 0x000386C8
		public void Select(SiegeWeapon weapon)
		{
			if (this.SelectedWeapons.Contains(weapon) || !SiegeWeaponController.IsWeaponSelectable(weapon))
			{
				Debug.FailedAssert("Weapon already selected or is not selectable", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "Select", 82);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new SelectSiegeWeapon(weapon.Id));
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Add(weapon);
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0003A53F File Offset: 0x0003873F
		public void ClearSelectedWeapons()
		{
			bool isClient = GameNetwork.IsClient;
			this._selectedWeapons.Clear();
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0003A564 File Offset: 0x00038764
		public void Deselect(SiegeWeapon weapon)
		{
			if (!this.SelectedWeapons.Contains(weapon))
			{
				Debug.FailedAssert("Trying to deselect an unselected weapon", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "Deselect", 113);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new UnselectSiegeWeapon(weapon.Id));
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Remove(weapon);
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0003A5D4 File Offset: 0x000387D4
		public void SelectAll()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new SelectAllSiegeWeapons());
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Clear();
			foreach (SiegeWeapon siegeWeapon in this._availableWeapons)
			{
				if (SiegeWeaponController.IsWeaponSelectable(siegeWeapon))
				{
					this._selectedWeapons.Add(siegeWeapon);
				}
			}
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0003A66C File Offset: 0x0003886C
		public static bool IsWeaponSelectable(SiegeWeapon weapon)
		{
			return !weapon.IsDestroyed && !weapon.IsDeactivated;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0003A684 File Offset: 0x00038884
		public static SiegeWeaponOrderType GetActiveOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			if (!(weapon is RangedSiegeWeapon))
			{
				return SiegeWeaponOrderType.Attack;
			}
			switch (((RangedSiegeWeapon)weapon).Focus)
			{
			case RangedSiegeWeapon.FiringFocus.Troops:
				return SiegeWeaponOrderType.FireAtTroops;
			case RangedSiegeWeapon.FiringFocus.Walls:
				return SiegeWeaponOrderType.FireAtWalls;
			case RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons:
				return SiegeWeaponOrderType.FireAtRangedSiegeWeapons;
			case RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons:
				return SiegeWeaponOrderType.FireAtPrimarySiegeWeapons;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetActiveOrderOf", 169);
				return SiegeWeaponOrderType.FireAtTroops;
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003A6EB File Offset: 0x000388EB
		public static SiegeWeaponOrderType GetActiveMovementOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			return SiegeWeaponOrderType.Attack;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0003A6F8 File Offset: 0x000388F8
		public static SiegeWeaponOrderType GetActiveFacingOrderOf(SiegeWeapon weapon)
		{
			if (!(weapon is RangedSiegeWeapon))
			{
				return SiegeWeaponOrderType.FireAtWalls;
			}
			switch (((RangedSiegeWeapon)weapon).Focus)
			{
			case RangedSiegeWeapon.FiringFocus.Troops:
				return SiegeWeaponOrderType.FireAtTroops;
			case RangedSiegeWeapon.FiringFocus.Walls:
				return SiegeWeaponOrderType.FireAtWalls;
			case RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons:
				return SiegeWeaponOrderType.FireAtRangedSiegeWeapons;
			case RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons:
				return SiegeWeaponOrderType.FireAtPrimarySiegeWeapons;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetActiveFacingOrderOf", 207);
				return SiegeWeaponOrderType.FireAtTroops;
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0003A755 File Offset: 0x00038955
		public static SiegeWeaponOrderType GetActiveFiringOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			return SiegeWeaponOrderType.Attack;
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0003A762 File Offset: 0x00038962
		public static SiegeWeaponOrderType GetActiveAIControlOrderOf(SiegeWeapon weapon)
		{
			if (weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.AIControlOn;
			}
			return SiegeWeaponOrderType.AIControlOff;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0003A770 File Offset: 0x00038970
		private void SetOrderAux(SiegeWeaponOrderType order, SiegeWeapon weapon)
		{
			switch (order)
			{
			case SiegeWeaponOrderType.Stop:
			case SiegeWeaponOrderType.AIControlOff:
				weapon.SetForcedUse(false);
				return;
			case SiegeWeaponOrderType.Attack:
			case SiegeWeaponOrderType.AIControlOn:
				weapon.SetForcedUse(true);
				return;
			case SiegeWeaponOrderType.FireAtWalls:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon != null)
				{
					rangedSiegeWeapon.Focus = RangedSiegeWeapon.FiringFocus.Walls;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtTroops:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon2 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon2 != null)
				{
					rangedSiegeWeapon2.Focus = RangedSiegeWeapon.FiringFocus.Troops;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtRangedSiegeWeapons:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon3 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon3 != null)
				{
					rangedSiegeWeapon3.Focus = RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtPrimarySiegeWeapons:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon4 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon4 != null)
				{
					rangedSiegeWeapon4.Focus = RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons;
					return;
				}
				break;
			}
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "SetOrderAux", 297);
				break;
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0003A834 File Offset: 0x00038A34
		public void SetOrder(SiegeWeaponOrderType order)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplySiegeWeaponOrder(order));
				GameNetwork.EndModuleEventAsClient();
			}
			foreach (SiegeWeapon siegeWeapon in this.SelectedWeapons)
			{
				this.SetOrderAux(order, siegeWeapon);
			}
			Action<SiegeWeaponOrderType, IEnumerable<SiegeWeapon>> onOrderIssued = this.OnOrderIssued;
			if (onOrderIssued == null)
			{
				return;
			}
			onOrderIssued(order, this.SelectedWeapons);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0003A8BC File Offset: 0x00038ABC
		public int GetShortcutIndexOf(SiegeWeapon weapon)
		{
			FormationAI.BehaviorSide sideOf = SiegeWeaponController.GetSideOf(weapon);
			int num = ((sideOf == FormationAI.BehaviorSide.Left) ? 1 : ((sideOf == FormationAI.BehaviorSide.Right) ? 2 : 0));
			if (!(weapon is IPrimarySiegeWeapon))
			{
				num += 3;
			}
			return num;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0003A8EC File Offset: 0x00038AEC
		private static FormationAI.BehaviorSide GetSideOf(SiegeWeapon weapon)
		{
			IPrimarySiegeWeapon primarySiegeWeapon = weapon as IPrimarySiegeWeapon;
			if (primarySiegeWeapon != null)
			{
				return primarySiegeWeapon.WeaponSide;
			}
			if (weapon is RangedSiegeWeapon)
			{
				return FormationAI.BehaviorSide.Middle;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetSideOf", 349);
			return FormationAI.BehaviorSide.Middle;
		}

		// Token: 0x0400048A RID: 1162
		private readonly Mission _mission;

		// Token: 0x0400048B RID: 1163
		private readonly Team _team;

		// Token: 0x0400048C RID: 1164
		private List<SiegeWeapon> _availableWeapons;

		// Token: 0x0400048D RID: 1165
		private MBList<SiegeWeapon> _selectedWeapons;
	}
}
