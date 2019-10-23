﻿/*
 *Ваши права на использования кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using OsEngine.Entity;
using MessageBox = System.Windows.MessageBox;

namespace OsEngine.Alerts
{

    /// <summary>
    /// Окно создания и редактирования алерта
    /// </summary>
    public partial class AlertToChartCreateUi
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="alert">алерт для редактирования, если null будет создан новый</param>
        /// <param name="keeper">хранилище алертов</param>
        public AlertToChartCreateUi(AlertToChart alert, AlertMaster keeper) 
        {
            InitializeComponent();

            _waitOne = false;
            _waitTwo = false;
            NeadToSave = false;
            _candleOneTime = DateTime.MinValue;
            _candleOneValue = 0;
            _candleTwoTime = DateTime.MinValue;
            _candleTwoValue = 0;
            _keeper = keeper;

            ComboBoxType.Items.Add(ChartAlertType.Line);
            ComboBoxType.Items.Add(ChartAlertType.FibonacciChannel);
            ComboBoxType.Items.Add(ChartAlertType.FibonacciSpeedLine);
            ComboBoxType.Items.Add(ChartAlertType.HorisontalLine);

            ComboBoxType.SelectedItem = ChartAlertType.Line;

            ComboBoxType.Text = ChartAlertType.Line.ToString();

// фейерверки по умолчанию
            CheckBoxOnOff.IsChecked = false;
            CheckBoxMusicAlert.IsChecked = false;
            CheckBoxWindow.IsChecked = false;

            ComboBoxFatLine.Text = "2";
            TextBoxLabelAlert.Text = "";

            System.Drawing.Color red = System.Drawing.Color.DarkRed;

            ButtonColorLabel.Background =
                new SolidColorBrush(Color.FromArgb(red.A,red.R,red.G,red.B));

            ButtonColorLine.Background =
                new SolidColorBrush(Color.FromArgb(red.A, red.R, red.G, red.B));

            CheckBoxWindow.IsChecked = false;
            TextBoxAlertMessage.Text = "Алерт Сработал!";

// торговые настойки по умолчанию
            TextBoxVolumeReaction.Text = "1";

            ComboBoxSignalType.Items.Add((SignalType.Buy));
            ComboBoxSignalType.Items.Add((SignalType.Sell));
            ComboBoxSignalType.Items.Add((SignalType.CloseAll));
            ComboBoxSignalType.Items.Add((SignalType.CloseOne));
            ComboBoxSignalType.Items.Add((SignalType.None));
            ComboBoxSignalType.SelectedItem = SignalType.None;
            ComboBoxSignalType.SelectionChanged += ComboBoxSignalType_SelectionChanged;

            ComboBoxOrderType.Items.Add((OrderPriceType.Market));
            ComboBoxOrderType.Items.Add((OrderPriceType.Limit));
            ComboBoxOrderType.SelectedItem = OrderPriceType.Market;

            TextBoxSlippage.Text = "0";
            TextBoxClosePosition.Text = "0";

            ComboBoxMusicType.Items.Add(AlertMusic.Bird);
            ComboBoxMusicType.Items.Add(AlertMusic.Duck);
            ComboBoxMusicType.Items.Add(AlertMusic.Wolf);
            ComboBoxMusicType.SelectedItem = AlertMusic.Bird;

            if (alert != null)
            {
                MyAlert = alert;
                LoadFromAlert();
                ComboBoxType.IsEnabled = false;
                NeadToSave = true;
            }
            
            CheckBoxOnOff.Click += CheckBoxOnOff_Click;
            CheckBoxMusicAlert.Click += CheckBoxOnOff_Click;
            CheckBoxWindow.Click += CheckBoxOnOff_Click;

            ComboBoxFatLine.SelectionChanged += ComboBoxFatLine_SelectionChanged;

            TextBoxAlertMessage.TextChanged += TextBoxAlertMessage_TextChanged;
            TextBoxLabelAlert.TextChanged += TextBoxAlertMessage_TextChanged;

            ComboBoxOrderType.SelectionChanged += ComboBoxOrderType_SelectionChanged;
            HideTradeButton();

            LabelOsa.MouseDown += LabelOsa_MouseDown;
        }

        void LabelOsa_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://o-s-a.net");
        }


        /// <summary>
        /// спрятать ненужные контролы для торговли
        /// </summary>
        private void HideTradeButton()
        {
            SignalType type;

            Enum.TryParse(ComboBoxSignalType.SelectedItem.ToString(), true, out type);

            if (type == SignalType.None)
            {
                TextBoxSlippage.IsEnabled = false;
                TextBoxClosePosition.IsEnabled = false;
                TextBoxVolumeReaction.IsEnabled = false;
                ComboBoxOrderType.IsEnabled = false;
                return;
            }
            else if (type == SignalType.CloseAll)
            {
                TextBoxSlippage.IsEnabled = true;
                TextBoxClosePosition.IsEnabled = false;
                TextBoxVolumeReaction.IsEnabled = true;
                ComboBoxOrderType.IsEnabled = true;
            }
            else if (type == SignalType.CloseOne)
            {
                TextBoxSlippage.IsEnabled = true;
                TextBoxClosePosition.IsEnabled = true;
                TextBoxVolumeReaction.IsEnabled = true;
                ComboBoxOrderType.IsEnabled = true;
            }
            else if (type == SignalType.Buy)
            {
                TextBoxSlippage.IsEnabled = true;
                TextBoxClosePosition.IsEnabled = false;
                TextBoxVolumeReaction.IsEnabled = true;
                ComboBoxOrderType.IsEnabled = true;
            }
            else if (type == SignalType.Sell)
            {
                TextBoxSlippage.IsEnabled = true;
                TextBoxClosePosition.IsEnabled = false;
                TextBoxVolumeReaction.IsEnabled = true;
                ComboBoxOrderType.IsEnabled = true;
            }

            OrderPriceType orderType;

            Enum.TryParse(ComboBoxOrderType.SelectedItem.ToString(), true, out orderType);

            if (orderType == OrderPriceType.Limit)
            {
                TextBoxSlippage.IsEnabled = true;
            }
            else
            {
                TextBoxSlippage.IsEnabled = false;
            }
        }

        /// <summary>
        /// хранилище Алертов
        /// </summary>
        private readonly AlertMaster _keeper; 

        /// <summary>
        /// текущий Алерт
        /// </summary>
        public AlertToChart MyAlert;

        /// <summary>
        /// нужно ли сохранять Алерт после закрытия окна
        /// </summary>
        public bool NeadToSave;

        /// <summary>
        /// загрузить настройки Алерта на форму
        /// </summary>
        private void LoadFromAlert()
        {
            ComboBoxSignalType.SelectedItem = MyAlert.SignalType;
            ComboBoxType.SelectedItem = MyAlert.Type;
            CheckBoxOnOff.IsChecked = MyAlert.IsOn;
            CheckBoxMusicAlert.IsChecked = MyAlert.IsMusicOn;
            ComboBoxMusicType.SelectedItem = MyAlert.Music;

            ComboBoxFatLine.Text = MyAlert.BorderWidth.ToString();
            TextBoxLabelAlert.Text = MyAlert.Label;

            System.Drawing.Color labelColor = MyAlert.ColorLabel;
            System.Drawing.Color labelLine = MyAlert.ColorLine;

            ButtonColorLabel.Background =
                new SolidColorBrush(Color.FromArgb(labelColor.A, labelColor.R, labelColor.G, labelColor.B));

            ButtonColorLine.Background =
                new SolidColorBrush(Color.FromArgb(labelLine.A, labelLine.R, labelLine.G, labelLine.B));


            CheckBoxWindow.IsChecked = MyAlert.IsMessageOn;
            TextBoxAlertMessage.Text = MyAlert.Message;
            TextBoxVolumeReaction.Text = MyAlert.VolumeReaction.ToString();

            TextBoxSlippage.Text = MyAlert.Slippage.ToString(new CultureInfo("ru-RU"));
            TextBoxClosePosition.Text = MyAlert.NumberClosePosition.ToString();
            TextBoxVolumeReaction.Text = MyAlert.VolumeReaction.ToString();
            ComboBoxOrderType.SelectedItem = MyAlert.OrderPriceType;
            ComboBoxSignalType.SelectedItem = MyAlert.SignalType;
        }

//перехват изменения контролов

        /// <summary>
        /// пользователь переключил тип ордера
        /// </summary>
        void ComboBoxOrderType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            HideTradeButton();
        }

        /// <summary>
        /// ползователь изменил тип сигнала
        /// </summary>
        void ComboBoxSignalType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            HideTradeButton();
        }

        /// <summary>
        /// клик на галочке вкл/выкл
        /// </summary>
        void CheckBoxOnOff_Click(object sender, RoutedEventArgs e)
        {
            SaveAlert();
        }

        /// <summary>
        /// тексе подписи алерта
        /// </summary>
        void TextBoxAlertMessage_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveAlert();
        }

        /// <summary>
        /// толщина линии изменена
        /// </summary>
        void ComboBoxFatLine_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SaveAlert();
        }

        //работа со слайдером

        /// <summary>
        /// изменилось положение слайдера
        /// </summary>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ComboBoxType.Text == ChartAlertType.Line.ToString() ||
                _waitOne|| 
                _waitTwo|| 
                _arrayCandles == null)
            {
                 return;
            }

            SetReadyLineAlert(_arrayCandles);

        }

        /// <summary>
        ///  последние входящие данные 
        /// </summary>
        private List<Candle> _arrayCandles;

        // ожидание входящих данных

        /// <summary>
        /// ждём клика по чарту чтобы прорисовать горизонтальную линию
        /// </summary>
        private bool _waitHorisont;

        /// <summary>
        /// ждём клика по чарту чтобы прорисовать горизонтальную линию
        /// </summary>
        private bool _waitOne;

        /// <summary>
        /// ждём клика по чарту чтобы прорисовать горизонтальную линию
        /// </summary>
        private bool _waitTwo;

        //точки

        /// <summary>
        /// индекс первой точки
        /// </summary>
        private int _candleOneNumber;

        /// <summary>
        /// время первой точки
        /// </summary>
        private DateTime _candleOneTime;

        /// <summary>
        /// значение первой точки
        /// </summary>
        private decimal _candleOneValue;

        /// <summary>
        /// индекс второй точки
        /// </summary>
        private int _candleTwoNumber;

        /// <summary>
        /// время второй точки
        /// </summary>
        private DateTime _candleTwoTime;

        /// <summary>
        /// значение второй точки
        /// </summary>
        private decimal _candleTwoValue;

        // создание алертов

        /// <summary>
        /// загрузить с чарта новую точку с данными
        /// </summary>
        /// <param name="arrayCandles">массив свечек</param>
        /// <param name="numberCandle">индекс свечи</param>
        /// <param name="valueY">значение игрик</param>
        public void SetFormChart(List<Candle> arrayCandles, int numberCandle, decimal valueY)
        {
            _arrayCandles = arrayCandles;

            if (_waitOne == false && _waitTwo == false && _waitHorisont == false)
            {
                return;
            }

            if (arrayCandles == null ||
                numberCandle < 0 ||
                numberCandle > arrayCandles.Count ||
                valueY <= 0 ||
                arrayCandles.Count < 10)
            {
                return;
            }

            // находим свечку, в которую ткнули

            Candle candle = arrayCandles[numberCandle];

            if (_waitOne)
            {
                if (candle.TimeStart == _candleTwoTime)
                {
                    MessageBox.Show("Линия не может быть построена из двух точек на одной оси Х");
                    return;
                }
                _candleOneTime = candle.TimeStart;
                _candleOneValue = valueY;
                _candleOneNumber = numberCandle;
                _waitOne = false;
                _waitTwo = true;
                return;
            }
            else if (_waitTwo)
            {
                if (candle.TimeStart == _candleOneTime)
                {
                    MessageBox.Show("Линия не может быть построена из двух точек на одной оси Х");
                    return;
                }

                _candleTwoTime = candle.TimeStart;
                _candleTwoValue = valueY;
                _candleTwoNumber = numberCandle;
                _waitTwo = false;
            }

            if (_waitHorisont)
            {
                _candleOneTime = candle.TimeStart;
                _candleOneValue = valueY;
                _candleOneNumber = numberCandle;

                if (numberCandle != 0)
                {
                    _candleTwoTime = arrayCandles[0].TimeStart;
                    _candleTwoValue = valueY;
                    _candleTwoNumber = 0;
                }
                else
                {
                    _candleTwoTime = arrayCandles[3].TimeStart;
                    _candleTwoValue = valueY;
                    _candleTwoNumber = 3;
                }
                _waitHorisont = false;
            }

            if (_candleOneTime != DateTime.MinValue && _candleTwoTime != DateTime.MinValue)
            {
                //меняем местами точки, если они установлены не в правильном порядке
                if (_candleOneNumber > _candleTwoNumber)
                {
                    int glassNumber = _candleTwoNumber;
                    DateTime glassTime = _candleTwoTime;
                    decimal glassPoint = _candleTwoValue;

                    _candleTwoNumber = _candleOneNumber;
                    _candleTwoTime = _candleOneTime;
                    _candleTwoValue = _candleOneValue;

                    _candleOneNumber = glassNumber;
                    _candleOneTime = glassTime;
                    _candleOneValue = glassPoint;
                }

                SetReadyLineAlert(arrayCandles);
            }
        }

        /// <summary>
        /// сохранить Алерт
        /// </summary>
        private void SaveAlert() 
        {

            if (MyAlert == null)
            {
                return;
            }

            SetSettingsForomWindow();

            _keeper.Delete(MyAlert);

            _keeper.SetNewAlert(MyAlert);
        }

        /// <summary>
        /// загрузить в Алерт новые настройки, с формы
        /// </summary>
        private void SetSettingsForomWindow()
        {
            try
            {
                Convert.ToInt32(TextBoxVolumeReaction.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("В одном из полей не допустимые значения");
            }

            if (MyAlert == null)
            {
                return;
            }

            
            MyAlert.IsMusicOn = CheckBoxMusicAlert.IsChecked.Value;
            MyAlert.IsOn = CheckBoxOnOff.IsChecked.Value;
            MyAlert.IsMessageOn = CheckBoxWindow.IsChecked.Value;

            MyAlert.Label = TextBoxLabelAlert.Text;
          //  MyAlert.ColorLabel = HostColorLabel.Child.BackColor;

           // MyAlert.ColorLine = HostColorLine.Child.BackColor;
            MyAlert.BorderWidth = Convert.ToInt32(ComboBoxFatLine.SelectedItem);

            MyAlert.Message = TextBoxAlertMessage.Text;

            Enum.TryParse(ComboBoxType.Text, true, out MyAlert.Type);

            Enum.TryParse(ComboBoxSignalType.Text, true, out MyAlert.SignalType);
            MyAlert.VolumeReaction = Convert.ToInt32(TextBoxVolumeReaction.Text);

            MyAlert.Slippage = Convert.ToDecimal(TextBoxSlippage.Text);
            MyAlert.NumberClosePosition = Convert.ToInt32(TextBoxClosePosition.Text);
            Enum.TryParse(ComboBoxOrderType.Text, true, out MyAlert.OrderPriceType);

            Enum.TryParse(ComboBoxMusicType.Text,out MyAlert.Music);


            System.Windows.Media.Color  labelColor = ((SolidColorBrush)ButtonColorLabel.Background).Color;
            MyAlert.ColorLabel = System.Drawing.Color.FromArgb(labelColor.A, labelColor.R, labelColor.G, labelColor.B);

            System.Windows.Media.Color lineColor = ((SolidColorBrush)ButtonColorLine.Background).Color;
            MyAlert.ColorLine = System.Drawing.Color.FromArgb(lineColor.A, lineColor.R, lineColor.G, lineColor.B);


        }

        /// <summary>
        /// преобразуем указанные ранее значения в Алерт
        /// </summary>
        /// <param name="candles">массив свечек</param>
        private void SetReadyLineAlert(List<Candle> candles)
        {
            if (candles == null ||
                 (candles[candles.Count - 1].TimeStart - candles[0].TimeStart).Hours < 1)
            {
                return;
            }

            // создаём новый алерт
            AlertToChart alert = new AlertToChart(_keeper.HostAllert);
            alert.Name = null;
            alert.Lines = GetAlertLines(candles);

            if (MyAlert != null)
            {
                _keeper.Delete(MyAlert);
            }

            MyAlert = alert;

            SaveAlert();
        }

        /// <summary>
        /// взять Линии для Алерта
        /// </summary>
        /// <param name="candles">массив свечек</param>
        /// <returns>линии алерта</returns>
        private ChartAlertLine[] GetAlertLines(List<Candle> candles)
        {
            if (ComboBoxType.Text == ChartAlertType.Line.ToString() ||
                ComboBoxType.Text == ChartAlertType.HorisontalLine.ToString())
            {
                decimal valueOne = _candleOneValue;
                decimal valueTwo = _candleTwoValue;
                int numberOne = _candleOneNumber;
                int numberTwo = _candleTwoNumber;

                ChartAlertLine[] lines = { AlertLineCreate(valueOne, valueTwo, numberOne, numberTwo, candles) };
                return lines;
            }

            if (ComboBoxType.Text == ChartAlertType.FibonacciSpeedLine.ToString())
            {
                return GetSpeedAlertLines(candles);
            }


            if (ComboBoxType.Text == ChartAlertType.FibonacciChannel.ToString())
            {
               return GetChanalLines(candles);
            }

            return null;
        }

        /// <summary>
        /// взять Линии для Алерта типа SpeedLine
        /// </summary>
        /// <param name="candles">свечки</param>
        /// <returns>линии</returns>
        private ChartAlertLine[] GetSpeedAlertLines(List<Candle> candles)
        {
            //1) Указываем первую точку
            //2) Указываем вторую точку
            //3) Между точками строиться прямоугольник
            //4) По вертикальному катету накладываем три точки. При этом на восходящем движении отсчет идет снизу вверх: пропорция 0.382______ 0.487_______ 0.618. Проводим через них три линии.

            decimal valueOneClick = _candleOneValue;

            decimal valueTwoClick;

            decimal devider = Convert.ToDecimal(Slider.Value - 100);

            valueTwoClick = _candleTwoValue + _candleTwoValue * Convert.ToDecimal(devider / 1000);

            // 1 если нажаты точки на одной прямой
            if (valueTwoClick == valueOneClick)
            {
                decimal valueOne = _candleOneValue;
                decimal valueTwo = _candleTwoValue;
                int numberOne = _candleOneNumber;
                int numberTwo = _candleTwoNumber;

                ChartAlertLine[] lines = { AlertLineCreate(valueOne, valueTwo, numberOne, numberTwo, candles) };
                return lines;
            }

            // 2 находим высоту катета

            decimal highKatet;

            bool isUpSpeedLine;

            if (valueTwoClick > valueOneClick)
            {
                isUpSpeedLine = true;
                highKatet = valueTwoClick - valueOneClick;
            }
            else
            {
                isUpSpeedLine = false;
                highKatet = valueOneClick - valueTwoClick;
            }

            // объявляем точки для трёх линий

            decimal firstValueToAllLine = _candleOneValue;
            int firstNumberToAllLine = _candleOneNumber;

            int secondNumberToAllLine = _candleTwoNumber;

            decimal oneLineValue;
            decimal twoLineValue;
            decimal threeLineValue;

            if (isUpSpeedLine)
            {
                oneLineValue = valueOneClick + highKatet * 0.382m;
                twoLineValue = valueOneClick + highKatet * 0.487m;
                threeLineValue = valueOneClick + highKatet * 0.618m;
            }
            else
            {
                oneLineValue = valueOneClick - highKatet * 0.382m;
                twoLineValue = valueOneClick - highKatet * 0.487m;
                threeLineValue = valueOneClick - highKatet * 0.618m;
            }

            ChartAlertLine oneLine = AlertLineCreate(firstValueToAllLine, oneLineValue, firstNumberToAllLine,
                secondNumberToAllLine, candles);

            ChartAlertLine twoLine = AlertLineCreate(firstValueToAllLine, twoLineValue, firstNumberToAllLine,
            secondNumberToAllLine, candles);

            ChartAlertLine treeLine = AlertLineCreate(firstValueToAllLine, threeLineValue, firstNumberToAllLine,
            secondNumberToAllLine, candles);

            ChartAlertLine[] alertLines = {oneLine, twoLine, treeLine};

            return alertLines;

        }

        /// <summary>
        /// взять Линии для Алерта типа Chanal
        /// </summary>
        /// <param name="candles">свечи</param>
        /// <returns>линии</returns>
        private ChartAlertLine[] GetChanalLines(List<Candle> candles)
        {
            if (Slider.Value == 100)
            {
                decimal valueOne = _candleOneValue;
                decimal valueTwo = _candleTwoValue;
                int numberOne = _candleOneNumber;
                int numberTwo = _candleTwoNumber;

                ChartAlertLine[] lines = { AlertLineCreate(valueOne, valueTwo, numberOne, numberTwo, candles) };
                return lines;
            }

            // 1 берём точки

            decimal onePoint = _candleOneValue;
            int oneNumber = _candleOneNumber; // первая точка линии

            decimal twoPoint = _candleTwoValue;
            int twoPNumber = _candleTwoNumber; // вторая точка линии

            decimal l023value1;
            decimal l023value2;

            decimal l038value1;
            decimal l038value2;

            decimal l050value1;
            decimal l050value2;

            decimal l061value1;
            decimal l061value2;

            decimal l076value1;
            decimal l076value2;

            decimal l0100value1;
            decimal l0100value2;

            decimal l0161value1;
            decimal l0161value2;

            decimal l0261value1;
            decimal l0261value2;

            decimal l0423value1;
            decimal l0423value2;

            decimal sliderValue = Convert.ToDecimal(Slider.Value);
            decimal devider;

            if (Slider.Value > 100)
            {
                devider = (sliderValue - 100) / 1000; // 100 / 1000 = 0,1

                l023value1 = onePoint + devider * onePoint * 0.23m;
                l023value2 = twoPoint + devider * twoPoint * 0.23m;

                l038value1 = onePoint + devider * onePoint * 0.38m;
                l038value2 = twoPoint + devider * twoPoint * 0.38m;

                l050value1 = onePoint + devider * onePoint * 0.50m;
                l050value2 = twoPoint + devider * twoPoint * 0.50m;

                l061value1 = onePoint + devider * onePoint * 0.61m;
                l061value2 = twoPoint + devider * twoPoint * 0.61m;

                l076value1 = onePoint + devider * onePoint * 0.76m;
                l076value2 = twoPoint + devider * twoPoint * 0.76m;

                l0100value1 = onePoint + devider * onePoint * 1m;
                l0100value2 = twoPoint + devider * twoPoint * 1m;

                l0161value1 = onePoint + devider * onePoint * 1.61m;
                l0161value2 = twoPoint + devider * twoPoint * 1.61m;

                l0261value1 = onePoint + devider * onePoint * 2.61m;
                l0261value2 = twoPoint + devider * twoPoint * 2.61m;

                l0423value1 = onePoint + devider * onePoint * 4.23m;
                l0423value2 = twoPoint + devider * twoPoint * 4.423m;
            }
            else
            {
                devider = (100 - sliderValue) / 1000; // 100 / 1000 = 0,1

                l023value1 = onePoint - devider * onePoint * 0.23m;
                l023value2 = twoPoint - devider * twoPoint * 0.23m;

                l038value1 = onePoint - devider * onePoint * 0.38m;
                l038value2 = twoPoint - devider * twoPoint * 0.38m;

                l050value1 = onePoint - devider * onePoint * 0.50m;
                l050value2 = twoPoint - devider * twoPoint * 0.50m;

                l061value1 = onePoint - devider * onePoint * 0.61m;
                l061value2 = twoPoint - devider * twoPoint * 0.61m;

                l076value1 = onePoint - devider * onePoint * 0.76m;
                l076value2 = twoPoint - devider * twoPoint * 0.76m;

                l0100value1 = onePoint - devider * onePoint * 1m;
                l0100value2 = twoPoint - devider * twoPoint * 1m;

                l0161value1 = onePoint - devider * onePoint * 1.61m;
                l0161value2 = twoPoint - devider * twoPoint * 1.61m;

                l0261value1 = onePoint - devider * onePoint * 2.61m;
                l0261value2 = twoPoint - devider * twoPoint * 2.61m;

                l0423value1 = onePoint - devider * onePoint * 4.23m;
                l0423value2 = twoPoint - devider * twoPoint * 4.23m;
            }
            
            
            
                
            
          

            // 2 расс


            ChartAlertLine oneLine = AlertLineCreate(onePoint, twoPoint, oneNumber, twoPNumber, candles);

            ChartAlertLine l023Line = AlertLineCreate(l023value1, l023value2, oneNumber, twoPNumber, candles);

            ChartAlertLine l038Line = AlertLineCreate(l038value1, l038value2, oneNumber, twoPNumber, candles);

            ChartAlertLine l050Line = AlertLineCreate(l050value1, l050value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l061Line = AlertLineCreate(l061value1, l061value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l076Line = AlertLineCreate(l076value1, l076value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l100Line = AlertLineCreate(l0100value1, l0100value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l161Line = AlertLineCreate(l0161value1, l0161value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l261Line = AlertLineCreate(l0261value1, l0261value2, oneNumber, twoPNumber, candles);
            ChartAlertLine l423Line = AlertLineCreate(l0423value1, l0423value2, oneNumber, twoPNumber, candles);

            ChartAlertLine[] alertLines = new ChartAlertLine[10];

            alertLines[0] =  oneLine;
            alertLines[1] = l023Line;
            alertLines[2] = l038Line;
            alertLines[3] = l050Line;
            alertLines[4] = l061Line;
            alertLines[5] = l076Line;
            alertLines[6] = l100Line;
            alertLines[7] = l161Line;
            alertLines[8] = l261Line;
            alertLines[9] = l423Line;

            return alertLines;

        }

        /// <summary>
        /// Создать линию из точек
        /// </summary>
        /// <param name="valueOne">значение первой точки</param>
        /// <param name="valueTwo">значение второй точки</param>
        /// <param name="numberOne">номер первой точки</param>
        /// <param name="numberTwo">номер второй точки</param>
        /// <param name="candles">свечи</param>
        /// <returns>алерт</returns>
        private ChartAlertLine AlertLineCreate(decimal valueOne, decimal valueTwo, int numberOne, int numberTwo, List<Candle> candles)
        {
            // 2 рассчитываем движение индикатора за свечу на данном ТФ

            decimal stepCorner; // сколько наша линия проходит за свечку

            stepCorner = (valueTwo - valueOne) / (numberTwo - numberOne + 1);

            // 3 теперь строим массив значений линии параллельный свечному массиву

            decimal[] lineDecimals = new decimal[candles.Count];
            decimal point = valueOne;
            lineDecimals[numberOne] = point;

            for (int i = numberOne + 1; i < lineDecimals.Length; i++)
            { // бежим вперёд по массиву
                lineDecimals[i] = point;
                point += stepCorner;
            }

            point = valueOne;
            for (int i = numberOne - 1; i > -1; i--)
            { // бежим назад по массиву
                lineDecimals[i] = point;
                point -= stepCorner;
            }

            // 4 находим ближайший от начала час  и следующий за ним

            int firstHourCandle = -1;

            int secondHourCandle = -1;

            int nowHour = candles[candles.Count - 1].TimeStart.Hour;

            for (int i = candles.Count - 1; i > -1; i--)
            {
                if (nowHour != candles[i].TimeStart.Hour)
                {
                    nowHour = candles[i].TimeStart.Hour;

                    if (firstHourCandle == -1)
                    {
                        firstHourCandle = i + 1;
                    }
                    else
                    {
                        secondHourCandle = i + 1;
                        break;
                    }
                }
            }

            // 6 рассчитываем реальное положение алерта. В часах.

            if(secondHourCandle <0 ||
                firstHourCandle <0)
            {
                return null;
            }

            DateTime startTime = candles[secondHourCandle].TimeStart;
            DateTime endTime = candles[firstHourCandle].TimeStart;

            decimal startPoint = lineDecimals[secondHourCandle];
            decimal endPoint = lineDecimals[firstHourCandle];

            // 5 далее рассчитываем значение линии на первом и втором часе

            ChartAlertLine line = new ChartAlertLine();
            line.TimeFirstPoint = startTime;
            line.ValueFirstPoint = startPoint;

            line.TimeSecondPoint = endTime;
            line.ValueSecondPoint = endPoint;

            return line;
        }

        /// <summary>
        /// кнопка сохранить
        /// </summary>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveAlert();
            NeadToSave = true;
            Close();
        }

        /// <summary>
        /// кнопка цвет подписи
        /// </summary>
        private void ButtonColorLabel_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog ui = new ColorDialog();

            System.Windows.Media.Color labelColor = ((SolidColorBrush)ButtonColorLabel.Background).Color;
            ui.Color = System.Drawing.Color.FromArgb(labelColor.A, labelColor.R, labelColor.G, labelColor.B);

            ui.ShowDialog();

            System.Drawing.Color newColor = ui.Color;
            ButtonColorLabel.Background =
                new SolidColorBrush(Color.FromArgb(newColor.A, newColor.R, newColor.G, newColor.B));

            SaveAlert();
        }

        /// <summary>
        /// кнопка цвет линии
        /// </summary>
        private void ButtonColorLine_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog ui = new ColorDialog();

            System.Windows.Media.Color lineColor = ((SolidColorBrush)ButtonColorLine.Background).Color;
            ui.Color = System.Drawing.Color.FromArgb(lineColor.A, lineColor.R, lineColor.G, lineColor.B);
            ui.ShowDialog();

            System.Drawing.Color newColor = ui.Color;
            ButtonColorLine.Background =
                new SolidColorBrush(Color.FromArgb(newColor.A, newColor.R, newColor.G, newColor.B));

            SaveAlert();
        }

        /// <summary>
        /// кнопка указать наклонную линию
        /// </summary>
        private void ButtonSendFirst_Click(object sender, RoutedEventArgs e)
        {
            ChartAlertType type;

            Enum.TryParse(ComboBoxType.Text, out type);

            if (type == ChartAlertType.FibonacciChannel)
            {
                if (_waitOne == false)
                {
                    _waitOne = true;
                    _waitTwo = false;
                }
                else if (_waitOne)
                {
                    _waitOne = false;
                    _waitTwo = false;
                }
                Slider.Value = 100;
            }
            else if (type == ChartAlertType.Line)
            {
                if (_waitOne == false)
                {
                    _waitOne = true;
                    _waitTwo = false;
                }
                else if (_waitOne)
                {
                    _waitOne = false;
                    _waitTwo = false;
                }
            }
            else if (type == ChartAlertType.FibonacciSpeedLine)
            {
                if (_waitOne == false)
                {
                    _waitOne = true;
                    _waitTwo = false;
                }
                else if (_waitOne)
                {
                    _waitOne = false;
                    _waitTwo = false;
                }
                Slider.Value = 100;
            }
            else if (type == ChartAlertType.HorisontalLine)
            {
                _waitHorisont = true;
                _waitOne = false;
                _waitTwo = false;
            }

          
        }

    }
}
