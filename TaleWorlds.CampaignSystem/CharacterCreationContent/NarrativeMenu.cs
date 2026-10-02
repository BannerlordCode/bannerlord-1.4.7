using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000211 RID: 529
	public sealed class NarrativeMenu
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x00090EC2 File Offset: 0x0008F0C2
		public List<NarrativeMenuCharacter> Characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x00090ECA File Offset: 0x0008F0CA
		public MBReadOnlyList<NarrativeMenuOption> CharacterCreationMenuOptions
		{
			get
			{
				return this._characterCreationMenuOptions;
			}
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00090ED4 File Offset: 0x0008F0D4
		public NarrativeMenu(string stringId, string inputMenuId, string outputMenuId, TextObject title, TextObject description, List<NarrativeMenuCharacter> characters, NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate getNarrativeMenuCharacterArgs)
		{
			this.StringId = stringId;
			this.InputMenuId = inputMenuId;
			this.OutputMenuId = outputMenuId;
			this.Title = title;
			this.Description = description;
			this._characters = characters;
			this.GetNarrativeMenuCharacterArgs = getNarrativeMenuCharacterArgs;
			this._characterCreationMenuOptions = new MBList<NarrativeMenuOption>();
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00090F27 File Offset: 0x0008F127
		public void AddNarrativeMenuOption(NarrativeMenuOption narrativeMenuOption)
		{
			this._characterCreationMenuOptions.Add(narrativeMenuOption);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00090F35 File Offset: 0x0008F135
		public void RemoveNarrativeMenuOption(NarrativeMenuOption narrativeMenuOption)
		{
			this._characterCreationMenuOptions.Remove(narrativeMenuOption);
		}

		// Token: 0x0400096C RID: 2412
		public readonly string StringId;

		// Token: 0x0400096D RID: 2413
		public readonly string InputMenuId;

		// Token: 0x0400096E RID: 2414
		public readonly string OutputMenuId;

		// Token: 0x0400096F RID: 2415
		public readonly TextObject Title;

		// Token: 0x04000970 RID: 2416
		public readonly TextObject Description;

		// Token: 0x04000971 RID: 2417
		private readonly List<NarrativeMenuCharacter> _characters;

		// Token: 0x04000972 RID: 2418
		private readonly MBList<NarrativeMenuOption> _characterCreationMenuOptions;

		// Token: 0x04000973 RID: 2419
		public readonly NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate GetNarrativeMenuCharacterArgs;

		// Token: 0x0200060A RID: 1546
		// (Invoke) Token: 0x0600502E RID: 20526
		public delegate List<NarrativeMenuCharacterArgs> GetNarrativeMenuCharacterArgsDelegate(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager);
	}
}
