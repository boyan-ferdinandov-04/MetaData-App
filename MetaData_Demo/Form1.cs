using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace MetaData_Demo
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

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
            fileDialog.Filter = "mp3 files|*.mp3";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
                using (var mp3File = TagLib.File.Create(fileDialog.FileName))
                {
                    selectedFilePath = fileDialog.FileName;
                    textBox1.Text = selectedFilePath;

                }
            }

        }
        private void MetaDataLoader(string path)
        {
            using (var mp3File = TagLib.File.Create(path))
            {
                artistBox.Text = mp3File.Tag.FirstAlbumArtist;
                albumBox.Text = mp3File.Tag.Album.ToString();
                trackBox.Text = mp3File.Tag.Track.ToString();
                yearBox.Text = mp3File.Tag.Year.ToString();
                titleBox.Text = mp3File.Tag.Title.ToString();
            }
        }

        private void Save(string path)
        {
            using (var mp3File = TagLib.File.Create(path))
            {
                mp3File.Tag.AlbumArtists = new string[] { artistBox.Text };
                mp3File.Tag.Track = uint.Parse(trackBox.Text);
                mp3File.Tag.Album = albumBox.Text;
                mp3File.Tag.Year = uint.Parse(yearBox.Text);
                mp3File.Tag.Title = titleBox.Text;
                mp3File.Save();
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(selectedFilePath))
            {
                Save(selectedFilePath);
                MessageBox.Show("Metadata saved successfully.");
            }
            else
            {
                throw new Exception("Please select an mp3 file");
            }
        }
    }
}
