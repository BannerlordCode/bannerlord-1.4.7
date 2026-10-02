using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications
{
	// Token: 0x02000022 RID: 34
	public class SettlementNotificationItemBaseVM : ViewModel
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000E124 File Offset: 0x0000C324
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000E12C File Offset: 0x0000C32C
		public int CreatedTick { get; set; }

		// Token: 0x06000346 RID: 838 RVA: 0x0000E135 File Offset: 0x0000C335
		public SettlementNotificationItemBaseVM(Action<SettlementNotificationItemBaseVM> onRemove, int createdTick)
		{
			this._onRemove = onRemove;
			this.RelationType = 0;
			this.CreatedTick = createdTick;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E152 File Offset: 0x0000C352
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000E160 File Offset: 0x0000C360
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000E168 File Offset: 0x0000C368
		public string CharacterName
		{
			get
			{
				return this._characterName;
			}
			set
			{
				if (value != this._characterName)
				{
					this._characterName = value;
					base.OnPropertyChangedWithValue<string>(value, "CharacterName");
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000E18B File Offset: 0x0000C38B
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000E193 File Offset: 0x0000C393
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (value != this._relationType)
				{
					this._relationType = value;
					base.OnPropertyChangedWithValue(value, "RelationType");
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000E1B1 File Offset: 0x0000C3B1
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000E1B9 File Offset: 0x0000C3B9
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000E1DC File Offset: 0x0000C3DC
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000E1E4 File Offset: 0x0000C3E4
		public CharacterImageIdentifierVM CharacterVisual
		{
			get
			{
				return this._characterVisual;
			}
			set
			{
				if (value != this._characterVisual)
				{
					this._characterVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "CharacterVisual");
				}
			}
		}

		// Token: 0x040001A9 RID: 425
		private readonly Action<SettlementNotificationItemBaseVM> _onRemove;

		// Token: 0x040001AB RID: 427
		private CharacterImageIdentifierVM _characterVisual;

		// Token: 0x040001AC RID: 428
		private string _text;

		// Token: 0x040001AD RID: 429
		private string _characterName;

		// Token: 0x040001AE RID: 430
		private int _relationType;
	}
}
