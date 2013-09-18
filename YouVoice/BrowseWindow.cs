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
        protected delegate void VoidDelegate1Int(int One);
        protected delegate void VoidDelegate1Char(char One);
        protected delegate void VoidDelegate2String(string One, string Two);

        #region Members
        public string ChosenVideo { get; protected set; }
        public string CurrentSearchTerm { get; protected set; }

        protected int SearchResultsQuantity { get; set; }

        SearchListResponse Response;

        protected int Page;

        protected Listener CommandReceiver;
        protected Listener SpellerReceiver;
        protected DictationListener DictationReceiver;
        #endregion

        #region Methods

        #region Constructor
        public BrowseWindow()
        {
            InitializeComponent();

            Videos.Focus();

            ChosenVideo = null;

            Page = 0;

            #region CommandReceiver
            this.CommandReceiver = new Listener();

            this.CommandReceiver.AddCommand(new Command("Search", Search, new List<string>() { "Search", "Find" }));
            this.CommandReceiver.AddCommand(new Command("Exit", Exit, new List<string>() { "Exit", "Quit", "Close" }));

            this.CommandReceiver.AddCommand(new Command("Play", Play, new List<string>() { "Play", "Run", "Watch" }));

            this.CommandReceiver.AddCommand(new Command("Next", NextPage, new List<string>() { "Next", "Next page" }));
            this.CommandReceiver.AddCommand(new Command("Previous", PreviousPage, new List<string>() { "Previous", "Previous page" }));

            this.CommandReceiver.AddCommand(new Command("Down", () =>
            {

            }, new List<string>() { "Down", "Scroll down" }));

            this.CommandReceiver.AddCommand(new Command("Spell", () =>
            {
                CommandReceiver.Disable();
                SpellerReceiver.Enable();
            }, new List<string>() { "Spell", "Start spelling" }));

            this.CommandReceiver.AddCommand(new Command("Dictate", () =>
            {
                CommandReceiver.Disable();
                DictationReceiver.Enable();
            }, new List<string>() { "Dictate", "Start dictating" }));
            #endregion

            #region SpellerReceiver
            this.SpellerReceiver = new Listener();

            // Exit spell mode command
            this.SpellerReceiver.AddCommand(new Command("DisableSpell", () =>
            {
                // Switch controls
                SpellerReceiver.Disable();
                CommandReceiver.Enable();
            }, new List<string>() { "Stop", "Stop spelling" }));

            // Exit spell mode, start dictation mode command
            this.SpellerReceiver.AddCommand(new Command("Dictate", () =>
            {
                SpellerReceiver.Disable();
                SpellerReceiver.Enable();
            }, new List<string>() { "Dictate", "Start dictating" }));

            // Letters
            for (char CurrentLetter = 'A'; CurrentLetter <= 'Z'; CurrentLetter++)
            {
                char CurrentLetterTemp = CurrentLetter;
                this.SpellerReceiver.AddCommand(new Command("Letter" + CurrentLetter, () =>
                {
                    AddLetter(CurrentLetterTemp);
                }, new List<string>() { Convert.ToString(CurrentLetter) }));
            }

            // Backspace command
            this.SpellerReceiver.AddCommand(new Command("Backspace", () =>
            {
                RemoveLastLetter();
            }, new List<string>() { "Backspace", "Delete" }));

            // Clear command
            this.SpellerReceiver.AddCommand(new Command("Clear", () =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new VoidDelegate(this.SearchBox.Clear));
                }
                else
                {
                    this.SearchBox.Clear();
                }
            }, new List<string>() { "Clear", "Empty" }));

            #endregion

            #region DictationReceiver
            this.DictationReceiver = new DictationListener(SearchBox.Text);

            // Stop dictating
            this.DictationReceiver.AddCommand(new Command("DisableDictate", () =>
            {
                DictationReceiver.Disable();
                CommandReceiver.Enable();
            }, new List<string>() { "Stop", "Stop dictating" }));

            // Stop dictating, start spelling
            this.DictationReceiver.AddCommand(new Command("Spell", () =>
            {
                SpellerReceiver.Enable();
                DictationReceiver.Disable();
            }, new List<string>() { "Spell", "Start spelling" }));

            #endregion

            this.CommandReceiver.Start();

            this.Videos.SelectedIndexChanged += Videos_SelectedIndexChanged;
        }
        #endregion

        #region Generics
        protected void Search()
        {
            Search(SearchBox.Text, "");
        }
        protected void Search(string SearchTerm, string PageToken)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate2String(Search), new string[] { SearchTerm, PageToken });
            }
            else
            {
                // Check that no unnecessary searches are made
                if (SearchBox.Text != CurrentSearchTerm)
                {
                    CurrentSearchTerm = SearchBox.Text;

                    #region Request
                    // Build the request (videos and playlists)
                    SearchResource.ListRequest Request = Globals.Youtube.Search.List("snippet");
                    Request.Q = SearchTerm;
                    Request.Key = Globals.Key;
                    Request.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                    if (!string.IsNullOrWhiteSpace(PageToken))
                    {
                        Request.PageToken = PageToken;
                    }
                    Request.MaxResults = (int)SearchResults.Value;
                    Request.PrettyPrint = true;
                    Request.Type = "video,playlist";
                    #endregion

                    #region Response
                    // Acquire response
                    Response = Request.Execute();
                    #endregion

                    #region Commands
                    // Remove old commands
                    string CommandName = "SearchResults";
                    for (int x = 0; x < CommandReceiver.Commands.Count; x++)
                    {
                        try
                        {
                            if (CommandReceiver.Commands[x].Name.Substring(0, CommandName.Length) == CommandName)
                            {
                                // SearchResults command - remove it
                                CommandReceiver.UnloadCommand(x);
                            }
                        }
                        catch
                        {
                            // Ignore
                        }
                    }
                    // Add new commands (from 0 to the number of results on the page)
                    for (int x = 0; x <= SearchResults.Value; x++)
                    {
                        int X = x;
                        List<string> SearchResultsCommands = new List<string>();
                        SearchResultsCommands.Add((X + 1) + (X == 0 ? " result" : " results"));
                        SearchResultsCommands.Add("Set " + (X + 1) + (X == 0 ? " result" : " results"));

                        this.CommandReceiver.AddCommand(new Command("SearchResults" + (X + 1), () =>
                        {
                            SearchResultsQuantity = X;
                            SetSearchResults();
                        }, SearchResultsCommands));
                    }
                    #endregion

                    #region Cleanup
                    // Remove all current videos
                    for (int x = 0; x < Videos.Items.Count; x++)
                    {
                        // Remove commands with names in the pattern "PlayX" or "SelectX"
                        CommandReceiver.UnloadCommand("Play" + (x + 1));
                        CommandReceiver.UnloadCommand("Select" + (x + 1));
                        Videos.Items.RemoveAt(x);
                    }
                    #endregion

                    #region New Videos
                    // Process response
                    for (int x = 0; x < Response.Items.Count; x++)
                    {
                        VideoListViewItem NewItem = new VideoListViewItem();

                        #region Attribute transfer
                        // Store the description
                        NewItem.Description = Response.Items[x].Snippet.Description;

                        // Store the number
                        NewItem.Text = Convert.ToString(x + 1);

                        // Store the thumbnail url
                        NewItem.ThumbnailUrl = Response.Items[x].Snippet.Thumbnails.Default.Url;
                        #endregion

                        #region Commands
                        // Add a command to select this video
                        int X = x;
                        CommandReceiver.AddCommand(new Command("Select" + (x + 1), new Action(() =>
                        {
                            SelectVideo(X);
                        }), new List<string>() { "Select " + (x + 1), "Choose " + (x + 1), "Click " + (x + 1) }));

                        // Add a command to play this video
                        CommandReceiver.AddCommand(new Command("Play" + (x + 1), () =>
                        {
                            Play(X);
                        }, new List<string>() { "Play " + (x + 1), "Run " + (x + 1), "Watch " + (x + 1) }));

                        #endregion

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
                    #endregion
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
        protected void Play()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Play));
            }
            else
            {
                if (Videos.SelectedIndices.Count > 0)
                {
                    // Play the first (and only) selected item
                    Play(Videos.SelectedIndices[0]);
                }
            }
        }
        protected void NextPage()
        {
            Search(SearchBox.Text, Response.NextPageToken);
            Page++;
        }
        protected void PreviousPage()
        {
            Search(SearchBox.Text, Response.PrevPageToken);
            Page--;
        }
        protected void SelectVideo(int Index)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate1Int(SelectVideo), Index);
            }
            else
            {
                if (Videos.Items.Count >= Index)
                {
                    Videos.Items[Index].Selected = true;
                    Videos.Items[Index].EnsureVisible();
                }
            }
        }
        protected void Play(int VideoIndex)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate1Int(Play), VideoIndex);
            }
            else
            {
                if (Videos.Items.Count >= VideoIndex)
                {
                    ChosenVideo = ((VideoListViewItem)Videos.Items[VideoIndex]).VideoUrl;
                    this.Close();
                }
            }
        }
        protected void AddLetter(char Letter)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate1Char(AddLetter), Letter);
            }
            else
            {
                SearchBox.Text += Letter;
            }
        }
        protected void RemoveLastLetter()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(RemoveLastLetter));
            }
            else
            {
                SearchBox.Text.Remove(SearchBox.Text.Length - 1);
            }
        }

        protected void SetSearchResults()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(SetSearchResults));
            }
            else
            {
                SearchResults.Value = SearchResultsQuantity;
            }
        }
        #endregion

        #region Handlers
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

        protected override void OnLoad(EventArgs e)
        {
            // Set the focus to this control
            Videos.Focus();

            base.OnLoad(e);
        }
        #endregion

        #endregion
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
        public Image Thumbnail
        {
            get
            {
                if (m_Thumbnail != null)
                {
                    // If the thumbnail has been downloaded
                    return m_Thumbnail;
                }
                else
                {
                    // Download the thumbnail and store
                    if (!string.IsNullOrWhiteSpace(ThumbnailUrl))
                    {
                        WebRequest ThumbnailRequest = WebRequest.Create(ThumbnailUrl);
                        m_Thumbnail = Image.FromStream(ThumbnailRequest.GetResponse().GetResponseStream());
                        return m_Thumbnail;
                    }
                }

                // No thumbnail url specified
                return null;
            }
            set
            {
                Thumbnail = value;
            }
        }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }

        private Image m_Thumbnail;
    }
}
