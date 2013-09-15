using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouVoice
{
    public partial class BrowseWindow : Form
    {
        protected delegate void VoidDelegate();

        public Video ChosenVideo { get; protected set; }

        protected Listener CommandReceiver;

        public BrowseWindow()
        {
            InitializeComponent();

            ChosenVideo = null;

            this.CommandReceiver = new Listener();
            this.CommandReceiver.UnloadCommands();

            this.CommandReceiver.AddCommand(new Command("Search", Search, new List<string>() { "Search", "Find" }));
            this.CommandReceiver.AddCommand(new Command("Exit", Exit, new List<string>() { "Exit", "Quit", "Close" }));

            this.CommandReceiver.Start();
        }

        protected void Search()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Search));
            }
            else
            {
                // Build the request (videos and playlists)
                SearchResource.ListRequest Request = Globals.Youtube.Search.List("snippet");
                Request.Q = SearchBox.Text;
                Request.Key = Globals.Key;
                Request.Order = SearchResource.ListRequest.OrderEnum.Relevance;

                // Acquire response
                SearchListResponse Response = Request.Execute();

                // Prcoess response
                // Remove all current videos
                Videos.Items.Clear();
                foreach (SearchResult r in Response.Items)
                {
                    ListViewItem NewItem = new ListViewItem();
                    switch (r.Id.Kind)
                    {
                        case "youtube#video":
                            NewItem.Text = (r.Snippet.Title + " " + r.Id.VideoId);
                            break;

                        case "youtube#playlist":
                            NewItem.Text = (r.Snippet.Title + " " + r.Id.PlaylistId);
                            break;
                    }

                    Videos.Items.Add(NewItem);
                }
            }
        }
        protected void Exit()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Exit));
            }
            else
            {
                this.CommandReceiver.Stop();
                this.Close();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Search();
        }
    }
}
