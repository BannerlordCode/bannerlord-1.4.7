using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007C RID: 124
	public class CharacterData
	{
		// Token: 0x0600106E RID: 4206 RVA: 0x0004E5F9 File Offset: 0x0004C7F9
		private CharacterData()
		{
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0004E604 File Offset: 0x0004C804
		private static CharacterData CreateFrom(Hero hero)
		{
			CharacterData characterData = new CharacterData();
			characterData.Name = hero.Name.ToString();
			characterData.Age = hero.Age;
			characterData.Culture = hero.Culture.StringId;
			characterData.Gold = hero.Gold;
			characterData.Race = hero.CharacterObject.Race;
			characterData.Level = hero.Level;
			characterData.IsFemale = hero.IsFemale;
			characterData.Weight = hero.Weight;
			characterData.Build = hero.Build;
			characterData.CivilianEquipmentCode = hero.CivilianEquipment.CalculateEquipmentCode();
			characterData.StealthEquipmentCode = hero.StealthEquipment.CalculateEquipmentCode();
			characterData.BattleEquipmentCode = hero.BattleEquipment.CalculateEquipmentCode();
			characterData.BodyPropertyKeys = new ulong[]
			{
				hero.StaticBodyProperties.KeyPart1,
				hero.StaticBodyProperties.KeyPart2,
				hero.StaticBodyProperties.KeyPart3,
				hero.StaticBodyProperties.KeyPart4,
				hero.StaticBodyProperties.KeyPart5,
				hero.StaticBodyProperties.KeyPart6,
				hero.StaticBodyProperties.KeyPart7,
				hero.StaticBodyProperties.KeyPart8
			};
			characterData.UnspentAttributePoints = hero.HeroDeveloper.UnspentAttributePoints;
			characterData.UnspentFocusPoints = hero.HeroDeveloper.UnspentFocusPoints;
			characterData.SkillsArray = new CharacterData.SkillObjectData[Skills.All.Count];
			characterData.AttributesArray = new CharacterData.PropertyObjectData[Attributes.All.Count];
			characterData.Traits = new CharacterData.PropertyObjectData[TraitObject.All.Count];
			for (int i = 0; i < Skills.All.Count; i++)
			{
				characterData.SkillsArray[i] = new CharacterData.SkillObjectData(Skills.All[i].StringId, hero.GetSkillValue(Skills.All[i]), hero.HeroDeveloper.GetSkillXpProgress(Skills.All[i]), hero.HeroDeveloper.GetFocus(Skills.All[i]));
			}
			List<string> list = new List<string>();
			for (int j = 0; j < PerkObject.All.Count; j++)
			{
				if (hero.GetPerkValue(PerkObject.All[j]))
				{
					list.Add(PerkObject.All[j].StringId);
				}
			}
			characterData.UnlockedPerks = list.ToArray();
			for (int k = 0; k < Attributes.All.Count; k++)
			{
				characterData.AttributesArray[k] = new CharacterData.PropertyObjectData(Attributes.All[k].StringId, hero.GetAttributeValue(Attributes.All[k]));
			}
			for (int l = 0; l < TraitObject.All.Count; l++)
			{
				characterData.Traits[l] = new CharacterData.PropertyObjectData(TraitObject.All[l].StringId, hero.GetTraitLevel(TraitObject.All[l]));
			}
			return characterData;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0004E914 File Offset: 0x0004CB14
		private static void InitializeHeroFromCharacterData(Hero target, CharacterData characterData)
		{
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(characterData.Name, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(characterData.Name, null));
			target.Gold = characterData.Gold;
			target.IsFemale = characterData.IsFemale;
			target.CharacterObject.Race = characterData.Race;
			float num = characterData.Age;
			if (num < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				num = (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
			}
			target.SetBirthDay(CampaignTime.YearsFromNow(-num));
			target.Weight = characterData.Weight;
			target.Build = characterData.Build;
			target.Level = characterData.Level;
			Equipment equipment = Equipment.CreateFromEquipmentCode(characterData.BattleEquipmentCode);
			Equipment equipment2 = Equipment.CreateFromEquipmentCode(characterData.CivilianEquipmentCode);
			Equipment equipment3 = null;
			if (!string.IsNullOrEmpty(characterData.StealthEquipmentCode))
			{
				equipment3 = Equipment.CreateFromEquipmentCode(characterData.StealthEquipmentCode);
			}
			for (int i = 0; i < 12; i++)
			{
				if (target.PartyBelongedTo != null)
				{
					if (!target.BattleEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.BattleEquipment[i], 1);
					}
					if (!target.CivilianEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.CivilianEquipment[i], 1);
					}
					if (!target.StealthEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.StealthEquipment[i], 1);
					}
				}
				target.BattleEquipment[i] = equipment[i];
				target.CivilianEquipment[i] = equipment2[i];
				if (equipment3 != null)
				{
					target.StealthEquipment[i] = equipment3[i];
				}
			}
			CultureObject @object = MBObjectManager.Instance.GetObject<CultureObject>(characterData.Culture);
			if (@object != null)
			{
				target.Culture = @object;
			}
			ulong[] bodyPropertyKeys = characterData.BodyPropertyKeys;
			target.StaticBodyProperties = new StaticBodyProperties(bodyPropertyKeys[0], bodyPropertyKeys[1], bodyPropertyKeys[2], bodyPropertyKeys[3], bodyPropertyKeys[4], bodyPropertyKeys[5], bodyPropertyKeys[6], bodyPropertyKeys[7]);
			target.HeroDeveloper.UnspentFocusPoints = characterData.UnspentFocusPoints;
			target.HeroDeveloper.UnspentAttributePoints = characterData.UnspentAttributePoints;
			for (int j = 0; j < characterData.SkillsArray.Length; j++)
			{
				CharacterData.SkillObjectData skillObjectData = characterData.SkillsArray[j];
				string stringId = skillObjectData.StringId;
				int num2 = skillObjectData.Value;
				int num3 = skillObjectData.Focus;
				int progress = skillObjectData.Progress;
				SkillObject object2 = MBObjectManager.Instance.GetObject<SkillObject>(stringId);
				if (object2 != null)
				{
					int focus = target.HeroDeveloper.GetFocus(object2);
					num2 = Math.Max(0, num2);
					int xpRequiredForSkillLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(num2);
					int xpRequiredForSkillLevel2 = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(num2 + 1);
					int num4 = Math.Min(progress + xpRequiredForSkillLevel, xpRequiredForSkillLevel2);
					target.HeroDeveloper.SetSkillXp(object2, (float)num4);
					target.SetSkillValue(object2, num2);
					num3 = Math.Max(Math.Min(num3, Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill), 0);
					if (focus < num3)
					{
						target.HeroDeveloper.AddFocus(object2, num3 - focus, false);
					}
					else
					{
						target.HeroDeveloper.RemoveFocus(object2, focus - num3);
					}
				}
			}
			for (int k = 0; k < characterData.Traits.Length; k++)
			{
				CharacterData.PropertyObjectData propertyObjectData = characterData.Traits[k];
				string stringId2 = propertyObjectData.StringId;
				int num5 = propertyObjectData.Value;
				TraitObject object3 = MBObjectManager.Instance.GetObject<TraitObject>(stringId2);
				if (object3 != null)
				{
					num5 = Math.Max(Math.Min(num5, object3.MaxValue), object3.MinValue);
					target.SetTraitLevel(object3, num5);
				}
			}
			for (int l = 0; l < characterData.AttributesArray.Length; l++)
			{
				CharacterData.PropertyObjectData propertyObjectData2 = characterData.AttributesArray[l];
				string stringId3 = propertyObjectData2.StringId;
				int value = propertyObjectData2.Value;
				CharacterAttribute object4 = MBObjectManager.Instance.GetObject<CharacterAttribute>(stringId3);
				if (object4 != null)
				{
					int num6 = ((target.GetAttributeValue(object4) > value) ? (value - target.GetAttributeValue(object4)) : (value - target.GetAttributeValue(object4)));
					target.HeroDeveloper.AddAttribute(object4, num6, false);
				}
			}
			target.ClearPerks();
			for (int m = 0; m < characterData.UnlockedPerks.Length; m++)
			{
				string text = characterData.UnlockedPerks[m];
				PerkObject object5 = MBObjectManager.Instance.GetObject<PerkObject>(text);
				if (object5 != null)
				{
					target.HeroDeveloper.AddPerk(object5);
				}
			}
			target.HeroDeveloper.SetInitialLevel(target.Level);
			target.SetName(textObject2, textObject);
			Hero.SetHeroEncyclopediaTextAndLinks(target);
			if (GameStateManager.Current.ActiveState is MapState && target.PartyBelongedTo != null)
			{
				target.PartyBelongedTo.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0004EE38 File Offset: 0x0004D038
		public static void ExportCharacter(Hero hero, string path)
		{
			CharacterData characterData = CharacterData.CreateFrom(hero);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterData));
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				xmlSerializer.Serialize(streamWriter, characterData);
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0004EE88 File Offset: 0x0004D088
		public static void ImportCharacter(Hero hero, string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterData));
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				CharacterData characterData = (CharacterData)xmlSerializer.Deserialize(fileStream);
				CharacterData.InitializeHeroFromCharacterData(Hero.MainHero, characterData);
			}
		}

		// Token: 0x040004A1 RID: 1185
		public const string CharacterDataExtension = "char";

		// Token: 0x040004A2 RID: 1186
		[XmlElement]
		public string Name;

		// Token: 0x040004A3 RID: 1187
		[XmlElement]
		public bool IsFemale;

		// Token: 0x040004A4 RID: 1188
		[XmlElement]
		public int Gold;

		// Token: 0x040004A5 RID: 1189
		[XmlElement]
		public int Race;

		// Token: 0x040004A6 RID: 1190
		[XmlElement]
		public int Level;

		// Token: 0x040004A7 RID: 1191
		[XmlElement]
		public string Culture;

		// Token: 0x040004A8 RID: 1192
		[XmlElement]
		public float Age;

		// Token: 0x040004A9 RID: 1193
		[XmlElement]
		public float Weight;

		// Token: 0x040004AA RID: 1194
		[XmlElement]
		public float Build;

		// Token: 0x040004AB RID: 1195
		[XmlElement]
		public string CivilianEquipmentCode;

		// Token: 0x040004AC RID: 1196
		[XmlElement]
		public string BattleEquipmentCode;

		// Token: 0x040004AD RID: 1197
		[XmlElement]
		public string StealthEquipmentCode;

		// Token: 0x040004AE RID: 1198
		[XmlArray("BodyPropertyKeys")]
		[XmlArrayItem("Key")]
		public ulong[] BodyPropertyKeys;

		// Token: 0x040004AF RID: 1199
		[XmlElement]
		public int UnspentFocusPoints;

		// Token: 0x040004B0 RID: 1200
		[XmlElement]
		public int UnspentAttributePoints;

		// Token: 0x040004B1 RID: 1201
		[XmlArray("Perks")]
		[XmlArrayItem("Perk")]
		public string[] UnlockedPerks;

		// Token: 0x040004B2 RID: 1202
		[XmlArray("Attributes")]
		[XmlArrayItem("Attribute")]
		public CharacterData.PropertyObjectData[] AttributesArray;

		// Token: 0x040004B3 RID: 1203
		[XmlArray("Traits")]
		[XmlArrayItem("Trait")]
		public CharacterData.PropertyObjectData[] Traits;

		// Token: 0x040004B4 RID: 1204
		[XmlArray("Skills")]
		[XmlArrayItem("Skill")]
		public CharacterData.SkillObjectData[] SkillsArray;

		// Token: 0x0200053E RID: 1342
		public class PropertyObjectData
		{
			// Token: 0x06004CFA RID: 19706 RVA: 0x0017F7DD File Offset: 0x0017D9DD
			public PropertyObjectData(string id, int value)
			{
				this.StringId = id;
				this.Value = value;
			}

			// Token: 0x06004CFB RID: 19707 RVA: 0x0017F7F3 File Offset: 0x0017D9F3
			public PropertyObjectData()
			{
			}

			// Token: 0x0400167D RID: 5757
			[XmlElement]
			public string StringId;

			// Token: 0x0400167E RID: 5758
			[XmlElement]
			public int Value;
		}

		// Token: 0x0200053F RID: 1343
		public class SkillObjectData : CharacterData.PropertyObjectData
		{
			// Token: 0x06004CFC RID: 19708 RVA: 0x0017F7FB File Offset: 0x0017D9FB
			public SkillObjectData(string id, int value, int progress, int focus)
				: base(id, value)
			{
				this.Focus = focus;
				this.Progress = progress;
			}

			// Token: 0x06004CFD RID: 19709 RVA: 0x0017F814 File Offset: 0x0017DA14
			public SkillObjectData()
			{
			}

			// Token: 0x0400167F RID: 5759
			[XmlElement]
			public int Focus;

			// Token: 0x04001680 RID: 5760
			[XmlElement]
			public int Progress;
		}
	}
}
