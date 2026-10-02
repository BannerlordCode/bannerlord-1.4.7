using System;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x02000018 RID: 24
	public class NameplateVM : ViewModel
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00009D28 File Offset: 0x00007F28
		// (set) Token: 0x06000241 RID: 577 RVA: 0x00009D30 File Offset: 0x00007F30
		public double Scale { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009D39 File Offset: 0x00007F39
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00009D41 File Offset: 0x00007F41
		public int NameplateOrder { get; set; }

		// Token: 0x06000245 RID: 581 RVA: 0x00009D52 File Offset: 0x00007F52
		protected void OnTutorialNotificationElementChanged(TutorialNotificationElementChangeEvent obj)
		{
			this.RefreshTutorialStatus(((obj != null) ? obj.NewNotificationElementID : null) ?? string.Empty);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009D6F File Offset: 0x00007F6F
		public virtual void RefreshDynamicProperties(bool forceUpdate)
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009D71 File Offset: 0x00007F71
		public virtual void RefreshPosition()
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009D73 File Offset: 0x00007F73
		public virtual void RefreshRelationStatus()
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009D75 File Offset: 0x00007F75
		public virtual void RefreshTutorialStatus(string newTutorialHighlightElementID)
		{
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009D77 File Offset: 0x00007F77
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009D7F File Offset: 0x00007F7F
		public string FactionColor
		{
			get
			{
				return this._factionColor;
			}
			set
			{
				if (value != this._factionColor)
				{
					this._factionColor = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionColor");
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009DA2 File Offset: 0x00007FA2
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009DAA File Offset: 0x00007FAA
		public float DistanceToCamera
		{
			get
			{
				return this._distanceToCamera;
			}
			set
			{
				if (value != this._distanceToCamera)
				{
					this._distanceToCamera = value;
					base.OnPropertyChangedWithValue(value, "DistanceToCamera");
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009DC8 File Offset: 0x00007FC8
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009DD0 File Offset: 0x00007FD0
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (value != this._isVisibleOnMap)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChangedWithValue(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009DEE File Offset: 0x00007FEE
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00009DF6 File Offset: 0x00007FF6
		public bool IsTargetedByTutorial
		{
			get
			{
				return this._isTargetedByTutorial;
			}
			set
			{
				if (value != this._isTargetedByTutorial)
				{
					this._isTargetedByTutorial = value;
					base.OnPropertyChangedWithValue(value, "IsTargetedByTutorial");
					base.OnPropertyChanged("ShouldShowFullName");
					base.OnPropertyChanged("IsTracked");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00009E2A File Offset: 0x0000802A
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00009E32 File Offset: 0x00008032
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00009E55 File Offset: 0x00008055
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00009E5D File Offset: 0x0000805D
		public bool CanParley
		{
			get
			{
				return this._canParley;
			}
			set
			{
				if (value != this._canParley)
				{
					this._canParley = value;
					base.OnPropertyChangedWithValue(value, "CanParley");
				}
			}
		}

		// Token: 0x04000106 RID: 262
		protected bool _bindIsTargetedByTutorial;

		// Token: 0x04000107 RID: 263
		private Vec2 _position;

		// Token: 0x04000108 RID: 264
		private bool _isVisibleOnMap;

		// Token: 0x04000109 RID: 265
		private string _factionColor;

		// Token: 0x0400010A RID: 266
		private bool _isTargetedByTutorial;

		// Token: 0x0400010B RID: 267
		private float _distanceToCamera;

		// Token: 0x0400010C RID: 268
		private bool _canParley;

		// Token: 0x0200007E RID: 126
		protected enum NameplateSize
		{
			// Token: 0x0400036C RID: 876
			Small,
			// Token: 0x0400036D RID: 877
			Normal,
			// Token: 0x0400036E RID: 878
			Big
		}
	}
}
