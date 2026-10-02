using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TaleWorlds.Library
{
	// Token: 0x020000A6 RID: 166
	public class VirtualFolders
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x00016358 File Offset: 0x00014558
		public static string GetFileContent(string filePath, Type type = null)
		{
			if (VirtualFolders._useVirtualFolders)
			{
				if (type == null)
				{
					type = typeof(VirtualFolders);
				}
				return VirtualFolders.GetVirtualFileContent(filePath, type);
			}
			if (filePath.Contains("__MODULE_NAME__"))
			{
				string text = "__MODULE_NAME__";
				string text2 = Regex.Escape(text) + "(.*?)" + Regex.Escape(text);
				string value = Regex.Match(filePath, text2).Groups[1].Value;
				filePath = filePath.Replace(text + value + text, VirtualFolders.PlatformDLCPaths[value]);
			}
			if (!File.Exists(filePath))
			{
				return "";
			}
			return File.ReadAllText(filePath);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000163FC File Offset: 0x000145FC
		private static string GetVirtualFileContent(string filePath, Type type)
		{
			string fileName = Path.GetFileName(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			Type type2 = VirtualFolders.GetNestedDirectory(directoryName, type);
			if (type2 == null)
			{
				type2 = type;
				string[] array = directoryName.Split(new char[] { Path.DirectorySeparatorChar });
				int num = 0;
				while (type2 != null && num != array.Length)
				{
					if (!string.IsNullOrEmpty(array[num]))
					{
						type2 = VirtualFolders.GetNestedDirectory(array[num], type2);
					}
					num++;
				}
			}
			if (type2 != null)
			{
				FieldInfo[] fields = type2.GetFields();
				for (int i = 0; i < fields.Length; i++)
				{
					VirtualFileAttribute[] array2 = (VirtualFileAttribute[])fields[i].GetCustomAttributesSafe(typeof(VirtualFileAttribute), false);
					if (array2[0].Name == fileName)
					{
						return array2[0].Content;
					}
				}
			}
			return "";
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000164D4 File Offset: 0x000146D4
		private static Type GetNestedDirectory(string name, Type type)
		{
			foreach (Type type2 in type.GetNestedTypes())
			{
				if (((VirtualDirectoryAttribute[])type2.GetCustomAttributesSafe(typeof(VirtualDirectoryAttribute), false))[0].Name == name)
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x040001E0 RID: 480
		private static readonly bool _useVirtualFolders = true;

		// Token: 0x040001E1 RID: 481
		public static Dictionary<string, string> PlatformDLCPaths = new Dictionary<string, string>();

		// Token: 0x020000F2 RID: 242
		[VirtualDirectory("..")]
		public class Win64_Shipping_Client
		{
			// Token: 0x020000FD RID: 253
			[VirtualDirectory("..")]
			public class bin
			{
				// Token: 0x020000FE RID: 254
				[VirtualDirectory("Parameters")]
				public class Parameters
				{
					// Token: 0x04000348 RID: 840
					[VirtualFile("Environment", "gcKU8aujp3jrCqLXXTMhgR10BWT7d40BMQjVCc8j6ed3wcHMP_Ppm0QxV86cbjQonf.qjc.JyKVE_kU4YwLqKDu9w2daHZlUzBVDOq_k0bPuPQAWYKm.4pupPDvJKdW3oEoRfYAvu5IXFXi1drKMaYL8.RrBBdcwrmmZTVKww60-")]
					public string Environment;

					// Token: 0x04000349 RID: 841
					[VirtualFile("Version.xml", "<Version>\t<Singleplayer Value=\"v1.4.7.117484\"/></Version> ")]
					public string Version;

					// Token: 0x0400034A RID: 842
					[VirtualFile("ClientProfile.xml", "<ClientProfile Value=\"Azure.Discovery\"/>")]
					public string ClientProfile;

					// Token: 0x020000FF RID: 255
					[VirtualDirectory("ClientProfiles")]
					public class ClientProfiles
					{
						// Token: 0x02000100 RID: 256
						[VirtualDirectory("Azure.Discovery")]
						public class AzureDiscovery
						{
							// Token: 0x0400034B RID: 843
							[VirtualFile("LobbyClient.xml", "<Configuration>\t<SessionProvider Type=\"ThreadedRest\" />\t<Clients>\t\t<Client Type=\"LobbyClient\" />\t</Clients>\t<Parameters>\t\t<Parameter Name=\"LobbyClient.ServiceDiscovery.Address\" Value=\"https://bannerlord-service-discovery.bannerlord-services-3.net/\" />\t\t<Parameter Name=\"LobbyClient.Address\" Value=\"service://bannerlord.lobby/\" />\t</Parameters></Configuration>")]
							public string LobbyClient;
						}
					}
				}
			}
		}
	}
}
