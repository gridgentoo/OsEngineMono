﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System.Windows;

namespace OsEngine.Logging
{
    /// <summary>
    /// Окно настроек сервера почтовой рассылки
    /// </summary>
    public partial class ServerSmsUi
    {
        public ServerSmsUi() // конструктор
        {
            InitializeComponent();

            ServerSms serverSms = ServerSms.GetSmsServer();

            TextBoxMyLogin.Text = serverSms.SmscLogin;
            TextBoxPassword.Text = serverSms.SmscPassword;

            if (serverSms.Phones != null)
            {
                string[] phones = serverSms.Phones.Split(',');

                for (int i = 0; i < phones.Length -1; i++)
                {
                    TextBoxFones.Text += phones[i] + "\n";
                }
            }
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e) // принять
        {
            ServerSms serverSms = ServerSms.GetSmsServer();
            serverSms.SmscLogin = TextBoxMyLogin.Text;
            serverSms.SmscPassword = TextBoxPassword.Text;

            string[] lockal = TextBoxFones.Text.Split('\n');

            string shit = "";

            for (int i = 0; i < lockal.Length; i++)
            {
                shit += lockal[i] += ",";
            }

            serverSms.Phones = shit;

            serverSms.Save();
            Close();
        }
    }
}
