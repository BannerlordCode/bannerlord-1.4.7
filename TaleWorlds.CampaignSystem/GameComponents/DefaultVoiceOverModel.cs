using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000166 RID: 358
	public class DefaultVoiceOverModel : VoiceOverModel
	{
		// Token: 0x06001B15 RID: 6933 RVA: 0x0008C6F4 File Offset: 0x0008A8F4
		public override string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject)
		{
			if (voiceObject == null)
			{
				return "";
			}
			string text = "";
			string text2 = character.StringId + "_" + (CharacterObject.PlayerCharacter.IsFemale ? "female" : "male");
			foreach (string text3 in voiceObject.VoicePaths)
			{
				if (text3.Contains(text2))
				{
					text = text3;
					break;
				}
				if (text3.Contains(character.StringId + "_"))
				{
					text = text3;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				string accentClass = Campaign.Current.Models.VoiceOverModel.GetAccentClass(character.Culture, ConversationTagHelper.UsesHighRegister(character));
				Debug.Print("accentClass: " + accentClass, 0, Debug.DebugColor.White, 17592186044416UL);
				string text4 = (character.IsFemale ? "female" : "male");
				string stringId = character.GetPersona().StringId;
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", text4, "_", stringId, "_.+" }));
				list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", text4, "_generic_.+" }));
				this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
				if (list.IsEmpty<string>())
				{
					list2.Clear();
					list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", stringId, "_.+" }));
					list2.Add(".+\\\\" + accentClass + "_generic_.+");
					list2.Add(string.Concat(new string[] { ".+\\\\", text4, "_", stringId, "_.+" }));
					list2.Add(".+\\\\" + text4 + "_generic_.+");
					this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
					if (list.IsEmpty<string>())
					{
						list2.Clear();
						list2.Add(".+\\\\" + stringId + "_.+");
						list2.Add(".+\\\\generic_.+");
						list2.Add(".+" + accentClass + "_.+");
						this.CheckPossibleMatches(voiceObject, list2, ref list, true, character.IsFemale);
					}
				}
				if (!list.IsEmpty<string>())
				{
					if (character.IsHero)
					{
						text = list[character.HeroObject.RandomInt(list.Count)];
					}
					else if (MobileParty.ConversationParty != null)
					{
						text = list[MobileParty.ConversationParty.RandomInt(list.Count)];
					}
					else
					{
						text = list.GetRandomElement<string>();
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text, 0, Debug.DebugColor.White, 17592186044416UL);
			text = text.Replace("$PLATFORM", "PC");
			return text + ".ogg";
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0008CA54 File Offset: 0x0008AC54
		private void CheckPossibleMatches(VoiceObject voiceObject, List<string> possibleMatches, ref List<string> possibleVoicePaths, bool doubleCheckForGender = false, bool isFemale = false)
		{
			foreach (string text in possibleMatches)
			{
				Regex regex = new Regex(text, RegexOptions.IgnoreCase);
				foreach (string text2 in voiceObject.VoicePaths)
				{
					if (regex.Match(text2).Success && !possibleVoicePaths.Contains(text2))
					{
						if (doubleCheckForGender)
						{
							if (text2.Contains("_male") || text2.Contains("_female"))
							{
								string text3 = (isFemale ? "_female" : "_male");
								if (text2.Contains(text3))
								{
									possibleVoicePaths.Add(text2);
								}
							}
						}
						else
						{
							possibleVoicePaths.Add(text2);
						}
					}
				}
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0008CB48 File Offset: 0x0008AD48
		public override string GetAccentClass(CultureObject culture, bool isHighClass)
		{
			if (culture.StringId == "empire")
			{
				if (isHighClass)
				{
					return "imperial_high";
				}
				return "imperial_low";
			}
			else
			{
				if (culture.StringId == "vlandia")
				{
					return "vlandian";
				}
				if (culture.StringId == "sturgia")
				{
					return "sturgian";
				}
				if (culture.StringId == "khuzait")
				{
					return "khuzait";
				}
				if (culture.StringId == "aserai")
				{
					return "aserai";
				}
				if (culture.StringId == "battania")
				{
					return "battanian";
				}
				if (culture.StringId == "forest_bandits")
				{
					return "forest_bandits";
				}
				if (culture.StringId == "sea_raiders")
				{
					return "sea_raiders";
				}
				if (culture.StringId == "mountain_bandits")
				{
					return "mountain_bandits";
				}
				if (culture.StringId == "desert_bandits")
				{
					return "desert_bandits";
				}
				if (culture.StringId == "steppe_bandits")
				{
					return "steppe_bandits";
				}
				if (culture.StringId == "looters")
				{
					return "looters";
				}
				return "";
			}
		}

		// Token: 0x04000921 RID: 2337
		private const string ImperialHighClass = "imperial_high";

		// Token: 0x04000922 RID: 2338
		private const string ImperialLowClass = "imperial_low";

		// Token: 0x04000923 RID: 2339
		private const string VlandianClass = "vlandian";

		// Token: 0x04000924 RID: 2340
		private const string SturgianClass = "sturgian";

		// Token: 0x04000925 RID: 2341
		private const string KhuzaitClass = "khuzait";

		// Token: 0x04000926 RID: 2342
		private const string AseraiClass = "aserai";

		// Token: 0x04000927 RID: 2343
		private const string BattanianClass = "battanian";

		// Token: 0x04000928 RID: 2344
		private const string ForestBanditClass = "forest_bandits";

		// Token: 0x04000929 RID: 2345
		private const string SeaBanditClass = "sea_raiders";

		// Token: 0x0400092A RID: 2346
		private const string MountainBanditClass = "mountain_bandits";

		// Token: 0x0400092B RID: 2347
		private const string DesertBanditClass = "desert_bandits";

		// Token: 0x0400092C RID: 2348
		private const string SteppeBanditClass = "steppe_bandits";

		// Token: 0x0400092D RID: 2349
		private const string LootersClass = "looters";

		// Token: 0x0400092E RID: 2350
		private const string Male = "male";

		// Token: 0x0400092F RID: 2351
		private const string Female = "female";

		// Token: 0x04000930 RID: 2352
		private const string GenericPersonaId = "generic";
	}
}
