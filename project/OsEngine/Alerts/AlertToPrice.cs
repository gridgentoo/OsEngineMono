﻿/*
 *Ваши права на использования кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Collections.Generic;
using System.IO;
using OsEngine.Entity;
using OsEngine.Properties;

namespace OsEngine.Alerts
{
    public class AlertToPrice: IIAlert
    {
        public AlertToPrice(string name)
        {
            TypeAlert = AlertType.PriceAlert;
            SignalType = SignalType.None;
            MusicType = AlertMusic.Duck;

            Name = name;
        }

        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(@"Engine\" + Name + @"Alert.txt", false))
                {
                    writer.WriteLine(Message);
                    writer.WriteLine(IsOn);
                    writer.WriteLine(MessageIsOn);
                    writer.WriteLine(MusicType);

                    writer.WriteLine(SignalType);
                    writer.WriteLine(VolumeReaction);
                    writer.WriteLine(Slippage);
                    writer.WriteLine(NumberClosePosition);
                    writer.WriteLine(OrderPriceType);

                    writer.WriteLine(TypeActivation);
                    writer.WriteLine(PriceActivation);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public void Load()
        {
            if (!File.Exists(@"Engine\" + Name + @"Alert.txt"))
            {
                return;
            }
            try
            {
                using (StreamReader reader = new StreamReader(@"Engine\" + Name + @"Alert.txt"))
                {
                    Message = reader.ReadLine();
                    IsOn = Convert.ToBoolean(reader.ReadLine());
                    MessageIsOn = Convert.ToBoolean(reader.ReadLine());

                    Enum.TryParse(reader.ReadLine(), out MusicType);
                    Enum.TryParse(reader.ReadLine(), true, out SignalType);
                    VolumeReaction = Convert.ToInt32(reader.ReadLine());
                    Slippage = Convert.ToDecimal(reader.ReadLine());
                    NumberClosePosition = Convert.ToInt32(reader.ReadLine());
                    Enum.TryParse(reader.ReadLine(), true, out OrderPriceType);

                    reader.Close();
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public void Delete()
        {
            if (File.Exists(@"Engine\" + Name + @"Alert.txt"))
            {
                File.Delete(@"Engine\" + Name + @"Alert.txt");
            }
        }

        public void ShowDialog()
        {
            AlertToPriceCreateUi ui = new AlertToPriceCreateUi(this);
            ui.ShowDialog();
        }

        public bool IsOn { get; set; }

        public string Name { get; set; }

        public AlertType TypeAlert { get; set; }

        public AlertSignal CheckSignal(List<Candle> candles)
        {
            if (IsOn == false || candles == null)
            {
                return null;
            }


            // 3 бежим по линиям аллерта и проверяем срабатывание

            if (TypeActivation == PriceAlertTypeActivation.PriceLowerOrEqual &&
                candles[candles.Count - 1].Close <= PriceActivation ||
                TypeActivation == PriceAlertTypeActivation.PriceHigherOrEqual &&
                candles[candles.Count - 1].Close >= PriceActivation)
            {
                IsOn = false;
                if (MessageIsOn)
                {
                    UnmanagedMemoryStream stream = Resources.Bird;

                    if (MusicType == AlertMusic.Duck)
                    {
                        stream = Resources.Duck;
                    }
                    if (MusicType == AlertMusic.Wolf)
                    {
                        stream = Resources.wolf01;
                    }

                    AlertMessageManager.ThrowAlert(stream, Name, Message);
                }
                if (SignalType != SignalType.None)
                {
                    return new AlertSignal { SignalType = SignalType, Volume = VolumeReaction, NumberClosingPosition = NumberClosePosition, PriceType = OrderPriceType, Slipage = Slippage };
                }
            }

            return null;
        }

// индивидуальные настройки

        public PriceAlertTypeActivation TypeActivation;

        public decimal PriceActivation;

        public SignalType SignalType;

        /// <summary>
        /// объём для исполнения
        /// </summary>
        public int VolumeReaction;

        /// <summary>
        /// проскальзывание
        /// </summary>
        public decimal Slippage;

        /// <summary>
        /// номер позиции которая будет закрыта
        /// </summary>
        public int NumberClosePosition;

        /// <summary>
        /// тип ордера 
        /// </summary>
        public OrderPriceType OrderPriceType;

        /// <summary>
        /// влкючено ли выбрасывание Окна сообщения
        /// </summary>
        public bool MessageIsOn;

        /// <summary>
        /// текст сообщения, выбрасываемый при срабатывании Алерта
        /// </summary>
        public string Message;

        /// <summary>
        /// путь к файлу с музыкой
        /// </summary>
        public AlertMusic MusicType;

    }

    /// <summary>
    /// условие активации Алерта по цене
    /// </summary>
    public enum PriceAlertTypeActivation
    {
        /// <summary>
        /// цена выше или равна значению
        /// </summary>
        PriceHigherOrEqual,

        /// <summary>
        /// цена ниже или равна значению
        /// </summary>
        PriceLowerOrEqual
    }
}
