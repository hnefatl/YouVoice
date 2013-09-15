using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Net;

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

            this.Videos.SelectedIndexChanged += Videos_SelectedIndexChanged;
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
                Request.MaxResults = (int)SearchResults.Value;
                Request.Type = "video,playlist";

                // Acquire response
                SearchListResponse Response = Request.Execute();

                // Prcoess response
                // Remove all current videos
                Videos.Items.Clear();
                for (int x = 0; x < Response.Items.Count; x++)
                {
                    VideoListViewItem NewItem = new VideoListViewItem();
                    // Download the thumbnail and store
                    if (!string.IsNullOrWhiteSpace(Response.Items[x].Snippet.Thumbnails.Default.Url))
                    {
                        WebRequest ThumbnailRequest = WebRequest.Create(Response.Items[x].Snippet.Thumbnails.Default.Url);
                        NewItem.Thumbnail = Image.FromStream(ThumbnailRequest.GetResponse().GetResponseStream());
                    }
                    NewItem.Description = Response.Items[x].Snippet.Description;

                    NewItem.Text = Convert.ToString(x + 1);

                    switch (Response.Items[x].Id.Kind)
                    {
                        case "youtube#video":
                            NewItem.VideoUrl = Response.Items[x].Id.VideoId;
                            NewItem.SubItems.Add(Response.Items[x].Snippet.Title);
                            NewItem.SubItems.Add("Video");
                            break;

                        case "youtube#playlist":
                            NewItem.PlaylistUrl = Response.Items[x].Id.PlaylistId;
                            NewItem.SubItems.Add(Response.Items[x].Snippet.Title);
                            NewItem.SubItems.Add("Playlist");
                            break;
                    }

                    NewItem.SubItems.Add(Response.Items[x].Snippet.ChannelTitle);

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

        private void Videos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView Sender = (ListView)sender;
            if (Sender.SelectedItems.Count > 0)
            {
                VideoListViewItem Selected = Sender.SelectedItems[0] as VideoListViewItem;

                // Load the image
                VideoThumbnail.Image = Selected.Thumbnail;
                VideoThumbnail.Width = Selected.Thumbnail.Width;
                VideoThumbnail.Height = Selected.Thumbnail.Height;

                // Override the description text
                VideoDescription.Text = Selected.Description;

                // Resize the description to accommodate for the thumbnail
                VideoDescription.Location = new Point(VideoDescription.Location.X, VideoThumbnail.Height + (11 * 2));
                VideoDescription.Height = ((this.Height - 22) - 63) - VideoThumbnail.Height;
            }
        }
    }

    class VideoListViewItem : ListViewItem
    {
        public VideoListViewItem()
        {
            VideoUrl = null;
            PlaylistUrl = null;
        }

        public string VideoUrl { get; set; }
        public string PlaylistUrl { get; set; }
        public Image Thumbnail { get; set; }
        public string Description { get; set; }
    }
}
