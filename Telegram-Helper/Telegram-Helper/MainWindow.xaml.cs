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

            string token, chatId, text;

            try
            {
                using (FileStream fs = new FileStream("setings.json", FileMode.OpenOrCreate))
                {
                    SetingTelegram setingTelegram = await JsonSerializer.DeserializeAsync<SetingTelegram>(fs);
                    token = setingTelegram.Token;
                    chatId = setingTelegram.ChatId;
                }
                text = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chatId + "&text=" +
                       "<b>Имя пользователя: </b><i> " + Environment.UserName + "</i>\n" +
                       "<b>Имя компьютера:</b> <i>" + Environment.MachineName + "</i>>\n" +
                       "<b>Текст сообжения: </b><i>" + TextBox.Text + "</i>" +
                       "&parse_mode=html";

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
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message + "\n Свяжитесь с администарторм сетей");
            }

        }

    }
}
