using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011B RID: 283
	public class MapSiegeMachineButtonWidget : ButtonWidget
	{
		// Token: 0x06000EEE RID: 3822 RVA: 0x000293EA File Offset: 0x000275EA
		public MapSiegeMachineButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00029408 File Offset: 0x00027608
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ColoredImageWidget != null && !this._machineSpritesUpdated)
			{
				this.SetStylesSprite(this.ColoredImageWidget, "SPGeneral\\Siege\\" + this.MachineID);
				this._machineSpritesUpdated = true;
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00029444 File Offset: 0x00027644
		private void SetStylesSprite(Widget widget, string spriteName)
		{
			widget.Sprite = base.Context.SpriteData.GetSprite(spriteName);
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0002945D File Offset: 0x0002765D
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x00029465 File Offset: 0x00027665
		[Editor(false)]
		public Widget ColoredImageWidget
		{
			get
			{
				return this._coloredImageWidget;
			}
			set
			{
				if (value != this._coloredImageWidget)
				{
					this._coloredImageWidget = value;
					base.OnPropertyChanged<Widget>(value, "ColoredImageWidget");
				}
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00029483 File Offset: 0x00027683
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0002948B File Offset: 0x0002768B
		[Editor(false)]
		public bool IsDeploymentTarget
		{
			get
			{
				return this._isDeploymentTarget;
			}
			set
			{
				if (value != this._isDeploymentTarget)
				{
					this._isDeploymentTarget = value;
					base.OnPropertyChanged(value, "IsDeploymentTarget");
				}
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x000294A9 File Offset: 0x000276A9
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x000294B1 File Offset: 0x000276B1
		[Editor(false)]
		public string MachineID
		{
			get
			{
				return this._machineID;
			}
			set
			{
				if (value != this._machineID)
				{
					this._machineID = value;
					base.OnPropertyChanged<string>(value, "MachineID");
					this._machineSpritesUpdated = false;
				}
			}
		}

		// Token: 0x040006CC RID: 1740
		private Vec2 _orgClipSize = new Vec2(-1f, -1f);

		// Token: 0x040006CD RID: 1741
		private bool _machineSpritesUpdated;

		// Token: 0x040006CE RID: 1742
		private Widget _coloredImageWidget;

		// Token: 0x040006CF RID: 1743
		private bool _isDeploymentTarget;

		// Token: 0x040006D0 RID: 1744
		private string _machineID;
	}
}
