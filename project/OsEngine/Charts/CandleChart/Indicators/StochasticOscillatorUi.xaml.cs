﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/
using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Forms.TextBox;

namespace OsEngine.Charts.CandleChart.Indicators
{
    /// <summary>
    /// Логика взаимодействия для StochasticOscillatorUi.xaml
    /// </summary>
    public partial class StochasticOscillatorUi 
    {
       /// <summary>
        /// индикатор
        /// </summary>
        private StochasticOscillator _so;

        /// <summary>
        /// изменялись ли настройки индикатора
        /// </summary>
        public bool IsChange;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="so">индикатор для настроек</param>
        public StochasticOscillatorUi(StochasticOscillator so)
        {
            InitializeComponent();
            _so = so;

            TextBoxLenght.Text = _so.P1.ToString();
            TextBoxLenght2.Text = _so.P2.ToString();
            TextBoxLenght3.Text = _so.P3.ToString();

            HostColor1.Child = new TextBox();
            HostColor1.Child.BackColor = _so.ColorUp;

            HostColor2.Child = new TextBox();
            HostColor2.Child.BackColor = _so.ColorDown;

            CheckBoxPaintOnOff.IsChecked = _so.PaintOn;


            Movingbox.Items.Add(MovingAverageTypeCalculation.Exponential);
            Movingbox.Items.Add(MovingAverageTypeCalculation.Simple);
            Movingbox.Items.Add(MovingAverageTypeCalculation.Weighted);
            Movingbox.Items.Add(MovingAverageTypeCalculation.Adaptive);
            Movingbox.SelectedItem = _so.TypeCalculationAverage;

            Movingbox.SelectedItem = _so.TypeIndicator;
        }

        /// <summary>
        /// кнопка принять
        /// </summary>
        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(TextBoxLenght.Text) <= 0||
                    Convert.ToInt32(TextBoxLenght2.Text) <= 0 ||
                    Convert.ToInt32(TextBoxLenght3.Text) <= 0)
                {
                    throw new Exception("error");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Процесс сохранения прерван. В одном из полей недопустимые значения");
                return;
            }

            _so.ColorUp = HostColor1.Child.BackColor;
            _so.ColorDown = HostColor2.Child.BackColor;

            _so.P1 = Convert.ToInt32(TextBoxLenght.Text);
            _so.P2 = Convert.ToInt32(TextBoxLenght2.Text);
            _so.P3 = Convert.ToInt32(TextBoxLenght3.Text);

            _so.PaintOn = CheckBoxPaintOnOff.IsChecked.Value;

            Enum.TryParse(Movingbox.Text, out _so.TypeCalculationAverage);

            _so.Save();

            IsChange = true;
            Close();
        }

        /// <summary>
        /// кнопка настроить цвет
        /// </summary>
        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = HostColor1.Child.BackColor;
            dialog.ShowDialog();
            HostColor1.Child.BackColor = dialog.Color;
        }

        private void ButtonColor2_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = HostColor2.Child.BackColor;
            dialog.ShowDialog();
            HostColor2.Child.BackColor = dialog.Color;
        }
    }
}
