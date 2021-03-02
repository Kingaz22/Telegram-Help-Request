using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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

namespace Telegram_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class SettingTelegram
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

            try
            {
                string token, chatId;

                await using (var fs = new FileStream("setings.json", FileMode.OpenOrCreate))
                {
                    var settingTelegram = await JsonSerializer.DeserializeAsync<SettingTelegram>(fs);
                    token = settingTelegram.Token;
                    chatId = settingTelegram.ChatId;
                }
                var text = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chatId + "&text=" +
                           "<b>Имя пользователя: </b><i> " + Environment.UserName + "</i>\n" +
                           "<b>Имя компьютера:</b> <i>" + Environment.MachineName + "</i>\n" +
                           "<b>Контакт:</b> <i>" + TextBox1.Text + "</i>\n" +
                           "<b>Текст сообщения: </b><i>" + TextBox.Text + "</i>" +
                           "&parse_mode=html";

                var request = WebRequest.Create(text);
                var response = await request.GetResponseAsync();
                await using (var stream = response.GetResponseStream())
                {
                    using var reader = new StreamReader(stream ?? throw new InvalidOperationException());
                    await reader.ReadToEndAsync();
                }
                response.Close();
                MessageBox.Show("Сообщение отправлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message + "\n Свяжитесь со специалистом по телефону +375-44-789-48-07");
            }

        }
    }
}
