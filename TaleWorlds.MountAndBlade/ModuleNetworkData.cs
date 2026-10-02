using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000305 RID: 773
	public static class ModuleNetworkData
	{
		// Token: 0x06002C37 RID: 11319 RVA: 0x000A978C File Offset: 0x000A798C
		public static EquipmentElement ReadItemReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			MBObjectBase mbobjectBase2 = null;
			if (flag)
			{
				mbobjectBase2 = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			}
			ItemObject itemObject = mbobjectBase as ItemObject;
			return new EquipmentElement(itemObject, null, mbobjectBase2 as ItemObject, false);
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000A97D0 File Offset: 0x000A79D0
		public static void WriteItemReferenceToPacket(EquipmentElement equipElement)
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(equipElement.Item, CompressionBasic.GUIDCompressionInfo);
			if (equipElement.CosmeticItem != null)
			{
				GameNetworkMessage.WriteBoolToPacket(true);
				GameNetworkMessage.WriteObjectReferenceToPacket(equipElement.CosmeticItem, CompressionBasic.GUIDCompressionInfo);
				return;
			}
			GameNetworkMessage.WriteBoolToPacket(false);
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000A9808 File Offset: 0x000A7A08
		public static MissionWeapon ReadWeaponReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			if (GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid))
			{
				return MissionWeapon.Invalid;
			}
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref bufferReadValid);
			int num2 = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponReloadPhaseCompressionInfo, ref bufferReadValid);
			short num3 = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			Banner banner = null;
			if (flag)
			{
				string text = GameNetworkMessage.ReadBannerCodeFromPacket(ref bufferReadValid);
				if (bufferReadValid)
				{
					banner = new Banner(text);
				}
			}
			ItemObject itemObject = mbobjectBase as ItemObject;
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			MissionWeapon? missionWeapon = null;
			if (bufferReadValid && flag2)
			{
				MBObjectBase mbobjectBase2 = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
				int num4 = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref bufferReadValid);
				ItemObject itemObject2 = mbobjectBase2 as ItemObject;
				missionWeapon = new MissionWeapon?(new MissionWeapon(itemObject2, null, banner, (short)num4));
			}
			return new MissionWeapon(itemObject, null, banner, (short)num, (short)num2, missionWeapon)
			{
				CurrentUsageIndex = (int)num3
			};
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000A98E0 File Offset: 0x000A7AE0
		public static void WriteWeaponReferenceToPacket(MissionWeapon weapon)
		{
			GameNetworkMessage.WriteBoolToPacket(weapon.IsEmpty);
			if (!weapon.IsEmpty)
			{
				GameNetworkMessage.WriteObjectReferenceToPacket(weapon.Item, CompressionBasic.GUIDCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)weapon.RawDataForNetwork, CompressionBasic.ItemDataValueCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)weapon.ReloadPhase, CompressionMission.WeaponReloadPhaseCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(weapon.CurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
				bool flag = weapon.Banner != null;
				GameNetworkMessage.WriteBoolToPacket(flag);
				if (flag)
				{
					GameNetworkMessage.WriteBannerCodeToPacket(weapon.Banner.BannerCode);
				}
				MissionWeapon ammoWeapon = weapon.AmmoWeapon;
				bool flag2 = !ammoWeapon.IsEmpty;
				GameNetworkMessage.WriteBoolToPacket(flag2);
				if (flag2)
				{
					GameNetworkMessage.WriteObjectReferenceToPacket(ammoWeapon.Item, CompressionBasic.GUIDCompressionInfo);
					GameNetworkMessage.WriteIntToPacket((int)ammoWeapon.RawDataForNetwork, CompressionBasic.ItemDataValueCompressionInfo);
				}
			}
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000A99A8 File Offset: 0x000A7BA8
		public static MissionWeapon ReadMissileWeaponReferenceFromPacket(MBObjectManager objectManager, ref bool bufferReadValid)
		{
			MBObjectBase mbobjectBase = GameNetworkMessage.ReadObjectReferenceFromPacket(objectManager, CompressionBasic.GUIDCompressionInfo, ref bufferReadValid);
			short num = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref bufferReadValid);
			ItemObject itemObject = mbobjectBase as ItemObject;
			return new MissionWeapon(itemObject, null, null, 1)
			{
				CurrentUsageIndex = (int)num
			};
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000A99E8 File Offset: 0x000A7BE8
		public static void WriteMissileWeaponReferenceToPacket(MissionWeapon weapon)
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(weapon.Item, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(weapon.CurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}
	}
}
