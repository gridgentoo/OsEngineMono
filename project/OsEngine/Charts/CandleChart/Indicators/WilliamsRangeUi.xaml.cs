﻿/*
 * Если вы не покупали лицензии, то Ваши права на использования кода ограничены не коммерческим использованием и 
 * регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Forms.TextBox;

namespace OsEngine.Charts.CandleChart.Indicators
{
    /// <summary>
    /// Логика взаимодействия для WilliamsRangeUi.xaml
    /// </summary>
    public partial class WilliamsRangeUi
    {
        /// <summary>
        /// индикатор
        /// </summary>
        private WilliamsRange _wr;

        /// <summary>
        /// изменялись ли настройки индикатора
        /// </summary>
        public bool IsChange;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="wr">индикатор для настроек</param>
        public WilliamsRangeUi(WilliamsRange wr)
        {
            InitializeComponent();
            _wr = wr;

            TextBoxLenght.Text = _wr.Nperiod.ToString();

            HostColorBase.Child = new TextBox();
            HostColorBase.Child.BackColor = _wr.ColorBase;
            CheckBoxPaintOnOff.IsChecked = _wr.PaintOn;


        }

        /// <summary>
        /// кнопка принять
        /// </summary>
        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(TextBoxLenght.Text) <= 0)
                {
                    throw new Exception("error");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Процесс сохранения прерван. В одном из полей недопустимые значения");
                return;
            }

            _wr.ColorBase = HostColorBase.Child.BackColor;
            _wr.Nperiod = Convert.ToInt32(TextBoxLenght.Text);
            if (CheckBoxPaintOnOff.IsChecked.HasValue)
            {
                _wr.PaintOn = CheckBoxPaintOnOff.IsChecked.Value;
            }
            

            _wr.Save();

            IsChange = true;
            Close();
        }

        /// <summary>
        /// кнопка настроить цвет
        /// </summary>
        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = HostColorBase.Child.BackColor;
            dialog.ShowDialog();
            HostColorBase.Child.BackColor = dialog.Color;
        }
    }
}

