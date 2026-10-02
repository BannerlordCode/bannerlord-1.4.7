using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000020 RID: 32
	public class GamepadCursorMarkerWidget : BrushWidget
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00006ADE File Offset: 0x00004CDE
		public GamepadCursorMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00006AE7 File Offset: 0x00004CE7
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00006AEF File Offset: 0x00004CEF
		public bool FlipVisual
		{
			get
			{
				return this._flipVisual;
			}
			set
			{
				if (value != this._flipVisual)
				{
					this._flipVisual = value;
					base.Brush.DefaultLayer.HorizontalFlip = value;
				}
			}
		}

		// Token: 0x040000C7 RID: 199
		private bool _flipVisual;
	}
}
