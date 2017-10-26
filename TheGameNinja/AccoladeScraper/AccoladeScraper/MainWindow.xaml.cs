using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Windows.Controls;

namespace AccoladeScraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccoladesEntities db = new AccoladesEntities();
        
        public MainWindow()
        {
            InitializeComponent();
            InitControls();
            
        }

        public void InitControls()
        {
            txtImagesPath.Text =
                System.Configuration.ConfigurationManager.AppSettings.Get("defaultImagePath");

            IQueryable<VideoGame> videoGames =
                from vg in db.VideoGames orderby vg.Name select vg;

            cmbVideoGame.ItemsSource = videoGames;
            cmbVideoGame.SelectedValuePath = "Id";
            cmbVideoGame.DisplayMemberPath = "Name";

            tbInfo.Text = db.Connection.DataSource;

            ShowConnectionStatus();
        }

        public static string ScreenScrape(string url)
        {
            return new System.Net.WebClient().DownloadString(url);
        }

        private void btnScrape_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVideoGame.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Please select a game before attempting a scrape.");
            }
            else
            {
                tbHTML.Clear();
                //tbHTML.Text += "VideoGameID,ImageUrl,Name,Points,Description\n";
            
                HtmlWeb hw = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(txtGameUrl.Text);
                HtmlNodeCollection htmlNodeCollection = doc.DocumentNode.SelectNodes("//tr");

                foreach (HtmlNode link in htmlNodeCollection.Nodes())
                {
                    HtmlAttribute att = link.Attributes["class"];
                    if (att != null)
                    {
                        if (att.Value == "ac1")
                        {
                            HtmlNodeCollection nc = link.ChildNodes;
                            foreach (HtmlNode hn in nc.Nodes())
                            {
                                HtmlAttribute a = hn.Attributes["src"];
                                if (a != null)
                                {
                                    //tbHTML.Text += "IMAGE: " + a.Value + "\n";
                                    tbHTML.Text += a.Value + "|";
                                }
                            }
                        }
                        if (att.Value == "ac2")
                        {
                            //tbHTML.Text += link.Name + link.InnerHtml + "\n";
                            tbHTML.Text += link.InnerText.Trim() + "|";
                        }
                        if (att.Value == "ac3")
                        {
                            string accoladeDescription = link.InnerText.Trim();

                            if (!(Regex.IsMatch(accoladeDescription, @"&nbsp;")) && !(Regex.IsMatch(accoladeDescription, @"\(\d+\)")))
                            {
                                accoladeDescription = Regex.Replace(accoladeDescription, @"[ \t]{2,}|\t", " ");
                                accoladeDescription = Regex.Replace(accoladeDescription, @"\r|\n", "");


                                //tbHTML.Text += "DESCRIPTION: " + accoladeDescription + "\n\n\n";
                                tbHTML.Text += accoladeDescription + "\n";
                            }
                        }
                        if (att.Value == "ac4")
                        {
                            //tbHTML.Text += "GAMER POINTS: " + link.InnerText.Trim() + "\n";
                            tbHTML.Text += link.InnerText.Trim() + "|";
                        }
                    }
                }
            }
            tbInfo.Text = "Scrape finished.";
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            AccoladesEntities db = new AccoladesEntities();

            // Check if you have already imported records for this VideoGameID
            int videogameID = Convert.ToInt32(cmbVideoGame.SelectedValue);

            if (db.Accolades.Where(v => v.VideoGameId == videogameID).Count() > 0)
            {
                System.Windows.MessageBox.Show("You have already imported Accolades for " +
                    db.VideoGames.Where(v => v.Id == videogameID).First().Name + ". Please check you have selected the correct game");

            }
            else
            {
                StringCollection lines = new StringCollection();
                int lineCount = tbHTML.LineCount;

                for (int line = 0; line < lineCount; line++)
                        lines.Add(tbHTML.GetLineText(line));

                foreach (string s in lines)
                {
                    string[] a;
                    a = s.Split('|');
                    // /images/accolades/271/0Dc8P2Nh.jpg|Completed Welcome.|10|Successfully complete the Welcome To Rapture Level.
                    if (a.Count() == 4)
                    {
                        Accolade accolade = new Accolade();
                        accolade.VideoGameId = Convert.ToInt32(cmbVideoGame.SelectedValue);
                        string videoGameName = GetVideoGameName();
                        string accoladeName = GetAccoladeName(a[1]);
                        string fileName = videoGameName + "_" + accoladeName + ".jpg";

                        accolade.ImageUrl = "C:\\Users\\Max\\Dropbox\\TGN Images\\AccoladeImages\\" + fileName;
                        accolade.Name = a[1];
                        
                        accolade.Description = a[3];
                        
                        db.Accolades.AddObject(accolade);
                        db.SaveChanges();

                    }
                }
            }
            tbInfo.Text = "Import finished";
        }

        private void btnGetImages_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("This process will attempt to copy the images for the selected game to a local folder.");

            if (cmbVideoGame.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Please select a game before attempting a scrape.");
            }

            StringCollection lines = new StringCollection();
            int lineCount = tbHTML.LineCount;

            for (int line = 0; line < lineCount; line++)
                    lines.Add(tbHTML.GetLineText(line));

            foreach (string s in lines)
            {
                string[] a;
                a = s.Split('|');
                // /images/accolades/271/0Dc8P2Nh.jpg|Completed Welcome.|10|Successfully complete the Welcome To Rapture Level.
                if (a.Count() == 4)
                {
                    string videoGameName = GetVideoGameName();

                    string accoladeName = GetAccoladeName(a[1]);

                    //TODO Assuming jpg
                    string fileName = videoGameName + "_" + accoladeName + ".jpg";

                    string imageUrl = txtImageUrl.Text  + a[0];

                    byte[] imageBytes = GetBytesFromUrl(imageUrl);

                    try
                    {
                        WriteBytesToFile(fileName, imageBytes);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        System.Windows.MessageBox.Show("The directory you specified was not found.");
                        return;
                    }

                }
            }
            tbInfo.Text = "Images retrieved.";
        }

        private static string GetAccoladeName(string a)
        {
            string accoladeName = Regex.Replace(a, "[^0-9a-zA-Z]+", "");
            return accoladeName;
        }

        private string GetVideoGameName()
        {
            // Prefix the filename with the VideoGame Name. Strip out spaces and special characters first.
            string videoGameName = Regex.Replace(cmbVideoGame.Text, "[^0-9a-zA-Z]+", "");
            return videoGameName;
        }

        private byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();
            //int i;
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            myResp.Close();
            return b;
        }

        private void WriteBytesToFile(string fileName, byte[] content)
        {
            fileName = txtImagesPath.Text + "\\" + fileName;

            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            try
            {
                w.Write(content);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                fs.Close();
                w.Close();
            }
        }

        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (db.Connection.State == System.Data.ConnectionState.Open)
            {
                db.Connection.Close();
            }
            else
            {
                db.Connection.Open();
            }

            ShowConnectionStatus();
        }

        private void ShowConnectionStatus()
        {
            txtDatabaseServer.Text = db.Connection.DataSource.ToString() + " Status: " + db.Connection.State;
        }

        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog selectFolderDialog = new FolderBrowserDialog();
            selectFolderDialog.ShowNewFolderButton = true;
            if (selectFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtImagesPath.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void cmbSite_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)cmbSite.SelectedItem;

            switch (comboBoxItem.Content.ToString())
            {
                case "Playstation Trophies":
                    {
                        txtGameUrl.Text = "http://www.playstationtrophies.org/game/alien-isolation/trophies/";
                        txtImageUrl.Text = "http://www.playstationtrophies.org/";
                        break;
                    }
                case "XBox Accolades":
                    {
                        txtGameUrl.Text = "http://www.xboxachievements.org/game/alien-isolation/achievements/";
                        txtImageUrl.Text = "http://www.xboxachievements.org/";
                        break;
                    }
                default:
                    {
                        txtGameUrl.Text = "Please select a site";
                        break;
                    }
            }
        }
    }
}
