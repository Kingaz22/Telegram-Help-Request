using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows;

namespace Telegram_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class SetingTelegram
        {
            public string Token { get; set; }
            public string ChatId { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            string token, chatId, userNameWin, compName, text, str;

            userNameWin = Environment.UserName;
            compName = Environment.MachineName;
            try
            {

                using (FileStream fs = new FileStream("setings.json", FileMode.OpenOrCreate))
                {
                    SetingTelegram restoredPerson = await JsonSerializer.DeserializeAsync<SetingTelegram>(fs);
                    token = restoredPerson.Token;
                    chatId = restoredPerson.ChatId;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не найден файл настроек");
                throw;
            }


            str = "<b>Имя пользователя: </b><i> " + userNameWin + "</i>  <b>Имя компьютера:</b>  <i>" + compName + "</i> <b> Текст сообжения: </b><i>" + TextBox.Text + "</i>";



            text = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chatId + "&text=" + str + "&parse_mode=html";

            try
            {
                WebRequest request = WebRequest.Create(text);
                WebResponse response = await request.GetResponseAsync();
                await using (Stream stream = response.GetResponseStream())
                {
                    using StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException());
                    reader.ReadToEnd();
                }
                response.Close();
                MessageBox.Show("Сообщение отправлено");

            }
            catch (Exception)
            {
                MessageBox.Show("Сообщение не отправлено, проверте подключение интернета");
                throw;
            }

            
        }
    }
}
