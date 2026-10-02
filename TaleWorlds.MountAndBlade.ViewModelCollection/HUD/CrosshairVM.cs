using System;
using System.Collections.ObjectModel;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004D RID: 77
	public class CrosshairVM : ViewModel
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x00017E3D File Offset: 0x0001603D
		public CrosshairVM()
		{
			this.ReloadPhases = new MBBindingList<ReloadPhaseItemVM>();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00017E50 File Offset: 0x00016050
		public void SetProperties(double accuracy, double scale)
		{
			this.CrosshairAccuracy = accuracy;
			this.CrosshairScale = scale;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00017E60 File Offset: 0x00016060
		public void SetArrowProperties(double topArrowOpacity, double rightArrowOpacity, double bottomArrowOpacity, double leftArrowOpacity)
		{
			this.TopArrowOpacity = topArrowOpacity;
			this.BottomArrowOpacity = bottomArrowOpacity;
			this.RightArrowOpacity = rightArrowOpacity;
			this.LeftArrowOpacity = leftArrowOpacity;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00017E80 File Offset: 0x00016080
		public void SetReloadProperties(in StackArray.StackArray10FloatFloatTuple reloadPhases, int reloadPhaseCount)
		{
			if (reloadPhaseCount == 0)
			{
				this.IsReloadPhasesVisible = false;
			}
			else
			{
				for (int i = 0; i < reloadPhaseCount; i++)
				{
					StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
					if (stackArray10FloatFloatTuple[i].Item1 < 1f)
					{
						this.IsReloadPhasesVisible = true;
						break;
					}
				}
			}
			this.PopulateReloadPhases(in reloadPhases, reloadPhaseCount);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00017ED4 File Offset: 0x000160D4
		private void PopulateReloadPhases(in StackArray.StackArray10FloatFloatTuple reloadPhases, int reloadPhaseCount)
		{
			if (reloadPhaseCount != this.ReloadPhases.Count)
			{
				this.ReloadPhases.Clear();
				for (int i = 0; i < reloadPhaseCount; i++)
				{
					Collection<ReloadPhaseItemVM> reloadPhases2 = this.ReloadPhases;
					StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
					float item = stackArray10FloatFloatTuple[i].Item1;
					stackArray10FloatFloatTuple = reloadPhases;
					reloadPhases2.Add(new ReloadPhaseItemVM(item, stackArray10FloatFloatTuple[i].Item2));
				}
				return;
			}
			for (int j = 0; j < reloadPhaseCount; j++)
			{
				ReloadPhaseItemVM reloadPhaseItemVM = this.ReloadPhases[j];
				StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
				float item2 = stackArray10FloatFloatTuple[j].Item1;
				stackArray10FloatFloatTuple = reloadPhases;
				reloadPhaseItemVM.Update(item2, stackArray10FloatFloatTuple[j].Item2);
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00017F84 File Offset: 0x00016184
		public void ShowHitMarker(bool isVictimDead, bool isHumanoidHeadShot)
		{
			this.IsVictimDead = isVictimDead;
			this.IsHitMarkerVisible = false;
			this.IsHitMarkerVisible = true;
			this.IsHumanoidHeadshot = false;
			this.IsHumanoidHeadshot = isHumanoidHeadShot;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00017FA9 File Offset: 0x000161A9
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x00017FB1 File Offset: 0x000161B1
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00017FCF File Offset: 0x000161CF
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x00017FD7 File Offset: 0x000161D7
		[DataSourceProperty]
		public bool IsReloadPhasesVisible
		{
			get
			{
				return this._isReloadPhasesVisible;
			}
			set
			{
				if (value != this._isReloadPhasesVisible)
				{
					this._isReloadPhasesVisible = value;
					base.OnPropertyChangedWithValue(value, "IsReloadPhasesVisible");
				}
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00017FF5 File Offset: 0x000161F5
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x00017FFD File Offset: 0x000161FD
		[DataSourceProperty]
		public bool IsHitMarkerVisible
		{
			get
			{
				return this._isHitMarkerVisible;
			}
			set
			{
				if (value != this._isHitMarkerVisible)
				{
					this._isHitMarkerVisible = value;
					base.OnPropertyChangedWithValue(value, "IsHitMarkerVisible");
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001801B File Offset: 0x0001621B
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00018023 File Offset: 0x00016223
		[DataSourceProperty]
		public bool IsVictimDead
		{
			get
			{
				return this._isVictimDead;
			}
			set
			{
				if (value != this._isVictimDead)
				{
					this._isVictimDead = value;
					base.OnPropertyChangedWithValue(value, "IsVictimDead");
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00018041 File Offset: 0x00016241
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00018049 File Offset: 0x00016249
		[DataSourceProperty]
		public bool IsHumanoidHeadshot
		{
			get
			{
				return this._isHumanoidHeadshot;
			}
			set
			{
				if (value != this._isHumanoidHeadshot)
				{
					this._isHumanoidHeadshot = value;
					base.OnPropertyChangedWithValue(value, "IsHumanoidHeadshot");
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00018067 File Offset: 0x00016267
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0001806F File Offset: 0x0001626F
		[DataSourceProperty]
		public double TopArrowOpacity
		{
			get
			{
				return this._topArrowOpacity;
			}
			set
			{
				if (value != this._topArrowOpacity)
				{
					this._topArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "TopArrowOpacity");
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001808D File Offset: 0x0001628D
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00018095 File Offset: 0x00016295
		[DataSourceProperty]
		public MBBindingList<ReloadPhaseItemVM> ReloadPhases
		{
			get
			{
				return this._reloadPhases;
			}
			set
			{
				if (value != this._reloadPhases)
				{
					this._reloadPhases = value;
					base.OnPropertyChangedWithValue<MBBindingList<ReloadPhaseItemVM>>(value, "ReloadPhases");
				}
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000180B3 File Offset: 0x000162B3
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x000180BB File Offset: 0x000162BB
		[DataSourceProperty]
		public double BottomArrowOpacity
		{
			get
			{
				return this._bottomArrowOpacity;
			}
			set
			{
				if (value != this._bottomArrowOpacity)
				{
					this._bottomArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "BottomArrowOpacity");
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x000180D9 File Offset: 0x000162D9
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x000180E1 File Offset: 0x000162E1
		[DataSourceProperty]
		public double RightArrowOpacity
		{
			get
			{
				return this._rightArrowOpacity;
			}
			set
			{
				if (value != this._rightArrowOpacity)
				{
					this._rightArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "RightArrowOpacity");
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x000180FF File Offset: 0x000162FF
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00018107 File Offset: 0x00016307
		[DataSourceProperty]
		public double LeftArrowOpacity
		{
			get
			{
				return this._leftArrowOpacity;
			}
			set
			{
				if (value != this._leftArrowOpacity)
				{
					this._leftArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "LeftArrowOpacity");
				}
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00018125 File Offset: 0x00016325
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0001812D File Offset: 0x0001632D
		[DataSourceProperty]
		public bool IsTargetInvalid
		{
			get
			{
				return this._isTargetInvalid;
			}
			set
			{
				if (value != this._isTargetInvalid)
				{
					this._isTargetInvalid = value;
					base.OnPropertyChangedWithValue(value, "IsTargetInvalid");
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001814B File Offset: 0x0001634B
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00018153 File Offset: 0x00016353
		[DataSourceProperty]
		public double CrosshairAccuracy
		{
			get
			{
				return this._crosshairAccuracy;
			}
			set
			{
				if (value != this._crosshairAccuracy)
				{
					this._crosshairAccuracy = value;
					base.OnPropertyChangedWithValue(value, "CrosshairAccuracy");
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00018171 File Offset: 0x00016371
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x00018179 File Offset: 0x00016379
		[DataSourceProperty]
		public double CrosshairScale
		{
			get
			{
				return this._crosshairScale;
			}
			set
			{
				if (value != this._crosshairScale)
				{
					this._crosshairScale = value;
					base.OnPropertyChangedWithValue(value, "CrosshairScale");
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00018197 File Offset: 0x00016397
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001819F File Offset: 0x0001639F
		[DataSourceProperty]
		public int CrosshairType
		{
			get
			{
				return this._crosshairType;
			}
			set
			{
				if (value != this._crosshairType)
				{
					this._crosshairType = value;
					base.OnPropertyChangedWithValue(value, "CrosshairType");
				}
			}
		}

		// Token: 0x040002DE RID: 734
		private bool _isVisible;

		// Token: 0x040002DF RID: 735
		private bool _isReloadPhasesVisible;

		// Token: 0x040002E0 RID: 736
		private bool _isHitMarkerVisible;

		// Token: 0x040002E1 RID: 737
		private bool _isVictimDead;

		// Token: 0x040002E2 RID: 738
		private bool _isHumanoidHeadshot;

		// Token: 0x040002E3 RID: 739
		private bool _isTargetInvalid;

		// Token: 0x040002E4 RID: 740
		private MBBindingList<ReloadPhaseItemVM> _reloadPhases;

		// Token: 0x040002E5 RID: 741
		private double _crosshairAccuracy;

		// Token: 0x040002E6 RID: 742
		private double _crosshairScale;

		// Token: 0x040002E7 RID: 743
		private double _topArrowOpacity;

		// Token: 0x040002E8 RID: 744
		private double _bottomArrowOpacity;

		// Token: 0x040002E9 RID: 745
		private double _rightArrowOpacity;

		// Token: 0x040002EA RID: 746
		private double _leftArrowOpacity;

		// Token: 0x040002EB RID: 747
		private int _crosshairType;
	}
}
