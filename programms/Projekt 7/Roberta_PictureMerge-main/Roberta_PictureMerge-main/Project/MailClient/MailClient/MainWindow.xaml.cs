using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MailClient {
    /// <summary>
    /// Program by Moritz Bernhofer
    /// 2BHIF
    /// 2023
    /// dc: Moritz#6043
    /// </summary>
    public partial class MainWindow : Window {
        public bool EmailLock { get; set; } = false;
        private String path = @"..\..\..\..\..\OutputPictures";
        private List<string> filepaths = new List<string>();
        private String currentPicturePath = String.Empty;
        private DispatcherTimer LookForPictureTimer;


        public MainWindow() {
            InitializeComponent();
            LookForPictureTimer= new DispatcherTimer();
            LookForPictureTimer.Interval = TimeSpan.FromSeconds(1);
            LookForPictureTimer.Tick += LockForPicture;
            LookForPictureTimer.Start();
        }

        private void LockForPicture(object? sender, EventArgs args) {
            if (!Directory.Exists(path)) return;

            string[] files = Directory.GetFiles(path);

            if(files.Length == 0) return;
            foreach(string filePath in files) {

                FileInfo fileInfo= new FileInfo(filePath);
                if (fileInfo.Extension != ".png") return;

                if(!filepaths.Contains(filePath)) {
                    currentPicturePath= filePath;
                    filepaths.Add(filePath);
                    UpdateImage();
                    UpdateFileName(fileInfo.Name);
                }
            }
                
        }

        private void UpdateFileName(string name) {
            fileName.Text = name;
        }

        private void UpdateImage() {
            string path = System.IO.Path.GetFullPath(currentPicturePath);
            PictureDisplay.Source = new BitmapImage(new Uri(path));
        }


        private void Email_TextBox_KeyPress(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) {
                return;
            }
            if (EmailLock) {
                Console.Text += "\n" + "wating for Email To Send";
                return;
            }
            if (!EmailBox.Text.Contains("@") || EmailBox.Text == String.Empty) {
                Console.Text += "\n" + "no Email recognised";
                return;
            }
            Console.Text += "\n" + "trying to Send...";
            EmailLock = true;
            SendMail(EmailBox.Text);
            EmailBox.Text = String.Empty;
        }

        private void SendMail(string text) {
            try {
                using (MailMessage mail = new($"{text}", $"{text}", "EmailTest", "TestImageSend"))
                using(Attachment data = new(currentPicturePath))
                using (SmtpClient smtp = new("smtp.gmail.com", 587)) {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("naoteamhtlleonding@gmail.com", "bsmvxwdldadbhjci");
                    smtp.EnableSsl = true;
                    mail.Attachments.Add(data);
                    smtp.Send(mail);
                }
                Console.Text += "\nEmail succesfully send";
                EmailLock = false;
            }
            catch (Exception ex) {
                Console.Text += "\n Email send not succesfull error: ";
                Console.Text += $"\n{ex.Message}";
                EmailLock = false;
            }
            
        }
    }
}

