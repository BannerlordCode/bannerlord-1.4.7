using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys
{
	// Token: 0x0200007B RID: 123
	public class AuxiliaryKeyOptionVM : KeyOptionVM
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x00021D04 File Offset: 0x0001FF04
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x00021D0C File Offset: 0x0001FF0C
		public HotKey CurrentHotKey { get; private set; }

		// Token: 0x060009BE RID: 2494 RVA: 0x00021D18 File Offset: 0x0001FF18
		public AuxiliaryKeyOptionVM(HotKey hotKey, Action<KeyOptionVM> onKeybindRequest, Action<AuxiliaryKeyOptionVM, InputKey> onKeySet, Func<AuxiliaryKeyOptionVM, string> getExtraInformation)
			: base(hotKey.GroupId, hotKey.Id, onKeybindRequest)
		{
			this._onKeySet = onKeySet;
			this._getExtraInformation = getExtraInformation;
			this.CurrentHotKey = hotKey;
			Key key;
			if (!Input.IsGamepadActive)
			{
				key = this.CurrentHotKey.Keys.FirstOrDefault<Key>((Key x) => !x.IsControllerInput);
			}
			else
			{
				key = this.CurrentHotKey.Keys.FirstOrDefault<Key>((Key x) => x.IsControllerInput);
			}
			base.Key = key;
			if (base.Key == null)
			{
				base.Key = new Key(InputKey.Invalid);
			}
			base.CurrentKey = new Key(base.Key.InputKey);
			this.RefreshValues();
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00021DF4 File Offset: 0x0001FFF4
		public override void RefreshValues()
		{
			base.RefreshValues();
			string text = this.CurrentHotKey.Id;
			TextObject textObject;
			if (Module.CurrentModule.GlobalTextManager.TryGetText("str_hotkey_name", this._groupId + "_" + this._id, out textObject))
			{
				text = textObject.ToString();
			}
			base.Name = text;
			string text2 = "";
			TextObject textObject2;
			if (Module.CurrentModule.GlobalTextManager.TryGetText("str_hotkey_description", this._groupId + "_" + this._id, out textObject2))
			{
				text2 = textObject2.ToString();
			}
			GameTextManager globalTextManager = Module.CurrentModule.GlobalTextManager;
			base.OptionValueText = globalTextManager.GetHotKeyGameTextFromKeyID(base.CurrentKey.ToString().ToLower()).ToString();
			string text3 = base.OptionValueText;
			foreach (HotKey.Modifiers modifiers in new List<HotKey.Modifiers>
			{
				HotKey.Modifiers.Alt,
				HotKey.Modifiers.Shift,
				HotKey.Modifiers.Control
			})
			{
				if (this.CurrentHotKey.HasModifier(modifiers))
				{
					MBTextManager.SetTextVariable("KEY", text3, false);
					MBTextManager.SetTextVariable("MODIFIER", globalTextManager.GetHotKeyGameTextFromKeyID("any" + modifiers.ToString().ToLower()).ToString(), false);
					text3 = globalTextManager.FindText("str_hot_key_with_modifier", null).ToString();
				}
			}
			TextObject textObject3 = new TextObject("{=ol0rBSrb}{STR1}{newline}{STR2}", null);
			textObject3.SetTextVariable("STR1", text3);
			textObject3.SetTextVariable("STR2", text2);
			textObject3.SetTextVariable("newline", "\n \n");
			base.Description = textObject3.ToString();
			Func<AuxiliaryKeyOptionVM, string> getExtraInformation = this._getExtraInformation;
			base.ExtraInformationText = ((getExtraInformation != null) ? getExtraInformation(this) : null);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00021FDC File Offset: 0x000201DC
		private void ExecuteKeybindRequest()
		{
			this._onKeybindRequest(this);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00021FEA File Offset: 0x000201EA
		public override void Set(InputKey newKey)
		{
			this._onKeySet(this, newKey);
			this.RefreshValues();
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00022000 File Offset: 0x00020200
		public override void Update()
		{
			Key key;
			if (!Input.IsGamepadActive)
			{
				key = this.CurrentHotKey.Keys.FirstOrDefault<Key>((Key x) => !x.IsControllerInput);
			}
			else
			{
				key = this.CurrentHotKey.Keys.FirstOrDefault<Key>((Key x) => x.IsControllerInput);
			}
			base.Key = key;
			if (base.Key == null)
			{
				base.Key = new Key(InputKey.Invalid);
			}
			base.CurrentKey = new Key(base.Key.InputKey);
			this.UpdateIsChanged();
			this.RefreshValues();
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000220B6 File Offset: 0x000202B6
		public override void OnDone()
		{
			base.Key.ChangeKey(base.CurrentKey.InputKey);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000220CE File Offset: 0x000202CE
		internal override void UpdateIsChanged()
		{
			base.IsChanged = base.CurrentKey != base.Key;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000220E7 File Offset: 0x000202E7
		public override void ExecuteRevert()
		{
			this.Set(base.Key.InputKey);
		}

		// Token: 0x0400044F RID: 1103
		private readonly Action<AuxiliaryKeyOptionVM, InputKey> _onKeySet;

		// Token: 0x04000450 RID: 1104
		private readonly Func<AuxiliaryKeyOptionVM, string> _getExtraInformation;
	}
}
