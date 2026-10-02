using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C6 RID: 710
	public class AgentVisualsData
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x0009A78C File Offset: 0x0009898C
		// (set) Token: 0x060028C7 RID: 10439 RVA: 0x0009A794 File Offset: 0x00098994
		public MBActionSet ActionSetData { get; private set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x0009A79D File Offset: 0x0009899D
		// (set) Token: 0x060028C9 RID: 10441 RVA: 0x0009A7A5 File Offset: 0x000989A5
		public MatrixFrame FrameData { get; private set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x0009A7AE File Offset: 0x000989AE
		// (set) Token: 0x060028CB RID: 10443 RVA: 0x0009A7B6 File Offset: 0x000989B6
		public BodyProperties BodyPropertiesData { get; private set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x0009A7BF File Offset: 0x000989BF
		// (set) Token: 0x060028CD RID: 10445 RVA: 0x0009A7C7 File Offset: 0x000989C7
		public Equipment EquipmentData { get; private set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x0009A7D0 File Offset: 0x000989D0
		// (set) Token: 0x060028CF RID: 10447 RVA: 0x0009A7D8 File Offset: 0x000989D8
		public int RightWieldedItemIndexData { get; private set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x0009A7E1 File Offset: 0x000989E1
		// (set) Token: 0x060028D1 RID: 10449 RVA: 0x0009A7E9 File Offset: 0x000989E9
		public int LeftWieldedItemIndexData { get; private set; }

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0009A7F2 File Offset: 0x000989F2
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x0009A7FA File Offset: 0x000989FA
		public SkeletonType SkeletonTypeData { get; private set; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x0009A803 File Offset: 0x00098A03
		// (set) Token: 0x060028D5 RID: 10453 RVA: 0x0009A80B File Offset: 0x00098A0B
		public Banner BannerData { get; private set; }

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x0009A814 File Offset: 0x00098A14
		// (set) Token: 0x060028D7 RID: 10455 RVA: 0x0009A81C File Offset: 0x00098A1C
		public GameEntity CachedWeaponSlot0Entity { get; private set; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x0009A825 File Offset: 0x00098A25
		// (set) Token: 0x060028D9 RID: 10457 RVA: 0x0009A82D File Offset: 0x00098A2D
		public GameEntity CachedWeaponSlot1Entity { get; private set; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x0009A836 File Offset: 0x00098A36
		// (set) Token: 0x060028DB RID: 10459 RVA: 0x0009A83E File Offset: 0x00098A3E
		public GameEntity CachedWeaponSlot2Entity { get; private set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x0009A847 File Offset: 0x00098A47
		// (set) Token: 0x060028DD RID: 10461 RVA: 0x0009A84F File Offset: 0x00098A4F
		public GameEntity CachedWeaponSlot3Entity { get; private set; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x0009A858 File Offset: 0x00098A58
		// (set) Token: 0x060028DF RID: 10463 RVA: 0x0009A860 File Offset: 0x00098A60
		public GameEntity CachedWeaponSlot4Entity { get; private set; }

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060028E0 RID: 10464 RVA: 0x0009A869 File Offset: 0x00098A69
		// (set) Token: 0x060028E1 RID: 10465 RVA: 0x0009A871 File Offset: 0x00098A71
		public Scene SceneData { get; private set; }

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060028E2 RID: 10466 RVA: 0x0009A87A File Offset: 0x00098A7A
		// (set) Token: 0x060028E3 RID: 10467 RVA: 0x0009A882 File Offset: 0x00098A82
		public Monster MonsterData { get; private set; }

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x0009A88B File Offset: 0x00098A8B
		// (set) Token: 0x060028E5 RID: 10469 RVA: 0x0009A893 File Offset: 0x00098A93
		public bool PrepareImmediatelyData { get; private set; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x0009A89C File Offset: 0x00098A9C
		// (set) Token: 0x060028E7 RID: 10471 RVA: 0x0009A8A4 File Offset: 0x00098AA4
		public bool UseScaledWeaponsData { get; private set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x0009A8AD File Offset: 0x00098AAD
		// (set) Token: 0x060028E9 RID: 10473 RVA: 0x0009A8B5 File Offset: 0x00098AB5
		public bool UseTranslucencyData { get; private set; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x0009A8BE File Offset: 0x00098ABE
		// (set) Token: 0x060028EB RID: 10475 RVA: 0x0009A8C6 File Offset: 0x00098AC6
		public bool UseTesselationData { get; private set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x0009A8CF File Offset: 0x00098ACF
		// (set) Token: 0x060028ED RID: 10477 RVA: 0x0009A8D7 File Offset: 0x00098AD7
		public bool UseMorphAnimsData { get; private set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x0009A8E0 File Offset: 0x00098AE0
		// (set) Token: 0x060028EF RID: 10479 RVA: 0x0009A8E8 File Offset: 0x00098AE8
		public uint ClothColor1Data { get; private set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060028F0 RID: 10480 RVA: 0x0009A8F1 File Offset: 0x00098AF1
		// (set) Token: 0x060028F1 RID: 10481 RVA: 0x0009A8F9 File Offset: 0x00098AF9
		public uint ClothColor2Data { get; private set; }

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060028F2 RID: 10482 RVA: 0x0009A902 File Offset: 0x00098B02
		// (set) Token: 0x060028F3 RID: 10483 RVA: 0x0009A90A File Offset: 0x00098B0A
		public float ScaleData { get; private set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060028F4 RID: 10484 RVA: 0x0009A913 File Offset: 0x00098B13
		// (set) Token: 0x060028F5 RID: 10485 RVA: 0x0009A91B File Offset: 0x00098B1B
		public string CharacterObjectStringIdData { get; private set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x0009A924 File Offset: 0x00098B24
		// (set) Token: 0x060028F7 RID: 10487 RVA: 0x0009A92C File Offset: 0x00098B2C
		public ActionIndexCache ActionCodeData { get; private set; } = ActionIndexCache.act_none;

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x0009A935 File Offset: 0x00098B35
		// (set) Token: 0x060028F9 RID: 10489 RVA: 0x0009A93D File Offset: 0x00098B3D
		public GameEntity EntityData { get; private set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060028FA RID: 10490 RVA: 0x0009A946 File Offset: 0x00098B46
		// (set) Token: 0x060028FB RID: 10491 RVA: 0x0009A94E File Offset: 0x00098B4E
		public bool HasClippingPlaneData { get; private set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060028FC RID: 10492 RVA: 0x0009A957 File Offset: 0x00098B57
		// (set) Token: 0x060028FD RID: 10493 RVA: 0x0009A95F File Offset: 0x00098B5F
		public string MountCreationKeyData { get; private set; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x0009A968 File Offset: 0x00098B68
		// (set) Token: 0x060028FF RID: 10495 RVA: 0x0009A970 File Offset: 0x00098B70
		public bool AddColorRandomnessData { get; private set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002900 RID: 10496 RVA: 0x0009A979 File Offset: 0x00098B79
		// (set) Token: 0x06002901 RID: 10497 RVA: 0x0009A981 File Offset: 0x00098B81
		public int RaceData { get; private set; }

		// Token: 0x06002902 RID: 10498 RVA: 0x0009A98C File Offset: 0x00098B8C
		public AgentVisualsData(AgentVisualsData agentVisualsData)
		{
			this.AgentVisuals = agentVisualsData.AgentVisuals;
			this.ActionSetData = agentVisualsData.ActionSetData;
			this.FrameData = agentVisualsData.FrameData;
			this.BodyPropertiesData = agentVisualsData.BodyPropertiesData;
			this.EquipmentData = agentVisualsData.EquipmentData;
			this.RightWieldedItemIndexData = agentVisualsData.RightWieldedItemIndexData;
			this.LeftWieldedItemIndexData = agentVisualsData.LeftWieldedItemIndexData;
			this.SkeletonTypeData = agentVisualsData.SkeletonTypeData;
			this.BannerData = agentVisualsData.BannerData;
			this.CachedWeaponSlot0Entity = agentVisualsData.CachedWeaponSlot0Entity;
			this.CachedWeaponSlot1Entity = agentVisualsData.CachedWeaponSlot1Entity;
			this.CachedWeaponSlot2Entity = agentVisualsData.CachedWeaponSlot2Entity;
			this.CachedWeaponSlot3Entity = agentVisualsData.CachedWeaponSlot3Entity;
			this.CachedWeaponSlot4Entity = agentVisualsData.CachedWeaponSlot4Entity;
			this.SceneData = agentVisualsData.SceneData;
			this.MonsterData = agentVisualsData.MonsterData;
			this.PrepareImmediatelyData = agentVisualsData.PrepareImmediatelyData;
			this.UseScaledWeaponsData = agentVisualsData.UseScaledWeaponsData;
			this.UseTranslucencyData = agentVisualsData.UseTranslucencyData;
			this.UseTesselationData = agentVisualsData.UseTesselationData;
			this.UseMorphAnimsData = agentVisualsData.UseMorphAnimsData;
			this.ClothColor1Data = agentVisualsData.ClothColor1Data;
			this.ClothColor2Data = agentVisualsData.ClothColor2Data;
			this.ScaleData = agentVisualsData.ScaleData;
			this.ActionCodeData = agentVisualsData.ActionCodeData;
			this.EntityData = agentVisualsData.EntityData;
			this.CharacterObjectStringIdData = agentVisualsData.CharacterObjectStringIdData;
			this.HasClippingPlaneData = agentVisualsData.HasClippingPlaneData;
			this.MountCreationKeyData = agentVisualsData.MountCreationKeyData;
			this.AddColorRandomnessData = agentVisualsData.AddColorRandomnessData;
			this.RaceData = agentVisualsData.RaceData;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0009AB1E File Offset: 0x00098D1E
		public AgentVisualsData()
		{
			this.ClothColor1Data = uint.MaxValue;
			this.ClothColor2Data = uint.MaxValue;
			this.RightWieldedItemIndexData = -1;
			this.LeftWieldedItemIndexData = -1;
			this.ScaleData = 0f;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0009AB58 File Offset: 0x00098D58
		public AgentVisualsData Equipment(Equipment equipment)
		{
			this.EquipmentData = equipment;
			return this;
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0009AB62 File Offset: 0x00098D62
		public AgentVisualsData BodyProperties(BodyProperties bodyProperties)
		{
			this.BodyPropertiesData = bodyProperties;
			return this;
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0009AB6C File Offset: 0x00098D6C
		public AgentVisualsData Frame(MatrixFrame frame)
		{
			this.FrameData = frame;
			return this;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0009AB76 File Offset: 0x00098D76
		public AgentVisualsData ActionSet(MBActionSet actionSet)
		{
			this.ActionSetData = actionSet;
			return this;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0009AB80 File Offset: 0x00098D80
		public AgentVisualsData Scene(Scene scene)
		{
			this.SceneData = scene;
			return this;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x0009AB8A File Offset: 0x00098D8A
		public AgentVisualsData Monster(Monster monster)
		{
			this.MonsterData = monster;
			return this;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x0009AB94 File Offset: 0x00098D94
		public AgentVisualsData PrepareImmediately(bool prepareImmediately)
		{
			this.PrepareImmediatelyData = prepareImmediately;
			return this;
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x0009AB9E File Offset: 0x00098D9E
		public AgentVisualsData UseScaledWeapons(bool useScaledWeapons)
		{
			this.UseScaledWeaponsData = useScaledWeapons;
			return this;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x0009ABA8 File Offset: 0x00098DA8
		public AgentVisualsData SkeletonType(SkeletonType skeletonType)
		{
			this.SkeletonTypeData = skeletonType;
			return this;
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0009ABB2 File Offset: 0x00098DB2
		public AgentVisualsData UseMorphAnims(bool useMorphAnims)
		{
			this.UseMorphAnimsData = useMorphAnims;
			return this;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0009ABBC File Offset: 0x00098DBC
		public AgentVisualsData ClothColor1(uint clothColor1)
		{
			this.ClothColor1Data = clothColor1;
			return this;
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x0009ABC6 File Offset: 0x00098DC6
		public AgentVisualsData ClothColor2(uint clothColor2)
		{
			this.ClothColor2Data = clothColor2;
			return this;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x0009ABD0 File Offset: 0x00098DD0
		public AgentVisualsData Banner(Banner banner)
		{
			this.BannerData = banner;
			return this;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0009ABDA File Offset: 0x00098DDA
		public AgentVisualsData Race(int race)
		{
			this.RaceData = race;
			return this;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x0009ABE4 File Offset: 0x00098DE4
		public GameEntity GetCachedWeaponEntity(EquipmentIndex slotIndex)
		{
			switch (slotIndex)
			{
			case EquipmentIndex.WeaponItemBeginSlot:
				return this.CachedWeaponSlot0Entity;
			case EquipmentIndex.Weapon1:
				return this.CachedWeaponSlot1Entity;
			case EquipmentIndex.Weapon2:
				return this.CachedWeaponSlot2Entity;
			case EquipmentIndex.Weapon3:
				return this.CachedWeaponSlot3Entity;
			case EquipmentIndex.ExtraWeaponSlot:
				return this.CachedWeaponSlot4Entity;
			default:
				return null;
			}
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0009AC34 File Offset: 0x00098E34
		public AgentVisualsData CachedWeaponEntity(EquipmentIndex slotIndex, GameEntity cachedWeaponEntity)
		{
			switch (slotIndex)
			{
			case EquipmentIndex.WeaponItemBeginSlot:
				this.CachedWeaponSlot0Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon1:
				this.CachedWeaponSlot1Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon2:
				this.CachedWeaponSlot2Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon3:
				this.CachedWeaponSlot3Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.ExtraWeaponSlot:
				this.CachedWeaponSlot4Entity = cachedWeaponEntity;
				break;
			}
			return this;
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x0009AC89 File Offset: 0x00098E89
		public AgentVisualsData Entity(GameEntity entity)
		{
			this.EntityData = entity;
			return this;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x0009AC93 File Offset: 0x00098E93
		public AgentVisualsData UseTranslucency(bool useTranslucency)
		{
			this.UseTranslucencyData = useTranslucency;
			return this;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0009AC9D File Offset: 0x00098E9D
		public AgentVisualsData UseTesselation(bool useTesselation)
		{
			this.UseTesselationData = useTesselation;
			return this;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0009ACA7 File Offset: 0x00098EA7
		public AgentVisualsData ActionCode(in ActionIndexCache actionCode)
		{
			this.ActionCodeData = actionCode;
			return this;
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0009ACB6 File Offset: 0x00098EB6
		public AgentVisualsData RightWieldedItemIndex(int rightWieldedItemIndex)
		{
			this.RightWieldedItemIndexData = rightWieldedItemIndex;
			return this;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x0009ACC0 File Offset: 0x00098EC0
		public AgentVisualsData LeftWieldedItemIndex(int leftWieldedItemIndex)
		{
			this.LeftWieldedItemIndexData = leftWieldedItemIndex;
			return this;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x0009ACCA File Offset: 0x00098ECA
		public AgentVisualsData Scale(float scale)
		{
			this.ScaleData = scale;
			return this;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0009ACD4 File Offset: 0x00098ED4
		public AgentVisualsData CharacterObjectStringId(string characterObjectStringId)
		{
			this.CharacterObjectStringIdData = characterObjectStringId;
			return this;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x0009ACDE File Offset: 0x00098EDE
		public AgentVisualsData HasClippingPlane(bool hasClippingPlane)
		{
			this.HasClippingPlaneData = hasClippingPlane;
			return this;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x0009ACE8 File Offset: 0x00098EE8
		public AgentVisualsData MountCreationKey(string mountCreationKey)
		{
			this.MountCreationKeyData = mountCreationKey;
			return this;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x0009ACF2 File Offset: 0x00098EF2
		public AgentVisualsData AddColorRandomness(bool addColorRandomness)
		{
			this.AddColorRandomnessData = addColorRandomness;
			return this;
		}

		// Token: 0x04000FA9 RID: 4009
		public MBAgentVisuals AgentVisuals;
	}
}
