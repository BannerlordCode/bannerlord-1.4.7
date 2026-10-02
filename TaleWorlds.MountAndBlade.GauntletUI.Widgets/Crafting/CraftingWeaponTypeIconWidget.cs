using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016C RID: 364
	public class CraftingWeaponTypeIconWidget : Widget
	{
		// Token: 0x0600132E RID: 4910 RVA: 0x000343DB File Offset: 0x000325DB
		public CraftingWeaponTypeIconWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000343E4 File Offset: 0x000325E4
		private void UpdateIconVisual()
		{
			base.Sprite = base.Context.SpriteData.GetSprite("Crafting\\WeaponTypes\\" + this.WeaponType);
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001330 RID: 4912 RVA: 0x0003440C File Offset: 0x0003260C
		// (set) Token: 0x06001331 RID: 4913 RVA: 0x00034414 File Offset: 0x00032614
		[Editor(false)]
		public string WeaponType
		{
			get
			{
				return this._weaponType;
			}
			set
			{
				if (value != this._weaponType)
				{
					this._weaponType = value;
					this.UpdateIconVisual();
					base.OnPropertyChanged<string>(value, "WeaponType");
				}
			}
		}

		// Token: 0x040008B4 RID: 2228
		private string _weaponType;
	}
}
