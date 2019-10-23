﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Windows;
using OsEngine.Logging;

namespace OsEngine.Market.Servers.Finam
{
    /// <summary>
    /// Логика взаимодействия для FinamServerUi.xaml
    /// </summary>
    public partial class FinamServerUi
    {
        private FinamServer _server;
        public FinamServerUi(FinamServer server, Log log)
        {
            InitializeComponent();
            _server = server;

            TextBoxServerAdress.Text = _server.ServerAdress;

            LabelStatus.Content = _server.ServerStatus;
            _server.ConnectStatusChangeEvent += _server_ConnectStatusChangeEvent;
            log.StartPaint(Host);
        }

        void _server_ConnectStatusChangeEvent(string state)
        {
            if (!TextBoxServerAdress.Dispatcher.CheckAccess())
            {
                TextBoxServerAdress.Dispatcher.Invoke(new Action<string>(_server_ConnectStatusChangeEvent), state);
                return;
            }

            LabelStatus.Content = state;
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            _server.ServerAdress = TextBoxServerAdress.Text;
            _server.Save();
            _server.StartServer();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            _server.StopServer();
        }
    }
}
