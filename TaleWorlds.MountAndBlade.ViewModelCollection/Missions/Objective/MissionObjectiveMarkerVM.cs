using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003C RID: 60
	public class MissionObjectiveMarkerVM : ViewModel
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x00014F22 File Offset: 0x00013122
		public MissionObjectiveMarkerVM(MissionObjectiveTarget target)
		{
			this.Target = target;
			this.IsEnabled = true;
			this.IsActive = true;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00014F3F File Offset: 0x0001313F
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ObjectiveName = this.Target.GetName().ToString();
			this.ObjectiveTypeId = "ActiveQuest";
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00014F68 File Offset: 0x00013168
		public void UpdateActiveState()
		{
			this.IsActive = this.Target.IsActive();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014F7C File Offset: 0x0001317C
		public void UpdatePosition(Camera missionCamera)
		{
			Vec3 globalPosition = this.Target.GetGlobalPosition();
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, globalPosition, ref num, ref num2, ref num3);
			if (num3 >= 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(globalPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-5000f, -5000f);
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00015003 File Offset: 0x00013203
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x0001500B File Offset: 0x0001320B
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00015029 File Offset: 0x00013229
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00015031 File Offset: 0x00013231
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
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001504F File Offset: 0x0001324F
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00015057 File Offset: 0x00013257
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00015075 File Offset: 0x00013275
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0001507D File Offset: 0x0001327D
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value != this._screenPosition)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x000150A0 File Offset: 0x000132A0
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x000150A8 File Offset: 0x000132A8
		[DataSourceProperty]
		public string ObjectiveTypeId
		{
			get
			{
				return this._objectiveTypeId;
			}
			set
			{
				if (value != this._objectiveTypeId)
				{
					this._objectiveTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveTypeId");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x000150CB File Offset: 0x000132CB
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x000150D3 File Offset: 0x000132D3
		[DataSourceProperty]
		public string ObjectiveName
		{
			get
			{
				return this._objectiveName;
			}
			set
			{
				if (value != this._objectiveName)
				{
					this._objectiveName = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveName");
				}
			}
		}

		// Token: 0x0400026F RID: 623
		public readonly MissionObjectiveTarget Target;

		// Token: 0x04000270 RID: 624
		private int _distance;

		// Token: 0x04000271 RID: 625
		private bool _isEnabled;

		// Token: 0x04000272 RID: 626
		private bool _isActive;

		// Token: 0x04000273 RID: 627
		private Vec2 _screenPosition;

		// Token: 0x04000274 RID: 628
		private string _objectiveTypeId;

		// Token: 0x04000275 RID: 629
		private string _objectiveName;
	}
}
