using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017B RID: 379
	public class ChatLogItemWidget : Widget
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x000354EB File Offset: 0x000336EB
		public ChatLogItemWidget(UIContext context)
			: base(context)
		{
			this._fullyInsideAction = new Action<Widget>(this.UpdateWidgetFullyInside);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00035524 File Offset: 0x00033724
		private void UpdateWidgetFullyInside(Widget widget)
		{
			widget.DoNotRenderIfNotFullyInsideScissor = false;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0003552D File Offset: 0x0003372D
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			base.ApplyActionToAllChildrenRecursive(this._fullyInsideAction);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00035544 File Offset: 0x00033744
		private void PostMessage(string message)
		{
			if (message.IndexOf(this._detailOpeningTag, StringComparison.Ordinal) > 0)
			{
				foreach (ChatLogItemWidget.ChatMultiLineElement chatMultiLineElement in this.GetFormattedLinesFromMessage(message))
				{
					RichTextWidget richTextWidget = new RichTextWidget(base.Context)
					{
						Id = "FormattedLineRichTextWidget",
						WidthSizePolicy = SizePolicy.StretchToParent,
						HeightSizePolicy = SizePolicy.CoverChildren,
						Brush = this.OneLineTextWidget.ReadOnlyBrush,
						MarginTop = -2f,
						MarginBottom = -2f,
						IsEnabled = false,
						Text = chatMultiLineElement.Line,
						MarginLeft = (float)(chatMultiLineElement.IdentModifier * this._defaultMarginLeftPerIndent) * base._inverseScaleToUse,
						ClipContents = false,
						DoNotRenderIfNotFullyInsideScissor = false
					};
					this.CollapsableWidget.AddChild(richTextWidget);
				}
				this.CollapsableWidget.IsVisible = true;
				this.OneLineTextWidget.IsVisible = false;
				return;
			}
			this.OneLineTextWidget.Text = message;
			this.CollapsableWidget.IsVisible = false;
			this.OneLineTextWidget.IsVisible = true;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0003567C File Offset: 0x0003387C
		private List<ChatLogItemWidget.ChatMultiLineElement> GetFormattedLinesFromMessage(string message)
		{
			List<ChatLogItemWidget.ChatMultiLineElement> list = new List<ChatLogItemWidget.ChatMultiLineElement>();
			XmlDocument xmlDocument = new XmlDocument();
			int num = message.IndexOf(this._detailOpeningTag, StringComparison.Ordinal);
			string text = message.Substring(0, num);
			string text2 = message.Substring(num, message.Length - num);
			text2 = this._detailOpeningTag + text2 + this._detailClosingTag;
			list.Add(new ChatLogItemWidget.ChatMultiLineElement(text, 0));
			try
			{
				xmlDocument.LoadXml(text2);
				this.AddLinesFromXMLRecur(xmlDocument.FirstChild, ref list, 0);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Couldn't parse chat log message: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Chat\\ChatLogItemWidget.cs", "GetFormattedLinesFromMessage", 111);
			}
			return list;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00035730 File Offset: 0x00033930
		private void AddLinesFromXMLRecur(XmlNode currentNode, ref List<ChatLogItemWidget.ChatMultiLineElement> lineList, int currentIndentModifier)
		{
			if (currentNode.NodeType == XmlNodeType.Text)
			{
				lineList.Add(new ChatLogItemWidget.ChatMultiLineElement(currentNode.InnerText, currentIndentModifier));
				for (int i = 0; i < currentNode.ChildNodes.Count; i++)
				{
					this.AddLinesFromXMLRecur(currentNode.ChildNodes.Item(i), ref lineList, currentIndentModifier + 1);
				}
				return;
			}
			for (int j = 0; j < currentNode.ChildNodes.Count; j++)
			{
				this.AddLinesFromXMLRecur(currentNode.ChildNodes.Item(j), ref lineList, currentIndentModifier + 1);
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x000357B2 File Offset: 0x000339B2
		// (set) Token: 0x060013AB RID: 5035 RVA: 0x000357BA File Offset: 0x000339BA
		[Editor(false)]
		public RichTextWidget OneLineTextWidget
		{
			get
			{
				return this._oneLineTextWidget;
			}
			set
			{
				if (this._oneLineTextWidget != value)
				{
					this._oneLineTextWidget = value;
				}
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x000357CC File Offset: 0x000339CC
		// (set) Token: 0x060013AD RID: 5037 RVA: 0x000357D4 File Offset: 0x000339D4
		[Editor(false)]
		public ChatCollapsableListPanel CollapsableWidget
		{
			get
			{
				return this._collapsableWidget;
			}
			set
			{
				if (this._collapsableWidget != value)
				{
					this._collapsableWidget = value;
				}
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x000357E6 File Offset: 0x000339E6
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x000357EE File Offset: 0x000339EE
		[Editor(false)]
		public string ChatLine
		{
			get
			{
				return this._chatLine;
			}
			set
			{
				if (this._chatLine != value)
				{
					this._chatLine = value;
					this.PostMessage(value);
				}
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0003580C File Offset: 0x00033A0C
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x00035814 File Offset: 0x00033A14
		[Editor(false)]
		public ChatLogWidget ChatLogWidget
		{
			get
			{
				return this._chatLogWidget;
			}
			set
			{
				if (this._chatLogWidget != value)
				{
					this._chatLogWidget = value;
				}
			}
		}

		// Token: 0x040008DF RID: 2271
		private int _defaultMarginLeftPerIndent = 20;

		// Token: 0x040008E0 RID: 2272
		private string _detailOpeningTag = "<Detail>";

		// Token: 0x040008E1 RID: 2273
		private string _detailClosingTag = "</Detail>";

		// Token: 0x040008E2 RID: 2274
		private Action<Widget> _fullyInsideAction;

		// Token: 0x040008E3 RID: 2275
		private ChatLogWidget _chatLogWidget;

		// Token: 0x040008E4 RID: 2276
		private string _chatLine;

		// Token: 0x040008E5 RID: 2277
		private RichTextWidget _oneLineTextWidget;

		// Token: 0x040008E6 RID: 2278
		private ChatCollapsableListPanel _collapsableWidget;

		// Token: 0x020001D4 RID: 468
		public struct ChatMultiLineElement
		{
			// Token: 0x06001577 RID: 5495 RVA: 0x00039FE7 File Offset: 0x000381E7
			public ChatMultiLineElement(string line, int identModifier)
			{
				this.Line = line;
				this.IdentModifier = identModifier;
			}

			// Token: 0x04000A54 RID: 2644
			public string Line;

			// Token: 0x04000A55 RID: 2645
			public int IdentModifier;
		}
	}
}
