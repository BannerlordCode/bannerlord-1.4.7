using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009A RID: 154
	public abstract class MissionMarkerTargetVM : ViewModel
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000F05 RID: 3845
		public abstract Vec3 WorldPosition { get; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000F06 RID: 3846
		protected abstract float HeightOffset { get; }

		// Token: 0x06000F07 RID: 3847 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
		public MissionMarkerTargetVM(MissionMarkerType markerType)
		{
			this.MissionMarkerType = markerType;
			this.MarkerType = (int)markerType;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0002E6BC File Offset: 0x0002C8BC
		public virtual void UpdateScreenPosition(Camera missionCamera)
		{
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			Vec3 worldPosition = this.WorldPosition;
			worldPosition.z += this.HeightOffset;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, worldPosition, ref num, ref num2, ref num3);
			if (num3 > 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(this.WorldPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-100f, -100f);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0002E754 File Offset: 0x0002C954
		protected void RefreshColor(uint color, uint color2)
		{
			if (color != 0U)
			{
				string text = color.ToString("X");
				char c = text[0];
				char c2 = text[1];
				text = text.Remove(0, 2);
				text = text.Add(c.ToString() + c2.ToString(), false);
				this.Color = "#" + text;
			}
			else
			{
				this.Color = "#FFFFFFFF";
			}
			if (color2 != 0U)
			{
				string text2 = color2.ToString("X");
				char c3 = text2[0];
				char c4 = text2[1];
				text2 = text2.Remove(0, 2);
				text2 = text2.Add(c3.ToString() + c4.ToString(), false);
				this.Color2 = "#" + text2;
				return;
			}
			this.Color2 = "#FFFFFFFF";
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0002E826 File Offset: 0x0002CA26
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0002E82E File Offset: 0x0002CA2E
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0002E869 File Offset: 0x0002CA69
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x0002E871 File Offset: 0x0002CA71
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002E894 File Offset: 0x0002CA94
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x0002E89C File Offset: 0x0002CA9C
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

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0002E8BA File Offset: 0x0002CABA
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x0002E8C2 File Offset: 0x0002CAC2
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

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002E8E0 File Offset: 0x0002CAE0
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x0002E8E8 File Offset: 0x0002CAE8
		[DataSourceProperty]
		public string Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (value != this._color)
				{
					this._color = value;
					base.OnPropertyChangedWithValue<string>(value, "Color");
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0002E90B File Offset: 0x0002CB0B
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x0002E913 File Offset: 0x0002CB13
		[DataSourceProperty]
		public string Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				if (value != this._color2)
				{
					this._color2 = value;
					base.OnPropertyChangedWithValue<string>(value, "Color2");
				}
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x0002E936 File Offset: 0x0002CB36
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x0002E93E File Offset: 0x0002CB3E
		[DataSourceProperty]
		public int MarkerType
		{
			get
			{
				return this._markerType;
			}
			set
			{
				if (value != this._markerType)
				{
					this._markerType = value;
					base.OnPropertyChangedWithValue(value, "MarkerType");
				}
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0002E95C File Offset: 0x0002CB5C
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x0002E964 File Offset: 0x0002CB64
		[DataSourceProperty]
		public string VisualState
		{
			get
			{
				return this._visualState;
			}
			set
			{
				if (value != this._visualState)
				{
					this._visualState = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualState");
				}
			}
		}

		// Token: 0x040006EE RID: 1774
		public readonly MissionMarkerType MissionMarkerType;

		// Token: 0x040006EF RID: 1775
		private Vec2 _screenPosition;

		// Token: 0x040006F0 RID: 1776
		private int _distance;

		// Token: 0x040006F1 RID: 1777
		private string _name;

		// Token: 0x040006F2 RID: 1778
		private bool _isEnabled;

		// Token: 0x040006F3 RID: 1779
		private string _color;

		// Token: 0x040006F4 RID: 1780
		private string _color2;

		// Token: 0x040006F5 RID: 1781
		private int _markerType;

		// Token: 0x040006F6 RID: 1782
		private string _visualState;
	}
}
