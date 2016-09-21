using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;

namespace DesignOfEffectiveLenghtOfColumns
{
    enum FramePropertiesOfSimpleColumn : byte    // 8 бит, первые шесть из которых отвечают за тип рамы (состояние радио кнопок)
    {
        FrameType = 0x01,                    // 0000 0001 | 01 | Тип рамы: 0 - свободная, 1 - несвободная
        NumberOfStoreys = 0x02,              // 0000 0010 | 02 | Этажность: 0 - одноэтажная, 1- многоэтажная
        ConcernedStorey1stBit = 0x04,        // 0000 0100 | 04 | см. следующий бит 
        ConcernedStorey2ndBit = 0x08,        // 0000 1000 | 08 | Рассматриваемый этаж: 00 - нижний, 10 - средний, 11 - верхний
        BearingType = 0x10,                  // 0001 0000 | 16 | Опирание колонн на фундамент: 0 - шарнирное, 1 - жесткое
        ConcernedColumn = 0x20,              // 0010 0000 | 32 | Рассматриваемая колонна: 0 - крайняя, 1- средняя
        
        // Возможные комбинации:
        FreeOnestoreySwivelbearingExterme = 0x00,                           // 0000 0000 | 00
        FreeOnestoreySwivelbearingMiddle = 0x20,                            // 0010 0000 | 32
        FreeOnestoreyRigidbearingExterme = 0x10,                            // 0001 0000 | 16
        FreeOnestoreyRigidbearingMiddle = 0x30,                             // 0011 0000 | 48

        FreeMultiplestoreysBottomstoreySwivelbearingExterme = 0x02,         // 0000 0010 | 02
        FreeMultiplestoreysBottomstoreySwivelbearingMiddle = 0x22,          // 0010 0010 | 34
        FreeMultiplestoreysBottomstoreyRigidbearingExterme = 0x12,          // 0001 0010 | 18
        FreeMultiplestoreysBottomstoreyRigidbearingMiddle = 0x32,           // 0011 0010 | 50

        FreeMultiplestoreysMiddlestoreyExterme = 0x0A,                      // 0000 1010 | 10
        FreeMultiplestoreysMiddlestoreyMiddle = 0x2A,                       // 0010 1010 | 25
        FreeMultiplestoreysTopstoreyExterme = 0x0E,                         // 0000 1110 | 14
        FreeMultiplestoreysTopstoreyMiddle = 0x2E,                          // 0010 1110 | 46

        UnfreeOnestoreySwivelbearingExterme = 0x01,                         // 0000 0001 | 01
        UnfreeOnestoreySwivelbearingMiddle = 0x21,                          // 0010 0001 | 33
        UnfreeOnestoreyRigidbearingExterme = 0x11,                          // 0001 0001 | 17
        UnfreeOnestoreyRigidbearingMiddle = 0x31,                           // 0011 0001 | 49

        UnfreeMultiplestoreysBottomstoreySwivelbearingExterme = 0x03,       // 0000 0011 | 03
        UnfreeMultiplestoreysBottomstoreySwivelbearingMiddle = 0x23,        // 0010 0011 | 35
        UnfreeMultiplestoreysBottomstoreyRigidbearingExterme = 0x13,        // 0001 0011 | 19
        UnfreeMultiplestoreysBottomstoreyRigidbearingMiddle = 0x33,         // 0011 0011 | 51

        UnfreeMultiplestoreysMiddlestoreyExterme = 0x0B,                    // 0000 1011 | 11
        UnfreeMultiplestoreysMiddlestoreyMiddle = 0x2B,                     // 0010 1011 | 26
        UnfreeMultiplestoreysTopstoreyExterme = 0x0F,                       // 0000 1111 | 15
        UnfreeMultiplestoreysTopstoreyMiddle = 0x2F,                        // 0010 1111 | 47
    }

    partial class EffectiveLenghtOfSimpleColumn : UserControl, IMyUserControl
    { 
    // инициализация необходимых полей и обьектов
        DesignOfEffectiveLenghtOfSimpleColumn designOfEffectiveLenghtOfSimpleColumn = new DesignOfEffectiveLenghtOfSimpleColumn();
        FramePropertiesOfSimpleColumn framePropertiesOfSimpleColumn = 0x00;     // 0000 0000 | 00 
        bool actualResults = false;

    // конструктор юзер контрола
        public EffectiveLenghtOfSimpleColumn()
        {
            InitializeComponent();
            InitializeOfControlsProperties();
        } 

    // внутренние вспомагательные методы
        private void InitializeOfControlsProperties()
        {
            // заполнения текста контролов из файла реурсов
            grbxFrameType.Text = GeneralRes.FrameType;
            rbtnFreeFrame.Text = GeneralRes.FreeFrame;
            rbtnUnfreeFrame.Text = GeneralRes.UnfreeFrame;
            grbxNumberOfStoreys.Text = GeneralRes.NumberOfStoreys;
            rbtnOneStorey.Text = GeneralRes.OneStorey;
            rbtnMultipleStoreys.Text = GeneralRes.MultipleStoreys;
            grbxConcernedStorey.Text = GeneralRes.ConcernedStorey;
            rbtnBottomStorey.Text = GeneralRes.BottomStorey;
            rbtnMiddleStorey.Text = GeneralRes.MiddleStorey;
            rbtnTopStorey.Text = GeneralRes.TopStorey;
            grbxBearingType.Text = GeneralRes.BearingType;
            rbtnSwivelBearing.Text = GeneralRes.swivelBearing;
            rbtnRigidBearing.Text = GeneralRes.rigidBearing;
            grbxConcernedColumn.Text = GeneralRes.ConcernedColumn;
            rbtnExtermeColumn.Text = GeneralRes.ExtermeColumn;
            rbtnMiddleColumn.Text = GeneralRes.MiddleColumn;
            grbxParamsOfFrameOnLeft.Text = GeneralRes.ParamsOfFrame;
            lblTxtHeight.Text = GeneralRes.Height;
            lblTxtSpan.Text = GeneralRes.Span;
            rbtnMomInertValues.Text = GeneralRes.MomInertOfColumn
                + "\n\n" + GeneralRes.MomInertOfGirder;
            rbtnMomInertRatio.Text = GeneralRes.MomInetrtRatioOneGirder;
            lblHeightLeft.Text = GeneralRes.h;
            lblSpanLeft.Text = GeneralRes.l;
            lblMomInertOfColumnLeft.Text = GeneralRes.Ic;
            lblMomInertOfGirderLeft.Text = GeneralRes.Is;
            lblMomInertOfGirderLeftBottom.Text = GeneralRes.Ii;
            lblMomInertRatioLeft.Text = GeneralRes.IcOnIs;
            lblMomInertRatioLeftBottom.Text = GeneralRes.IcOnIi;
            lblUnitsHeightLeft.Text = GeneralRes.m;
            lblUnitsSpanLeft.Text = GeneralRes.m;
            lblUnitsMomInertOfColumnLeft.Text = GeneralRes.cm4;
            lblUnitsMomInertOfGirderLeft.Text = GeneralRes.cm4;
            lblUnitsMomInertOfGirderLeftBottom.Text = GeneralRes.cm4;
            grbxNumberOfSpans.Text = GeneralRes.NumberOfSpans;
            lblNumberOfSpans.Text = GeneralRes.k;
            grbxParamsOfFrameOnRight.Text = GeneralRes.ParamsOfFrameOnRight;
            chbxDuplicateLeftToRight.Text = GeneralRes.DuplicateLeftToRight;
            lblHeightRight.Text = GeneralRes.h;
            lblSpanRight.Text = GeneralRes.l2;
            lblMomInertOfColumnRight.Text = GeneralRes.Ic;
            lblMomInertOfGirderRight.Text = GeneralRes.Is2;
            lblMomInertOfGirderRightBottom.Text = GeneralRes.Ii2;
            lblMomInertRatioRight.Text = GeneralRes.IcOnIs2;
            lblMomInertRatioRightBottom.Text = GeneralRes.IcOnIi2;
            lblUnitsHeightRight.Text = GeneralRes.m;
            lblUnitsSpanRight.Text = GeneralRes.m;
            lblUnitsMomInertOfColumnRight.Text = GeneralRes.cm4;
            lblUnitsMomInertOfGirderRight.Text = GeneralRes.cm4;
            lblUnitsMomInertOfGirderRightBottom.Text = GeneralRes.cm4;
            btnDesign.Text = GeneralRes.Design;
            grbxResults.Text = GeneralRes.Results;
            lblCoefficient.Text = GeneralRes.Coefficient;
            lblEffectiveLenghtOfColumn.Text = GeneralRes.EffectiveLenghtOfColumn;
            lblMu.Text = GeneralRes.mu;
            lblLef.Text = GeneralRes.lef;
            lblUnitsEffectiveLenghtOfColumn.Text = GeneralRes.m; 
          
            // для "аккуратного" отображения рядов, если задавать в дизайнере, то слетает подпись и неудобно работать  
            tableLayoutPanel3.SetRowSpan(rbtnMomInertValues, 2);      
            tableLayoutPanel3.SetRowSpan(rbtnMomInertRatio, 1);
        }        

        private void DrawPicture()  // метод для смены изображения из файла ресурсов в зависимости от текущих параметров рамы
        {
            switch (framePropertiesOfSimpleColumn)
            {
                case FramePropertiesOfSimpleColumn.FreeOnestoreySwivelbearingExterme:
                    pictureBox1.Image = GeneralRes.FreeOnestoreySwivelbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeOnestoreySwivelbearingMiddle:
                    pictureBox1.Image = GeneralRes.FreeOnestoreySwivelbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.FreeOnestoreyRigidbearingExterme:
                    pictureBox1.Image = GeneralRes.FreeOnestoreyRigidbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeOnestoreyRigidbearingMiddle:
                    pictureBox1.Image = GeneralRes.FreeOnestoreyRigidbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysBottomstoreySwivelbearingExterme:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysBottomstoreySwivelbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysBottomstoreySwivelbearingMiddle:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysBottomstoreySwivelbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysBottomstoreyRigidbearingExterme:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysBottomstoreyRigidbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysBottomstoreyRigidbearingMiddle:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysBottomstoreyRigidbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysMiddlestoreyExterme:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysMiddlestoreyExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysMiddlestoreyMiddle:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysMiddlestoreyMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysTopstoreyExterme:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysTopstoreyExterme;
                    break;
                case FramePropertiesOfSimpleColumn.FreeMultiplestoreysTopstoreyMiddle:
                    pictureBox1.Image = GeneralRes.FreeMultiplestoreysTopstoreyMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeOnestoreySwivelbearingExterme:
                    pictureBox1.Image = GeneralRes.UnfreeOnestoreySwivelbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeOnestoreySwivelbearingMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeOnestoreySwivelbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeOnestoreyRigidbearingExterme:
                    pictureBox1.Image = GeneralRes.UnfreeOnestoreyRigidbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeOnestoreyRigidbearingMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeOnestoreyRigidbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysBottomstoreySwivelbearingExterme:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysBottomstoreySwivelbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysBottomstoreySwivelbearingMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysBottomstoreySwivelbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysBottomstoreyRigidbearingExterme:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysBottomstoreyRigidbearingExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysBottomstoreyRigidbearingMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysBottomstoreyRigidbearingMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysMiddlestoreyExterme:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysMiddlestoreyExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysMiddlestoreyMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysMiddlestoreyMiddle;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysTopstoreyExterme:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysTopstoreyExterme;
                    break;
                case FramePropertiesOfSimpleColumn.UnfreeMultiplestoreysTopstoreyMiddle:
                    pictureBox1.Image = GeneralRes.UnfreeMultiplestoreysTopstoreyMiddle;
                    break;
            }
        }

        private void AddBottomGigderControls()   // метод, добавляющий контролы параметнов нижнего ригеля
        {
            if (!lblMomInertOfGirderLeftBottom.Visible)  // если контролы уже видимы, то не выполнять
            {
                rbtnMomInertValues.Text = GeneralRes.MomInertOfColumn
                    + "\n\n" + GeneralRes.MomInertOfUpperGirder
                    + "\n\n" + GeneralRes.MomInertOfBottomGirder;

                lblMomInertOfGirderLeftBottom.Visible = true;
                tbMomInertOfGirderLeftBottom.Visible = true;
                lblUnitsMomInertOfGirderLeftBottom.Visible = true;
                tableLayoutPanel3.SetRowSpan(rbtnMomInertValues, 3); 

                rbtnMomInertRatio.Text = GeneralRes.MomInetrtRatioTwoGirders;
                lblMomInertRatioLeftBottom.Visible = true;
                tbMomInertRatioLeftBottom.Visible = true;

                tableLayoutPanel3.SetRowSpan(rbtnMomInertRatio, 2);

                tableLayoutPanel3.RowStyles[4].SizeType = SizeType.Percent;
                tableLayoutPanel3.RowStyles[4].Height = 20;
                tableLayoutPanel3.RowStyles[6].SizeType = SizeType.Percent;
                tableLayoutPanel3.RowStyles[6].Height = 20;

                tableLayoutPanel4.RowStyles[5].SizeType = SizeType.Percent;
                tableLayoutPanel4.RowStyles[5].Height = 20;
                tableLayoutPanel4.RowStyles[7].SizeType = SizeType.Percent;
                tableLayoutPanel4.RowStyles[7].Height = 20;

                if (rbtnMomInertValues.Checked)
                {
                    lblMomInertOfGirderLeftBottom.Enabled = true;
                    tbMomInertOfGirderLeftBottom.Enabled = true;
                    lblUnitsMomInertOfGirderLeftBottom.Enabled = true;

                    lblMomInertRatioLeftBottom.Enabled = false;
                    tbMomInertRatioLeftBottom.Enabled = false;
                    tbMomInertRatioLeftBottom.Text = string.Empty;
                }
                else
                {
                    lblMomInertOfGirderLeftBottom.Enabled = false;
                    tbMomInertOfGirderLeftBottom.Enabled = false;
                    tbMomInertOfGirderLeftBottom.Text = string.Empty;
                    lblUnitsMomInertOfGirderLeftBottom.Enabled = false;

                    lblMomInertRatioLeftBottom.Enabled = true;
                    tbMomInertRatioLeftBottom.Enabled = true;
                }

                // смена состояния контролов справа
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                {
                    lblMomInertOfGirderRightBottom.Visible = true;
                    tbMomInertOfGirderRightBottom.Visible = true;
                    lblUnitsMomInertOfGirderRightBottom.Visible = true;

                    lblMomInertRatioRightBottom.Visible = true;
                    tbMomInertRatioRightBottom.Visible = true;

                    if (rbtnMomInertValues.Checked)
                    {
                        lblMomInertOfGirderRightBottom.Enabled = true;
                        tbMomInertOfGirderRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertOfGirderRightBottom.Text = tbMomInertOfGirderLeftBottom.Text;
                            tbMomInertOfGirderRightBottom.ReadOnly = true;
                        }
                        else
                            tbMomInertOfGirderRightBottom.ReadOnly = false;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = true;

                        lblMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.Text = string.Empty;
                    }
                    else
                    {
                        lblMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.Text = string.Empty;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = false;

                        lblMomInertRatioRightBottom.Enabled = true;
                        tbMomInertRatioRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertRatioRightBottom.Text = tbMomInertRatioLeftBottom.Text;
                            tbMomInertRatioRightBottom.ReadOnly = true;
                        }
                        else
                            tbMomInertRatioRightBottom.ReadOnly = false;
                    }
                }
            }
        }

        private void RemoveBottomGigderControls()  // метод, скрывающий контролы параметнов нижнего ригеля
        {
            rbtnMomInertValues.Text = GeneralRes.MomInertOfColumn
                           + "\n\n" + GeneralRes.MomInertOfGirder;            

            lblMomInertOfGirderLeftBottom.Visible = false;
            tbMomInertOfGirderLeftBottom.Visible = false;
            lblUnitsMomInertOfGirderLeftBottom.Visible = false;
            tableLayoutPanel3.SetRowSpan(rbtnMomInertValues, 2); 


            rbtnMomInertRatio.Text = GeneralRes.MomInetrtRatioOneGirder;
            lblMomInertRatioLeftBottom.Visible = false;
            tbMomInertRatioLeftBottom.Visible = false;

            tableLayoutPanel3.SetRowSpan(rbtnMomInertRatio, 1);

            tableLayoutPanel3.RowStyles[4].SizeType = SizeType.AutoSize;
            tableLayoutPanel3.RowStyles[6].SizeType = SizeType.AutoSize;

            tableLayoutPanel4.RowStyles[5].SizeType = SizeType.AutoSize;
            tableLayoutPanel4.RowStyles[7].SizeType = SizeType.AutoSize;

            // смена состояния контролов справа
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
            {                
                lblMomInertOfGirderRightBottom.Visible = false;
                tbMomInertOfGirderRightBottom.Visible = false;
                lblUnitsMomInertOfGirderRightBottom.Visible = false;
          
                lblMomInertRatioRightBottom.Visible = false;
                tbMomInertRatioRightBottom.Visible = false;
            }      
        }

        private void ValidationOfTBValue(TextBox textBox, Color validBackColor)  // метод меняющий цфет фона текстбокса в зависимости от его содержания
        {            
            double a;
            if ((double.TryParse(textBox.Text, out a) && a > 0) || textBox.Text == string.Empty)
                textBox.BackColor = validBackColor;
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);
        }

        private void ValidationOfInputLeft(object sender, EventArgs e)  // метод проверяющий содержимое поля текстбокса при вводе, для текстбоксов слева
        {
            CleanResultTextboxes();
            TextBox textBox = sender as TextBox;
            ValidationOfTBValue(textBox, Color.White);
        }

        private void ValidationOfInputRight(object sender, EventArgs e)  // метод проверяющий содержимое поля текстбокса при вводе, для текстбоксов справа
        {
            CleanResultTextboxes();
            TextBox textBox = sender as TextBox;
            double a;
            if ((double.TryParse(textBox.Text, out a) && a > 0) || textBox.Text == string.Empty)
            {
                textBox.BackColor = Color.White;
                if (chbxDuplicateLeftToRight.Checked || textBox.Name == tbHeightRight.Name || textBox.Name == tbMomInertOfColumnRight.Name)
                    textBox.BackColor = Control.DefaultBackColor;
            }
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);
        }

        private bool DataValidation()  // метод проверяющий содержимое текстбоксов на соответствие пределам, в случае соответствия передает содержимое в поля класса расчета, в случае несоответствия - выдает ошибку
        {
            int quantityOfInputAbsent = 0;
            string msgInputAbsent = string.Empty;

            int quantityOfValuesNotNumbers = 0;
            string msgValuesNotNumbers = string.Empty;

            bool NumberOfSpansNotNaturalNumber = false;
            string msgNumberOfSpansNotNaturalNumber = string.Empty;

            int quantityOfValuesInvalid = 0;
            string msgValuesInvalid = string.Empty;

            bool NumberOfSpansInvalid = false;
            string msgNumberOfSpansInvalid = string.Empty;
            
            TextBox[] arrTextBox = {
                                    tbHeightLeft,
                                    tbSpanLeft,
                                    tbMomInertOfColumnLeft,
                                    tbMomInertOfGirderLeft,
                                    tbMomInertOfGirderLeftBottom,
                                    tbMomInertRatioLeft,
                                    tbMomInertRatioLeftBottom,
                                    tbSpanRight,
                                    tbMomInertOfGirderRight,
                                    tbMomInertOfGirderRightBottom,
                                    tbMomInertRatioRight,
                                    tbMomInertRatioRightBottom,
                                    tbNumberOfSpans
                                   };
            // массив обозначений полей для текста ошибки
            string[] arrNameForErr = {
                                      GeneralRes.h,
                                      GeneralRes.l1,
                                      GeneralRes.Ic,
                                      GeneralRes.Is1,
                                      GeneralRes.Ii1,
                                      GeneralRes.IcOnIs1,
                                      GeneralRes.IcOnIi1,
                                      GeneralRes.l2,
                                      GeneralRes.Is2,
                                      GeneralRes.Ii2,
                                      GeneralRes.IcOnIs2,
                                      GeneralRes.IcOnIi2,
                                      GeneralRes.k
                                     };
            // изменение обозначения некоторых полей при определении расчетной длинны крайней колонны
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) == 0)
            {
                arrNameForErr[1] = GeneralRes.l;
                arrNameForErr[3] = GeneralRes.Is;
                arrNameForErr[4] = GeneralRes.Ii;
                arrNameForErr[5] = GeneralRes.IcOnIs;
                arrNameForErr[6] = GeneralRes.IcOnIi;
            }

            // цикл проверки на "пусто" для видимых и доступных текстбоксов
            for (int i = 0; i < arrTextBox.Length; i++)
            {
                if (arrTextBox[i].Visible == true && arrTextBox[i].Enabled == true)
                {
                    if (arrTextBox[i].Text.Length == 0)
                    {
                        msgInputAbsent = msgInputAbsent + " " + arrNameForErr[i] + ",";
                        quantityOfInputAbsent++;
                    }
                }
            }
            if (quantityOfInputAbsent > 0) // при наличии пустых текстбоксов выдается ошибка
            {
                string msgPrefix = GeneralRes.NotDefineValuesFor;
                if (quantityOfInputAbsent == 1)
                    msgPrefix = GeneralRes.NotDefineValueFor;
                msgInputAbsent = msgPrefix + msgInputAbsent.Remove(msgInputAbsent.Length - 1, 1) + "!\n";
            }
            
            double fictiveNumberOfSpans = 0;  // для того чтоб если К не число, оно было в списке со всеми не числами, а иначе отдельно будет "не целое число"
            double[] arrValues = {
                                  designOfEffectiveLenghtOfSimpleColumn.height,
                                  designOfEffectiveLenghtOfSimpleColumn.spanLeft,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertOfColumn,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeft,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeftBottom,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeft,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeftBottom,
                                  designOfEffectiveLenghtOfSimpleColumn.spanRight,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRight,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRightBottom,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertRatioRight,
                                  designOfEffectiveLenghtOfSimpleColumn.momInertRatioRightBottom,
                                  fictiveNumberOfSpans
                                 };
            int valueNumberOfSpans = designOfEffectiveLenghtOfSimpleColumn.numberOfSpans;
            // цикл проверки на отрицательность и равенство нулю значений для видимых и доступных текстбоксов, с последующей конвертацией отличных от нуля положительных значений
            for (int i = 0; i < arrValues.Length; i++)
            {
                if (arrTextBox[i].Visible == true && arrTextBox[i].Enabled == true)
                {
                    if (arrTextBox[i].Text.Length != 0)  // чтоб области ошибок не накладывались
                    {
                        if (!double.TryParse(arrTextBox[i].Text, out arrValues[i]))
                        {
                            msgValuesNotNumbers = msgValuesNotNumbers + " " + arrNameForErr[i] + ",";
                            quantityOfValuesNotNumbers++;
                        }
                        else
                        {
                            if (arrValues[i] <= 0)
                            {
                                if (i == arrValues.Length - 1) // кол-во пролетов не должно пропроверяться <= 0, дальше реализована проверка < 2
                                    break;
                                msgValuesInvalid = msgValuesInvalid + " " + arrNameForErr[i] + ",";
                                quantityOfValuesInvalid++;
                            }
                        }   
                    }                    
                }
                else
                    arrValues[i] = -1;      // так как все поля должны быть заполнены, невидимым и недоступным присваивается флаг
            }

            if (quantityOfValuesNotNumbers > 0)   // при наличии значений текстбоксов содержащих не числа
            {
                string msgPrefix = GeneralRes.Values;
                string msgPostfix = " " + GeneralRes.NotANumbers;
                if (quantityOfValuesNotNumbers == 1)
                {
                    msgPrefix = GeneralRes.Value;
                    msgPostfix = " " + GeneralRes.NotANumber;
                }
                msgValuesNotNumbers = msgPrefix + msgValuesNotNumbers.Remove(msgValuesNotNumbers.Length - 1, 1) + msgPostfix + "\n";
            }
            
            if (quantityOfValuesInvalid > 0)   // при наличии значений текстбоксов меньше или равных нулю формирование текста ошибки
            {
                string msgPrefix = GeneralRes.Values;
                string msgPostfix = " " + GeneralRes.mustBeGreaterThanZeroPlural;
                if (quantityOfValuesInvalid == 1)
                {
                    msgPrefix = GeneralRes.Value;
                    msgPostfix = " " + GeneralRes.mustBeGreaterThanZeroSingle;
                }
                msgValuesInvalid = msgPrefix + msgValuesInvalid.Remove(msgValuesInvalid.Length - 1, 1) + msgPostfix + "\n";
            }

            // отдельная реализация проверки и конвертации поля "кол-во пролетов"
            if (tbNumberOfSpans.Visible == true && tbNumberOfSpans.Enabled == true)
            {
                if (tbNumberOfSpans.Text.Length != 0 && double.TryParse(arrTextBox[12].Text, out arrValues[12]))  // чтоб области ошибок не накладывались
                {
                    if (!int.TryParse(tbNumberOfSpans.Text, out valueNumberOfSpans))
                    {
                        msgNumberOfSpansNotNaturalNumber = GeneralRes.NumOfSpansNotWholeNumber + "\n";
                        NumberOfSpansNotNaturalNumber = true;
                    }
                    else
                    {
                        if (tbNumberOfSpans.Text.Length != 0 && double.TryParse(arrTextBox[12].Text, out arrValues[12]) && valueNumberOfSpans < 2)
                        {
                            msgNumberOfSpansInvalid = GeneralRes.NumberOfSpansShallBeAtLeastTwo + "\n";
                            NumberOfSpansInvalid = true;
                        }
                    }
                }
            }
            else
                valueNumberOfSpans = -1;      // так как все поля должны быть заполнены, невидимым и недоступным присваивается флаг
              
            // присваивание полученных значений полям объекта класса расчета       
            designOfEffectiveLenghtOfSimpleColumn.height = arrValues[0];
            designOfEffectiveLenghtOfSimpleColumn.spanLeft = arrValues[1];
            designOfEffectiveLenghtOfSimpleColumn.momInertOfColumn = arrValues[2];
            designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeft = arrValues[3];
            designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeftBottom = arrValues[4];
            designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeft = arrValues[5];
            designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeftBottom = arrValues[6];
            designOfEffectiveLenghtOfSimpleColumn.spanRight = arrValues[7];
            designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRight = arrValues[8];
            designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRightBottom = arrValues[9];
            designOfEffectiveLenghtOfSimpleColumn.momInertRatioRight = arrValues[10];
            designOfEffectiveLenghtOfSimpleColumn.momInertRatioRightBottom = arrValues[11];
            designOfEffectiveLenghtOfSimpleColumn.numberOfSpans = valueNumberOfSpans;
            
            if (quantityOfInputAbsent > 0 || quantityOfValuesNotNumbers > 0 || NumberOfSpansNotNaturalNumber == true ||
                quantityOfValuesInvalid > 0 || NumberOfSpansInvalid == true)
            {
                MessageBox.Show(msgInputAbsent + msgValuesNotNumbers + msgNumberOfSpansNotNaturalNumber +
                    msgValuesInvalid + msgNumberOfSpansInvalid, GeneralRes.Attention);
                return false;
            }
            else
                return true;    
        }

        private void CleanResultTextboxes()  // метод очищающий поля результатов
        {
            if (actualResults)
            {
                tbResultMu.Text = string.Empty;
                designOfEffectiveLenghtOfSimpleColumn.mu = -1;
                tbResultEffectiveLenght.Text = string.Empty;
                designOfEffectiveLenghtOfSimpleColumn.effectiveLenght = -1;
                actualResults = false;
            }
        }

        private void SetTextboxLastValue(TextBox textbox, double value)   // метод, задающий последнее сохраненное значение текстбоску, при значении меньше 0, т.е. флаг "-1", текстбокс задается пустым. Используется при инициализации
        {
            if (value > 0)
                textbox.Text = value.ToString();
            else
                textbox.Text = string.Empty;
        }

        public void SetLastControlState()   // метод, "восстанавливающий" последнее сохраненное состояние юзерконтрола. Используется при инициализации
        {
            designOfEffectiveLenghtOfSimpleColumn.ReadEffectiveLength();
            {
                framePropertiesOfSimpleColumn = designOfEffectiveLenghtOfSimpleColumn.framePropertiesSimpleColumn;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.FrameType) == 0)
                    rbtnFreeFrame.Checked = true;
                else
                    rbtnUnfreeFrame.Checked = true;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.NumberOfStoreys) == 0)
                    rbtnOneStorey.Checked = true;
                else
                    rbtnMultipleStoreys.Checked = true;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) == 0)
                    rbtnBottomStorey.Checked = true;
                else if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey1stBit) == 0)
                    rbtnMiddleStorey.Checked = true;
                else
                    rbtnTopStorey.Checked = true;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.BearingType) == 0)
                    rbtnSwivelBearing.Checked = true;
                else
                    rbtnRigidBearing.Checked = true;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) == 0)
                    rbtnExtermeColumn.Checked = true;
                else
                    rbtnMiddleColumn.Checked = true;
                if (designOfEffectiveLenghtOfSimpleColumn.momInertValues)
                    rbtnMomInertValues.Checked = true;
                else
                    rbtnMomInertRatio.Checked = true;
                if (designOfEffectiveLenghtOfSimpleColumn.duplicateLeftToRight)
                    chbxDuplicateLeftToRight.Checked = true;
                else
                    chbxDuplicateLeftToRight.Checked = false;

                SetTextboxLastValue(tbHeightLeft, designOfEffectiveLenghtOfSimpleColumn.height);
                SetTextboxLastValue(tbSpanLeft, designOfEffectiveLenghtOfSimpleColumn.spanLeft);
                SetTextboxLastValue(tbMomInertOfColumnLeft, designOfEffectiveLenghtOfSimpleColumn.momInertOfColumn);
                SetTextboxLastValue(tbMomInertOfGirderLeft, designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeft);
                SetTextboxLastValue(tbMomInertOfGirderLeftBottom, designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderLeftBottom);
                SetTextboxLastValue(tbMomInertRatioLeft, designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeft);
                SetTextboxLastValue(tbMomInertRatioLeftBottom, designOfEffectiveLenghtOfSimpleColumn.momInertRatioLeftBottom);
                SetTextboxLastValue(tbNumberOfSpans, designOfEffectiveLenghtOfSimpleColumn.numberOfSpans);
                SetTextboxLastValue(tbHeightRight, designOfEffectiveLenghtOfSimpleColumn.height);
                SetTextboxLastValue(tbSpanRight, designOfEffectiveLenghtOfSimpleColumn.spanRight);
                SetTextboxLastValue(tbMomInertOfColumnRight, designOfEffectiveLenghtOfSimpleColumn.momInertOfColumn);
                SetTextboxLastValue(tbMomInertOfGirderRight, designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRight);
                SetTextboxLastValue(tbMomInertOfGirderRightBottom, designOfEffectiveLenghtOfSimpleColumn.momInertOfGirderRightBottom);
                SetTextboxLastValue(tbMomInertRatioRight, designOfEffectiveLenghtOfSimpleColumn.momInertRatioRight);
                SetTextboxLastValue(tbMomInertRatioRightBottom, designOfEffectiveLenghtOfSimpleColumn.momInertRatioRightBottom);
                SetTextboxLastValue(tbResultMu, Math.Round(designOfEffectiveLenghtOfSimpleColumn.mu, 3));
                SetTextboxLastValue(tbResultEffectiveLenght, Math.Round(designOfEffectiveLenghtOfSimpleColumn.effectiveLenght, 3));

                if (tbResultMu.Text == String.Empty && tbResultEffectiveLenght.Text == String.Empty)
                    actualResults = false;
                else
                    actualResults = true;
            }            
        }

        public void UploadLastData()  // метод, сохраняющий состояние юзерконтрола. Используется для последующей инициализации
        {
            designOfEffectiveLenghtOfSimpleColumn.WriteEffectiveLength();
        }

    // Обработка выбора параметров рамы (рабиобатонны)
        private void rbtnFreeFrame_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if(rbtnFreeFrame.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.FrameType;   // xxxx xxx0 | см. описание unum FrameProperties
                DrawPicture();
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                {
                    grbxNumberOfSpans.Enabled = true;
                    tbNumberOfSpans.Enabled = true;
                    tbNumberOfSpans.BackColor = Color.White;
                }
            }
            else
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn | FramePropertiesOfSimpleColumn.FrameType;    // xxxx xxx1 | см. описание unum FrameProperties                
                DrawPicture();
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                {
                    grbxNumberOfSpans.Enabled = false;
                    tbNumberOfSpans.Enabled = false;
                    tbNumberOfSpans.Text = string.Empty;
                    tbNumberOfSpans.BackColor = Control.DefaultBackColor;
                }                
            }
        }        

        private void rbtnOneStorey_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnOneStorey.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.NumberOfStoreys;   // xxxx xx0x | см. описание unum FrameProperties
                DrawPicture();
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)                    
                    rbtnBottomStorey.Checked = true;
                grbxConcernedStorey.Enabled = false;
            }
            else
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn | FramePropertiesOfSimpleColumn.NumberOfStoreys;   // xxxx xx1x | см. описание unum FrameProperties
                DrawPicture();
                grbxConcernedStorey.Enabled = true;
            }
        }

        private void rbtnBottomStorey_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnBottomStorey.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.ConcernedStorey1stBit 
                    & ~FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit;   // xxxx 00xx  | см. описание unum FrameProperties
                DrawPicture();
                RemoveBottomGigderControls();
                grbxBearingType.Enabled = true;
            }           
        }
       
        private void rbtnMiddleStorey_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if(rbtnMiddleStorey.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.ConcernedStorey1stBit
                   | FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit;   // xxxx 10xx | см. описание unum FrameProperties
                DrawPicture();
                rbtnSwivelBearing.Checked = true;
                grbxBearingType.Enabled = false;               
                AddBottomGigderControls();
            }           
        }

        private void rbtnTopStorey_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnTopStorey.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn | FramePropertiesOfSimpleColumn.ConcernedStorey1stBit
                    | FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit;   // xxxx 11xx | см. описание unum FrameProperties
                DrawPicture();
                rbtnSwivelBearing.Checked = true;
                grbxBearingType.Enabled = false;
                AddBottomGigderControls();
            }
        }
        
        private void rbtnSwivelBearing_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnSwivelBearing.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.BearingType;   // xxx0 xxxx | см. описание unum FrameProperties
                DrawPicture();
            }
            else
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn | FramePropertiesOfSimpleColumn.BearingType;    // xxx1 xxxx | см. описание unum FrameProperties
                DrawPicture();
            }
        }
        
        private void rbtnExtermeColumn_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnExtermeColumn.Checked)
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn & ~FramePropertiesOfSimpleColumn.ConcernedColumn;   // xx0x xxxx | см. описание unum FrameProperties              
                DrawPicture();
            // параметры слева
                // изменение подписей
                grbxParamsOfFrameOnLeft.Text = GeneralRes.ParamsOfFrame;
                lblSpanLeft.Text = GeneralRes.l;
                lblMomInertOfGirderLeft.Text = GeneralRes.Is;
                lblMomInertRatioLeft.Text = GeneralRes.IcOnIs;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)  // для нижних ригелей
                {
                    lblMomInertOfGirderLeftBottom.Text = GeneralRes.Ii;
                    lblMomInertRatioLeftBottom.Text = GeneralRes.IcOnIi;
                }
            // параметры справа и кол-во пролетов
                grbxNumberOfSpans.Visible = false;
                grbxParamsOfFrameOnRight.Visible = false;
            }
            else
            {
                framePropertiesOfSimpleColumn = framePropertiesOfSimpleColumn | FramePropertiesOfSimpleColumn.ConcernedColumn;   // xx1x xxxx | см. описание unum FrameProperties
                DrawPicture();
            // параметры слева
                // изменение подписей
                grbxParamsOfFrameOnLeft.Text = GeneralRes.ParamsOfFrameOnLeft;
                lblSpanLeft.Text = GeneralRes.l1;
                lblMomInertOfGirderLeft.Text = GeneralRes.Is1;
                lblMomInertRatioLeft.Text = GeneralRes.IcOnIs1;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)  // для нижних ригелей
                {
                    lblMomInertOfGirderLeftBottom.Text = GeneralRes.Ii1;
                    lblMomInertRatioLeftBottom.Text = GeneralRes.IcOnIi1;
                }
            // параметры справа и кол-во пролетов
                grbxNumberOfSpans.Visible = true;
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.FrameType) == 0)
                {
                    grbxNumberOfSpans.Enabled = true;
                    tbNumberOfSpans.Enabled = true;
                    tbNumberOfSpans.BackColor = Color.White;
                }  
                else
                {
                    grbxNumberOfSpans.Enabled = false;
                    tbNumberOfSpans.Enabled = false;
                    tbNumberOfSpans.Text = string.Empty;
                    tbNumberOfSpans.BackColor = Control.DefaultBackColor;
                }
            // параметры справа
                grbxParamsOfFrameOnRight.Visible = true;
                // высота колонны
                tbHeightRight.Text = tbHeightLeft.Text;
                // длина пролета
                if (chbxDuplicateLeftToRight.Checked)
                {
                    tbSpanRight.Text = tbSpanLeft.Text;
                    tbSpanRight.ReadOnly = true;
                }
                else
                    tbSpanRight.ReadOnly = false; 
                // моменты инерции
                if (rbtnMomInertValues.Checked)  // активны величины моментов инерции
                {
                    // момент инерции колонны
                    lblMomInertOfColumnRight.Enabled = true;
                    tbMomInertOfColumnRight.Enabled = true;
                    tbMomInertOfColumnRight.Text = tbMomInertOfColumnLeft.Text;
                    tbMomInertOfColumnRight.ReadOnly = true;
                    lblUnitsMomInertOfColumnRight.Enabled = true;
                    // момент инерции верхнего ригеля
                    lblMomInertOfGirderRight.Enabled = true;
                    tbMomInertOfGirderRight.Enabled = true;
                    if (chbxDuplicateLeftToRight.Checked)
                    {
                        tbMomInertOfGirderRight.Text = tbMomInertOfGirderLeft.Text;
                        tbMomInertOfGirderRight.ReadOnly = true;
                    }
                    else
                        tbMomInertOfGirderRight.ReadOnly = false;
                    lblUnitsMomInertOfGirderRight.Enabled = true;
                    // момент инерции нижнего ригеля
                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertOfGirderRightBottom.Visible = true;
                        lblMomInertOfGirderRightBottom.Enabled = true;                        
                        tbMomInertOfGirderRightBottom.Visible = true;
                        tbMomInertOfGirderRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertOfGirderRightBottom.Text = tbMomInertOfGirderLeftBottom.Text;
                            tbMomInertOfGirderRightBottom.ReadOnly = true;
                        }
                        else
                            tbMomInertOfGirderRightBottom.ReadOnly = false;
                        lblUnitsMomInertOfGirderRightBottom.Visible = true;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = true;                        
                    }
                    else
                    {
                        lblMomInertOfGirderRightBottom.Visible = false;
                        tbMomInertOfGirderRightBottom.Visible = false;
                        lblUnitsMomInertOfGirderRightBottom.Visible = false;
                    }
                    // соотношение моментов инерции колонны и верхнего ригеля
                    lblMomInertRatioRight.Enabled = false;
                    tbMomInertRatioRight.Enabled = false;
                    tbMomInertRatioRight.Text = string.Empty;
                    // соотношение моментов инерции колонны и нижнего ригеля
                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertRatioRightBottom.Visible = true;
                        lblMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.Visible = true;
                        tbMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.Text = string.Empty;
                    }
                    else
                    {
                        lblMomInertRatioRightBottom.Visible = false;
                        tbMomInertRatioRightBottom.Visible = false;
                    }
                }
                else  // активны соотношения моментов инерции колонны ригелей
                {
                    // момент инерции колонны 
                    lblMomInertOfColumnRight.Enabled = false;
                    tbMomInertOfColumnRight.Enabled = false;
                    tbMomInertOfColumnRight.Text = string.Empty;
                    lblUnitsMomInertOfColumnRight.Enabled = false;
                    // момент инерции верхнего ригеля
                    lblMomInertOfGirderRight.Enabled = false;
                    tbMomInertOfGirderRight.Enabled = false;
                    tbMomInertOfGirderRight.Text = string.Empty;
                    lblUnitsMomInertOfGirderRight.Enabled = false;
                    // момент инерции нижнего ригеля
                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertOfGirderRightBottom.Visible = true;
                        lblMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.Visible = true;
                        tbMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.Text = string.Empty;
                        lblUnitsMomInertOfGirderRightBottom.Visible = true;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = false;
                    }
                    else
                    {
                        lblMomInertOfGirderRightBottom.Visible = false;
                        tbMomInertOfGirderRightBottom.Visible = false;
                        lblUnitsMomInertOfGirderRightBottom.Visible = false;
                    }
                    // соотношение моментов инерции колонны и верхнего ригеля
                    lblMomInertRatioRight.Enabled = true;
                    tbMomInertRatioRight.Enabled = true;
                    if (chbxDuplicateLeftToRight.Checked)
                    {
                        tbMomInertRatioRight.Text = tbMomInertRatioLeft.Text;
                        tbMomInertRatioRight.ReadOnly = true;
                    }
                    else
                        tbMomInertRatioRight.ReadOnly = false;
                    // соотношение моментов инерции колонны и нижнего ригеля
                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertRatioRightBottom.Visible = true;
                        lblMomInertRatioRightBottom.Enabled = true;                        
                        tbMomInertRatioRightBottom.Visible= true;                        
                        tbMomInertRatioRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertRatioRightBottom.Text = tbMomInertRatioLeftBottom.Text;
                            tbMomInertRatioRightBottom.ReadOnly = true;
                        }
                        else
                            tbMomInertRatioRightBottom.ReadOnly = false;
                    }
                    else
                    {
                        lblMomInertRatioRightBottom.Visible = false;
                        tbMomInertRatioRightBottom.Visible = false; 
                    }
                }                
            }            
        }

    // Обработка изменения параметров рамы    
        // текстбоксы
        private void tbHeightLeft_TextChanged(object sender, EventArgs e)
        {   
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)            
                tbHeightRight.Text = tbHeightLeft.Text;            
        }

        private void tbSpanLeft_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                if (chbxDuplicateLeftToRight.Checked)
                    tbSpanRight.Text = tbSpanLeft.Text;
        }

        private void tbMomInertColumnLeft_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                tbMomInertOfColumnRight.Text = tbMomInertOfColumnLeft.Text;
        }

        private void tbMomInertGirderLeft_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                if (chbxDuplicateLeftToRight.Checked)
                    tbMomInertOfGirderRight.Text = tbMomInertOfGirderLeft.Text;
        }

        private void tbMomInertGirderLeftBottom_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                if (chbxDuplicateLeftToRight.Checked)
                    tbMomInertOfGirderRightBottom.Text = tbMomInertOfGirderLeftBottom.Text;
        }

        private void tbMomInertRatioColumnToGirderLeft_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                if (chbxDuplicateLeftToRight.Checked)
                    tbMomInertRatioRight.Text = tbMomInertRatioLeft.Text;
        }

        private void tbMomInertRatioColumnToGirderLeftBottom_TextChanged(object sender, EventArgs e)
        {
            ValidationOfInputLeft(sender, e);
            if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                if (chbxDuplicateLeftToRight.Checked)
                    tbMomInertRatioRightBottom.Text = tbMomInertRatioLeftBottom.Text;
        }

        private void tbNumberOfSpans_TextChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            TextBox textBox = sender as TextBox;
            int a;
            if ((int.TryParse(textBox.Text, out a) && a >= 2) || textBox.Text == string.Empty)
                textBox.BackColor = Color.White;
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);
        }  

        // радиобаттон
        private void rbtnMomInertValues_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
        // Управление состоянием параметров моментов инерции
            if (rbtnMomInertValues.Checked)
            {
            // слева, активно Values
                lblMomInertOfColumnLeft.Enabled = true;
                tbMomInertOfColumnLeft.Enabled = true;
                tbMomInertOfColumnLeft.BackColor = Color.White;
                lblUnitsMomInertOfColumnLeft.Enabled = true;

                lblMomInertOfGirderLeft.Enabled = true;
                tbMomInertOfGirderLeft.Enabled = true;
                tbMomInertOfGirderLeft.BackColor = Color.White;
                lblUnitsMomInertOfGirderLeft.Enabled = true;

                lblMomInertRatioLeft.Enabled = false;
                tbMomInertRatioLeft.Text = string.Empty;
                tbMomInertRatioLeft.Enabled = false;
                tbMomInertRatioLeft.BackColor = Control.DefaultBackColor;

                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                {
                    lblMomInertOfGirderLeftBottom.Enabled = true;
                    tbMomInertOfGirderLeftBottom.Enabled = true;
                    tbMomInertOfGirderLeftBottom.BackColor = Color.White;
                    lblUnitsMomInertOfGirderLeftBottom.Enabled = true;               
                
                    lblMomInertRatioLeftBottom.Enabled = false;
                    tbMomInertRatioLeftBottom.Text = string.Empty;
                    tbMomInertRatioLeftBottom.Enabled = false;
                    tbMomInertRatioLeftBottom.BackColor = Control.DefaultBackColor;
                }

            //справа, активно Values
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                {
                    lblMomInertOfColumnRight.Enabled = true;
                    tbMomInertOfColumnRight.Enabled = true;
                    lblUnitsMomInertOfColumnRight.Enabled = true;

                    lblMomInertOfGirderRight.Enabled = true;
                    tbMomInertOfGirderRight.Enabled = true;
                    lblUnitsMomInertOfGirderRight.Enabled = true;
                    if (chbxDuplicateLeftToRight.Checked)
                    {
                        tbMomInertOfGirderRight.ReadOnly = true;
                        tbMomInertOfGirderRight.BackColor = Control.DefaultBackColor;
                    }
                    else
                    {
                        tbMomInertOfGirderRight.ReadOnly = false;
                        tbMomInertOfGirderRight.BackColor = Color.White;
                    }

                    lblMomInertRatioRight.Enabled = false;
                    tbMomInertRatioRight.Text = string.Empty;
                    tbMomInertRatioRight.Enabled = false;
                    tbMomInertRatioRight.BackColor = Control.DefaultBackColor;

                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertOfGirderRightBottom.Enabled = true;
                        tbMomInertOfGirderRightBottom.Enabled = true;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertOfGirderRightBottom.ReadOnly = true;
                            tbMomInertOfGirderRightBottom.BackColor = Control.DefaultBackColor;
                        }
                        else
                        {
                            tbMomInertOfGirderRightBottom.ReadOnly = false;
                            tbMomInertOfGirderRightBottom.BackColor = Color.White;
                        }

                        lblMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.Text = string.Empty;
                        tbMomInertRatioRightBottom.Enabled = false;
                        tbMomInertRatioRightBottom.BackColor = Control.DefaultBackColor;
                    }
                }                
            }
            else
            {
            // слева, активно Ratio
                lblMomInertOfColumnLeft.Enabled = false;
                tbMomInertOfColumnLeft.Text = string.Empty;                
                tbMomInertOfColumnLeft.Enabled = false;
                tbMomInertOfColumnLeft.BackColor = Control.DefaultBackColor;
                lblUnitsMomInertOfColumnLeft.Enabled = false;

                lblMomInertOfGirderLeft.Enabled = false;
                tbMomInertOfGirderLeft.Text = string.Empty;
                tbMomInertOfGirderLeft.Enabled = false;
                tbMomInertOfGirderLeft.BackColor = Control.DefaultBackColor;
                lblUnitsMomInertOfGirderLeft.Enabled = false;

                lblMomInertRatioLeft.Enabled = true;
                tbMomInertRatioLeft.Enabled = true;
                tbMomInertRatioLeft.BackColor = Color.White;

                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                {
                    lblMomInertOfGirderLeftBottom.Enabled = false;
                    tbMomInertOfGirderLeftBottom.Text = string.Empty;
                    tbMomInertOfGirderLeftBottom.Enabled = false;
                    tbMomInertOfGirderLeftBottom.BackColor = Control.DefaultBackColor;
                    lblUnitsMomInertOfGirderLeftBottom.Enabled = false;

                    lblMomInertRatioLeftBottom.Enabled = true;
                    tbMomInertRatioLeftBottom.Enabled = true;
                    tbMomInertRatioLeftBottom.BackColor = Color.White;
                }
                
            // справа, активно Ratio
                if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)
                {
                    lblMomInertOfColumnRight.Enabled = false;
                    tbMomInertOfColumnRight.Text = string.Empty;
                    tbMomInertOfColumnRight.Enabled = false;
                    lblUnitsMomInertOfColumnRight.Enabled = false;

                    lblMomInertOfGirderRight.Enabled = false;
                    tbMomInertOfGirderRight.Text = string.Empty;
                    tbMomInertOfGirderRight.Enabled = false;
                    tbMomInertOfGirderRight.BackColor = Control.DefaultBackColor;
                    lblUnitsMomInertOfGirderRight.Enabled = false;

                    lblMomInertRatioRight.Enabled = true;
                    tbMomInertRatioRight.Enabled = true;
                    if (chbxDuplicateLeftToRight.Checked)
                    {
                        tbMomInertRatioRight.ReadOnly = true;
                        tbMomInertRatioRight.BackColor = Control.DefaultBackColor;
                    }
                    else
                    {
                        tbMomInertRatioRight.ReadOnly = false;
                        tbMomInertRatioRight.BackColor = Color.White;                    
                    }
                    
                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        lblMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.Text = string.Empty;
                        tbMomInertOfGirderRightBottom.Enabled = false;
                        tbMomInertOfGirderRightBottom.BackColor = Control.DefaultBackColor;
                        lblUnitsMomInertOfGirderRightBottom.Enabled = false;

                        lblMomInertRatioRightBottom.Enabled = true;
                        tbMomInertRatioRightBottom.Enabled = true;
                        if (chbxDuplicateLeftToRight.Checked)
                        {
                            tbMomInertRatioRightBottom.ReadOnly = true;
                            tbMomInertRatioRightBottom.BackColor = Control.DefaultBackColor;
                        }
                        else
                        {
                            tbMomInertRatioRightBottom.ReadOnly = false;
                            tbMomInertRatioRightBottom.BackColor = Color.White;
                        }
                    }
                }                
            }
        }        
        // чекбокс
        private void chbxDuplicateLeftToRight_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (chbxDuplicateLeftToRight.Checked)
            {
                tbSpanRight.ReadOnly = true;
                tbSpanRight.Text = tbSpanLeft.Text;
                ValidationOfTBValue(tbSpanRight, Control.DefaultBackColor);

                if (rbtnMomInertValues.Checked)
                {
                    tbMomInertOfGirderRight.ReadOnly = true;
                    tbMomInertOfGirderRight.Text = tbMomInertOfGirderLeft.Text;
                    ValidationOfTBValue(tbMomInertOfGirderRight, Control.DefaultBackColor);

                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        tbMomInertOfGirderRightBottom.ReadOnly = true;
                        tbMomInertOfGirderRightBottom.Text = tbMomInertOfGirderLeftBottom.Text;
                        ValidationOfTBValue(tbMomInertOfGirderRightBottom, Control.DefaultBackColor);
                    }
                }
                else
                {
                    tbMomInertRatioRight.ReadOnly = true;
                    tbMomInertRatioRight.Text = tbMomInertRatioLeft.Text;
                    ValidationOfTBValue(tbMomInertRatioRight, Control.DefaultBackColor);

                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    { 
                        tbMomInertRatioRightBottom.ReadOnly = true;
                        tbMomInertRatioRightBottom.Text = tbMomInertRatioLeftBottom.Text;
                        ValidationOfTBValue(tbMomInertRatioRightBottom, Control.DefaultBackColor);
                    }
                }
            }
            else
            {
                tbSpanRight.ReadOnly = false;
                ValidationOfTBValue(tbSpanRight, Color.White);

                if (rbtnMomInertValues.Checked)
                {
                    tbMomInertOfGirderRight.ReadOnly = false;
                    ValidationOfTBValue(tbMomInertOfGirderRight, Color.White);

                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        tbMomInertOfGirderRightBottom.ReadOnly = false;
                        ValidationOfTBValue(tbMomInertOfGirderRightBottom, Color.White);
                    }
                }
                else
                {
                    tbMomInertRatioRight.ReadOnly = false;
                    ValidationOfTBValue(tbMomInertRatioRight, Color.White);

                    if ((framePropertiesOfSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                    {
                        tbMomInertRatioRightBottom.ReadOnly = false;
                        ValidationOfTBValue(tbMomInertRatioRightBottom, Color.White);
                    }
                }
            }
        }                 
        
    // Обработка нажатия кнопки "Расчет"
        private void btnDesign_Click(object sender, EventArgs e)
        {
            if (!actualResults)
            {                
                // проверка текстбоксов на соответствие ограничениям и конвертация допустимых значений
                if (!DataValidation())
                    return;
                // передача параметров рамы
                designOfEffectiveLenghtOfSimpleColumn.framePropertiesSimpleColumn = framePropertiesOfSimpleColumn;
                // передача способа задания моментов инерции
                designOfEffectiveLenghtOfSimpleColumn.momInertValues = rbtnMomInertValues.Checked;
                // передача параметра дублирования параметров правой части в левую
                designOfEffectiveLenghtOfSimpleColumn.duplicateLeftToRight = chbxDuplicateLeftToRight.Checked;

                // собственно расчет
                actualResults = designOfEffectiveLenghtOfSimpleColumn.CalculateEffectiveLength();

                // Прием и вывод решения
                tbResultMu.Text = Math.Round(designOfEffectiveLenghtOfSimpleColumn.mu, 3).ToString();
                tbResultEffectiveLenght.Text = Math.Round(designOfEffectiveLenghtOfSimpleColumn.effectiveLenght, 3).ToString();
            }            
        }          
    }
}
