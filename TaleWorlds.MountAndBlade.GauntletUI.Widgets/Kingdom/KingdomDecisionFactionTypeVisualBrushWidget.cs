using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000133 RID: 307
	public class KingdomDecisionFactionTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002BF81 File Offset: 0x0002A181
		public KingdomDecisionFactionTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002BF95 File Offset: 0x0002A195
		private void SetVisualState(string type)
		{
			this.RegisterBrushStatesOfWidget();
			this.SetState(type);
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0002BFA4 File Offset: 0x0002A1A4
		// (set) Token: 0x06000FFA RID: 4090 RVA: 0x0002BFAC File Offset: 0x0002A1AC
		[Editor(false)]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (this._factionName != value)
				{
					this._factionName = value;
					base.OnPropertyChanged<string>(value, "FactionName");
					if (value != null)
					{
						this.SetVisualState(value);
					}
				}
			}
		}

		// Token: 0x0400073F RID: 1855
		private string _factionName = "";
	}
}
