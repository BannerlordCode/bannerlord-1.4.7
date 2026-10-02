using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003E2 RID: 994
	public class MissionSiegeWeaponsController : IMissionSiegeWeaponsController
	{
		// Token: 0x060036B9 RID: 14009 RVA: 0x000E2D04 File Offset: 0x000E0F04
		public MissionSiegeWeaponsController(BattleSideEnum side, List<MissionSiegeWeapon> weapons)
		{
			this._side = side;
			this._weapons = weapons;
			this._undeployedWeapons = new List<MissionSiegeWeapon>(this._weapons);
			this._deployedWeapons = new Dictionary<DestructableComponent, MissionSiegeWeapon>();
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000E2D38 File Offset: 0x000E0F38
		public int GetMaxDeployableWeaponCount(Type t)
		{
			int num = 0;
			using (List<MissionSiegeWeapon>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (MissionSiegeWeaponsController.GetSiegeWeaponBaseType(enumerator.Current.Type) == t)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000E2D9C File Offset: 0x000E0F9C
		public IEnumerable<IMissionSiegeWeapon> GetSiegeWeapons()
		{
			return this._weapons.Cast<IMissionSiegeWeapon>();
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000E2DAC File Offset: 0x000E0FAC
		public void OnWeaponDeployed(SiegeWeapon missionWeapon)
		{
			SiegeEngineType missionWeaponType = missionWeapon.GetSiegeEngineType();
			int num = this._undeployedWeapons.FindIndex((MissionSiegeWeapon uw) => uw.Type == missionWeaponType);
			MissionSiegeWeapon missionSiegeWeapon = this._undeployedWeapons[num];
			DestructableComponent destructionComponent = missionWeapon.DestructionComponent;
			destructionComponent.MaxHitPoint = missionSiegeWeapon.MaxHealth;
			destructionComponent.HitPoint = missionSiegeWeapon.InitialHealth;
			destructionComponent.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
			destructionComponent.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
			this._undeployedWeapons.RemoveAt(num);
			this._deployedWeapons.Add(destructionComponent, missionSiegeWeapon);
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000E2E4C File Offset: 0x000E104C
		public void OnWeaponUndeployed(SiegeWeapon missionWeapon)
		{
			DestructableComponent destructionComponent = missionWeapon.DestructionComponent;
			MissionSiegeWeapon missionSiegeWeapon;
			this._deployedWeapons.TryGetValue(destructionComponent, out missionSiegeWeapon);
			SiegeEngineType siegeEngineType = missionWeapon.GetSiegeEngineType();
			destructionComponent.MaxHitPoint = (float)siegeEngineType.BaseHitPoints;
			destructionComponent.HitPoint = destructionComponent.MaxHitPoint;
			destructionComponent.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
			destructionComponent.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
			this._deployedWeapons.Remove(destructionComponent);
			this._undeployedWeapons.Add(missionSiegeWeapon);
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000E2ECC File Offset: 0x000E10CC
		private void OnWeaponHit(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			MissionSiegeWeapon missionSiegeWeapon;
			if (target.BattleSide == this._side && this._deployedWeapons.TryGetValue(target, out missionSiegeWeapon))
			{
				float num = Math.Max(0f, missionSiegeWeapon.Health - (float)inflictedDamage);
				missionSiegeWeapon.SetHealth(num);
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000E2F14 File Offset: 0x000E1114
		private void OnWeaponDestroyed(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			MissionSiegeWeapon missionSiegeWeapon;
			if (target.BattleSide == this._side && this._deployedWeapons.TryGetValue(target, out missionSiegeWeapon))
			{
				missionSiegeWeapon.SetHealth(0f);
				target.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
				target.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
				this._deployedWeapons.Remove(target);
			}
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000E2F7C File Offset: 0x000E117C
		public static Type GetWeaponType(ScriptComponentBehavior weapon)
		{
			if (weapon is UsableGameObjectGroup)
			{
				return weapon.GameEntity.GetChildren().SelectMany<WeakGameEntity, ScriptComponentBehavior>((WeakGameEntity c) => c.GetScriptComponents()).First<ScriptComponentBehavior>((ScriptComponentBehavior s) => s is IFocusable)
					.GetType();
			}
			return weapon.GetType();
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000E2FF4 File Offset: 0x000E11F4
		private static Type GetSiegeWeaponBaseType(SiegeEngineType siegeWeaponType)
		{
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
			{
				return typeof(SiegeLadder);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
			{
				return typeof(Ballista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
			{
				return typeof(FireBallista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ram)
			{
				return typeof(BatteringRam);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower)
			{
				return typeof(SiegeTower);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
			{
				return typeof(Mangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
			{
				return typeof(FireMangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet)
			{
				return typeof(Trebuchet);
			}
			return null;
		}

		// Token: 0x04001791 RID: 6033
		private readonly List<MissionSiegeWeapon> _weapons;

		// Token: 0x04001792 RID: 6034
		private readonly List<MissionSiegeWeapon> _undeployedWeapons;

		// Token: 0x04001793 RID: 6035
		private readonly Dictionary<DestructableComponent, MissionSiegeWeapon> _deployedWeapons;

		// Token: 0x04001794 RID: 6036
		private BattleSideEnum _side;
	}
}
