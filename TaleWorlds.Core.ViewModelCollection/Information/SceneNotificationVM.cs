using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000019 RID: 25
	public class SceneNotificationVM : ViewModel
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004AB0 File Offset: 0x00002CB0
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public SceneNotificationData ActiveData { get; private set; }

		// Token: 0x06000146 RID: 326 RVA: 0x00004AC1 File Offset: 0x00002CC1
		public SceneNotificationVM(Action onPositiveTrigger, Action closeNotification, Func<string> getContinueInputText)
		{
			this._onPositiveTrigger = onPositiveTrigger;
			this._closeNotification = closeNotification;
			this._getContinueInputText = getContinueInputText;
			this.IsShown = false;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004AE5 File Offset: 0x00002CE5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ClickToContinueText = this._getContinueInputText();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00004AFE File Offset: 0x00002CFE
		public void CreateNotification(SceneNotificationData data)
		{
			this.SetData(data);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004B08 File Offset: 0x00002D08
		public void ClearData()
		{
			this.IsShown = false;
			this.ActiveData = null;
			this.Scene = null;
			base.OnPropertyChanged("TitleText");
			base.OnPropertyChanged("AffirmativeDescription");
			base.OnPropertyChanged("CancelDescription");
			base.OnPropertyChanged("SceneID");
			base.OnPropertyChanged("IsAffirmativeOptionShown");
			base.OnPropertyChanged("IsNegativeOptionShown");
			base.OnPropertyChanged("AffirmativeText");
			base.OnPropertyChanged("NegativeText");
			base.OnPropertyChanged("AffirmativeAction");
			base.OnPropertyChanged("NegativeAction");
			base.OnPropertyChanged("AffirmativeTitleText");
			base.OnPropertyChanged("NegativeTitleText");
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00004BB0 File Offset: 0x00002DB0
		private void SetData(SceneNotificationData data)
		{
			this.ActiveData = data;
			base.OnPropertyChanged("TitleText");
			base.OnPropertyChanged("AffirmativeDescription");
			base.OnPropertyChanged("CancelDescription");
			base.OnPropertyChanged("SceneID");
			base.OnPropertyChanged("IsButtonOkShown");
			base.OnPropertyChanged("IsButtonCancelShown");
			base.OnPropertyChanged("ButtonOkLabel");
			base.OnPropertyChanged("ButtonCancelLabel");
			base.OnPropertyChanged("AffirmativeAction");
			base.OnPropertyChanged("NegativeAction");
			base.OnPropertyChanged("AffirmativeTitleText");
			base.OnPropertyChanged("NegativeTitleText");
			this.SetAffirmativeHintProperties(this.ActiveData.AffirmativeHintText, this.ActiveData.AffirmativeHintTextExtended);
			this.AffirmativeHint = new BasicTooltipViewModel(() => this._affirmativeHintTooltipProperties);
			this.RefreshValues();
			this.IsShown = true;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004C88 File Offset: 0x00002E88
		private void SetAffirmativeHintProperties(TextObject defaultHint, TextObject extendedHint)
		{
			this._affirmativeHintTooltipProperties = new List<TooltipProperty>();
			if (!string.IsNullOrEmpty((defaultHint != null) ? defaultHint.ToString() : null))
			{
				if (!string.IsNullOrEmpty((extendedHint != null) ? extendedHint.ToString() : null))
				{
					this._affirmativeHintTooltipProperties.Add(new TooltipProperty("", defaultHint.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None)
					{
						OnlyShowWhenNotExtended = true
					});
					this._affirmativeHintTooltipProperties.Add(new TooltipProperty("", extendedHint.ToString(), 0, true, TooltipProperty.TooltipPropertyFlags.None));
					return;
				}
				this._affirmativeHintTooltipProperties.Add(new TooltipProperty("", defaultHint.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004D28 File Offset: 0x00002F28
		public void ExecuteAffirmativeProcess()
		{
			Action onPositiveTrigger = this._onPositiveTrigger;
			if (onPositiveTrigger != null)
			{
				onPositiveTrigger();
			}
			SceneNotificationData activeData = this.ActiveData;
			if (activeData == null)
			{
				return;
			}
			activeData.OnAffirmativeAction();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004D4B File Offset: 0x00002F4B
		public void ExecuteClose()
		{
			SceneNotificationData activeData = this.ActiveData;
			this._closeNotification();
			if (activeData == null)
			{
				return;
			}
			activeData.OnCloseAction();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004D68 File Offset: 0x00002F68
		public void ExecuteNegativeProcess()
		{
			SceneNotificationData activeData = this.ActiveData;
			this._closeNotification();
			if (activeData == null)
			{
				return;
			}
			activeData.OnNegativeAction();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004D85 File Offset: 0x00002F85
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004D8D File Offset: 0x00002F8D
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00004D95 File Offset: 0x00002F95
		[DataSourceProperty]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (this._isShown != value)
				{
					this._isShown = value;
					base.OnPropertyChangedWithValue(value, "IsShown");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00004DB3 File Offset: 0x00002FB3
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00004DBB File Offset: 0x00002FBB
		[DataSourceProperty]
		public bool IsReady
		{
			get
			{
				return this._isReady;
			}
			set
			{
				if (this._isReady != value)
				{
					this._isReady = value;
					base.OnPropertyChangedWithValue(value, "IsReady");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004DD9 File Offset: 0x00002FD9
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00004DE1 File Offset: 0x00002FE1
		[DataSourceProperty]
		public string ClickToContinueText
		{
			get
			{
				return this._clickToContinueText;
			}
			set
			{
				if (this._clickToContinueText != value)
				{
					this._clickToContinueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToContinueText");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004E04 File Offset: 0x00003004
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				return ((activeData != null) ? activeData.TitleText.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00004E26 File Offset: 0x00003026
		[DataSourceProperty]
		public string AffirmativeDescription
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject affirmativeDescriptionText = activeData.AffirmativeDescriptionText;
					text = ((affirmativeDescriptionText != null) ? affirmativeDescriptionText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004E4F File Offset: 0x0000304F
		[DataSourceProperty]
		public string CancelDescription
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject negativeDescriptionText = activeData.NegativeDescriptionText;
					text = ((negativeDescriptionText != null) ? negativeDescriptionText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00004E78 File Offset: 0x00003078
		[DataSourceProperty]
		public string SceneID
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				return ((activeData != null) ? activeData.SceneID : null) ?? string.Empty;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00004E95 File Offset: 0x00003095
		[DataSourceProperty]
		public string ButtonOkLabel
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject affirmativeText = activeData.AffirmativeText;
					text = ((affirmativeText != null) ? affirmativeText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00004EBE File Offset: 0x000030BE
		[DataSourceProperty]
		public string ButtonCancelLabel
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject negativeText = activeData.NegativeText;
					text = ((negativeText != null) ? negativeText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00004EE7 File Offset: 0x000030E7
		[DataSourceProperty]
		public bool IsButtonOkShown
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				return activeData != null && activeData.IsAffirmativeOptionShown;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00004EFA File Offset: 0x000030FA
		[DataSourceProperty]
		public bool IsButtonCancelShown
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				return activeData != null && activeData.IsNegativeOptionShown;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004F0D File Offset: 0x0000310D
		[DataSourceProperty]
		public string AffirmativeTitleText
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject affirmativeTitleText = activeData.AffirmativeTitleText;
					text = ((affirmativeTitleText != null) ? affirmativeTitleText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00004F36 File Offset: 0x00003136
		[DataSourceProperty]
		public string NegativeTitleText
		{
			get
			{
				SceneNotificationData activeData = this.ActiveData;
				string text;
				if (activeData == null)
				{
					text = null;
				}
				else
				{
					TextObject negativeTitleText = activeData.NegativeTitleText;
					text = ((negativeTitleText != null) ? negativeTitleText.ToString() : null);
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004F5F File Offset: 0x0000315F
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00004F67 File Offset: 0x00003167
		[DataSourceProperty]
		public object Scene
		{
			get
			{
				return this._scene;
			}
			set
			{
				if (this._scene != value)
				{
					this._scene = value;
					base.OnPropertyChangedWithValue<object>(value, "Scene");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004F85 File Offset: 0x00003185
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00004F8D File Offset: 0x0000318D
		[DataSourceProperty]
		public float EndProgress
		{
			get
			{
				return this._endProgress;
			}
			set
			{
				if (this._endProgress != value)
				{
					this._endProgress = value;
					base.OnPropertyChangedWithValue(value, "EndProgress");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00004FAB File Offset: 0x000031AB
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00004FB3 File Offset: 0x000031B3
		[DataSourceProperty]
		public BasicTooltipViewModel AffirmativeHint
		{
			get
			{
				return this._affirmativeHint;
			}
			set
			{
				if (value != this._affirmativeHint)
				{
					this._affirmativeHint = value;
					base.OnPropertyChanged("AffirmativeHint");
				}
			}
		}

		// Token: 0x04000087 RID: 135
		private readonly Action _closeNotification;

		// Token: 0x04000088 RID: 136
		private readonly Action _onPositiveTrigger;

		// Token: 0x04000089 RID: 137
		private readonly Func<string> _getContinueInputText;

		// Token: 0x0400008A RID: 138
		private List<TooltipProperty> _affirmativeHintTooltipProperties;

		// Token: 0x0400008B RID: 139
		private bool _isShown;

		// Token: 0x0400008C RID: 140
		private bool _isReady;

		// Token: 0x0400008D RID: 141
		private object _scene;

		// Token: 0x0400008E RID: 142
		private float _endProgress;

		// Token: 0x0400008F RID: 143
		private string _clickToContinueText;

		// Token: 0x04000090 RID: 144
		private BasicTooltipViewModel _affirmativeHint;
	}
}
