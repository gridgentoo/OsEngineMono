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
    /// Логика взаимодействия для MovingAverageUi.xaml
    /// </summary>
    public partial class VolumeOscillatorUi
    {
        /// <summary>
        /// индикатор который мы настраиваем
        /// </summary>
        private VolumeOscillator _mA;

        /// <summary>
        /// изменился ли индикатор
        /// </summary>
        public bool IsChange;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="mA">индикатор для настройки</param>
        public VolumeOscillatorUi(VolumeOscillator mA)
        {
            InitializeComponent();
            _mA = mA;

            TextBoxLenght1.Text = _mA.Lenght1.ToString();
            TextBoxLenght2.Text = _mA.Lenght2.ToString();
            HostColor.Child = new TextBox();
            HostColor.Child.BackColor = _mA.ColorBase;

            CheckBoxPaintOnOff.IsChecked = _mA.PaintOn;
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.Open);
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.High);
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.Low);
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.Close);
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.Median);
            //ComboBoxPriceField.Items.Add(StandardDeviationTypePoints.Typical);

            //ComboBoxPriceField.SelectedItem = _mA.TypePointsToSearch;
        }

        /// <summary>
        /// кнопка принять
        /// </summary>
        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((Convert.ToInt32(TextBoxLenght1.Text) <= 0) || (Convert.ToInt32(TextBoxLenght2.Text) <= 0))
                {
                    throw new Exception("error");
                }
                //Enum.TryParse(ComboBoxMovingType.SelectedItem.ToString(), true, out _mA.TypeCalculationAverage);
            }
            catch (Exception)
            {
                MessageBox.Show("Процесс сохранения прерван. В одном из полей недопустимые значения");
                return;
            }

            _mA.ColorBase = HostColor.Child.BackColor;
            _mA.Lenght1 = Convert.ToInt32(TextBoxLenght1.Text);
            _mA.Lenght2 = Convert.ToInt32(TextBoxLenght2.Text);
            _mA.PaintOn = CheckBoxPaintOnOff.IsChecked.Value;
            //Enum.TryParse(ComboBoxPriceField.SelectedItem.ToString(), true, out _mA.TypePointsToSearch);

            _mA.Save();
            IsChange = true;
            Close();
        }

        /// <summary>
        /// кнопка изменить цвет
        /// </summary>
        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = HostColor.Child.BackColor;
            dialog.ShowDialog();

            HostColor.Child.BackColor = dialog.Color;
        }

    }
}
