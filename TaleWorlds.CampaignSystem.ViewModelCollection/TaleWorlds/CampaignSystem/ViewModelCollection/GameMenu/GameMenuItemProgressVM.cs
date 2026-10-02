using System;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x0200009B RID: 155
	public class GameMenuItemProgressVM : ViewModel
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x0003ED16 File Offset: 0x0003CF16
		public void InitializeWith(MenuContext context, int virtualIndex)
		{
			this._context = context;
			this._virtualIndex = virtualIndex;
			this._gameMenuManager = Campaign.Current.GameMenuManager;
			this.RefreshValues();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0003ED3C File Offset: 0x0003CF3C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._text1 = Campaign.Current.GameMenuManager.GetVirtualMenuOptionText(this._context, this._virtualIndex).ToString();
			this._text2 = Campaign.Current.GameMenuManager.GetVirtualMenuOptionText2(this._context, this._virtualIndex).ToString();
			this.Refresh();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003EDA4 File Offset: 0x0003CFA4
		private void Refresh()
		{
			switch (this._gameMenuManager.GetVirtualMenuAndOptionType(this._context))
			{
			case GameMenu.MenuAndOptionType.WaitMenuShowProgressAndHoursOption:
			{
				float num = Campaign.Current.GameMenuManager.GetVirtualMenuTargetWaitHours(this._context);
				num = (float)MathF.Round(num);
				if (num > 1f)
				{
					GameTexts.SetVariable("PLURAL_HOURS", 1);
				}
				else
				{
					GameTexts.SetVariable("PLURAL_HOURS", 0);
				}
				GameTexts.SetVariable("HOUR", num.ToString());
				this.ProgressText = GameTexts.FindText("str_hours", null).ToString();
				goto IL_00BB;
			}
			case GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption:
				this.ProgressText = "";
				goto IL_00BB;
			}
			Debug.FailedAssert("Shouldn't create game menu progress for normal options", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\GameMenuItemProgressVM.cs", "Refresh", 69);
			return;
			IL_00BB:
			this.Text = (Campaign.Current.GameMenuManager.GetVirtualMenuIsWaitActive(this._context) ? this._text2 : this._text1);
			float virtualMenuProgress = Campaign.Current.GameMenuManager.GetVirtualMenuProgress(this._context);
			this.Progress = (float)MathF.Round(virtualMenuProgress * 100f);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0003EEC2 File Offset: 0x0003D0C2
		public void OnTick()
		{
			this.Refresh();
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0003EECA File Offset: 0x0003D0CA
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0003EED2 File Offset: 0x0003D0D2
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0003EEF5 File Offset: 0x0003D0F5
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0003EEFD File Offset: 0x0003D0FD
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (value != this._progressText)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0003EF20 File Offset: 0x0003D120
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x0003EF28 File Offset: 0x0003D128
		[DataSourceProperty]
		public float Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (value != this._progress)
				{
					this._progress = value;
					base.OnPropertyChangedWithValue(value, "Progress");
				}
			}
		}

		// Token: 0x040006CE RID: 1742
		private MenuContext _context;

		// Token: 0x040006CF RID: 1743
		private GameMenuManager _gameMenuManager;

		// Token: 0x040006D0 RID: 1744
		private int _virtualIndex;

		// Token: 0x040006D1 RID: 1745
		private string _text1 = "";

		// Token: 0x040006D2 RID: 1746
		private string _text2 = "";

		// Token: 0x040006D3 RID: 1747
		private string _text;

		// Token: 0x040006D4 RID: 1748
		private string _progressText;

		// Token: 0x040006D5 RID: 1749
		private float _progress;
	}
}
