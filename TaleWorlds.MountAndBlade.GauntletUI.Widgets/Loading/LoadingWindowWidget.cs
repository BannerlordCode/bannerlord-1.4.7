using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Loading
{
	// Token: 0x0200012E RID: 302
	public class LoadingWindowWidget : Widget
	{
		// Token: 0x06000FC2 RID: 4034 RVA: 0x0002B78F File Offset: 0x0002998F
		public LoadingWindowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0002B798 File Offset: 0x00029998
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.AnimWidget != null && base.IsVisible && this.AnimWidget.IsVisible)
			{
				this.AnimWidget.PositionXOffset = MathF.PingPong(-200f, 200f, this._totalDt);
				this._totalDt += dt * 500f;
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0002B7FD File Offset: 0x000299FD
		private void UpdateStates()
		{
			base.IsVisible = this.IsActive;
			base.IsEnabled = this.IsActive;
			base.ParentWidget.IsVisible = this.IsActive;
			base.ParentWidget.IsEnabled = this.IsActive;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002B83C File Offset: 0x00029A3C
		private void UpdateImage(string imageName)
		{
			Sprite sprite = base.Context.SpriteData.GetSprite(imageName);
			if (sprite == null)
			{
				base.Sprite = base.Context.SpriteData.GetSprite("background_1");
				return;
			}
			base.Sprite = sprite;
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0002B881 File Offset: 0x00029A81
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0002B889 File Offset: 0x00029A89
		[Editor(false)]
		public Widget AnimWidget
		{
			get
			{
				return this._animWidget;
			}
			set
			{
				if (this._animWidget != value)
				{
					this._animWidget = value;
					base.OnPropertyChanged<Widget>(value, "AnimWidget");
				}
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0002B8A7 File Offset: 0x00029AA7
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x0002B8AF File Offset: 0x00029AAF
		[Editor(false)]
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
					base.OnPropertyChanged(value, "IsActive");
					this.UpdateStates();
				}
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0002B8D3 File Offset: 0x00029AD3
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0002B8DB File Offset: 0x00029ADB
		[Editor(false)]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (this._imageName != value)
				{
					this._imageName = value;
					base.OnPropertyChanged<string>(value, "ImageName");
					this.UpdateImage(value);
				}
			}
		}

		// Token: 0x04000726 RID: 1830
		private const string _defaultBackgroundSpriteData = "background_1";

		// Token: 0x04000727 RID: 1831
		private const float _animWidgetMaxOffset = 200f;

		// Token: 0x04000728 RID: 1832
		private float _totalDt;

		// Token: 0x04000729 RID: 1833
		private Widget _animWidget;

		// Token: 0x0400072A RID: 1834
		private bool _isActive;

		// Token: 0x0400072B RID: 1835
		private string _imageName;
	}
}
