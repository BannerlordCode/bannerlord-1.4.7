using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AD RID: 429
	[ScriptingInterfaceBase]
	internal interface IMBNetwork
	{
		// Token: 0x0600185F RID: 6239
		[EngineMethod("get_multiplayer_disabled", false, null, false)]
		bool GetMultiplayerDisabled();

		// Token: 0x06001860 RID: 6240
		[EngineMethod("is_dedicated_server", false, null, false)]
		bool IsDedicatedServer();

		// Token: 0x06001861 RID: 6241
		[EngineMethod("initialize_server_side", false, null, false)]
		void InitializeServerSide(int port);

		// Token: 0x06001862 RID: 6242
		[EngineMethod("initialize_client_side", false, null, false)]
		void InitializeClientSide(string serverAddress, int port, int sessionKey, int playerIndex);

		// Token: 0x06001863 RID: 6243
		[EngineMethod("terminate_server_side", false, null, false)]
		void TerminateServerSide();

		// Token: 0x06001864 RID: 6244
		[EngineMethod("terminate_client_side", false, null, false)]
		void TerminateClientSide();

		// Token: 0x06001865 RID: 6245
		[EngineMethod("server_ping", false, null, false)]
		void ServerPing(string serverAddress, int port);

		// Token: 0x06001866 RID: 6246
		[EngineMethod("add_peer_to_disconnect", false, null, false)]
		void AddPeerToDisconnect(int peer);

		// Token: 0x06001867 RID: 6247
		[EngineMethod("prepare_new_udp_session", false, null, false)]
		void PrepareNewUdpSession(int player, int sessionKey);

		// Token: 0x06001868 RID: 6248
		[EngineMethod("get_active_udp_sessions_ip_address", false, null, false)]
		string GetActiveUdpSessionsIpAddress();

		// Token: 0x06001869 RID: 6249
		[EngineMethod("can_add_new_players_on_server", false, null, false)]
		bool CanAddNewPlayersOnServer(int numPlayers);

		// Token: 0x0600186A RID: 6250
		[EngineMethod("add_new_player_on_server", false, null, false)]
		int AddNewPlayerOnServer(bool serverPlayer);

		// Token: 0x0600186B RID: 6251
		[EngineMethod("add_new_bot_on_server", false, null, false)]
		int AddNewBotOnServer();

		// Token: 0x0600186C RID: 6252
		[EngineMethod("remove_bot_on_server", false, null, false)]
		void RemoveBotOnServer(int botPlayerIndex);

		// Token: 0x0600186D RID: 6253
		[EngineMethod("reset_mission_data", false, null, false)]
		void ResetMissionData();

		// Token: 0x0600186E RID: 6254
		[EngineMethod("begin_broadcast_module_event", false, null, false)]
		void BeginBroadcastModuleEvent();

		// Token: 0x0600186F RID: 6255
		[EngineMethod("end_broadcast_module_event", false, null, false)]
		void EndBroadcastModuleEvent(int broadcastFlags, int targetPlayer, bool isReliable);

		// Token: 0x06001870 RID: 6256
		[EngineMethod("elapsed_time_since_last_udp_packet_arrived", false, null, false)]
		double ElapsedTimeSinceLastUdpPacketArrived();

		// Token: 0x06001871 RID: 6257
		[EngineMethod("begin_module_event_as_client", false, null, false)]
		void BeginModuleEventAsClient(bool isReliable);

		// Token: 0x06001872 RID: 6258
		[EngineMethod("end_module_event_as_client", false, null, false)]
		void EndModuleEventAsClient(bool isReliable);

		// Token: 0x06001873 RID: 6259
		[EngineMethod("read_int_from_packet", false, null, false)]
		bool ReadIntFromPacket(ref CompressionInfo.Integer compressionInfo, out int output);

		// Token: 0x06001874 RID: 6260
		[EngineMethod("read_uint_from_packet", false, null, false)]
		bool ReadUintFromPacket(ref CompressionInfo.UnsignedInteger compressionInfo, out uint output);

		// Token: 0x06001875 RID: 6261
		[EngineMethod("read_long_from_packet", false, null, false)]
		bool ReadLongFromPacket(ref CompressionInfo.LongInteger compressionInfo, out long output);

		// Token: 0x06001876 RID: 6262
		[EngineMethod("read_ulong_from_packet", false, null, false)]
		bool ReadUlongFromPacket(ref CompressionInfo.UnsignedLongInteger compressionInfo, out ulong output);

		// Token: 0x06001877 RID: 6263
		[EngineMethod("read_float_from_packet", false, null, false)]
		bool ReadFloatFromPacket(ref CompressionInfo.Float compressionInfo, out float output);

		// Token: 0x06001878 RID: 6264
		[EngineMethod("read_string_from_packet", false, null, false)]
		string ReadStringFromPacket(ref bool bufferReadValid);

		// Token: 0x06001879 RID: 6265
		[EngineMethod("write_int_to_packet", false, null, false)]
		void WriteIntToPacket(int value, ref CompressionInfo.Integer compressionInfo);

		// Token: 0x0600187A RID: 6266
		[EngineMethod("write_uint_to_packet", false, null, false)]
		void WriteUintToPacket(uint value, ref CompressionInfo.UnsignedInteger compressionInfo);

		// Token: 0x0600187B RID: 6267
		[EngineMethod("write_long_to_packet", false, null, false)]
		void WriteLongToPacket(long value, ref CompressionInfo.LongInteger compressionInfo);

		// Token: 0x0600187C RID: 6268
		[EngineMethod("write_ulong_to_packet", false, null, false)]
		void WriteUlongToPacket(ulong value, ref CompressionInfo.UnsignedLongInteger compressionInfo);

		// Token: 0x0600187D RID: 6269
		[EngineMethod("write_float_to_packet", false, null, false)]
		void WriteFloatToPacket(float value, ref CompressionInfo.Float compressionInfo);

		// Token: 0x0600187E RID: 6270
		[EngineMethod("write_string_to_packet", false, null, false)]
		void WriteStringToPacket(string value);

		// Token: 0x0600187F RID: 6271
		[EngineMethod("read_byte_array_from_packet", false, null, false)]
		int ReadByteArrayFromPacket(byte[] buffer, int offset, int bufferCapacity, ref bool bufferReadValid);

		// Token: 0x06001880 RID: 6272
		[EngineMethod("write_byte_array_to_packet", false, null, false)]
		void WriteByteArrayToPacket(byte[] value, int offset, int size);

		// Token: 0x06001881 RID: 6273
		[EngineMethod("set_server_bandwidth_limit_in_mbps", false, null, false)]
		void SetServerBandwidthLimitInMbps(double value);

		// Token: 0x06001882 RID: 6274
		[EngineMethod("set_server_tick_rate", false, null, false)]
		void SetServerTickRate(double value);

		// Token: 0x06001883 RID: 6275
		[EngineMethod("reset_debug_variables", false, null, false)]
		void ResetDebugVariables();

		// Token: 0x06001884 RID: 6276
		[EngineMethod("print_debug_stats", false, null, false)]
		void PrintDebugStats();

		// Token: 0x06001885 RID: 6277
		[EngineMethod("get_average_packet_loss_ratio", false, null, false)]
		float GetAveragePacketLossRatio();

		// Token: 0x06001886 RID: 6278
		[EngineMethod("get_debug_uploads_in_bits", false, null, false)]
		void GetDebugUploadsInBits(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct);

		// Token: 0x06001887 RID: 6279
		[EngineMethod("reset_debug_uploads", false, null, false)]
		void ResetDebugUploads();

		// Token: 0x06001888 RID: 6280
		[EngineMethod("print_replication_table_statistics", false, null, false)]
		void PrintReplicationTableStatistics();

		// Token: 0x06001889 RID: 6281
		[EngineMethod("clear_replication_table_statistics", false, null, false)]
		void ClearReplicationTableStatistics();

		// Token: 0x0600188A RID: 6282
		[EngineMethod("set_server_frame_rate", false, null, false)]
		void SetServerFrameRate(double limit);
	}
}
