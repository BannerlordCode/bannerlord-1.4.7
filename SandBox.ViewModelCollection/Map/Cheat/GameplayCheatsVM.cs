using System;
using System.Collections.Generic;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x02000051 RID: 81
	public class GameplayCheatsVM : ViewModel
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x00012D5C File Offset: 0x00010F5C
		public GameplayCheatsVM(Action onClose, IEnumerable<GameplayCheatBase> cheats)
		{
			this._onClose = onClose;
			this._initialCheatList = cheats;
			this.Cheats = new MBBindingList<CheatItemBaseVM>();
			this._activeCheatGroups = new List<CheatGroupItemVM>();
			this._mainTitleText = new TextObject("{=OYtysXzk}Cheats", null);
			this.FillWithCheats(cheats);
			this.RefreshValues();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00012DB4 File Offset: 0x00010FB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			for (int i = 0; i < this.Cheats.Count; i++)
			{
				this.Cheats[i].RefreshValues();
			}
			if (this._activeCheatGroups.Count > 0)
			{
				TextObject textObject = new TextObject("{=1tiF5JhE}{TITLE} > {SUBTITLE}", null);
				for (int j = 0; j < this._activeCheatGroups.Count; j++)
				{
					if (j == 0)
					{
						textObject.SetTextVariable("TITLE", this._mainTitleText.ToString());
					}
					else
					{
						textObject.SetTextVariable("TITLE", textObject.ToString());
					}
					textObject.SetTextVariable("SUBTITLE", this._activeCheatGroups[j].Name.ToString());
				}
				this.Title = textObject.ToString();
				this.ButtonCloseLabel = GameTexts.FindText("str_back", null).ToString();
				return;
			}
			this.Title = this._mainTitleText.ToString();
			this.ButtonCloseLabel = GameTexts.FindText("str_close", null).ToString();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00012EBB File Offset: 0x000110BB
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM closeInputKey = this.CloseInputKey;
			if (closeInputKey == null)
			{
				return;
			}
			closeInputKey.OnFinalize();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00012ED4 File Offset: 0x000110D4
		private void FillWithCheats(IEnumerable<GameplayCheatBase> cheats)
		{
			this.Cheats.Clear();
			foreach (GameplayCheatBase gameplayCheatBase in cheats)
			{
				GameplayCheatItem gameplayCheatItem;
				GameplayCheatGroup gameplayCheatGroup;
				if ((gameplayCheatItem = gameplayCheatBase as GameplayCheatItem) != null)
				{
					this.Cheats.Add(new CheatActionItemVM(gameplayCheatItem, new Action<CheatActionItemVM>(this.OnCheatActionExecuted)));
				}
				else if ((gameplayCheatGroup = gameplayCheatBase as GameplayCheatGroup) != null)
				{
					this.Cheats.Add(new CheatGroupItemVM(gameplayCheatGroup, new Action<CheatGroupItemVM>(this.OnCheatGroupSelected)));
				}
			}
			this.RefreshValues();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00012F78 File Offset: 0x00011178
		private void OnCheatActionExecuted(CheatActionItemVM cheatItem)
		{
			this._activeCheatGroups.Clear();
			this.FillWithCheats(this._initialCheatList);
			TextObject textObject = new TextObject("{=1QAEyN2V}Cheat Used: {CHEAT}", null);
			textObject.SetTextVariable("CHEAT", cheatItem.Name.ToString());
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00012FCD File Offset: 0x000111CD
		private void OnCheatGroupSelected(CheatGroupItemVM cheatGroup)
		{
			this._activeCheatGroups.Add(cheatGroup);
			IEnumerable<GameplayCheatBase> enumerable;
			if (cheatGroup == null)
			{
				enumerable = null;
			}
			else
			{
				GameplayCheatGroup cheatGroup2 = cheatGroup.CheatGroup;
				enumerable = ((cheatGroup2 != null) ? cheatGroup2.GetCheats() : null);
			}
			this.FillWithCheats(enumerable ?? this._initialCheatList);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00013004 File Offset: 0x00011204
		public void ExecuteClose()
		{
			if (this._activeCheatGroups.Count > 0)
			{
				this._activeCheatGroups.RemoveAt(this._activeCheatGroups.Count - 1);
				if (this._activeCheatGroups.Count > 0)
				{
					this.FillWithCheats(this._activeCheatGroups[this._activeCheatGroups.Count - 1].CheatGroup.GetCheats());
					return;
				}
				this.FillWithCheats(this._initialCheatList);
				return;
			}
			else
			{
				Action onClose = this._onClose;
				if (onClose == null)
				{
					return;
				}
				onClose();
				return;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001308B File Offset: 0x0001128B
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x00013093 File Offset: 0x00011293
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000130B6 File Offset: 0x000112B6
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x000130BE File Offset: 0x000112BE
		[DataSourceProperty]
		public string ButtonCloseLabel
		{
			get
			{
				return this._buttonCloseLabel;
			}
			set
			{
				if (value != this._buttonCloseLabel)
				{
					this._buttonCloseLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCloseLabel");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000130E1 File Offset: 0x000112E1
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x000130E9 File Offset: 0x000112E9
		[DataSourceProperty]
		public MBBindingList<CheatItemBaseVM> Cheats
		{
			get
			{
				return this._cheats;
			}
			set
			{
				if (value != this._cheats)
				{
					this._cheats = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheatItemBaseVM>>(value, "Cheats");
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00013107 File Offset: 0x00011307
		public void SetCloseInputKey(HotKey hotKey)
		{
			this.CloseInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00013116 File Offset: 0x00011316
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x0001311E File Offset: 0x0001131E
		[DataSourceProperty]
		public InputKeyItemVM CloseInputKey
		{
			get
			{
				return this._closeInputKey;
			}
			set
			{
				if (value != this._closeInputKey)
				{
					this._closeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CloseInputKey");
				}
			}
		}

		// Token: 0x0400026F RID: 623
		private readonly Action _onClose;

		// Token: 0x04000270 RID: 624
		private readonly IEnumerable<GameplayCheatBase> _initialCheatList;

		// Token: 0x04000271 RID: 625
		private readonly TextObject _mainTitleText;

		// Token: 0x04000272 RID: 626
		private List<CheatGroupItemVM> _activeCheatGroups;

		// Token: 0x04000273 RID: 627
		private string _title;

		// Token: 0x04000274 RID: 628
		private string _buttonCloseLabel;

		// Token: 0x04000275 RID: 629
		private MBBindingList<CheatItemBaseVM> _cheats;

		// Token: 0x04000276 RID: 630
		private InputKeyItemVM _closeInputKey;
	}
}
