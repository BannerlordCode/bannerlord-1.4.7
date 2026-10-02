using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Information.RundownTooltip
{
	// Token: 0x0200001C RID: 28
	public class RundownTooltipVM : TooltipBaseVM
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00005573 File Offset: 0x00003773
		public bool IsInitializedProperly { get; }

		// Token: 0x0600018A RID: 394 RVA: 0x0000557C File Offset: 0x0000377C
		public RundownTooltipVM(Type invokedType, object[] invokedArgs)
			: base(invokedType, invokedArgs)
		{
			this.Lines = new MBBindingList<RundownLineVM>();
			if (invokedArgs.Length == 5)
			{
				this._titleTextSource = invokedArgs[2] as TextObject;
				this._summaryTextSource = invokedArgs[3] as TextObject;
				this._valueCategorization = (RundownTooltipVM.ValueCategorization)invokedArgs[4];
				bool flag = !TextObject.IsNullOrEmpty(this._titleTextSource);
				bool flag2 = !TextObject.IsNullOrEmpty(this._summaryTextSource);
				this.IsInitializedProperly = flag && flag2;
			}
			else
			{
				Debug.FailedAssert("Unexpected number of arguments for rundown tooltip", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core.ViewModelCollection\\Information\\RundownTooltip\\RundownTooltipVM.cs", ".ctor", 46);
			}
			this.ValueCategorizationAsInt = (int)this._valueCategorization;
			this._isPeriodicRefreshEnabled = true;
			this._periodicRefreshDelay = 1f;
			this.Refresh();
			this.RefreshValues();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005635 File Offset: 0x00003835
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleTextSource = this._titleTextSource;
			this.TitleText = ((titleTextSource != null) ? titleTextSource.ToString() : null);
			this.RefreshExtendText();
			this.RefreshExpectedChangeText();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00005661 File Offset: 0x00003861
		protected override void OnPeriodicRefresh()
		{
			base.OnPeriodicRefresh();
			this.Refresh();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000566F File Offset: 0x0000386F
		protected override void OnIsExtendedChanged()
		{
			base.OnIsExtendedChanged();
			base.IsActive = false;
			this.Refresh();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00005684 File Offset: 0x00003884
		private void Refresh()
		{
			base.InvokeRefreshData<RundownTooltipVM>(this);
			this.RefreshExtendText();
			this.RefreshExpectedChangeText();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000569C File Offset: 0x0000389C
		private void RefreshExpectedChangeText()
		{
			if (this._summaryTextSource != null)
			{
				string text = "DefaultChange";
				if (this._valueCategorization != RundownTooltipVM.ValueCategorization.None)
				{
					text = (((float)((this._valueCategorization == RundownTooltipVM.ValueCategorization.LargeIsBetter) ? 1 : (-1)) * this.CurrentExpectedChange < 0f) ? "NegativeChange" : "PositiveChange");
				}
				TextObject textObject = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null);
				textObject.SetTextVariable("LEFT", this._summaryTextSource.ToString());
				textObject.SetTextVariable("RIGHT", string.Concat(new string[]
				{
					"<span style=\"",
					text,
					"\">",
					string.Format("{0:0.##}", this.CurrentExpectedChange),
					"</span>"
				}));
				this.ExpectedChangeText = textObject.ToString();
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000576B File Offset: 0x0000396B
		private void RefreshExtendText()
		{
			GameTexts.SetVariable("EXTEND_KEY", GameTexts.FindText("str_game_key_text", "anyalt").ToString());
			this.ExtendText = GameTexts.FindText("str_map_tooltip_info", null).ToString();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000057A4 File Offset: 0x000039A4
		public static void RefreshGenericRundownTooltip(RundownTooltipVM rundownTooltip, object[] args)
		{
			rundownTooltip.IsActive = rundownTooltip.IsInitializedProperly;
			if (rundownTooltip.IsActive)
			{
				Func<List<RundownLineVM>> func = args[0] as Func<List<RundownLineVM>>;
				Func<List<RundownLineVM>> func2 = args[1] as Func<List<RundownLineVM>>;
				float num = 0f;
				rundownTooltip.Lines.Clear();
				Func<List<RundownLineVM>> func3 = ((rundownTooltip.IsExtended && func2 != null) ? func2 : func);
				List<RundownLineVM> list = ((func3 != null) ? func3() : null);
				if (list != null)
				{
					foreach (RundownLineVM rundownLineVM in list)
					{
						num += rundownLineVM.Value;
						rundownTooltip.Lines.Add(rundownLineVM);
					}
				}
				rundownTooltip.CurrentExpectedChange = num;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00005864 File Offset: 0x00003A64
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000586C File Offset: 0x00003A6C
		[DataSourceProperty]
		public MBBindingList<RundownLineVM> Lines
		{
			get
			{
				return this._lines;
			}
			set
			{
				if (value != this._lines)
				{
					this._lines = value;
					base.OnPropertyChangedWithValue<MBBindingList<RundownLineVM>>(value, "Lines");
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000588A File Offset: 0x00003A8A
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00005892 File Offset: 0x00003A92
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000058B5 File Offset: 0x00003AB5
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000058BD File Offset: 0x00003ABD
		[DataSourceProperty]
		public string ExpectedChangeText
		{
			get
			{
				return this._expectedChangeText;
			}
			set
			{
				if (value != this._expectedChangeText)
				{
					this._expectedChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpectedChangeText");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000058E0 File Offset: 0x00003AE0
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000058E8 File Offset: 0x00003AE8
		[DataSourceProperty]
		public int ValueCategorizationAsInt
		{
			get
			{
				return this._valueCategorizationAsInt;
			}
			set
			{
				if (value != this._valueCategorizationAsInt)
				{
					this._valueCategorizationAsInt = value;
					base.OnPropertyChangedWithValue(value, "ValueCategorizationAsInt");
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00005906 File Offset: 0x00003B06
		// (set) Token: 0x0600019B RID: 411 RVA: 0x0000590E File Offset: 0x00003B0E
		[DataSourceProperty]
		public string ExtendText
		{
			get
			{
				return this._extendText;
			}
			set
			{
				if (value != this._extendText)
				{
					this._extendText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExtendText");
				}
			}
		}

		// Token: 0x0400009E RID: 158
		public float CurrentExpectedChange;

		// Token: 0x0400009F RID: 159
		private readonly RundownTooltipVM.ValueCategorization _valueCategorization;

		// Token: 0x040000A0 RID: 160
		private readonly TextObject _titleTextSource;

		// Token: 0x040000A1 RID: 161
		private readonly TextObject _summaryTextSource;

		// Token: 0x040000A2 RID: 162
		private MBBindingList<RundownLineVM> _lines;

		// Token: 0x040000A3 RID: 163
		private string _titleText;

		// Token: 0x040000A4 RID: 164
		private string _expectedChangeText;

		// Token: 0x040000A5 RID: 165
		private int _valueCategorizationAsInt;

		// Token: 0x040000A6 RID: 166
		private string _extendText;

		// Token: 0x02000039 RID: 57
		public enum ValueCategorization
		{
			// Token: 0x040000F9 RID: 249
			None,
			// Token: 0x040000FA RID: 250
			LargeIsBetter,
			// Token: 0x040000FB RID: 251
			SmallIsBetter
		}
	}
}
