using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003D RID: 61
	public class MissionObjectiveMarkersVM : ViewModel
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x000150F6 File Offset: 0x000132F6
		public MissionObjectiveMarkersVM(MissionObjectiveLogic objectiveLogic, Camera missionCamera)
		{
			this.Targets = new MBBindingList<MissionObjectiveMarkerVM>();
			this._distanceComparer = new MissionObjectiveMarkersVM.MarkerDistanceComparer();
			this._objectiveLogic = objectiveLogic;
			this._missionCamera = missionCamera;
			this.IsEnabled = true;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00015129 File Offset: 0x00013329
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.IsEnabled)
			{
				this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM x)
				{
					x.RefreshValues();
				});
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00015163 File Offset: 0x00013363
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00015198 File Offset: 0x00013398
		public void UpdateObjective(MissionObjective objective)
		{
			if (this._latestObjective == objective)
			{
				return;
			}
			this._latestObjective = objective;
			this.Targets.Clear();
			if (this._latestObjective != null)
			{
				MBReadOnlyList<MissionObjectiveTarget> targetsCopy = this._latestObjective.GetTargetsCopy();
				if (targetsCopy != null)
				{
					for (int i = 0; i < targetsCopy.Count; i++)
					{
						MissionObjectiveMarkerVM missionObjectiveMarkerVM = new MissionObjectiveMarkerVM(targetsCopy[i]);
						this.Targets.Add(missionObjectiveMarkerVM);
					}
				}
			}
			this.RefreshValues();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00015208 File Offset: 0x00013408
		public void Tick(float dt)
		{
			this.UpdateTargets();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00015210 File Offset: 0x00013410
		private void UpdateTargets()
		{
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionObjectiveMarkerVM missionObjectiveMarkerVM = this.Targets[i];
				missionObjectiveMarkerVM.UpdateActiveState();
				if (missionObjectiveMarkerVM.IsActive)
				{
					missionObjectiveMarkerVM.UpdatePosition(this._missionCamera);
				}
			}
			this.Targets.Sort(this._distanceComparer);
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001526B File Offset: 0x0001346B
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00015273 File Offset: 0x00013473
		[DataSourceProperty]
		public MBBindingList<MissionObjectiveMarkerVM> Targets
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
					base.OnPropertyChangedWithValue<MBBindingList<MissionObjectiveMarkerVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00015291 File Offset: 0x00013491
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0001529C File Offset: 0x0001349C
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
					this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM t)
					{
						t.IsEnabled = value;
					});
				}
			}
		}

		// Token: 0x04000276 RID: 630
		private readonly Camera _missionCamera;

		// Token: 0x04000277 RID: 631
		private readonly MissionObjectiveMarkersVM.MarkerDistanceComparer _distanceComparer;

		// Token: 0x04000278 RID: 632
		private readonly MissionObjectiveLogic _objectiveLogic;

		// Token: 0x04000279 RID: 633
		private MissionObjective _latestObjective;

		// Token: 0x0400027A RID: 634
		private MBBindingList<MissionObjectiveMarkerVM> _targets;

		// Token: 0x0400027B RID: 635
		private bool _isEnabled;

		// Token: 0x020000E1 RID: 225
		private class MarkerDistanceComparer : IComparer<MissionObjectiveMarkerVM>
		{
			// Token: 0x06000CD6 RID: 3286 RVA: 0x0002A004 File Offset: 0x00028204
			public int Compare(MissionObjectiveMarkerVM x, MissionObjectiveMarkerVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
