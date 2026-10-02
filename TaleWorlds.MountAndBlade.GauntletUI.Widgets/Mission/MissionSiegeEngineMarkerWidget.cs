using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E2 RID: 226
	public class MissionSiegeEngineMarkerWidget : Widget
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x000206A1 File Offset: 0x0001E8A1
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x000206A9 File Offset: 0x0001E8A9
		public SliderWidget Slider { get; set; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x000206B2 File Offset: 0x0001E8B2
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x000206BA File Offset: 0x0001E8BA
		public BrushWidget MachineIconParent { get; set; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x000206C3 File Offset: 0x0001E8C3
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x000206CB File Offset: 0x0001E8CB
		public Brush EnemyBrush { get; set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x000206D4 File Offset: 0x0001E8D4
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x000206DC File Offset: 0x0001E8DC
		public Brush AllyBrush { get; set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x000206E5 File Offset: 0x0001E8E5
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x000206ED File Offset: 0x0001E8ED
		public Vec2 ScreenPosition { get; set; }

		// Token: 0x06000BAE RID: 2990 RVA: 0x000206F8 File Offset: 0x0001E8F8
		public MissionSiegeEngineMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002074C File Offset: 0x0001E94C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.ScaledPositionXOffset = this.ScreenPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.ScreenPosition.y;
			float num = (this.IsActive ? 0.65f : 0f);
			float num2 = MathF.Lerp(base.AlphaFactor, num, dt * 10f, 1E-05f);
			this.SetGlobalAlphaRecursively(num2);
			if (!this._isBrushChanged)
			{
				this.MachineIconParent.Brush = (this.IsEnemy ? this.EnemyBrush : this.AllyBrush);
				this._isBrushChanged = true;
			}
			this.UpdateColorOfSlider();
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00020800 File Offset: 0x0001EA00
		private void SetMachineTypeIcon(string machineType)
		{
			string text = "SPGeneral\\MapSiege\\" + machineType;
			this.MachineTypeIconWidget.Sprite = base.Context.SpriteData.GetSprite(text);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00020838 File Offset: 0x0001EA38
		private void UpdateColorOfSlider()
		{
			(this.Slider.Filler as BrushWidget).Brush.Color = Color.Lerp(this._emptyColor, this._fullColor, this.Slider.ValueFloat / this.Slider.MaxValueFloat);
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00020887 File Offset: 0x0001EA87
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x0002088F File Offset: 0x0001EA8F
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (this._isEnemy != value)
				{
					this._isEnemy = value;
				}
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x000208A1 File Offset: 0x0001EAA1
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x000208A9 File Offset: 0x0001EAA9
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
				}
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x000208BB File Offset: 0x0001EABB
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x000208C3 File Offset: 0x0001EAC3
		public string EngineType
		{
			get
			{
				return this._engineType;
			}
			set
			{
				if (this._engineType != value)
				{
					this._engineType = value;
					this.SetMachineTypeIcon(value);
				}
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x000208E1 File Offset: 0x0001EAE1
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x000208E9 File Offset: 0x0001EAE9
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

		// Token: 0x04000540 RID: 1344
		private Color _fullColor = new Color(0.2784314f, 0.9882353f, 0.44313726f, 1f);

		// Token: 0x04000541 RID: 1345
		private Color _emptyColor = new Color(0.9882353f, 0.2784314f, 0.2784314f, 1f);

		// Token: 0x04000547 RID: 1351
		private bool _isBrushChanged;

		// Token: 0x04000548 RID: 1352
		private bool _isEnemy;

		// Token: 0x04000549 RID: 1353
		private bool _isActive;

		// Token: 0x0400054A RID: 1354
		private Widget _machineTypeIconWidget;

		// Token: 0x0400054B RID: 1355
		private string _engineType;
	}
}
