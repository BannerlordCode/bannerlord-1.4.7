using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000313 RID: 787
	public abstract class MPPerkEffectBase
	{
		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000AC828 File Offset: 0x000AAA28
		public virtual bool IsTickRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000AC82B File Offset: 0x000AAA2B
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000AC833 File Offset: 0x000AAA33
		public bool IsDisabledInWarmup { get; protected set; }

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000AC83C File Offset: 0x000AAA3C
		public virtual void OnUpdate(Agent agent, bool newState)
		{
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000AC840 File Offset: 0x000AAA40
		public virtual void OnTick(MissionPeer peer, int tickCount)
		{
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				MBReadOnlyList<IFormationUnit> mbreadOnlyList;
				if (peer == null)
				{
					mbreadOnlyList = null;
				}
				else
				{
					Formation controlledFormation = peer.ControlledFormation;
					mbreadOnlyList = ((controlledFormation != null) ? controlledFormation.Arrangement.GetAllUnits() : null);
				}
				MBReadOnlyList<IFormationUnit> mbreadOnlyList2 = mbreadOnlyList;
				if (mbreadOnlyList2 == null)
				{
					return;
				}
				using (List<IFormationUnit>.Enumerator enumerator = mbreadOnlyList2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent;
						if ((agent = enumerator.Current as Agent) != null && agent.IsActive())
						{
							this.OnTick(agent, tickCount);
						}
					}
					return;
				}
			}
			if (peer != null)
			{
				Agent controlledAgent = peer.ControlledAgent;
				bool? flag = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					this.OnTick(peer.ControlledAgent, tickCount);
				}
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000AC918 File Offset: 0x000AAB18
		public virtual void OnTick(Agent agent, int tickCount)
		{
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000AC91A File Offset: 0x000AAB1A
		public virtual float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			return 0f;
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000AC921 File Offset: 0x000AAB21
		public virtual float GetMountDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			return 0f;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000AC928 File Offset: 0x000AAB28
		public virtual float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000AC92F File Offset: 0x000AAB2F
		public virtual float GetMountDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000AC936 File Offset: 0x000AAB36
		public virtual float GetSpeedBonusEffectiveness(Agent attacker, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000AC93D File Offset: 0x000AAB3D
		public virtual float GetShieldDamage(bool isCorrectSideBlock)
		{
			return 0f;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000AC944 File Offset: 0x000AAB44
		public virtual float GetShieldDamageTaken(bool isCorrectSideBlock)
		{
			return 0f;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000AC94B File Offset: 0x000AAB4B
		public virtual float GetRangedAccuracy()
		{
			return 0f;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000AC952 File Offset: 0x000AAB52
		public virtual float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
		{
			return 0f;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000AC959 File Offset: 0x000AAB59
		public virtual float GetDamageInterruptionThreshold()
		{
			return 0f;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000AC960 File Offset: 0x000AAB60
		public virtual float GetMountManeuver()
		{
			return 0f;
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000AC967 File Offset: 0x000AAB67
		public virtual float GetMountSpeed()
		{
			return 0f;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000AC96E File Offset: 0x000AAB6E
		public virtual float GetRangedHeadShotDamage()
		{
			return 0f;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000AC975 File Offset: 0x000AAB75
		public virtual int GetGoldOnKill(float attackerValue, float victimValue)
		{
			return 0;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000AC978 File Offset: 0x000AAB78
		public virtual int GetGoldOnAssist()
		{
			return 0;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000AC97B File Offset: 0x000AAB7B
		public virtual int GetRewardedGoldOnAssist()
		{
			return 0;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000AC97E File Offset: 0x000AAB7E
		public virtual bool GetIsTeamRewardedOnDeath()
		{
			return false;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000AC981 File Offset: 0x000AAB81
		public virtual void CalculateRewardedGoldOnDeath(Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000AC983 File Offset: 0x000AAB83
		public virtual float GetDrivenPropertyBonus(DrivenProperty drivenProperty, float baseValue)
		{
			return 0f;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000AC98A File Offset: 0x000AAB8A
		public virtual float GetEncumbrance(bool isOnBody)
		{
			return 0f;
		}

		// Token: 0x06002CDD RID: 11485
		protected abstract void Deserialize(XmlNode node);
	}
}
