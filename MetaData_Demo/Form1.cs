

using TagLib;

namespace MetaData_Demo
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;
        private TagLib.IPicture Picture;
        private MemoryStream imageStream;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "audio files|*.mp3;*.flac;*.m4a;*.wav";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
                using (var audioFile = TagLib.File.Create(fileDialog.FileName))
                {
                    selectedFilePath = fileDialog.FileName;
                    textBox1.Text = selectedFilePath;
                    MetaDataLoader(selectedFilePath);
                }
            }

        }
        private void MetaDataLoader(string path)
        {
            using (var mp3File = TagLib.File.Create(path))
            {
                string[]? genres = mp3File.Tag.Genres;

                artistBox.Text = mp3File.Tag.FirstAlbumArtist ?? string.Empty;
                albumBox.Text = mp3File.Tag.Album?.ToString() ?? string.Empty;
                trackBox.Text = mp3File.Tag.Track.ToString() ?? string.Empty;
                yearBox.Text = mp3File.Tag.Year.ToString() ?? string.Empty;
                titleBox.Text = mp3File.Tag.Title ?? string.Empty;

                if (genres != null)
                {
                    foreach (var genre in genres)
                    {
                        genreBox.Text = $"{genre} ";
                    }
                }

                if (mp3File.Tag.Pictures.Length > 0)
                {
                    var picture = mp3File.Tag.Pictures[0];
                    using (MemoryStream ms = new MemoryStream(picture.Data.Data))
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }

            }
        }

        private void Save(string path)
        {
            using (var mp3File = TagLib.File.Create(path))
            {
                string[] mainArtists = artistBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] contributingArtists = contributingArtistBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] genres = genreBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                mp3File.Tag.Performers = mainArtists;
                mp3File.Tag.Performers = contributingArtists;

                mp3File.Tag.Track = uint.Parse(trackBox.Text);
                mp3File.Tag.Album = albumBox.Text;
                mp3File.Tag.Year = uint.Parse(yearBox.Text);
                mp3File.Tag.Title = titleBox.Text;
                mp3File.Tag.Genres = genres;


                if (imageStream != null)
                {
                    imageStream.Position = 0;
                    mp3File.Tag.Pictures = new Picture[]
                    {
                        new TagLib.Picture(ByteVector.FromStream(imageStream))
                    };
                }

                mp3File.Save();
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                Save(selectedFilePath);
                MessageBox.Show("Metadata saved successfully.");
            }
            else
            {
                throw new Exception("Please select an audio file");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Pictures|*.jpg;*.png";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        imageStream = new();
                        fs.CopyTo(imageStream);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = Image.FromStream(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not load the image. " + ex.Message);
                }
            }
        }
    }
}
