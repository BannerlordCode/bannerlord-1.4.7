using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000315 RID: 789
	public class MPPerkObject : IReadOnlyPerkObject
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002CED RID: 11501 RVA: 0x000AC999 File Offset: 0x000AAB99
		public TextObject Name
		{
			get
			{
				return new TextObject(this._name, null);
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002CEE RID: 11502 RVA: 0x000AC9A7 File Offset: 0x000AABA7
		public TextObject Description
		{
			get
			{
				return new TextObject(this._description, null);
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002CEF RID: 11503 RVA: 0x000AC9B5 File Offset: 0x000AABB5
		public bool HasBannerBearer { get; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000AC9BD File Offset: 0x000AABBD
		public List<string> GameModes { get; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x000AC9C5 File Offset: 0x000AABC5
		public int PerkListIndex { get; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000AC9CD File Offset: 0x000AABCD
		public string IconId { get; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000AC9D5 File Offset: 0x000AABD5
		public string HeroIdleAnimOverride { get; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x000AC9DD File Offset: 0x000AABDD
		public string HeroMountIdleAnimOverride { get; }

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000AC9E5 File Offset: 0x000AABE5
		public string TroopIdleAnimOverride { get; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000AC9ED File Offset: 0x000AABED
		public string TroopMountIdleAnimOverride { get; }

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000AC9F8 File Offset: 0x000AABF8
		public MPPerkObject(MissionPeer peer, string name, string description, List<string> gameModes, int perkListIndex, string iconId, IEnumerable<MPConditionalEffect> conditionalEffects, IEnumerable<MPPerkEffectBase> effects, string heroIdleAnimOverride, string heroMountIdleAnimOverride, string troopIdleAnimOverride, string troopMountIdleAnimOverride)
		{
			this._peer = peer;
			this._name = name;
			this._description = description;
			this.GameModes = gameModes;
			this.PerkListIndex = perkListIndex;
			this.IconId = iconId;
			this._conditionalEffects = new MPConditionalEffect.ConditionalEffectContainer(conditionalEffects);
			this._effects = new List<MPPerkEffectBase>(effects);
			this.HeroIdleAnimOverride = heroIdleAnimOverride;
			this.HeroMountIdleAnimOverride = heroMountIdleAnimOverride;
			this.TroopIdleAnimOverride = troopIdleAnimOverride;
			this.TroopMountIdleAnimOverride = troopMountIdleAnimOverride;
			this._perkEventFlags = MPPerkCondition.PerkEventFlags.None;
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				foreach (MPPerkCondition mpperkCondition in mpconditionalEffect.Conditions)
				{
					this._perkEventFlags |= mpperkCondition.EventFlags;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect2 in this._conditionalEffects)
			{
				using (List<MPPerkCondition>.Enumerator enumerator2 = mpconditionalEffect2.Conditions.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current is BannerBearerCondition)
						{
							this.HasBannerBearer = true;
						}
					}
				}
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000ACB88 File Offset: 0x000AAD88
		private MPPerkObject(XmlNode node)
		{
			this._peer = null;
			this._conditionalEffects = new MPConditionalEffect.ConditionalEffectContainer();
			this._effects = new List<MPPerkEffectBase>();
			this._name = node.Attributes["name"].Value;
			this._description = node.Attributes["description"].Value;
			this.GameModes = new List<string>(node.Attributes["game_mode"].Value.Split(new char[] { ',' }));
			for (int i = 0; i < this.GameModes.Count; i++)
			{
				this.GameModes[i] = this.GameModes[i].Trim();
			}
			this.IconId = node.Attributes["icon"].Value;
			this.PerkListIndex = 0;
			XmlNode xmlNode = node.Attributes["perk_list"];
			if (xmlNode != null)
			{
				this.PerkListIndex = Convert.ToInt32(xmlNode.Value);
				int perkListIndex = this.PerkListIndex;
				this.PerkListIndex = perkListIndex - 1;
			}
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.NodeType != XmlNodeType.SignificantWhitespace)
				{
					if (xmlNode2.Name == "ConditionalEffect")
					{
						this._conditionalEffects.Add(new MPConditionalEffect(this.GameModes, xmlNode2));
					}
					else if (xmlNode2.Name == "Effect")
					{
						this._effects.Add(MPPerkEffect.CreateFrom(xmlNode2));
					}
					else if (xmlNode2.Name == "OnSpawnEffect")
					{
						this._effects.Add(MPOnSpawnPerkEffect.CreateFrom(xmlNode2));
					}
					else if (xmlNode2.Name == "RandomOnSpawnEffect")
					{
						this._effects.Add(MPRandomOnSpawnPerkEffect.CreateFrom(xmlNode2));
					}
					else
					{
						Debug.FailedAssert("Unknown child element", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPPerkObject.cs", ".ctor", 750);
					}
				}
			}
			XmlAttribute xmlAttribute = node.Attributes["hero_idle_anim"];
			this.HeroIdleAnimOverride = ((xmlAttribute != null) ? xmlAttribute.Value : null);
			XmlAttribute xmlAttribute2 = node.Attributes["hero_mount_idle_anim"];
			this.HeroMountIdleAnimOverride = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			XmlAttribute xmlAttribute3 = node.Attributes["troop_idle_anim"];
			this.TroopIdleAnimOverride = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
			XmlAttribute xmlAttribute4 = node.Attributes["troop_mount_idle_anim"];
			this.TroopMountIdleAnimOverride = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
			this._perkEventFlags = MPPerkCondition.PerkEventFlags.None;
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				foreach (MPPerkCondition mpperkCondition in mpconditionalEffect.Conditions)
				{
					this._perkEventFlags |= mpperkCondition.EventFlags;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect2 in this._conditionalEffects)
			{
				using (List<MPPerkCondition>.Enumerator enumerator3 = mpconditionalEffect2.Conditions.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current is BannerBearerCondition)
						{
							this.HasBannerBearer = true;
						}
					}
				}
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000ACF70 File Offset: 0x000AB170
		public MPPerkObject Clone(MissionPeer peer)
		{
			return new MPPerkObject(peer, this._name, this._description, this.GameModes, this.PerkListIndex, this.IconId, this._conditionalEffects, this._effects, this.HeroIdleAnimOverride, this.HeroMountIdleAnimOverride, this.TroopIdleAnimOverride, this.TroopMountIdleAnimOverride);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000ACFC5 File Offset: 0x000AB1C5
		public void Reset()
		{
			this._conditionalEffects.ResetStates();
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000ACFD4 File Offset: 0x000AB1D4
		private void OnEvent(bool isWarmup, MPPerkCondition.PerkEventFlags flags)
		{
			if ((flags & this._perkEventFlags) != MPPerkCondition.PerkEventFlags.None)
			{
				foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
				{
					if ((flags & mpconditionalEffect.EventFlags) != MPPerkCondition.PerkEventFlags.None)
					{
						mpconditionalEffect.OnEvent(isWarmup, this._peer, this._conditionalEffects);
					}
				}
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000AD048 File Offset: 0x000AB248
		private void OnEvent(bool isWarmup, Agent agent, MPPerkCondition.PerkEventFlags flags)
		{
			if (((agent != null) ? agent.MissionPeer : null) == null && agent != null)
			{
				MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
			}
			if ((flags & this._perkEventFlags) != MPPerkCondition.PerkEventFlags.None)
			{
				foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
				{
					if ((flags & mpconditionalEffect.EventFlags) != MPPerkCondition.PerkEventFlags.None)
					{
						mpconditionalEffect.OnEvent(isWarmup, agent, this._conditionalEffects);
					}
				}
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000AD0D0 File Offset: 0x000AB2D0
		private void OnTick(bool isWarmup, int tickCount)
		{
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.IsTickRequired)
				{
					mpconditionalEffect.OnTick(isWarmup, this._peer, tickCount);
				}
			}
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if ((!isWarmup || !mpperkEffectBase.IsDisabledInWarmup) && mpperkEffectBase.IsTickRequired)
				{
					mpperkEffectBase.OnTick(this._peer, tickCount);
				}
			}
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000AD190 File Offset: 0x000AB390
		private float GetDamage(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamage(attackerWeapon, damageType, isAlternativeAttack);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamage(attackerWeapon, damageType, isAlternativeAttack);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000AD2B4 File Offset: 0x000AB4B4
		private float GetMountDamage(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountDamage(attackerWeapon, damageType, isAlternativeAttack);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountDamage(attackerWeapon, damageType, isAlternativeAttack);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000AD3D8 File Offset: 0x000AB5D8
		private float GetDamageTaken(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamageTaken(attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamageTaken(attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000AD4F8 File Offset: 0x000AB6F8
		private float GetMountDamageTaken(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountDamageTaken(attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountDamageTaken(attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000AD618 File Offset: 0x000AB818
		private float GetSpeedBonusEffectiveness(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetSpeedBonusEffectiveness(agent, attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetSpeedBonusEffectiveness(agent, attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000AD73C File Offset: 0x000AB93C
		private float GetShieldDamage(bool isWarmup, Agent attacker, Agent defender, bool isCorrectSideBlock)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetShieldDamage(isCorrectSideBlock);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(attacker))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetShieldDamage(isCorrectSideBlock);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000AD840 File Offset: 0x000ABA40
		private float GetShieldDamageTaken(bool isWarmup, Agent attacker, Agent defender, bool isCorrectSideBlock)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetShieldDamageTaken(isCorrectSideBlock);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(defender))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetShieldDamageTaken(isCorrectSideBlock);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000AD944 File Offset: 0x000ABB44
		private float GetRangedAccuracy(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRangedAccuracy();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRangedAccuracy();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000ADA60 File Offset: 0x000ABC60
		private float GetThrowingWeaponSpeed(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetThrowingWeaponSpeed(attackerWeapon);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetThrowingWeaponSpeed(attackerWeapon);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000ADB7C File Offset: 0x000ABD7C
		private float GetDamageInterruptionThreshold(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamageInterruptionThreshold();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamageInterruptionThreshold();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000ADC98 File Offset: 0x000ABE98
		private float GetMountManeuver(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountManeuver();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountManeuver();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000ADDB4 File Offset: 0x000ABFB4
		private float GetMountSpeed(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountSpeed();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountSpeed();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000ADED0 File Offset: 0x000AC0D0
		private float GetRangedHeadShotDamage(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRangedHeadShotDamage();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRangedHeadShotDamage();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000ADFEC File Offset: 0x000AC1EC
		public int GetExtraTroopCount(bool isWarmup)
		{
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetExtraTroopCount();
				}
			}
			return num;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000AE05C File Offset: 0x000AC25C
		public List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isWarmup, bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAllEquipments = false)
		{
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					alternativeEquipments = onSpawnPerkEffect.GetAlternativeEquipments(isPlayer, alternativeEquipments, getAllEquipments);
				}
			}
			return alternativeEquipments;
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000AE0CC File Offset: 0x000AC2CC
		private float GetDrivenPropertyBonus(bool isWarmup, Agent agent, DrivenProperty drivenProperty, float baseValue)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDrivenPropertyBonus(drivenProperty, baseValue);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDrivenPropertyBonus(drivenProperty, baseValue);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000AE1EC File Offset: 0x000AC3EC
		public float GetDrivenPropertyBonusOnSpawn(bool isWarmup, bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetDrivenPropertyBonusOnSpawn(isPlayer, drivenProperty, baseValue);
				}
			}
			return num;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000AE264 File Offset: 0x000AC464
		public float GetHitpoints(bool isWarmup, bool isPlayer)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetHitpoints(isPlayer);
				}
			}
			return num;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000AE2D8 File Offset: 0x000AC4D8
		private float GetEncumbrance(bool isWarmup, Agent agent, bool isOnBody)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetEncumbrance(isOnBody);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetEncumbrance(isOnBody);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000AE3F4 File Offset: 0x000AC5F4
		private int GetGoldOnKill(bool isWarmup, Agent agent, float attackerValue, float victimValue)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetGoldOnKill(attackerValue, victimValue);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetGoldOnKill(attackerValue, victimValue);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000AE510 File Offset: 0x000AC710
		private int GetGoldOnAssist(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetGoldOnAssist();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetGoldOnAssist();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000AE628 File Offset: 0x000AC828
		private int GetRewardedGoldOnAssist(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRewardedGoldOnAssist();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRewardedGoldOnAssist();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000AE740 File Offset: 0x000AC940
		private bool GetIsTeamRewardedOnDeath(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if ((!isWarmup || !mpperkEffectBase.IsDisabledInWarmup) && mpperkEffectBase.GetIsTeamRewardedOnDeath())
				{
					return true;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if ((!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup) && mpperkEffectBase2.GetIsTeamRewardedOnDeath())
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000AE860 File Offset: 0x000ACA60
		private void CalculateRewardedGoldOnDeath(bool isWarmup, Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			teamMembers.Shuffle<ValueTuple<MissionPeer, int>>();
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					mpperkEffectBase.CalculateRewardedGoldOnDeath(agent, teamMembers);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							mpperkEffectBase2.CalculateRewardedGoldOnDeath(agent, teamMembers);
						}
					}
				}
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000AE974 File Offset: 0x000ACB74
		public static int GetTroopCount(MultiplayerClassDivisions.MPHeroClass heroClass, int botsPerFormation, MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler)
		{
			int num = MathF.Ceiling((float)botsPerFormation * heroClass.TroopMultiplier - 1E-05f);
			if (onSpawnPerkHandler != null)
			{
				num += (int)onSpawnPerkHandler.GetExtraTroopCount();
			}
			return MathF.Max(num, 1);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000AE9AA File Offset: 0x000ACBAA
		public static IReadOnlyPerkObject Deserialize(XmlNode node)
		{
			return new MPPerkObject(node);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000AE9B4 File Offset: 0x000ACBB4
		public static MPPerkObject.MPPerkHandler GetPerkHandler(Agent agent)
		{
			object obj;
			if (agent == null)
			{
				obj = null;
			}
			else
			{
				MissionPeer missionPeer = agent.MissionPeer;
				obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (agent == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
					obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = obj2;
			if (mbreadOnlyList != null && mbreadOnlyList.Count > 0 && !agent.IsMount)
			{
				return new MPPerkObject.MPPerkHandlerInstance(agent);
			}
			return null;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000AEA14 File Offset: 0x000ACC14
		public static MPPerkObject.MPPerkHandler GetPerkHandler(MissionPeer peer)
		{
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = ((peer != null) ? peer.SelectedPerks : null) ?? ((peer != null) ? peer.SelectedPerks : null);
			if (mbreadOnlyList != null && mbreadOnlyList.Count > 0)
			{
				return new MPPerkObject.MPPerkHandlerInstance(peer);
			}
			return null;
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000AEA54 File Offset: 0x000ACC54
		public static MPPerkObject.MPCombatPerkHandler GetCombatPerkHandler(Agent attacker, Agent defender)
		{
			Agent agent = ((attacker != null && attacker.IsMount) ? attacker.RiderAgent : attacker);
			object obj;
			if (agent == null)
			{
				obj = null;
			}
			else
			{
				MissionPeer missionPeer = agent.MissionPeer;
				obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (agent == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
					obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = obj2;
			Agent agent2 = ((defender != null && defender.IsMount) ? defender.RiderAgent : defender);
			object obj3;
			if (agent2 == null)
			{
				obj3 = null;
			}
			else
			{
				MissionPeer missionPeer2 = agent2.MissionPeer;
				obj3 = ((missionPeer2 != null) ? missionPeer2.SelectedPerks : null);
			}
			object obj4;
			if ((obj4 = obj3) == null)
			{
				if (agent2 == null)
				{
					obj4 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer2 = agent2.OwningAgentMissionPeer;
					obj4 = ((owningAgentMissionPeer2 != null) ? owningAgentMissionPeer2.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList2 = obj4;
			if (attacker != defender && ((mbreadOnlyList != null && mbreadOnlyList.Count > 0) || (mbreadOnlyList2 != null && mbreadOnlyList2.Count > 0)))
			{
				return new MPPerkObject.MPCombatPerkHandlerInstance(attacker, defender);
			}
			return null;
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000AEB1A File Offset: 0x000ACD1A
		public static MPPerkObject.MPOnSpawnPerkHandler GetOnSpawnPerkHandler(MissionPeer peer)
		{
			if ((((peer != null) ? peer.SelectedPerks : null) ?? ((peer != null) ? peer.SelectedPerks : null)) != null)
			{
				return new MPPerkObject.MPOnSpawnPerkHandlerInstance(peer);
			}
			return null;
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000AEB42 File Offset: 0x000ACD42
		public static MPPerkObject.MPOnSpawnPerkHandler GetOnSpawnPerkHandler(IEnumerable<IReadOnlyPerkObject> perks)
		{
			if (perks != null)
			{
				return new MPPerkObject.MPOnSpawnPerkHandlerInstance(perks);
			}
			return null;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000AEB50 File Offset: 0x000ACD50
		public static void RaiseEventForAllPeers(MPPerkCondition.PerkEventFlags flags)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(networkCommunicator.GetComponent<MissionPeer>());
					if (perkHandler != null)
					{
						perkHandler.OnEvent(flags);
					}
				}
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000AEBB8 File Offset: 0x000ACDB8
		public static void RaiseEventForAllPeersOnTeam(Team side, MPPerkCondition.PerkEventFlags flags)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team == side)
					{
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(component);
						if (perkHandler != null)
						{
							perkHandler.OnEvent(flags);
						}
					}
				}
			}
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000AEC30 File Offset: 0x000ACE30
		public static void TickAllPeerPerks(int tickCount)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team != null && component.Culture != null && component.Team.Side != BattleSideEnum.None)
					{
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(component);
						if (perkHandler != null)
						{
							perkHandler.OnTick(tickCount);
						}
					}
				}
			}
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000AECBC File Offset: 0x000ACEBC
		[CommandLineFunctionality.CommandLineArgumentFunction("raise_event", "mp_perks")]
		public static string RaiseEventForAllPeersCommand(List<string> strings)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				MPPerkCondition.PerkEventFlags perkEventFlags = MPPerkCondition.PerkEventFlags.None;
				using (List<string>.Enumerator enumerator = strings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MPPerkCondition.PerkEventFlags perkEventFlags2;
						if (Enum.TryParse<MPPerkCondition.PerkEventFlags>(enumerator.Current, true, out perkEventFlags2))
						{
							perkEventFlags |= perkEventFlags2;
						}
					}
				}
				MPPerkObject.RaiseEventForAllPeers(perkEventFlags);
				return "Raised event with flags " + perkEventFlags;
			}
			return "Can't run this command on clients";
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000AED38 File Offset: 0x000ACF38
		[CommandLineFunctionality.CommandLineArgumentFunction("tick_perks", "mp_perks")]
		public static string TickAllPeerPerksCommand(List<string> strings)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				int num;
				if (strings.Count == 0 || !int.TryParse(strings[0], out num))
				{
					num = 1;
				}
				MPPerkObject.TickAllPeerPerks(num);
				return "Peer perks on tick with tick count " + num;
			}
			return "Can't run this command on clients";
		}

		// Token: 0x040011BC RID: 4540
		private readonly MissionPeer _peer;

		// Token: 0x040011BD RID: 4541
		private readonly MPConditionalEffect.ConditionalEffectContainer _conditionalEffects;

		// Token: 0x040011BE RID: 4542
		private readonly MPPerkCondition.PerkEventFlags _perkEventFlags;

		// Token: 0x040011BF RID: 4543
		private readonly string _name;

		// Token: 0x040011C0 RID: 4544
		private readonly string _description;

		// Token: 0x040011C1 RID: 4545
		private readonly List<MPPerkEffectBase> _effects;

		// Token: 0x020005FB RID: 1531
		private class MPOnSpawnPerkHandlerInstance : MPPerkObject.MPOnSpawnPerkHandler
		{
			// Token: 0x06003F48 RID: 16200 RVA: 0x000F6835 File Offset: 0x000F4A35
			public MPOnSpawnPerkHandlerInstance(IEnumerable<IReadOnlyPerkObject> perks)
				: base(perks)
			{
			}

			// Token: 0x06003F49 RID: 16201 RVA: 0x000F683E File Offset: 0x000F4A3E
			public MPOnSpawnPerkHandlerInstance(MissionPeer peer)
				: base(peer)
			{
			}
		}

		// Token: 0x020005FC RID: 1532
		private class MPPerkHandlerInstance : MPPerkObject.MPPerkHandler
		{
			// Token: 0x06003F4A RID: 16202 RVA: 0x000F6847 File Offset: 0x000F4A47
			public MPPerkHandlerInstance(Agent agent)
				: base(agent)
			{
			}

			// Token: 0x06003F4B RID: 16203 RVA: 0x000F6850 File Offset: 0x000F4A50
			public MPPerkHandlerInstance(MissionPeer peer)
				: base(peer)
			{
			}
		}

		// Token: 0x020005FD RID: 1533
		private class MPCombatPerkHandlerInstance : MPPerkObject.MPCombatPerkHandler
		{
			// Token: 0x06003F4C RID: 16204 RVA: 0x000F6859 File Offset: 0x000F4A59
			public MPCombatPerkHandlerInstance(Agent attacker, Agent defender)
				: base(attacker, defender)
			{
			}
		}

		// Token: 0x020005FE RID: 1534
		public class MPOnSpawnPerkHandler
		{
			// Token: 0x17000AA4 RID: 2724
			// (get) Token: 0x06003F4D RID: 16205 RVA: 0x000F6864 File Offset: 0x000F4A64
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x06003F4E RID: 16206 RVA: 0x000F68CA File Offset: 0x000F4ACA
			protected MPOnSpawnPerkHandler(IEnumerable<IReadOnlyPerkObject> perks)
			{
				this._perks = perks;
			}

			// Token: 0x06003F4F RID: 16207 RVA: 0x000F68D9 File Offset: 0x000F4AD9
			protected MPOnSpawnPerkHandler(MissionPeer peer)
			{
				this._perks = peer.SelectedPerks;
			}

			// Token: 0x06003F50 RID: 16208 RVA: 0x000F68F0 File Offset: 0x000F4AF0
			public float GetExtraTroopCount()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						num += (float)readOnlyPerkObject.GetExtraTroopCount(isWarmup);
					}
				}
				return num;
			}

			// Token: 0x06003F51 RID: 16209 RVA: 0x000F6954 File Offset: 0x000F4B54
			public IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer)
			{
				List<ValueTuple<EquipmentIndex, EquipmentElement>> list = null;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						list = readOnlyPerkObject.GetAlternativeEquipments(isWarmup, isPlayer, list, false);
					}
				}
				return list;
			}

			// Token: 0x06003F52 RID: 16210 RVA: 0x000F69B4 File Offset: 0x000F4BB4
			public float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						num += readOnlyPerkObject.GetDrivenPropertyBonusOnSpawn(isWarmup, isPlayer, drivenProperty, baseValue);
					}
				}
				return num;
			}

			// Token: 0x06003F53 RID: 16211 RVA: 0x000F6A18 File Offset: 0x000F4C18
			public float GetHitpoints(bool isPlayer)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					num += readOnlyPerkObject.GetHitpoints(isWarmup, isPlayer);
				}
				return num;
			}

			// Token: 0x04002022 RID: 8226
			private IEnumerable<IReadOnlyPerkObject> _perks;
		}

		// Token: 0x020005FF RID: 1535
		public class MPPerkHandler
		{
			// Token: 0x17000AA5 RID: 2725
			// (get) Token: 0x06003F54 RID: 16212 RVA: 0x000F6A78 File Offset: 0x000F4C78
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x06003F55 RID: 16213 RVA: 0x000F6AE0 File Offset: 0x000F4CE0
			protected MPPerkHandler(Agent agent)
			{
				this._agent = agent;
				Agent agent2 = this._agent;
				object obj;
				if (agent2 == null)
				{
					obj = null;
				}
				else
				{
					MissionPeer missionPeer = agent2.MissionPeer;
					obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					Agent agent3 = this._agent;
					if (agent3 == null)
					{
						obj2 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer = agent3.OwningAgentMissionPeer;
						obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
					}
				}
				this._perks = obj2 ?? new MBList<MPPerkObject>();
			}

			// Token: 0x06003F56 RID: 16214 RVA: 0x000F6B49 File Offset: 0x000F4D49
			protected MPPerkHandler(MissionPeer peer)
			{
				this._agent = ((peer != null) ? peer.ControlledAgent : null);
				this._perks = ((peer != null) ? peer.SelectedPerks : null) ?? new MBList<MPPerkObject>();
			}

			// Token: 0x06003F57 RID: 16215 RVA: 0x000F6B80 File Offset: 0x000F4D80
			public void OnEvent(MPPerkCondition.PerkEventFlags flags)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnEvent(isWarmup, flags);
				}
			}

			// Token: 0x06003F58 RID: 16216 RVA: 0x000F6BDC File Offset: 0x000F4DDC
			public void OnEvent(Agent agent, MPPerkCondition.PerkEventFlags flags)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnEvent(isWarmup, agent, flags);
				}
			}

			// Token: 0x06003F59 RID: 16217 RVA: 0x000F6C38 File Offset: 0x000F4E38
			public void OnTick(int tickCount)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnTick(isWarmup, tickCount);
				}
			}

			// Token: 0x06003F5A RID: 16218 RVA: 0x000F6C94 File Offset: 0x000F4E94
			public float GetDrivenPropertyBonus(DrivenProperty drivenProperty, float baseValue)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetDrivenPropertyBonus(isWarmup, this._agent, drivenProperty, baseValue);
				}
				return num;
			}

			// Token: 0x06003F5B RID: 16219 RVA: 0x000F6D00 File Offset: 0x000F4F00
			public float GetRangedAccuracy()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetRangedAccuracy(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F5C RID: 16220 RVA: 0x000F6D6C File Offset: 0x000F4F6C
			public float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetThrowingWeaponSpeed(isWarmup, this._agent, attackerWeapon);
				}
				return num;
			}

			// Token: 0x06003F5D RID: 16221 RVA: 0x000F6DD8 File Offset: 0x000F4FD8
			public float GetDamageInterruptionThreshold()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetDamageInterruptionThreshold(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F5E RID: 16222 RVA: 0x000F6E44 File Offset: 0x000F5044
			public float GetMountManeuver()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetMountManeuver(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F5F RID: 16223 RVA: 0x000F6EB0 File Offset: 0x000F50B0
			public float GetMountSpeed()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetMountSpeed(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F60 RID: 16224 RVA: 0x000F6F1C File Offset: 0x000F511C
			public int GetGoldOnKill(float attackerValue, float victimValue)
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetGoldOnKill(isWarmup, this._agent, attackerValue, victimValue);
				}
				return num;
			}

			// Token: 0x06003F61 RID: 16225 RVA: 0x000F6F84 File Offset: 0x000F5184
			public int GetGoldOnAssist()
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetGoldOnAssist(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F62 RID: 16226 RVA: 0x000F6FEC File Offset: 0x000F51EC
			public int GetRewardedGoldOnAssist()
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetRewardedGoldOnAssist(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06003F63 RID: 16227 RVA: 0x000F7054 File Offset: 0x000F5254
			public bool GetIsTeamRewardedOnDeath()
			{
				bool isWarmup = this.IsWarmup;
				using (List<MPPerkObject>.Enumerator enumerator = this._perks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetIsTeamRewardedOnDeath(isWarmup, this._agent))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06003F64 RID: 16228 RVA: 0x000F70BC File Offset: 0x000F52BC
			public IEnumerable<ValueTuple<MissionPeer, int>> GetTeamGoldRewardsOnDeath()
			{
				if (this.GetIsTeamRewardedOnDeath())
				{
					Agent agent = this._agent;
					MissionPeer missionPeer;
					if ((missionPeer = ((agent != null) ? agent.MissionPeer : null)) == null)
					{
						Agent agent2 = this._agent;
						missionPeer = ((agent2 != null) ? agent2.OwningAgentMissionPeer : null);
					}
					MissionPeer missionPeer2 = missionPeer;
					List<ValueTuple<MissionPeer, int>> list = new List<ValueTuple<MissionPeer, int>>();
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						if (component != missionPeer2 && component.Team == missionPeer2.Team)
						{
							list.Add(new ValueTuple<MissionPeer, int>(component, 0));
						}
					}
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._perks)
					{
						mpperkObject.CalculateRewardedGoldOnDeath(isWarmup, this._agent, list);
					}
					return list;
				}
				return null;
			}

			// Token: 0x06003F65 RID: 16229 RVA: 0x000F71BC File Offset: 0x000F53BC
			public float GetEncumbrance(bool isOnBody)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetEncumbrance(isWarmup, this._agent, isOnBody);
				}
				return num;
			}

			// Token: 0x04002023 RID: 8227
			private readonly Agent _agent;

			// Token: 0x04002024 RID: 8228
			private readonly MBReadOnlyList<MPPerkObject> _perks;
		}

		// Token: 0x02000600 RID: 1536
		public class MPCombatPerkHandler
		{
			// Token: 0x17000AA6 RID: 2726
			// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000F7228 File Offset: 0x000F5428
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x06003F67 RID: 16231 RVA: 0x000F7290 File Offset: 0x000F5490
			protected MPCombatPerkHandler(Agent attacker, Agent defender)
			{
				this._attacker = attacker;
				this._defender = defender;
				attacker = ((attacker != null && attacker.IsMount) ? attacker.RiderAgent : attacker);
				defender = ((defender != null && defender.IsMount) ? defender.RiderAgent : defender);
				MBList<MPPerkObject> mblist;
				if (attacker == null)
				{
					mblist = null;
				}
				else
				{
					MissionPeer missionPeer = attacker.MissionPeer;
					mblist = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
				}
				MBList<MPPerkObject> mblist2;
				if ((mblist2 = mblist) == null)
				{
					MBList<MPPerkObject> mblist3;
					if (attacker == null)
					{
						mblist3 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer = attacker.OwningAgentMissionPeer;
						mblist3 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
					}
					mblist2 = mblist3 ?? new MBList<MPPerkObject>();
				}
				this._attackerPerks = mblist2;
				MBList<MPPerkObject> mblist4;
				if (defender == null)
				{
					mblist4 = null;
				}
				else
				{
					MissionPeer missionPeer2 = defender.MissionPeer;
					mblist4 = ((missionPeer2 != null) ? missionPeer2.SelectedPerks : null);
				}
				MBList<MPPerkObject> mblist5;
				if ((mblist5 = mblist4) == null)
				{
					MBList<MPPerkObject> mblist6;
					if (defender == null)
					{
						mblist6 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer2 = defender.OwningAgentMissionPeer;
						mblist6 = ((owningAgentMissionPeer2 != null) ? owningAgentMissionPeer2.SelectedPerks : null);
					}
					mblist5 = mblist6 ?? new MBList<MPPerkObject>();
				}
				this._defenderPerks = mblist5;
			}

			// Token: 0x06003F68 RID: 16232 RVA: 0x000F7364 File Offset: 0x000F5564
			public float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
			{
				float num = 0f;
				if (this._attackerPerks.Count > 0 && this._defender != null)
				{
					bool isWarmup = this.IsWarmup;
					if (this._defender.IsMount)
					{
						foreach (MPPerkObject mpperkObject in this._attackerPerks)
						{
							num += mpperkObject.GetMountDamage(isWarmup, this._attacker, attackerWeapon, damageType, isAlternativeAttack);
						}
					}
					foreach (MPPerkObject mpperkObject2 in this._attackerPerks)
					{
						num += mpperkObject2.GetDamage(isWarmup, this._attacker, attackerWeapon, damageType, isAlternativeAttack);
					}
				}
				return num;
			}

			// Token: 0x06003F69 RID: 16233 RVA: 0x000F744C File Offset: 0x000F564C
			public float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
			{
				float num = 0f;
				if (this._defenderPerks.Count > 0)
				{
					bool isWarmup = this.IsWarmup;
					if (this._defender.IsMount)
					{
						using (List<MPPerkObject>.Enumerator enumerator = this._defenderPerks.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MPPerkObject mpperkObject = enumerator.Current;
								num += mpperkObject.GetMountDamageTaken(isWarmup, this._defender, attackerWeapon, damageType);
							}
							return num;
						}
					}
					foreach (MPPerkObject mpperkObject2 in this._defenderPerks)
					{
						num += mpperkObject2.GetDamageTaken(isWarmup, this._defender, attackerWeapon, damageType);
					}
				}
				return num;
			}

			// Token: 0x06003F6A RID: 16234 RVA: 0x000F7528 File Offset: 0x000F5728
			public float GetSpeedBonusEffectiveness(WeaponComponentData attackerWeapon, DamageTypes damageType)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._attackerPerks)
				{
					num += mpperkObject.GetSpeedBonusEffectiveness(isWarmup, this._attacker, attackerWeapon, damageType);
				}
				return num;
			}

			// Token: 0x06003F6B RID: 16235 RVA: 0x000F7594 File Offset: 0x000F5794
			public float GetShieldDamage(bool isCorrectSideBlock)
			{
				float num = 0f;
				if (this._defender != null)
				{
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._attackerPerks)
					{
						num += mpperkObject.GetShieldDamage(isWarmup, this._attacker, this._defender, isCorrectSideBlock);
					}
				}
				return num;
			}

			// Token: 0x06003F6C RID: 16236 RVA: 0x000F7610 File Offset: 0x000F5810
			public float GetShieldDamageTaken(bool isCorrectSideBlock)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._defenderPerks)
				{
					num += mpperkObject.GetShieldDamageTaken(isWarmup, this._attacker, this._defender, isCorrectSideBlock);
				}
				return num;
			}

			// Token: 0x06003F6D RID: 16237 RVA: 0x000F7684 File Offset: 0x000F5884
			public float GetRangedHeadShotDamage()
			{
				float num = 0f;
				if (this._attacker != null)
				{
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._attackerPerks)
					{
						num += mpperkObject.GetRangedHeadShotDamage(isWarmup, this._attacker);
					}
				}
				return num;
			}

			// Token: 0x04002025 RID: 8229
			private readonly Agent _attacker;

			// Token: 0x04002026 RID: 8230
			private readonly Agent _defender;

			// Token: 0x04002027 RID: 8231
			private readonly MBReadOnlyList<MPPerkObject> _attackerPerks;

			// Token: 0x04002028 RID: 8232
			private readonly MBReadOnlyList<MPPerkObject> _defenderPerks;
		}
	}
}
