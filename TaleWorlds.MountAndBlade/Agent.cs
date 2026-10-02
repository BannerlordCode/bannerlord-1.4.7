using System;
using System.Collections.Generic;
using System.Diagnostics;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020000FA RID: 250
	public sealed class Agent : DotNetObject, IAgent, IFocusable, IUsable, IFormationUnit, ITrackableBase
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0000F80C File Offset: 0x0000DA0C
		public static Agent Main
		{
			get
			{
				Mission mission = Mission.Current;
				if (mission == null)
				{
					return null;
				}
				return mission.MainAgent;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600093B RID: 2363 RVA: 0x0000F820 File Offset: 0x0000DA20
		// (remove) Token: 0x0600093C RID: 2364 RVA: 0x0000F858 File Offset: 0x0000DA58
		public event Agent.OnAgentHealthChangedDelegate OnAgentHealthChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600093D RID: 2365 RVA: 0x0000F890 File Offset: 0x0000DA90
		// (remove) Token: 0x0600093E RID: 2366 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		public event Agent.OnMountHealthChangedDelegate OnMountHealthChanged;

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0000F8FD File Offset: 0x0000DAFD
		public bool IsPlayerControlled
		{
			get
			{
				return this.IsMine || this.MissionPeer != null;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0000F912 File Offset: 0x0000DB12
		public bool IsMine
		{
			get
			{
				return this.Controller == AgentControllerType.Player;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0000F91D File Offset: 0x0000DB1D
		public bool IsMainAgent
		{
			get
			{
				return this == Agent.Main;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0000F927 File Offset: 0x0000DB27
		public bool IsHuman
		{
			get
			{
				return (this.GetAgentFlags() & AgentFlag.IsHumanoid) > AgentFlag.None;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0000F938 File Offset: 0x0000DB38
		public bool IsMount
		{
			get
			{
				return (this.GetAgentFlags() & AgentFlag.Mountable) > AgentFlag.None;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0000F945 File Offset: 0x0000DB45
		public bool IsAIControlled
		{
			get
			{
				return this.Controller == AgentControllerType.AI && !GameNetwork.IsClientOrReplay;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0000F95A File Offset: 0x0000DB5A
		public bool IsPlayerTroop
		{
			get
			{
				return !GameNetwork.IsMultiplayer && this.Origin != null && this.Origin.Troop == Game.Current.PlayerTroop;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0000F984 File Offset: 0x0000DB84
		public bool IsUsingGameObject
		{
			get
			{
				return this.CurrentlyUsedGameObject != null;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0000F98F File Offset: 0x0000DB8F
		public bool CanLeadFormationsRemotely
		{
			get
			{
				return this._canLeadFormationsRemotely;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0000F997 File Offset: 0x0000DB97
		public bool IsDetachableFromFormation
		{
			get
			{
				return this._isDetachableFromFormation;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0000F99F File Offset: 0x0000DB9F
		public float AgentScale
		{
			get
			{
				return MBAPI.IMBAgent.GetAgentScale(this.GetPtr());
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0000F9B1 File Offset: 0x0000DBB1
		public bool CrouchMode
		{
			get
			{
				return MBAPI.IMBAgent.GetCrouchMode(this.GetPtr());
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0000F9C3 File Offset: 0x0000DBC3
		public bool WalkMode
		{
			get
			{
				return MBAPI.IMBAgent.GetWalkMode(this.GetPtr());
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0000F9D5 File Offset: 0x0000DBD5
		public Vec3 Position
		{
			get
			{
				return AgentHelper.GetAgentPosition(this.PositionPointer);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0000F9E2 File Offset: 0x0000DBE2
		public AgentMovementMode MovementMode
		{
			get
			{
				return AgentHelper.GetAgentMovementMode(this._movementModePointer);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0000F9EF File Offset: 0x0000DBEF
		public Vec3 VisualPosition
		{
			get
			{
				return MBAPI.IMBAgent.GetVisualPosition(this.GetPtr());
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0000FA01 File Offset: 0x0000DC01
		public Vec2 MovementVelocity
		{
			get
			{
				return MBAPI.IMBAgent.GetMovementVelocity(this.GetPtr());
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0000FA13 File Offset: 0x0000DC13
		public Vec3 AverageVelocity
		{
			get
			{
				return MBAPI.IMBAgent.GetAverageVelocity(this.GetPtr());
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0000FA25 File Offset: 0x0000DC25
		public float MovementDirectionAsAngle
		{
			get
			{
				return AgentHelper.GetAgentMovementDirectionAsAngle(this._movementDirectionPointer);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0000FA32 File Offset: 0x0000DC32
		public bool IsLookRotationInSlowMotion
		{
			get
			{
				return MBAPI.IMBAgent.IsLookRotationInSlowMotion(this.GetPtr());
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public Agent.AgentPropertiesModifiers PropertyModifiers
		{
			get
			{
				return this._propertyModifiers;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		public MBActionSet ActionSet
		{
			get
			{
				return new MBActionSet(MBAPI.IMBAgent.GetActionSetNo(this.GetPtr()));
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0000FA63 File Offset: 0x0000DC63
		public MBReadOnlyList<AgentComponent> Components
		{
			get
			{
				return this._components;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0000FA6B File Offset: 0x0000DC6B
		public MBReadOnlyList<Agent.Hitter> HitterList
		{
			get
			{
				return this._hitterList;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0000FA73 File Offset: 0x0000DC73
		public Agent.GuardMode CurrentGuardMode
		{
			get
			{
				return MBAPI.IMBAgent.GetCurrentGuardMode(this.GetPtr());
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0000FA85 File Offset: 0x0000DC85
		public Agent ImmediateEnemy
		{
			get
			{
				return MBAPI.IMBAgent.GetImmediateEnemy(this.GetPtr());
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0000FA97 File Offset: 0x0000DC97
		public bool IsDoingPassiveAttack
		{
			get
			{
				return MBAPI.IMBAgent.GetIsDoingPassiveAttack(this.GetPtr());
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0000FAA9 File Offset: 0x0000DCA9
		public bool IsPassiveUsageConditionsAreMet
		{
			get
			{
				return MBAPI.IMBAgent.GetIsPassiveUsageConditionsAreMet(this.GetPtr());
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0000FABB File Offset: 0x0000DCBB
		public float CurrentAimingError
		{
			get
			{
				return MBAPI.IMBAgent.GetCurrentAimingError(this.GetPtr());
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0000FACD File Offset: 0x0000DCCD
		public float CurrentAimingTurbulance
		{
			get
			{
				return MBAPI.IMBAgent.GetCurrentAimingTurbulance(this.GetPtr());
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0000FADF File Offset: 0x0000DCDF
		public Agent.UsageDirection AttackDirection
		{
			get
			{
				return MBAPI.IMBAgent.GetAttackDirectionUsage(this.GetPtr());
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0000FAF1 File Offset: 0x0000DCF1
		public float WalkingSpeedLimitOfMountable
		{
			get
			{
				return MBAPI.IMBAgent.GetWalkSpeedLimitOfMountable(this.GetPtr());
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0000FB03 File Offset: 0x0000DD03
		public Agent RiderAgent
		{
			get
			{
				return this.GetRiderAgentAux();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0000FB0B File Offset: 0x0000DD0B
		public bool HasMount
		{
			get
			{
				return this.MountAgent != null;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0000FB16 File Offset: 0x0000DD16
		public bool CanLogCombatFor
		{
			get
			{
				return (this.RiderAgent != null && !this.RiderAgent.IsAIControlled) || (!this.IsMount && !this.IsAIControlled);
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0000FB42 File Offset: 0x0000DD42
		public float MissileRangeAdjusted
		{
			get
			{
				return this.GetMissileRangeWithHeightDifference();
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0000FB4A File Offset: 0x0000DD4A
		public float MaximumMissileRange
		{
			get
			{
				return this.GetMissileRange();
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0000FB52 File Offset: 0x0000DD52
		FocusableObjectType IFocusable.FocusableObjectType
		{
			get
			{
				if (!this.IsMount)
				{
					return FocusableObjectType.Agent;
				}
				return FocusableObjectType.Mount;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0000FB5F File Offset: 0x0000DD5F
		bool IFocusable.IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0000FB62 File Offset: 0x0000DD62
		public string Name
		{
			get
			{
				if (this.MissionPeer == null)
				{
					return this._name.ToString();
				}
				return this.MissionPeer.Name;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0000FB83 File Offset: 0x0000DD83
		public TextObject NameTextObject
		{
			get
			{
				if (this.MissionPeer == null)
				{
					return this._name;
				}
				return new TextObject("{=!}" + this.MissionPeer.Name, null);
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0000FBAF File Offset: 0x0000DDAF
		public AgentMovementLockedState MovementLockedState
		{
			get
			{
				return this.GetMovementLockedState();
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0000FBB7 File Offset: 0x0000DDB7
		public Monster Monster { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0000FBBF File Offset: 0x0000DDBF
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x0000FBC7 File Offset: 0x0000DDC7
		public bool IsRunningAway { get; private set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0000FBD8 File Offset: 0x0000DDD8
		public BodyProperties BodyPropertiesValue { get; private set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0000FBE1 File Offset: 0x0000DDE1
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0000FBE9 File Offset: 0x0000DDE9
		public CommonAIComponent CommonAIComponent { get; private set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0000FBF2 File Offset: 0x0000DDF2
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0000FBFA File Offset: 0x0000DDFA
		public HumanAIComponent HumanAIComponent { get; private set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0000FC03 File Offset: 0x0000DE03
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x0000FC0B File Offset: 0x0000DE0B
		public int BodyPropertiesSeed { get; internal set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0000FC14 File Offset: 0x0000DE14
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		public float LastRangedHitTime { get; private set; } = float.MinValue;

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0000FC25 File Offset: 0x0000DE25
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x0000FC2D File Offset: 0x0000DE2D
		public float LastMeleeHitTime { get; private set; } = float.MinValue;

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0000FC36 File Offset: 0x0000DE36
		// (set) Token: 0x06000979 RID: 2425 RVA: 0x0000FC3E File Offset: 0x0000DE3E
		public float LastRangedAttackTime { get; private set; } = float.MinValue;

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0000FC47 File Offset: 0x0000DE47
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x0000FC4F File Offset: 0x0000DE4F
		public float LastMeleeAttackTime { get; private set; } = float.MinValue;

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0000FC58 File Offset: 0x0000DE58
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0000FC60 File Offset: 0x0000DE60
		public bool IsFemale { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0000FC69 File Offset: 0x0000DE69
		public ItemObject Banner
		{
			get
			{
				MissionEquipment equipment = this.Equipment;
				if (equipment == null)
				{
					return null;
				}
				return equipment.GetBanner();
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0000FC7C File Offset: 0x0000DE7C
		public ItemObject FormationBanner
		{
			get
			{
				return this._formationBanner;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0000FC84 File Offset: 0x0000DE84
		public MissionWeapon WieldedWeapon
		{
			get
			{
				EquipmentIndex primaryWieldedItemIndex = this.GetPrimaryWieldedItemIndex();
				if (primaryWieldedItemIndex < EquipmentIndex.WeaponItemBeginSlot)
				{
					return MissionWeapon.Invalid;
				}
				return this.Equipment[primaryWieldedItemIndex];
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0000FCAE File Offset: 0x0000DEAE
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0000FCB6 File Offset: 0x0000DEB6
		public bool IsItemUseDisabled { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0000FCBF File Offset: 0x0000DEBF
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x0000FCC7 File Offset: 0x0000DEC7
		public bool SyncHealthToAllClients { get; private set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0000FCD0 File Offset: 0x0000DED0
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public UsableMissionObject CurrentlyUsedGameObject { get; private set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0000FCE1 File Offset: 0x0000DEE1
		public bool CombatActionsEnabled
		{
			get
			{
				return this.CurrentlyUsedGameObject == null || !this.CurrentlyUsedGameObject.DisableCombatActionsOnUse;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0000FCFB File Offset: 0x0000DEFB
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0000FD03 File Offset: 0x0000DF03
		public Mission Mission { get; private set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0000FD0C File Offset: 0x0000DF0C
		public bool IsHero
		{
			get
			{
				return this.Character != null && this.Character.IsHero;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0000FD23 File Offset: 0x0000DF23
		public int Index { get; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0000FD2B File Offset: 0x0000DF2B
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x0000FD33 File Offset: 0x0000DF33
		public MissionEquipment Equipment { get; private set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0000FD44 File Offset: 0x0000DF44
		public TextObject AgentRole { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0000FD4D File Offset: 0x0000DF4D
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0000FD55 File Offset: 0x0000DF55
		public bool HasBeenBuilt { get; private set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0000FD5E File Offset: 0x0000DF5E
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0000FD66 File Offset: 0x0000DF66
		public Agent.MortalityState CurrentMortalityState { get; private set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0000FD6F File Offset: 0x0000DF6F
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0000FD77 File Offset: 0x0000DF77
		public Equipment SpawnEquipment { get; private set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0000FD80 File Offset: 0x0000DF80
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0000FD88 File Offset: 0x0000DF88
		public FormationPositionPreference FormationPositionPreference { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0000FD91 File Offset: 0x0000DF91
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0000FD99 File Offset: 0x0000DF99
		public bool RandomizeColors { get; private set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0000FDAA File Offset: 0x0000DFAA
		public float CharacterPowerCached { get; private set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0000FDB3 File Offset: 0x0000DFB3
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0000FDBB File Offset: 0x0000DFBB
		public float WalkSpeedCached { get; private set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		public IAgentOriginBase Origin { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0000FDD5 File Offset: 0x0000DFD5
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0000FDDD File Offset: 0x0000DFDD
		public Team Team { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0000FDE6 File Offset: 0x0000DFE6
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0000FDEE File Offset: 0x0000DFEE
		public int KillCount { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0000FDF7 File Offset: 0x0000DFF7
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0000FDFF File Offset: 0x0000DFFF
		public AgentDrivenProperties AgentDrivenProperties { get; private set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0000FE08 File Offset: 0x0000E008
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0000FE10 File Offset: 0x0000E010
		public float BaseHealthLimit { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0000FE19 File Offset: 0x0000E019
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0000FE21 File Offset: 0x0000E021
		public string HorseCreationKey { get; private set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0000FE2A File Offset: 0x0000E02A
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x0000FE32 File Offset: 0x0000E032
		public float HealthLimit { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0000FE3B File Offset: 0x0000E03B
		public bool IsRangedCached
		{
			get
			{
				return this.Equipment.ContainsNonConsumableRangedWeaponWithAmmo();
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0000FE48 File Offset: 0x0000E048
		public bool HasAnyRangedWeaponCached
		{
			get
			{
				return this.IsRangedCached || this.Equipment.ContainsThrownWeapon();
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0000FE5F File Offset: 0x0000E05F
		public bool HasMeleeWeaponCached
		{
			get
			{
				return this.Equipment.ContainsMeleeWeapon();
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0000FE6C File Offset: 0x0000E06C
		public bool HasShieldCached
		{
			get
			{
				return this.Equipment.ContainsShield();
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0000FE79 File Offset: 0x0000E079
		public bool HasSpearCached
		{
			get
			{
				return this.Equipment.ContainsSpear();
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0000FE86 File Offset: 0x0000E086
		public bool HasThrownCached
		{
			get
			{
				return this.Equipment.ContainsThrownWeapon();
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0000FE93 File Offset: 0x0000E093
		public bool CanPerformBraceCached
		{
			get
			{
				return this.CanPerformBrace();
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0000FE9B File Offset: 0x0000E09B
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0000FEAD File Offset: 0x0000E0AD
		public Agent.AIStateFlag AIStateFlags
		{
			get
			{
				return MBAPI.IMBAgent.GetAIStateFlags(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetAIStateFlags(this.GetPtr(), value);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				MBAPI.IMBAgent.GetRotationFrame(this.GetPtr(), ref matrixFrame);
				return matrixFrame;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0000FEE8 File Offset: 0x0000E0E8
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0000FEFA File Offset: 0x0000E0FA
		public Agent.MovementControlFlag MovementFlags
		{
			get
			{
				return MBAPI.IMBAgent.GetMovementFlags(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetMovementFlags(this.GetPtr(), value);
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0000FF0D File Offset: 0x0000E10D
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0000FF1F File Offset: 0x0000E11F
		public Vec2 MovementInputVector
		{
			get
			{
				return MBAPI.IMBAgent.GetMovementInputVector(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetMovementInputVector(this.GetPtr(), value);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0000FF34 File Offset: 0x0000E134
		public CapsuleData CollisionCapsule
		{
			get
			{
				CapsuleData capsuleData = default(CapsuleData);
				MBAPI.IMBAgent.GetCollisionCapsule(this.GetPtr(), ref capsuleData);
				return capsuleData;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0000FF5C File Offset: 0x0000E15C
		public Vec3 CollisionCapsuleCenter
		{
			get
			{
				ValueTuple<Vec3, Vec3> boxMinMax = this.CollisionCapsule.GetBoxMinMax();
				Vec3 item = boxMinMax.Item1;
				Vec3 item2 = boxMinMax.Item2;
				return (item + item2) * 0.5f;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0000FF98 File Offset: 0x0000E198
		public MBAgentVisuals AgentVisuals
		{
			get
			{
				MBAgentVisuals agentVisuals;
				if (!this._visualsWeakRef.TryGetTarget(out agentVisuals))
				{
					agentVisuals = MBAPI.IMBAgent.GetAgentVisuals(this.GetPtr());
					this._visualsWeakRef.SetTarget(agentVisuals);
				}
				return agentVisuals;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0000FFD2 File Offset: 0x0000E1D2
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		public bool HeadCameraMode
		{
			get
			{
				return MBAPI.IMBAgent.GetHeadCameraMode(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetHeadCameraMode(this.GetPtr(), value);
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0000FFF7 File Offset: 0x0000E1F7
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0000FFFF File Offset: 0x0000E1FF
		public Agent MountAgent
		{
			get
			{
				return this.GetMountAgentAux();
			}
			private set
			{
				this.SetMountAgent(value);
				this.UpdateAgentStats();
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0001000E File Offset: 0x0000E20E
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x00010016 File Offset: 0x0000E216
		public IDetachment Detachment
		{
			get
			{
				return this._detachment;
			}
			set
			{
				this._detachment = value;
				if (this._detachment != null)
				{
					Formation formation = this.Formation;
					if (formation == null)
					{
						return;
					}
					formation.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(this);
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00010042 File Offset: 0x0000E242
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x00010050 File Offset: 0x0000E250
		public bool IsPaused
		{
			get
			{
				return this.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.Paused);
			}
			set
			{
				if (value)
				{
					this.AIStateFlags |= Agent.AIStateFlag.Paused;
					return;
				}
				this.AIStateFlags &= ~Agent.AIStateFlag.Paused;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00010073 File Offset: 0x0000E273
		public bool IsDetachedFromFormation
		{
			get
			{
				return this._detachment != null;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00010080 File Offset: 0x0000E280
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x000100A4 File Offset: 0x0000E2A4
		public Agent.WatchState CurrentWatchState
		{
			get
			{
				Agent.AIStateFlag aistateFlag = this.AIStateFlags & Agent.AIStateFlag.Alarmed;
				if (aistateFlag == Agent.AIStateFlag.Cautious)
				{
					return Agent.WatchState.Cautious;
				}
				if (aistateFlag == Agent.AIStateFlag.Alarmed)
				{
					return Agent.WatchState.Alarmed;
				}
				return Agent.WatchState.Patrolling;
			}
			private set
			{
				switch (value)
				{
				case Agent.WatchState.Patrolling:
					this.SetAlarmState(Agent.AIStateFlag.None);
					return;
				case Agent.WatchState.Cautious:
					this.SetAlarmState(Agent.AIStateFlag.Cautious);
					return;
				case Agent.WatchState.Alarmed:
					this.SetAlarmState(Agent.AIStateFlag.Alarmed);
					return;
				default:
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Agent.cs", "CurrentWatchState", 933);
					return;
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x000100F9 File Offset: 0x0000E2F9
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00010101 File Offset: 0x0000E301
		public float Defensiveness
		{
			get
			{
				return this._defensiveness;
			}
			set
			{
				if (MathF.Abs(value - this._defensiveness) > 0.0001f)
				{
					this._defensiveness = value;
					this.UpdateAgentProperties();
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00010124 File Offset: 0x0000E324
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0001012C File Offset: 0x0000E32C
		public Formation Formation
		{
			get
			{
				return this._formation;
			}
			set
			{
				if (this._formation != value)
				{
					if (GameNetwork.IsServer && this.HasBeenBuilt && this.Mission.GetMissionBehavior<MissionNetworkComponent>() != null)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new AgentSetFormation(this.Index, (value != null) ? value.Index : (-1)));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					this.SetNativeFormationNo((value != null) ? value.Index : (-1));
					IDetachment detachment = null;
					float num = 0f;
					if (this._formation != null)
					{
						if (this.IsDetachedFromFormation)
						{
							detachment = this.Detachment;
							num = this.DetachmentWeight;
						}
						this._formation.RemoveUnit(this);
						foreach (IDetachment detachment2 in this._formation.Detachments)
						{
							if (!detachment2.IsUsedByFormation(value))
							{
								this.Team.DetachmentManager.RemoveScoresOfAgentFromDetachment(this, detachment2);
							}
						}
					}
					this._formation = value;
					if (this._formation != null)
					{
						if (!this._formation.HasBeenPositioned)
						{
							this._formation.SetPositioning(new WorldPosition?(this.GetWorldPosition()), new Vec2?(this.LookDirection.AsVec2), null);
						}
						this._formation.AddUnit(this);
						if (detachment != null && this._formation.Detachments.IndexOf(detachment) >= 0 && detachment.IsStandingPointAvailableForAgent(this))
						{
							detachment.AddAgent(this, -1, Agent.AIScriptedFrameFlags.None);
							this._formation.DetachUnit(this, detachment.IsLoose);
							this.Detachment = detachment;
							this.DetachmentWeight = num;
						}
					}
					foreach (AgentComponent agentComponent in this._components)
					{
						agentComponent.OnFormationSet();
					}
					this.ForceUpdateCachedAndFormationValues(this._formation != null && this._formation.PostponeCostlyOperations, false);
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x00010334 File Offset: 0x0000E534
		IFormationUnit IFormationUnit.FollowedUnit
		{
			get
			{
				if (!this.IsActive())
				{
					return null;
				}
				if (this.IsAIControlled)
				{
					return this.GetFollowedUnit();
				}
				return null;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x00010350 File Offset: 0x0000E550
		public bool IsShieldUsageEncouraged
		{
			get
			{
				return this.Formation.FiringOrder.OrderEnum == FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire || !this.Equipment.HasAnyWeaponWithFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00010378 File Offset: 0x0000E578
		public bool IsPlayerUnit
		{
			get
			{
				return this.IsPlayerControlled || this.IsPlayerTroop;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0001038A File Offset: 0x0000E58A
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x00010398 File Offset: 0x0000E598
		public AgentControllerType Controller
		{
			get
			{
				return AgentHelper.GetAgentControllerType(this._controllerTypePointer);
			}
			set
			{
				AgentControllerType controller = this.Controller;
				if (value != controller)
				{
					if (value == AgentControllerType.Player && this.IsDetachedFromFormation)
					{
						this._detachment.RemoveAgent(this);
						Formation formation = this._formation;
						if (formation != null)
						{
							formation.AttachUnit(this);
						}
					}
					MBAPI.IMBAgent.SetController(this.GetPtr(), value);
					bool flag = value == AgentControllerType.Player;
					if (flag)
					{
						this.Mission.MainAgent = this;
						this.SetAgentFlags(this.GetAgentFlags() | AgentFlag.CanRide);
					}
					Formation formation2 = this.Formation;
					if (formation2 != null)
					{
						formation2.OnAgentControllerChanged(this, controller);
					}
					if (value != AgentControllerType.AI && this.GetAgentFlags().HasAnyFlag(AgentFlag.IsHumanoid))
					{
						Agent mountAgent = this.MountAgent;
						if (mountAgent != null)
						{
							mountAgent.SetMaximumSpeedLimit(-1f, false);
						}
						this.SetMaximumSpeedLimit(-1f, false);
						if (this.WalkMode)
						{
							this.EventControlFlags |= Agent.EventControlFlag.Run;
						}
					}
					foreach (MissionBehavior missionBehavior in this.Mission.MissionBehaviors)
					{
						missionBehavior.OnAgentControllerChanged(this, controller);
					}
					if (flag)
					{
						foreach (MissionBehavior missionBehavior2 in this.Mission.MissionBehaviors)
						{
							missionBehavior2.OnAgentControllerSetToPlayer(this.Mission.MainAgent);
						}
					}
					if (GameNetwork.IsServer)
					{
						MissionPeer missionPeer = this.MissionPeer;
						NetworkCommunicator networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
						if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
						{
							GameNetwork.BeginModuleEventAsServer(networkCommunicator);
							GameNetwork.WriteMessage(new SetAgentIsPlayer(this.Index, this.Controller != AgentControllerType.AI));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00010564 File Offset: 0x0000E764
		public uint ClothingColor1
		{
			get
			{
				if (this._clothingColor1 != null)
				{
					return this._clothingColor1.Value;
				}
				if (this.Team != null)
				{
					return this.Team.Color;
				}
				Debug.FailedAssert("Clothing color is not set.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Agent.cs", "ClothingColor1", 1150);
				return uint.MaxValue;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public uint ClothingColor2
		{
			get
			{
				uint? clothingColor = this._clothingColor2;
				if (clothingColor == null)
				{
					return this.ClothingColor1;
				}
				return clothingColor.GetValueOrDefault();
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x000105E4 File Offset: 0x0000E7E4
		public MatrixFrame LookFrame
		{
			get
			{
				return new MatrixFrame
				{
					origin = this.Position,
					rotation = this.LookRotation
				};
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00010614 File Offset: 0x0000E814
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x00010626 File Offset: 0x0000E826
		public float LookDirectionAsAngle
		{
			get
			{
				return MBAPI.IMBAgent.GetLookDirectionAsAngle(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetLookDirectionAsAngle(this.GetPtr(), value);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0001063C File Offset: 0x0000E83C
		public Mat3 LookRotation
		{
			get
			{
				Mat3 mat;
				mat.f = this.LookDirection;
				mat.u = Vec3.Up;
				mat.s = Vec3.CrossProduct(mat.f, mat.u);
				mat.s.Normalize();
				mat.u = Vec3.CrossProduct(mat.s, mat.f);
				return mat;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000106A0 File Offset: 0x0000E8A0
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x000106B2 File Offset: 0x0000E8B2
		public bool IsLookDirectionLocked
		{
			get
			{
				return MBAPI.IMBAgent.GetIsLookDirectionLocked(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetIsLookDirectionLocked(this.GetPtr(), value);
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x000106C8 File Offset: 0x0000E8C8
		public bool IsCheering
		{
			get
			{
				ActionIndexCache currentAction = this.GetCurrentAction(1);
				for (int i = 0; i < Agent.DefaultTauntActions.Length; i++)
				{
					ActionIndexCache[] defaultTauntActions = Agent.DefaultTauntActions;
					if (Agent.DefaultTauntActions[i] == currentAction)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00010711 File Offset: 0x0000E911
		public bool IsInBeingStruckAction
		{
			get
			{
				return MBMath.IsBetween((int)this.GetCurrentActionType(1), 48, 52) || MBMath.IsBetween((int)this.GetCurrentActionType(0), 48, 52);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00010737 File Offset: 0x0000E937
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x00010740 File Offset: 0x0000E940
		public MissionPeer MissionPeer
		{
			get
			{
				return this._missionPeer;
			}
			set
			{
				if (this._missionPeer != value)
				{
					MissionPeer missionPeer = this._missionPeer;
					this._missionPeer = value;
					if (missionPeer != null && missionPeer.ControlledAgent == this)
					{
						missionPeer.ControlledAgent = null;
					}
					if (this._missionPeer != null && this._missionPeer.ControlledAgent != this)
					{
						this._missionPeer.ControlledAgent = this;
						if (GameNetwork.IsServerOrRecorder)
						{
							this.SyncHealthToClients();
							Agent.OnAgentHealthChangedDelegate onAgentHealthChanged = this.OnAgentHealthChanged;
							if (onAgentHealthChanged != null)
							{
								onAgentHealthChanged(this, this.Health, this.Health);
							}
						}
					}
					if (value != null)
					{
						this.Controller = (value.IsMine ? AgentControllerType.Player : AgentControllerType.None);
					}
					if (GameNetwork.IsServer && this.IsHuman && !this._isDeleted)
					{
						NetworkCommunicator networkCommunicator = ((value != null) ? value.GetNetworkPeer() : null);
						this.SetNetworkPeer(networkCommunicator);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetAgentPeer(this.Index, networkCommunicator));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00010824 File Offset: 0x0000EA24
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0001082C File Offset: 0x0000EA2C
		public BasicCharacterObject Character
		{
			get
			{
				return this._character;
			}
			set
			{
				this._character = value;
				if (value != null)
				{
					this.Health = (float)this._character.HitPoints;
					this.BaseHealthLimit = (float)this._character.MaxHitPoints();
					this.HealthLimit = this.BaseHealthLimit;
					this.CharacterPowerCached = value.GetPower();
					this._name = value.Name;
					this.IsFemale = value.IsFemale;
				}
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x00010897 File Offset: 0x0000EA97
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0001089F File Offset: 0x0000EA9F
		public float LastDetachmentTickAgentTime { get; private set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x000108A8 File Offset: 0x0000EAA8
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x000108B0 File Offset: 0x0000EAB0
		public MissionPeer OwningAgentMissionPeer { get; private set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x000108B9 File Offset: 0x0000EAB9
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x000108C1 File Offset: 0x0000EAC1
		public MissionRepresentativeBase MissionRepresentative { get; private set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x000108CA File Offset: 0x0000EACA
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x000108D2 File Offset: 0x0000EAD2
		public bool IsInLadderQueue { get; private set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x000108DB File Offset: 0x0000EADB
		IMissionTeam IAgent.Team
		{
			get
			{
				return this.Team;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x000108E3 File Offset: 0x0000EAE3
		IFormationArrangement IFormationUnit.Formation
		{
			get
			{
				return this._formation.Arrangement;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x000108F0 File Offset: 0x0000EAF0
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x000108F8 File Offset: 0x0000EAF8
		int IFormationUnit.FormationFileIndex { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00010901 File Offset: 0x0000EB01
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x00010909 File Offset: 0x0000EB09
		int IFormationUnit.FormationRankIndex { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00010912 File Offset: 0x0000EB12
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x00010944 File Offset: 0x0000EB44
		public Vec2 LocalPositionError
		{
			get
			{
				return this._localPositionError * this.Formation.Interval * this.Formation.UnitDiameter * 0.75f;
			}
			private set
			{
				this._localPositionError = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0001094D File Offset: 0x0000EB4D
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x00010955 File Offset: 0x0000EB55
		public float DetachmentWeight { get; private set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0001095E File Offset: 0x0000EB5E
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x00010966 File Offset: 0x0000EB66
		public int DetachmentIndex { get; private set; } = -1;

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0001096F File Offset: 0x0000EB6F
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x00010977 File Offset: 0x0000EB77
		public bool IsFormationFrameEnabled { get; private set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00010980 File Offset: 0x0000EB80
		private UIntPtr Pointer
		{
			get
			{
				return this._pointer;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00010988 File Offset: 0x0000EB88
		private UIntPtr FlagsPointer
		{
			get
			{
				return this._flagsPointer;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00010990 File Offset: 0x0000EB90
		private UIntPtr PositionPointer
		{
			get
			{
				return this._positionPointer;
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00010998 File Offset: 0x0000EB98
		internal Agent(Mission mission, Mission.AgentCreationResult creationResult, Agent.CreationType creationType, Monster monster, int creationIndex)
		{
			this.AgentRole = TextObject.GetEmpty();
			this.Mission = mission;
			this.Index = creationResult.Index;
			this._pointer = creationResult.AgentPtr;
			this._positionPointer = creationResult.PositionPtr;
			this._flagsPointer = creationResult.FlagsPtr;
			this._indexPointer = creationResult.IndexPtr;
			this._statePointer = creationResult.StatePtr;
			this._movementModePointer = creationResult.MovementModePointer;
			this._controllerTypePointer = creationResult.ControllerPointer;
			this._movementDirectionPointer = creationResult.MovementDirectionPointer;
			this._primaryWieldedItemIndexPointer = creationResult.PrimaryWieldedItemIndexPointer;
			this._offHandWieldedItemIndexPointer = creationResult.OffHandWieldedItemIndexPointer;
			this._channel0CurrentActionPointer = creationResult.Channel0CurrentActionPointer;
			this._channel1CurrentActionPointer = creationResult.Channel1CurrentActionPointer;
			this._maximumForwardUnlimitedSpeed = creationResult.MaximumForwardUnlimitedSpeed;
			this._lastHitInfo = default(Agent.AgentLastHitInfo);
			this._lastHitInfo.Initialize();
			MBAPI.IMBAgent.SetMonoObject(this.GetPtr(), this);
			this.Monster = monster;
			this.KillCount = 0;
			this.HasBeenBuilt = false;
			this._creationType = creationType;
			this._agentControllers = new List<AgentController>();
			this._components = new MBList<AgentComponent>();
			this._hitterList = new MBList<Agent.Hitter>();
			((IFormationUnit)this).FormationFileIndex = -1;
			((IFormationUnit)this).FormationRankIndex = -1;
			this._synchedBodyComponents = null;
			this._cachedAndFormationValuesUpdateTimer = new Timer(this.Mission.CurrentTime, 0.45f + MBRandom.RandomFloat * 0.1f, true);
			this._creationIndex = creationIndex;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00010B5D File Offset: 0x0000ED5D
		bool IAgent.IsEnemyOf(IAgent agent)
		{
			return this.IsEnemyOf((Agent)agent);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00010B6B File Offset: 0x0000ED6B
		bool IAgent.IsFriendOf(IAgent agent)
		{
			return this.IsFriendOf((Agent)agent);
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00010B79 File Offset: 0x0000ED79
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00010B8B File Offset: 0x0000ED8B
		public Vec3 LookDirection
		{
			get
			{
				return MBAPI.IMBAgent.GetLookDirection(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetLookDirection(this.GetPtr(), value);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00010B9E File Offset: 0x0000ED9E
		public bool IsLookDirectionLow
		{
			get
			{
				return this.LookDirection.z < 0f;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x00010BB2 File Offset: 0x0000EDB2
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x00010BBC File Offset: 0x0000EDBC
		public float Health
		{
			get
			{
				return this._health;
			}
			set
			{
				float num = (float)(value.ApproximatelyEqualsTo(0f, 1E-05f) ? 0 : MathF.Ceiling(value));
				if (!this._health.ApproximatelyEqualsTo(num, 1E-05f))
				{
					float health = this._health;
					this._health = num;
					if (GameNetwork.IsServerOrRecorder)
					{
						this.SyncHealthToClients();
					}
					Agent.OnAgentHealthChangedDelegate onAgentHealthChanged = this.OnAgentHealthChanged;
					if (onAgentHealthChanged != null)
					{
						onAgentHealthChanged(this, health, this._health);
					}
					if (this.RiderAgent != null)
					{
						Agent.OnMountHealthChangedDelegate onMountHealthChanged = this.RiderAgent.OnMountHealthChanged;
						if (onMountHealthChanged == null)
						{
							return;
						}
						onMountHealthChanged(this.RiderAgent, this, health, this._health);
					}
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00010C58 File Offset: 0x0000EE58
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00010C74 File Offset: 0x0000EE74
		public float Age
		{
			get
			{
				return this.BodyPropertiesValue.Age;
			}
			set
			{
				this.BodyPropertiesValue = new BodyProperties(new DynamicBodyProperties(value, this.BodyPropertiesValue.Weight, this.BodyPropertiesValue.Build), this.BodyPropertiesValue.StaticProperties);
				BodyProperties bodyPropertiesValue = this.BodyPropertiesValue;
				this.BodyPropertiesValue = bodyPropertiesValue;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00010CCC File Offset: 0x0000EECC
		public Vec3 Velocity
		{
			get
			{
				Vec2 movementVelocity = MBAPI.IMBAgent.GetMovementVelocity(this.GetPtr());
				Vec3 vec = new Vec3(movementVelocity, 0f, -1f);
				return this.Frame.rotation.TransformToParent(in vec);
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00010D11 File Offset: 0x0000EF11
		[MBCallback(null, false)]
		internal void SetAgentAIPerformingRetreatBehavior(bool isAgentAIPerformingRetreatBehavior)
		{
			if (!GameNetwork.IsClientOrReplay && this.Mission != null)
			{
				this.IsRunningAway = isAgentAIPerformingRetreatBehavior;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00010D29 File Offset: 0x0000EF29
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00010D3B File Offset: 0x0000EF3B
		public Agent.EventControlFlag EventControlFlags
		{
			get
			{
				return MBAPI.IMBAgent.GetEventControlFlags(this.GetPtr());
			}
			set
			{
				MBAPI.IMBAgent.SetEventControlFlags(this.GetPtr(), value);
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00010D4E File Offset: 0x0000EF4E
		public bool GetHasOnAiInputSetCallback()
		{
			return MBAPI.IMBAgent.GetHasOnAiInputSetCallback(this.GetPtr());
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00010D60 File Offset: 0x0000EF60
		public void SetHasOnAiInputSetCallback(bool value)
		{
			MBAPI.IMBAgent.SetHasOnAiInputSetCallback(this.GetPtr(), value);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00010D74 File Offset: 0x0000EF74
		[MBCallback(null, false)]
		internal void OnAIInputSet(ref Agent.EventControlFlag eventFlag, ref Agent.MovementControlFlag movementFlag, ref Vec2 inputVector)
		{
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnAIInputSet(ref eventFlag, ref movementFlag, ref inputVector);
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00010DC8 File Offset: 0x0000EFC8
		[MBCallback(null, false)]
		public float GetMissileRangeWithHeightDifferenceAux(float targetZ)
		{
			return MBAPI.IMBAgent.GetMissileRangeWithHeightDifference(this.GetPtr(), targetZ);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00010DDB File Offset: 0x0000EFDB
		[MBCallback(null, false)]
		internal int GetFormationUnitSpacing()
		{
			return this.Formation.UnitSpacing;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		[MBCallback(null, false)]
		public string GetSoundAndCollisionInfoClassName()
		{
			return this.Monster.SoundAndCollisionInfoClassName;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		[MBCallback(null, false)]
		internal bool IsInSameFormationWith(Agent otherAgent)
		{
			Formation formation = otherAgent.Formation;
			return this.Formation != null && formation != null && this.Formation == formation;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00010E22 File Offset: 0x0000F022
		[MBCallback(null, false)]
		internal void OnWeaponSwitchingToAlternativeStart(EquipmentIndex slotIndex, int usageIndex)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new StartSwitchingWeaponUsageIndex(this.Index, slotIndex, usageIndex, Agent.MovementFlagToDirection(this.MovementFlags)));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00010E55 File Offset: 0x0000F055
		[MBCallback(null, false)]
		internal void OnWeaponReloadPhaseChange(EquipmentIndex slotIndex, short reloadPhase)
		{
			this.Equipment.SetReloadPhaseOfSlot(slotIndex, reloadPhase);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetWeaponReloadPhase(this.Index, slotIndex, reloadPhase));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00010E8C File Offset: 0x0000F08C
		[MBCallback(null, false)]
		internal void OnWeaponAmmoReload(EquipmentIndex slotIndex, EquipmentIndex ammoSlotIndex, short totalAmmo)
		{
			if (this.Equipment[slotIndex].CurrentUsageItem.IsRangedWeapon)
			{
				this.Equipment.SetReloadedAmmoOfSlot(slotIndex, ammoSlotIndex, totalAmmo);
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetWeaponAmmoData(this.Index, slotIndex, ammoSlotIndex, totalAmmo));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			this.UpdateAgentProperties();
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00010EF0 File Offset: 0x0000F0F0
		[MBCallback(null, false)]
		internal void OnWeaponAmmoConsume(EquipmentIndex slotIndex, short totalAmmo)
		{
			if (this.Equipment[slotIndex].CurrentUsageItem.IsRangedWeapon)
			{
				this.Equipment.SetConsumedAmmoOfSlot(slotIndex, totalAmmo);
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetWeaponAmmoData(this.Index, slotIndex, EquipmentIndex.None, totalAmmo));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			this.UpdateAgentProperties();
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00010F52 File Offset: 0x0000F152
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00010F5F File Offset: 0x0000F15F
		public AgentState State
		{
			get
			{
				return AgentHelper.GetAgentState(this._statePointer);
			}
			set
			{
				if (this.State != value)
				{
					MBAPI.IMBAgent.SetStateFlags(this.GetPtr(), value);
				}
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00010F7C File Offset: 0x0000F17C
		[MBCallback(null, false)]
		internal void OnShieldDamaged(EquipmentIndex slotIndex, int inflictedDamage)
		{
			int num = MathF.Max(0, (int)this.Equipment[slotIndex].HitPoints - inflictedDamage);
			this.ChangeWeaponHitPoints(slotIndex, (short)num);
			if (num == 0)
			{
				this.RemoveEquippedWeapon(slotIndex);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00010FBC File Offset: 0x0000F1BC
		public MissionWeapon WieldedOffhandWeapon
		{
			get
			{
				EquipmentIndex offhandWieldedItemIndex = this.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex < EquipmentIndex.WeaponItemBeginSlot)
				{
					return MissionWeapon.Invalid;
				}
				return this.Equipment[offhandWieldedItemIndex];
			}
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00010FE8 File Offset: 0x0000F1E8
		[MBCallback(null, false)]
		internal void OnWeaponAmmoRemoved(EquipmentIndex slotIndex)
		{
			if (!this.Equipment[slotIndex].AmmoWeapon.IsEmpty)
			{
				this.Equipment.SetConsumedAmmoOfSlot(slotIndex, 0);
			}
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00011020 File Offset: 0x0000F220
		[MBCallback(null, false)]
		internal void OnMount(Agent mount)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				if (mount.IsAIControlled && mount.IsRetreating(false))
				{
					mount.StopRetreatingMoraleComponent();
				}
				this.CheckToDropFlaggedItem();
			}
			if (this.HasBeenBuilt)
			{
				foreach (AgentComponent agentComponent in this._components)
				{
					agentComponent.OnMount(mount);
				}
				this.Mission.OnAgentMount(this);
			}
			this.UpdateAgentStats();
			Action onAgentMountedStateChanged = this.OnAgentMountedStateChanged;
			if (onAgentMountedStateChanged != null)
			{
				onAgentMountedStateChanged();
			}
			this._isBracingCacheValid = false;
			if (GameNetwork.IsServerOrRecorder)
			{
				mount.SyncHealthToClients();
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000110D4 File Offset: 0x0000F2D4
		[MBCallback(null, false)]
		internal void OnDismount(Agent mount)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				Formation formation = this.Formation;
				if (formation != null)
				{
					formation.OnAgentLostMount(this);
				}
				this.CheckToDropFlaggedItem();
			}
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnDismount(mount);
			}
			this.Mission.OnAgentDismount(this);
			this._isBracingCacheValid = false;
			if (this.IsActive())
			{
				this.UpdateAgentStats();
				Action onAgentMountedStateChanged = this.OnAgentMountedStateChanged;
				if (onAgentMountedStateChanged == null)
				{
					return;
				}
				onAgentMountedStateChanged();
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00011178 File Offset: 0x0000F378
		[MBCallback(null, false)]
		internal void OnAgentAlarmedStateChanged(Agent.AIStateFlag flag)
		{
			foreach (MissionBehavior missionBehavior in Mission.Current.MissionBehaviors)
			{
				missionBehavior.OnAgentAlarmedStateChanged(this, flag);
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x000111D0 File Offset: 0x0000F3D0
		[MBCallback(null, false)]
		internal void OnRetreating()
		{
			if (!GameNetwork.IsClientOrReplay && this.Mission != null && !this.Mission.MissionEnded)
			{
				if (this.IsUsingGameObject && !(this.CurrentlyUsedGameObject is SpawnedItemEntity))
				{
					this.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				foreach (AgentComponent agentComponent in this._components)
				{
					agentComponent.OnRetreating();
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001125C File Offset: 0x0000F45C
		[MBCallback(null, true)]
		internal void UpdateMountAgentCache(Agent newMountAgent)
		{
			this._cachedMountAgent = newMountAgent;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00011265 File Offset: 0x0000F465
		[MBCallback(null, false)]
		internal void UpdateRiderAgentCache(Agent newRiderAgent)
		{
			this._cachedRiderAgent = newRiderAgent;
			if (newRiderAgent == null)
			{
				Mission.Current.AddMountWithoutRider(this);
				return;
			}
			Mission.Current.RemoveMountWithoutRider(this);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00011288 File Offset: 0x0000F488
		[MBCallback(null, false)]
		public void UpdateAgentStats()
		{
			if (this.IsActive())
			{
				this.UpdateAgentProperties();
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00011298 File Offset: 0x0000F498
		[MBCallback(null, true)]
		public float GetWeaponInaccuracy(EquipmentIndex weaponSlotIndex, int weaponUsageIndex)
		{
			WeaponComponentData weaponComponentDataForUsage = this.Equipment[weaponSlotIndex].GetWeaponComponentDataForUsage(weaponUsageIndex);
			int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(this, weaponComponentDataForUsage.RelevantSkill);
			return MissionGameModels.Current.AgentStatCalculateModel.GetWeaponInaccuracy(this, weaponComponentDataForUsage, effectiveSkill);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x000112E4 File Offset: 0x0000F4E4
		[MBCallback(null, false)]
		public float DebugGetHealth()
		{
			return this.Health;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000112EC File Offset: 0x0000F4EC
		public void SetTargetPosition(Vec2 value)
		{
			MBAPI.IMBAgent.SetTargetPosition(this.GetPtr(), ref value);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00011300 File Offset: 0x0000F500
		public void SetTargetZ(float targetZ)
		{
			MBAPI.IMBAgent.SetTargetZ(this.GetPtr(), targetZ);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00011313 File Offset: 0x0000F513
		public void SetTargetUp(in Vec3 targetUp)
		{
			MBAPI.IMBAgent.SetTargetUp(this.GetPtr(), in targetUp);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00011326 File Offset: 0x0000F526
		public void SetCanLeadFormationsRemotely(bool value)
		{
			this._canLeadFormationsRemotely = value;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001132F File Offset: 0x0000F52F
		public void SetAveragePingInMilliseconds(double averagePingInMilliseconds)
		{
			MBAPI.IMBAgent.SetAveragePingInMilliseconds(this.GetPtr(), averagePingInMilliseconds);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00011342 File Offset: 0x0000F542
		public void SetTargetPositionAndDirection(in Vec2 targetPosition, in Vec3 targetDirection)
		{
			MBAPI.IMBAgent.SetTargetPositionAndDirection(this.GetPtr(), in targetPosition, in targetDirection);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00011356 File Offset: 0x0000F556
		public void AddAcceleration(in Vec3 acceleration)
		{
			MBAPI.IMBAgent.AddAcceleration(this.GetPtr(), in acceleration);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00011369 File Offset: 0x0000F569
		public void SetWeaponGuard(Agent.UsageDirection direction)
		{
			MBAPI.IMBAgent.SetWeaponGuard(this.GetPtr(), direction);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001137C File Offset: 0x0000F57C
		public void SetWatchState(Agent.WatchState watchState)
		{
			this.CurrentWatchState = watchState;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00011385 File Offset: 0x0000F585
		public bool IsAlarmStateNormal()
		{
			return (this.AIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00011392 File Offset: 0x0000F592
		public bool IsCautious()
		{
			return (this.AIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Cautious;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001139F File Offset: 0x0000F59F
		public bool IsPatrollingCautious()
		{
			return (this.AIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.PatrollingCautious;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000113AC File Offset: 0x0000F5AC
		public bool IsAlarmed()
		{
			return (this.AIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Alarmed;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000113B9 File Offset: 0x0000F5B9
		public bool SetAlarmState(Agent.AIStateFlag alarmStateFlag)
		{
			if ((this.AIStateFlags & Agent.AIStateFlag.Alarmed) != alarmStateFlag)
			{
				MBAPI.IMBAgent.SetAIAlarmState(this.GetPtr(), alarmStateFlag);
				return true;
			}
			return false;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000113DA File Offset: 0x0000F5DA
		public void SetTargetFormationIndex(int targetFormationIndex)
		{
			MBAPI.IMBAgent.SetTargetFormationIndex(this.GetPtr(), targetFormationIndex);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000113F0 File Offset: 0x0000F5F0
		[MBCallback(null, false)]
		internal void OnWieldedItemIndexChange(bool isOffHand, bool isWieldedInstantly, bool isWieldedOnSpawn)
		{
			if (this.IsMainAgent)
			{
				Agent.OnMainAgentWieldedItemChangeDelegate onMainAgentWieldedItemChange = this.OnMainAgentWieldedItemChange;
				if (onMainAgentWieldedItemChange != null)
				{
					onMainAgentWieldedItemChange();
				}
			}
			Action onAgentWieldedItemChange = this.OnAgentWieldedItemChange;
			if (onAgentWieldedItemChange != null)
			{
				onAgentWieldedItemChange();
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				int num = 0;
				EquipmentIndex primaryWieldedItemIndex = this.GetPrimaryWieldedItemIndex();
				if (primaryWieldedItemIndex != EquipmentIndex.None)
				{
					num = this.Equipment[primaryWieldedItemIndex].CurrentUsageIndex;
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetWieldedItemIndex(this.Index, isOffHand, isWieldedInstantly, isWieldedOnSpawn, isOffHand ? this.GetOffhandWieldedItemIndex() : this.GetPrimaryWieldedItemIndex(), num));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.CheckEquipmentForCapeClothSimulationStateChange();
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00011485 File Offset: 0x0000F685
		public void StartRagdollAsCorpse()
		{
			MBAPI.IMBAgent.StartRagdollAsCorpse(this.GetPtr());
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00011497 File Offset: 0x0000F697
		public void EndRagdollAsCorpse()
		{
			MBAPI.IMBAgent.EndRagdollAsCorpse(this.GetPtr());
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000114A9 File Offset: 0x0000F6A9
		public bool IsAddedAsCorpse()
		{
			return MBAPI.IMBAgent.IsAddedAsCorpse(this.GetPtr());
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000114BB File Offset: 0x0000F6BB
		public void AddAsCorpse()
		{
			MBAPI.IMBAgent.AddAsCorpse(this.GetPtr());
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000114CD File Offset: 0x0000F6CD
		public void SetOverridenStrikeAndDeathAction(in ActionIndexCache strikeAction, in ActionIndexCache deathAction)
		{
			MBAPI.IMBAgent.SetOverridenStrikeAndDeathAction(this.GetPtr(), strikeAction.Index, deathAction.Index);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000114EB File Offset: 0x0000F6EB
		public void ApplyForceOnRagdoll(sbyte boneIndex, in Vec3 force)
		{
			MBAPI.IMBAgent.ApplyForceOnRagdoll(this.GetPtr(), boneIndex, in force);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x000114FF File Offset: 0x0000F6FF
		public void SetVelocityLimitsOnRagdoll(float linearVelocityLimit, float angularVelocityLimit)
		{
			MBAPI.IMBAgent.SetVelocityLimitsOnRagdoll(this.GetPtr(), linearVelocityLimit, angularVelocityLimit);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00011513 File Offset: 0x0000F713
		public WorldPosition GetAILastSuspiciousPosition()
		{
			return MBAPI.IMBAgent.GetAILastSuspiciousPosition(this.GetPtr());
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00011528 File Offset: 0x0000F728
		public void SetAILastSuspiciousPosition(WorldPosition lastSuspiciousPosition, bool checkNavMeshForCorrection)
		{
			Vec3 vec3WithoutValidity = lastSuspiciousPosition.GetVec3WithoutValidity();
			if (!checkNavMeshForCorrection)
			{
				MBAPI.IMBAgent.SetAILastSuspiciousPosition(this.GetPtr(), in lastSuspiciousPosition);
				return;
			}
			int num = 0;
			Vec2 vec = (this.Position.AsVec2 - lastSuspiciousPosition.AsVec2).Normalized();
			while (lastSuspiciousPosition.GetNavMesh() == UIntPtr.Zero && num < 15)
			{
				Vec2 vec2 = vec;
				if (num > 0)
				{
					vec2.RotateCCW(MBRandom.RandomFloat * (float)num * 0.3f - (float)num * 0.15f);
				}
				lastSuspiciousPosition.SetVec3(UIntPtr.Zero, vec3WithoutValidity, false);
				lastSuspiciousPosition.SetVec2(lastSuspiciousPosition.AsVec2 + vec2 * 0.4f * (float)(num + 2));
				num++;
			}
			if (lastSuspiciousPosition.GetNavMesh() != UIntPtr.Zero)
			{
				MBAPI.IMBAgent.SetAILastSuspiciousPosition(this.GetPtr(), in lastSuspiciousPosition);
				return;
			}
			WorldPosition worldPosition = this.GetWorldPosition();
			worldPosition.SetVec2(worldPosition.AsVec2 + (lastSuspiciousPosition.AsVec2 - this.Position.AsVec2).Normalized() * 0.1f);
			MBAPI.IMBAgent.SetAILastSuspiciousPosition(this.GetPtr(), in worldPosition);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00011675 File Offset: 0x0000F875
		public WorldPosition GetAIMoveDestination()
		{
			return MBAPI.IMBAgent.GetAIMoveDestination(this.GetPtr());
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00011687 File Offset: 0x0000F887
		public Vec2 FindLongestDirectMoveToPosition(Vec2 targetPosition, bool checkBoundaries, bool checkFriendlyAgents, out bool isCollidedWithAgent)
		{
			return MBAPI.IMBAgent.FindLongestDirectMoveToPosition(this.GetPtr(), targetPosition, checkBoundaries, checkFriendlyAgents, out isCollidedWithAgent);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0001169E File Offset: 0x0000F89E
		public float GetAIMoveStartTolerance()
		{
			return MBAPI.IMBAgent.GetAIMoveStopTolerance(this.GetPtr()) * 1.2f;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x000116B6 File Offset: 0x0000F8B6
		public float GetAIMoveStopTolerance()
		{
			return MBAPI.IMBAgent.GetAIMoveStopTolerance(this.GetPtr());
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000116C8 File Offset: 0x0000F8C8
		public unsafe bool GetBaseFormationFrame(out WorldPosition formationPosition, out Vec2 formationDirection)
		{
			bool flag = false;
			if (this.Formation != null && ((this.MovementMode & AgentMovementMode.WaterDiving) == AgentMovementMode.Land || this.Mission.IsTeleportingAgents))
			{
				formationPosition = this.Formation.GetOrderPositionOfUnit(this);
				if (this.IsDetachedFromFormation)
				{
					Formation formation = this.Formation;
					WorldFrame? worldFrame = null;
					MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
					if (movementOrder.MovementState != MovementOrder.MovementStateEnum.Charge || (this.Detachment != null && (!this.Detachment.IsLoose || formationPosition.IsValid)))
					{
						worldFrame = formation.GetDetachmentFrame(this);
					}
					if (worldFrame != null)
					{
						formationDirection = worldFrame.Value.Rotation.f.AsVec2.Normalized();
						flag = true;
					}
					else
					{
						formationDirection = Vec2.Invalid;
					}
				}
				else
				{
					formationDirection = this.Formation.GetDirectionOfUnit(this);
					flag = formationPosition.IsValid;
				}
			}
			else
			{
				formationPosition = WorldPosition.Invalid;
				formationDirection = Vec2.Invalid;
			}
			if (formationPosition.IsValid && formationPosition.GetNavMeshMT() == UIntPtr.Zero)
			{
				UIntPtr nearestNavMesh = formationPosition.GetNearestNavMesh();
				if (nearestNavMesh != UIntPtr.Zero)
				{
					Vec2 vec = Mission.Current.Scene.FindClosestExitPositionForPositionOnABoundaryFace(formationPosition.GetVec3WithoutValidity(), nearestNavMesh);
					if (vec.IsValid)
					{
						formationPosition.SetVec2(vec);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00011834 File Offset: 0x0000FA34
		public bool IsAIAtMoveDestination()
		{
			float aimoveStartTolerance = this.GetAIMoveStartTolerance();
			return this.GetAIMoveDestination().AsVec2.DistanceSquared(this.Position.AsVec2) <= aimoveStartTolerance * aimoveStartTolerance;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00011874 File Offset: 0x0000FA74
		public void SetFormationBanner(ItemObject banner)
		{
			this._formationBanner = banner;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001187D File Offset: 0x0000FA7D
		public void SetIsAIPaused(bool isPaused)
		{
			this.IsPaused = isPaused;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00011886 File Offset: 0x0000FA86
		public void ResetEnemyCaches()
		{
			MBAPI.IMBAgent.ResetEnemyCaches(this.GetPtr());
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00011898 File Offset: 0x0000FA98
		public void SetTargetPositionSynched(ref Vec2 targetPosition)
		{
			if (this.MovementLockedState == AgentMovementLockedState.None || this.GetTargetPosition() != targetPosition)
			{
				if (GameNetwork.IsClientOrReplay)
				{
					this._lastSynchedTargetPosition = targetPosition;
					this._checkIfTargetFrameIsChanged = true;
					return;
				}
				this.SetTargetPosition(targetPosition);
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetAgentTargetPosition(this.Index, ref targetPosition));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001190C File Offset: 0x0000FB0C
		public void SetTargetPositionAndDirectionSynched(ref Vec2 targetPosition, ref Vec3 targetDirection)
		{
			if (this.MovementLockedState == AgentMovementLockedState.None || this.GetTargetDirection() != targetDirection)
			{
				if (GameNetwork.IsClientOrReplay)
				{
					this._lastSynchedTargetDirection = targetDirection;
					this._checkIfTargetFrameIsChanged = true;
					return;
				}
				this.SetTargetPositionAndDirection(in targetPosition, in targetDirection);
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetAgentTargetPositionAndDirection(this.Index, ref targetPosition, ref targetDirection));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001197D File Offset: 0x0000FB7D
		public void SetBodyArmorMaterialType(ArmorComponent.ArmorMaterialTypes bodyArmorMaterialType)
		{
			MBAPI.IMBAgent.SetBodyArmorMaterialType(this.GetPtr(), bodyArmorMaterialType);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00011990 File Offset: 0x0000FB90
		public void SetUsedGameObjectForClient(UsableMissionObject usedObject)
		{
			this.CurrentlyUsedGameObject = usedObject;
			usedObject.OnUse(this, -1);
			this.Mission.OnObjectUsed(this, usedObject);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000119B0 File Offset: 0x0000FBB0
		public void SetTeam(Team team, bool sync)
		{
			if (this.Team != team)
			{
				Team team2 = this.Team;
				Team team3 = this.Team;
				if (team3 != null)
				{
					team3.RemoveAgentFromTeam(this);
				}
				this.Team = team;
				Team team4 = this.Team;
				if (team4 != null)
				{
					team4.AddAgentToTeam(this);
				}
				this.SetTeamInternal((team != null) ? team.MBTeam : MBTeam.InvalidTeam);
				if (sync && GameNetwork.IsServer && this.Mission.HasMissionBehavior<MissionNetworkComponent>())
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AgentSetTeam(this.Index, (team != null) ? team.TeamIndex : (-1)));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				foreach (MissionBehavior missionBehavior in Mission.Current.MissionBehaviors)
				{
					missionBehavior.OnAgentTeamChanged(team2, team, this);
				}
			}
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00011A9C File Offset: 0x0000FC9C
		public void SetClothingColor1(uint color)
		{
			this._clothingColor1 = new uint?(color);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00011AAA File Offset: 0x0000FCAA
		public void SetClothingColor2(uint color)
		{
			this._clothingColor2 = new uint?(color);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		public void SetWieldedItemIndexAsClient(Agent.HandIndex handIndex, EquipmentIndex equipmentIndex, bool isWieldedInstantly, bool isWieldedOnSpawn, int mainHandCurrentUsageIndex)
		{
			MBAPI.IMBAgent.SetWieldedItemIndexAsClient(this.GetPtr(), (int)handIndex, (int)equipmentIndex, isWieldedInstantly, isWieldedOnSpawn, mainHandCurrentUsageIndex);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00011AD1 File Offset: 0x0000FCD1
		public void SetPreciseRangedAimingEnabled(bool set)
		{
			if (set)
			{
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.RangerCanMoveForClearTarget);
				return;
			}
			this.SetScriptedFlags(this.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.RangerCanMoveForClearTarget);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00011AFB File Offset: 0x0000FCFB
		public void SetAsConversationAgent(bool set)
		{
			if (set)
			{
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.InConversation);
				this.DisableLookToPointOfInterest();
				return;
			}
			this.SetScriptedFlags(this.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.InConversation);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00011B2C File Offset: 0x0000FD2C
		public void OnConversationStarted()
		{
			this.SetActionChannel(0, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_reload, 0f, 1f, 0f, 0.4f, 0f, false, -0.2f, 0, true);
			this.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_reload, 0f, 1f, 0f, 0.4f, 0f, false, -0.2f, 0, true);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00011B9D File Offset: 0x0000FD9D
		public void SetCrouchMode(bool set)
		{
			if (set)
			{
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.Crouch);
				return;
			}
			this.SetScriptedFlags(this.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.Crouch);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00011BC7 File Offset: 0x0000FDC7
		public void SetWeaponAmountInSlot(EquipmentIndex equipmentSlot, short amount, bool enforcePrimaryItem)
		{
			MBAPI.IMBAgent.SetWeaponAmountInSlot(this.GetPtr(), (int)equipmentSlot, amount, enforcePrimaryItem);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00011BDC File Offset: 0x0000FDDC
		public void SetDraggingMode(bool set)
		{
			if (set)
			{
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.Drag);
				return;
			}
			this.SetScriptedFlags(this.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.Drag);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00011C06 File Offset: 0x0000FE06
		public void SetWeaponAmmoAsClient(EquipmentIndex equipmentIndex, EquipmentIndex ammoEquipmentIndex, short ammo)
		{
			MBAPI.IMBAgent.SetWeaponAmmoAsClient(this.GetPtr(), (int)equipmentIndex, (int)ammoEquipmentIndex, ammo);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00011C1B File Offset: 0x0000FE1B
		public void SetWeaponReloadPhaseAsClient(EquipmentIndex equipmentIndex, short reloadState)
		{
			MBAPI.IMBAgent.SetWeaponReloadPhaseAsClient(this.GetPtr(), (int)equipmentIndex, reloadState);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00011C2F File Offset: 0x0000FE2F
		public void SetReloadAmmoInSlot(EquipmentIndex equipmentIndex, EquipmentIndex ammoSlotIndex, short reloadedAmmo)
		{
			MBAPI.IMBAgent.SetReloadAmmoInSlot(this.GetPtr(), (int)equipmentIndex, (int)ammoSlotIndex, reloadedAmmo);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00011C44 File Offset: 0x0000FE44
		public void SetUsageIndexOfWeaponInSlotAsClient(EquipmentIndex slotIndex, int usageIndex)
		{
			MBAPI.IMBAgent.SetUsageIndexOfWeaponInSlotAsClient(this.GetPtr(), (int)slotIndex, usageIndex);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00011C58 File Offset: 0x0000FE58
		public void SetRandomizeColors(bool shouldRandomize)
		{
			this.RandomizeColors = shouldRandomize;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00011C61 File Offset: 0x0000FE61
		[MBCallback(null, false)]
		internal void OnRemoveWeapon(EquipmentIndex slotIndex)
		{
			this.RemoveEquippedWeapon(slotIndex);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00011C6C File Offset: 0x0000FE6C
		public void SetFormationFrameDisabled()
		{
			MBAPI.IMBAgent.SetFormationFrameDisabled(this.GetPtr());
			if (this.IsFormationFrameEnabled)
			{
				this.IsFormationFrameEnabled = false;
				this._changedFormationPosition = new WorldPosition(this.Mission.Scene, UIntPtr.Zero, Vec3.Zero, false);
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00011CBC File Offset: 0x0000FEBC
		public void SetFormationFrameEnabled(WorldPosition position, Vec2 direction, Vec2 positionVelocity, float formationDirectionEnforcingFactor)
		{
			bool flag = MBAPI.IMBAgent.SetFormationFrameEnabled(this.GetPtr(), position, direction, positionVelocity, formationDirectionEnforcingFactor, this.Mission.IsTeleportingAgents);
			if (!this.IsFormationFrameEnabled)
			{
				flag = true;
				this.IsFormationFrameEnabled = true;
			}
			if (flag)
			{
				this._changedFormationPosition = position;
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00011D05 File Offset: 0x0000FF05
		public void SetShouldCatchUpWithFormation(bool value)
		{
			MBAPI.IMBAgent.SetShouldCatchUpWithFormation(this.GetPtr(), value);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00011D18 File Offset: 0x0000FF18
		public void SetFormationIntegrityData(Vec2 position, Vec2 currentFormationDirection, Vec2 averageVelocityOfCloseAgents, float averageMaxUnlimitedSpeedOfCloseAgents, float deviationOfPositions, bool shouldKeepWithFormationInsteadOfMovingToAgent)
		{
			MBAPI.IMBAgent.SetFormationIntegrityData(this.GetPtr(), in position, in currentFormationDirection, in averageVelocityOfCloseAgents, averageMaxUnlimitedSpeedOfCloseAgents, deviationOfPositions, shouldKeepWithFormationInsteadOfMovingToAgent);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00011D36 File Offset: 0x0000FF36
		public bool IsCrouchingAllowed()
		{
			return MBAPI.IMBAgent.IsCrouchingAllowed(this.GetPtr());
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00011D48 File Offset: 0x0000FF48
		[MBCallback(null, false)]
		internal void OnWeaponUsageIndexChange(EquipmentIndex slotIndex, int usageIndex)
		{
			this.Equipment.SetUsageIndexOfSlot(slotIndex, usageIndex);
			this.UpdateAgentProperties();
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new WeaponUsageIndexChangeMessage(this.Index, slotIndex, usageIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00011D83 File Offset: 0x0000FF83
		public void SetCurrentActionProgress(int channelNo, float progress)
		{
			MBAPI.IMBAgent.SetCurrentActionProgress(this.GetPtr(), channelNo, progress);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00011D97 File Offset: 0x0000FF97
		public void SetCurrentActionSpeed(int channelNo, float speed)
		{
			MBAPI.IMBAgent.SetCurrentActionSpeed(this.GetPtr(), channelNo, speed);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00011DAC File Offset: 0x0000FFAC
		public bool SetActionChannel(int channelNo, in ActionIndexCache actionIndexCache, bool ignorePriority = false, AnimFlags additionalFlags = (AnimFlags)0UL, float blendWithNextActionFactor = 0f, float actionSpeed = 1f, float blendInPeriod = -0.2f, float blendOutPeriodToNoAnim = 0.4f, float startProgress = 0f, bool useLinearSmoothing = false, float blendOutPeriod = -0.2f, int actionShift = 0, bool forceFaceMorphRestart = true)
		{
			int index = actionIndexCache.Index;
			return MBAPI.IMBAgent.SetActionChannel(this.GetPtr(), channelNo, index + actionShift, (ulong)additionalFlags, ignorePriority, blendWithNextActionFactor, actionSpeed, blendInPeriod, blendOutPeriodToNoAnim, startProgress, useLinearSmoothing, blendOutPeriod, forceFaceMorphRestart);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00011DE8 File Offset: 0x0000FFE8
		[MBCallback(null, false)]
		internal void OnWeaponAmountChange(EquipmentIndex slotIndex, short amount)
		{
			this.Equipment.SetAmountOfSlot(slotIndex, amount, false);
			this.UpdateAgentProperties();
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetWeaponNetworkData(this.Index, slotIndex, amount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00011E24 File Offset: 0x00010024
		public void SetAttackState(int attackState)
		{
			MBAPI.IMBAgent.SetAttackState(this.GetPtr(), attackState);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00011E37 File Offset: 0x00010037
		public void SetAIBehaviorParams(HumanAIComponent.AISimpleBehaviorKind behavior, float y1, float x2, float y2, float x3, float y3)
		{
			MBAPI.IMBAgent.SetAIBehaviorParams(this.GetPtr(), (int)behavior, y1, x2, y2, x3, y3);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00011E52 File Offset: 0x00010052
		public void SetAllBehaviorParams(HumanAIComponent.BehaviorValues[] behaviorParams)
		{
			MBAPI.IMBAgent.SetAllAIBehaviorParams(this.GetPtr(), behaviorParams);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00011E65 File Offset: 0x00010065
		public void SetMovementDirection(in Vec2 direction)
		{
			MBAPI.IMBAgent.SetMovementDirection(this.GetPtr(), in direction);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00011E78 File Offset: 0x00010078
		public void SetScriptedFlags(Agent.AIScriptedFrameFlags flags)
		{
			MBAPI.IMBAgent.SetScriptedFlags(this.GetPtr(), (int)flags);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00011E8B File Offset: 0x0001008B
		public void SetScriptedCombatFlags(Agent.AISpecialCombatModeFlags flags)
		{
			MBAPI.IMBAgent.SetScriptedCombatFlags(this.GetPtr(), (int)flags);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00011EA0 File Offset: 0x000100A0
		public void SetScriptedPositionAndDirection(ref WorldPosition scriptedPosition, float scriptedDirection, bool addHumanLikeDelay, Agent.AIScriptedFrameFlags additionalFlags = Agent.AIScriptedFrameFlags.None)
		{
			MBAPI.IMBAgent.SetScriptedPositionAndDirection(this.GetPtr(), ref scriptedPosition, scriptedDirection, addHumanLikeDelay, (int)additionalFlags);
			if (this.Mission.IsTeleportingAgents && scriptedPosition.AsVec2 != this.Position.AsVec2)
			{
				this.TeleportToPosition(scriptedPosition.GetGroundVec3());
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00011EF8 File Offset: 0x000100F8
		public void SetScriptedPosition(ref WorldPosition position, bool addHumanLikeDelay, Agent.AIScriptedFrameFlags additionalFlags = Agent.AIScriptedFrameFlags.None)
		{
			MBAPI.IMBAgent.SetScriptedPosition(this.GetPtr(), ref position, addHumanLikeDelay, (int)additionalFlags);
			if (this.Mission.IsTeleportingAgents && position.AsVec2 != this.Position.AsVec2)
			{
				this.TeleportToPosition(position.GetGroundVec3());
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00011F4D File Offset: 0x0001014D
		public void SetScriptedTargetEntity(WeakGameEntity target, Agent.AISpecialCombatModeFlags additionalFlags = Agent.AISpecialCombatModeFlags.None, bool ignoreIfAlreadyAttacking = false)
		{
			MBAPI.IMBAgent.SetScriptedTargetEntity(this.GetPtr(), target.Pointer, (int)additionalFlags, ignoreIfAlreadyAttacking);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00011F68 File Offset: 0x00010168
		public void SetAgentExcludeStateForFaceGroupId(int faceGroupId, bool isExcluded)
		{
			MBAPI.IMBAgent.SetAgentExcludeStateForFaceGroupId(this.GetPtr(), faceGroupId, isExcluded);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00011F7C File Offset: 0x0001017C
		public void SetLookAgent(Agent agent)
		{
			this._lookAgentCache = agent;
			MBAPI.IMBAgent.SetLookAgent(this.GetPtr(), (agent != null) ? agent.GetPtr() : UIntPtr.Zero);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00011FA5 File Offset: 0x000101A5
		public void SetInteractionAgent(Agent agent)
		{
			MBAPI.IMBAgent.SetInteractionAgent(this.GetPtr(), (agent != null) ? agent.GetPtr() : UIntPtr.Zero);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00011FC7 File Offset: 0x000101C7
		public void SetLookToPointOfInterest(Vec3 point)
		{
			MBAPI.IMBAgent.SetLookToPointOfInterest(this.GetPtr(), point);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00011FDA File Offset: 0x000101DA
		public void SetAgentFlags(AgentFlag agentFlags)
		{
			MBAPI.IMBAgent.SetAgentFlags(this.GetPtr(), (uint)agentFlags);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00011FED File Offset: 0x000101ED
		public void SetSelectedMountIndex(int mountIndex)
		{
			MBAPI.IMBAgent.SetSelectedMountIndex(this.GetPtr(), mountIndex);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00012000 File Offset: 0x00010200
		public int GetFiringOrder()
		{
			return MBAPI.IMBAgent.GetFiringOrder(this.GetPtr());
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00012012 File Offset: 0x00010212
		public int GetRidingOrder()
		{
			return MBAPI.IMBAgent.GetRidingOrder(this.GetPtr());
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00012024 File Offset: 0x00010224
		public int GetSelectedMountIndex()
		{
			return MBAPI.IMBAgent.GetSelectedMountIndex(this.GetPtr());
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00012036 File Offset: 0x00010236
		public int GetTargetFormationIndex()
		{
			return MBAPI.IMBAgent.GetTargetFormationIndex(this.GetPtr());
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00012048 File Offset: 0x00010248
		public void SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum order)
		{
			MBAPI.IMBAgent.SetFiringOrder(this.GetPtr(), (int)order);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0001205B File Offset: 0x0001025B
		public void SetRidingOrder(RidingOrder.RidingOrderEnum order)
		{
			MBAPI.IMBAgent.SetRidingOrder(this.GetPtr(), (int)order);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0001206E File Offset: 0x0001026E
		public void SetAgentFacialAnimation(Agent.FacialAnimChannel channel, string animationName, bool loop)
		{
			MBAPI.IMBAgent.SetAgentFacialAnimation(this.GetPtr(), (int)channel, animationName, loop);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00012083 File Offset: 0x00010283
		public bool SetHandInverseKinematicsFrame(in MatrixFrame leftGlobalFrame, in MatrixFrame rightGlobalFrame)
		{
			return MBAPI.IMBAgent.SetHandInverseKinematicsFrame(this.GetPtr(), in leftGlobalFrame, in rightGlobalFrame);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00012097 File Offset: 0x00010297
		public void SetNativeFormationNo(int formationNo)
		{
			MBAPI.IMBAgent.SetFormationNo(this.GetPtr(), formationNo);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000120AA File Offset: 0x000102AA
		public void SetDirectionChangeTendency(float tendency)
		{
			MBAPI.IMBAgent.SetDirectionChangeTendency(this.GetPtr(), tendency);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000120C0 File Offset: 0x000102C0
		public float GetBattleImportance()
		{
			BasicCharacterObject character = this.Character;
			float num = ((character != null) ? character.GetBattlePower() : 1f);
			if (this.Team != null && this == this.Team.GeneralAgent)
			{
				num *= 2f;
			}
			else if (this.Formation != null && this == this.Formation.Captain)
			{
				num *= 1.2f;
			}
			return num;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00012124 File Offset: 0x00010324
		public TroopTraitsMask GetTraitsMask()
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (this.HasMount)
			{
				troopTraitsMask |= TroopTraitsMask.Mount;
			}
			if (this.IsRangedCached)
			{
				troopTraitsMask |= TroopTraitsMask.Ranged;
			}
			else
			{
				troopTraitsMask |= TroopTraitsMask.Melee;
			}
			if (this.HasShieldCached)
			{
				troopTraitsMask |= TroopTraitsMask.Shield;
			}
			if (this.HasSpearCached)
			{
				troopTraitsMask |= TroopTraitsMask.Spear;
			}
			if (this.HasThrownCached)
			{
				troopTraitsMask |= TroopTraitsMask.Thrown;
			}
			if (MissionGameModels.Current.AgentStatCalculateModel.HasHeavyArmor(this))
			{
				troopTraitsMask |= TroopTraitsMask.Armor;
			}
			return troopTraitsMask;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00012190 File Offset: 0x00010390
		public void SetSynchedPrefabComponentVisibility(int componentIndex, bool visibility)
		{
			this._synchedBodyComponents[componentIndex].SetVisible(visibility);
			this.AgentVisuals.LazyUpdateAgentRendererData();
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetAgentPrefabComponentVisibility(this.Index, componentIndex, visibility));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000121E0 File Offset: 0x000103E0
		public void SetActionSet(ref AnimationSystemData animationSystemData)
		{
			MBAPI.IMBAgent.SetActionSet(this.GetPtr(), ref animationSystemData);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetAgentActionSet(this.Index, animationSystemData));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00012220 File Offset: 0x00010420
		public void SetColumnwiseFollowAgent(Agent followAgent, ref Vec2 followPosition)
		{
			if (!this.IsAIControlled)
			{
				return;
			}
			int num = ((followAgent != null) ? followAgent.Index : (-1));
			MBAPI.IMBAgent.SetColumnwiseFollowAgent(this.GetPtr(), num, ref followPosition);
			this.SetFollowedUnit(followAgent);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001225C File Offset: 0x0001045C
		public void SetHandInverseKinematicsFrameForMissionObjectUsage(in MatrixFrame localIKFrame, in MatrixFrame boundEntityGlobalFrame, float animationHeightDifference = 0f)
		{
			if (this.GetCurrentAction(1) != ActionIndexCache.act_none && this.GetActionChannelWeight(1) > 0f)
			{
				MBAPI.IMBAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(this.GetPtr(), in localIKFrame, in boundEntityGlobalFrame, animationHeightDifference);
				return;
			}
			this.ClearHandInverseKinematics();
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0001229A File Offset: 0x0001049A
		public void SetWantsToYell()
		{
			this._wantsToYell = true;
			this._yellTimer = MBRandom.RandomFloat * 0.3f + 0.1f;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000122BC File Offset: 0x000104BC
		public void SetCapeClothSimulator(GameEntityComponent clothSimulatorComponent)
		{
			ClothSimulatorComponent clothSimulatorComponent2 = clothSimulatorComponent as ClothSimulatorComponent;
			this._capeClothSimulator = clothSimulatorComponent2;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x000122D7 File Offset: 0x000104D7
		public Vec2 GetTargetPosition()
		{
			return MBAPI.IMBAgent.GetTargetPosition(this.GetPtr());
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000122E9 File Offset: 0x000104E9
		public Vec3 GetTargetDirection()
		{
			return MBAPI.IMBAgent.GetTargetDirection(this.GetPtr());
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000122FB File Offset: 0x000104FB
		public float GetAimingTimer()
		{
			return MBAPI.IMBAgent.GetAimingTimer(this.GetPtr());
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00012310 File Offset: 0x00010510
		public float GetInteractionDistanceToUsable(IUsable usable)
		{
			Agent agent;
			if ((agent = usable as Agent) != null && agent.IsActive())
			{
				if (!agent.IsMount)
				{
					return 3f;
				}
				return 1.75f;
			}
			else
			{
				SpawnedItemEntity spawnedItemEntity;
				if ((spawnedItemEntity = usable as SpawnedItemEntity) != null && spawnedItemEntity.IsBanner())
				{
					return 3f;
				}
				StandingPoint standingPoint;
				if ((standingPoint = usable as StandingPoint) == null)
				{
					return MissionGameModels.Current.AgentStatCalculateModel.GetInteractionDistance(this);
				}
				if (!this.IsAIControlled)
				{
					if (standingPoint.CustomPlayerInteractionDistance <= 0f)
					{
						return 2f;
					}
					return standingPoint.CustomPlayerInteractionDistance;
				}
				else
				{
					if (!this.WalkMode || !this.MovementMode.HasAnyFlag(AgentMovementMode.Land))
					{
						return 1f;
					}
					return 0.5f;
				}
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000123B9 File Offset: 0x000105B9
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			if (this.IsMount && !userAgent.CheckSkillForMounting(this))
			{
				return GameTexts.FindText("str_ui_riding_skill_not_adequate_to_mount", null);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000123E0 File Offset: 0x000105E0
		public T GetController<T>() where T : AgentController
		{
			for (int i = 0; i < this._agentControllers.Count; i++)
			{
				if (this._agentControllers[i] is T)
				{
					return (T)((object)this._agentControllers[i]);
				}
			}
			return default(T);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00012431 File Offset: 0x00010631
		public EquipmentIndex GetPrimaryWieldedItemIndex()
		{
			return AgentHelper.GetPrimaryWieldedItemIndex(this._primaryWieldedItemIndexPointer);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001243E File Offset: 0x0001063E
		public EquipmentIndex GetOffhandWieldedItemIndex()
		{
			return AgentHelper.GetOffhandWieldedItemIndex(this._offHandWieldedItemIndexPointer);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0001244B File Offset: 0x0001064B
		public float GetMaximumForwardUnlimitedSpeed()
		{
			return AgentHelper.GetMaximumForwardUnlimitedSpeed(this._maximumForwardUnlimitedSpeed);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00012458 File Offset: 0x00010658
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return this.NameTextObject;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00012460 File Offset: 0x00010660
		public WeakGameEntity GetWeaponEntityFromEquipmentSlot(EquipmentIndex slotIndex)
		{
			return new WeakGameEntity(MBAPI.IMBAgent.GetWeaponEntityFromEquipmentSlot(this.GetPtr(), (int)slotIndex));
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00012478 File Offset: 0x00010678
		public WorldPosition GetRetreatPos()
		{
			return MBAPI.IMBAgent.GetRetreatPos(this.GetPtr());
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0001248A File Offset: 0x0001068A
		public Agent.AIScriptedFrameFlags GetScriptedFlags()
		{
			return (Agent.AIScriptedFrameFlags)MBAPI.IMBAgent.GetScriptedFlags(this.GetPtr());
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0001249C File Offset: 0x0001069C
		public Agent.AISpecialCombatModeFlags GetScriptedCombatFlags()
		{
			return (Agent.AISpecialCombatModeFlags)MBAPI.IMBAgent.GetScriptedCombatFlags(this.GetPtr());
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000124AE File Offset: 0x000106AE
		public WeakGameEntity GetSteppedEntity()
		{
			return new WeakGameEntity(MBAPI.IMBAgent.GetSteppedEntityId(this.GetPtr()));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x000124C5 File Offset: 0x000106C5
		public WeakGameEntity GetSteppedRootEntity()
		{
			return new WeakGameEntity(MBAPI.IMBAgent.GetSteppedRootEntity(this.GetPtr()));
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000124DC File Offset: 0x000106DC
		public BodyFlags GetSteppedBodyFlags()
		{
			return MBAPI.IMBAgent.GetSteppedBodyFlags(this.GetPtr());
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000124EE File Offset: 0x000106EE
		public AnimFlags GetCurrentAnimationFlag(int channelNo)
		{
			return (AnimFlags)MBAPI.IMBAgent.GetCurrentAnimationFlags(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00012501 File Offset: 0x00010701
		public ActionIndexCache GetCurrentAction(int channelNo)
		{
			return new ActionIndexCache((channelNo == 0) ? AgentHelper.GetChannel0CurrentActionIndex(this._channel0CurrentActionPointer) : AgentHelper.GetChannel1CurrentActionIndex(this._channel1CurrentActionPointer));
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00012523 File Offset: 0x00010723
		public Agent.ActionCodeType GetCurrentActionType(int channelNo)
		{
			return (Agent.ActionCodeType)MBAPI.IMBAgent.GetCurrentActionType(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00012536 File Offset: 0x00010736
		public Agent.ActionStage GetCurrentActionStage(int channelNo)
		{
			return (Agent.ActionStage)MBAPI.IMBAgent.GetCurrentActionStage(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00012549 File Offset: 0x00010749
		public Agent.UsageDirection GetCurrentActionDirection(int channelNo)
		{
			return (Agent.UsageDirection)MBAPI.IMBAgent.GetCurrentActionDirection(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001255C File Offset: 0x0001075C
		public int GetCurrentActionPriority(int channelNo)
		{
			return MBAPI.IMBAgent.GetCurrentActionPriority(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001256F File Offset: 0x0001076F
		public float GetCurrentActionProgress(int channelNo)
		{
			return MBAPI.IMBAgent.GetCurrentActionProgress(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00012582 File Offset: 0x00010782
		public float GetActionChannelWeight(int channelNo)
		{
			return MBAPI.IMBAgent.GetActionChannelWeight(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00012595 File Offset: 0x00010795
		public float GetActionChannelCurrentActionWeight(int channelNo)
		{
			return MBAPI.IMBAgent.GetActionChannelCurrentActionWeight(this.GetPtr(), channelNo);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000125A8 File Offset: 0x000107A8
		public WorldFrame GetWorldFrame()
		{
			return new WorldFrame(this.LookRotation, this.GetWorldPosition());
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000125BB File Offset: 0x000107BB
		public float GetLookDownLimit()
		{
			return MBAPI.IMBAgent.GetLookDownLimit(this.GetPtr());
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000125CD File Offset: 0x000107CD
		public float GetEyeGlobalHeight()
		{
			return MBAPI.IMBAgent.GetEyeGlobalHeight(this.GetPtr());
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000125DF File Offset: 0x000107DF
		public float GetMaximumSpeedLimit()
		{
			return MBAPI.IMBAgent.GetMaximumSpeedLimit(this.GetPtr());
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000125F1 File Offset: 0x000107F1
		public Vec2 GetCurrentVelocity()
		{
			return MBAPI.IMBAgent.GetCurrentVelocity(this.GetPtr());
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00012603 File Offset: 0x00010803
		public float GetTurnSpeed()
		{
			return MBAPI.IMBAgent.GetTurnSpeed(this.GetPtr());
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00012615 File Offset: 0x00010815
		public float GetCurrentSpeedLimit()
		{
			return MBAPI.IMBAgent.GetCurrentSpeedLimit(this.GetPtr());
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00012627 File Offset: 0x00010827
		public Vec3 GetRealGlobalVelocity()
		{
			return MBAPI.IMBAgent.GetRealGlobalVelocity(this.GetPtr());
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00012639 File Offset: 0x00010839
		public Vec3 GetAverageRealGlobalVelocity()
		{
			return MBAPI.IMBAgent.GetAverageRealGlobalVelocity(this.GetPtr());
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001264B File Offset: 0x0001084B
		public Vec2 GetMovementDirection()
		{
			return Vec2.FromRotation(this.MovementDirectionAsAngle);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00012658 File Offset: 0x00010858
		public Vec3 GetCurWeaponOffset()
		{
			return MBAPI.IMBAgent.GetCurWeaponOffset(this.GetPtr());
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001266A File Offset: 0x0001086A
		public bool GetIsLeftStance()
		{
			return MBAPI.IMBAgent.GetIsLeftStance(this.GetPtr());
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001267C File Offset: 0x0001087C
		public float GetPathDistanceToPoint(ref Vec3 point)
		{
			return MBAPI.IMBAgent.GetPathDistanceToPoint(this.GetPtr(), ref point);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001268F File Offset: 0x0001088F
		public int GetCurrentNavigationFaceId()
		{
			return MBAPI.IMBAgent.GetCurrentNavigationFaceId(this.GetPtr());
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000126A1 File Offset: 0x000108A1
		public WorldPosition GetWorldPosition()
		{
			return MBAPI.IMBAgent.GetWorldPosition(this.GetPtr());
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000126B3 File Offset: 0x000108B3
		public int GetGroundMaterialForCollisionEffect()
		{
			return MBAPI.IMBAgent.GetGroundMaterialForCollisionEffect(this.GetPtr());
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000126C5 File Offset: 0x000108C5
		public Agent GetLookAgent()
		{
			return this._lookAgentCache;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x000126CD File Offset: 0x000108CD
		public Agent GetTargetAgent()
		{
			return MBAPI.IMBAgent.GetTargetAgent(this.GetPtr());
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000126DF File Offset: 0x000108DF
		public void SetTargetAgent(Agent agent)
		{
			MBAPI.IMBAgent.SetTargetAgent(this.GetPtr(), (agent != null) ? agent.Index : (-1));
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000126FD File Offset: 0x000108FD
		public void SetAutomaticTargetSelection(bool enable)
		{
			MBAPI.IMBAgent.SetAutomaticTargetSelection(this.GetPtr(), enable);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00012710 File Offset: 0x00010910
		public AgentFlag GetAgentFlags()
		{
			return AgentHelper.GetAgentFlags(this.FlagsPointer);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0001271D File Offset: 0x0001091D
		public string GetAgentFacialAnimation()
		{
			return MBAPI.IMBAgent.GetAgentFacialAnimation(this.GetPtr());
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001272F File Offset: 0x0001092F
		public string GetAgentVoiceDefinition()
		{
			return MBAPI.IMBAgent.GetAgentVoiceDefinition(this.GetPtr());
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00012741 File Offset: 0x00010941
		public Vec3 GetEyeGlobalPosition()
		{
			return MBAPI.IMBAgent.GetEyeGlobalPosition(this.GetPtr());
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00012753 File Offset: 0x00010953
		public Vec3 GetChestGlobalPosition()
		{
			return MBAPI.IMBAgent.GetChestGlobalPosition(this.GetPtr());
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00012765 File Offset: 0x00010965
		public Agent.MovementControlFlag GetDefendMovementFlag()
		{
			return MBAPI.IMBAgent.GetDefendMovementFlag(this.GetPtr());
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00012777 File Offset: 0x00010977
		public Agent.UsageDirection GetAttackDirection()
		{
			return MBAPI.IMBAgent.GetAttackDirection(this.GetPtr());
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001278C File Offset: 0x0001098C
		public WeaponInfo GetWieldedWeaponInfo(Agent.HandIndex handIndex)
		{
			bool flag = false;
			bool flag2 = false;
			if (MBAPI.IMBAgent.GetWieldedWeaponInfo(this.GetPtr(), (int)handIndex, ref flag, ref flag2))
			{
				return new WeaponInfo(true, flag, flag2);
			}
			return new WeaponInfo(false, false, false);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000127C8 File Offset: 0x000109C8
		public Vec2 GetBodyRotationConstraint(int channelIndex = 1)
		{
			return MBAPI.IMBAgent.GetBodyRotationConstraint(this.GetPtr(), channelIndex).AsVec2;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000127EE File Offset: 0x000109EE
		public float GetTotalEncumbrance()
		{
			return this.AgentDrivenProperties.ArmorEncumbrance + this.AgentDrivenProperties.WeaponsEncumbrance;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00012807 File Offset: 0x00010A07
		public float GetTotalMass()
		{
			return MBAPI.IMBAgent.GetTotalMass(this.GetPtr());
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0001281C File Offset: 0x00010A1C
		public T GetComponent<T>() where T : AgentComponent
		{
			for (int i = 0; i < this._components.Count; i++)
			{
				if (this._components[i] is T)
				{
					return (T)((object)this._components[i]);
				}
			}
			return default(T);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0001286D File Offset: 0x00010A6D
		public float GetAgentDrivenPropertyValue(DrivenProperty type)
		{
			return this.AgentDrivenProperties.GetStat(type);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001287C File Offset: 0x00010A7C
		public UsableMachine GetSteppedMachine()
		{
			WeakGameEntity weakGameEntity = this.GetSteppedEntity();
			while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			if (weakGameEntity.IsValid)
			{
				return weakGameEntity.GetFirstScriptOfType<UsableMachine>();
			}
			return null;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000128BE File Offset: 0x00010ABE
		public int GetAttachedWeaponsCount()
		{
			List<ValueTuple<MissionWeapon, MatrixFrame, sbyte>> attachedWeapons = this._attachedWeapons;
			if (attachedWeapons == null)
			{
				return 0;
			}
			return attachedWeapons.Count;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000128D1 File Offset: 0x00010AD1
		public MissionWeapon GetAttachedWeapon(int index)
		{
			return this._attachedWeapons[index].Item1;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000128E4 File Offset: 0x00010AE4
		public MatrixFrame GetAttachedWeaponFrame(int index)
		{
			return this._attachedWeapons[index].Item2;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000128F7 File Offset: 0x00010AF7
		public sbyte GetAttachedWeaponBoneIndex(int index)
		{
			return this._attachedWeapons[index].Item3;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0001290A File Offset: 0x00010B0A
		public void DeleteAttachedWeapon(int index)
		{
			this._attachedWeapons.RemoveAt(index);
			MBAPI.IMBAgent.DeleteAttachedWeaponFromBone(this.GetPtr(), index);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0001292C File Offset: 0x00010B2C
		public bool HasRangedWeapon(bool checkHasAmmo = false)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				int num;
				bool flag;
				bool flag2;
				if (!this.Equipment[equipmentIndex].IsEmpty && this.Equipment[equipmentIndex].GetRangedUsageIndex() >= 0 && (!checkHasAmmo || this.Equipment.HasAmmo(equipmentIndex, out num, out flag, out flag2)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0001298C File Offset: 0x00010B8C
		public MatrixFrame GetBoneEntitialFrameAtAnimationProgress(sbyte boneIndex, int animationIndex, float progress)
		{
			return MBAPI.IMBAgent.GetBoneEntitialFrameAtAnimationProgress(this.GetPtr(), boneIndex, animationIndex, progress);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000129A4 File Offset: 0x00010BA4
		public MatrixFrame GetBoneEntitialFrame(sbyte boneIndex, bool useBoneMapping)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			MBAPI.IMBAgent.GetBoneEntitialFrame(this.GetPtr(), boneIndex, useBoneMapping, ref identity);
			return identity;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000129CC File Offset: 0x00010BCC
		public void GetFormationFileAndRankInfo(out int fileIndex, out int rankIndex)
		{
			fileIndex = ((IFormationUnit)this).FormationFileIndex;
			rankIndex = ((IFormationUnit)this).FormationRankIndex;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000129EC File Offset: 0x00010BEC
		public void GetFormationFileAndRankInfo(out int fileIndex, out int rankIndex, out int fileCount, out int rankCount)
		{
			fileIndex = ((IFormationUnit)this).FormationFileIndex;
			rankIndex = ((IFormationUnit)this).FormationRankIndex;
			LineFormation lineFormation;
			if ((lineFormation = ((IFormationUnit)this).Formation as LineFormation) != null)
			{
				lineFormation.GetFormationInfo(out fileCount, out rankCount);
				return;
			}
			fileCount = -1;
			rankCount = -1;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00012A2B File Offset: 0x00010C2B
		internal Vec2 GetWallDirectionOfRelativeFormationLocation()
		{
			return this.Formation.GetWallDirectionOfRelativeFormationLocation(this);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00012A39 File Offset: 0x00010C39
		public void SetMortalityState(Agent.MortalityState newState)
		{
			this.CurrentMortalityState = newState;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00012A42 File Offset: 0x00010C42
		public void ToggleInvulnerable()
		{
			if (this.CurrentMortalityState == Agent.MortalityState.Invulnerable)
			{
				this.CurrentMortalityState = Agent.MortalityState.Mortal;
				return;
			}
			this.CurrentMortalityState = Agent.MortalityState.Invulnerable;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00012A5C File Offset: 0x00010C5C
		public float GetArmLength()
		{
			return this.Monster.ArmLength * this.AgentScale;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00012A70 File Offset: 0x00010C70
		public float GetArmWeight()
		{
			return this.Monster.ArmWeight * this.AgentScale;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00012A84 File Offset: 0x00010C84
		public void GetRunningSimulationDataUntilMaximumSpeedReached(ref float combatAccelerationTime, ref float maxSpeed, float[] speedValues)
		{
			MBAPI.IMBAgent.GetRunningSimulationDataUntilMaximumSpeedReached(this.GetPtr(), ref combatAccelerationTime, ref maxSpeed, speedValues);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00012A99 File Offset: 0x00010C99
		public void SetMaximumSpeedLimit(float maximumSpeedLimit, bool isMultiplier)
		{
			MBAPI.IMBAgent.SetMaximumSpeedLimit(this.GetPtr(), maximumSpeedLimit, isMultiplier);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00012AB0 File Offset: 0x00010CB0
		public float GetBaseArmorEffectivenessForBodyPart(BoneBodyPartType bodyPart)
		{
			if (!this.IsHuman)
			{
				return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorTorso);
			}
			if (bodyPart == BoneBodyPartType.None)
			{
				return 0f;
			}
			if (bodyPart == BoneBodyPartType.Head || bodyPart == BoneBodyPartType.Neck)
			{
				return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorHead);
			}
			if (bodyPart == BoneBodyPartType.Legs)
			{
				return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorLegs);
			}
			if (bodyPart == BoneBodyPartType.ArmLeft || bodyPart == BoneBodyPartType.ArmRight)
			{
				return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorArms);
			}
			if (bodyPart == BoneBodyPartType.ShoulderLeft || bodyPart == BoneBodyPartType.ShoulderRight || bodyPart == BoneBodyPartType.Chest || bodyPart == BoneBodyPartType.Abdomen)
			{
				return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorTorso);
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Agent.cs", "GetBaseArmorEffectivenessForBodyPart", 3238);
			return this.GetAgentDrivenPropertyValue(DrivenProperty.ArmorTorso);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00012B40 File Offset: 0x00010D40
		public AITargetVisibilityState GetLastTargetVisibilityState()
		{
			return (AITargetVisibilityState)MBAPI.IMBAgent.GetLastTargetVisibilityState(this.GetPtr());
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00012B52 File Offset: 0x00010D52
		public float GetMissileRange()
		{
			return MBAPI.IMBAgent.GetMissileRange(this.GetPtr());
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00012B64 File Offset: 0x00010D64
		public void SetAgentIdleAnimationStatus(bool idleEnabled)
		{
			MBAPI.IMBAgent.SetAgentIdleAnimationStatus(this.GetPtr(), idleEnabled);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00012B78 File Offset: 0x00010D78
		public ItemObject GetWeaponToReplaceOnQuickAction(SpawnedItemEntity spawnedItem, out EquipmentIndex possibleSlotIndex)
		{
			EquipmentIndex equipmentIndex = MissionEquipment.SelectWeaponPickUpSlot(this, spawnedItem.WeaponCopy, spawnedItem.IsStuckMissile());
			possibleSlotIndex = equipmentIndex;
			if (equipmentIndex != EquipmentIndex.None && !this.Equipment[equipmentIndex].IsEmpty && ((!spawnedItem.IsStuckMissile() && !spawnedItem.WeaponCopy.IsAnyConsumable()) || this.Equipment[equipmentIndex].Item.PrimaryWeapon.WeaponClass != spawnedItem.WeaponCopy.Item.PrimaryWeapon.WeaponClass || !this.Equipment[equipmentIndex].IsAnyConsumable() || this.Equipment[equipmentIndex].Amount == this.Equipment[equipmentIndex].ModifiedMaxAmount))
			{
				return this.Equipment[equipmentIndex].Item;
			}
			return null;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00012C64 File Offset: 0x00010E64
		public Agent.Hitter GetAssistingHitter(MissionPeer killerPeer)
		{
			Agent.Hitter hitter = null;
			foreach (Agent.Hitter hitter2 in this.HitterList)
			{
				if (hitter2.HitterPeer != killerPeer && (hitter == null || hitter2.Damage > hitter.Damage))
				{
					hitter = hitter2;
				}
			}
			if (hitter != null && hitter.Damage >= 35f)
			{
				return hitter;
			}
			return null;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00012CE4 File Offset: 0x00010EE4
		public bool CanReachAgent(Agent otherAgent)
		{
			float interactionDistanceToUsable = this.GetInteractionDistanceToUsable(otherAgent);
			return this.Position.DistanceSquared(otherAgent.Position) < interactionDistanceToUsable * interactionDistanceToUsable;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00012D14 File Offset: 0x00010F14
		public bool CanInteractWithAgent(Agent otherAgent, float userAgentCameraElevation)
		{
			bool flag = false;
			foreach (MissionBehavior missionBehavior in Mission.Current.MissionBehaviors)
			{
				flag = flag || missionBehavior.IsThereAgentAction(this, otherAgent);
			}
			if (!flag)
			{
				return false;
			}
			bool flag2 = this.CanReachAgent(otherAgent);
			if (!otherAgent.IsMount)
			{
				return this.IsAbleToUseMachine() && flag2;
			}
			if ((this.MountAgent == null && this.GetCurrentAction(0) != ActionIndexCache.act_none) || (this.MountAgent != null && !this.IsAbleToUseMachine()))
			{
				return false;
			}
			if (otherAgent.RiderAgent == null)
			{
				return this.MountAgent == null && flag2 && otherAgent.GetCurrentActionType(0) != Agent.ActionCodeType.Rear;
			}
			if (otherAgent != this.MountAgent)
			{
				return false;
			}
			float num = this.GetLookDownLimit() + 0.4f;
			return flag2 && userAgentCameraElevation < num && this.GetCurrentVelocity().LengthSquared < 0.25f && otherAgent.GetCurrentActionType(0) != Agent.ActionCodeType.Rear;
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00012E30 File Offset: 0x00011030
		public bool CanBeAssignedForScriptedMovement()
		{
			return this.IsActive() && this.IsAIControlled && !this.IsDetachedFromFormation && !this.IsRunningAway && (this.GetScriptedFlags() & Agent.AIScriptedFrameFlags.GoToPosition) == Agent.AIScriptedFrameFlags.None && !this.InteractingWithAnyGameObject() && !this._isLadderQueueUsing;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00012E6F File Offset: 0x0001106F
		public bool CanReachAndUseObject(UsableMissionObject gameObject, float distanceSq)
		{
			return this.CanReachObject(gameObject, distanceSq) && this.CanUseObject(gameObject);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00012E84 File Offset: 0x00011084
		public bool CanReachObject(UsableMissionObject gameObject, float distanceSq)
		{
			return this.CanReachObjectFromPosition(gameObject, distanceSq, this.Position);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00012E94 File Offset: 0x00011094
		public bool CanReachObjectFromPosition(UsableMissionObject gameObject, float distanceSq, Vec3 position)
		{
			if (this.IsItemUseDisabled || this.IsUsingGameObject)
			{
				return false;
			}
			float interactionDistanceToUsable = this.GetInteractionDistanceToUsable(gameObject);
			return distanceSq <= interactionDistanceToUsable * interactionDistanceToUsable && MathF.Abs(gameObject.InteractionEntity.GlobalPosition.z - position.z) <= interactionDistanceToUsable * 2f;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00012EEE File Offset: 0x000110EE
		public bool CanUseObject(UsableMissionObject gameObject)
		{
			return !gameObject.IsDisabledForAgent(this) && gameObject.IsUsableByAgent(this);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00012F02 File Offset: 0x00011102
		public bool CanMoveDirectlyToPosition(in Vec2 position)
		{
			return MBAPI.IMBAgent.CanMoveDirectlyToPosition(this.GetPtr(), in position);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00012F18 File Offset: 0x00011118
		public bool CanInteractableWeaponBePickedUp(SpawnedItemEntity spawnedItem)
		{
			EquipmentIndex equipmentIndex;
			return (!spawnedItem.IsBanner() || MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(spawnedItem, this)) && (this.GetWeaponToReplaceOnQuickAction(spawnedItem, out equipmentIndex) != null || equipmentIndex == EquipmentIndex.None);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00012F53 File Offset: 0x00011153
		public bool CanQuickPickUp(SpawnedItemEntity spawnedItem)
		{
			return (!spawnedItem.IsBanner() || MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(spawnedItem, this)) && MissionEquipment.SelectWeaponPickUpSlot(this, spawnedItem.WeaponCopy, spawnedItem.IsStuckMissile()) != EquipmentIndex.None;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00012F8C File Offset: 0x0001118C
		public unsafe bool CanTeleport()
		{
			return this.Mission.IsTeleportingAgents && (this.Formation == null || this.Mission.Mode != MissionMode.Deployment || this.Formation.GetReadonlyMovementOrderReference()->OrderEnum == MovementOrder.MovementOrderEnum.Move);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00012FD8 File Offset: 0x000111D8
		public bool IsActive()
		{
			return this.State == AgentState.Active;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00012FE3 File Offset: 0x000111E3
		public bool IsRetreating()
		{
			return MBAPI.IMBAgent.IsRetreating(this.GetPtr());
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00012FF5 File Offset: 0x000111F5
		public bool IsFadingOut()
		{
			return MBAPI.IMBAgent.IsFadingOut(this.GetPtr());
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00013007 File Offset: 0x00011207
		public void SetAgentDrivenPropertyValueFromConsole(DrivenProperty type, float val)
		{
			this.AgentDrivenProperties.SetStat(type, val);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00013016 File Offset: 0x00011216
		public bool IsOnLand()
		{
			return (this.MovementMode & AgentMovementMode.WaterDiving) == AgentMovementMode.Land;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00013024 File Offset: 0x00011224
		public bool IsInWater()
		{
			AgentMovementMode agentMovementMode = this.MovementMode & AgentMovementMode.WaterDiving;
			return agentMovementMode == AgentMovementMode.WaterSurface || agentMovementMode == AgentMovementMode.WaterDiving;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00013044 File Offset: 0x00011244
		public bool IsAbleToUseMachine()
		{
			return (this.MovementMode & AgentMovementMode.WaterDiving) > AgentMovementMode.None;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00013051 File Offset: 0x00011251
		public bool IsAgentParentEntitySameAs(GameEntity toBeChecked)
		{
			return toBeChecked == MBAPI.IMBAgent.GetAgentParentEntity(this.GetPtr());
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00013069 File Offset: 0x00011269
		public void SetExcludedFromGravity(bool exclude, bool applyAverageGlobalVelocity)
		{
			MBAPI.IMBAgent.SetExcludedFromGravity(this.GetPtr(), exclude, applyAverageGlobalVelocity);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0001307D File Offset: 0x0001127D
		public void SetForceAttachedEntity(WeakGameEntity willBeAttached)
		{
			MBAPI.IMBAgent.SetForceAttachedEntity(this.GetPtr(), willBeAttached.Pointer);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00013096 File Offset: 0x00011296
		public bool IsSliding()
		{
			return MBAPI.IMBAgent.IsSliding(this.GetPtr());
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000130A8 File Offset: 0x000112A8
		public bool IsSitting()
		{
			Agent.ActionCodeType currentActionType = this.GetCurrentActionType(0);
			return currentActionType == Agent.ActionCodeType.Sit || currentActionType == Agent.ActionCodeType.SitOnTheFloor || currentActionType == Agent.ActionCodeType.SitOnAThrone;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000130D0 File Offset: 0x000112D0
		public bool IsReleasingChainAttackInMultiplayer()
		{
			bool flag = false;
			if (Mission.Current.CurrentTime - this._lastMultiplayerQuickReadyDetectedTime < 0.75f && this.GetCurrentActionStage(1) == Agent.ActionStage.AttackRelease)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00013104 File Offset: 0x00011304
		public bool IsCameraAttachable()
		{
			return !this._isDeleted && (!this._isRemoved || this._removalTime + 2.1f > this.Mission.CurrentTime) && this.IsHuman && this.AgentVisuals != null && this.AgentVisuals.IsValid() && (GameNetwork.IsSessionActive || this.Controller > AgentControllerType.None);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00013171 File Offset: 0x00011371
		public bool IsSynchedPrefabComponentVisible(int componentIndex)
		{
			return this._synchedBodyComponents[componentIndex].GetVisible();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00013184 File Offset: 0x00011384
		public bool IsEnemyOf(Agent otherAgent)
		{
			return MBAPI.IMBAgent.IsEnemy(this.GetPtr(), otherAgent.GetPtr());
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0001319C File Offset: 0x0001139C
		public bool IsFriendOf(Agent otherAgent)
		{
			return MBAPI.IMBAgent.IsFriend(this.GetPtr(), otherAgent.GetPtr());
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000131B4 File Offset: 0x000113B4
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x000131B6 File Offset: 0x000113B6
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000131B8 File Offset: 0x000113B8
		public void OnItemRemovedFromScene()
		{
			this.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000131C2 File Offset: 0x000113C2
		public void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			this.Mission.OnAgentInteraction(userAgent, this, agentBoneIndex);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000131D2 File Offset: 0x000113D2
		public void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x000131D4 File Offset: 0x000113D4
		public void OnWeaponDrop(EquipmentIndex equipmentSlot)
		{
			MissionWeapon missionWeapon = this.Equipment[equipmentSlot];
			this.Equipment[equipmentSlot] = MissionWeapon.Invalid;
			this.WeaponEquipped(equipmentSlot, in WeaponData.InvalidWeaponData, null, in WeaponData.InvalidWeaponData, null, WeakGameEntity.Invalid, false, false);
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnWeaponDrop(missionWeapon);
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00013260 File Offset: 0x00011460
		public void OnItemPickup(SpawnedItemEntity spawnedItemEntity, EquipmentIndex weaponPickUpSlotIndex, out bool removeWeapon)
		{
			removeWeapon = true;
			bool flag = true;
			MissionWeapon weaponCopy = spawnedItemEntity.WeaponCopy;
			if (weaponPickUpSlotIndex == EquipmentIndex.None)
			{
				weaponPickUpSlotIndex = MissionEquipment.SelectWeaponPickUpSlot(this, weaponCopy, spawnedItemEntity.IsStuckMissile());
			}
			bool flag2 = false;
			if (weaponPickUpSlotIndex == EquipmentIndex.ExtraWeaponSlot)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					flag2 = true;
					if (!this.Equipment[weaponPickUpSlotIndex].IsEmpty)
					{
						this.DropItem(weaponPickUpSlotIndex, this.Equipment[weaponPickUpSlotIndex].Item.PrimaryWeapon.WeaponClass);
					}
				}
			}
			else if (weaponPickUpSlotIndex != EquipmentIndex.None)
			{
				int num = 0;
				if ((spawnedItemEntity.IsStuckMissile() || spawnedItemEntity.WeaponCopy.IsAnyConsumable()) && !this.Equipment[weaponPickUpSlotIndex].IsEmpty && this.Equipment[weaponPickUpSlotIndex].IsSameType(weaponCopy) && this.Equipment[weaponPickUpSlotIndex].IsAnyConsumable())
				{
					num = (int)(this.Equipment[weaponPickUpSlotIndex].ModifiedMaxAmount - this.Equipment[weaponPickUpSlotIndex].Amount);
				}
				if (num > 0)
				{
					short num2 = (short)MathF.Min(num, (int)weaponCopy.Amount);
					if (num2 != weaponCopy.Amount)
					{
						removeWeapon = false;
						if (!GameNetwork.IsClientOrReplay)
						{
							spawnedItemEntity.ConsumeWeaponAmount(num2);
							if (GameNetwork.IsServer)
							{
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new ConsumeWeaponAmount(spawnedItemEntity.Id, num2));
								GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
							}
						}
					}
					if (!GameNetwork.IsClientOrReplay)
					{
						this.SetWeaponAmountInSlot(weaponPickUpSlotIndex, this.Equipment[weaponPickUpSlotIndex].Amount + num2, true);
						if (this.GetPrimaryWieldedItemIndex() == EquipmentIndex.None && (weaponCopy.Item.PrimaryWeapon.IsRangedWeapon || weaponCopy.Item.PrimaryWeapon.IsMeleeWeapon))
						{
							flag2 = true;
						}
					}
				}
				else if (!GameNetwork.IsClientOrReplay)
				{
					flag2 = true;
					if (!this.Equipment[weaponPickUpSlotIndex].IsEmpty)
					{
						this.DropItem(weaponPickUpSlotIndex, weaponCopy.Item.PrimaryWeapon.WeaponClass);
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				flag = MissionEquipment.DoesWeaponFitToSlot(weaponPickUpSlotIndex, weaponCopy);
				if (flag)
				{
					this.EquipWeaponFromSpawnedItemEntity(weaponPickUpSlotIndex, spawnedItemEntity, removeWeapon);
					if (flag2)
					{
						EquipmentIndex equipmentIndex = weaponPickUpSlotIndex;
						if (weaponCopy.Item.PrimaryWeapon.AmmoClass == weaponCopy.Item.PrimaryWeapon.WeaponClass)
						{
							for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < weaponPickUpSlotIndex; equipmentIndex2++)
							{
								if (!this.Equipment[equipmentIndex2].IsEmpty && weaponCopy.IsEqualTo(this.Equipment[equipmentIndex2]))
								{
									equipmentIndex = equipmentIndex2;
									break;
								}
							}
						}
						this.TryToWieldWeaponInSlot(equipmentIndex, Agent.WeaponWieldActionType.InstantAfterPickUp, false);
					}
					for (int i = 0; i < this._components.Count; i++)
					{
						this._components[i].OnItemPickup(spawnedItemEntity);
					}
					if (this.Controller == AgentControllerType.AI)
					{
						this.HumanAIComponent.ItemPickupDone(spawnedItemEntity);
					}
				}
			}
			if (flag)
			{
				this.Mission.TriggerOnItemPickUpEvent(this, spawnedItemEntity);
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0001354C File Offset: 0x0001174C
		public float GetDistanceTo(Agent other)
		{
			if (other == null)
			{
				Debug.FailedAssert("Comparing distance with null agent", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Agent.cs", "GetDistanceTo", 3749);
				return 0f;
			}
			return this.Position.Distance(other.Position);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0001358F File Offset: 0x0001178F
		public bool CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirection(int navigationFaceId, in Vec3 direction, float overridenCostForFaceId)
		{
			return MBAPI.IMBAgent.CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirection(this.GetPtr(), navigationFaceId, in direction, overridenCostForFaceId);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000135A4 File Offset: 0x000117A4
		public bool IsTargetNavigationFaceIdBetween(int navigationFaceIdStart, int navigationFaceIdEnd)
		{
			return MBAPI.IMBAgent.IsTargetNavigationFaceIdBetween(this.GetPtr(), navigationFaceIdStart, navigationFaceIdEnd);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000135B8 File Offset: 0x000117B8
		Vec3 ITrackableBase.GetPosition()
		{
			return this.Position;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000135C0 File Offset: 0x000117C0
		TextObject ITrackableBase.GetName()
		{
			if (this.Character != null)
			{
				return new TextObject(this.Character.Name.ToString(), null);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000135E8 File Offset: 0x000117E8
		public void CheckEquipmentForCapeClothSimulationStateChange()
		{
			if (this._capeClothSimulator != null)
			{
				bool flag = false;
				EquipmentIndex offhandWieldedItemIndex = this.GetOffhandWieldedItemIndex();
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
				{
					MissionWeapon missionWeapon = this.Equipment[equipmentIndex];
					if (!missionWeapon.IsEmpty && missionWeapon.IsShield() && equipmentIndex != offhandWieldedItemIndex)
					{
						flag = true;
						break;
					}
				}
				this._capeClothSimulator.SetMaxDistanceMultiplier(flag ? 0f : 1f);
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0001365C File Offset: 0x0001185C
		public void CheckToDropFlaggedItem()
		{
			if (this.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon))
			{
				for (int i = 0; i < 2; i++)
				{
					EquipmentIndex equipmentIndex = ((i == 0) ? this.GetPrimaryWieldedItemIndex() : this.GetOffhandWieldedItemIndex());
					if (equipmentIndex != EquipmentIndex.None && this.Equipment[equipmentIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction))
					{
						this.DropItem(equipmentIndex, WeaponClass.Undefined);
					}
				}
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000136CC File Offset: 0x000118CC
		public bool CheckSkillForMounting(Agent mountAgent)
		{
			int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(this, DefaultSkills.Riding);
			return (this.GetAgentFlags() & AgentFlag.CanRide) > AgentFlag.None && (float)effectiveSkill >= mountAgent.GetAgentDrivenPropertyValue(DrivenProperty.MountDifficulty);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0001370F File Offset: 0x0001190F
		public void InitializeSpawnEquipment(Equipment spawnEquipment)
		{
			this.SpawnEquipment = spawnEquipment;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00013718 File Offset: 0x00011918
		public void InitializeMissionEquipment(MissionEquipment missionEquipment, Banner banner)
		{
			this.Equipment = missionEquipment ?? new MissionEquipment(this.SpawnEquipment, banner);
			MissionEquipment equipment = this.Equipment;
			equipment.OnWeaponSlotUpdated = (Action)Delegate.Combine(equipment.OnWeaponSlotUpdated, new Action(this.OnWeaponSlotUpdated));
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00013758 File Offset: 0x00011958
		public void InitializeAgentProperties(Equipment spawnEquipment, AgentBuildData agentBuildData)
		{
			this._propertyModifiers = default(Agent.AgentPropertiesModifiers);
			this.AgentDrivenProperties = new AgentDrivenProperties();
			float[] array = this.AgentDrivenProperties.InitializeDrivenProperties(this, spawnEquipment, agentBuildData);
			this.UpdateDrivenProperties(array);
			if (this.IsMount && this.RiderAgent == null)
			{
				Mission.Current.AddMountWithoutRider(this);
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000137AD File Offset: 0x000119AD
		public void UpdateFormationOrders()
		{
			if (this.Formation != null && !this.IsRetreating())
			{
				this.EnforceShieldUsage(ArrangementOrder.GetShieldDirectionOfUnit(this.Formation, this, this.Formation.ArrangementOrder.OrderEnum));
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000137E1 File Offset: 0x000119E1
		public void UpdateWeapons()
		{
			MBAPI.IMBAgent.UpdateWeapons(this.GetPtr());
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000137F4 File Offset: 0x000119F4
		public void UpdateAgentProperties()
		{
			if (this.AgentDrivenProperties != null)
			{
				float[] array = this.AgentDrivenProperties.UpdateDrivenProperties(this);
				this.UpdateDrivenProperties(array);
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0001381D File Offset: 0x00011A1D
		public void UpdateCustomDrivenProperties()
		{
			if (this.AgentDrivenProperties != null)
			{
				this.UpdateDrivenProperties(this.AgentDrivenProperties.Values);
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00013838 File Offset: 0x00011A38
		public void UpdateBodyProperties(BodyProperties bodyProperties)
		{
			this.BodyPropertiesValue = bodyProperties;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00013841 File Offset: 0x00011A41
		public void UpdateSyncHealthToAllClients(bool value)
		{
			this.SyncHealthToAllClients = value;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0001384C File Offset: 0x00011A4C
		public void UpdateSpawnEquipmentAndRefreshVisuals(Equipment newSpawnEquipment)
		{
			this.SpawnEquipment = newSpawnEquipment;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SynchronizeAgentSpawnEquipment(this.Index, this.SpawnEquipment));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.AgentVisuals.ClearVisualComponents(false, false);
			this.Mission.OnEquipItemsFromSpawnEquipment(this, Agent.CreationType.FromCharacterObj);
			this.AgentVisuals.ClearAllWeaponMeshes();
			MissionEquipment equipment = this.Equipment;
			Equipment spawnEquipment = this.SpawnEquipment;
			IAgentOriginBase origin = this.Origin;
			equipment.FillFrom(spawnEquipment, (origin != null) ? origin.Banner : null);
			this.CheckEquipmentForCapeClothSimulationStateChange();
			this.EquipItemsFromSpawnEquipment(true, true, false, 0);
			this.UpdateAgentProperties();
			if (!Mission.Current.DoesMissionRequireCivilianEquipment && !GameNetwork.IsClientOrReplay)
			{
				this.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, TaleWorlds.Core.Equipment.InitialWeaponEquipPreference.Any);
			}
			this.PreloadForRendering();
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0001390C File Offset: 0x00011B0C
		public void ForceUpdateCachedAndFormationValues(bool updateOnlyMovement, bool arrangementChangeAllowed)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.IsAIControlled)
			{
				this.ParallelUpdateCachedAndFormationValuesForAIAgent(updateOnlyMovement);
			}
			else
			{
				if (!updateOnlyMovement)
				{
					Agent mountAgent = this.MountAgent;
					this.WalkSpeedCached = ((mountAgent != null) ? mountAgent.WalkingSpeedLimitOfMountable : this.Monster.WalkingSpeedLimit);
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					WorldPosition worldPosition;
					Vec2 vec;
					this.GetBaseFormationFrame(out worldPosition, out vec);
					this.TrySetFormationFrame(in worldPosition, in vec);
				}
			}
			this.ApplyFormationValuesPostUpdate(updateOnlyMovement, arrangementChangeAllowed);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0001397D File Offset: 0x00011B7D
		private void ParallelUpdateCachedAndFormationValuesForAIAgent(bool updateOnlyMovement)
		{
			if (!updateOnlyMovement)
			{
				Agent mountAgent = this.MountAgent;
				this.WalkSpeedCached = ((mountAgent != null) ? mountAgent.WalkingSpeedLimitOfMountable : this.Monster.WalkingSpeedLimit);
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.HumanAIComponent.ParallelUpdateFormationMovement();
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000139B8 File Offset: 0x00011BB8
		private void ApplyFormationValuesPostUpdate(bool updateOnlyMovement, bool arrangementChangeAllowed)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (!updateOnlyMovement)
				{
					Formation formation = this.Formation;
					if (formation != null)
					{
						formation.Team.DetachmentManager.TickAgent(this);
					}
				}
				if (!updateOnlyMovement && this.IsAIControlled)
				{
					this.UpdateFormationOrders();
					if (this.Formation != null)
					{
						int num;
						int num2;
						int num3;
						int num4;
						this.GetFormationFileAndRankInfo(out num, out num2, out num3, out num4);
						Vec2 wallDirectionOfRelativeFormationLocation = this.GetWallDirectionOfRelativeFormationLocation();
						MBAPI.IMBAgent.SetFormationInfo(this.GetPtr(), num, num2, num3, num4, this.Formation.CountOfUnits, wallDirectionOfRelativeFormationLocation, this.Formation.UnitSpacing);
					}
				}
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00013A50 File Offset: 0x00011C50
		public void UpdateDirectionChangeTendency()
		{
			float num = 1f;
			if (this.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall && !this.IsDetachedFromFormation)
			{
				num = this.Formation.Arrangement.GetDirectionChangeTendencyOfUnit(this);
			}
			this.SetDirectionChangeTendency(num);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00013A97 File Offset: 0x00011C97
		public void UpdateLastRangedAttackTimeDueToAnAttack(float newTime)
		{
			this.LastRangedAttackTime = newTime;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00013AA0 File Offset: 0x00011CA0
		public void InvalidateTargetAgent()
		{
			MBAPI.IMBAgent.InvalidateTargetAgent(this.GetPtr());
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00013AB2 File Offset: 0x00011CB2
		public void InvalidateAIWeaponSelections()
		{
			MBAPI.IMBAgent.InvalidateAIWeaponSelections(this.GetPtr());
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00013AC4 File Offset: 0x00011CC4
		public void ResetLookAgent()
		{
			this.SetLookAgent(null);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00013ACD File Offset: 0x00011CCD
		public void ResetGuard()
		{
			MBAPI.IMBAgent.ResetGuard(this.GetPtr());
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00013ADF File Offset: 0x00011CDF
		public void ResetAgentProperties()
		{
			this.AgentDrivenProperties = null;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00013AE8 File Offset: 0x00011CE8
		public void ResetAiWaitBeforeShootFactor()
		{
			this._propertyModifiers.resetAiWaitBeforeShootFactor = true;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00013AF6 File Offset: 0x00011CF6
		public void ClearTargetFrame()
		{
			this._checkIfTargetFrameIsChanged = false;
			if (this.MovementLockedState != AgentMovementLockedState.None)
			{
				this.ClearTargetFrameAux();
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new ClearAgentTargetFrame(this.Index));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00013B31 File Offset: 0x00011D31
		public void ClearEquipment()
		{
			MBAPI.IMBAgent.ClearEquipment(this.GetPtr());
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00013B43 File Offset: 0x00011D43
		public void ClearHandInverseKinematics()
		{
			MBAPI.IMBAgent.ClearHandInverseKinematics(this.GetPtr());
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00013B55 File Offset: 0x00011D55
		public void ClearAttachedWeapons()
		{
			List<ValueTuple<MissionWeapon, MatrixFrame, sbyte>> attachedWeapons = this._attachedWeapons;
			if (attachedWeapons == null)
			{
				return;
			}
			attachedWeapons.Clear();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00013B68 File Offset: 0x00011D68
		public void SetDetachableFromFormation(bool value)
		{
			bool isDetachableFromFormation = this._isDetachableFromFormation;
			if (isDetachableFromFormation != value)
			{
				if (isDetachableFromFormation)
				{
					if (this.IsDetachedFromFormation)
					{
						this.TryAttachToFormation();
					}
					this.TryRemoveAllDetachmentScores();
				}
				this._isDetachableFromFormation = value;
				if (!this.IsPlayerControlled)
				{
					if (isDetachableFromFormation)
					{
						Formation formation = this._formation;
						if (formation == null)
						{
							return;
						}
						formation.OnUndetachableNonPlayerUnitAdded(this);
						return;
					}
					else
					{
						Formation formation2 = this._formation;
						if (formation2 == null)
						{
							return;
						}
						formation2.OnUndetachableNonPlayerUnitRemoved(this);
					}
				}
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00013BCE File Offset: 0x00011DCE
		public bool TryAttachToFormation()
		{
			if (this.IsDetachedFromFormation)
			{
				this._detachment.RemoveAgent(this);
				Formation formation = this._formation;
				if (formation != null)
				{
					formation.AttachUnit(this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00013BF9 File Offset: 0x00011DF9
		public bool TryRemoveAllDetachmentScores()
		{
			if (this.IsAIControlled)
			{
				Formation formation = this._formation;
				if (formation != null)
				{
					Team team = formation.Team;
					if (team != null)
					{
						team.DetachmentManager.RemoveScoresOfAgentFromDetachments(this);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00013C28 File Offset: 0x00011E28
		public unsafe bool TrySetFormationFrame(in WorldPosition formationPosition, in Vec2 formationDirection)
		{
			WorldPosition worldPosition = formationPosition;
			bool flag = worldPosition.IsValid;
			WorldPosition worldPosition2 = formationPosition;
			if (!flag)
			{
				this.SetFormationFrameDisabled();
				return false;
			}
			if (!GameNetwork.IsMultiplayer && this.Mission.Mode == MissionMode.Deployment && !this.Mission.IsNavalBattle)
			{
				IMissionDeploymentPlan deploymentPlan = this.Mission.DeploymentPlan;
				if (deploymentPlan.SupportsNavmesh(this.Formation.Team))
				{
					deploymentPlan.ProjectPositionToDeploymentBoundaries(this.Formation.Team, ref worldPosition2);
				}
				flag = this.Mission.IsFormationUnitPositionAvailable(ref worldPosition2, this.Team);
			}
			if (flag)
			{
				WorldPosition worldPosition3 = worldPosition2;
				Vec2 vec = formationDirection;
				MovementOrder movementOrder = *this.Formation.GetReadonlyMovementOrderReference();
				this.SetFormationFrameEnabled(worldPosition3, vec, movementOrder.GetTargetVelocity(), this.Formation.CalculateFormationDirectionEnforcingFactorForRank(((IFormationUnit)this).FormationRankIndex));
				return true;
			}
			this.SetFormationFrameDisabled();
			return false;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00013D01 File Offset: 0x00011F01
		public void EnforceShieldUsage(Agent.UsageDirection shieldDirection)
		{
			MBAPI.IMBAgent.EnforceShieldUsage(this.GetPtr(), shieldDirection);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00013D14 File Offset: 0x00011F14
		public bool ObjectHasVacantPosition(UsableMissionObject gameObject)
		{
			return !gameObject.HasUser || gameObject.HasAIUser;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00013D26 File Offset: 0x00011F26
		public bool InteractingWithAnyGameObject()
		{
			return this.IsUsingGameObject || (this.IsAIControlled && this.AIInterestedInAnyGameObject());
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00013D44 File Offset: 0x00011F44
		private void StopUsingGameObjectAux(bool isSuccessful, Agent.StopUsingGameObjectFlags flags)
		{
			UsableMachine usableMachine = ((this.Controller != AgentControllerType.AI || !this.IsDetachableFromFormation || this.Formation == null) ? null : (this.Detachment as UsableMachine));
			if (usableMachine == null)
			{
				flags &= ~Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject;
			}
			UsableMissionObject currentlyUsedGameObject = this.CurrentlyUsedGameObject;
			UsableMissionObject usableMissionObject = null;
			if (!this.IsUsingGameObject && this.IsAIControlled)
			{
				if (this.AIMoveToGameObjectIsEnabled())
				{
					usableMissionObject = this.HumanAIComponent.GetCurrentlyMovingGameObject();
				}
				else
				{
					usableMissionObject = this.HumanAIComponent.GetCurrentlyDefendingGameObject();
				}
			}
			if (this.IsUsingGameObject)
			{
				bool flag = this.CurrentlyUsedGameObject.LockUserFrames || this.CurrentlyUsedGameObject.LockUserPositions;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new StopUsingObject(this.Index, isSuccessful));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.CurrentlyUsedGameObject.OnUseStopped(this, isSuccessful, this._usedObjectPreferenceIndex);
				this.CurrentlyUsedGameObject = null;
				if (this.IsAIControlled)
				{
					this.AIUseGameObjectDisable();
				}
				this._usedObjectPreferenceIndex = -1;
				if (flag)
				{
					this.ClearTargetFrame();
				}
			}
			else if (this.IsAIControlled)
			{
				if (this.AIDefendGameObjectIsEnabled())
				{
					this.AIDefendGameObjectDisable();
				}
				else
				{
					this.AIMoveToGameObjectDisable();
				}
			}
			if (this.State == AgentState.Active)
			{
				if (this.IsAIControlled)
				{
					this.DisableScriptedMovement();
					if (usableMachine != null)
					{
						foreach (StandingPoint standingPoint in usableMachine.StandingPoints)
						{
							standingPoint.FavoredUser = this;
						}
					}
				}
				this.AfterStoppedUsingMissionObject(usableMachine, currentlyUsedGameObject, usableMissionObject, isSuccessful, flags);
			}
			this.Mission.OnObjectStoppedBeingUsed(this, currentlyUsedGameObject);
			this._components.ForEach(delegate(AgentComponent ac)
			{
				ac.OnStopUsingGameObject();
			});
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00013F00 File Offset: 0x00012100
		public void StopUsingGameObjectMT(bool isSuccessful = true, Agent.StopUsingGameObjectFlags flags = Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject)
		{
			object stopUsingGameObjectLock = Agent._stopUsingGameObjectLock;
			lock (stopUsingGameObjectLock)
			{
				this.StopUsingGameObjectAux(isSuccessful, flags);
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00013F44 File Offset: 0x00012144
		public void StopUsingGameObject(bool isSuccessful = true, Agent.StopUsingGameObjectFlags flags = Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject)
		{
			this.StopUsingGameObjectAux(isSuccessful, flags);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00013F4E File Offset: 0x0001214E
		public void HandleStopUsingAction()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestStopUsingObject());
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			this.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00013F74 File Offset: 0x00012174
		public void HandleStartUsingAction(UsableMissionObject targetObject, int preferenceIndex)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestUseObject(targetObject.Id, preferenceIndex));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			this.UseGameObject(targetObject, preferenceIndex);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00013FA4 File Offset: 0x000121A4
		public AgentController AddController(Type type)
		{
			AgentController agentController = null;
			if (type.IsSubclassOf(typeof(AgentController)))
			{
				agentController = Activator.CreateInstance(type) as AgentController;
			}
			if (agentController != null)
			{
				agentController.Owner = this;
				agentController.Mission = this.Mission;
				this._agentControllers.Add(agentController);
				agentController.OnInitialize();
			}
			return agentController;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00013FFC File Offset: 0x000121FC
		public AgentController RemoveController(Type type)
		{
			for (int i = 0; i < this._agentControllers.Count; i++)
			{
				if (type.IsInstanceOfType(this._agentControllers[i]))
				{
					AgentController agentController = this._agentControllers[i];
					this._agentControllers.RemoveAt(i);
					return agentController;
				}
			}
			return null;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00014050 File Offset: 0x00012250
		public bool CanThrustAttackStickToBone(BoneBodyPartType bodyPart)
		{
			if (this.IsHuman)
			{
				BoneBodyPartType[] array = new BoneBodyPartType[]
				{
					BoneBodyPartType.Abdomen,
					BoneBodyPartType.Legs,
					BoneBodyPartType.Chest,
					BoneBodyPartType.Neck,
					BoneBodyPartType.ShoulderLeft,
					BoneBodyPartType.ShoulderRight,
					BoneBodyPartType.ArmLeft,
					BoneBodyPartType.ArmRight
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (bodyPart == array[i])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0001408E File Offset: 0x0001228E
		public void GetOldWieldedItemInfo(out int rightHandSlotIndex, out int rightHandUsageIndex, out int leftHandSlotIndex, out int leftHandUsageIndex)
		{
			MBAPI.IMBAgent.GetOldWieldedItemInfo(this.GetPtr(), out rightHandSlotIndex, out rightHandUsageIndex, out leftHandSlotIndex, out leftHandUsageIndex);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000140A5 File Offset: 0x000122A5
		public void StartSwitchingWeaponUsageIndexAsClient(EquipmentIndex equipmentIndex, int usageIndex, Agent.UsageDirection currentMovementFlagUsageDirection)
		{
			MBAPI.IMBAgent.StartSwitchingWeaponUsageIndexAsClient(this.GetPtr(), (int)equipmentIndex, usageIndex, currentMovementFlagUsageDirection);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000140BA File Offset: 0x000122BA
		public void TryToWieldWeaponInSlot(EquipmentIndex slotIndex, Agent.WeaponWieldActionType type, bool isWieldedOnSpawn)
		{
			MBAPI.IMBAgent.TryToWieldWeaponInSlot(this.GetPtr(), (int)slotIndex, (int)type, isWieldedOnSpawn);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000140CF File Offset: 0x000122CF
		public void PrepareWeaponForDropInEquipmentSlot(EquipmentIndex slotIndex, bool dropWithHolster)
		{
			MBAPI.IMBAgent.PrepareWeaponForDropInEquipmentSlot(this.GetPtr(), (int)slotIndex, dropWithHolster);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000140E4 File Offset: 0x000122E4
		public void AddHitter(MissionPeer peer, float damage, bool isFriendlyHit)
		{
			Agent.Hitter hitter = this._hitterList.Find((Agent.Hitter h) => h.HitterPeer == peer && h.IsFriendlyHit == isFriendlyHit);
			if (hitter == null)
			{
				hitter = new Agent.Hitter(peer, damage, isFriendlyHit);
				this._hitterList.Add(hitter);
				return;
			}
			hitter.IncreaseDamage(damage);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00014147 File Offset: 0x00012347
		public void TryToSheathWeaponInHand(Agent.HandIndex handIndex, Agent.WeaponWieldActionType type)
		{
			MBAPI.IMBAgent.TryToSheathWeaponInHand(this.GetPtr(), (int)handIndex, (int)type);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0001415C File Offset: 0x0001235C
		public void RemoveHitter(MissionPeer peer, bool isFriendlyHit)
		{
			Agent.Hitter hitter = this._hitterList.Find((Agent.Hitter h) => h.HitterPeer == peer && h.IsFriendlyHit == isFriendlyHit);
			if (hitter != null)
			{
				this._hitterList.Remove(hitter);
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000141A5 File Offset: 0x000123A5
		public void Retreat(WorldPosition retreatPos)
		{
			MBAPI.IMBAgent.SetRetreatMode(this.GetPtr(), retreatPos, true);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000141B9 File Offset: 0x000123B9
		public void StopRetreating()
		{
			MBAPI.IMBAgent.SetRetreatMode(this.GetPtr(), WorldPosition.Invalid, false);
			this.IsRunningAway = false;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000141D8 File Offset: 0x000123D8
		public void UseGameObject(UsableMissionObject usedObject, int preferenceIndex = -1)
		{
			if (usedObject.LockUserFrames)
			{
				WorldFrame userFrameForAgent = usedObject.GetUserFrameForAgent(this);
				Vec2 asVec = userFrameForAgent.Origin.AsVec2;
				this.SetTargetPositionAndDirection(in asVec, in userFrameForAgent.Rotation.f);
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.NoAttack);
			}
			else if (usedObject.LockUserPositions)
			{
				this.SetTargetPosition(usedObject.GetUserFrameForAgent(this).Origin.AsVec2);
				this.SetScriptedFlags(this.GetScriptedFlags() | Agent.AIScriptedFrameFlags.NoAttack);
			}
			if (this.IsActive() && this.IsAIControlled && this.AIMoveToGameObjectIsEnabled())
			{
				this.AIMoveToGameObjectDisable();
				Formation formation = this.Formation;
				if (formation != null)
				{
					formation.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(this);
				}
			}
			this.CurrentlyUsedGameObject = usedObject;
			this._usedObjectPreferenceIndex = preferenceIndex;
			if (this.IsAIControlled)
			{
				this.AIUseGameObjectEnable();
			}
			if (!this.IsInWater() || this.GetPrimaryWieldedItemIndex() != EquipmentIndex.None || this.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
			{
				this.SaveEquipmentsOnHand();
			}
			usedObject.OnUse(this, -1);
			this.Mission.OnObjectUsed(this, usedObject);
			if (usedObject.IsInstantUse && !GameNetwork.IsClientOrReplay && this.IsActive() && this.InteractingWithAnyGameObject())
			{
				this.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00014308 File Offset: 0x00012508
		public void SaveEquipmentsOnHand()
		{
			this._equipmentOnMainHandBeforeUsingObject = this.GetPrimaryWieldedItemIndex();
			this._equipmentOnOffHandBeforeUsingObject = this.GetOffhandWieldedItemIndex();
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00014322 File Offset: 0x00012522
		public void StartFadingOut()
		{
			MBAPI.IMBAgent.StartFadingOut(this.GetPtr());
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00014334 File Offset: 0x00012534
		public bool IsWandering()
		{
			return MBAPI.IMBAgent.IsWandering(this.GetPtr());
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00014346 File Offset: 0x00012546
		public void SetRenderCheckEnabled(bool value)
		{
			MBAPI.IMBAgent.SetRenderCheckEnabled(this.GetPtr(), value);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00014359 File Offset: 0x00012559
		public bool GetRenderCheckEnabled()
		{
			return MBAPI.IMBAgent.GetRenderCheckEnabled(this.GetPtr());
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0001436B File Offset: 0x0001256B
		public Vec3 ComputeAnimationDisplacement(float dt)
		{
			return MBAPI.IMBAgent.ComputeAnimationDisplacement(this.GetPtr(), dt);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0001437E File Offset: 0x0001257E
		public void TickActionChannels(float dt)
		{
			MBAPI.IMBAgent.TickActionChannels(this.GetPtr(), dt);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00014391 File Offset: 0x00012591
		public void SetIsPhysicsForceClosed(bool isPhysicsForceClosed)
		{
			MBAPI.IMBAgent.SetIsPhysicsForceClosed(this.GetPtr(), isPhysicsForceClosed);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x000143A4 File Offset: 0x000125A4
		public void LockAgentReplicationTableDataWithCurrentReliableSequenceNo(NetworkCommunicator peer)
		{
			MBDebug.Print(string.Concat(new object[] { "peer: ", peer.UserName, " index: ", this.Index, " name: ", this.Name }), 0, Debug.DebugColor.White, 17592186044416UL);
			MBAPI.IMBAgent.LockAgentReplicationTableDataWithCurrentReliableSequenceNo(this.GetPtr(), peer.Index);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0001441C File Offset: 0x0001261C
		public void TeleportToPosition(Vec3 position)
		{
			if (this.MountAgent != null)
			{
				MBAPI.IMBAgent.SetPosition(this.MountAgent.GetPtr(), ref position);
			}
			MBAPI.IMBAgent.SetPosition(this.GetPtr(), ref position);
			if (this.RiderAgent != null)
			{
				MBAPI.IMBAgent.SetPosition(this.RiderAgent.GetPtr(), ref position);
			}
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnAgentTeleported();
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000144BC File Offset: 0x000126BC
		public void FadeOut(bool hideInstantly, bool hideMount)
		{
			MBAPI.IMBAgent.FadeOut(this.GetPtr(), hideInstantly);
			if (hideMount && this.HasMount)
			{
				this.MountAgent.FadeOut(hideMount, false);
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000144E7 File Offset: 0x000126E7
		public void FadeIn()
		{
			MBAPI.IMBAgent.FadeIn(this.GetPtr());
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000144F9 File Offset: 0x000126F9
		public void DisableScriptedMovement()
		{
			MBAPI.IMBAgent.DisableScriptedMovement(this.GetPtr());
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0001450B File Offset: 0x0001270B
		public void DisableScriptedCombatMovement()
		{
			MBAPI.IMBAgent.DisableScriptedCombatMovement(this.GetPtr());
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0001451D File Offset: 0x0001271D
		public void ForceAiBehaviorSelection()
		{
			MBAPI.IMBAgent.ForceAiBehaviorSelection(this.GetPtr());
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00014530 File Offset: 0x00012730
		public bool HasPathThroughNavigationFaceIdFromDirectionMT(int navigationFaceId, Vec2 direction)
		{
			object pathCheckObjectLock = Agent._pathCheckObjectLock;
			bool flag2;
			lock (pathCheckObjectLock)
			{
				flag2 = MBAPI.IMBAgent.HasPathThroughNavigationFaceIdFromDirection(this.GetPtr(), navigationFaceId, ref direction);
			}
			return flag2;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00014580 File Offset: 0x00012780
		public bool HasPathThroughNavigationFaceIdFromDirection(int navigationFaceId, Vec2 direction)
		{
			return MBAPI.IMBAgent.HasPathThroughNavigationFaceIdFromDirection(this.GetPtr(), navigationFaceId, ref direction);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00014595 File Offset: 0x00012795
		public void DisableLookToPointOfInterest()
		{
			MBAPI.IMBAgent.DisableLookToPointOfInterest(this.GetPtr());
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000145A7 File Offset: 0x000127A7
		public CompositeComponent AddPrefabComponentToBone(string prefabName, sbyte boneIndex)
		{
			return MBAPI.IMBAgent.AddPrefabToAgentBone(this.GetPtr(), prefabName, boneIndex);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000145BB File Offset: 0x000127BB
		public void MakeVoice(SkinVoiceManager.SkinVoiceType voiceType, SkinVoiceManager.CombatVoiceNetworkPredictionType predictionType)
		{
			MBAPI.IMBAgent.MakeVoice(this.GetPtr(), voiceType.Index, (int)predictionType);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000145D5 File Offset: 0x000127D5
		public void YellAfterDelay(float delayTimeInSecond)
		{
			MBAPI.IMBAgent.YellAfterDelay(this.GetPtr(), delayTimeInSecond);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000145E8 File Offset: 0x000127E8
		public void WieldNextWeapon(Agent.HandIndex weaponIndex, Agent.WeaponWieldActionType wieldActionType = Agent.WeaponWieldActionType.WithAnimation)
		{
			MBAPI.IMBAgent.WieldNextWeapon(this.GetPtr(), (int)weaponIndex, (int)wieldActionType);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000145FC File Offset: 0x000127FC
		public Agent.MovementControlFlag AttackDirectionToMovementFlag(Agent.UsageDirection direction)
		{
			return MBAPI.IMBAgent.AttackDirectionToMovementFlag(this.GetPtr(), direction);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0001460F File Offset: 0x0001280F
		public Agent.MovementControlFlag DefendDirectionToMovementFlag(Agent.UsageDirection direction)
		{
			return MBAPI.IMBAgent.DefendDirectionToMovementFlag(this.GetPtr(), direction);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00014622 File Offset: 0x00012822
		public bool KickClear()
		{
			return MBAPI.IMBAgent.KickClear(this.GetPtr());
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00014634 File Offset: 0x00012834
		public Agent.UsageDirection PlayerAttackDirection()
		{
			return MBAPI.IMBAgent.PlayerAttackDirection(this.GetPtr());
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00014648 File Offset: 0x00012848
		public ValueTuple<sbyte, sbyte> GetRandomPairOfRealBloodBurstBoneIndices()
		{
			sbyte b = -1;
			sbyte b2 = -1;
			if (this.Monster.BloodBurstBoneIndices.Length != 0)
			{
				int num = MBRandom.RandomInt(this.Monster.BloodBurstBoneIndices.Length / 2);
				b = this.Monster.BloodBurstBoneIndices[num * 2];
				b2 = this.Monster.BloodBurstBoneIndices[num * 2 + 1];
			}
			return new ValueTuple<sbyte, sbyte>(b, b2);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000146A5 File Offset: 0x000128A5
		public void CreateBloodBurstAtLimb(sbyte realBoneIndex, float scale)
		{
			MBAPI.IMBAgent.CreateBloodBurstAtLimb(this.GetPtr(), realBoneIndex, scale);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x000146BC File Offset: 0x000128BC
		public void AddComponent(AgentComponent agentComponent)
		{
			this._components.Add(agentComponent);
			CommonAIComponent commonAIComponent;
			if ((commonAIComponent = agentComponent as CommonAIComponent) != null)
			{
				this.CommonAIComponent = commonAIComponent;
				return;
			}
			HumanAIComponent humanAIComponent;
			if ((humanAIComponent = agentComponent as HumanAIComponent) != null)
			{
				this.HumanAIComponent = humanAIComponent;
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000146F8 File Offset: 0x000128F8
		public bool RemoveComponent(AgentComponent agentComponent)
		{
			bool flag = this._components.Remove(agentComponent);
			if (flag)
			{
				agentComponent.OnComponentRemoved();
				if (this.CommonAIComponent == agentComponent)
				{
					this.CommonAIComponent = null;
					return flag;
				}
				if (this.HumanAIComponent == agentComponent)
				{
					this.HumanAIComponent = null;
				}
			}
			return flag;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00014730 File Offset: 0x00012930
		public void HandleTaunt(int tauntIndex, bool isDefaultTaunt)
		{
			if (tauntIndex < 0)
			{
				return;
			}
			if (isDefaultTaunt)
			{
				ActionIndexCache actionIndexCache = Agent.DefaultTauntActions[tauntIndex];
				this.SetActionChannel(1, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				this.MakeVoice(SkinVoiceManager.VoiceType.Victory, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				ActionIndexCache suitableTauntAction = CosmeticsManagerHelper.GetSuitableTauntAction(this, tauntIndex);
				if (suitableTauntAction.Index >= 0)
				{
					this.SetActionChannel(1, in suitableTauntAction, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000147D4 File Offset: 0x000129D4
		public void HandleBark(int indexOfBark)
		{
			if (indexOfBark < SkinVoiceManager.VoiceType.MpBarks.Length && !GameNetwork.IsClientOrReplay)
			{
				this.MakeVoice(SkinVoiceManager.VoiceType.MpBarks[indexOfBark], SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				if (GameNetwork.IsMultiplayer)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BarkAgent(this.Index, indexOfBark));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, this.MissionPeer.GetNetworkPeer());
				}
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00014834 File Offset: 0x00012A34
		public void HandleDropWeapon(bool isDefendPressed, EquipmentIndex forcedSlotIndexToDropWeaponFrom)
		{
			Agent.ActionCodeType currentActionType = this.GetCurrentActionType(1);
			if (this.State == AgentState.Active && currentActionType != Agent.ActionCodeType.ReleaseMelee && currentActionType != Agent.ActionCodeType.ReleaseRanged && currentActionType != Agent.ActionCodeType.ReleaseThrowing && currentActionType != Agent.ActionCodeType.WeaponBash)
			{
				EquipmentIndex equipmentIndex = forcedSlotIndexToDropWeaponFrom;
				if (equipmentIndex == EquipmentIndex.None)
				{
					EquipmentIndex primaryWieldedItemIndex = this.GetPrimaryWieldedItemIndex();
					EquipmentIndex offhandWieldedItemIndex = this.GetOffhandWieldedItemIndex();
					if (offhandWieldedItemIndex >= EquipmentIndex.WeaponItemBeginSlot && isDefendPressed)
					{
						equipmentIndex = offhandWieldedItemIndex;
					}
					else if (primaryWieldedItemIndex >= EquipmentIndex.WeaponItemBeginSlot)
					{
						equipmentIndex = primaryWieldedItemIndex;
					}
					else if (offhandWieldedItemIndex >= EquipmentIndex.WeaponItemBeginSlot)
					{
						equipmentIndex = offhandWieldedItemIndex;
					}
					else
					{
						for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
						{
							if (!this.Equipment[equipmentIndex2].IsEmpty && this.Equipment[equipmentIndex2].Item.PrimaryWeapon.IsConsumable)
							{
								if (this.Equipment[equipmentIndex2].Item.PrimaryWeapon.IsRangedWeapon)
								{
									if (this.Equipment[equipmentIndex2].Amount == 0)
									{
										equipmentIndex = equipmentIndex2;
										break;
									}
								}
								else
								{
									bool flag = false;
									for (EquipmentIndex equipmentIndex3 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex3 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex3++)
									{
										if (!this.Equipment[equipmentIndex3].IsEmpty && this.Equipment[equipmentIndex3].HasAnyUsageWithAmmoClass(this.Equipment[equipmentIndex2].Item.PrimaryWeapon.WeaponClass) && this.Equipment[equipmentIndex2].Amount > 0)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										equipmentIndex = equipmentIndex2;
										break;
									}
								}
							}
						}
					}
				}
				if (equipmentIndex != EquipmentIndex.None && !this.Equipment[equipmentIndex].IsEmpty)
				{
					this.DropItem(equipmentIndex, WeaponClass.Undefined);
					this.UpdateAgentProperties();
				}
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00014A04 File Offset: 0x00012C04
		public void DropItem(EquipmentIndex itemIndex, WeaponClass pickedUpItemType = WeaponClass.Undefined)
		{
			if (this.Equipment[itemIndex].CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.AffectsArea | WeaponFlags.Burning))
			{
				MatrixFrame boneEntitialFrameWithIndex = this.AgentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex(this.Monster.MainHandItemBoneIndex);
				MatrixFrame globalFrame = this.AgentVisuals.GetGlobalFrame();
				MatrixFrame matrixFrame = globalFrame.TransformToParent(in boneEntitialFrameWithIndex);
				Vec3 vec = globalFrame.origin + globalFrame.rotation.f - matrixFrame.origin;
				vec.Normalize();
				Mat3 identity = Mat3.Identity;
				identity.f = vec;
				identity.Orthonormalize();
				Mission.Current.OnAgentShootMissile(this, itemIndex, matrixFrame.origin, vec, identity, false, false, -1);
				this.RemoveEquippedWeapon(itemIndex);
				return;
			}
			MBAPI.IMBAgent.DropItem(this.GetPtr(), (int)itemIndex, (int)pickedUpItemType);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00014AE4 File Offset: 0x00012CE4
		public void EquipItemsFromSpawnEquipment(bool neededBatchedItems, bool prepareImmediately, bool useFaceCache, int faceCacheID)
		{
			this.Mission.OnEquipItemsFromSpawnEquipmentBegin(this, this._creationType);
			switch (this._creationType)
			{
			case Agent.CreationType.FromRoster:
			case Agent.CreationType.FromCharacterObj:
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					WeaponData weaponData = WeaponData.InvalidWeaponData;
					WeaponStatsData[] array = null;
					WeaponData weaponData2 = WeaponData.InvalidWeaponData;
					WeaponStatsData[] array2 = null;
					if (!this.Equipment[equipmentIndex].IsEmpty)
					{
						weaponData = this.Equipment[equipmentIndex].GetWeaponData(neededBatchedItems);
						array = this.Equipment[equipmentIndex].GetWeaponStatsData();
						weaponData2 = this.Equipment[equipmentIndex].GetAmmoWeaponData(neededBatchedItems);
						array2 = this.Equipment[equipmentIndex].GetAmmoWeaponStatsData();
					}
					this.WeaponEquipped(equipmentIndex, in weaponData, array, in weaponData2, array2, WeakGameEntity.Invalid, true, true);
					weaponData.DeinitializeManagedPointers();
					weaponData2.DeinitializeManagedPointers();
					for (int i = 0; i < this.Equipment[equipmentIndex].GetAttachedWeaponsCount(); i++)
					{
						MatrixFrame attachedWeaponFrame = this.Equipment[equipmentIndex].GetAttachedWeaponFrame(i);
						MissionWeapon attachedWeapon = this.Equipment[equipmentIndex].GetAttachedWeapon(i);
						this.AttachWeaponToWeaponAux(equipmentIndex, ref attachedWeapon, null, ref attachedWeaponFrame);
					}
				}
				this.AddSkinMeshes(!neededBatchedItems || prepareImmediately, useFaceCache, faceCacheID);
				break;
			}
			}
			this.UpdateAgentProperties();
			this.Mission.OnEquipItemsFromSpawnEquipment(this, this._creationType);
			this.CheckEquipmentForCapeClothSimulationStateChange();
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00014C6C File Offset: 0x00012E6C
		public void WieldInitialWeapons(Agent.WeaponWieldActionType wieldActionType = Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference initialWeaponEquipPreference = TaleWorlds.Core.Equipment.InitialWeaponEquipPreference.Any)
		{
			EquipmentIndex primaryWieldedItemIndex = this.GetPrimaryWieldedItemIndex();
			EquipmentIndex offhandWieldedItemIndex = this.GetOffhandWieldedItemIndex();
			bool flag;
			this.SpawnEquipment.GetInitialWeaponIndicesToEquip(out primaryWieldedItemIndex, out offhandWieldedItemIndex, out flag, initialWeaponEquipPreference);
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				this.TryToWieldWeaponInSlot(offhandWieldedItemIndex, wieldActionType, true);
			}
			if (primaryWieldedItemIndex != EquipmentIndex.None)
			{
				this.TryToWieldWeaponInSlot(primaryWieldedItemIndex, wieldActionType, true);
				if (this.GetPrimaryWieldedItemIndex() == EquipmentIndex.None)
				{
					this.WieldNextWeapon(Agent.HandIndex.MainHand, wieldActionType);
				}
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00014CC4 File Offset: 0x00012EC4
		public void ChangeWeaponHitPoints(EquipmentIndex slotIndex, short hitPoints)
		{
			this.Equipment.SetHitPointsOfSlot(slotIndex, hitPoints, false);
			this.SetWeaponHitPointsInSlot(slotIndex, hitPoints);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetWeaponNetworkData(this.Index, slotIndex, hitPoints));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnWeaponHPChanged(this.Equipment[slotIndex].Item, (int)hitPoints);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00014D68 File Offset: 0x00012F68
		public bool HasWeapon()
		{
			for (int i = 0; i < 5; i++)
			{
				WeaponComponentData currentUsageItem = this.Equipment[i].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00014DAB File Offset: 0x00012FAB
		public void AttachWeaponToWeapon(EquipmentIndex slotIndex, MissionWeapon weapon, GameEntity weaponEntity, ref MatrixFrame attachLocalFrame)
		{
			this.Equipment.AttachWeaponToWeaponInSlot(slotIndex, ref weapon, ref attachLocalFrame);
			this.AttachWeaponToWeaponAux(slotIndex, ref weapon, weaponEntity, ref attachLocalFrame);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00014DC9 File Offset: 0x00012FC9
		public void AttachWeaponToBone(MissionWeapon weapon, GameEntity weaponEntity, sbyte boneIndex, ref MatrixFrame attachLocalFrame)
		{
			if (this._attachedWeapons == null)
			{
				this._attachedWeapons = new List<ValueTuple<MissionWeapon, MatrixFrame, sbyte>>();
			}
			this._attachedWeapons.Add(new ValueTuple<MissionWeapon, MatrixFrame, sbyte>(weapon, attachLocalFrame, boneIndex));
			this.AttachWeaponToBoneAux(ref weapon, weaponEntity, boneIndex, ref attachLocalFrame);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00014E04 File Offset: 0x00013004
		public void RestoreShieldHitPoints()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (!this.Equipment[equipmentIndex].IsEmpty && this.Equipment[equipmentIndex].CurrentUsageItem.IsShield)
				{
					this.ChangeWeaponHitPoints(equipmentIndex, this.Equipment[equipmentIndex].ModifiedMaxHitPoints);
				}
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00014E6C File Offset: 0x0001306C
		public void Die(Blow b, Agent.KillInfo overrideKillInfo = Agent.KillInfo.Invalid)
		{
			if (this.Formation != null)
			{
				this.Formation.Team.QuerySystem.RegisterDeath();
				if (b.IsMissile)
				{
					this.Formation.Team.QuerySystem.RegisterDeathByRanged();
				}
			}
			this.Health = 0f;
			if (overrideKillInfo != Agent.KillInfo.TeamSwitch && (b.OwnerId == -1 || b.OwnerId == this.Index) && this.IsHuman && this._lastHitInfo.CanOverrideBlow)
			{
				b.OwnerId = this._lastHitInfo.LastBlowOwnerId;
				b.AttackType = this._lastHitInfo.LastBlowAttackType;
			}
			MBAPI.IMBAgent.Die(this.GetPtr(), ref b, (sbyte)overrideKillInfo);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00014F27 File Offset: 0x00013127
		public void MakeDead(bool isKilled, ActionIndexCache actionIndex, int corpsesToFadeIndex = -1)
		{
			MBAPI.IMBAgent.MakeDead(this.GetPtr(), isKilled, actionIndex.Index, corpsesToFadeIndex);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00014F42 File Offset: 0x00013142
		public void RegisterBlow(Blow blow, in AttackCollisionData collisionData)
		{
			this.HandleBlow(ref blow, in collisionData);
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00014F50 File Offset: 0x00013150
		public void CreateBlowFromBlowAsReflection(in Blow blow, in AttackCollisionData collisionData, out Blow outBlow, out AttackCollisionData outCollisionData)
		{
			outBlow = blow;
			outBlow.InflictedDamage = blow.SelfInflictedDamage;
			outBlow.GlobalPosition = this.Position;
			outBlow.BoneIndex = 0;
			outBlow.BlowFlag = BlowFlags.None;
			outCollisionData = collisionData;
			outCollisionData.UpdateCollisionPositionAndBoneForReflect(collisionData.InflictedDamage, this.Position, 0);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00014FB0 File Offset: 0x000131B0
		public void TickParallel(float dt)
		{
			if (this.IsActive())
			{
				if (GameNetwork.IsMultiplayer && this.GetCurrentActionStage(1) == Agent.ActionStage.AttackQuickReady)
				{
					this._lastMultiplayerQuickReadyDetectedTime = Mission.Current.CurrentTime;
				}
				if (this._checkIfTargetFrameIsChanged)
				{
					Vec2 vec = ((this.MovementLockedState != AgentMovementLockedState.None) ? this.GetTargetPosition() : this.LookFrame.origin.AsVec2);
					Vec3 vec2 = ((this.MovementLockedState != AgentMovementLockedState.None) ? this.GetTargetDirection() : this.LookFrame.rotation.f);
					AgentMovementLockedState movementLockedState = this.MovementLockedState;
					if (movementLockedState != AgentMovementLockedState.PositionLocked)
					{
						if (movementLockedState == AgentMovementLockedState.FrameLocked)
						{
							this._checkIfTargetFrameIsChanged = this._lastSynchedTargetPosition != vec || this._lastSynchedTargetDirection != vec2;
						}
					}
					else
					{
						this._checkIfTargetFrameIsChanged = this._lastSynchedTargetPosition != vec;
					}
					if (this._checkIfTargetFrameIsChanged)
					{
						if (this.MovementLockedState == AgentMovementLockedState.FrameLocked)
						{
							Vec2 vec3 = MBMath.Lerp(vec, this._lastSynchedTargetPosition, 5f * dt, 0.005f);
							Vec3 vec4 = MBMath.Lerp(vec2, this._lastSynchedTargetDirection, 5f * dt, 0.005f);
							this.SetTargetPositionAndDirection(in vec3, in vec4);
						}
						else
						{
							this.SetTargetPosition(MBMath.Lerp(vec, this._lastSynchedTargetPosition, 5f * dt, 0.005f));
						}
					}
				}
				foreach (AgentComponent agentComponent in this._components)
				{
					agentComponent.OnTickParallel(dt);
				}
				if (this.Mission.AllowAiTicking && this.IsAIControlled && this._cachedAndFormationValuesUpdateTimer.Check(this.Mission.CurrentTime) && this.Formation != null)
				{
					this._cachedAndFormationValuesUpdateTimer.AdjustStartTime(-5f);
					this.ParallelUpdateCachedAndFormationValuesForAIAgent(false);
				}
				if (this._wantsToYell)
				{
					if (this._yellTimer > 0f)
					{
						this._yellTimer -= dt;
					}
					else
					{
						this.MakeVoice((this.MountAgent != null) ? SkinVoiceManager.VoiceType.HorseRally : SkinVoiceManager.VoiceType.Yell, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
						this._wantsToYell = false;
					}
				}
				if (this.IsPlayerControlled && this.IsCheering && this.MovementInputVector != Vec2.Zero)
				{
					this.SetActionChannel(1, in ActionIndexCache.act_none, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
			}
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0001521C File Offset: 0x0001341C
		public void Tick(float dt)
		{
			if (this._changedFormationPosition.IsValid)
			{
				if (this.IsFormationFrameEnabled)
				{
					Formation formation = this.Formation;
					if (formation != null)
					{
						Team team = formation.Team;
						if (team != null)
						{
							TeamAIComponent teamAI = team.TeamAI;
							if (teamAI != null)
							{
								teamAI.OnFormationFrameChanged(this, true, this._changedFormationPosition);
							}
						}
					}
					if (this.Mission.IsTeleportingAgents)
					{
						this.TeleportToPosition(this._changedFormationPosition.GetGroundVec3());
					}
				}
				else
				{
					Formation formation2 = this.Formation;
					if (formation2 != null)
					{
						Team team2 = formation2.Team;
						if (team2 != null)
						{
							TeamAIComponent teamAI2 = team2.TeamAI;
							if (teamAI2 != null)
							{
								teamAI2.OnFormationFrameChanged(this, false, WorldPosition.Invalid);
							}
						}
					}
				}
				this._changedFormationPosition = WorldPosition.Invalid;
			}
			if (this.IsActive())
			{
				foreach (AgentComponent agentComponent in this._components)
				{
					agentComponent.OnTick(dt);
				}
				if (this.Mission.AllowAiTicking && this.IsAIControlled)
				{
					this.TickAsAI();
					return;
				}
			}
			else
			{
				MissionPeer missionPeer = this.MissionPeer;
				if (((missionPeer != null) ? missionPeer.ControlledAgent : null) == this && !this.IsCameraAttachable())
				{
					this.MissionPeer.ControlledAgent = null;
				}
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0001535C File Offset: 0x0001355C
		[Conditional("DEBUG")]
		public void DebugMore()
		{
			MBAPI.IMBAgent.DebugMore(this.GetPtr());
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00015370 File Offset: 0x00013570
		public void Mount(Agent mountAgent)
		{
			bool flag = mountAgent.GetCurrentActionType(0) == Agent.ActionCodeType.Rear;
			if (this.MountAgent == null && mountAgent.RiderAgent == null)
			{
				if (this.CheckSkillForMounting(mountAgent) && !flag && this.GetCurrentAction(0) == ActionIndexCache.act_none)
				{
					this.EventControlFlags |= Agent.EventControlFlag.Mount;
					this.SetInteractionAgent(mountAgent);
					return;
				}
			}
			else if (this.MountAgent == mountAgent && !flag)
			{
				this.EventControlFlags |= Agent.EventControlFlag.Dismount;
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000153E8 File Offset: 0x000135E8
		public void EquipWeaponToExtraSlotAndWield(ref MissionWeapon weapon)
		{
			if (!this.Equipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty)
			{
				this.DropItem(EquipmentIndex.ExtraWeaponSlot, WeaponClass.Undefined);
			}
			this.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref weapon);
			this.TryToWieldWeaponInSlot(EquipmentIndex.ExtraWeaponSlot, Agent.WeaponWieldActionType.InstantAfterPickUp, false);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00015424 File Offset: 0x00013624
		public void RemoveEquippedWeapon(EquipmentIndex slotIndex)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RemoveEquippedWeapon(this.Index, slotIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.Equipment[slotIndex] = MissionWeapon.Invalid;
			this.WeaponEquipped(slotIndex, in WeaponData.InvalidWeaponData, null, in WeaponData.InvalidWeaponData, null, WeakGameEntity.Invalid, true, false);
			this.UpdateAgentProperties();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00015488 File Offset: 0x00013688
		public void EquipWeaponWithNewEntity(EquipmentIndex slotIndex, ref MissionWeapon weapon)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new EquipWeaponWithNewEntity(this.Index, slotIndex, weapon));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.Equipment[slotIndex] = weapon;
			WeaponData weaponData = WeaponData.InvalidWeaponData;
			WeaponStatsData[] array = null;
			WeaponData weaponData2 = WeaponData.InvalidWeaponData;
			WeaponStatsData[] array2 = null;
			if (!weapon.IsEmpty)
			{
				weaponData = weapon.GetWeaponData(true);
				array = weapon.GetWeaponStatsData();
				weaponData2 = weapon.GetAmmoWeaponData(true);
				array2 = weapon.GetAmmoWeaponStatsData();
			}
			this.WeaponEquipped(slotIndex, in weaponData, array, in weaponData2, array2, WeakGameEntity.Invalid, true, true);
			weaponData.DeinitializeManagedPointers();
			weaponData2.DeinitializeManagedPointers();
			for (int i = 0; i < weapon.GetAttachedWeaponsCount(); i++)
			{
				MissionWeapon attachedWeapon = weapon.GetAttachedWeapon(i);
				MatrixFrame attachedWeaponFrame = weapon.GetAttachedWeaponFrame(i);
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AttachWeaponToWeaponInAgentEquipmentSlot(attachedWeapon, this.Index, slotIndex, attachedWeaponFrame));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.AttachWeaponToWeaponAux(slotIndex, ref attachedWeapon, null, ref attachedWeaponFrame);
			}
			this.UpdateAgentProperties();
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00015590 File Offset: 0x00013790
		public void EquipWeaponFromSpawnedItemEntity(EquipmentIndex slotIndex, SpawnedItemEntity spawnedItemEntity, bool removeWeapon)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new EquipWeaponFromSpawnedItemEntity(this.Index, slotIndex, spawnedItemEntity.Id, removeWeapon));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			if (spawnedItemEntity.GameEntity.Parent.IsValid && spawnedItemEntity.GameEntity.Parent.HasScriptOfType<SpawnedItemEntity>())
			{
				SpawnedItemEntity firstScriptOfType = spawnedItemEntity.GameEntity.Parent.GetFirstScriptOfType<SpawnedItemEntity>();
				int num = -1;
				for (int i = 0; i < firstScriptOfType.GameEntity.ChildCount; i++)
				{
					if (firstScriptOfType.GameEntity.GetChild(i) == spawnedItemEntity.GameEntity)
					{
						num = i;
						break;
					}
				}
				firstScriptOfType.WeaponCopy.RemoveAttachedWeapon(num);
			}
			if (removeWeapon)
			{
				if (!this.Equipment[slotIndex].IsEmpty)
				{
					using (new TWSharedMutexWriteLock(Scene.PhysicsAndRayCastLock))
					{
						spawnedItemEntity.GameEntity.Remove(73);
						return;
					}
				}
				WeakGameEntity gameEntity = spawnedItemEntity.GameEntity;
				using (new TWSharedMutexWriteLock(Scene.PhysicsAndRayCastLock))
				{
					gameEntity.RemovePhysics(false);
				}
				gameEntity.RemoveScriptComponent(spawnedItemEntity.ScriptComponent.Pointer, 10);
				gameEntity.SetVisibilityExcludeParents(true);
				MissionWeapon weaponCopy = spawnedItemEntity.WeaponCopy;
				this.Equipment[slotIndex] = weaponCopy;
				WeaponData weaponData = weaponCopy.GetWeaponData(true);
				WeaponStatsData[] weaponStatsData = weaponCopy.GetWeaponStatsData();
				WeaponData ammoWeaponData = weaponCopy.GetAmmoWeaponData(true);
				WeaponStatsData[] ammoWeaponStatsData = weaponCopy.GetAmmoWeaponStatsData();
				this.WeaponEquipped(slotIndex, in weaponData, weaponStatsData, in ammoWeaponData, ammoWeaponStatsData, gameEntity, true, false);
				weaponData.DeinitializeManagedPointers();
				for (int j = 0; j < weaponCopy.GetAttachedWeaponsCount(); j++)
				{
					MatrixFrame attachedWeaponFrame = weaponCopy.GetAttachedWeaponFrame(j);
					MissionWeapon attachedWeapon = weaponCopy.GetAttachedWeapon(j);
					this.AttachWeaponToWeaponAux(slotIndex, ref attachedWeapon, null, ref attachedWeaponFrame);
				}
				this.UpdateAgentProperties();
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000157A0 File Offset: 0x000139A0
		public void PreloadForRendering()
		{
			this.PreloadForRenderingAux();
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000157A8 File Offset: 0x000139A8
		public int AddSynchedPrefabComponentToBone(string prefabName, sbyte boneIndex)
		{
			if (this._synchedBodyComponents == null)
			{
				this._synchedBodyComponents = new List<CompositeComponent>();
			}
			if (!GameEntity.PrefabExists(prefabName))
			{
				MBDebug.ShowWarning("Missing prefab for agent logic :" + prefabName);
				prefabName = "rock_001";
			}
			CompositeComponent compositeComponent = this.AddPrefabComponentToBone(prefabName, boneIndex);
			int count = this._synchedBodyComponents.Count;
			this._synchedBodyComponents.Add(compositeComponent);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new AddPrefabComponentToAgentBone(this.Index, prefabName, boneIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			return count;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00015830 File Offset: 0x00013A30
		public bool WillDropWieldedShield(SpawnedItemEntity spawnedItem)
		{
			EquipmentIndex offhandWieldedItemIndex = this.GetOffhandWieldedItemIndex();
			if (offhandWieldedItemIndex != EquipmentIndex.None && spawnedItem.WeaponCopy.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand) && spawnedItem.WeaponCopy.HasAllUsagesWithAnyWeaponFlag(WeaponFlags.NotUsableWithOneHand))
			{
				bool flag = false;
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
				{
					if (equipmentIndex != offhandWieldedItemIndex && !this.Equipment[equipmentIndex].IsEmpty && this.Equipment[equipmentIndex].IsShield())
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000158C0 File Offset: 0x00013AC0
		public bool HadSameTypeOfConsumableOrShieldOnSpawn(WeaponClass weaponClass)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (!this.SpawnEquipment[equipmentIndex].IsEmpty)
				{
					foreach (WeaponComponentData weaponComponentData in this.SpawnEquipment[equipmentIndex].Item.Weapons)
					{
						if ((weaponComponentData.IsConsumable || weaponComponentData.IsShield) && weaponComponentData.WeaponClass == weaponClass)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00015964 File Offset: 0x00013B64
		public override int GetHashCode()
		{
			return this._creationIndex;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0001596C File Offset: 0x00013B6C
		public bool TryGetImmediateEnemyAgentMovementData(out float maximumForwardUnlimitedSpeed, out Vec3 position)
		{
			return MBAPI.IMBAgent.TryGetImmediateEnemyAgentMovementData(this.GetPtr(), out maximumForwardUnlimitedSpeed, out position);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00015980 File Offset: 0x00013B80
		public bool HasLostShield()
		{
			bool flag = false;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (this.SpawnEquipment[equipmentIndex].Item != null && this.SpawnEquipment[equipmentIndex].Item.PrimaryWeapon.IsShield)
				{
					flag = true;
					break;
				}
			}
			bool flag2 = false;
			if (flag)
			{
				for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
				{
					if (!this.Equipment[equipmentIndex2].IsEmpty && this.Equipment[equipmentIndex2].Item != null && this.Equipment[equipmentIndex2].Item.PrimaryWeapon.IsShield)
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && !flag2;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00015A49 File Offset: 0x00013C49
		public bool CanPerformBrace()
		{
			if (!this._isBracingCacheValid)
			{
				this._canPerformBrace = !this.HasMount && !this.HasShieldCached && this.Equipment.HasAnyWeaponWithItemUsageSetFlags((ItemObject.ItemUsageSetFlags)18);
				this._isBracingCacheValid = true;
			}
			return this._canPerformBrace;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00015A86 File Offset: 0x00013C86
		public void SetLastDetachmentTickAgentTime(float lastDetachmentTickAgentTime)
		{
			this.LastDetachmentTickAgentTime = lastDetachmentTickAgentTime;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00015A8F File Offset: 0x00013C8F
		public void SetDetachmentWeight(float newDetachmentWeight)
		{
			this.DetachmentWeight = newDetachmentWeight;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00015A98 File Offset: 0x00013C98
		public void SetDetachmentIndex(int newDetachmentIndex)
		{
			this.DetachmentIndex = newDetachmentIndex;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00015AA1 File Offset: 0x00013CA1
		public void SetOwningAgentMissionPeer(MissionPeer owningAgentMissionPeer)
		{
			this.OwningAgentMissionPeer = owningAgentMissionPeer;
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetAgentOwningMissionPeer(this.Index, (owningAgentMissionPeer != null) ? owningAgentMissionPeer.Peer : null));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00015AD9 File Offset: 0x00013CD9
		public void SetMissionRepresentative(MissionRepresentativeBase missionRepresentative)
		{
			this.MissionRepresentative = missionRepresentative;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00015AE2 File Offset: 0x00013CE2
		public void SetIsLadderQueueUsing(bool isLadderQueueUsing)
		{
			this._isLadderQueueUsing = isLadderQueueUsing;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00015AEB File Offset: 0x00013CEB
		public void SetIsInLadderQueue(bool isInLadderQueue)
		{
			this.IsInLadderQueue = isInLadderQueue;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public void UpdateLocalPositionError()
		{
			float num = 0.5f - this.Character.SkillFactor * 0.5f;
			float num2 = 0.1f + (1f - this.Character.SkillFactor);
			Vec2 vec = Vec2.Forward;
			vec.RotateCCW(3.1415927f * MBRandom.RandomFloat);
			vec *= MBRandom.RandomFloatRanged(num2 - num) + num;
			this.LocalPositionError = vec;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00015B61 File Offset: 0x00013D61
		public void YellingBehaviour()
		{
			if (this.IsAIControlled && MBRandom.RandomFloat < this.GetMorale() * 0.0033f)
			{
				this.YellAfterDelay(1.5f + MBRandom.RandomFloat);
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00015B8F File Offset: 0x00013D8F
		internal void SetMountAgentBeforeBuild(Agent mount)
		{
			this.MountAgent = mount;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00015B98 File Offset: 0x00013D98
		internal void SetMountInitialValues(TextObject name, string horseCreationKey)
		{
			this._name = name;
			this.HorseCreationKey = horseCreationKey;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00015BA8 File Offset: 0x00013DA8
		internal void SetInitialAgentScale(float initialScale)
		{
			MBAPI.IMBAgent.SetAgentScale(this.GetPtr(), initialScale);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00015BBB File Offset: 0x00013DBB
		internal void InitializeAgentRecord()
		{
			MBAPI.IMBAgent.InitializeAgentRecord(this.GetPtr());
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00015BCD File Offset: 0x00013DCD
		internal void OnDelete()
		{
			this._isDeleted = true;
			this.MissionPeer = null;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00015BDD File Offset: 0x00013DDD
		internal void OnFleeing()
		{
			this.RelieveFromCaptaincy();
			if (this.Formation != null)
			{
				this.Formation.Team.DetachmentManager.OnAgentRemoved(this);
				this.Formation = null;
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00015C0C File Offset: 0x00013E0C
		internal void OnRemove()
		{
			this._isRemoved = true;
			this._removalTime = this.Mission.CurrentTime;
			IAgentOriginBase origin = this.Origin;
			if (origin != null)
			{
				origin.OnAgentRemoved(this.Health);
			}
			this.RelieveFromCaptaincy();
			Team team = this.Team;
			if (team != null)
			{
				team.OnAgentRemoved(this);
			}
			if (this.Formation != null)
			{
				this.Formation.Team.DetachmentManager.OnAgentRemoved(this);
				this.Formation = null;
			}
			if (((this.HumanAIComponent != null && this.InteractingWithAnyGameObject()) || this.IsUsingGameObject) && !GameNetwork.IsClientOrReplay && this.Mission != null && !this.Mission.MissionEnded)
			{
				this.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.None);
			}
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.OnAgentRemoved();
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00015D04 File Offset: 0x00013F04
		internal void InitializeComponents()
		{
			foreach (AgentComponent agentComponent in this._components)
			{
				agentComponent.Initialize();
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00015D54 File Offset: 0x00013F54
		internal void Build(AgentBuildData agentBuildData)
		{
			this.BuildAux();
			this.HasBeenBuilt = true;
			this.Controller = (this.GetAgentFlags().HasAnyFlag(AgentFlag.IsHumanoid) ? agentBuildData.AgentController : AgentControllerType.AI);
			this.Formation = ((!this.IsMount) ? ((agentBuildData != null) ? agentBuildData.AgentFormation : null) : null);
			MissionGameModels missionGameModels = MissionGameModels.Current;
			if (missionGameModels != null)
			{
				missionGameModels.AgentStatCalculateModel.InitializeMissionEquipment(this);
			}
			this.InitializeAgentProperties(this.SpawnEquipment, agentBuildData);
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (!networkCommunicator.IsMine && networkCommunicator.IsSynchronized)
					{
						this.LockAgentReplicationTableDataWithCurrentReliableSequenceNo(networkCommunicator);
					}
				}
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00015E2C File Offset: 0x0001402C
		private void PreloadForRenderingAux()
		{
			MBAPI.IMBAgent.PreloadForRendering(this.GetPtr());
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00015E40 File Offset: 0x00014040
		internal void Clear()
		{
			this.Mission = null;
			this._pointer = UIntPtr.Zero;
			this._positionPointer = UIntPtr.Zero;
			this._flagsPointer = UIntPtr.Zero;
			this._indexPointer = UIntPtr.Zero;
			this._statePointer = UIntPtr.Zero;
			this._movementModePointer = UIntPtr.Zero;
			this._controllerTypePointer = UIntPtr.Zero;
			this._movementDirectionPointer = UIntPtr.Zero;
			this._primaryWieldedItemIndexPointer = UIntPtr.Zero;
			this._offHandWieldedItemIndexPointer = UIntPtr.Zero;
			this._channel0CurrentActionPointer = UIntPtr.Zero;
			this._channel1CurrentActionPointer = UIntPtr.Zero;
			this._maximumForwardUnlimitedSpeed = UIntPtr.Zero;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00015EE3 File Offset: 0x000140E3
		public bool HasPathThroughNavigationFacesIDFromDirection(int navigationFaceID_1, int navigationFaceID_2, int navigationFaceID_3, Vec2 direction)
		{
			return MBAPI.IMBAgent.HasPathThroughNavigationFacesIDFromDirection(this.GetPtr(), navigationFaceID_1, navigationFaceID_2, navigationFaceID_3, ref direction);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00015EFC File Offset: 0x000140FC
		public bool HasPathThroughNavigationFacesIDFromDirectionMT(int navigationFaceID_1, int navigationFaceID_2, int navigationFaceID_3, Vec2 direction)
		{
			object pathCheckObjectLock = Agent._pathCheckObjectLock;
			bool flag2;
			lock (pathCheckObjectLock)
			{
				flag2 = MBAPI.IMBAgent.HasPathThroughNavigationFacesIDFromDirection(this.GetPtr(), navigationFaceID_1, navigationFaceID_2, navigationFaceID_3, ref direction);
			}
			return flag2;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00015F4C File Offset: 0x0001414C
		private void AfterStoppedUsingMissionObject(UsableMachine usableMachine, UsableMissionObject usedObject, UsableMissionObject movingToOrDefendingObject, bool isSuccessful, Agent.StopUsingGameObjectFlags flags)
		{
			if (this.IsAIControlled)
			{
				if (flags.HasAnyFlag(Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject))
				{
					Formation formation = this.Formation;
					if (formation != null)
					{
						formation.AttachUnit(this);
					}
				}
				if (flags.HasAnyFlag(Agent.StopUsingGameObjectFlags.DefendAfterStoppingUsingGameObject))
				{
					UsableMissionObject usableMissionObject = usedObject ?? movingToOrDefendingObject;
					this.AIDefendGameObjectEnable(usableMissionObject, usableMachine);
				}
			}
			StandingPoint standingPoint;
			if ((standingPoint = usedObject as StandingPoint) != null && standingPoint.AutoEquipWeaponsOnUseStopped && !flags.HasAnyFlag(Agent.StopUsingGameObjectFlags.DoNotWieldWeaponAfterStoppingUsingGameObject))
			{
				bool flag = !isSuccessful;
				bool flag2 = this._equipmentOnMainHandBeforeUsingObject != EquipmentIndex.None;
				if (this._equipmentOnOffHandBeforeUsingObject != EquipmentIndex.None)
				{
					Agent.WeaponWieldActionType weaponWieldActionType = ((flag && !flag2) ? Agent.WeaponWieldActionType.WithAnimation : Agent.WeaponWieldActionType.Instant);
					this.Mission.AddTickActionMT(Mission.MissionTickAction.TryToWieldWeaponInSlot, this, (int)this._equipmentOnOffHandBeforeUsingObject, (int)weaponWieldActionType);
				}
				if (flag2)
				{
					Agent.WeaponWieldActionType weaponWieldActionType2 = (flag ? Agent.WeaponWieldActionType.WithAnimation : Agent.WeaponWieldActionType.Instant);
					this.Mission.AddTickActionMT(Mission.MissionTickAction.TryToWieldWeaponInSlot, this, (int)this._equipmentOnMainHandBeforeUsingObject, (int)weaponWieldActionType2);
				}
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00016010 File Offset: 0x00014210
		private UIntPtr GetPtr()
		{
			return this.Pointer;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00016018 File Offset: 0x00014218
		private void SetWeaponHitPointsInSlot(EquipmentIndex equipmentIndex, short hitPoints)
		{
			MBAPI.IMBAgent.SetWeaponHitPointsInSlot(this.GetPtr(), (int)equipmentIndex, hitPoints);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0001602C File Offset: 0x0001422C
		private AgentMovementLockedState GetMovementLockedState()
		{
			return MBAPI.IMBAgent.GetMovementLockedState(this.GetPtr());
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00016040 File Offset: 0x00014240
		private void AttachWeaponToBoneAux(ref MissionWeapon weapon, GameEntity weaponEntity, sbyte boneIndex, ref MatrixFrame attachLocalFrame)
		{
			WeaponData weaponData = weapon.GetWeaponData(true);
			MBAPI.IMBAgent.AttachWeaponToBone(this.GetPtr(), in weaponData, weapon.GetWeaponStatsData(), weapon.WeaponsCount, (weaponEntity != null) ? weaponEntity.Pointer : UIntPtr.Zero, boneIndex, ref attachLocalFrame);
			weaponData.DeinitializeManagedPointers();
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0001608D File Offset: 0x0001428D
		private Agent GetRiderAgentAux()
		{
			return this._cachedRiderAgent;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00016098 File Offset: 0x00014298
		private void AttachWeaponToWeaponAux(EquipmentIndex slotIndex, ref MissionWeapon weapon, GameEntity weaponEntity, ref MatrixFrame attachLocalFrame)
		{
			WeaponData weaponData = weapon.GetWeaponData(true);
			MBAPI.IMBAgent.AttachWeaponToWeaponInSlot(this.GetPtr(), in weaponData, weapon.GetWeaponStatsData(), weapon.WeaponsCount, (weaponEntity != null) ? weaponEntity.Pointer : UIntPtr.Zero, (int)slotIndex, ref attachLocalFrame);
			weaponData.DeinitializeManagedPointers();
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000160E5 File Offset: 0x000142E5
		private Agent GetMountAgentAux()
		{
			return this._cachedMountAgent;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000160F0 File Offset: 0x000142F0
		private void SetMountAgent(Agent mountAgent)
		{
			int num = ((mountAgent == null) ? (-1) : mountAgent.Index);
			MBAPI.IMBAgent.SetMountAgent(this.GetPtr(), num);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0001611C File Offset: 0x0001431C
		private void RelieveFromCaptaincy()
		{
			if (this._canLeadFormationsRemotely && this.Team != null)
			{
				using (List<Formation>.Enumerator enumerator = this.Team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation = enumerator.Current;
						if (formation.Captain == this)
						{
							formation.Captain = null;
						}
					}
					return;
				}
			}
			if (this.Formation != null && this.Formation.Captain == this)
			{
				this.Formation.Captain = null;
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000161B0 File Offset: 0x000143B0
		private void SetTeamInternal(MBTeam team)
		{
			MBAPI.IMBAgent.SetTeam(this.GetPtr(), team.Index);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000161C8 File Offset: 0x000143C8
		private void WeaponEquipped(EquipmentIndex equipmentSlot, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, WeakGameEntity weaponEntity, bool removeOldWeaponFromScene, bool isWieldedOnSpawn)
		{
			MBAPI.IMBAgent.WeaponEquipped(this.GetPtr(), (int)equipmentSlot, in weaponData, weaponStatsData, (weaponStatsData != null) ? weaponStatsData.Length : 0, in ammoWeaponData, ammoWeaponStatsData, (ammoWeaponStatsData != null) ? ammoWeaponStatsData.Length : 0, weaponEntity.Pointer, removeOldWeaponFromScene, isWieldedOnSpawn);
			this.CheckEquipmentForCapeClothSimulationStateChange();
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00016211 File Offset: 0x00014411
		private Agent GetRiderAgent()
		{
			return MBAPI.IMBAgent.GetRiderAgent(this.GetPtr());
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00016223 File Offset: 0x00014423
		public void SetInitialFrame(in Vec3 initialPosition, in Vec2 initialDirection, bool canSpawnOutsideOfMissionBoundary = false)
		{
			MBAPI.IMBAgent.SetInitialFrame(this.GetPtr(), in initialPosition, in initialDirection, canSpawnOutsideOfMissionBoundary);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00016238 File Offset: 0x00014438
		private void UpdateDrivenProperties(float[] values)
		{
			MBAPI.IMBAgent.UpdateDrivenProperties(this.GetPtr(), values);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0001624C File Offset: 0x0001444C
		private void UpdateLastAttackAndHitTimes(Agent attackerAgent, bool isMissile)
		{
			float currentTime = this.Mission.CurrentTime;
			if (isMissile)
			{
				this.LastRangedHitTime = currentTime;
			}
			else
			{
				this.LastMeleeHitTime = currentTime;
			}
			if (attackerAgent != this && attackerAgent != null)
			{
				if (isMissile)
				{
					attackerAgent.LastRangedAttackTime = currentTime;
					return;
				}
				attackerAgent.LastMeleeAttackTime = currentTime;
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00016291 File Offset: 0x00014491
		private void SetNetworkPeer(NetworkCommunicator newPeer)
		{
			MBAPI.IMBAgent.SetNetworkPeer(this.GetPtr(), (newPeer != null) ? newPeer.Index : (-1));
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000162AF File Offset: 0x000144AF
		private void ClearTargetFrameAux()
		{
			MBAPI.IMBAgent.ClearTargetFrame(this.GetPtr());
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000162C1 File Offset: 0x000144C1
		[Conditional("_RGL_KEEP_ASSERTS")]
		private void CheckUnmanagedAgentValid()
		{
			AgentHelper.GetAgentIndex(this._indexPointer);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000162CF File Offset: 0x000144CF
		private void BuildAux()
		{
			MBAPI.IMBAgent.Build(this.GetPtr(), this.Monster.EyeOffsetWrtHead);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000162EC File Offset: 0x000144EC
		public void ClearTargetZ()
		{
			MBAPI.IMBAgent.ClearTargetZ(this.GetPtr());
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00016300 File Offset: 0x00014500
		private float GetMissileRangeWithHeightDifference()
		{
			if (!this.IsMount && (this.IsRangedCached || this.HasThrownCached))
			{
				Formation formation = this.Formation;
				if (((formation != null) ? formation.CachedClosestEnemyFormation : null) != null)
				{
					return this.GetMissileRangeWithHeightDifferenceAux(this.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.GetNavMeshZ());
				}
			}
			return 0f;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00016364 File Offset: 0x00014564
		private void AddSkinMeshes(bool prepareImmediately, bool useFaceCache, int faceCacheID)
		{
			SkinMask skinMeshesMask = this.SpawnEquipment.GetSkinMeshesMask();
			bool flag = this.IsFemale && this.BodyPropertiesValue.Age >= 14f;
			SkinGenerationParams skinGenerationParams = new SkinGenerationParams((int)skinMeshesMask, this.SpawnEquipment.GetUnderwearType(flag), (int)this.SpawnEquipment.BodyMeshType, (int)this.SpawnEquipment.HairCoverType, (int)this.SpawnEquipment.BeardCoverType, (int)this.SpawnEquipment.BodyDeformType, prepareImmediately, this.Character.FaceDirtAmount, this.IsFemale ? 1 : 0, this.Character.Race, false, false, faceCacheID);
			this.AgentVisuals.AddSkinMeshes(skinGenerationParams, this.BodyPropertiesValue, prepareImmediately, useFaceCache);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0001641C File Offset: 0x0001461C
		private void HandleBlow(ref Blow b, in AttackCollisionData collisionData)
		{
			b.BaseMagnitude = MathF.Min(b.BaseMagnitude, 1000f);
			b.DamagedPercentage = (float)b.InflictedDamage / this.HealthLimit;
			Agent agent = ((b.OwnerId != -1) ? this.Mission.FindAgentWithIndex(b.OwnerId) : null);
			if (!b.BlowFlag.HasAnyFlag(BlowFlags.NoSound))
			{
				bool flag = b.IsBlowCrit(this.Monster.HitPoints * 4);
				bool flag2 = b.IsBlowLow(this.Monster.HitPoints);
				bool flag3 = agent == null || agent.IsHuman;
				bool flag4 = b.BlowFlag.HasAnyFlag(BlowFlags.NonTipThrust);
				int hitSound = b.WeaponRecord.GetHitSound(flag3, flag, flag2, flag4, b.AttackType, b.DamageType);
				float soundParameterForArmorType = Agent.GetSoundParameterForArmorType(this.GetProtectorArmorMaterialOfBone(b.BoneIndex));
				SoundEventParameter soundEventParameter = new SoundEventParameter("Armor Type", soundParameterForArmorType);
				this.Mission.MakeSound(hitSound, b.GlobalPosition, false, true, b.OwnerId, this.Index, ref soundEventParameter);
				if (b.IsMissile && agent != null)
				{
					int soundCodeMissionCombatPlayerhit = CombatSoundContainer.SoundCodeMissionCombatPlayerhit;
					this.Mission.MakeSoundOnlyOnRelatedPeer(soundCodeMissionCombatPlayerhit, b.GlobalPosition, agent.Index);
				}
				if (!collisionData.IsSneakAttack)
				{
					this.Mission.AddSoundAlarmFactorToAgents(agent, in b.GlobalPosition, 7f);
				}
			}
			if (b.InflictedDamage <= 0)
			{
				return;
			}
			this.UpdateLastAttackAndHitTimes(agent, b.IsMissile);
			float health = this.Health;
			float num = (((float)b.InflictedDamage > health) ? health : ((float)b.InflictedDamage));
			if (this.CurrentMortalityState == Agent.MortalityState.Immortal || this.Mission.DisableDying || Mission.Current.Mode == MissionMode.Conversation || Mission.Current.Mode == MissionMode.CutScene)
			{
				num = 0f;
			}
			float num2 = health - num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			this.Health = num2;
			if (agent != null && agent != this && this.IsHuman)
			{
				if (agent.IsMount && agent.RiderAgent != null)
				{
					this._lastHitInfo.RegisterLastBlow(agent.RiderAgent.Index, b.AttackType);
				}
				else if (agent.IsHuman)
				{
					this._lastHitInfo.RegisterLastBlow(b.OwnerId, b.AttackType);
				}
			}
			if (!this.Mission.DisableDying && Mission.Current.Mode != MissionMode.Conversation && Mission.Current.Mode != MissionMode.CutScene)
			{
				this.Mission.OnAgentHit(this, agent, in b, in collisionData, false, num);
			}
			if (this.Health < 1f)
			{
				Agent.KillInfo killInfo = (b.IsFallDamage ? Agent.KillInfo.Gravity : Agent.KillInfo.Invalid);
				this.Die(b, killInfo);
			}
			this.HandleBlowAux(ref b);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000166C4 File Offset: 0x000148C4
		private void HandleBlowAux(ref Blow b)
		{
			MBAPI.IMBAgent.HandleBlowAux(this.GetPtr(), ref b);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000166D8 File Offset: 0x000148D8
		private ArmorComponent.ArmorMaterialTypes GetProtectorArmorMaterialOfBone(sbyte boneIndex)
		{
			if (boneIndex >= 0)
			{
				EquipmentIndex equipmentIndex = EquipmentIndex.None;
				switch (this.AgentVisuals.GetBoneTypeData(boneIndex).BodyPartType)
				{
				case BoneBodyPartType.Head:
				case BoneBodyPartType.Neck:
					equipmentIndex = EquipmentIndex.NumAllWeaponSlots;
					break;
				case BoneBodyPartType.Chest:
				case BoneBodyPartType.Abdomen:
				case BoneBodyPartType.ShoulderLeft:
				case BoneBodyPartType.ShoulderRight:
					equipmentIndex = EquipmentIndex.Body;
					break;
				case BoneBodyPartType.ArmLeft:
				case BoneBodyPartType.ArmRight:
					equipmentIndex = EquipmentIndex.Gloves;
					break;
				case BoneBodyPartType.Legs:
					equipmentIndex = EquipmentIndex.Leg;
					break;
				}
				if (equipmentIndex != EquipmentIndex.None && this.SpawnEquipment[equipmentIndex].Item != null)
				{
					return this.SpawnEquipment[equipmentIndex].Item.ArmorComponent.MaterialType;
				}
			}
			return ArmorComponent.ArmorMaterialTypes.None;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00016774 File Offset: 0x00014974
		private void TickAsAI()
		{
			if (this._cachedAndFormationValuesUpdateTimer.Check(this.Mission.CurrentTime) && this.Formation != null)
			{
				this.ApplyFormationValuesPostUpdate(false, true);
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x000167A0 File Offset: 0x000149A0
		private void SyncHealthToClients()
		{
			if (this.SyncHealthToAllClients && (!this.IsMount || this.RiderAgent != null))
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetAgentHealth(this.Index, (int)this.Health));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			NetworkCommunicator networkCommunicator;
			if (!this.IsMount)
			{
				MissionPeer missionPeer = this.MissionPeer;
				networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
			}
			else
			{
				Agent riderAgent = this.RiderAgent;
				if (riderAgent == null)
				{
					networkCommunicator = null;
				}
				else
				{
					MissionPeer missionPeer2 = riderAgent.MissionPeer;
					networkCommunicator = ((missionPeer2 != null) ? missionPeer2.GetNetworkPeer() : null);
				}
			}
			NetworkCommunicator networkCommunicator2 = networkCommunicator;
			if (networkCommunicator2 != null && !networkCommunicator2.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
				GameNetwork.WriteMessage(new SetAgentHealth(this.Index, (int)this.Health));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00016851 File Offset: 0x00014A51
		private void OnWeaponSlotUpdated()
		{
			this._isBracingCacheValid = false;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0001685C File Offset: 0x00014A5C
		public static Agent.UsageDirection MovementFlagToDirection(Agent.MovementControlFlag flag)
		{
			if (flag.HasAnyFlag(Agent.MovementControlFlag.AttackDown))
			{
				return Agent.UsageDirection.AttackDown;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.AttackUp))
			{
				return Agent.UsageDirection.AttackUp;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.AttackLeft))
			{
				return Agent.UsageDirection.AttackLeft;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.AttackRight))
			{
				return Agent.UsageDirection.AttackRight;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.DefendDown))
			{
				return Agent.UsageDirection.DefendDown;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.DefendUp))
			{
				return Agent.UsageDirection.AttackEnd;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.DefendLeft))
			{
				return Agent.UsageDirection.DefendLeft;
			}
			if (flag.HasAnyFlag(Agent.MovementControlFlag.DefendRight))
			{
				return Agent.UsageDirection.DefendRight;
			}
			return Agent.UsageDirection.None;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000168DF File Offset: 0x00014ADF
		public static Agent.UsageDirection GetActionDirection(int actionIndex)
		{
			return MBAPI.IMBAgent.GetActionDirection(actionIndex);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000168EC File Offset: 0x00014AEC
		public static int GetMonsterUsageIndex(string monsterUsage)
		{
			return MBAPI.IMBAgent.GetMonsterUsageIndex(monsterUsage);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000168F9 File Offset: 0x00014AF9
		public static float GetSoundParameterForArmorType(ArmorComponent.ArmorMaterialTypes armorMaterialType)
		{
			return (float)armorMaterialType * 0.1f;
		}

		// Token: 0x04000232 RID: 562
		public const float BecomeTeenagerAge = 14f;

		// Token: 0x04000233 RID: 563
		public const float MaxMountInteractionDistance = 1.75f;

		// Token: 0x04000234 RID: 564
		public const float DismountVelocityLimit = 0.5f;

		// Token: 0x04000235 RID: 565
		public const float HealthDyingThreshold = 1f;

		// Token: 0x04000236 RID: 566
		public const float CachedAndFormationValuesUpdateTime = 0.5f;

		// Token: 0x04000237 RID: 567
		public const float MaxInteractionDistance = 3f;

		// Token: 0x04000238 RID: 568
		public const float MaxFocusDistance = 10f;

		// Token: 0x04000239 RID: 569
		private const float ChainAttackDetectionTimeout = 0.75f;

		// Token: 0x0400023A RID: 570
		public static readonly ActionIndexCache[] DefaultTauntActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_taunt_cheer_1,
			ActionIndexCache.act_taunt_cheer_2,
			ActionIndexCache.act_taunt_cheer_3,
			ActionIndexCache.act_taunt_cheer_4
		};

		// Token: 0x0400023B RID: 571
		private static readonly object _stopUsingGameObjectLock = new object();

		// Token: 0x0400023C RID: 572
		private static readonly object _pathCheckObjectLock = new object();

		// Token: 0x0400023D RID: 573
		public Agent.OnMainAgentWieldedItemChangeDelegate OnMainAgentWieldedItemChange;

		// Token: 0x0400023E RID: 574
		public Action OnAgentMountedStateChanged;

		// Token: 0x0400023F RID: 575
		public Action OnAgentWieldedItemChange;

		// Token: 0x04000242 RID: 578
		private readonly MBList<AgentComponent> _components;

		// Token: 0x04000243 RID: 579
		private readonly Agent.CreationType _creationType;

		// Token: 0x04000244 RID: 580
		private readonly List<AgentController> _agentControllers;

		// Token: 0x04000245 RID: 581
		private readonly Timer _cachedAndFormationValuesUpdateTimer;

		// Token: 0x04000246 RID: 582
		private Agent _cachedMountAgent;

		// Token: 0x04000247 RID: 583
		private Agent _cachedRiderAgent;

		// Token: 0x04000248 RID: 584
		private BasicCharacterObject _character;

		// Token: 0x04000249 RID: 585
		private uint? _clothingColor1;

		// Token: 0x0400024A RID: 586
		private uint? _clothingColor2;

		// Token: 0x0400024B RID: 587
		private EquipmentIndex _equipmentOnMainHandBeforeUsingObject;

		// Token: 0x0400024C RID: 588
		private EquipmentIndex _equipmentOnOffHandBeforeUsingObject;

		// Token: 0x0400024D RID: 589
		private float _defensiveness;

		// Token: 0x0400024E RID: 590
		private UIntPtr _positionPointer;

		// Token: 0x0400024F RID: 591
		private UIntPtr _pointer;

		// Token: 0x04000250 RID: 592
		private UIntPtr _flagsPointer;

		// Token: 0x04000251 RID: 593
		private UIntPtr _indexPointer;

		// Token: 0x04000252 RID: 594
		private UIntPtr _statePointer;

		// Token: 0x04000253 RID: 595
		private UIntPtr _movementModePointer;

		// Token: 0x04000254 RID: 596
		private UIntPtr _controllerTypePointer;

		// Token: 0x04000255 RID: 597
		private UIntPtr _movementDirectionPointer;

		// Token: 0x04000256 RID: 598
		private UIntPtr _primaryWieldedItemIndexPointer;

		// Token: 0x04000257 RID: 599
		private float _lastMultiplayerQuickReadyDetectedTime;

		// Token: 0x04000258 RID: 600
		private UIntPtr _offHandWieldedItemIndexPointer;

		// Token: 0x04000259 RID: 601
		private UIntPtr _channel0CurrentActionPointer;

		// Token: 0x0400025A RID: 602
		private UIntPtr _channel1CurrentActionPointer;

		// Token: 0x0400025B RID: 603
		private UIntPtr _maximumForwardUnlimitedSpeed;

		// Token: 0x0400025C RID: 604
		private Agent _lookAgentCache;

		// Token: 0x0400025D RID: 605
		private IDetachment _detachment;

		// Token: 0x0400025E RID: 606
		private readonly MBList<Agent.Hitter> _hitterList;

		// Token: 0x0400025F RID: 607
		private List<ValueTuple<MissionWeapon, MatrixFrame, sbyte>> _attachedWeapons;

		// Token: 0x04000260 RID: 608
		private float _health;

		// Token: 0x04000261 RID: 609
		private MissionPeer _missionPeer;

		// Token: 0x04000262 RID: 610
		private TextObject _name;

		// Token: 0x04000263 RID: 611
		private float _removalTime;

		// Token: 0x04000264 RID: 612
		private List<CompositeComponent> _synchedBodyComponents;

		// Token: 0x04000265 RID: 613
		private Formation _formation;

		// Token: 0x04000266 RID: 614
		private bool _checkIfTargetFrameIsChanged;

		// Token: 0x04000267 RID: 615
		private Agent.AgentPropertiesModifiers _propertyModifiers;

		// Token: 0x04000268 RID: 616
		private int _usedObjectPreferenceIndex = -1;

		// Token: 0x04000269 RID: 617
		private bool _isDeleted;

		// Token: 0x0400026A RID: 618
		private bool _wantsToYell;

		// Token: 0x0400026B RID: 619
		private float _yellTimer;

		// Token: 0x0400026C RID: 620
		private Vec3 _lastSynchedTargetDirection;

		// Token: 0x0400026D RID: 621
		private Vec2 _lastSynchedTargetPosition;

		// Token: 0x0400026E RID: 622
		private Agent.AgentLastHitInfo _lastHitInfo;

		// Token: 0x0400026F RID: 623
		private ClothSimulatorComponent _capeClothSimulator;

		// Token: 0x04000270 RID: 624
		private bool _isRemoved;

		// Token: 0x04000271 RID: 625
		private WeakReference<MBAgentVisuals> _visualsWeakRef = new WeakReference<MBAgentVisuals>(null);

		// Token: 0x04000272 RID: 626
		private readonly int _creationIndex;

		// Token: 0x04000273 RID: 627
		private bool _canLeadFormationsRemotely;

		// Token: 0x04000274 RID: 628
		private bool _isDetachableFromFormation = true;

		// Token: 0x04000275 RID: 629
		private ItemObject _formationBanner;

		// Token: 0x04000276 RID: 630
		private bool _isLadderQueueUsing;

		// Token: 0x04000277 RID: 631
		private WorldPosition _changedFormationPosition;

		// Token: 0x04000278 RID: 632
		private Vec2 _localPositionError;

		// Token: 0x04000279 RID: 633
		private bool _canPerformBrace;

		// Token: 0x0400027A RID: 634
		private bool _isBracingCacheValid;

		// Token: 0x0200040A RID: 1034
		public class Hitter
		{
			// Token: 0x17000A13 RID: 2579
			// (get) Token: 0x060037E2 RID: 14306 RVA: 0x000E59F4 File Offset: 0x000E3BF4
			// (set) Token: 0x060037E3 RID: 14307 RVA: 0x000E59FC File Offset: 0x000E3BFC
			public float Damage { get; private set; }

			// Token: 0x060037E4 RID: 14308 RVA: 0x000E5A05 File Offset: 0x000E3C05
			public Hitter(MissionPeer peer, float damage, bool isFriendlyHit)
			{
				this.HitterPeer = peer;
				this.Damage = damage;
				this.IsFriendlyHit = isFriendlyHit;
			}

			// Token: 0x060037E5 RID: 14309 RVA: 0x000E5A22 File Offset: 0x000E3C22
			public void IncreaseDamage(float amount)
			{
				this.Damage += amount;
			}

			// Token: 0x040017FF RID: 6143
			public const float AssistMinDamage = 35f;

			// Token: 0x04001800 RID: 6144
			public readonly MissionPeer HitterPeer;

			// Token: 0x04001801 RID: 6145
			public readonly bool IsFriendlyHit;
		}

		// Token: 0x0200040B RID: 1035
		public struct AgentLastHitInfo
		{
			// Token: 0x17000A14 RID: 2580
			// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000E5A32 File Offset: 0x000E3C32
			// (set) Token: 0x060037E7 RID: 14311 RVA: 0x000E5A3A File Offset: 0x000E3C3A
			public int LastBlowOwnerId { get; private set; }

			// Token: 0x17000A15 RID: 2581
			// (get) Token: 0x060037E8 RID: 14312 RVA: 0x000E5A43 File Offset: 0x000E3C43
			// (set) Token: 0x060037E9 RID: 14313 RVA: 0x000E5A4B File Offset: 0x000E3C4B
			public AgentAttackType LastBlowAttackType { get; private set; }

			// Token: 0x17000A16 RID: 2582
			// (get) Token: 0x060037EA RID: 14314 RVA: 0x000E5A54 File Offset: 0x000E3C54
			public bool CanOverrideBlow
			{
				get
				{
					return this.LastBlowOwnerId >= 0 && this._lastBlowTimer.ElapsedTime <= 5f;
				}
			}

			// Token: 0x060037EB RID: 14315 RVA: 0x000E5A76 File Offset: 0x000E3C76
			public void Initialize()
			{
				this.LastBlowOwnerId = -1;
				this.LastBlowAttackType = AgentAttackType.Standard;
				this._lastBlowTimer = new BasicMissionTimer();
			}

			// Token: 0x060037EC RID: 14316 RVA: 0x000E5A91 File Offset: 0x000E3C91
			public void RegisterLastBlow(int ownerId, AgentAttackType attackType)
			{
				this._lastBlowTimer.Reset();
				this.LastBlowOwnerId = ownerId;
				this.LastBlowAttackType = attackType;
			}

			// Token: 0x04001803 RID: 6147
			private BasicMissionTimer _lastBlowTimer;
		}

		// Token: 0x0200040C RID: 1036
		public struct AgentPropertiesModifiers
		{
			// Token: 0x04001806 RID: 6150
			public bool resetAiWaitBeforeShootFactor;
		}

		// Token: 0x0200040D RID: 1037
		public struct StackArray8Agent
		{
			// Token: 0x17000A17 RID: 2583
			public Agent this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					default:
						return null;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x04001807 RID: 6151
			private Agent _element0;

			// Token: 0x04001808 RID: 6152
			private Agent _element1;

			// Token: 0x04001809 RID: 6153
			private Agent _element2;

			// Token: 0x0400180A RID: 6154
			private Agent _element3;

			// Token: 0x0400180B RID: 6155
			private Agent _element4;

			// Token: 0x0400180C RID: 6156
			private Agent _element5;

			// Token: 0x0400180D RID: 6157
			private Agent _element6;

			// Token: 0x0400180E RID: 6158
			private Agent _element7;

			// Token: 0x0400180F RID: 6159
			public const int Length = 8;
		}

		// Token: 0x0200040E RID: 1038
		public enum ActionStage
		{
			// Token: 0x04001811 RID: 6161
			None = -1,
			// Token: 0x04001812 RID: 6162
			AttackReady,
			// Token: 0x04001813 RID: 6163
			AttackQuickReady,
			// Token: 0x04001814 RID: 6164
			AttackRelease,
			// Token: 0x04001815 RID: 6165
			ReloadMidPhase,
			// Token: 0x04001816 RID: 6166
			ReloadLastPhase,
			// Token: 0x04001817 RID: 6167
			Defend,
			// Token: 0x04001818 RID: 6168
			DefendParry,
			// Token: 0x04001819 RID: 6169
			NumActionStages
		}

		// Token: 0x0200040F RID: 1039
		[Flags]
		public enum AIScriptedFrameFlags
		{
			// Token: 0x0400181B RID: 6171
			None = 0,
			// Token: 0x0400181C RID: 6172
			GoToPosition = 1,
			// Token: 0x0400181D RID: 6173
			NoAttack = 2,
			// Token: 0x0400181E RID: 6174
			ConsiderRotation = 4,
			// Token: 0x0400181F RID: 6175
			NeverSlowDown = 8,
			// Token: 0x04001820 RID: 6176
			DoNotRun = 16,
			// Token: 0x04001821 RID: 6177
			GoWithoutMount = 32,
			// Token: 0x04001822 RID: 6178
			RangerCanMoveForClearTarget = 128,
			// Token: 0x04001823 RID: 6179
			InConversation = 256,
			// Token: 0x04001824 RID: 6180
			Crouch = 512,
			// Token: 0x04001825 RID: 6181
			Drag = 1024
		}

		// Token: 0x02000410 RID: 1040
		[Flags]
		public enum AISpecialCombatModeFlags
		{
			// Token: 0x04001827 RID: 6183
			None = 0,
			// Token: 0x04001828 RID: 6184
			AttackEntity = 1,
			// Token: 0x04001829 RID: 6185
			SurroundAttackEntity = 2,
			// Token: 0x0400182A RID: 6186
			IgnoreAmmoLimitForRangeCalculation = 1024
		}

		// Token: 0x02000411 RID: 1041
		[Flags]
		[EngineStruct("Ai_state_flag", true, "aisf", false)]
		public enum AIStateFlag : uint
		{
			// Token: 0x0400182C RID: 6188
			None = 0U,
			// Token: 0x0400182D RID: 6189
			Cautious = 1U,
			// Token: 0x0400182E RID: 6190
			PatrollingCautious = 2U,
			// Token: 0x0400182F RID: 6191
			Alarmed = 3U,
			// Token: 0x04001830 RID: 6192
			Paused = 8U,
			// Token: 0x04001831 RID: 6193
			UseObjectMoving = 16U,
			// Token: 0x04001832 RID: 6194
			UseObjectUsing = 32U,
			// Token: 0x04001833 RID: 6195
			UseObjectWaiting = 64U,
			// Token: 0x04001834 RID: 6196
			ColumnwiseFollow = 256U,
			// Token: 0x04001835 RID: 6197
			AlarmStateMask = 3U
		}

		// Token: 0x02000412 RID: 1042
		public enum WatchState
		{
			// Token: 0x04001837 RID: 6199
			Patrolling,
			// Token: 0x04001838 RID: 6200
			Cautious,
			// Token: 0x04001839 RID: 6201
			Alarmed
		}

		// Token: 0x02000413 RID: 1043
		public enum MortalityState
		{
			// Token: 0x0400183B RID: 6203
			Mortal,
			// Token: 0x0400183C RID: 6204
			Invulnerable,
			// Token: 0x0400183D RID: 6205
			Immortal
		}

		// Token: 0x02000414 RID: 1044
		public enum CreationType
		{
			// Token: 0x0400183F RID: 6207
			Invalid,
			// Token: 0x04001840 RID: 6208
			FromRoster,
			// Token: 0x04001841 RID: 6209
			FromHorseObj,
			// Token: 0x04001842 RID: 6210
			FromCharacterObj
		}

		// Token: 0x02000415 RID: 1045
		[Flags]
		[EngineStruct("Agent_event_control_flags", true, "agce", false)]
		public enum EventControlFlag : uint
		{
			// Token: 0x04001844 RID: 6212
			None = 0U,
			// Token: 0x04001845 RID: 6213
			Dismount = 1U,
			// Token: 0x04001846 RID: 6214
			Mount = 2U,
			// Token: 0x04001847 RID: 6215
			Rear = 4U,
			// Token: 0x04001848 RID: 6216
			Jump = 8U,
			// Token: 0x04001849 RID: 6217
			Wield0 = 16U,
			// Token: 0x0400184A RID: 6218
			Wield1 = 32U,
			// Token: 0x0400184B RID: 6219
			Wield2 = 64U,
			// Token: 0x0400184C RID: 6220
			Wield3 = 128U,
			// Token: 0x0400184D RID: 6221
			Sheath0 = 256U,
			// Token: 0x0400184E RID: 6222
			Sheath1 = 512U,
			// Token: 0x0400184F RID: 6223
			ToggleAlternativeWeapon = 1024U,
			// Token: 0x04001850 RID: 6224
			Walk = 2048U,
			// Token: 0x04001851 RID: 6225
			Run = 4096U,
			// Token: 0x04001852 RID: 6226
			Crouch = 8192U,
			// Token: 0x04001853 RID: 6227
			Stand = 16384U,
			// Token: 0x04001854 RID: 6228
			Kick = 32768U,
			// Token: 0x04001855 RID: 6229
			DoubleTapToDirectionUp = 65536U,
			// Token: 0x04001856 RID: 6230
			DoubleTapToDirectionDown = 131072U,
			// Token: 0x04001857 RID: 6231
			DoubleTapToDirectionLeft = 196608U,
			// Token: 0x04001858 RID: 6232
			DoubleTapToDirectionRight = 262144U,
			// Token: 0x04001859 RID: 6233
			DoubleTapToDirectionMask = 458752U
		}

		// Token: 0x02000416 RID: 1046
		public enum FacialAnimChannel
		{
			// Token: 0x0400185B RID: 6235
			High,
			// Token: 0x0400185C RID: 6236
			Mid,
			// Token: 0x0400185D RID: 6237
			Low,
			// Token: 0x0400185E RID: 6238
			num_facial_anim_channels
		}

		// Token: 0x02000417 RID: 1047
		[EngineStruct("Action_code_type", true, "actt", false)]
		public enum ActionCodeType
		{
			// Token: 0x04001860 RID: 6240
			Other,
			// Token: 0x04001861 RID: 6241
			DefendFist,
			// Token: 0x04001862 RID: 6242
			DefendShield,
			// Token: 0x04001863 RID: 6243
			DefendForward2h,
			// Token: 0x04001864 RID: 6244
			DefendUp2h,
			// Token: 0x04001865 RID: 6245
			DefendRight2h,
			// Token: 0x04001866 RID: 6246
			DefendLeft2h,
			// Token: 0x04001867 RID: 6247
			DefendForward1h,
			// Token: 0x04001868 RID: 6248
			DefendUp1h,
			// Token: 0x04001869 RID: 6249
			DefendRight1h,
			// Token: 0x0400186A RID: 6250
			DefendLeft1h,
			// Token: 0x0400186B RID: 6251
			DefendForwardStaff,
			// Token: 0x0400186C RID: 6252
			DefendUpStaff,
			// Token: 0x0400186D RID: 6253
			DefendRightStaff,
			// Token: 0x0400186E RID: 6254
			DefendLeftStaff,
			// Token: 0x0400186F RID: 6255
			ReadyRanged,
			// Token: 0x04001870 RID: 6256
			ReleaseRanged,
			// Token: 0x04001871 RID: 6257
			ReleaseThrowing,
			// Token: 0x04001872 RID: 6258
			Reload,
			// Token: 0x04001873 RID: 6259
			ReadyMelee,
			// Token: 0x04001874 RID: 6260
			ReleaseMelee,
			// Token: 0x04001875 RID: 6261
			ParriedMelee,
			// Token: 0x04001876 RID: 6262
			BlockedMelee,
			// Token: 0x04001877 RID: 6263
			Fall,
			// Token: 0x04001878 RID: 6264
			JumpStart,
			// Token: 0x04001879 RID: 6265
			Jump,
			// Token: 0x0400187A RID: 6266
			JumpEnd,
			// Token: 0x0400187B RID: 6267
			JumpEndHard,
			// Token: 0x0400187C RID: 6268
			Kick,
			// Token: 0x0400187D RID: 6269
			KickContinue,
			// Token: 0x0400187E RID: 6270
			KickHit,
			// Token: 0x0400187F RID: 6271
			WeaponBash,
			// Token: 0x04001880 RID: 6272
			PassiveUsage,
			// Token: 0x04001881 RID: 6273
			EquipUnequip,
			// Token: 0x04001882 RID: 6274
			SwitchAlternative,
			// Token: 0x04001883 RID: 6275
			Idle,
			// Token: 0x04001884 RID: 6276
			Guard,
			// Token: 0x04001885 RID: 6277
			Mount,
			// Token: 0x04001886 RID: 6278
			Dismount,
			// Token: 0x04001887 RID: 6279
			Dash,
			// Token: 0x04001888 RID: 6280
			MountQuickStop,
			// Token: 0x04001889 RID: 6281
			HitObject,
			// Token: 0x0400188A RID: 6282
			Sit,
			// Token: 0x0400188B RID: 6283
			SitOnTheFloor,
			// Token: 0x0400188C RID: 6284
			SitOnAThrone,
			// Token: 0x0400188D RID: 6285
			LadderRaise,
			// Token: 0x0400188E RID: 6286
			LadderRaiseEnd,
			// Token: 0x0400188F RID: 6287
			Rear,
			// Token: 0x04001890 RID: 6288
			StrikeLight,
			// Token: 0x04001891 RID: 6289
			StrikeMedium,
			// Token: 0x04001892 RID: 6290
			StrikeHeavy,
			// Token: 0x04001893 RID: 6291
			StrikeKnockBack,
			// Token: 0x04001894 RID: 6292
			MountStrike,
			// Token: 0x04001895 RID: 6293
			Count,
			// Token: 0x04001896 RID: 6294
			StrikeBegin = 48,
			// Token: 0x04001897 RID: 6295
			StrikeEnd = 52,
			// Token: 0x04001898 RID: 6296
			DefendAllBegin = 1,
			// Token: 0x04001899 RID: 6297
			DefendAllEnd = 15,
			// Token: 0x0400189A RID: 6298
			AttackMeleeAllBegin = 19,
			// Token: 0x0400189B RID: 6299
			AttackMeleeAllEnd = 23,
			// Token: 0x0400189C RID: 6300
			AttackMeleeAndRangedAllBegin = 15,
			// Token: 0x0400189D RID: 6301
			AttackMeleeAndRangedAllEnd = 23,
			// Token: 0x0400189E RID: 6302
			CombatAllBegin = 1,
			// Token: 0x0400189F RID: 6303
			CombatAllEnd = 23,
			// Token: 0x040018A0 RID: 6304
			JumpAllBegin,
			// Token: 0x040018A1 RID: 6305
			JumpAllEnd = 28,
			// Token: 0x040018A2 RID: 6306
			FallAllBegin = 25,
			// Token: 0x040018A3 RID: 6307
			FallAllEnd = 28,
			// Token: 0x040018A4 RID: 6308
			KickAllBegin = 28,
			// Token: 0x040018A5 RID: 6309
			KickAllEnd = 31,
			// Token: 0x040018A6 RID: 6310
			AlternativeAttackAllBegin = 28,
			// Token: 0x040018A7 RID: 6311
			AlternativeAttackAllEnd = 32
		}

		// Token: 0x02000418 RID: 1048
		[EngineStruct("Agent_guard_mode", true, "guard_mode", false)]
		public enum GuardMode
		{
			// Token: 0x040018A9 RID: 6313
			MarkForDeletion = -2,
			// Token: 0x040018AA RID: 6314
			None,
			// Token: 0x040018AB RID: 6315
			Up,
			// Token: 0x040018AC RID: 6316
			Down,
			// Token: 0x040018AD RID: 6317
			Left,
			// Token: 0x040018AE RID: 6318
			Right
		}

		// Token: 0x02000419 RID: 1049
		public enum HandIndex
		{
			// Token: 0x040018B0 RID: 6320
			MainHand,
			// Token: 0x040018B1 RID: 6321
			OffHand
		}

		// Token: 0x0200041A RID: 1050
		[EngineStruct("rglInt8", false, null)]
		public enum KillInfo : sbyte
		{
			// Token: 0x040018B3 RID: 6323
			Invalid = -1,
			// Token: 0x040018B4 RID: 6324
			Headshot,
			// Token: 0x040018B5 RID: 6325
			CouchedLance,
			// Token: 0x040018B6 RID: 6326
			Punch,
			// Token: 0x040018B7 RID: 6327
			MountHit,
			// Token: 0x040018B8 RID: 6328
			Bow,
			// Token: 0x040018B9 RID: 6329
			Crossbow,
			// Token: 0x040018BA RID: 6330
			ThrowingAxe,
			// Token: 0x040018BB RID: 6331
			ThrowingKnife,
			// Token: 0x040018BC RID: 6332
			Javelin,
			// Token: 0x040018BD RID: 6333
			Stone,
			// Token: 0x040018BE RID: 6334
			Pistol,
			// Token: 0x040018BF RID: 6335
			Musket,
			// Token: 0x040018C0 RID: 6336
			OneHandedSword,
			// Token: 0x040018C1 RID: 6337
			TwoHandedSword,
			// Token: 0x040018C2 RID: 6338
			OneHandedAxe,
			// Token: 0x040018C3 RID: 6339
			TwoHandedAxe,
			// Token: 0x040018C4 RID: 6340
			Mace,
			// Token: 0x040018C5 RID: 6341
			Spear,
			// Token: 0x040018C6 RID: 6342
			Morningstar,
			// Token: 0x040018C7 RID: 6343
			Maul,
			// Token: 0x040018C8 RID: 6344
			Backstabbed,
			// Token: 0x040018C9 RID: 6345
			Gravity,
			// Token: 0x040018CA RID: 6346
			ShieldBash,
			// Token: 0x040018CB RID: 6347
			WeaponBash,
			// Token: 0x040018CC RID: 6348
			Kick,
			// Token: 0x040018CD RID: 6349
			TeamSwitch
		}

		// Token: 0x0200041B RID: 1051
		public enum MovementBehaviorType
		{
			// Token: 0x040018CF RID: 6351
			Engaged,
			// Token: 0x040018D0 RID: 6352
			Idle,
			// Token: 0x040018D1 RID: 6353
			Flee
		}

		// Token: 0x0200041C RID: 1052
		[Flags]
		[EngineStruct("Agent_movement_control_flags", true, "agcm", false)]
		public enum MovementControlFlag : uint
		{
			// Token: 0x040018D3 RID: 6355
			None = 0U,
			// Token: 0x040018D4 RID: 6356
			Forward = 1U,
			// Token: 0x040018D5 RID: 6357
			Backward = 2U,
			// Token: 0x040018D6 RID: 6358
			StrafeRight = 4U,
			// Token: 0x040018D7 RID: 6359
			StrafeLeft = 8U,
			// Token: 0x040018D8 RID: 6360
			TurnRight = 16U,
			// Token: 0x040018D9 RID: 6361
			TurnLeft = 32U,
			// Token: 0x040018DA RID: 6362
			AttackLeft = 64U,
			// Token: 0x040018DB RID: 6363
			AttackRight = 128U,
			// Token: 0x040018DC RID: 6364
			AttackUp = 256U,
			// Token: 0x040018DD RID: 6365
			AttackDown = 512U,
			// Token: 0x040018DE RID: 6366
			DefendLeft = 1024U,
			// Token: 0x040018DF RID: 6367
			DefendRight = 2048U,
			// Token: 0x040018E0 RID: 6368
			DefendUp = 4096U,
			// Token: 0x040018E1 RID: 6369
			DefendDown = 8192U,
			// Token: 0x040018E2 RID: 6370
			DefendAuto = 16384U,
			// Token: 0x040018E3 RID: 6371
			DefendBlock = 32768U,
			// Token: 0x040018E4 RID: 6372
			Action = 65536U,
			// Token: 0x040018E5 RID: 6373
			AttackMask = 960U,
			// Token: 0x040018E6 RID: 6374
			DefendMask = 31744U,
			// Token: 0x040018E7 RID: 6375
			DefendDirMask = 15360U,
			// Token: 0x040018E8 RID: 6376
			MoveMask = 63U,
			// Token: 0x040018E9 RID: 6377
			MaxValue = 131071U
		}

		// Token: 0x0200041D RID: 1053
		public enum UnderAttackType
		{
			// Token: 0x040018EB RID: 6379
			NotUnderAttack,
			// Token: 0x040018EC RID: 6380
			UnderMeleeAttack,
			// Token: 0x040018ED RID: 6381
			UnderRangedAttack
		}

		// Token: 0x0200041E RID: 1054
		[EngineStruct("Usage_direction", true, "ud", false)]
		public enum UsageDirection
		{
			// Token: 0x040018EF RID: 6383
			None = -1,
			// Token: 0x040018F0 RID: 6384
			AttackUp,
			// Token: 0x040018F1 RID: 6385
			AttackDown,
			// Token: 0x040018F2 RID: 6386
			AttackLeft,
			// Token: 0x040018F3 RID: 6387
			AttackRight,
			// Token: 0x040018F4 RID: 6388
			AttackBegin = 0,
			// Token: 0x040018F5 RID: 6389
			AttackEnd = 4,
			// Token: 0x040018F6 RID: 6390
			DefendUp = 4,
			// Token: 0x040018F7 RID: 6391
			DefendDown,
			// Token: 0x040018F8 RID: 6392
			DefendLeft,
			// Token: 0x040018F9 RID: 6393
			DefendRight,
			// Token: 0x040018FA RID: 6394
			DefendBegin = 4,
			// Token: 0x040018FB RID: 6395
			DefendAny = 8,
			// Token: 0x040018FC RID: 6396
			DefendEnd,
			// Token: 0x040018FD RID: 6397
			AttackAny = 9
		}

		// Token: 0x0200041F RID: 1055
		[EngineStruct("Weapon_wield_action_type", false, null)]
		public enum WeaponWieldActionType
		{
			// Token: 0x040018FF RID: 6399
			WithAnimation,
			// Token: 0x04001900 RID: 6400
			Instant,
			// Token: 0x04001901 RID: 6401
			InstantAfterPickUp,
			// Token: 0x04001902 RID: 6402
			WithAnimationUninterruptible
		}

		// Token: 0x02000420 RID: 1056
		[Flags]
		public enum StopUsingGameObjectFlags : byte
		{
			// Token: 0x04001904 RID: 6404
			None = 0,
			// Token: 0x04001905 RID: 6405
			AutoAttachAfterStoppingUsingGameObject = 1,
			// Token: 0x04001906 RID: 6406
			DoNotWieldWeaponAfterStoppingUsingGameObject = 2,
			// Token: 0x04001907 RID: 6407
			DefendAfterStoppingUsingGameObject = 4
		}

		// Token: 0x02000421 RID: 1057
		// (Invoke) Token: 0x060037F0 RID: 14320
		public delegate void OnAgentHealthChangedDelegate(Agent agent, float oldHealth, float newHealth);

		// Token: 0x02000422 RID: 1058
		// (Invoke) Token: 0x060037F4 RID: 14324
		public delegate void OnMountHealthChangedDelegate(Agent agent, Agent mount, float oldHealth, float newHealth);

		// Token: 0x02000423 RID: 1059
		// (Invoke) Token: 0x060037F8 RID: 14328
		public delegate void OnMainAgentWieldedItemChangeDelegate();
	}
}
