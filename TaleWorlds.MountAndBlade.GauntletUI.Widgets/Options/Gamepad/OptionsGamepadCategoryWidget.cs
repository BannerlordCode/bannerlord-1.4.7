using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007A RID: 122
	public class OptionsGamepadCategoryWidget : Widget
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000135B0 File Offset: 0x000117B0
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x000135B8 File Offset: 0x000117B8
		public Widget Playstation4LayoutParentWidget { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000135C1 File Offset: 0x000117C1
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000135C9 File Offset: 0x000117C9
		public Widget Playstation5LayoutParentWidget { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x000135D2 File Offset: 0x000117D2
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x000135DA File Offset: 0x000117DA
		public Widget XboxLayoutParentWidget { get; set; }

		// Token: 0x060006AD RID: 1709 RVA: 0x000135E3 File Offset: 0x000117E3
		public OptionsGamepadCategoryWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000135F3 File Offset: 0x000117F3
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				this.SetGamepadLayoutVisibility(this.CurrentGamepadType);
				this._initalized = true;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00013618 File Offset: 0x00011818
		private void SetGamepadLayoutVisibility(int gamepadType)
		{
			this.XboxLayoutParentWidget.IsVisible = false;
			this.Playstation4LayoutParentWidget.IsVisible = false;
			this.Playstation5LayoutParentWidget.IsVisible = false;
			if (gamepadType == 0)
			{
				this.XboxLayoutParentWidget.IsVisible = true;
				return;
			}
			if (gamepadType == 1)
			{
				this.Playstation4LayoutParentWidget.IsVisible = true;
				return;
			}
			if (gamepadType == 2)
			{
				this.Playstation5LayoutParentWidget.IsVisible = true;
				return;
			}
			this.XboxLayoutParentWidget.IsVisible = true;
			Debug.FailedAssert("This kind of gamepad is not visually supported", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Options\\Gamepad\\OptionsGamepadCategoryWidget.cs", "SetGamepadLayoutVisibility", 47);
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001369D File Offset: 0x0001189D
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x000136A5 File Offset: 0x000118A5
		public int CurrentGamepadType
		{
			get
			{
				return this._currentGamepadType;
			}
			set
			{
				if (this._currentGamepadType != value)
				{
					this._currentGamepadType = value;
				}
			}
		}

		// Token: 0x040002DD RID: 733
		private bool _initalized;

		// Token: 0x040002DE RID: 734
		private int _currentGamepadType = -1;
	}
}
