using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core
{
	// Token: 0x02000090 RID: 144
	public class InquiryElement
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x0001CF6F File Offset: 0x0001B16F
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = true;
			this.Hint = null;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001CF9A File Offset: 0x0001B19A
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier, bool isEnabled, string hint)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = isEnabled;
			this.Hint = hint;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001CFC8 File Offset: 0x0001B1C8
		public bool HasSameContentWith(object other)
		{
			InquiryElement inquiryElement;
			if ((inquiryElement = other as InquiryElement) != null)
			{
				if (this.Title == inquiryElement.Title)
				{
					if (this.ImageIdentifier != null || inquiryElement.ImageIdentifier != null)
					{
						ImageIdentifier imageIdentifier = this.ImageIdentifier;
						if (imageIdentifier == null || !imageIdentifier.Equals(inquiryElement.ImageIdentifier))
						{
							return false;
						}
					}
					if (this.Identifier == inquiryElement.Identifier && this.IsEnabled == inquiryElement.IsEnabled)
					{
						return this.Hint == inquiryElement.Hint;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0400045C RID: 1116
		public readonly string Title;

		// Token: 0x0400045D RID: 1117
		public readonly ImageIdentifier ImageIdentifier;

		// Token: 0x0400045E RID: 1118
		public readonly object Identifier;

		// Token: 0x0400045F RID: 1119
		public readonly bool IsEnabled;

		// Token: 0x04000460 RID: 1120
		public readonly string Hint;
	}
}
