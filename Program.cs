using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GestionarPapelera
{
    static class Program
    {
        // CAMBIA esta contraseña antes de compilar.
        // La contraseña NO se guarda en texto plano: se compara mediante SHA-256.
        private const string PasswordHashHex = "REPLACE_WITH_SHA256";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var form = new PasswordForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = "shell:RecycleBinFolder",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir la Papelera.\r\n\r\n" + ex.Message,
                    "Gestionar Papelera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static bool CheckPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hex = BitConverter.ToString(hash).Replace("-", "");
                return string.Equals(hex, PasswordHashHex, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    public class PasswordForm : Form
    {
        private TextBox passwordBox;
        private Button acceptButton;
        private Button cancelButton;
        private int attempts;

        public PasswordForm()
        {
            Text = "Gestionar Papelera";
            Width = 420;
            Height = 190;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = true;

            var label = new Label
            {
                Text = "Introduzca la contraseña para gestionar la Papelera:",
                AutoSize = true,
                Left = 20,
                Top = 20
            };

            passwordBox = new TextBox
            {
                Left = 20,
                Top = 52,
                Width = 360,
                UseSystemPasswordChar = true
            };

            acceptButton = new Button
            {
                Text = "Aceptar",
                Left = 205,
                Top = 95,
                Width = 85
            };
            acceptButton.Click += AcceptButton_Click;

            cancelButton = new Button
            {
                Text = "Cancelar",
                Left = 295,
                Top = 95,
                Width = 85,
                DialogResult = DialogResult.Cancel
            };

            AcceptButton = acceptButton;
            CancelButton = cancelButton;

            Controls.Add(label);
            Controls.Add(passwordBox);
            Controls.Add(acceptButton);
            Controls.Add(cancelButton);

            Shown += (s, e) => passwordBox.Focus();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (Program.CheckPassword(passwordBox.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            attempts++;
            passwordBox.Clear();
            MessageBox.Show(
                "Contraseña incorrecta.",
                "Gestionar Papelera",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            if (attempts >= 5)
            {
                MessageBox.Show(
                    "Se han superado 5 intentos.",
                    "Gestionar Papelera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                passwordBox.Focus();
            }
        }
    }
}
