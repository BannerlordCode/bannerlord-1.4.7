using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003E RID: 62
	public class MissionObjectiveVM : ViewModel
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x000152F8 File Offset: 0x000134F8
		public MissionObjectiveVM(MissionObjectiveLogic objectiveLogic, Camera missionCamera)
		{
			this.Markers = new MissionObjectiveMarkersVM(objectiveLogic, missionCamera);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00015310 File Offset: 0x00013510
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._currentObjective != null)
			{
				this.Title = this._currentObjective.Name.ToString();
				this.Description = this._currentObjective.Description.ToString();
			}
			this.Markers.RefreshValues();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00015364 File Offset: 0x00013564
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated -= this.OnObjectiveUpdated;
			}
			CharacterImageIdentifierVM objectiveGiverIdentifier = this.ObjectiveGiverIdentifier;
			if (objectiveGiverIdentifier != null)
			{
				objectiveGiverIdentifier.OnFinalize();
			}
			this.Markers.OnFinalize();
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000153B4 File Offset: 0x000135B4
		public void UpdateObjective(MissionObjective objective)
		{
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated -= this.OnObjectiveUpdated;
			}
			this._currentObjective = objective;
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated += this.OnObjectiveUpdated;
			}
			this.RefreshCurrentObjectiveData();
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001540C File Offset: 0x0001360C
		public void Tick(float dt)
		{
			if (this._isCurrentObjectiveDirty)
			{
				this.RefreshCurrentObjectiveData();
				this._isCurrentObjectiveDirty = false;
			}
			this.Markers.Tick(dt);
			if (this._currentObjective != null)
			{
				MissionObjectiveProgressInfo currentProgress = this._currentObjective.GetCurrentProgress();
				if (this.IsProgressDirty(in currentProgress))
				{
					this.HasProgress = currentProgress.HasProgress;
					this.CurrentProgress = currentProgress.CurrentProgressAmount;
					this.RequiredProgress = currentProgress.RequiredProgressAmount;
					this.ProgressText = GameTexts.FindText("str_LEFT_over_RIGHT_no_space", null).SetTextVariable("LEFT", this.CurrentProgress).SetTextVariable("RIGHT", this.RequiredProgress)
						.ToString();
				}
			}
			this.UpdateObjectiveGiver();
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000154B9 File Offset: 0x000136B9
		private void RefreshCurrentObjectiveData()
		{
			this.Markers.UpdateObjective(this._currentObjective);
			this.IsEnabled = this._currentObjective != null;
			this.RefreshValues();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x000154E4 File Offset: 0x000136E4
		private void UpdateObjectiveGiver()
		{
			BasicCharacterObject currentObjectiveGiver = this._currentObjectiveGiver;
			MissionObjective currentObjective = this._currentObjective;
			if (currentObjectiveGiver == ((currentObjective != null) ? currentObjective.ObjectiveGiver : null))
			{
				return;
			}
			MissionObjective currentObjective2 = this._currentObjective;
			this._currentObjectiveGiver = ((currentObjective2 != null) ? currentObjective2.ObjectiveGiver : null);
			this.HasObjectiveGiver = this._currentObjectiveGiver != null;
			CharacterImageIdentifierVM objectiveGiverIdentifier = this.ObjectiveGiverIdentifier;
			if (objectiveGiverIdentifier != null)
			{
				objectiveGiverIdentifier.OnFinalize();
			}
			if (this.HasObjectiveGiver)
			{
				this.ObjectiveGiverIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this._currentObjectiveGiver));
			}
			BasicCharacterObject currentObjectiveGiver2 = this._currentObjectiveGiver;
			string text;
			if (currentObjectiveGiver2 == null)
			{
				text = null;
			}
			else
			{
				TextObject name = currentObjectiveGiver2.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.ObjectiveGiverName = text ?? string.Empty;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00015590 File Offset: 0x00013790
		private bool IsProgressDirty(in MissionObjectiveProgressInfo progressInfo)
		{
			MissionObjectiveProgressInfo missionObjectiveProgressInfo = progressInfo;
			return missionObjectiveProgressInfo.HasProgress != this.HasProgress || progressInfo.CurrentProgressAmount != this.CurrentProgress || progressInfo.RequiredProgressAmount != this.RequiredProgress;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000155D4 File Offset: 0x000137D4
		private void OnObjectiveUpdated()
		{
			this._isCurrentObjectiveDirty = true;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x000155DD File Offset: 0x000137DD
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x000155E5 File Offset: 0x000137E5
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (this._title != value)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
					this.HasTitle = !string.IsNullOrEmpty(value);
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00015617 File Offset: 0x00013817
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0001561F File Offset: 0x0001381F
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (this._description != value)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
					this.HasDescription = !string.IsNullOrEmpty(value);
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00015651 File Offset: 0x00013851
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00015659 File Offset: 0x00013859
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (this._progressText != value)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001567C File Offset: 0x0001387C
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00015684 File Offset: 0x00013884
		[DataSourceProperty]
		public string ObjectiveGiverName
		{
			get
			{
				return this._objectiveGiverName;
			}
			set
			{
				if (this._objectiveGiverName != value)
				{
					this._objectiveGiverName = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveGiverName");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000156A7 File Offset: 0x000138A7
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x000156AF File Offset: 0x000138AF
		[DataSourceProperty]
		public bool HasObjectiveGiver
		{
			get
			{
				return this._hasObjectiveGiver;
			}
			set
			{
				if (this._hasObjectiveGiver != value)
				{
					this._hasObjectiveGiver = value;
					base.OnPropertyChangedWithValue(value, "HasObjectiveGiver");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x000156CD File Offset: 0x000138CD
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x000156D5 File Offset: 0x000138D5
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.Markers.IsEnabled = value;
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000156FF File Offset: 0x000138FF
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00015707 File Offset: 0x00013907
		[DataSourceProperty]
		public bool HasTitle
		{
			get
			{
				return this._hasTitle;
			}
			set
			{
				if (this._hasTitle != value)
				{
					this._hasTitle = value;
					base.OnPropertyChangedWithValue(value, "HasTitle");
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00015725 File Offset: 0x00013925
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0001572D File Offset: 0x0001392D
		[DataSourceProperty]
		public bool HasDescription
		{
			get
			{
				return this._hasDescription;
			}
			set
			{
				if (this._hasDescription != value)
				{
					this._hasDescription = value;
					base.OnPropertyChangedWithValue(value, "HasDescription");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001574B File Offset: 0x0001394B
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00015753 File Offset: 0x00013953
		[DataSourceProperty]
		public bool HasProgress
		{
			get
			{
				return this._hasProgress;
			}
			set
			{
				if (this._hasProgress != value)
				{
					this._hasProgress = value;
					base.OnPropertyChangedWithValue(value, "HasProgress");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00015771 File Offset: 0x00013971
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00015779 File Offset: 0x00013979
		[DataSourceProperty]
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (this._currentProgress != value)
				{
					this._currentProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProgress");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00015797 File Offset: 0x00013997
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x0001579F File Offset: 0x0001399F
		[DataSourceProperty]
		public int RequiredProgress
		{
			get
			{
				return this._requiredProgress;
			}
			set
			{
				if (this._requiredProgress != value)
				{
					this._requiredProgress = value;
					base.OnPropertyChangedWithValue(value, "RequiredProgress");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x000157BD File Offset: 0x000139BD
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x000157C5 File Offset: 0x000139C5
		[DataSourceProperty]
		public CharacterImageIdentifierVM ObjectiveGiverIdentifier
		{
			get
			{
				return this._objectiveGiverIdentifier;
			}
			set
			{
				if (this._objectiveGiverIdentifier != value)
				{
					this._objectiveGiverIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ObjectiveGiverIdentifier");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x000157E3 File Offset: 0x000139E3
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x000157EB File Offset: 0x000139EB
		[DataSourceProperty]
		public MissionObjectiveMarkersVM Markers
		{
			get
			{
				return this._markers;
			}
			set
			{
				if (this._markers != value)
				{
					this._markers = value;
					base.OnPropertyChangedWithValue<MissionObjectiveMarkersVM>(value, "Markers");
				}
			}
		}

		// Token: 0x0400027C RID: 636
		private MissionObjective _currentObjective;

		// Token: 0x0400027D RID: 637
		private BasicCharacterObject _currentObjectiveGiver;

		// Token: 0x0400027E RID: 638
		private bool _isCurrentObjectiveDirty;

		// Token: 0x0400027F RID: 639
		private string _title;

		// Token: 0x04000280 RID: 640
		private string _description;

		// Token: 0x04000281 RID: 641
		private string _progressText;

		// Token: 0x04000282 RID: 642
		private string _objectiveGiverName;

		// Token: 0x04000283 RID: 643
		private bool _hasObjectiveGiver;

		// Token: 0x04000284 RID: 644
		private bool _isEnabled;

		// Token: 0x04000285 RID: 645
		private bool _hasTitle;

		// Token: 0x04000286 RID: 646
		private bool _hasDescription;

		// Token: 0x04000287 RID: 647
		private bool _hasProgress;

		// Token: 0x04000288 RID: 648
		private int _currentProgress;

		// Token: 0x04000289 RID: 649
		private int _requiredProgress;

		// Token: 0x0400028A RID: 650
		private CharacterImageIdentifierVM _objectiveGiverIdentifier;

		// Token: 0x0400028B RID: 651
		private MissionObjectiveMarkersVM _markers;
	}
}
