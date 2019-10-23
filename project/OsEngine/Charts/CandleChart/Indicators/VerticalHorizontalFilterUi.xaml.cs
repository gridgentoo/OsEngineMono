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
    /// Логика взаимодействия для VerticalHorizontalFilterUi.xaml
    /// </summary>
    public partial class VerticalHorizontalFilterUi
    {
        /// <summary>
        /// индикатор
        /// </summary>
        private VerticalHorizontalFilter _vhf;

        /// <summary>
        /// изменялись ли настройки индикатора
        /// </summary>
        public bool IsChange;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="vhf">индикатор для настроек</param>
        public VerticalHorizontalFilterUi(VerticalHorizontalFilter vhf)
        {
            InitializeComponent();
            _vhf = vhf;

            TextBoxLenght.Text = _vhf.Nperiod.ToString();

            HostColorBase.Child = new TextBox();
            HostColorBase.Child.BackColor = _vhf.ColorBase;
            CheckBoxPaintOnOff.IsChecked = _vhf.PaintOn;


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

            _vhf.ColorBase = HostColorBase.Child.BackColor;
            _vhf.Nperiod = Convert.ToInt32(TextBoxLenght.Text);
            _vhf.PaintOn = CheckBoxPaintOnOff.IsChecked.Value;



            _vhf.Save();

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

