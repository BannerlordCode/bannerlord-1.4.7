using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000307 RID: 775
	public class MultiplayerClassDivisions
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000A9A0B File Offset: 0x000A7C0B
		// (set) Token: 0x06002C3E RID: 11326 RVA: 0x000A9A12 File Offset: 0x000A7C12
		public static List<MultiplayerClassDivisions.MPHeroClassGroup> MultiplayerHeroClassGroups { get; private set; }

		// Token: 0x06002C3F RID: 11327 RVA: 0x000A9A1C File Offset: 0x000A7C1C
		public static IEnumerable<MultiplayerClassDivisions.MPHeroClass> GetMPHeroClasses(BasicCultureObject culture)
		{
			return from x in MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>()
				where x.Culture == culture
				select x;
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000A9A51 File Offset: 0x000A7C51
		public static MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> GetMPHeroClasses()
		{
			return MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000A9A60 File Offset: 0x000A7C60
		public static MultiplayerClassDivisions.MPHeroClass GetMPHeroClassForCharacter(BasicCharacterObject character)
		{
			return MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>().FirstOrDefault<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass x) => x.HeroCharacter == character || x.TroopCharacter == character);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000A9A98 File Offset: 0x000A7C98
		public static List<List<IReadOnlyPerkObject>> GetAllPerksForHeroClass(MultiplayerClassDivisions.MPHeroClass heroClass, string forcedForGameMode = null)
		{
			List<List<IReadOnlyPerkObject>> list = new List<List<IReadOnlyPerkObject>>();
			for (int i = 0; i < 3; i++)
			{
				list.Add(heroClass.GetAllAvailablePerksForListIndex(i, forcedForGameMode).ToList<IReadOnlyPerkObject>());
			}
			return list;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x000A9ACC File Offset: 0x000A7CCC
		public static MultiplayerClassDivisions.MPHeroClass GetMPHeroClassForPeer(MissionPeer peer, bool skipTeamCheck = false)
		{
			Team team = peer.Team;
			if ((!skipTeamCheck && (team == null || team.Side == BattleSideEnum.None)) || (peer.SelectedTroopIndex < 0 && peer.ControlledAgent == null))
			{
				return null;
			}
			if (peer.ControlledAgent != null)
			{
				return MultiplayerClassDivisions.GetMPHeroClassForCharacter(peer.ControlledAgent.Character);
			}
			if (peer.SelectedTroopIndex >= 0)
			{
				return MultiplayerClassDivisions.GetMPHeroClasses(peer.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>()[peer.SelectedTroopIndex];
			}
			Debug.FailedAssert("This should not be seen.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerClassDivisions.cs", "GetMPHeroClassForPeer", 255);
			return null;
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000A9B5C File Offset: 0x000A7D5C
		public static TargetIconType GetMPHeroClassForFormation(Formation formation)
		{
			switch (formation.PhysicalClass)
			{
			case FormationClass.Infantry:
				return TargetIconType.Infantry_Light;
			case FormationClass.Ranged:
				return TargetIconType.Archer_Light;
			case FormationClass.Cavalry:
				return TargetIconType.Cavalry_Light;
			default:
				return TargetIconType.HorseArcher_Light;
			}
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000A9B8C File Offset: 0x000A7D8C
		public static List<List<IReadOnlyPerkObject>> GetAvailablePerksForPeer(MissionPeer missionPeer)
		{
			if (((missionPeer != null) ? missionPeer.Team : null) != null)
			{
				return MultiplayerClassDivisions.GetAllPerksForHeroClass(MultiplayerClassDivisions.GetMPHeroClassForPeer(missionPeer, false), null);
			}
			return new List<List<IReadOnlyPerkObject>>();
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000A9BB0 File Offset: 0x000A7DB0
		public static void Initialize()
		{
			MultiplayerClassDivisions.MultiplayerHeroClassGroups = new List<MultiplayerClassDivisions.MPHeroClassGroup>
			{
				new MultiplayerClassDivisions.MPHeroClassGroup("Infantry"),
				new MultiplayerClassDivisions.MPHeroClassGroup("Ranged"),
				new MultiplayerClassDivisions.MPHeroClassGroup("Cavalry"),
				new MultiplayerClassDivisions.MPHeroClassGroup("HorseArcher")
			};
			MultiplayerClassDivisions.AvailableCultures = from x in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>().ToArray()
				where x.IsMainCulture
				select x;
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000A9C3F File Offset: 0x000A7E3F
		public static void Release()
		{
			MultiplayerClassDivisions.MultiplayerHeroClassGroups.Clear();
			MultiplayerClassDivisions.AvailableCultures = null;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000A9C51 File Offset: 0x000A7E51
		private static BasicCharacterObject GetMPCharacter(string stringId)
		{
			return MBObjectManager.Instance.GetObject<BasicCharacterObject>(stringId);
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000A9C60 File Offset: 0x000A7E60
		public static int GetMinimumTroopCost(BasicCultureObject culture = null)
		{
			MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses();
			if (culture != null)
			{
				return mpheroClasses.Where<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass c) => c.Culture == culture).Min<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass troop) => troop.TroopCost);
			}
			return mpheroClasses.Min<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass troop) => troop.TroopCost);
		}

		// Token: 0x04001180 RID: 4480
		public static IEnumerable<BasicCultureObject> AvailableCultures;

		// Token: 0x020005E3 RID: 1507
		public class MPHeroClass : MBObjectBase
		{
			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x06003EBE RID: 16062 RVA: 0x000F5BB3 File Offset: 0x000F3DB3
			// (set) Token: 0x06003EBF RID: 16063 RVA: 0x000F5BBB File Offset: 0x000F3DBB
			public BasicCharacterObject HeroCharacter { get; private set; }

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x000F5BC4 File Offset: 0x000F3DC4
			// (set) Token: 0x06003EC1 RID: 16065 RVA: 0x000F5BCC File Offset: 0x000F3DCC
			public BasicCharacterObject TroopCharacter { get; private set; }

			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x06003EC2 RID: 16066 RVA: 0x000F5BD5 File Offset: 0x000F3DD5
			// (set) Token: 0x06003EC3 RID: 16067 RVA: 0x000F5BDD File Offset: 0x000F3DDD
			public BasicCharacterObject BannerBearerCharacter { get; private set; }

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x06003EC4 RID: 16068 RVA: 0x000F5BE6 File Offset: 0x000F3DE6
			// (set) Token: 0x06003EC5 RID: 16069 RVA: 0x000F5BEE File Offset: 0x000F3DEE
			public BasicCultureObject Culture { get; private set; }

			// Token: 0x17000A8C RID: 2700
			// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x000F5BF7 File Offset: 0x000F3DF7
			// (set) Token: 0x06003EC7 RID: 16071 RVA: 0x000F5BFF File Offset: 0x000F3DFF
			public MultiplayerClassDivisions.MPHeroClassGroup ClassGroup { get; private set; }

			// Token: 0x17000A8D RID: 2701
			// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x000F5C08 File Offset: 0x000F3E08
			// (set) Token: 0x06003EC9 RID: 16073 RVA: 0x000F5C10 File Offset: 0x000F3E10
			public string HeroIdleAnim { get; private set; }

			// Token: 0x17000A8E RID: 2702
			// (get) Token: 0x06003ECA RID: 16074 RVA: 0x000F5C19 File Offset: 0x000F3E19
			// (set) Token: 0x06003ECB RID: 16075 RVA: 0x000F5C21 File Offset: 0x000F3E21
			public string HeroMountIdleAnim { get; private set; }

			// Token: 0x17000A8F RID: 2703
			// (get) Token: 0x06003ECC RID: 16076 RVA: 0x000F5C2A File Offset: 0x000F3E2A
			// (set) Token: 0x06003ECD RID: 16077 RVA: 0x000F5C32 File Offset: 0x000F3E32
			public string TroopIdleAnim { get; private set; }

			// Token: 0x17000A90 RID: 2704
			// (get) Token: 0x06003ECE RID: 16078 RVA: 0x000F5C3B File Offset: 0x000F3E3B
			// (set) Token: 0x06003ECF RID: 16079 RVA: 0x000F5C43 File Offset: 0x000F3E43
			public string TroopMountIdleAnim { get; private set; }

			// Token: 0x17000A91 RID: 2705
			// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x000F5C4C File Offset: 0x000F3E4C
			// (set) Token: 0x06003ED1 RID: 16081 RVA: 0x000F5C54 File Offset: 0x000F3E54
			public int ArmorValue { get; private set; }

			// Token: 0x17000A92 RID: 2706
			// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x000F5C5D File Offset: 0x000F3E5D
			// (set) Token: 0x06003ED3 RID: 16083 RVA: 0x000F5C65 File Offset: 0x000F3E65
			public int Health { get; private set; }

			// Token: 0x17000A93 RID: 2707
			// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x000F5C6E File Offset: 0x000F3E6E
			// (set) Token: 0x06003ED5 RID: 16085 RVA: 0x000F5C76 File Offset: 0x000F3E76
			public float HeroMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000A94 RID: 2708
			// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x000F5C7F File Offset: 0x000F3E7F
			// (set) Token: 0x06003ED7 RID: 16087 RVA: 0x000F5C87 File Offset: 0x000F3E87
			public float HeroCombatMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000A95 RID: 2709
			// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x000F5C90 File Offset: 0x000F3E90
			// (set) Token: 0x06003ED9 RID: 16089 RVA: 0x000F5C98 File Offset: 0x000F3E98
			public float HeroTopSpeedReachDuration { get; private set; }

			// Token: 0x17000A96 RID: 2710
			// (get) Token: 0x06003EDA RID: 16090 RVA: 0x000F5CA1 File Offset: 0x000F3EA1
			// (set) Token: 0x06003EDB RID: 16091 RVA: 0x000F5CA9 File Offset: 0x000F3EA9
			public float TroopMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000A97 RID: 2711
			// (get) Token: 0x06003EDC RID: 16092 RVA: 0x000F5CB2 File Offset: 0x000F3EB2
			// (set) Token: 0x06003EDD RID: 16093 RVA: 0x000F5CBA File Offset: 0x000F3EBA
			public float TroopCombatMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000A98 RID: 2712
			// (get) Token: 0x06003EDE RID: 16094 RVA: 0x000F5CC3 File Offset: 0x000F3EC3
			// (set) Token: 0x06003EDF RID: 16095 RVA: 0x000F5CCB File Offset: 0x000F3ECB
			public float TroopTopSpeedReachDuration { get; private set; }

			// Token: 0x17000A99 RID: 2713
			// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x000F5CD4 File Offset: 0x000F3ED4
			// (set) Token: 0x06003EE1 RID: 16097 RVA: 0x000F5CDC File Offset: 0x000F3EDC
			public float TroopMultiplier { get; private set; }

			// Token: 0x17000A9A RID: 2714
			// (get) Token: 0x06003EE2 RID: 16098 RVA: 0x000F5CE5 File Offset: 0x000F3EE5
			// (set) Token: 0x06003EE3 RID: 16099 RVA: 0x000F5CED File Offset: 0x000F3EED
			public int TroopCost { get; private set; }

			// Token: 0x17000A9B RID: 2715
			// (get) Token: 0x06003EE4 RID: 16100 RVA: 0x000F5CF6 File Offset: 0x000F3EF6
			// (set) Token: 0x06003EE5 RID: 16101 RVA: 0x000F5CFE File Offset: 0x000F3EFE
			public int TroopCasualCost { get; private set; }

			// Token: 0x17000A9C RID: 2716
			// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x000F5D07 File Offset: 0x000F3F07
			// (set) Token: 0x06003EE7 RID: 16103 RVA: 0x000F5D0F File Offset: 0x000F3F0F
			public int TroopBattleCost { get; private set; }

			// Token: 0x17000A9D RID: 2717
			// (get) Token: 0x06003EE8 RID: 16104 RVA: 0x000F5D18 File Offset: 0x000F3F18
			// (set) Token: 0x06003EE9 RID: 16105 RVA: 0x000F5D20 File Offset: 0x000F3F20
			public int MeleeAI { get; private set; }

			// Token: 0x17000A9E RID: 2718
			// (get) Token: 0x06003EEA RID: 16106 RVA: 0x000F5D29 File Offset: 0x000F3F29
			// (set) Token: 0x06003EEB RID: 16107 RVA: 0x000F5D31 File Offset: 0x000F3F31
			public int RangedAI { get; private set; }

			// Token: 0x17000A9F RID: 2719
			// (get) Token: 0x06003EEC RID: 16108 RVA: 0x000F5D3A File Offset: 0x000F3F3A
			// (set) Token: 0x06003EED RID: 16109 RVA: 0x000F5D42 File Offset: 0x000F3F42
			public TextObject HeroInformation { get; private set; }

			// Token: 0x17000AA0 RID: 2720
			// (get) Token: 0x06003EEE RID: 16110 RVA: 0x000F5D4B File Offset: 0x000F3F4B
			// (set) Token: 0x06003EEF RID: 16111 RVA: 0x000F5D53 File Offset: 0x000F3F53
			public TextObject TroopInformation { get; private set; }

			// Token: 0x17000AA1 RID: 2721
			// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x000F5D5C File Offset: 0x000F3F5C
			// (set) Token: 0x06003EF1 RID: 16113 RVA: 0x000F5D64 File Offset: 0x000F3F64
			public TargetIconType IconType { get; private set; }

			// Token: 0x17000AA2 RID: 2722
			// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x000F5D6D File Offset: 0x000F3F6D
			public TextObject HeroName
			{
				get
				{
					return this.HeroCharacter.Name;
				}
			}

			// Token: 0x17000AA3 RID: 2723
			// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x000F5D7A File Offset: 0x000F3F7A
			public TextObject TroopName
			{
				get
				{
					return this.TroopCharacter.Name;
				}
			}

			// Token: 0x06003EF4 RID: 16116 RVA: 0x000F5D87 File Offset: 0x000F3F87
			public override bool Equals(object obj)
			{
				return obj is MultiplayerClassDivisions.MPHeroClass && ((MultiplayerClassDivisions.MPHeroClass)obj).StringId.Equals(base.StringId);
			}

			// Token: 0x06003EF5 RID: 16117 RVA: 0x000F5DA9 File Offset: 0x000F3FA9
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06003EF6 RID: 16118 RVA: 0x000F5DB4 File Offset: 0x000F3FB4
			public List<IReadOnlyPerkObject> GetAllAvailablePerksForListIndex(int index, string forcedForGameMode = null)
			{
				string text = forcedForGameMode ?? MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				List<IReadOnlyPerkObject> list = new List<IReadOnlyPerkObject>();
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					foreach (string text2 in readOnlyPerkObject.GameModes)
					{
						if ((text2.Equals(text, StringComparison.InvariantCultureIgnoreCase) || text2.Equals("all", StringComparison.InvariantCultureIgnoreCase)) && readOnlyPerkObject.PerkListIndex == index)
						{
							list.Add(readOnlyPerkObject);
							break;
						}
					}
				}
				return list;
			}

			// Token: 0x06003EF7 RID: 16119 RVA: 0x000F5E80 File Offset: 0x000F4080
			public override void Deserialize(MBObjectManager objectManager, XmlNode node)
			{
				base.Deserialize(objectManager, node);
				this.HeroCharacter = MultiplayerClassDivisions.GetMPCharacter(node.Attributes["hero"].Value);
				this.TroopCharacter = MultiplayerClassDivisions.GetMPCharacter(node.Attributes["troop"].Value);
				XmlAttribute xmlAttribute = node.Attributes["banner_bearer"];
				string text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				if (text != null)
				{
					this.BannerBearerCharacter = MultiplayerClassDivisions.GetMPCharacter(text);
				}
				XmlAttribute xmlAttribute2 = node.Attributes["hero_idle_anim"];
				this.HeroIdleAnim = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				XmlAttribute xmlAttribute3 = node.Attributes["hero_mount_idle_anim"];
				this.HeroMountIdleAnim = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				XmlAttribute xmlAttribute4 = node.Attributes["troop_idle_anim"];
				this.TroopIdleAnim = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
				XmlAttribute xmlAttribute5 = node.Attributes["troop_mount_idle_anim"];
				this.TroopMountIdleAnim = ((xmlAttribute5 != null) ? xmlAttribute5.Value : null);
				this.Culture = this.HeroCharacter.Culture;
				this.ClassGroup = new MultiplayerClassDivisions.MPHeroClassGroup(this.HeroCharacter.DefaultFormationClass.GetName());
				this.TroopMultiplier = (float)Convert.ToDouble(node.Attributes["multiplier"].Value);
				this.TroopCost = Convert.ToInt32(node.Attributes["cost"].Value);
				this.ArmorValue = Convert.ToInt32(node.Attributes["armor"].Value);
				XmlAttribute xmlAttribute6 = node.Attributes["casual_cost"];
				XmlAttribute xmlAttribute7 = node.Attributes["battle_cost"];
				this.TroopCasualCost = ((xmlAttribute6 != null) ? Convert.ToInt32(node.Attributes["casual_cost"].Value) : this.TroopCost);
				this.TroopBattleCost = ((xmlAttribute7 != null) ? Convert.ToInt32(node.Attributes["battle_cost"].Value) : this.TroopCost);
				this.Health = 100;
				this.MeleeAI = 50;
				this.RangedAI = 50;
				XmlNode xmlNode = node.Attributes["hitpoints"];
				if (xmlNode != null)
				{
					this.Health = Convert.ToInt32(xmlNode.Value);
				}
				this.HeroMovementSpeedMultiplier = (float)Convert.ToDouble(node.Attributes["movement_speed"].Value);
				this.HeroCombatMovementSpeedMultiplier = (float)Convert.ToDouble(node.Attributes["combat_movement_speed"].Value);
				this.HeroTopSpeedReachDuration = (float)Convert.ToDouble(node.Attributes["acceleration"].Value);
				XmlAttribute xmlAttribute8 = node.Attributes["troop_movement_speed"];
				XmlAttribute xmlAttribute9 = node.Attributes["troop_combat_movement_speed"];
				XmlAttribute xmlAttribute10 = node.Attributes["troop_acceleration"];
				this.TroopMovementSpeedMultiplier = ((xmlAttribute8 != null) ? ((float)Convert.ToDouble(xmlAttribute8.Value)) : this.HeroMovementSpeedMultiplier);
				this.TroopCombatMovementSpeedMultiplier = ((xmlAttribute9 != null) ? ((float)Convert.ToDouble(xmlAttribute9.Value)) : this.HeroCombatMovementSpeedMultiplier);
				this.TroopTopSpeedReachDuration = ((xmlAttribute10 != null) ? ((float)Convert.ToDouble(xmlAttribute10.Value)) : this.HeroTopSpeedReachDuration);
				this.MeleeAI = Convert.ToInt32(node.Attributes["melee_ai"].Value);
				this.RangedAI = Convert.ToInt32(node.Attributes["ranged_ai"].Value);
				TargetIconType targetIconType;
				if (Enum.TryParse<TargetIconType>(node.Attributes["icon"].Value, true, out targetIconType))
				{
					this.IconType = targetIconType;
				}
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.Name == "Perks")
					{
						this._perks = new List<IReadOnlyPerkObject>();
						foreach (object obj2 in xmlNode2.ChildNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment)
							{
								this._perks.Add(MPPerkObject.Deserialize(xmlNode3));
							}
						}
					}
				}
			}

			// Token: 0x06003EF8 RID: 16120 RVA: 0x000F630C File Offset: 0x000F450C
			public bool IsTroopCharacter(BasicCharacterObject character)
			{
				return this.TroopCharacter == character;
			}

			// Token: 0x04001FB5 RID: 8117
			private List<IReadOnlyPerkObject> _perks = new List<IReadOnlyPerkObject>();
		}

		// Token: 0x020005E4 RID: 1508
		public class MPHeroClassGroup
		{
			// Token: 0x06003EFA RID: 16122 RVA: 0x000F632A File Offset: 0x000F452A
			public MPHeroClassGroup(string stringId)
			{
				this.StringId = stringId;
				this.Name = GameTexts.FindText("str_troop_type_name", this.StringId);
			}

			// Token: 0x06003EFB RID: 16123 RVA: 0x000F634F File Offset: 0x000F454F
			public override bool Equals(object obj)
			{
				return obj is MultiplayerClassDivisions.MPHeroClassGroup && ((MultiplayerClassDivisions.MPHeroClassGroup)obj).StringId.Equals(this.StringId);
			}

			// Token: 0x06003EFC RID: 16124 RVA: 0x000F6371 File Offset: 0x000F4571
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x04001FB6 RID: 8118
			public readonly string StringId;

			// Token: 0x04001FB7 RID: 8119
			public readonly TextObject Name;
		}
	}
}
