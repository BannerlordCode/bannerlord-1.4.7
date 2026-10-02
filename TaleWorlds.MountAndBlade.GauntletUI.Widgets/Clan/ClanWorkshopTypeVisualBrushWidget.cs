using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000179 RID: 377
	public class ClanWorkshopTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06001390 RID: 5008 RVA: 0x000352C3 File Offset: 0x000334C3
		public ClanWorkshopTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x000352D7 File Offset: 0x000334D7
		private void SetVisualState(string type)
		{
			this.RegisterBrushStatesOfWidget();
			this.SetState(type);
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x000352E6 File Offset: 0x000334E6
		// (set) Token: 0x06001393 RID: 5011 RVA: 0x000352EE File Offset: 0x000334EE
		[Editor(false)]
		public string WorkshopType
		{
			get
			{
				return this._workshopType;
			}
			set
			{
				if (this._workshopType != value)
				{
					this._workshopType = value;
					base.OnPropertyChanged<string>(value, "WorkshopType");
					this.SetVisualState(value);
				}
			}
		}

		// Token: 0x040008DA RID: 2266
		private string _workshopType = "";
	}
}
