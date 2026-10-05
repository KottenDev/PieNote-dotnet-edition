using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace PieNote
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private readonly TextBox txt;
        private string? currentFilePath;
        private readonly MenuStrip menuStrip;

        public MainForm()
        {
            this.Text = "PieNote";
            this.Size = new Size(500, 400);


            menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("New", null, (s, e) => NewFile());
            fileMenu.DropDownItems.Add("Open...", null, (s, e) => OpenFile());
            fileMenu.DropDownItems.Add("Save", null, (s, e) => SaveFile());
            fileMenu.DropDownItems.Add("Save As...", null, (s, e) => SaveFileAs());
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => Application.Exit());

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About", null, (s, e) => OpenAbout());

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(helpMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);


            txt = new TextBox();
            txt.Multiline = true;
            txt.ScrollBars = ScrollBars.Both;
            txt.Dock = DockStyle.Fill;
            

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(10);
            panel.Controls.Add(txt);
            this.Controls.Add(panel);


            if (File.Exists("PieNote.ico"))
            {
                this.Icon = new Icon("PieNote.ico");
            }
        }

        private void NewFile()
        {
            currentFilePath = null;
            txt.Clear();
            this.Text = "Untitled - PieNote";
        }

        private void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = openFileDialog.FileName;
                    try
                    {
                        txt.Text = File.ReadAllText(currentFilePath);
                        this.Text = $"{currentFilePath} - PieNote";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to open file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveFile()
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                try
                {
                    File.WriteAllText(currentFilePath, txt.Text);
                    MessageBox.Show("File saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                SaveFileAs();
            }
        }

        private void SaveFileAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    try
                    {
                        File.WriteAllText(currentFilePath, txt.Text);
                        this.Text = $"{currentFilePath} - PieNote";
                        MessageBox.Show("File saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OpenAbout()
        {
            Form aboutWindow = new Form();
            aboutWindow.Text = "About";
            aboutWindow.Size = new Size(250, 150);
            aboutWindow.StartPosition = FormStartPosition.Manual;
            aboutWindow.FormBorderStyle = FormBorderStyle.FixedDialog;
            aboutWindow.MaximizeBox = false;
            aboutWindow.MinimizeBox = false;
            aboutWindow.Owner = this;

            int centerX = this.Location.X + (this.Width / 2) - (aboutWindow.Width / 2);
            int centerY = this.Location.Y + (this.Height / 2) - (aboutWindow.Height / 2);
            aboutWindow.Location = new Point(centerX, centerY);

            Label label = new Label();
            label.Text = "PieNote\nVersion 1.1\nBuilt By kotten.";
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            aboutWindow.Controls.Add(label);

            aboutWindow.ShowDialog(this);
        }
    }
}