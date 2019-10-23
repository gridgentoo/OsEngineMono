﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace OsEngine.Entity
{
    /// <summary>
    /// Свеча
    /// </summary>
    public class Candle
    {
        /// <summary>
        /// время начала свечи
        /// </summary>
        public DateTime TimeStart;

        /// <summary>
        /// цена открытия
        /// </summary>
        public decimal Open;

        /// <summary>
        /// максимальная цена за период
        /// </summary>
        public decimal High;

        /// <summary>
        /// цена закрытия
        /// </summary>
        public decimal Close;

        /// <summary>
        /// минимальная цена за период
        /// </summary>
        public decimal Low;

        /// <summary>
        /// объём
        /// </summary>
        public decimal Volume;

        /// <summary>
        /// статус завершённости свечи
        /// </summary>
        public CandleStates State;

        /// <summary>
        /// трейды составляющие эту свечу
        /// </summary>
        public List<Trade> Trades;

        /// <summary>
        /// растущая ли эта свеча
        /// </summary>
        public bool IsUp
        {
            get
            {
                if (Close > Open)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// падающая ли эта свеча
        /// </summary>
        public bool IsDown
        {
            get
            {
                if (Close < Open)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// загрузить состояние свечи из строки
        /// </summary>
        /// <param name="In">строка состояния</param>
        public void SetCandleFromString(string In)
        {
//20131001,100000,97.8000000,97.9900000,97.7500000,97.9000000,1
            //<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>
            string[] sIn = In.Split(',');

            int year = Convert.ToInt32(sIn[0].Substring(0, 4));
            int month = Convert.ToInt32(sIn[0].Substring(4, 2));
            int day = Convert.ToInt32(sIn[0].Substring(6, 2));

            int hour = Convert.ToInt32(sIn[1].Substring(0, 2));
            int minute = Convert.ToInt32(sIn[1].Substring(2, 2));
            int second = Convert.ToInt32(sIn[1].Substring(4, 2));

            TimeStart = new DateTime(year, month, day, hour, minute, second);

            string[] shit = sIn[2].Split('.');
            if (shit.Length == 2)
            {
                if (!Decimal.TryParse(shit[0] + '.' + shit[1], out Open))
                {
                    Open = Convert.ToDecimal(shit[0] + ',' + shit[1]);
                }
            }
            else
            {
                Open = Convert.ToDecimal(shit[0]);
            }

            shit = sIn[3].Split('.');

            if (shit.Length == 2)
            {
                if (!Decimal.TryParse(shit[0] + '.' + shit[1], out High))
                {
                    High = Convert.ToDecimal(shit[0] + ',' + shit[1]);
                }
            }
            else
            {
                High = Convert.ToDecimal(shit[0]);
            }

            shit = sIn[4].Split('.');
            if (shit.Length == 2)
            {
                if (!Decimal.TryParse(shit[0] + '.' + shit[1], out Low))
                {
                    Low = Convert.ToDecimal(shit[0] + ',' + shit[1]);
                }
            }
            else
            {
                Low = Convert.ToDecimal(shit[0]);
            }

            shit = sIn[5].Split('.');
            if (shit.Length == 2)
            {
                if (!Decimal.TryParse(shit[0] + '.' + shit[1], out Close))
                {
                    Close = Convert.ToDecimal(shit[0] + ',' + shit[1]);
                }
            }
            else
            {
                Close = Convert.ToDecimal(shit[0]);
            }

            shit = sIn[6].Split('.');

            try
            {
                if (Convert.ToInt32(shit[0]) == 0)
                {
                    Volume = 1;
                }
                else
                {
                    if (shit.Length == 2 && Convert.ToDecimal(shit[1]) != 0)
                    {
                        if (!Decimal.TryParse(shit[0] + '.' + shit[1], out Volume))
                        {
                            Volume = Convert.ToDecimal(shit[0] + ',' + shit[1]);
                        }
                    }
                    else
                    {
                        Volume = Convert.ToDecimal(shit[0]);
                    }
                }

            }
            catch (Exception)
            {
                Volume = 1;
            }

        }

        /// <summary>
        /// взять строку с подписями
        /// </summary>
        public string GetBeautifulString()
        {
            //Date - 20131001 Time - 100000 
            // Open - 97.8000000 High - 97.9900000 Low - 97.7500000 Close - 97.9000000

            string result = string.Empty;

            if (TimeStart.Day > 9)
            {
                result += TimeStart.Day.ToString();
            }
            else
            {
                result += "0" + TimeStart.Day;
            }

            result += ".";

            if (TimeStart.Month > 9)
            {
                result += TimeStart.Month.ToString();
            }
            else
            {
                result += "0" + TimeStart.Month;
            }
            result += ".";
            result += TimeStart.Year.ToString();

            result += " ";

            if (TimeStart.Hour > 9)
            {
                result += TimeStart.Hour.ToString();
            }
            else
            {
                result += "0" + TimeStart.Hour;
            }

            result += ":";

            if (TimeStart.Minute > 9)
            {
                result += TimeStart.Minute.ToString();
            }
            else
            {
                result += "0" + TimeStart.Minute;
            }

            result += ":";

            if (TimeStart.Second > 9)
            {
                result += TimeStart.Second.ToString();
            }
            else
            {
                result += "0" + TimeStart.Second;
            }
            result += "  \r\n";

            result += " O: ";
            result += Open.ToString(new CultureInfo("ru-RU"));
            result += " H: ";
            result += High.ToString(new CultureInfo("ru-RU"));
            result += " L: ";
            result += Low.ToString(new CultureInfo("ru-RU"));
            result += " C: ";
            result += Close.ToString(new CultureInfo("ru-RU"));

            return result;
        }


        private string _stringToSave;
        private decimal _closeWhenGotLastString;
        public string StringToSave
        {
            get
            {
                if (_closeWhenGotLastString == Close)
                {// если мы уже брали свечи раньше, не рассчитываем заного строку
                    return _stringToSave;
                }

                _closeWhenGotLastString = Close;

                _stringToSave = "";

                //20131001,100000,97.8000000,97.9900000,97.7500000,97.9000000,1
                //<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>

                string result = "";
                result += TimeStart.ToString("yyyyMMdd,HHmmss") + ",";

               // result += TimeStart.Date.ToString("yyyyMMdd") + ",";
                //result += TimeStart.TimeOfDay.ToString("HHmmss") + ",";

                result += Open.ToString(new CultureInfo("en-US")) + ",";
                result += High.ToString(new CultureInfo("en-US")) + ",";
                result += Low.ToString(new CultureInfo("en-US")) + ",";
                result += Close.ToString(new CultureInfo("en-US")) + ",";
                result += Volume.ToString(new CultureInfo("en-US"));

                _stringToSave = result;

                return _stringToSave;
            }
        }

    }

    /// <summary>
    /// состояние формирования свечи
    /// </summary>
    public enum CandleStates
    {
        /// <summary>
        /// завершено
        /// </summary>
        Finished,

        /// <summary>
        /// начато
        /// </summary>
        Started,

        /// <summary>
        /// неизвестно
        /// </summary>
        None
    }
}
