using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003BF RID: 959
	public abstract class GameNetworkMessage
	{
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060035AA RID: 13738 RVA: 0x000DD452 File Offset: 0x000DB652
		// (set) Token: 0x060035AB RID: 13739 RVA: 0x000DD45A File Offset: 0x000DB65A
		public int MessageId { get; set; }

		// Token: 0x060035AC RID: 13740 RVA: 0x000DD464 File Offset: 0x000DB664
		internal void Write()
		{
			DebugNetworkEventStatistics.StartEvent(base.GetType().Name, this.MessageId);
			GameNetworkMessage.WriteIntToPacket(this.MessageId, GameNetwork.IsClientOrReplay ? CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo : CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo);
			this.OnWrite();
			GameNetworkMessage.WriteIntToPacket(5, GameNetworkMessage.TestValueCompressionInfo);
			DebugNetworkEventStatistics.EndEvent();
		}

		// Token: 0x060035AD RID: 13741
		protected abstract void OnWrite();

		// Token: 0x060035AE RID: 13742 RVA: 0x000DD4BC File Offset: 0x000DB6BC
		internal bool Read()
		{
			bool flag = this.OnRead();
			bool flag2 = true;
			if (GameNetworkMessage.ReadIntFromPacket(GameNetworkMessage.TestValueCompressionInfo, ref flag2) != 5)
			{
				throw new MBNetworkBitException(base.GetType().Name);
			}
			return flag;
		}

		// Token: 0x060035AF RID: 13743
		protected abstract bool OnRead();

		// Token: 0x060035B0 RID: 13744 RVA: 0x000DD4F1 File Offset: 0x000DB6F1
		internal MultiplayerMessageFilter GetLogFilter()
		{
			return this.OnGetLogFilter();
		}

		// Token: 0x060035B1 RID: 13745
		protected abstract MultiplayerMessageFilter OnGetLogFilter();

		// Token: 0x060035B2 RID: 13746 RVA: 0x000DD4F9 File Offset: 0x000DB6F9
		internal string GetLogFormat()
		{
			return this.OnGetLogFormat();
		}

		// Token: 0x060035B3 RID: 13747
		protected abstract string OnGetLogFormat();

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060035B4 RID: 13748 RVA: 0x000DD501 File Offset: 0x000DB701
		public static bool IsClientMissionOver
		{
			get
			{
				return GameNetwork.IsClient && !NetworkMain.GameClient.IsInGame && !NetworkMain.CommunityClient.IsInGame;
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000DD528 File Offset: 0x000DB728
		public static bool ReadBoolFromPacket(ref bool bufferReadValid)
		{
			CompressionInfo.Integer integer = new CompressionInfo.Integer(0, 1);
			int num = 0;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref integer, out num);
			return num != 0;
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000DD55C File Offset: 0x000DB75C
		public static void WriteBoolToPacket(bool value)
		{
			CompressionInfo.Integer integer = new CompressionInfo.Integer(0, 1);
			MBAPI.IMBNetwork.WriteIntToPacket(value ? 1 : 0, ref integer);
			DebugNetworkEventStatistics.AddDataToStatistic(integer.GetNumBits());
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000DD594 File Offset: 0x000DB794
		public static int ReadIntFromPacket(CompressionInfo.Integer compressionInfo, ref bool bufferReadValid)
		{
			int num = 0;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000DD5BB File Offset: 0x000DB7BB
		public static void WriteIntToPacket(int value, CompressionInfo.Integer compressionInfo)
		{
			MBAPI.IMBNetwork.WriteIntToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000DD5D8 File Offset: 0x000DB7D8
		public static uint ReadUintFromPacket(CompressionInfo.UnsignedInteger compressionInfo, ref bool bufferReadValid)
		{
			uint num = 0U;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadUintFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000DD5FF File Offset: 0x000DB7FF
		public static void WriteUintToPacket(uint value, CompressionInfo.UnsignedInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUintToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000DD61C File Offset: 0x000DB81C
		public static long ReadLongFromPacket(CompressionInfo.LongInteger compressionInfo, ref bool bufferReadValid)
		{
			long num = 0L;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadLongFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000DD644 File Offset: 0x000DB844
		public static void WriteLongToPacket(long value, CompressionInfo.LongInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteLongToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000DD660 File Offset: 0x000DB860
		public static ulong ReadUlongFromPacket(CompressionInfo.UnsignedLongInteger compressionInfo, ref bool bufferReadValid)
		{
			ulong num = 0UL;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadUlongFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000DD688 File Offset: 0x000DB888
		public static void WriteUlongToPacket(ulong value, CompressionInfo.UnsignedLongInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUlongToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000DD6A4 File Offset: 0x000DB8A4
		public static float ReadFloatFromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = 0f;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadFloatFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000DD6CF File Offset: 0x000DB8CF
		public static void WriteFloatToPacket(float value, CompressionInfo.Float compressionInfo)
		{
			MBAPI.IMBNetwork.WriteFloatToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000DD6EC File Offset: 0x000DB8EC
		public static string ReadStringFromPacket(ref bool bufferReadValid)
		{
			byte[] array = new byte[1024];
			int num = GameNetworkMessage.ReadByteArrayFromPacket(array, 0, 1024, ref bufferReadValid);
			return GameNetworkMessage.StringEncoding.GetString(array, 0, num);
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000DD720 File Offset: 0x000DB920
		public static void WriteStringToPacket(string value)
		{
			byte[] array = (string.IsNullOrEmpty(value) ? new byte[0] : GameNetworkMessage.StringEncoding.GetBytes(value));
			GameNetworkMessage.WriteByteArrayToPacket(array, 0, array.Length);
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000DD753 File Offset: 0x000DB953
		public static int ReadByteArrayFromPacket(byte[] buffer, int offset, int bufferCapacity, ref bool bufferReadValid)
		{
			return MBAPI.IMBNetwork.ReadByteArrayFromPacket(buffer, offset, bufferCapacity, ref bufferReadValid);
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000DD764 File Offset: 0x000DB964
		public static void WriteBannerCodeToPacket(string bannerCode)
		{
			List<BannerData> list;
			Banner.TryGetBannerDataFromCode(bannerCode, out list);
			GameNetworkMessage.WriteIntToPacket(list.Count, CompressionBasic.BannerDataCountCompressionInfo);
			for (int i = 0; i < list.Count; i++)
			{
				BannerData bannerData = list[i];
				GameNetworkMessage.WriteIntToPacket(bannerData.MeshId, CompressionBasic.BannerDataMeshIdCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(bannerData.ColorId, CompressionBasic.BannerDataColorIndexCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(bannerData.ColorId2, CompressionBasic.BannerDataColorIndexCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Size.X, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Size.Y, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Position.X, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Position.Y, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteBoolToPacket(bannerData.DrawStroke);
				GameNetworkMessage.WriteBoolToPacket(bannerData.Mirror);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Rotation, CompressionBasic.BannerDataRotationCompressionInfo);
			}
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000DD864 File Offset: 0x000DBA64
		public static string ReadBannerCodeFromPacket(ref bool bufferReadValid)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataCountCompressionInfo, ref bufferReadValid);
			MBList<BannerData> mblist = new MBList<BannerData>(num);
			for (int i = 0; i < num; i++)
			{
				BannerData bannerData = new BannerData(GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataMeshIdCompressionInfo, ref bufferReadValid), GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataColorIndexCompressionInfo, ref bufferReadValid), GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataColorIndexCompressionInfo, ref bufferReadValid), new Vec2((float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid)), new Vec2((float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid)), GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid), GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataRotationCompressionInfo, ref bufferReadValid) * 0.0027777778f);
				mblist.Add(bannerData);
			}
			return Banner.GetBannerCodeFromBannerDataList(mblist);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000DD922 File Offset: 0x000DBB22
		public static void WriteByteArrayToPacket(byte[] value, int offset, int size)
		{
			MBAPI.IMBNetwork.WriteByteArrayToPacket(value, offset, size);
			DebugNetworkEventStatistics.AddDataToStatistic(MathF.Min(size, 1024) + 10);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000DD944 File Offset: 0x000DBB44
		public static MBActionSet ReadActionSetReferenceFromPacket(CompressionInfo.Integer compressionInfo, ref bool bufferReadValid)
		{
			if (bufferReadValid)
			{
				int num;
				bufferReadValid = MBAPI.IMBNetwork.ReadIntFromPacket(ref compressionInfo, out num);
				return new MBActionSet(num);
			}
			return MBActionSet.InvalidActionSet;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000DD971 File Offset: 0x000DBB71
		public static void WriteActionSetReferenceToPacket(MBActionSet actionSet, CompressionInfo.Integer compressionInfo)
		{
			MBAPI.IMBNetwork.WriteIntToPacket(actionSet.Index, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000DD994 File Offset: 0x000DBB94
		public static int ReadAgentIndexFromPacket(ref bool bufferReadValid)
		{
			CompressionInfo.Integer agentCompressionInfo = CompressionMission.AgentCompressionInfo;
			int num = -1;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref agentCompressionInfo, out num);
			return num;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000DD9C4 File Offset: 0x000DBBC4
		public static void WriteAgentIndexToPacket(int agentIndex)
		{
			CompressionInfo.Integer agentCompressionInfo = CompressionMission.AgentCompressionInfo;
			MBAPI.IMBNetwork.WriteIntToPacket(agentIndex, ref agentCompressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(agentCompressionInfo.GetNumBits());
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000DD9F0 File Offset: 0x000DBBF0
		public static MBObjectBase ReadObjectReferenceFromPacket(MBObjectManager objectManager, CompressionInfo.UnsignedInteger compressionInfo, ref bool bufferReadValid)
		{
			uint num = GameNetworkMessage.ReadUintFromPacket(compressionInfo, ref bufferReadValid);
			if (bufferReadValid && num > 0U)
			{
				MBGUID mbguid = new MBGUID(num);
				return objectManager.GetObject(mbguid);
			}
			return null;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000DDA20 File Offset: 0x000DBC20
		public static void WriteObjectReferenceToPacket(MBObjectBase value, CompressionInfo.UnsignedInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUintToPacket((value != null) ? value.Id.InternalValue : 0U, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000DDA5C File Offset: 0x000DBC5C
		public static VirtualPlayer ReadVirtualPlayerReferenceToPacket(ref bool bufferReadValid, bool canReturnNull = false)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if ((num >= 0 && !GameNetworkMessage.IsClientMissionOver) & bufferReadValid)
			{
				VirtualPlayer virtualPlayer;
				if (!flag)
				{
					virtualPlayer = GameNetwork.VirtualPlayers[num];
				}
				else
				{
					virtualPlayer = GameNetwork.DisconnectedNetworkPeers[num].VirtualPlayer;
				}
				return virtualPlayer;
			}
			return null;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000DDAB1 File Offset: 0x000DBCB1
		public static NetworkCommunicator ReadNetworkPeerReferenceFromPacket(ref bool bufferReadValid, bool canReturnNull = false)
		{
			VirtualPlayer virtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref bufferReadValid, canReturnNull);
			return ((virtualPlayer != null) ? virtualPlayer.Communicator : null) as NetworkCommunicator;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x000DDACC File Offset: 0x000DBCCC
		public static void WriteVirtualPlayerReferenceToPacket(VirtualPlayer virtualPlayer)
		{
			bool flag = false;
			int num = ((virtualPlayer != null) ? virtualPlayer.Index : (-1));
			if (num >= 0 && GameNetwork.VirtualPlayers[num] != virtualPlayer)
			{
				for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i++)
				{
					if (GameNetwork.DisconnectedNetworkPeers[i].VirtualPlayer == virtualPlayer)
					{
						num = i;
						flag = true;
						break;
					}
				}
			}
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(flag);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000DDB35 File Offset: 0x000DBD35
		public static void WriteNetworkPeerReferenceToPacket(NetworkCommunicator networkCommunicator)
		{
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket((networkCommunicator != null) ? networkCommunicator.VirtualPlayer : null);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000DDB48 File Offset: 0x000DBD48
		public static int ReadTeamIndexFromPacket(ref bool bufferReadValid)
		{
			return GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamCompressionInfo, ref bufferReadValid);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000DDB55 File Offset: 0x000DBD55
		public static void WriteTeamIndexToPacket(int teamIndex)
		{
			GameNetworkMessage.WriteIntToPacket(teamIndex, CompressionMission.TeamCompressionInfo);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000DDB64 File Offset: 0x000DBD64
		public static MissionObjectId ReadMissionObjectIdFromPacket(ref bool bufferReadValid)
		{
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref bufferReadValid);
			if (!bufferReadValid || num == -1 || GameNetworkMessage.IsClientMissionOver)
			{
				if (num != -1)
				{
					MBDebug.Print(string.Concat(new object[]
					{
						"Reading null MissionObject because IsClientMissionOver: ",
						GameNetworkMessage.IsClientMissionOver.ToString(),
						" valid read: ",
						bufferReadValid.ToString(),
						" MissionObject ID: ",
						num,
						" runtime: ",
						flag.ToString()
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				return new MissionObjectId(-1, false);
			}
			return new MissionObjectId(num, flag);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000DDC0E File Offset: 0x000DBE0E
		public static void WriteMissionObjectIdToPacket(MissionObjectId value)
		{
			GameNetworkMessage.WriteBoolToPacket(value.CreatedAtRuntime);
			GameNetworkMessage.WriteIntToPacket(value.Id, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000DDC2C File Offset: 0x000DBE2C
		public static Vec3 ReadVec3FromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num3 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			return new Vec3(num, num2, num3, -1f);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000DDC5C File Offset: 0x000DBE5C
		public static void WriteVec3ToPacket(Vec3 value, CompressionInfo.Float compressionInfo)
		{
			GameNetworkMessage.WriteFloatToPacket(value.x, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.y, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.z, compressionInfo);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000DDC84 File Offset: 0x000DBE84
		public static Vec2 ReadVec2FromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			return new Vec2(num, num2);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000DDCA6 File Offset: 0x000DBEA6
		public static void WriteVec2ToPacket(Vec2 value, CompressionInfo.Float compressionInfo)
		{
			GameNetworkMessage.WriteFloatToPacket(value.x, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.y, compressionInfo);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000DDCC0 File Offset: 0x000DBEC0
		public static Mat3 ReadRotationMatrixFromPacket(ref bool bufferReadValid)
		{
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			Vec3 vec3 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000DDCFC File Offset: 0x000DBEFC
		public static void WriteRotationMatrixToPacket(Mat3 value)
		{
			GameNetworkMessage.WriteVec3ToPacket(value.s, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(value.f, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(value.u, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000DDD30 File Offset: 0x000DBF30
		public static MatrixFrame ReadMatrixFrameFromPacket(ref bool bufferReadValid)
		{
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref bufferReadValid);
			MatrixFrame matrixFrame = new MatrixFrame(in mat, in vec);
			matrixFrame.Scale(in vec2);
			return matrixFrame;
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000DDD70 File Offset: 0x000DBF70
		public static void WriteMatrixFrameToPacket(MatrixFrame frame)
		{
			Vec3 scaleVector = frame.rotation.GetScaleVector();
			MatrixFrame matrixFrame = frame;
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			matrixFrame.Scale(in vec);
			GameNetworkMessage.WriteVec3ToPacket(matrixFrame.origin, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(scaleVector, CompressionBasic.ScaleCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(matrixFrame.rotation);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000DDDEC File Offset: 0x000DBFEC
		public static MatrixFrame ReadNonUniformTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			MatrixFrame matrixFrame = GameNetworkMessage.ReadUnitTransformFromPacket(positionCompressionInfo, quaternionCompressionInfo, ref bufferReadValid);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
			matrixFrame.rotation.ApplyScaleLocal(in vec);
			return matrixFrame;
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000DDE20 File Offset: 0x000DC020
		public static void WriteNonUniformTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			MatrixFrame matrixFrame = frame;
			Vec3 vec = matrixFrame.rotation.MakeUnit();
			GameNetworkMessage.WriteUnitTransformToPacket(matrixFrame, positionCompressionInfo, quaternionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(vec, CompressionBasic.ScaleCompressionInfo);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000DDE50 File Offset: 0x000DC050
		public static MatrixFrame ReadTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			MatrixFrame matrixFrame = GameNetworkMessage.ReadUnitTransformFromPacket(positionCompressionInfo, quaternionCompressionInfo, ref bufferReadValid);
			if (GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid))
			{
				float num = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
				matrixFrame.rotation.ApplyScaleLocal(num);
			}
			return matrixFrame;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000DDE88 File Offset: 0x000DC088
		public static void WriteTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			MatrixFrame matrixFrame = frame;
			Vec3 vec = matrixFrame.rotation.MakeUnit();
			GameNetworkMessage.WriteUnitTransformToPacket(matrixFrame, positionCompressionInfo, quaternionCompressionInfo);
			bool flag = !vec.x.ApproximatelyEqualsTo(1f, CompressionBasic.ScaleCompressionInfo.GetPrecision());
			GameNetworkMessage.WriteBoolToPacket(flag);
			if (flag)
			{
				GameNetworkMessage.WriteFloatToPacket(vec.x, CompressionBasic.ScaleCompressionInfo);
			}
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000DDEE4 File Offset: 0x000DC0E4
		public static MatrixFrame ReadUnitTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			return new MatrixFrame
			{
				origin = GameNetworkMessage.ReadVec3FromPacket(positionCompressionInfo, ref bufferReadValid),
				rotation = GameNetworkMessage.ReadQuaternionFromPacket(quaternionCompressionInfo, ref bufferReadValid).ToMat3()
			};
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000DDF1E File Offset: 0x000DC11E
		public static void WriteUnitTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			GameNetworkMessage.WriteVec3ToPacket(frame.origin, positionCompressionInfo);
			GameNetworkMessage.WriteQuaternionToPacket(frame.rotation.ToQuaternion(), quaternionCompressionInfo);
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000DDF40 File Offset: 0x000DC140
		public static Quaternion ReadQuaternionFromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			Quaternion quaternion = default(Quaternion);
			float num = 0f;
			int num2 = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.OmittedQuaternionComponentIndexCompressionInfo, ref bufferReadValid);
			for (int i = 0; i < 4; i++)
			{
				if (i != num2)
				{
					quaternion[i] = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
					num += quaternion[i] * quaternion[i];
				}
			}
			quaternion[num2] = MathF.Sqrt(1f - num);
			quaternion.SafeNormalize();
			return quaternion;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000DDFB8 File Offset: 0x000DC1B8
		public static void WriteQuaternionToPacket(Quaternion q, CompressionInfo.Float compressionInfo)
		{
			int num = -1;
			float num2 = 0f;
			Quaternion quaternion = q;
			quaternion.SafeNormalize();
			for (int i = 0; i < 4; i++)
			{
				float num3 = MathF.Abs(quaternion[i]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (quaternion[num] < 0f)
			{
				quaternion.Flip();
			}
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.OmittedQuaternionComponentIndexCompressionInfo);
			for (int j = 0; j < 4; j++)
			{
				if (j != num)
				{
					GameNetworkMessage.WriteFloatToPacket(quaternion[j], compressionInfo);
				}
			}
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000DE044 File Offset: 0x000DC244
		public static void WriteBodyPropertiesToPacket(BodyProperties bodyProperties)
		{
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Age, CompressionBasic.AgentAgeCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Weight, CompressionBasic.FaceKeyDataCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Build, CompressionBasic.FaceKeyDataCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart5, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart6, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart7, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart8, CompressionBasic.DebugULongNonCompressionInfo);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000DE10C File Offset: 0x000DC30C
		public static BodyProperties ReadBodyPropertiesFromPacket(ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentAgeCompressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.FaceKeyDataCompressionInfo, ref bufferReadValid);
			float num3 = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.FaceKeyDataCompressionInfo, ref bufferReadValid);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num5 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num6 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num7 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num8 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num9 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num10 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num11 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			if (bufferReadValid)
			{
				return new BodyProperties(new DynamicBodyProperties(num, num2, num3), new StaticBodyProperties(num4, num5, num6, num7, num8, num9, num10, num11));
			}
			return default(BodyProperties);
		}

		// Token: 0x0400170A RID: 5898
		private static readonly Encoding StringEncoding = new UTF8Encoding();

		// Token: 0x0400170B RID: 5899
		private static CompressionInfo.Integer TestValueCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x0400170C RID: 5900
		private const int ConstTestValue = 5;

		// Token: 0x02000686 RID: 1670
		// (Invoke) Token: 0x0600415E RID: 16734
		public delegate bool ClientMessageHandlerDelegate<T>(NetworkCommunicator peer, T message) where T : GameNetworkMessage;

		// Token: 0x02000687 RID: 1671
		// (Invoke) Token: 0x06004162 RID: 16738
		public delegate void ServerMessageHandlerDelegate<T>(T message) where T : GameNetworkMessage;
	}
}
