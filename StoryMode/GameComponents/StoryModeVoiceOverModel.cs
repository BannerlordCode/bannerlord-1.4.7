using System;
using System.Linq;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents
{
	// Token: 0x0200004C RID: 76
	public class StoryModeVoiceOverModel : VoiceOverModel
	{
		// Token: 0x06000480 RID: 1152 RVA: 0x000197AC File Offset: 0x000179AC
		public override string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject)
		{
			if (voiceObject == null)
			{
				return "";
			}
			if (!TutorialPhase.Instance.IsCompleted && TutorialPhase.Instance.TutorialVillageHeadman.CharacterObject == character)
			{
				string text = voiceObject.VoicePaths.First<string>();
				Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text, 0, Debug.DebugColor.White, 17592186044416UL);
				text = text.Replace("$PLATFORM", "PC");
				return text + ".ogg";
			}
			if (StoryModeHeroes.ElderBrother.CharacterObject != character)
			{
				return base.BaseModel.GetSoundPathForCharacter(character, voiceObject);
			}
			string text2 = "";
			string text3 = character.StringId + "_" + (CharacterObject.PlayerCharacter.IsFemale ? "female" : "male");
			foreach (string text4 in voiceObject.VoicePaths)
			{
				if (text4.Contains(text3))
				{
					text2 = text4;
					break;
				}
				if (text4.Contains(character.StringId + "_"))
				{
					text2 = text4;
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text2, 0, Debug.DebugColor.White, 17592186044416UL);
			text2 = text2.Replace("$PLATFORM", "PC");
			return text2 + ".ogg";
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001992C File Offset: 0x00017B2C
		public override string GetAccentClass(CultureObject culture, bool isHighClass)
		{
			return base.BaseModel.GetAccentClass(culture, isHighClass);
		}

		// Token: 0x04000194 RID: 404
		private const string Male = "male";

		// Token: 0x04000195 RID: 405
		private const string Female = "female";
	}
}
