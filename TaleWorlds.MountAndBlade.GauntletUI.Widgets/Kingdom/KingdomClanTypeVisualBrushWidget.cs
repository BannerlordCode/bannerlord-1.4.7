using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000132 RID: 306
	public class KingdomClanTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002BEE2 File Offset: 0x0002A0E2
		public KingdomClanTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
		private void UpdateTypeVisual()
		{
			if (this.Type == 0)
			{
				this.SetState("Normal");
				return;
			}
			if (this.Type == 1)
			{
				this.SetState("Leader");
				return;
			}
			if (this.Type == 2)
			{
				this.SetState("Mercenary");
				return;
			}
			Debug.FailedAssert("This clan type is not defined in widget", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Kingdom\\KingdomClanTypeVisualBrushWidget.cs", "UpdateTypeVisual", 37);
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0002BF55 File Offset: 0x0002A155
		// (set) Token: 0x06000FF6 RID: 4086 RVA: 0x0002BF5D File Offset: 0x0002A15D
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
					this.UpdateTypeVisual();
				}
			}
		}

		// Token: 0x0400073E RID: 1854
		private int _type = -1;
	}
}
