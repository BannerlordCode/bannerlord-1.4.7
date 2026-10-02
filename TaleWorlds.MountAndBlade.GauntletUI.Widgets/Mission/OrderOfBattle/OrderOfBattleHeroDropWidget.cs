using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F1 RID: 241
	public class OrderOfBattleHeroDropWidget : ButtonWidget
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x00021C30 File Offset: 0x0001FE30
		public OrderOfBattleHeroDropWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00021C39 File Offset: 0x0001FE39
		protected override bool OnPreviewDrop()
		{
			this.HandleSoundEvent();
			return true;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00021C42 File Offset: 0x0001FE42
		protected override void HandleClick()
		{
			this.HandleSoundEvent();
			base.HandleClick();
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00021C50 File Offset: 0x0001FE50
		private void HandleSoundEvent()
		{
			switch (this.FormationClass)
			{
			case 0:
				break;
			case 1:
				base.EventFired("Infantry", Array.Empty<object>());
				return;
			case 2:
				base.EventFired("Archers", Array.Empty<object>());
				return;
			case 3:
				base.EventFired("Cavalry", Array.Empty<object>());
				return;
			case 4:
				base.EventFired("HorseArchers", Array.Empty<object>());
				return;
			case 5:
				base.EventFired("InfantryArchers", Array.Empty<object>());
				return;
			case 6:
				base.EventFired("CavalryHorseArchers", Array.Empty<object>());
				break;
			default:
				return;
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00021CEC File Offset: 0x0001FEEC
		protected override bool OnPreviewDragHover()
		{
			return true;
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00021CEF File Offset: 0x0001FEEF
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x00021CF7 File Offset: 0x0001FEF7
		[DataSourceProperty]
		public int FormationClass
		{
			get
			{
				return this._formationClass;
			}
			set
			{
				if (value != this._formationClass)
				{
					this._formationClass = value;
					base.OnPropertyChanged(value, "FormationClass");
				}
			}
		}

		// Token: 0x04000593 RID: 1427
		private int _formationClass;
	}
}
