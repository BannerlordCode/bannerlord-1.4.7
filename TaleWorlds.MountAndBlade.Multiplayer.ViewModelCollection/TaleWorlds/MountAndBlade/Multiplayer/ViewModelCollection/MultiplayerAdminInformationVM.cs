using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000F RID: 15
	public class MultiplayerAdminInformationVM : ViewModel
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004483 File Offset: 0x00002683
		public MultiplayerAdminInformationVM()
		{
			this.MessageQueue = new MBBindingList<StringItemWithActionVM>();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004498 File Offset: 0x00002698
		public void OnNewMessageReceived(string message)
		{
			StringItemWithActionVM stringItemWithActionVM = new StringItemWithActionVM(new Action<object>(this.ExecuteRemoveMessage), message, message);
			this.MessageQueue.Add(stringItemWithActionVM);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000044C8 File Offset: 0x000026C8
		private void ExecuteRemoveMessage(object messageToRemove)
		{
			int num = this.MessageQueue.FindIndex<StringItemWithActionVM>((StringItemWithActionVM m) => m.ActionText == messageToRemove as string);
			this.MessageQueue.RemoveAt(num);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004506 File Offset: 0x00002706
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000450E File Offset: 0x0000270E
		[DataSourceProperty]
		public MBBindingList<StringItemWithActionVM> MessageQueue
		{
			get
			{
				return this._messageQueue;
			}
			set
			{
				if (value != this._messageQueue)
				{
					this._messageQueue = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithActionVM>>(value, "MessageQueue");
				}
			}
		}

		// Token: 0x0400006D RID: 109
		private MBBindingList<StringItemWithActionVM> _messageQueue;
	}
}
