using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EF RID: 239
	public class OrderOfBattleHeroButtonWidget : ButtonWidget
	{
		// Token: 0x06000C49 RID: 3145 RVA: 0x0002188A File Offset: 0x0001FA8A
		public OrderOfBattleHeroButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0002189C File Offset: 0x0001FA9C
		private void UpdateMainHeroHueFactor()
		{
			foreach (BrushLayer brushLayer in base.Brush.Layers)
			{
				brushLayer.HueFactor = (float)(this.IsMainHero ? this.MainHeroHueFactor : 0);
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00021904 File Offset: 0x0001FB04
		private void UpdateMainHeroAcceptEvents()
		{
			base.DoNotAcceptEvents = this.IsMainHero && !this.CanMainHeroAcceptEvents;
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00021920 File Offset: 0x0001FB20
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x00021928 File Offset: 0x0001FB28
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChanged(value, "IsMainHero");
					this.UpdateMainHeroHueFactor();
					this.UpdateMainHeroAcceptEvents();
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00021952 File Offset: 0x0001FB52
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0002195A File Offset: 0x0001FB5A
		public int MainHeroHueFactor
		{
			get
			{
				return this._mainHeroHueFactor;
			}
			set
			{
				if (value != this._mainHeroHueFactor)
				{
					this._mainHeroHueFactor = value;
					base.OnPropertyChanged(value, "MainHeroHueFactor");
					this.UpdateMainHeroHueFactor();
				}
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0002197E File Offset: 0x0001FB7E
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00021986 File Offset: 0x0001FB86
		public bool CanMainHeroAcceptEvents
		{
			get
			{
				return this._canMainHeroAcceptEvents;
			}
			set
			{
				if (value != this._canMainHeroAcceptEvents)
				{
					this._canMainHeroAcceptEvents = value;
					base.OnPropertyChanged(value, "CanMainHeroAcceptEvents");
					this.UpdateMainHeroAcceptEvents();
				}
			}
		}

		// Token: 0x0400058B RID: 1419
		private bool _isMainHero;

		// Token: 0x0400058C RID: 1420
		private int _mainHeroHueFactor;

		// Token: 0x0400058D RID: 1421
		private bool _canMainHeroAcceptEvents = true;
	}
}
