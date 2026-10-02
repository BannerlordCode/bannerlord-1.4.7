using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011C RID: 284
	public class MapSiegePOIBrushWidget : BrushWidget
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x000294DB File Offset: 0x000276DB
		private Color _fullColor
		{
			get
			{
				return new Color(0.2784314f, 0.9882353f, 0.44313726f, 1f);
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x000294F6 File Offset: 0x000276F6
		private Color _emptyColor
		{
			get
			{
				return new Color(0.9882353f, 0.2784314f, 0.2784314f, 1f);
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00029511 File Offset: 0x00027711
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x00029519 File Offset: 0x00027719
		public SliderWidget Slider { get; set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00029522 File Offset: 0x00027722
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0002952A File Offset: 0x0002772A
		public Brush ConstructionBrush { get; set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00029533 File Offset: 0x00027733
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x0002953B File Offset: 0x0002773B
		public Brush NormalBrush { get; set; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00029544 File Offset: 0x00027744
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0002954C File Offset: 0x0002774C
		public Vec2 ScreenPosition { get; set; }

		// Token: 0x06000F01 RID: 3841 RVA: 0x00029555 File Offset: 0x00027755
		public MapSiegePOIBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0002956C File Offset: 0x0002776C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.ScaledPositionXOffset = this.ScreenPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.ScreenPosition.y;
			float num = (float)(this.IsInVisibleRange ? 1 : 0);
			float num2 = MathF.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, num, dt * 10f, 1E-05f);
			this.SetGlobalAlphaRecursively(num2);
			base.IsEnabled = false;
			if (this._animState == MapSiegePOIBrushWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 5)
				{
					this._animState = MapSiegePOIBrushWidget.AnimState.Starting;
				}
			}
			else if (this._animState == MapSiegePOIBrushWidget.AnimState.Starting)
			{
				(this.Slider.Filler as BrushWidget).BrushRenderer.RestartAnimation();
				if (this.QueueIndex == 0)
				{
					this.HammerAnimWidget.BrushRenderer.RestartAnimation();
				}
				this._animState = MapSiegePOIBrushWidget.AnimState.Playing;
			}
			if (!this._isBrushChanged)
			{
				(this.Slider.Filler as BrushWidget).Brush = (this.IsConstructing ? this.ConstructionBrush : this.NormalBrush);
				this._animState = MapSiegePOIBrushWidget.AnimState.Start;
				this._isBrushChanged = true;
			}
			if (!this.IsConstructing)
			{
				this.UpdateColorOfSlider();
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000296A8 File Offset: 0x000278A8
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this.IsPOISelected = true;
			base.EventFired("OnSelection", Array.Empty<object>());
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000296C7 File Offset: 0x000278C7
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000296CF File Offset: 0x000278CF
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000296D8 File Offset: 0x000278D8
		private void SetMachineTypeIcon(int machineType)
		{
			string text = "SPGeneral\\MapSiege\\";
			switch (machineType)
			{
			case 0:
				text += "wall";
				break;
			case 1:
				text += "broken_wall";
				break;
			case 2:
				text += "ballista";
				break;
			case 3:
				text += "trebuchet";
				break;
			case 4:
				text += "ladder";
				break;
			case 5:
				text += "ram";
				break;
			case 6:
				text += "tower";
				break;
			case 7:
				text += "mangonel";
				break;
			default:
				text += "fallback";
				break;
			}
			this.MachineTypeIconWidget.Sprite = base.Context.SpriteData.GetSprite(text);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000297AC File Offset: 0x000279AC
		private void UpdateColorOfSlider()
		{
			(this.Slider.Filler as BrushWidget).Brush.Color = Color.Lerp(this._emptyColor, this._fullColor, this.Slider.ValueFloat / this.Slider.MaxValueFloat);
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x000297FB File Offset: 0x000279FB
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x00029803 File Offset: 0x00027A03
		public MapSiegeConstructionControllerWidget ConstructionControllerWidget
		{
			get
			{
				return this._constructionControllerWidget;
			}
			set
			{
				if (this._constructionControllerWidget != value)
				{
					this._constructionControllerWidget = value;
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x00029815 File Offset: 0x00027A15
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0002981D File Offset: 0x00027A1D
		public bool IsPlayerSidePOI
		{
			get
			{
				return this._isPlayerSidePOI;
			}
			set
			{
				if (this._isPlayerSidePOI != value)
				{
					this._isPlayerSidePOI = value;
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0002982F File Offset: 0x00027A2F
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00029837 File Offset: 0x00027A37
		public bool IsInVisibleRange
		{
			get
			{
				return this._isInVisibleRange;
			}
			set
			{
				if (this._isInVisibleRange != value)
				{
					this._isInVisibleRange = value;
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x00029849 File Offset: 0x00027A49
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x00029851 File Offset: 0x00027A51
		public bool IsPOISelected
		{
			get
			{
				return this._isPOISelected;
			}
			set
			{
				if (this._isPOISelected != value)
				{
					this._isPOISelected = value;
					this.ConstructionControllerWidget.SetCurrentPOIWidget(value ? this : null);
				}
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00029875 File Offset: 0x00027A75
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x0002987D File Offset: 0x00027A7D
		public bool IsConstructing
		{
			get
			{
				return this._isConstructing;
			}
			set
			{
				if (this._isConstructing != value)
				{
					this._isConstructing = value;
					this._isBrushChanged = false;
					this._animState = MapSiegePOIBrushWidget.AnimState.Idle;
				}
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002989D File Offset: 0x00027A9D
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x000298A5 File Offset: 0x00027AA5
		public int MachineType
		{
			get
			{
				return this._machineType;
			}
			set
			{
				if (this._machineType != value)
				{
					this._machineType = value;
					this.SetMachineTypeIcon(value);
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x000298BE File Offset: 0x00027ABE
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x000298C6 File Offset: 0x00027AC6
		public int QueueIndex
		{
			get
			{
				return this._queueIndex;
			}
			set
			{
				if (this._queueIndex != value)
				{
					this._queueIndex = value;
					this._animState = MapSiegePOIBrushWidget.AnimState.Start;
					this._tickCount = 0;
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000298E6 File Offset: 0x00027AE6
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x000298EE File Offset: 0x00027AEE
		public Widget MachineTypeIconWidget
		{
			get
			{
				return this._machineTypeIconWidget;
			}
			set
			{
				if (this._machineTypeIconWidget != value)
				{
					this._machineTypeIconWidget = value;
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x00029900 File Offset: 0x00027B00
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x00029908 File Offset: 0x00027B08
		public BrushWidget HammerAnimWidget
		{
			get
			{
				return this._hammerAnimWidget;
			}
			set
			{
				if (this._hammerAnimWidget != value)
				{
					this._hammerAnimWidget = value;
				}
			}
		}

		// Token: 0x040006D1 RID: 1745
		private MapSiegePOIBrushWidget.AnimState _animState;

		// Token: 0x040006D6 RID: 1750
		private bool _isBrushChanged;

		// Token: 0x040006D7 RID: 1751
		private int _tickCount;

		// Token: 0x040006D8 RID: 1752
		private bool _isConstructing;

		// Token: 0x040006D9 RID: 1753
		private bool _isPlayerSidePOI;

		// Token: 0x040006DA RID: 1754
		private bool _isInVisibleRange;

		// Token: 0x040006DB RID: 1755
		private bool _isPOISelected;

		// Token: 0x040006DC RID: 1756
		private BrushWidget _hammerAnimWidget;

		// Token: 0x040006DD RID: 1757
		private Widget _machineTypeIconWidget;

		// Token: 0x040006DE RID: 1758
		private int _machineType = -1;

		// Token: 0x040006DF RID: 1759
		private int _queueIndex = -1;

		// Token: 0x040006E0 RID: 1760
		private MapSiegeConstructionControllerWidget _constructionControllerWidget;

		// Token: 0x020001C9 RID: 457
		public enum AnimState
		{
			// Token: 0x04000A29 RID: 2601
			Idle,
			// Token: 0x04000A2A RID: 2602
			Start,
			// Token: 0x04000A2B RID: 2603
			Starting,
			// Token: 0x04000A2C RID: 2604
			Playing
		}
	}
}
