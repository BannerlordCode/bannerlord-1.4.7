using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D5 RID: 213
	public class AgentHealthWidget : Widget
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0001E843 File Offset: 0x0001CA43
		public AgentHealthWidget(UIContext context)
			: base(context)
		{
			this._healthDrops = new List<AgentHealthWidget.HealthDropData>();
			this.CheckVisibility();
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0001E874 File Offset: 0x0001CA74
		private void CreateHealthDrop(Widget container, float previousHealthRatio, float currentHealthRatio)
		{
			float num = container.Size.X / base._scaleToUse;
			float num2 = Mathf.Ceil(num * (previousHealthRatio - currentHealthRatio));
			float num3 = Mathf.Floor(num * currentHealthRatio);
			BrushWidget brushWidget = new BrushWidget(base.Context);
			brushWidget.WidthSizePolicy = SizePolicy.Fixed;
			brushWidget.HeightSizePolicy = SizePolicy.Fixed;
			brushWidget.Brush = this.HealthDropBrush;
			brushWidget.SuggestedWidth = num2;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Left;
			brushWidget.VerticalAlignment = VerticalAlignment.Center;
			brushWidget.PositionXOffset = num3;
			brushWidget.ParentWidget = container;
			this._healthDrops.Add(new AgentHealthWidget.HealthDropData(brushWidget, this.AnimationDelay + this.AnimationDuration));
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0001E924 File Offset: 0x0001CB24
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.HealthBar != null)
			{
				this.HealthBar.MaxAmount = this.MaxHealth;
				this.HealthBar.InitialAmount = this.Health;
			}
			if (this.HealthDropContainer != null)
			{
				this.HandleHealthDrops(dt);
			}
			this.CheckVisibility();
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0001E978 File Offset: 0x0001CB78
		private void HandleHealthDrops(float dt)
		{
			for (int i = this._healthDrops.Count - 1; i >= 0; i--)
			{
				AgentHealthWidget.HealthDropData healthDropData = this._healthDrops[i];
				healthDropData.LifeTime -= dt;
				if (healthDropData.LifeTime <= 0f)
				{
					this.HealthDropContainer.RemoveChild(healthDropData.Widget);
					this._healthDrops.RemoveAt(i);
				}
				else
				{
					float num = Mathf.Min(1f, healthDropData.LifeTime / this.AnimationDuration);
					healthDropData.Widget.Brush.AlphaFactor = num;
				}
			}
			float num2 = ((this.MaxHealth != 0) ? ((float)this.Health / (float)this.MaxHealth) : 0f);
			num2 = MathF.Clamp(num2, 0f, 1f);
			if (num2 != this._previousHealthRatio)
			{
				if (num2 > this._previousHealthRatio)
				{
					for (int j = this._healthDrops.Count - 1; j >= 0; j--)
					{
						this.HealthDropContainer.RemoveChild(this._healthDrops[j].Widget);
						this._healthDrops.RemoveAt(j);
					}
				}
				else if (base.IsVisible)
				{
					this.CreateHealthDrop(this.HealthDropContainer, this._previousHealthRatio, num2);
				}
				this._previousHealthRatio = num2;
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0001EABC File Offset: 0x0001CCBC
		private void CheckVisibility()
		{
			bool flag = this.ShowHealthBar;
			if (flag)
			{
				flag = (float)this._health > 0f || this._healthDrops.Count > 0;
			}
			base.IsVisible = flag;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0001EAFA File Offset: 0x0001CCFA
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x0001EB02 File Offset: 0x0001CD02
		[Editor(false)]
		public int Health
		{
			get
			{
				return this._health;
			}
			set
			{
				if (this._health != value)
				{
					this._health = value;
					base.OnPropertyChanged(value, "Health");
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0001EB20 File Offset: 0x0001CD20
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x0001EB28 File Offset: 0x0001CD28
		[Editor(false)]
		public int MaxHealth
		{
			get
			{
				return this._maxHealth;
			}
			set
			{
				if (this._maxHealth != value)
				{
					this._maxHealth = value;
					base.OnPropertyChanged(value, "MaxHealth");
				}
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0001EB46 File Offset: 0x0001CD46
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x0001EB4E File Offset: 0x0001CD4E
		[Editor(false)]
		public FillBarWidget HealthBar
		{
			get
			{
				return this._healthBar;
			}
			set
			{
				if (this._healthBar != value)
				{
					this._healthBar = value;
					base.OnPropertyChanged<FillBarWidget>(value, "HealthBar");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0001EB6C File Offset: 0x0001CD6C
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x0001EB74 File Offset: 0x0001CD74
		[Editor(false)]
		public Widget HealthDropContainer
		{
			get
			{
				return this._healthDropContainer;
			}
			set
			{
				if (this._healthDropContainer != value)
				{
					this._healthDropContainer = value;
					base.OnPropertyChanged<Widget>(value, "HealthDropContainer");
				}
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0001EB92 File Offset: 0x0001CD92
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x0001EB9A File Offset: 0x0001CD9A
		[Editor(false)]
		public Brush HealthDropBrush
		{
			get
			{
				return this._healthDropBrush;
			}
			set
			{
				if (this._healthDropBrush != value)
				{
					this._healthDropBrush = value;
					base.OnPropertyChanged<Brush>(value, "HealthDropBrush");
				}
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
		[Editor(false)]
		public bool ShowHealthBar
		{
			get
			{
				return this._showHealthBar;
			}
			set
			{
				if (this._showHealthBar != value)
				{
					this._showHealthBar = value;
					base.OnPropertyChanged(value, "ShowHealthBar");
				}
			}
		}

		// Token: 0x040004EE RID: 1262
		private float AnimationDelay = 0.2f;

		// Token: 0x040004EF RID: 1263
		private float AnimationDuration = 0.8f;

		// Token: 0x040004F0 RID: 1264
		private float _previousHealthRatio;

		// Token: 0x040004F1 RID: 1265
		private List<AgentHealthWidget.HealthDropData> _healthDrops;

		// Token: 0x040004F2 RID: 1266
		private int _health;

		// Token: 0x040004F3 RID: 1267
		private int _maxHealth;

		// Token: 0x040004F4 RID: 1268
		private bool _showHealthBar;

		// Token: 0x040004F5 RID: 1269
		private FillBarWidget _healthBar;

		// Token: 0x040004F6 RID: 1270
		private Widget _healthDropContainer;

		// Token: 0x040004F7 RID: 1271
		private Brush _healthDropBrush;

		// Token: 0x020001BE RID: 446
		public class HealthDropData
		{
			// Token: 0x06001548 RID: 5448 RVA: 0x00039CA6 File Offset: 0x00037EA6
			public HealthDropData(BrushWidget widget, float lifeTime)
			{
				this.Widget = widget;
				this.LifeTime = lifeTime;
			}

			// Token: 0x04000A0C RID: 2572
			public BrushWidget Widget;

			// Token: 0x04000A0D RID: 2573
			public float LifeTime;
		}
	}
}
