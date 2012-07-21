using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Calc
{
	public partial class HelpDialog : Form
	{
		HistoryView _historyView = null;

		public HelpDialog(HistoryView historyView)
		{
			_historyView = historyView;

			InitializeComponent();
		}

		private void HelpDialog_Load(object sender, EventArgs e)
		{
			HelpTopic[] topics = Help.Topics;

			foreach (HelpTopic topic in topics)
			{
				lstTopic.Items.Add(new TagString(topic.TopicText, topic));
			}
		}

		private void lstTopic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstTopic.SelectedIndex >= 0)
			{
				HelpTopic topic = (HelpTopic)((TagString)lstTopic.SelectedItem).Tag;

				//txtBody.Text = topic.TopicText + "\r\n\r\n" + topic.Body;

				HistoryLook topicLook = _historyView.GetHistoryLook(HistoryType.HelpTopic);
				HistoryLook bodyLook = _historyView.GetHistoryLook(HistoryType.HelpBody);

				txtBody.Text = topic.TopicText;
				txtBody.SelectAll();
				txtBody.SelectionFont = topicLook.Font;
				txtBody.SelectionColor = topicLook.TextColor;

				txtBody.Text += "\n\n";
				int bodyStart = txtBody.Text.Length;
				txtBody.Text += topic.Body;
				txtBody.Select(bodyStart, topic.Body.Length);
				txtBody.SelectionFont = bodyLook.Font;
				txtBody.SelectionColor = bodyLook.TextColor;

				txtBody.Select(0, 0);
			}
			else
			{
				txtBody.Text = "";
			}
		}
	}
}
