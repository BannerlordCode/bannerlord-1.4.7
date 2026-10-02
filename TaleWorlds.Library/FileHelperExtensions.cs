using System;
using System.Threading.Tasks;
using System.Xml;

namespace TaleWorlds.Library
{
	// Token: 0x02000032 RID: 50
	public static class FileHelperExtensions
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00006DFC File Offset: 0x00004FFC
		public static void Load(this XmlDocument document, PlatformFilePath path)
		{
			string fileContentString = FileHelper.GetFileContentString(path);
			if (!string.IsNullOrEmpty(fileContentString))
			{
				document.LoadXml(fileContentString);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006E20 File Offset: 0x00005020
		public static async Task LoadAsync(this XmlDocument document, PlatformFilePath path)
		{
			string text = await FileHelper.GetFileContentStringAsync(path);
			if (!string.IsNullOrEmpty(text))
			{
				document.LoadXml(text);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006E70 File Offset: 0x00005070
		public static void Save(this XmlDocument document, PlatformFilePath path)
		{
			string outerXml = document.OuterXml;
			FileHelper.SaveFileString(path, outerXml);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006E8C File Offset: 0x0000508C
		public static async Task SaveAsync(this XmlDocument document, PlatformFilePath path)
		{
			string outerXml = document.OuterXml;
			await FileHelper.SaveFileStringAsync(path, outerXml);
		}
	}
}
