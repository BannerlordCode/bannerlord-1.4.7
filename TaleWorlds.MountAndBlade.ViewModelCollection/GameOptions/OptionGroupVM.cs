using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000072 RID: 114
	public class OptionGroupVM : ViewModel
	{
		// Token: 0x060008EA RID: 2282 RVA: 0x0001DE2C File Offset: 0x0001C02C
		public OptionGroupVM(TextObject groupName, OptionsVM optionsBase, IEnumerable<IOptionData> optionsList)
		{
			this._groupName = groupName;
			this.Options = new MBBindingList<GenericOptionDataVM>();
			foreach (IOptionData optionData in optionsList)
			{
				this.Options.Add(optionsBase.GetOptionItem(optionData));
			}
			this.RefreshValues();
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001DEA0 File Offset: 0x0001C0A0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._groupName.ToString();
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
		internal List<IOptionData> GetManagedOptions()
		{
			List<IOptionData> list = new List<IOptionData>();
			foreach (GenericOptionDataVM genericOptionDataVM in this.Options)
			{
				if (!genericOptionDataVM.IsNative)
				{
					list.Add(genericOptionDataVM.GetOptionData());
				}
			}
			return list;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001DF54 File Offset: 0x0001C154
		internal bool IsChanged()
		{
			return this.Options.Any<GenericOptionDataVM>((GenericOptionDataVM o) => o.IsChanged());
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001DF80 File Offset: 0x0001C180
		internal void Cancel()
		{
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
			{
				o.Cancel();
			});
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001DFAC File Offset: 0x0001C1AC
		internal void InitializeDependentConfigs(Action<IOptionData, float> updateDependentConfigs)
		{
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
			{
				updateDependentConfigs(o.GetOptionData(), o.GetOptionData().GetValue(false));
			});
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0001DFDD File Offset: 0x0001C1DD
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0001DFE5 File Offset: 0x0001C1E5
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

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0001E008 File Offset: 0x0001C208
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0001E010 File Offset: 0x0001C210
		[DataSourceProperty]
		public MBBindingList<GenericOptionDataVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<GenericOptionDataVM>>(value, "Options");
				}
			}
		}

		// Token: 0x040003F2 RID: 1010
		private readonly TextObject _groupName;

		// Token: 0x040003F3 RID: 1011
		private const string ControllerIdentificationModifier = "_controller";

		// Token: 0x040003F4 RID: 1012
		private string _name;

		// Token: 0x040003F5 RID: 1013
		private MBBindingList<GenericOptionDataVM> _options;
	}
}
