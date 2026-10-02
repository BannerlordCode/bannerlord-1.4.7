using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000157 RID: 343
	public abstract class CharacterCreationStageBaseVM : ViewModel
	{
		// Token: 0x06002056 RID: 8278 RVA: 0x00076478 File Offset: 0x00074678
		protected CharacterCreationStageBaseVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
		{
			this.CharacterCreationManager = characterCreationManager;
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._affirmativeActionText = affirmativeActionText;
			this._negativeActionText = negativeActionText;
			TextObject affirmativeActionText2 = this._affirmativeActionText;
			this.NextStageText = ((affirmativeActionText2 != null) ? affirmativeActionText2.ToString() : null);
			TextObject negativeActionText2 = this._negativeActionText;
			this.PreviousStageText = ((negativeActionText2 != null) ? negativeActionText2.ToString() : null);
		}

		// Token: 0x06002057 RID: 8279
		public abstract void OnNextStage();

		// Token: 0x06002058 RID: 8280
		public abstract void OnPreviousStage();

		// Token: 0x06002059 RID: 8281
		public abstract bool CanAdvanceToNextStage();

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x00076516 File Offset: 0x00074716
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x0007651E File Offset: 0x0007471E
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

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x00076541 File Offset: 0x00074741
		// (set) Token: 0x0600205D RID: 8285 RVA: 0x00076549 File Offset: 0x00074749
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x0007656C File Offset: 0x0007476C
		// (set) Token: 0x0600205F RID: 8287 RVA: 0x00076574 File Offset: 0x00074774
		[DataSourceProperty]
		public string SelectionText
		{
			get
			{
				return this._selectionText;
			}
			set
			{
				if (value != this._selectionText)
				{
					this._selectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionText");
				}
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x00076597 File Offset: 0x00074797
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x0007659F File Offset: 0x0007479F
		[DataSourceProperty]
		public string NextStageText
		{
			get
			{
				return this._nextStageText;
			}
			set
			{
				if (value != this._nextStageText)
				{
					this._nextStageText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextStageText");
				}
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x000765C2 File Offset: 0x000747C2
		// (set) Token: 0x06002063 RID: 8291 RVA: 0x000765CA File Offset: 0x000747CA
		[DataSourceProperty]
		public string PreviousStageText
		{
			get
			{
				return this._previousStageText;
			}
			set
			{
				if (value != this._previousStageText)
				{
					this._previousStageText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousStageText");
				}
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x000765ED File Offset: 0x000747ED
		// (set) Token: 0x06002065 RID: 8293 RVA: 0x000765F5 File Offset: 0x000747F5
		[DataSourceProperty]
		public int TotalStageCount
		{
			get
			{
				return this._totalStageCount;
			}
			set
			{
				if (value != this._totalStageCount)
				{
					this._totalStageCount = value;
					base.OnPropertyChangedWithValue(value, "TotalStageCount");
				}
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00076613 File Offset: 0x00074813
		// (set) Token: 0x06002067 RID: 8295 RVA: 0x0007661B File Offset: 0x0007481B
		[DataSourceProperty]
		public int FurthestIndex
		{
			get
			{
				return this._furthestIndex;
			}
			set
			{
				if (value != this._furthestIndex)
				{
					this._furthestIndex = value;
					base.OnPropertyChangedWithValue(value, "FurthestIndex");
				}
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x00076639 File Offset: 0x00074839
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x00076641 File Offset: 0x00074841
		[DataSourceProperty]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (value != this._currentStageIndex)
				{
					this._currentStageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentStageIndex");
				}
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x0007665F File Offset: 0x0007485F
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x00076667 File Offset: 0x00074867
		[DataSourceProperty]
		public bool AnyItemSelected
		{
			get
			{
				return this._anyItemSelected;
			}
			set
			{
				if (value != this._anyItemSelected)
				{
					this._anyItemSelected = value;
					base.OnPropertyChangedWithValue(value, "AnyItemSelected");
				}
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x00076685 File Offset: 0x00074885
		// (set) Token: 0x0600206D RID: 8301 RVA: 0x0007668D File Offset: 0x0007488D
		[DataSourceProperty]
		public bool CanAdvance
		{
			get
			{
				return this._canAdvance;
			}
			set
			{
				if (value != this._canAdvance)
				{
					this._canAdvance = value;
					base.OnPropertyChangedWithValue(value, "CanAdvance");
				}
			}
		}

		// Token: 0x04000F08 RID: 3848
		protected readonly CharacterCreationManager CharacterCreationManager;

		// Token: 0x04000F09 RID: 3849
		protected readonly Action _affirmativeAction;

		// Token: 0x04000F0A RID: 3850
		protected readonly Action _negativeAction;

		// Token: 0x04000F0B RID: 3851
		protected readonly TextObject _affirmativeActionText;

		// Token: 0x04000F0C RID: 3852
		protected readonly TextObject _negativeActionText;

		// Token: 0x04000F0D RID: 3853
		private string _title = "";

		// Token: 0x04000F0E RID: 3854
		private string _description = "";

		// Token: 0x04000F0F RID: 3855
		private string _selectionText = "";

		// Token: 0x04000F10 RID: 3856
		private string _nextStageText;

		// Token: 0x04000F11 RID: 3857
		private string _previousStageText;

		// Token: 0x04000F12 RID: 3858
		private int _totalStageCount = -1;

		// Token: 0x04000F13 RID: 3859
		private int _currentStageIndex = -1;

		// Token: 0x04000F14 RID: 3860
		private int _furthestIndex = -1;

		// Token: 0x04000F15 RID: 3861
		private bool _anyItemSelected;

		// Token: 0x04000F16 RID: 3862
		private bool _canAdvance;
	}
}
