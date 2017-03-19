using Ookii.Dialogs;
using System;
using System.IO;
using System.Windows.Forms;

namespace RocksmithToolkitGUI.DLCPackageCreator
{
    public partial class VocalsForm : Form
    {
        public VocalsForm(string fontSngPath, string lyricArtPath, bool isCustom, string vocalsXmlPath)
        {
            InitializeComponent();
            SngPath = fontSngPath;
            ArtPath = lyricArtPath;
            IsCustom = isCustom;
            VocalsPath = vocalsXmlPath;
            PopulateKey();
            PopulateRichText();
        }

        public string ArtPath
        {
            get { return txtVocalsDdsPath.Text; }
            set { txtVocalsDdsPath.Text = value; }
        }

        public bool IsCustom
        {
            get { return chkCustomFont.Checked; }
            set { chkCustomFont.Checked = value; }
        }

        public string SngPath
        {
            get { return txtVocalsSngPath.Text; }
            set { txtVocalsSngPath.Text = value; }
        }

        public string VocalsPath
        {
            get { return txtVocalsXmlPath.Text; }
            set { txtVocalsXmlPath.Text = value; }
        }

        public void PopulateRichText()
        {
            if (!String.IsNullOrEmpty(VocalsPath))
                rtbVocals.LoadFile(VocalsPath, RichTextBoxStreamType.PlainText);
        }

        private void PopulateKey()
        {
            lstKey.Items.Add("Rocksmith Lyric Characters");
            lstKey.Items.Add("");
            lstKey.Items.Add("'-' individually highlight syllables");
            lstKey.Items.Add("'--' show as seperate syllables/words");
            lstKey.Items.Add("'+' split over new line or add line");
            lstKey.Items.Add("'++' split over two new lines");
            lstKey.Items.Add("");
            lstKey.Items.Add("periods and commas are not used");
            lstKey.Items.Add("'=' used to produce '-' in game");
            lstKey.Items.Add("");
            lstKey.Items.Add("Submit additional keys to Developers");
        }

        void btnVocalsXmlPath_Click(object sender, EventArgs e)
        {
            using (var f = new VistaOpenFileDialog())
            {
                f.FileName = VocalsPath;
                f.Filter = "Rocksmith XML Vocals Files (*_Vocals.xml)|*_Vocals.xml";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    VocalsPath = f.FileName;
                    PopulateRichText();
                }
            }
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VocalsPath))
                rtbVocals.SaveFile(VocalsPath, RichTextBoxStreamType.PlainText);

            if (File.Exists(SngPath) && File.Exists(ArtPath) || !IsCustom)
            {
                if (!IsCustom)
                    SngPath = "";

                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("One of required files are missing, please select both required files and try again.\r\n", DLCPackageCreator.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnVocalsDdsPath_Click(object sender, EventArgs e)
        {
            using (var f = new VistaOpenFileDialog())
            {
                f.FileName = ArtPath;
                f.Filter = "Rocksmith DDS Art Files (*.dds)|*.dds";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ArtPath = f.FileName;
                }
            }
        }

        void btnVocalsSngPath_Click(object sender, EventArgs e)
        {
            using (var f = new VistaOpenFileDialog())
            {
                f.FileName = SngPath;
                f.Filter = "Rocksmith SNG Custom Font Files (*.sng)|*.sng";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ArtPath = f.FileName;
                    IsCustom = true; //possible jvocals and regular vocals, but supported only one custom font texture
                }
            }
        }
    }
}
