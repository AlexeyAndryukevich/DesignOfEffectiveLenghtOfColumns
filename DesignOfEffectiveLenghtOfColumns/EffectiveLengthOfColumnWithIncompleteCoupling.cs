using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignOfEffectiveLenghtOfColumns
{
    enum FramePropertiesOfColumnWithIncompleteCoupling : byte    // 8 бит, первые два из которых отвечают за тип рамы (состояние радио кнопок), что определяет случай расчета
    {
        BearingOfColumn = 0x01,            // 0000 0010 | 02 | Опирание стойки: 0 - шарнирное, 1 - жесткое
        PositionOfCrossbrace = 0x02,       // 0000 0001 | 01 | Расположение связи: 0 - внизу, 1 - вверху

        // Возможные комбинации:
        SwivelBearingOfColumn_CrossbraceOnBottom = 0x00,         // 0000 0000 | 00
        RigidBearingOfColumn_CrossbraceOnBottom = 0x01,          // 0000 0001 | 01
        SwivelBearingOfColumn_CrossbraceOnTop = 0x02,            // 0000 0010 | 02
        RigidBearingOfColumn_CrossbraceOnTop = 0x03,             // 0000 0011 | 03        
    }


    partial class EffectiveLengthOfColumnWithIncompleteCoupling : UserControl, IMyUserControl
    {
    // инициализация необходимых переменных и обьектов
        DesignOfEffectiveLengthOfColumnWithIncompleteCoupling designOfEffectiveLengthOfColumnWithIncompleteCoupling = new DesignOfEffectiveLengthOfColumnWithIncompleteCoupling();
        FramePropertiesOfColumnWithIncompleteCoupling framePropertiesOfColumnWithIncompleteCoupling = 0x00;     // 0000 0000 | 00 
        bool actualResults = false;
    
        // конструктор юзер контрола
        public EffectiveLengthOfColumnWithIncompleteCoupling()
        {
            InitializeComponent();
            InitializeOfControlsProperties();
        }

    // свои вспомагательные методы  
        private void InitializeOfControlsProperties()   
        {
            // заполнение текста контролов из файла реурсов
            grbxBearingOfColumn.Text = GeneralRes.BearingOfColumn;
            rbtnSwivelBearing.Text = GeneralRes.swivelBearing;
            rbtnRigidBearing.Text = GeneralRes.rigidBearing;
            grbxPositionOfCrossbrace.Text = GeneralRes.PositionOfCrossbrace;
            grbxParamOfBottomPart.Text = GeneralRes.ParamOfColumn;
            grbxParamOfTopPart.Text = GeneralRes.ParamOfTopPart;
            lblTxtHeight.Text = GeneralRes.FullHeight;
            lblTxtHeightFree.Text = GeneralRes.FreeHeight;
            lblTxtMomInertBottom.Text = GeneralRes.MomInert;
            lblTxtForce.Text = GeneralRes.Force;
            lblIndHeightBottom.Text = GeneralRes.h;
            lblUnitHeightBottom.Text = GeneralRes.m;
            lblIndHeightFree.Text = GeneralRes.c;
            lblUnitHeightFree.Text = GeneralRes.m;
            lblIndMomInertBottom.Text = GeneralRes.I1;
            lblUnitMomInertBottom.Text = GeneralRes.cm4; 
            lblIndForceBottom.Text = GeneralRes.F1;
            lblUnitForceBottom.Text = GeneralRes.tf;
            lblIndHeightTop.Text = GeneralRes.h2;
            lblUnitHeightTop.Text = GeneralRes.m;
            lblIndMomInertTop.Text = GeneralRes.I2;
            lblUnitMomInertTop.Text = GeneralRes.cm4;
            lblIndForceTop.Text = GeneralRes.F2;
            lblUnitForceTop.Text = GeneralRes.tf;
            btnDesign.Text = GeneralRes.Design;
            grbxResultsOfBottomPart.Text = GeneralRes.Results;
            grbxResultsOfTopPart.Text = GeneralRes.ResultsOfTopPart;
            lblTxtMu.Text = GeneralRes.CoeffOfEffectLength;
            lblTxtEffectLength.Text = GeneralRes.EffectLengthMuOnH;
            lblIndMuBottom.Text = GeneralRes.mu;
            lblIndEffectLengthBottom.Text = GeneralRes.lef;
            lblUnitEffectLengthBottom.Text = GeneralRes.m;
            lblIndMuTop.Text = GeneralRes.mu2;
            lblIndEffectLengthTop.Text = GeneralRes.lef2;
            lblUnitEffectLengthTop.Text = GeneralRes.m;                
        }

        private void DrawPictures()  // метод для смены изображения из файла ресурсов в зависимости от текущих параметров рамы
        {
            switch (framePropertiesOfColumnWithIncompleteCoupling)
            {
                case FramePropertiesOfColumnWithIncompleteCoupling.SwivelBearingOfColumn_CrossbraceOnBottom:
                    picbxFrame.Image = GeneralRes.BottomCrossbracing_SwivelBearing;
                    break;
                case FramePropertiesOfColumnWithIncompleteCoupling.RigidBearingOfColumn_CrossbraceOnBottom:
                    picbxFrame.Image = GeneralRes.BottomCrossbracing_RigidBearing;
                    break;
                case FramePropertiesOfColumnWithIncompleteCoupling.SwivelBearingOfColumn_CrossbraceOnTop:
                    picbxFrame.Image = GeneralRes.TopCrossbracing_SwivelBearing;
                    break;
                case FramePropertiesOfColumnWithIncompleteCoupling.RigidBearingOfColumn_CrossbraceOnTop:
                    picbxFrame.Image = GeneralRes.TopCrossbracing_RigidBearing;
                    break;
            }
        }

        private void ValidationOfInput(object sender, EventArgs e)  // метод проверяющий содержимое поля текстбокса при вводе
        {
            CleanResultTextboxes();

            double a;
            TextBox textBox = sender as TextBox;
                       
            if ((double.TryParse(textBox.Text, out a) && a > 0) || textBox.Text == string.Empty)               
                textBox.BackColor = Color.White;
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);
        }

        private void ValidationOfInputCAndH(object sender, EventArgs e)  // частный случай метода для проверки при вводе содержимого полей текстбоксов "c" и "h"
        {
            CleanResultTextboxes();

            double a;
            double b;            
            TextBox currentTextbox = sender as TextBox;
            TextBox otherTextbox = tbHeightFree;

            if (currentTextbox.Name == tbHeightBottom.Name)            
                otherTextbox = tbHeightFree;            
            else if (currentTextbox.Name == tbHeightFree.Name)
                otherTextbox = tbHeightBottom;            

            if (double.TryParse(currentTextbox.Text, out a) && a > 0)
            {               
                if ((double.TryParse(otherTextbox.Text, out b) && b > 0))
                {
                    if ((currentTextbox.Name == tbHeightBottom.Name && a > b) || (currentTextbox.Name == tbHeightFree.Name && b > a))
                    {
                        currentTextbox.BackColor = Color.White;
                        otherTextbox.BackColor = Color.White;
                    }
                    else
                    {
                        currentTextbox.BackColor = Color.FromArgb(255, 128, 128);
                        otherTextbox.BackColor = Color.FromArgb(255, 128, 128);
                    }
                }
                else
                    currentTextbox.BackColor = Color.White;
            }
            else if (currentTextbox.Text == string.Empty)
            {
                currentTextbox.BackColor = Color.White;
                if (double.TryParse(otherTextbox.Text, out b) && b > 0)
                    otherTextbox.BackColor = Color.White;
            }
            else
            {
                currentTextbox.BackColor = Color.FromArgb(255, 128, 128);
                if (double.TryParse(otherTextbox.Text, out b) && b > 0)
                    otherTextbox.BackColor = Color.White;
            }
        }

        private void ValidationOfInputOfForceValue(object sender, EventArgs e)   // частный случай метода для проверки при вводе содержимого полей текстбоксов "F1" и "F2"
        {
            CleanResultTextboxes();

            TextBox textBox = sender as TextBox;
            double a;
            TextBox currentForceTB = tbForceTop;
            TextBox otherForceTB = tbForceBottom;
            double b;

            if ((double.TryParse(textBox.Text, out a) && a >= 0) || textBox.Text == string.Empty)
                textBox.BackColor = Color.White;
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);

            if (textBox.Name == tbForceTop.Name)
            {
                currentForceTB = tbForceTop;
                otherForceTB = tbForceBottom;
            }
            else if (textBox.Name == tbForceBottom.Name)
            {
                currentForceTB = tbForceBottom;
                otherForceTB = tbForceTop;
            }

            if (double.TryParse(otherForceTB.Text, out b) && b == 0)
            {
                if (double.TryParse(currentForceTB.Text, out a) && a == 0)
                {
                    currentForceTB.BackColor = Color.FromArgb(255, 128, 128);
                    otherForceTB.BackColor = Color.FromArgb(255, 128, 128);
                }
                else
                    otherForceTB.BackColor = Color.White;
            }
        }

        private bool DataValidation()  // метод проверяющий содержимое текстбоксов на соответствие пределам, в случае соответствия передает содержимое в поля класса расчета, в случае несоответствия - выдает ошибку
        {
            int quantityOfInputAbsent = 0;
            string msgInputAbsent = string.Empty;

            int quantityOfValuesNotNumbers = 0;
            string msgValuesNotNumbers = string.Empty;

            int quantityOfValuesInvalid = 0;
            string msgValuesInvalid = string.Empty;

            bool HEqualC = false;
            string msgInputHEqualC = string.Empty;

            bool F1plusF2Zero = false;
            string msgInputF1plusF2Zero = string.Empty;

            TextBox[] arrTextBox = {
                                    tbHeightBottom,
                                    tbHeightFree,
                                    tbMomInertBottom,
                                    tbForceBottom,
                                    tbHeightTop,
                                    tbMomInertTop,
                                    tbForceTop,
                                   };
            // массив обозначений полей для текста ошибки
            string[] arrNameForErr = {
                                      GeneralRes.h,
                                      GeneralRes.c,
                                      GeneralRes.I1,
                                      GeneralRes.F1,
                                      GeneralRes.h2,
                                      GeneralRes.I2,
                                      GeneralRes.F2,                                     
                                     };
            // изменение обозначения некоторых полей при определении расчетной длинны колонны рамы со связью вверху
            if ((framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace) != 0)
            {
                arrNameForErr[0] = GeneralRes.h1;               
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

            double[] arrValues = {                                 
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightBottom,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightFree,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertBottom,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceBottom,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightTop,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertTop,
                                  designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceTop,   
                                 };

            // цикл проверки на отрицательность и равенство нулю значений для видимых, с последующей конвертацией отличных от нуля положительных значений
            for (int i = 0; i < arrValues.Length; i++)
            {
                if (arrTextBox[i].Visible == true)
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
                            if (i != 3 && i != 6)
                            {
                                if (arrValues[i] <= 0)
                                {
                                    msgValuesInvalid = msgValuesInvalid + " " + arrNameForErr[i] + ",";
                                    quantityOfValuesInvalid++;
                                }
                            }
                            else
                            {
                                if (i == 6 && arrValues[3] == 0 && arrValues[6] == 0)
                                {
                                    msgInputF1plusF2Zero = GeneralRes.F2andF1CanNotEqualZero + "\n";
                                    F1plusF2Zero = true;
                                }
                                if (arrValues[i] < 0)
                                {
                                    msgValuesInvalid = msgValuesInvalid + " " + arrNameForErr[i] + ",";
                                    quantityOfValuesInvalid++;
                                }
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

            if (arrTextBox[0].Text != string.Empty && arrTextBox[1].Visible == true && arrTextBox[1].Text != string.Empty && arrValues[1] >= arrValues[0])
            {
                HEqualC = true;
                msgInputHEqualC = GeneralRes.ValueOfCCanNotBeLessOfH;
            }

            // присваивание полученных значений полям объекта класса расчета       
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightBottom = arrValues[0];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightFree = arrValues[1];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertBottom = arrValues[2];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceBottom = arrValues[3];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightTop = arrValues[4];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertTop = arrValues[5];
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceTop = arrValues[6];

            if (quantityOfInputAbsent > 0 || quantityOfValuesNotNumbers > 0 || quantityOfValuesInvalid > 0 || HEqualC == true || F1plusF2Zero == true)
            {
                MessageBox.Show(msgInputAbsent + msgValuesNotNumbers + msgValuesInvalid + msgInputHEqualC + msgInputF1plusF2Zero, GeneralRes.Attention);
                return false;
            }
            else
                return true;    
        }

        private void CleanResultTextboxes()  // метод очищающий поля результатов
        {
            if (actualResults)
            {
                tbMuBottom.Text = string.Empty;
                designOfEffectiveLengthOfColumnWithIncompleteCoupling.muBottom = -1;
                tbEffectLengthBottom.Text = string.Empty;
                designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthBottom = -1;  
                tbMuTop.Text = string.Empty;
                designOfEffectiveLengthOfColumnWithIncompleteCoupling.muTop = -1;
                tbEffectLengthTop.Text = string.Empty;
                designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthTop = -1;
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
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.ReadEffectiveLength();
            {
                framePropertiesOfColumnWithIncompleteCoupling = designOfEffectiveLengthOfColumnWithIncompleteCoupling.framePropertiesOfColumnWithIncompleteCoupling;
                if ((framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                    rbtnSwivelBearing.Checked = true;
                else
                    rbtnRigidBearing.Checked = true;
                if ((framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace) == 0)
                    rbtnCrossbraceOnBottom.Checked = true;
                else
                    rbtnCrossbraceOnTop.Checked = true;

                SetTextboxLastValue(tbHeightBottom, designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightBottom);
                SetTextboxLastValue(tbHeightFree, designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightFree);
                SetTextboxLastValue(tbMomInertBottom, designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertBottom);
                SetTextboxLastValue(tbForceBottom, designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceBottom);
                SetTextboxLastValue(tbHeightTop, designOfEffectiveLengthOfColumnWithIncompleteCoupling.heightTop);
                SetTextboxLastValue(tbMomInertTop, designOfEffectiveLengthOfColumnWithIncompleteCoupling.momInertTop);
                SetTextboxLastValue(tbForceTop, designOfEffectiveLengthOfColumnWithIncompleteCoupling.forceTop);
                SetTextboxLastValue(tbMuBottom, Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.muBottom, 3));
                SetTextboxLastValue(tbEffectLengthBottom, Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthBottom, 3));
                SetTextboxLastValue(tbMuTop, Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.muTop, 3));
                SetTextboxLastValue(tbEffectLengthTop, Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthTop, 3));

                if (tbMuBottom.Text == String.Empty && tbEffectLengthBottom.Text == String.Empty)
                    actualResults = false;
                else
                    actualResults = true;
            }
        }

        public void UploadLastData()  // метод, сохраняющий состояние юзерконтрола. Используется для последующей инициализации
        {
            designOfEffectiveLengthOfColumnWithIncompleteCoupling.WriteEffectiveLength();
        }


    // обработка событий контролов
        private void rbtnSwivelBearing_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();

            if (rbtnSwivelBearing.Checked)
                framePropertiesOfColumnWithIncompleteCoupling = framePropertiesOfColumnWithIncompleteCoupling & ~FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn;   // xxxx xxx0 | см. описание unum FrameProperties
            else
                framePropertiesOfColumnWithIncompleteCoupling = framePropertiesOfColumnWithIncompleteCoupling | FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn;    // xxxx xxx1 | см. описание unum FrameProperties
            DrawPictures();            
        }

        private void rbtnCrossbraceOnBottom_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnCrossbraceOnBottom.Checked)
            {
                framePropertiesOfColumnWithIncompleteCoupling = framePropertiesOfColumnWithIncompleteCoupling & ~FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace;   // xxxx xx0x | см. описание unum FrameProperties
                grbxParamOfTopPart.Visible = false;
                grbxResultsOfTopPart.Visible = false;

                tlpParamOfBottomPart.RowStyles[0].SizeType = SizeType.Percent;
                tlpParamOfBottomPart.RowStyles[0].Height = 50;  

                tlpParamOfBottomPart.RowStyles[1].SizeType = SizeType.Percent;
                tlpParamOfBottomPart.RowStyles[1].Height = 50;

                lblTxtHeightFree.Visible = true;
                lblIndHeightFree.Visible = true;
                tbHeightFree.Visible = true;
                lblUnitHeightFree.Visible = true;

                tlpParamOfBottomPart.RowStyles[2].SizeType = SizeType.AutoSize;
                lblTxtMomInertBottom.Visible = false;
                lblIndMomInertBottom.Visible = false;
                tbMomInertBottom.Visible = false;
                lblUnitMomInertBottom.Visible = false;

                tlpParamOfBottomPart.RowStyles[3].SizeType = SizeType.AutoSize;
                lblTxtForce.Visible = false;
                lblIndForceBottom.Visible = false;
                tbForceBottom.Visible = false;
                lblUnitForceBottom.Visible = false;

                grbxParamOfBottomPart.Text = GeneralRes.ParamOfColumn;
                lblTxtHeight.Text = GeneralRes.FullHeight;
                lblIndHeightBottom.Text = GeneralRes.h;
                grbxResultsOfBottomPart.Text = GeneralRes.Results;
                lblIndMuBottom.Text = GeneralRes.mu;
                lblTxtEffectLength.Text = GeneralRes.EffectLengthMuOnH;
                lblIndEffectLengthBottom.Text = GeneralRes.lef;

                tbHeightBottom.Text = string.Empty;
                tbMomInertBottom.Text = string.Empty;
                tbForceBottom.Text = string.Empty;
                tbHeightTop.Text = string.Empty;
                tbMomInertTop.Text = string.Empty;
                tbForceTop.Text = string.Empty;
            }
            else
            {
                framePropertiesOfColumnWithIncompleteCoupling = framePropertiesOfColumnWithIncompleteCoupling | FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace;    // xxxx xx1x | см. описание unum FrameProperties
                grbxParamOfTopPart.Visible = true;
                grbxResultsOfTopPart.Visible = true;

                tlpParamOfBottomPart.RowStyles[0].SizeType = SizeType.Percent;
                tlpParamOfBottomPart.RowStyles[0].Height = 34;

                tlpParamOfBottomPart.RowStyles[1].SizeType = SizeType.AutoSize;
                lblTxtHeightFree.Visible = false;
                lblIndHeightFree.Visible = false;
                tbHeightFree.Visible = false;
                lblUnitHeightFree.Visible = false;

                tlpParamOfBottomPart.RowStyles[2].SizeType = SizeType.Percent;
                tlpParamOfBottomPart.RowStyles[2].Height = 33;
                lblTxtMomInertBottom.Visible = true;
                lblIndMomInertBottom.Visible = true;
                tbMomInertBottom.Visible = true;
                lblUnitMomInertBottom.Visible = true;

                tlpParamOfBottomPart.RowStyles[3].SizeType = SizeType.Percent;
                tlpParamOfBottomPart.RowStyles[3].Height = 33;
                lblTxtForce.Visible = true;
                lblIndForceBottom.Visible = true;
                tbForceBottom.Visible = true;
                lblUnitForceBottom.Visible = true;

                grbxParamOfBottomPart.Text = GeneralRes.ParamOfBottomPart;
                lblTxtHeight.Text = GeneralRes.Height;
                lblIndHeightBottom.Text = GeneralRes.h1;
                grbxResultsOfBottomPart.Text = GeneralRes.ResultsOfBottomPart;
                lblIndMuBottom.Text = GeneralRes.mu1;
                lblTxtEffectLength.Text = GeneralRes.EffectLength;
                lblIndEffectLengthBottom.Text = GeneralRes.lef1;

                tbHeightBottom.Text = string.Empty;
                tbHeightFree.Text = string.Empty;               
            }
            DrawPictures();
        }

        private void btnDesign_Click(object sender, EventArgs e)
        {
            if (!actualResults)
            {
                // проверка текстбоксов на соответствие ограничениям и конвертация допустимых значений
                if (!DataValidation())
                    return;
                // передача параметров рамы
                designOfEffectiveLengthOfColumnWithIncompleteCoupling.framePropertiesOfColumnWithIncompleteCoupling = framePropertiesOfColumnWithIncompleteCoupling;

                // проверка расчетных параметров
                if ((framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace) != 0)
                {
                    string msgСheckoutOfParametrs;
                    if (!designOfEffectiveLengthOfColumnWithIncompleteCoupling.СheckoutOfParametrsOfDesign(out msgСheckoutOfParametrs))
                    {
                        MessageBox.Show(msgСheckoutOfParametrs, GeneralRes.Attention);
                        return;
                    }
                }

                // определение расчетной длины
                actualResults = designOfEffectiveLengthOfColumnWithIncompleteCoupling.CalculateEffectiveLength();
                if (actualResults == false)
                {
                    MessageBox.Show(GeneralRes.SolutionNotFound, GeneralRes.Attention);
                    return;
                }

                // вывод результатов
                tbMuBottom.Text = Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.muBottom, 3).ToString();
                tbEffectLengthBottom.Text = Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthBottom, 3).ToString();
                if ((framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace) != 0)
                {
                    tbMuTop.Text = Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.muTop, 3).ToString();
                    tbEffectLengthTop.Text = Math.Round(designOfEffectiveLengthOfColumnWithIncompleteCoupling.effectLengthTop, 3).ToString();
                }
            }
        }
    }
}
