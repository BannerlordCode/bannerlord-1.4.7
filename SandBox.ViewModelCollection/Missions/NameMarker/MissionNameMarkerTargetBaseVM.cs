using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000033 RID: 51
	public abstract class MissionNameMarkerTargetBaseVM : ViewModel
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00010292 File Offset: 0x0000E492
		public MissionNameMarkerTargetBaseVM()
		{
			this.Quests = new MBBindingList<QuestMarkerVM>();
		}

		// Token: 0x060003D8 RID: 984
		public abstract void UpdatePosition(Camera missionCamera);

		// Token: 0x060003D9 RID: 985
		public abstract bool Equals(MissionNameMarkerTargetBaseVM other);

		// Token: 0x060003DA RID: 986
		protected abstract TextObject GetName();

		// Token: 0x060003DB RID: 987 RVA: 0x000102BB File Offset: 0x0000E4BB
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.GetName().ToString();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000102D4 File Offset: 0x0000E4D4
		protected void UpdatePositionWith(Camera missionCamera, Vec3 worldPosition)
		{
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, worldPosition, ref num, ref num2, ref num3);
			if (num3 > 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(worldPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-500f, -500f);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001034E File Offset: 0x0000E54E
		public void SetEnabledState(bool enabled)
		{
			this.IsEnabled = this.IsPersistent || enabled;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001035E File Offset: 0x0000E55E
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00010366 File Offset: 0x0000E566
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00010384 File Offset: 0x0000E584
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0001038C File Offset: 0x0000E58C
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000103C7 File Offset: 0x0000E5C7
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x000103CF File Offset: 0x0000E5CF
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x000103F2 File Offset: 0x0000E5F2
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x000103FA File Offset: 0x0000E5FA
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001041D File Offset: 0x0000E61D
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x00010425 File Offset: 0x0000E625
		[DataSourceProperty]
		public string NameType
		{
			get
			{
				return this._nameType;
			}
			set
			{
				if (value != this._nameType)
				{
					this._nameType = value;
					base.OnPropertyChangedWithValue<string>(value, "NameType");
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00010448 File Offset: 0x0000E648
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x00010450 File Offset: 0x0000E650
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001046E File Offset: 0x0000E66E
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00010476 File Offset: 0x0000E676
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00010494 File Offset: 0x0000E694
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0001049C File Offset: 0x0000E69C
		[DataSourceProperty]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChangedWithValue(value, "IsTracked");
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000104BA File Offset: 0x0000E6BA
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x000104C2 File Offset: 0x0000E6C2
		[DataSourceProperty]
		public bool IsQuestMainStory
		{
			get
			{
				return this._isQuestMainStory;
			}
			set
			{
				if (value != this._isQuestMainStory)
				{
					this._isQuestMainStory = value;
					base.OnPropertyChangedWithValue(value, "IsQuestMainStory");
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000104E0 File Offset: 0x0000E6E0
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x000104E8 File Offset: 0x0000E6E8
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00010506 File Offset: 0x0000E706
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0001050E File Offset: 0x0000E70E
		[DataSourceProperty]
		public bool IsFriendly
		{
			get
			{
				return this._isFriendly;
			}
			set
			{
				if (value != this._isFriendly)
				{
					this._isFriendly = value;
					base.OnPropertyChangedWithValue(value, "IsFriendly");
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001052C File Offset: 0x0000E72C
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00010534 File Offset: 0x0000E734
		[DataSourceProperty]
		public bool IsPersistent
		{
			get
			{
				return this._isPersistent;
			}
			set
			{
				if (value != this._isPersistent)
				{
					this._isPersistent = value;
					base.OnPropertyChangedWithValue(value, "IsPersistent");
					if (this.IsPersistent)
					{
						this.SetEnabledState(true);
						return;
					}
					if (!this.IsEnabled)
					{
						this.SetEnabledState(false);
					}
				}
			}
		}

		// Token: 0x04000202 RID: 514
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000203 RID: 515
		private Vec2 _screenPosition;

		// Token: 0x04000204 RID: 516
		private int _distance;

		// Token: 0x04000205 RID: 517
		private string _name;

		// Token: 0x04000206 RID: 518
		private string _iconType = string.Empty;

		// Token: 0x04000207 RID: 519
		private string _nameType = string.Empty;

		// Token: 0x04000208 RID: 520
		private bool _isEnabled;

		// Token: 0x04000209 RID: 521
		private bool _isTracked;

		// Token: 0x0400020A RID: 522
		private bool _isQuestMainStory;

		// Token: 0x0400020B RID: 523
		private bool _isEnemy;

		// Token: 0x0400020C RID: 524
		private bool _isFriendly;

		// Token: 0x0400020D RID: 525
		private bool _isPersistent;
	}
}
