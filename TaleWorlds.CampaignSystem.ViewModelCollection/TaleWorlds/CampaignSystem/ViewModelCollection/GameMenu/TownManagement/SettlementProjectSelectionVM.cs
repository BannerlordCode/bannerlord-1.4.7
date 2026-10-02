using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A6 RID: 166
	public class SettlementProjectSelectionVM : ViewModel
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00041CFE File Offset: 0x0003FEFE
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x00041D06 File Offset: 0x0003FF06
		public List<Building> LocalDevelopmentList { get; private set; }

		// Token: 0x06001000 RID: 4096 RVA: 0x00041D0F File Offset: 0x0003FF0F
		public SettlementProjectSelectionVM(Settlement settlement, Action onAnyChangeInQueue)
		{
			this._settlement = settlement;
			this._town = settlement.Town;
			this._onAnyChangeInQueue = onAnyChangeInQueue;
			this.RefreshValues();
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00041D38 File Offset: 0x0003FF38
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ProjectsText = new TextObject("{=LpsoPtOo}Projects", null).ToString();
			this.DailyDefaultsText = GameTexts.FindText("str_town_management_daily_defaults", null).ToString();
			this.DailyDefaultsExplanationText = GameTexts.FindText("str_town_management_daily_defaults_explanation", null).ToString();
			this.QueueText = GameTexts.FindText("str_town_management_queue", null).ToString();
			this.Refresh();
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00041DAC File Offset: 0x0003FFAC
		public void Refresh()
		{
			this.AvailableProjects = new MBBindingList<SettlementBuildingProjectVM>();
			this.DailyDefaultList = new MBBindingList<SettlementDailyProjectVM>();
			this.LocalDevelopmentList = new List<Building>();
			this.CurrentDevelopmentQueue = new MBBindingList<SettlementBuildingProjectVM>();
			this.AvailableProjects.Clear();
			for (int i = 0; i < this._town.Buildings.Count; i++)
			{
				Building building = this._town.Buildings[i];
				if (!building.BuildingType.IsDailyProject)
				{
					SettlementBuildingProjectVM settlementBuildingProjectVM = new SettlementBuildingProjectVM(new Action<SettlementProjectVM, bool>(this.OnCurrentProjectSelection), new Action<SettlementProjectVM>(this.OnCurrentProjectSet), new Action(this.OnResetCurrentProject), building, this._settlement);
					this.AvailableProjects.Add(settlementBuildingProjectVM);
				}
				else
				{
					SettlementDailyProjectVM settlementDailyProjectVM = new SettlementDailyProjectVM(new Action<SettlementProjectVM, bool>(this.OnCurrentProjectSelection), new Action<SettlementProjectVM>(this.OnCurrentProjectSet), new Action(this.OnResetCurrentProject), building, this._settlement);
					this.DailyDefaultList.Add(settlementDailyProjectVM);
					if (settlementDailyProjectVM.Building == this._town.Buildings.FirstOrDefault<Building>((Building k) => k.IsCurrentlyDefault))
					{
						this.CurrentDailyDefault = settlementDailyProjectVM;
					}
				}
			}
			foreach (Building building2 in this._town.BuildingsInProgress)
			{
				this.LocalDevelopmentList.Add(building2);
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00041F4C File Offset: 0x0004014C
		private void OnCurrentProjectSet(SettlementProjectVM selectedItem)
		{
			this.CurrentSelectedProject = selectedItem;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00041F58 File Offset: 0x00040158
		private void OnCurrentProjectSelection(SettlementProjectVM selectedItem, bool isSetAsActiveDevelopment)
		{
			if (!selectedItem.IsDaily)
			{
				if (isSetAsActiveDevelopment)
				{
					if (this.LocalDevelopmentList.Exists((Building d) => d == selectedItem.Building))
					{
						int num = this.LocalDevelopmentList.IndexOf(selectedItem.Building) - 1;
						while (0 <= num)
						{
							this.LocalDevelopmentList[num + 1] = this.LocalDevelopmentList[num];
							num--;
						}
						this.LocalDevelopmentList.RemoveAt(0);
					}
					this.LocalDevelopmentList.Insert(0, selectedItem.Building);
				}
				else if (this.LocalDevelopmentList.Exists((Building d) => d == selectedItem.Building))
				{
					this.LocalDevelopmentList.Remove(selectedItem.Building);
				}
				else
				{
					this.LocalDevelopmentList.Add(selectedItem.Building);
				}
			}
			else
			{
				this.CurrentDailyDefault = selectedItem as SettlementDailyProjectVM;
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
			Action onAnyChangeInQueue = this._onAnyChangeInQueue;
			if (onAnyChangeInQueue == null)
			{
				return;
			}
			onAnyChangeInQueue();
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00042077 File Offset: 0x00040277
		private void OnResetCurrentProject()
		{
			this.RefreshCurrentSelectedProject();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00042080 File Offset: 0x00040280
		private void RefreshCurrentSelectedProject()
		{
			if (this.LocalDevelopmentList.Count > 0)
			{
				for (int i = 0; i < this.AvailableProjects.Count; i++)
				{
					SettlementBuildingProjectVM settlementBuildingProjectVM = this.AvailableProjects[i];
					if (settlementBuildingProjectVM.Building == this.LocalDevelopmentList[0])
					{
						this.CurrentSelectedProject = settlementBuildingProjectVM;
						return;
					}
				}
				return;
			}
			this.CurrentSelectedProject = this.CurrentDailyDefault;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000420E8 File Offset: 0x000402E8
		private void RefreshDevelopmentsQueueIndex()
		{
			this.CurrentDevelopmentQueue = new MBBindingList<SettlementBuildingProjectVM>();
			using (IEnumerator<SettlementBuildingProjectVM> enumerator = this.AvailableProjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SettlementBuildingProjectVM item = enumerator.Current;
					item.DevelopmentQueueIndex = -1;
					item.IsInQueue = this.LocalDevelopmentList.Any<Building>((Building d) => d.BuildingType == item.Building.BuildingType);
					item.IsCurrentActiveProject = false;
					if (item.IsInQueue)
					{
						int num = this.LocalDevelopmentList.IndexOf(item.Building);
						item.DevelopmentQueueIndex = num;
						if (num == 0)
						{
							item.IsCurrentActiveProject = true;
						}
						this.CurrentDevelopmentQueue.Add(item);
					}
					item.RefreshProductionText();
				}
			}
			Comparer<SettlementBuildingProjectVM> comparer = Comparer<SettlementBuildingProjectVM>.Create((SettlementBuildingProjectVM s1, SettlementBuildingProjectVM s2) => s1.DevelopmentQueueIndex.CompareTo(s2.DevelopmentQueueIndex));
			this.CurrentDevelopmentQueue.Sort(comparer);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00042210 File Offset: 0x00040410
		public void ExecuteChangeQueueOrder(SettlementBuildingProjectVM project, int index, string targetTag)
		{
			if (index == project.DevelopmentQueueIndex || targetTag != "CurrentDevelopmentQueue")
			{
				return;
			}
			this.LocalDevelopmentList.Remove(project.Building);
			if (index > project.DevelopmentQueueIndex)
			{
				this.LocalDevelopmentList.Insert(index - 1, project.Building);
			}
			else
			{
				this.LocalDevelopmentList.Insert(index, project.Building);
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
			Action onAnyChangeInQueue = this._onAnyChangeInQueue;
			if (onAnyChangeInQueue == null)
			{
				return;
			}
			onAnyChangeInQueue();
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00042293 File Offset: 0x00040493
		// (set) Token: 0x0600100A RID: 4106 RVA: 0x0004229B File Offset: 0x0004049B
		[DataSourceProperty]
		public string ProjectsText
		{
			get
			{
				return this._projectsText;
			}
			set
			{
				if (value != this._projectsText)
				{
					this._projectsText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProjectsText");
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x000422BE File Offset: 0x000404BE
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x000422C6 File Offset: 0x000404C6
		[DataSourceProperty]
		public string QueueText
		{
			get
			{
				return this._queueText;
			}
			set
			{
				if (value != this._queueText)
				{
					this._queueText = value;
					base.OnPropertyChangedWithValue<string>(value, "QueueText");
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x000422E9 File Offset: 0x000404E9
		// (set) Token: 0x0600100E RID: 4110 RVA: 0x000422F1 File Offset: 0x000404F1
		[DataSourceProperty]
		public string DailyDefaultsText
		{
			get
			{
				return this._dailyDefaultsText;
			}
			set
			{
				if (value != this._dailyDefaultsText)
				{
					this._dailyDefaultsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyDefaultsText");
				}
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x00042314 File Offset: 0x00040514
		// (set) Token: 0x06001010 RID: 4112 RVA: 0x0004231C File Offset: 0x0004051C
		[DataSourceProperty]
		public string DailyDefaultsExplanationText
		{
			get
			{
				return this._dailyDefaultsExplanationText;
			}
			set
			{
				if (value != this._dailyDefaultsExplanationText)
				{
					this._dailyDefaultsExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyDefaultsExplanationText");
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001011 RID: 4113 RVA: 0x0004233F File Offset: 0x0004053F
		// (set) Token: 0x06001012 RID: 4114 RVA: 0x00042347 File Offset: 0x00040547
		[DataSourceProperty]
		public SettlementProjectVM CurrentSelectedProject
		{
			get
			{
				return this._currentSelectedProject;
			}
			set
			{
				if (value != this._currentSelectedProject)
				{
					this._currentSelectedProject = value;
					base.OnPropertyChangedWithValue<SettlementProjectVM>(value, "CurrentSelectedProject");
					if (this._currentSelectedProject != null)
					{
						this._currentSelectedProject.RefreshProductionText();
					}
				}
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x00042378 File Offset: 0x00040578
		// (set) Token: 0x06001014 RID: 4116 RVA: 0x00042380 File Offset: 0x00040580
		[DataSourceProperty]
		public SettlementDailyProjectVM CurrentDailyDefault
		{
			get
			{
				return this._currentDailyDefault;
			}
			set
			{
				if (value != this._currentDailyDefault)
				{
					if (this._currentDailyDefault != null)
					{
						this._currentDailyDefault.IsDefault = false;
					}
					this._currentDailyDefault = value;
					base.OnPropertyChangedWithValue<SettlementDailyProjectVM>(value, "CurrentDailyDefault");
					if (this._currentDailyDefault != null)
					{
						this._currentDailyDefault.IsDefault = true;
					}
				}
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x000423D1 File Offset: 0x000405D1
		// (set) Token: 0x06001016 RID: 4118 RVA: 0x000423D9 File Offset: 0x000405D9
		[DataSourceProperty]
		public MBBindingList<SettlementBuildingProjectVM> AvailableProjects
		{
			get
			{
				return this._availableProjects;
			}
			set
			{
				if (value != this._availableProjects)
				{
					this._availableProjects = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementBuildingProjectVM>>(value, "AvailableProjects");
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x000423F7 File Offset: 0x000405F7
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x000423FF File Offset: 0x000405FF
		[DataSourceProperty]
		public MBBindingList<SettlementBuildingProjectVM> CurrentDevelopmentQueue
		{
			get
			{
				return this._currentDevelopmentQueue;
			}
			set
			{
				if (value != this._currentDevelopmentQueue)
				{
					this._currentDevelopmentQueue = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementBuildingProjectVM>>(value, "CurrentDevelopmentQueue");
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x0004241D File Offset: 0x0004061D
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x00042425 File Offset: 0x00040625
		[DataSourceProperty]
		public MBBindingList<SettlementDailyProjectVM> DailyDefaultList
		{
			get
			{
				return this._dailyDefaultList;
			}
			set
			{
				if (value != this._dailyDefaultList)
				{
					this._dailyDefaultList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementDailyProjectVM>>(value, "DailyDefaultList");
				}
			}
		}

		// Token: 0x0400074C RID: 1868
		private readonly Town _town;

		// Token: 0x0400074D RID: 1869
		private readonly Settlement _settlement;

		// Token: 0x0400074E RID: 1870
		private readonly Action _onAnyChangeInQueue;

		// Token: 0x04000750 RID: 1872
		private SettlementDailyProjectVM _currentDailyDefault;

		// Token: 0x04000751 RID: 1873
		private SettlementProjectVM _currentSelectedProject;

		// Token: 0x04000752 RID: 1874
		private MBBindingList<SettlementDailyProjectVM> _dailyDefaultList;

		// Token: 0x04000753 RID: 1875
		private MBBindingList<SettlementBuildingProjectVM> _currentDevelopmentQueue;

		// Token: 0x04000754 RID: 1876
		private MBBindingList<SettlementBuildingProjectVM> _availableProjects;

		// Token: 0x04000755 RID: 1877
		private string _projectsText;

		// Token: 0x04000756 RID: 1878
		private string _queueText;

		// Token: 0x04000757 RID: 1879
		private string _dailyDefaultsText;

		// Token: 0x04000758 RID: 1880
		private string _dailyDefaultsExplanationText;
	}
}
