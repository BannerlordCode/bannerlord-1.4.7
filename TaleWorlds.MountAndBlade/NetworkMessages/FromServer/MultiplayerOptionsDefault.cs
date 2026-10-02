using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005F RID: 95
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerOptionsDefault : GameNetworkMessage
	{
		// Token: 0x06000355 RID: 853 RVA: 0x00006624 File Offset: 0x00004824
		public MultiplayerOptionsDefault()
		{
			this._optionList = new List<MultiplayerOptions.OptionType>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				if (optionType.GetOptionProperty().Replication != MultiplayerOptionsProperty.ReplicationOccurrence.Never)
				{
					this._optionList.Add(optionType);
				}
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00006668 File Offset: 0x00004868
		protected override bool OnRead()
		{
			bool flag = true;
			for (int i = 0; i < this._optionList.Count; i++)
			{
				MultiplayerOptions.OptionType optionType = this._optionList[i];
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				switch (optionProperty.OptionValueType)
				{
				case MultiplayerOptions.OptionValueType.Bool:
				{
					bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
					optionType.SetValue(flag2, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				case MultiplayerOptions.OptionValueType.Integer:
				case MultiplayerOptions.OptionValueType.Enum:
				{
					int num = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true), ref flag);
					optionType.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				case MultiplayerOptions.OptionValueType.String:
				{
					string text = GameNetworkMessage.ReadStringFromPacket(ref flag);
					optionType.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				}
			}
			return flag;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00006718 File Offset: 0x00004918
		protected override void OnWrite()
		{
			for (int i = 0; i < this._optionList.Count; i++)
			{
				MultiplayerOptions.OptionType optionType = this._optionList[i];
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				switch (optionProperty.OptionValueType)
				{
				case MultiplayerOptions.OptionValueType.Bool:
					GameNetworkMessage.WriteBoolToPacket(optionType.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions));
					break;
				case MultiplayerOptions.OptionValueType.Integer:
				case MultiplayerOptions.OptionValueType.Enum:
					GameNetworkMessage.WriteIntToPacket(optionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions), new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true));
					break;
				case MultiplayerOptions.OptionValueType.String:
					GameNetworkMessage.WriteStringToPacket(optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions));
					break;
				}
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000067A8 File Offset: 0x000049A8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000067B0 File Offset: 0x000049B0
		protected override string OnGetLogFormat()
		{
			return "Receiving default multiplayer options.";
		}

		// Token: 0x040000A4 RID: 164
		private readonly List<MultiplayerOptions.OptionType> _optionList;
	}
}
