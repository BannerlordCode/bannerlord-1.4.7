using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000062 RID: 98
	public class MissionFormationMarkerVM : ViewModel
	{
		// Token: 0x060007D2 RID: 2002 RVA: 0x0001B7B3 File Offset: 0x000199B3
		public MissionFormationMarkerVM(Mission mission)
		{
			this._mission = mission;
			this._comparer = new MissionFormationMarkerVM.FormationMarkerDistanceComparer();
			this.Targets = new MBBindingList<MissionFormationMarkerTargetVM>();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001B7D8 File Offset: 0x000199D8
		public void RefreshFormationMarkers()
		{
			IEnumerable<Formation> formationList = this._mission.Teams.SelectMany<Team, Formation>((Team t) => t.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0));
			using (IEnumerator<Formation> enumerator = formationList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation = enumerator.Current;
					if (this.Targets.All<MissionFormationMarkerTargetVM>((MissionFormationMarkerTargetVM t) => t.Formation != formation))
					{
						MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = new MissionFormationMarkerTargetVM(formation);
						this.Targets.Add(missionFormationMarkerTargetVM);
						missionFormationMarkerTargetVM.IsEnabled = this.IsEnabled;
						missionFormationMarkerTargetVM.IsFormationTargetRelevant = this.IsFormationTargetRelevant;
						missionFormationMarkerTargetVM.ShowDistanceTexts = this.ShowDistanceTexts;
					}
				}
			}
			if (formationList.CountQ<Formation>() < this.Targets.Count)
			{
				foreach (MissionFormationMarkerTargetVM missionFormationMarkerTargetVM2 in this.Targets.WhereQ<MissionFormationMarkerTargetVM>((MissionFormationMarkerTargetVM t) => !formationList.Contains(t.Formation)).ToList<MissionFormationMarkerTargetVM>())
				{
					this.Targets.Remove(missionFormationMarkerTargetVM2);
				}
			}
			this.Targets.Sort(this._comparer);
			foreach (MissionFormationMarkerTargetVM missionFormationMarkerTargetVM3 in this.Targets)
			{
				missionFormationMarkerTargetVM3.Refresh();
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001B984 File Offset: 0x00019B84
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0001B98C File Offset: 0x00019B8C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].IsEnabled = value;
					}
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001B9DD File Offset: 0x00019BDD
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0001B9E8 File Offset: 0x00019BE8
		[DataSourceProperty]
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (value != this._isFormationTargetRelevant)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsFormationTargetRelevant");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].IsFormationTargetRelevant = value;
					}
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001BA39 File Offset: 0x00019C39
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001BA44 File Offset: 0x00019C44
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowDistanceTexts");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].ShowDistanceTexts = value;
					}
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001BA95 File Offset: 0x00019C95
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0001BA9D File Offset: 0x00019C9D
		[DataSourceProperty]
		public MBBindingList<MissionFormationMarkerTargetVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionFormationMarkerTargetVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x04000379 RID: 889
		private readonly Mission _mission;

		// Token: 0x0400037A RID: 890
		private readonly MissionFormationMarkerVM.FormationMarkerDistanceComparer _comparer;

		// Token: 0x0400037B RID: 891
		private bool _isEnabled;

		// Token: 0x0400037C RID: 892
		private bool _isFormationTargetRelevant;

		// Token: 0x0400037D RID: 893
		private bool _showDistanceTexts;

		// Token: 0x0400037E RID: 894
		private MBBindingList<MissionFormationMarkerTargetVM> _targets;

		// Token: 0x020000F8 RID: 248
		public class FormationMarkerDistanceComparer : IComparer<MissionFormationMarkerTargetVM>
		{
			// Token: 0x06000D21 RID: 3361 RVA: 0x0002A368 File Offset: 0x00028568
			public int Compare(MissionFormationMarkerTargetVM x, MissionFormationMarkerTargetVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
