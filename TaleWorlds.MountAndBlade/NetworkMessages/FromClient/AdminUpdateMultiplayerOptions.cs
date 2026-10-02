using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000015 RID: 21
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminUpdateMultiplayerOptions : GameNetworkMessage
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002C9A File Offset: 0x00000E9A
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public List<AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo> Options { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002CAB File Offset: 0x00000EAB
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002CB3 File Offset: 0x00000EB3
		public int OptionCount { get; private set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00002CBC File Offset: 0x00000EBC
		public AdminUpdateMultiplayerOptions()
		{
			this.Options = new List<AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo>();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002CCF File Offset: 0x00000ECF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002CD7 File Offset: 0x00000ED7
		protected override string OnGetLogFormat()
		{
			return "Admin requesting update multiplayer options on server";
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002CE0 File Offset: 0x00000EE0
		protected override bool OnRead()
		{
			bool flag = true;
			this.OptionCount = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 43, true), ref flag);
			for (int i = 0; i < this.OptionCount; i++)
			{
				AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = this.ReadOptionInfoFromPacket(ref flag);
				this.Options.Add(adminMultiplayerOptionInfo);
			}
			return flag;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002D2C File Offset: 0x00000F2C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Options.Count, new CompressionInfo.Integer(0, 43, true));
			for (int i = 0; i < this.Options.Count; i++)
			{
				this.WriteOptionInfoToPacket(this.Options[i]);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002D7C File Offset: 0x00000F7C
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, bool value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, int value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002DCC File Offset: 0x00000FCC
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, string value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo ReadOptionInfoFromPacket(ref bool bufferReadValid)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 43, true), ref bufferReadValid);
			MultiplayerOptions.MultiplayerOptionsAccessMode multiplayerOptionsAccessMode = (MultiplayerOptions.MultiplayerOptionsAccessMode)GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 3, true), ref bufferReadValid);
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo((MultiplayerOptions.OptionType)num, multiplayerOptionsAccessMode);
			MultiplayerOptionsProperty optionProperty = ((MultiplayerOptions.OptionType)num).GetOptionProperty();
			switch (optionProperty.OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
			{
				bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(flag);
				break;
			}
			case MultiplayerOptions.OptionValueType.Integer:
			case MultiplayerOptions.OptionValueType.Enum:
			{
				int num2 = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true), ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(num2);
				break;
			}
			case MultiplayerOptions.OptionValueType.String:
			{
				string text = GameNetworkMessage.ReadStringFromPacket(ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(text);
				break;
			}
			}
			return adminMultiplayerOptionInfo;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002E94 File Offset: 0x00001094
		private void WriteOptionInfoToPacket(AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo optionInfo)
		{
			GameNetworkMessage.WriteIntToPacket((int)optionInfo.OptionType, new CompressionInfo.Integer(0, 43, true));
			GameNetworkMessage.WriteIntToPacket((int)optionInfo.AccessMode, new CompressionInfo.Integer(0, 3, true));
			MultiplayerOptionsProperty optionProperty = optionInfo.OptionType.GetOptionProperty();
			switch (optionProperty.OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				GameNetworkMessage.WriteBoolToPacket(optionInfo.BoolValue);
				return;
			case MultiplayerOptions.OptionValueType.Integer:
			case MultiplayerOptions.OptionValueType.Enum:
				GameNetworkMessage.WriteIntToPacket(optionInfo.IntValue, new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true));
				return;
			case MultiplayerOptions.OptionValueType.String:
				GameNetworkMessage.WriteStringToPacket(optionInfo.StringValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x02000407 RID: 1031
		public class AdminMultiplayerOptionInfo
		{
			// Token: 0x17000A0E RID: 2574
			// (get) Token: 0x060037D2 RID: 14290 RVA: 0x000E5950 File Offset: 0x000E3B50
			public MultiplayerOptions.OptionType OptionType { get; }

			// Token: 0x17000A0F RID: 2575
			// (get) Token: 0x060037D3 RID: 14291 RVA: 0x000E5958 File Offset: 0x000E3B58
			public MultiplayerOptions.MultiplayerOptionsAccessMode AccessMode { get; }

			// Token: 0x17000A10 RID: 2576
			// (get) Token: 0x060037D4 RID: 14292 RVA: 0x000E5960 File Offset: 0x000E3B60
			// (set) Token: 0x060037D5 RID: 14293 RVA: 0x000E5968 File Offset: 0x000E3B68
			public string StringValue { get; private set; }

			// Token: 0x17000A11 RID: 2577
			// (get) Token: 0x060037D6 RID: 14294 RVA: 0x000E5971 File Offset: 0x000E3B71
			// (set) Token: 0x060037D7 RID: 14295 RVA: 0x000E5979 File Offset: 0x000E3B79
			public bool BoolValue { get; private set; }

			// Token: 0x17000A12 RID: 2578
			// (get) Token: 0x060037D8 RID: 14296 RVA: 0x000E5982 File Offset: 0x000E3B82
			// (set) Token: 0x060037D9 RID: 14297 RVA: 0x000E598A File Offset: 0x000E3B8A
			public int IntValue { get; private set; }

			// Token: 0x060037DA RID: 14298 RVA: 0x000E5993 File Offset: 0x000E3B93
			public AdminMultiplayerOptionInfo(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode)
			{
				this.OptionType = optionType;
				this.AccessMode = accessMode;
			}

			// Token: 0x060037DB RID: 14299 RVA: 0x000E59A9 File Offset: 0x000E3BA9
			internal void SetValue(string value)
			{
				this.StringValue = value;
			}

			// Token: 0x060037DC RID: 14300 RVA: 0x000E59B2 File Offset: 0x000E3BB2
			internal void SetValue(bool value)
			{
				this.BoolValue = value;
			}

			// Token: 0x060037DD RID: 14301 RVA: 0x000E59BB File Offset: 0x000E3BBB
			internal void SetValue(int value)
			{
				this.IntValue = value;
			}
		}
	}
}
