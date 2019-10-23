﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.IO;
using OsEngine.Market.Servers;

namespace OsEngine.Logging
{
    /// <summary>
    /// менеджер рассылки
    /// </summary>
    public class MessageSender
    {
 // настройки рассылки
        public bool MailSendOn;

        public bool MailSystemSendOn;
        public bool MailSignalSendOn;
        public bool MailErrorSendOn;
        public bool MailConnectSendOn;
        public bool MailTradeSendOn;
        public bool MailNoNameSendOn;

        public bool SmsSendOn;

        public bool SmsSystemSendOn;
        public bool SmsSignalSendOn;
        public bool SmsErrorSendOn;
        public bool SmsConnectSendOn;
        public bool SmsTradeSendOn;
        public bool SmsNoNameSendOn;

        private string _name; // имя

        public MessageSender(string name)
        {
            _name = name;
            Load();
        }

        /// <summary>
        /// показать окно настроек
        /// </summary>
        public void ShowDialog()
        {
            MessageSenderUi ui = new MessageSenderUi(this);
            ui.ShowDialog();
        }

        /// <summary>
        /// загрузить
        /// </summary>
        private void Load() 
        {
            if (!File.Exists(@"Engine\" + _name + @"MessageSender.txt"))
            {
                return;
            }
            try
            {
                using (StreamReader reader = new StreamReader(@"Engine\" + _name + @"MessageSender.txt"))
                {

                    MailSendOn =  Convert.ToBoolean(reader.ReadLine());

                    MailSystemSendOn = Convert.ToBoolean(reader.ReadLine());
                    MailSignalSendOn = Convert.ToBoolean(reader.ReadLine());
                    MailErrorSendOn = Convert.ToBoolean(reader.ReadLine());
                    MailConnectSendOn = Convert.ToBoolean(reader.ReadLine());
                    MailTradeSendOn = Convert.ToBoolean(reader.ReadLine());
                    MailNoNameSendOn = Convert.ToBoolean(reader.ReadLine());

                    SmsSendOn = Convert.ToBoolean(reader.ReadLine());

                    SmsSystemSendOn = Convert.ToBoolean(reader.ReadLine());
                    SmsSignalSendOn = Convert.ToBoolean(reader.ReadLine());
                    SmsErrorSendOn = Convert.ToBoolean(reader.ReadLine());
                    SmsConnectSendOn = Convert.ToBoolean(reader.ReadLine());
                    SmsTradeSendOn = Convert.ToBoolean(reader.ReadLine());
                    SmsNoNameSendOn = Convert.ToBoolean(reader.ReadLine());

                    reader.Close();
                }
            }
            catch (Exception)
            {
                // ignore
            }

        }

        /// <summary>
        /// сохранить
        /// </summary>
        public void Save() 
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(@"Engine\" + _name + @"MessageSender.txt", false))
                {
                    writer.WriteLine(MailSendOn);

                    writer.WriteLine(MailSystemSendOn);
                    writer.WriteLine(MailSignalSendOn);
                    writer.WriteLine(MailErrorSendOn);
                    writer.WriteLine(MailConnectSendOn);
                    writer.WriteLine(MailTradeSendOn);
                    writer.WriteLine(MailNoNameSendOn);

                    writer.WriteLine(SmsSendOn);

                   writer.WriteLine( SmsSystemSendOn);
                    writer.WriteLine(SmsSignalSendOn);
                    writer.WriteLine(SmsErrorSendOn);
                    writer.WriteLine(SmsConnectSendOn);
                    writer.WriteLine(SmsTradeSendOn);
                    writer.WriteLine(SmsNoNameSendOn);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// удалить
        /// </summary>
        public void Delete() 
        {
            if (File.Exists(@"Engine\" + _name + @"MessageSender.txt"))
            {
                File.Delete(@"Engine\" + _name + @"MessageSender.txt");
            }
        }

        /// <summary>
        /// Отправить сообщение. Если такой тип сообщений подписан на рассылку и сервера рассылки настроены, сообщение будет отправлено
        /// Если включен тестовый сервер - сообщение не будет отправленно
        /// </summary>
        public void AddNewMessage(LogMessage message)
        {
            if (ServerMaster.StartProgram != ServerStartProgramm.IsOsTrader)
            {
                return;
            }

            if (MailSendOn)
            {
                if (message.Type == LogMessageType.Connect &&
                    MailConnectSendOn)
                {
                    ServerMail.GetServer().Send(message, _name);
                }
                if (message.Type == LogMessageType.Error &&
                MailErrorSendOn)
                {
                    ServerMail.GetServer().Send(message, _name);
                }
                if (message.Type == LogMessageType.Signal &&
                    MailSignalSendOn)
                {
                    ServerMail.GetServer().Send(message, _name);
                }
                if (message.Type == LogMessageType.System &&
                    MailSystemSendOn)
                {
                    ServerMail.GetServer().Send(message, _name);
                }
                if (message.Type == LogMessageType.Trade &&
                    MailTradeSendOn)
                {
                    ServerMail.GetServer().Send(message, _name);
                }
            }
            if (SmsSendOn)
            {
                if (message.Type == LogMessageType.Connect &&
                    SmsConnectSendOn)
                {
                    ServerSms.GetSmsServer().Send(message.GetString());
                }
                if (message.Type == LogMessageType.Error &&
                SmsErrorSendOn)
                {
                    ServerSms.GetSmsServer().Send(message.GetString());
                }
                if (message.Type == LogMessageType.Signal &&
                    SmsSignalSendOn)
                {
                    ServerSms.GetSmsServer().Send(message.GetString());
                }
                if (message.Type == LogMessageType.System &&
                    SmsSystemSendOn)
                {
                    ServerSms.GetSmsServer().Send(message.GetString());
                }
                if (message.Type == LogMessageType.Trade &&
                    SmsTradeSendOn)
                {
                    ServerSms.GetSmsServer().Send(message.GetString());
                }
            }
        }
    }
}
