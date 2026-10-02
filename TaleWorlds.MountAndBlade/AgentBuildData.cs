using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020000FB RID: 251
	public class AgentBuildData
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00016960 File Offset: 0x00014B60
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x00016968 File Offset: 0x00014B68
		public AgentData AgentData { get; private set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00016971 File Offset: 0x00014B71
		public BasicCharacterObject AgentCharacter
		{
			get
			{
				return this.AgentData.AgentCharacter;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0001697E File Offset: 0x00014B7E
		public Monster AgentMonster
		{
			get
			{
				return this.AgentData.AgentMonster;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0001698B File Offset: 0x00014B8B
		public Equipment AgentOverridenSpawnEquipment
		{
			get
			{
				return this.AgentData.AgentOverridenEquipment;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00016998 File Offset: 0x00014B98
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x000169A0 File Offset: 0x00014BA0
		public MissionEquipment AgentOverridenSpawnMissionEquipment { get; private set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x000169A9 File Offset: 0x00014BA9
		public int AgentEquipmentSeed
		{
			get
			{
				return this.AgentData.AgentEquipmentSeed;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x000169B6 File Offset: 0x00014BB6
		public bool AgentNoHorses
		{
			get
			{
				return this.AgentData.AgentNoHorses;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x000169C3 File Offset: 0x00014BC3
		public string AgentMountKey
		{
			get
			{
				return this.AgentData.AgentMountKey;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x000169D0 File Offset: 0x00014BD0
		public bool AgentNoWeapons
		{
			get
			{
				return this.AgentData.AgentNoWeapons;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x000169DD File Offset: 0x00014BDD
		public bool AgentNoArmor
		{
			get
			{
				return this.AgentData.AgentNoArmor;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x000169EA File Offset: 0x00014BEA
		public bool AgentFixedEquipment
		{
			get
			{
				return this.AgentData.AgentFixedEquipment;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x000169F7 File Offset: 0x00014BF7
		public bool AgentCivilianEquipment
		{
			get
			{
				return this.AgentData.AgentCivilianEquipment;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00016A04 File Offset: 0x00014C04
		public uint AgentClothingColor1
		{
			get
			{
				return this.AgentData.AgentClothingColor1;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00016A11 File Offset: 0x00014C11
		public uint AgentClothingColor2
		{
			get
			{
				return this.AgentData.AgentClothingColor2;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00016A1E File Offset: 0x00014C1E
		public bool BodyPropertiesOverriden
		{
			get
			{
				return this.AgentData.BodyPropertiesOverriden;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00016A2B File Offset: 0x00014C2B
		public BodyProperties AgentBodyProperties
		{
			get
			{
				return this.AgentData.AgentBodyProperties;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x00016A38 File Offset: 0x00014C38
		public bool AgeOverriden
		{
			get
			{
				return this.AgentData.AgeOverriden;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x00016A45 File Offset: 0x00014C45
		public int AgentAge
		{
			get
			{
				return this.AgentData.AgentAge;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00016A52 File Offset: 0x00014C52
		public bool PrepareImmediately
		{
			get
			{
				return this.AgentData.PrepareImmediately;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00016A5F File Offset: 0x00014C5F
		public bool GenderOverriden
		{
			get
			{
				return this.AgentData.GenderOverriden;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00016A6C File Offset: 0x00014C6C
		public bool AgentIsFemale
		{
			get
			{
				return this.AgentData.AgentIsFemale;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00016A79 File Offset: 0x00014C79
		public int AgentRace
		{
			get
			{
				return this.AgentData.AgentRace;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00016A86 File Offset: 0x00014C86
		public IAgentOriginBase AgentOrigin
		{
			get
			{
				return this.AgentData.AgentOrigin;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00016A93 File Offset: 0x00014C93
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x00016A9B File Offset: 0x00014C9B
		public AgentControllerType AgentController { get; private set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00016AA4 File Offset: 0x00014CA4
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x00016AAC File Offset: 0x00014CAC
		public Team AgentTeam { get; private set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00016AB5 File Offset: 0x00014CB5
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x00016ABD File Offset: 0x00014CBD
		public bool AgentIsReinforcement { get; private set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00016AC6 File Offset: 0x00014CC6
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00016ACE File Offset: 0x00014CCE
		public bool AgentSpawnsIntoOwnFormation { get; private set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00016AD7 File Offset: 0x00014CD7
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x00016ADF File Offset: 0x00014CDF
		public bool AgentSpawnsUsingOwnTroopClass { get; private set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00016AE8 File Offset: 0x00014CE8
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x00016AF0 File Offset: 0x00014CF0
		public float MakeUnitStandOutDistance { get; private set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00016AF9 File Offset: 0x00014CF9
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x00016B01 File Offset: 0x00014D01
		public Vec3? AgentInitialPosition { get; private set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00016B0A File Offset: 0x00014D0A
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x00016B12 File Offset: 0x00014D12
		public Vec2? AgentInitialDirection { get; private set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00016B1B File Offset: 0x00014D1B
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x00016B23 File Offset: 0x00014D23
		public Formation AgentFormation { get; private set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00016B2C File Offset: 0x00014D2C
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x00016B34 File Offset: 0x00014D34
		public int AgentFormationTroopSpawnCount { get; private set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00016B3D File Offset: 0x00014D3D
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x00016B45 File Offset: 0x00014D45
		public int AgentFormationTroopSpawnIndex { get; private set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x00016B4E File Offset: 0x00014D4E
		// (set) Token: 0x06000BD5 RID: 3029 RVA: 0x00016B56 File Offset: 0x00014D56
		public MissionPeer AgentMissionPeer { get; private set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x00016B5F File Offset: 0x00014D5F
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x00016B67 File Offset: 0x00014D67
		public MissionPeer OwningAgentMissionPeer { get; private set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00016B70 File Offset: 0x00014D70
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x00016B78 File Offset: 0x00014D78
		public bool AgentIndexOverriden { get; private set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00016B81 File Offset: 0x00014D81
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x00016B89 File Offset: 0x00014D89
		public int AgentIndex { get; private set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00016B92 File Offset: 0x00014D92
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x00016B9A File Offset: 0x00014D9A
		public bool AgentMountIndexOverriden { get; private set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00016BA3 File Offset: 0x00014DA3
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x00016BAB File Offset: 0x00014DAB
		public int AgentMountIndex { get; private set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00016BB4 File Offset: 0x00014DB4
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x00016BBC File Offset: 0x00014DBC
		public int AgentVisualsIndex { get; private set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00016BC5 File Offset: 0x00014DC5
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x00016BCD File Offset: 0x00014DCD
		public Banner AgentBanner { get; private set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00016BD6 File Offset: 0x00014DD6
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00016BDE File Offset: 0x00014DDE
		public ItemObject AgentBannerItem { get; private set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00016BE7 File Offset: 0x00014DE7
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00016BEF File Offset: 0x00014DEF
		public ItemObject AgentBannerReplacementWeaponItem { get; private set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00016BF8 File Offset: 0x00014DF8
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00016C00 File Offset: 0x00014E00
		public bool AgentCanSpawnOutsideOfMissionBoundary { get; private set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00016C09 File Offset: 0x00014E09
		public bool RandomizeColors
		{
			get
			{
				return this.AgentCharacter != null && !this.AgentCharacter.IsHero && this.AgentMissionPeer == null;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00016C2B File Offset: 0x00014E2B
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00016C33 File Offset: 0x00014E33
		public bool UseFaceCache { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00016C3C File Offset: 0x00014E3C
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x00016C44 File Offset: 0x00014E44
		public int FaceCacheId { get; set; }

		// Token: 0x06000BEF RID: 3055 RVA: 0x00016C4D File Offset: 0x00014E4D
		private AgentBuildData()
		{
			this.AgentController = AgentControllerType.AI;
			this.AgentTeam = TaleWorlds.MountAndBlade.Team.Invalid;
			this.AgentFormation = null;
			this.AgentMissionPeer = null;
			this.AgentFormationTroopSpawnIndex = -1;
			this.UseFaceCache = false;
			this.FaceCacheId = 0;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00016C8A File Offset: 0x00014E8A
		public AgentBuildData(AgentData agentData)
			: this()
		{
			this.AgentData = agentData;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00016C99 File Offset: 0x00014E99
		public AgentBuildData(IAgentOriginBase agentOrigin)
			: this()
		{
			this.AgentData = new AgentData(agentOrigin);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00016CAD File Offset: 0x00014EAD
		public AgentBuildData(BasicCharacterObject characterObject)
			: this()
		{
			this.AgentData = new AgentData(characterObject);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00016CC1 File Offset: 0x00014EC1
		public AgentBuildData Character(BasicCharacterObject characterObject)
		{
			this.AgentData.Character(characterObject);
			return this;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00016CD1 File Offset: 0x00014ED1
		public AgentBuildData Controller(AgentControllerType controller)
		{
			this.AgentController = controller;
			return this;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00016CDB File Offset: 0x00014EDB
		public AgentBuildData Team(Team team)
		{
			this.AgentTeam = team;
			return this;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00016CE5 File Offset: 0x00014EE5
		public AgentBuildData IsReinforcement(bool isReinforcement)
		{
			this.AgentIsReinforcement = isReinforcement;
			return this;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00016CEF File Offset: 0x00014EEF
		public AgentBuildData SpawnsIntoOwnFormation(bool spawnIntoOwnFormation)
		{
			this.AgentSpawnsIntoOwnFormation = spawnIntoOwnFormation;
			return this;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00016CF9 File Offset: 0x00014EF9
		public AgentBuildData SpawnsUsingOwnTroopClass(bool spawnUsingOwnTroopClass)
		{
			this.AgentSpawnsUsingOwnTroopClass = spawnUsingOwnTroopClass;
			return this;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00016D03 File Offset: 0x00014F03
		public AgentBuildData MakeUnitStandOutOfFormationDistance(float makeUnitStandOutDistance)
		{
			this.MakeUnitStandOutDistance = makeUnitStandOutDistance;
			return this;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00016D0D File Offset: 0x00014F0D
		public AgentBuildData InitialPosition(in Vec3 position)
		{
			this.AgentInitialPosition = new Vec3?(position);
			return this;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00016D21 File Offset: 0x00014F21
		public AgentBuildData InitialDirection(in Vec2 direction)
		{
			this.AgentInitialDirection = new Vec2?(direction);
			return this;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00016D38 File Offset: 0x00014F38
		public AgentBuildData InitialFrameFromSpawnPointEntity(GameEntity entity)
		{
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			this.AgentInitialPosition = new Vec3?(globalFrame.origin);
			this.AgentInitialDirection = new Vec2?(globalFrame.rotation.f.AsVec2.Normalized());
			return this;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00016D84 File Offset: 0x00014F84
		public AgentBuildData InitialFrameFromSpawnPointEntity(WeakGameEntity entity)
		{
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			this.AgentInitialPosition = new Vec3?(globalFrame.origin);
			this.AgentInitialDirection = new Vec2?(globalFrame.rotation.f.AsVec2.Normalized());
			return this;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00016DCF File Offset: 0x00014FCF
		public AgentBuildData Formation(Formation formation)
		{
			this.AgentFormation = formation;
			return this;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00016DD9 File Offset: 0x00014FD9
		public AgentBuildData Monster(Monster monster)
		{
			this.AgentData.Monster(monster);
			return this;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00016DE9 File Offset: 0x00014FE9
		public AgentBuildData VisualsIndex(int index)
		{
			this.AgentVisualsIndex = index;
			return this;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00016DF3 File Offset: 0x00014FF3
		public AgentBuildData Equipment(Equipment equipment)
		{
			this.AgentData.Equipment(equipment);
			return this;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00016E03 File Offset: 0x00015003
		public AgentBuildData MissionEquipment(MissionEquipment missionEquipment)
		{
			this.AgentOverridenSpawnMissionEquipment = missionEquipment;
			return this;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00016E0D File Offset: 0x0001500D
		public AgentBuildData EquipmentSeed(int seed)
		{
			this.AgentData.EquipmentSeed(seed);
			return this;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00016E1D File Offset: 0x0001501D
		public AgentBuildData NoHorses(bool noHorses)
		{
			this.AgentData.NoHorses(noHorses);
			return this;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00016E2D File Offset: 0x0001502D
		public AgentBuildData NoWeapons(bool noWeapons)
		{
			this.AgentData.NoWeapons(noWeapons);
			return this;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00016E3D File Offset: 0x0001503D
		public AgentBuildData NoArmor(bool noArmor)
		{
			this.AgentData.NoArmor(noArmor);
			return this;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00016E4D File Offset: 0x0001504D
		public AgentBuildData FixedEquipment(bool fixedEquipment)
		{
			this.AgentData.FixedEquipment(fixedEquipment);
			return this;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00016E5D File Offset: 0x0001505D
		public AgentBuildData CivilianEquipment(bool civilianEquipment)
		{
			this.AgentData.CivilianEquipment(civilianEquipment);
			return this;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00016E6D File Offset: 0x0001506D
		public AgentBuildData SetPrepareImmediately()
		{
			this.AgentData.SetPrepareImmediately();
			return this;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00016E7C File Offset: 0x0001507C
		public AgentBuildData ClothingColor1(uint color)
		{
			this.AgentData.ClothingColor1(color);
			return this;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00016E8C File Offset: 0x0001508C
		public AgentBuildData ClothingColor2(uint color)
		{
			this.AgentData.ClothingColor2(color);
			return this;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00016E9C File Offset: 0x0001509C
		public AgentBuildData MissionPeer(MissionPeer missionPeer)
		{
			this.AgentMissionPeer = missionPeer;
			return this;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00016EA6 File Offset: 0x000150A6
		public AgentBuildData OwningMissionPeer(MissionPeer missionPeer)
		{
			this.OwningAgentMissionPeer = missionPeer;
			return this;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00016EB0 File Offset: 0x000150B0
		public AgentBuildData BodyProperties(BodyProperties bodyProperties)
		{
			this.AgentData.BodyProperties(bodyProperties);
			return this;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00016EC0 File Offset: 0x000150C0
		public AgentBuildData Age(int age)
		{
			this.AgentData.Age(age);
			return this;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00016ED0 File Offset: 0x000150D0
		public AgentBuildData TroopOrigin(IAgentOriginBase troopOrigin)
		{
			this.AgentData.TroopOrigin(troopOrigin);
			return this;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00016EE0 File Offset: 0x000150E0
		public AgentBuildData IsFemale(bool isFemale)
		{
			this.AgentData.IsFemale(isFemale);
			return this;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00016EF0 File Offset: 0x000150F0
		public AgentBuildData Race(int race)
		{
			this.AgentData.Race(race);
			return this;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00016F00 File Offset: 0x00015100
		public AgentBuildData MountKey(string mountKey)
		{
			this.AgentData.MountKey(mountKey);
			return this;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00016F10 File Offset: 0x00015110
		public AgentBuildData Index(int index)
		{
			this.AgentIndex = index;
			this.AgentIndexOverriden = true;
			return this;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00016F21 File Offset: 0x00015121
		public AgentBuildData MountIndex(int mountIndex)
		{
			this.AgentMountIndex = mountIndex;
			this.AgentMountIndexOverriden = true;
			return this;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00016F32 File Offset: 0x00015132
		public AgentBuildData Banner(Banner banner)
		{
			this.AgentBanner = banner;
			return this;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00016F3C File Offset: 0x0001513C
		public AgentBuildData BannerItem(ItemObject bannerItem)
		{
			this.AgentBannerItem = bannerItem;
			return this;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00016F46 File Offset: 0x00015146
		public AgentBuildData BannerReplacementWeaponItem(ItemObject weaponItem)
		{
			this.AgentBannerReplacementWeaponItem = weaponItem;
			return this;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00016F50 File Offset: 0x00015150
		public AgentBuildData FormationTroopSpawnCount(int formationTroopCount)
		{
			this.AgentFormationTroopSpawnCount = formationTroopCount;
			return this;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00016F5A File Offset: 0x0001515A
		public AgentBuildData FormationTroopSpawnIndex(int formationTroopIndex)
		{
			this.AgentFormationTroopSpawnIndex = formationTroopIndex;
			return this;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00016F64 File Offset: 0x00015164
		public AgentBuildData CanSpawnOutsideOfMissionBoundary(bool canSpawn)
		{
			this.AgentCanSpawnOutsideOfMissionBoundary = canSpawn;
			return this;
		}
	}
}
