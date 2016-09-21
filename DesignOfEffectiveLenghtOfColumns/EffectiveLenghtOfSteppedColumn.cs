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
    enum FramePropertiesOfSteppedColumn : byte    // 8 бит, первые два из которых отвечают за тип рамы (состояние радио кнопок), что определяет случай расчета
    {
        NumberOfSpans = 0x01,              // 0000 0001 | 01 | Кол-во пролетов: 0 - один, 1 - два и более
        BearingOfRafter = 0x02,            // 0000 0010 | 02 | Опирание стропильных конструкций: 0 - шарнирное, 1 - жесткое

        // Возможные комбинации:
        OneSpan_SwivelBearingOfRafter = 0x00,               // 0000 0000 | 00
        MultipleSpans_SwivelBearingOfRafter = 0x01,         // 0000 0001 | 01
        OneSpan_RigidBearingOfRafter = 0x02,                // 0000 0010 | 02
        MultipleSpans_RigidBearingOfRafter = 0x03,          // 0000 0011 | 03
    }


    partial class EffectiveLenghtOfSteppedColumn : UserControl, IMyUserControl
    {
    // инициализация необходимых переменных и обьектов
        DesignOfEffectiveLenghtOfSteppedColumn designOfEffectiveLenghtOfSteppedColumn = new DesignOfEffectiveLenghtOfSteppedColumn();
        FramePropertiesOfSteppedColumn framePropertiesOfSteppedColumn = 0x00;     // 0000 0000 | 00 
        bool actualResults = false;

    // конструктор юзер контрола
        public EffectiveLenghtOfSteppedColumn()
        {
            InitializeComponent();
            InitializeOfControlsProperties();
        }  
      
    // свои вспомагательные методы
        private void InitializeOfControlsProperties()   
        {
            // заполнение текста контролов из файла реурсов
            grbxNumberOfSpans.Text = GeneralRes.NumberOfSpans;
            rbtnOneSpan.Text = GeneralRes.oneSpan;
            rbtnMultipleSpans.Text = GeneralRes.multipleSpans;            
            grbxBearingOfRafter.Text = GeneralRes.BearingOfRafter;
            rbtnSwivelBearing.Text = GeneralRes.swivelBearing;
            rbtnRigidBearing.Text = GeneralRes.rigidBearing;            
            grbxParamOfTopPart.Text = GeneralRes.ParamOfTopPart;
            lblIndForceTop.Text = GeneralRes.F2;
            lblUnitForceTop.Text = GeneralRes.tf;             
            lblIndMomInertTop.Text = GeneralRes.I2;
            lblUnitMomInertTop.Text = GeneralRes.cm4;              
            lblIndLengthTop.Text = GeneralRes.l2;
            lblUnitLengthTop.Text = GeneralRes.m;
            grbxParamOfBottomPart.Text = GeneralRes.ParamOfBottomPart;            
            lblIndForceBottom.Text = GeneralRes.F1;
            lblUnitForceBottom.Text = GeneralRes.tf;            
            lblIndMomInertBottom.Text = GeneralRes.I1;
            lblUnitMomInertBottom.Text = GeneralRes.cm4;             
            lblIndLengthBottom.Text = GeneralRes.l1;
            lblUnitLengthBottom.Text = GeneralRes.m;           
            btnDesign.Text = GeneralRes.Design;            
            grbxResults.Text = GeneralRes.Results;
            lblIndMuTop.Text = GeneralRes.mu2;
            lblIndEffectLengthTop.Text = GeneralRes.lef2;
            lblUnitEffectLengthTop.Text = GeneralRes.m;
            lblIndMuBottom.Text = GeneralRes.mu1;
            lblIndEffectLengthBottom.Text = GeneralRes.lef1;
            lblUnitEffectLengthBottom.Text = GeneralRes.m;
        }

        private void DrawPictures()  // Ф-я отвечающая за смену изображений из файла ресурсов в зависимости от текущих параметров рамы
        {
            switch (framePropertiesOfSteppedColumn)
            {
                case FramePropertiesOfSteppedColumn.OneSpan_SwivelBearingOfRafter:
                    picbxFrame.Image = GeneralRes.Frame_OneSpan_SwivelBearingOfRafter;
                    picbxColumn.Image = GeneralRes.Column_OneSpan_SwivelBearingOfRafter;
                    break;
                case FramePropertiesOfSteppedColumn.MultipleSpans_SwivelBearingOfRafter:
                    picbxFrame.Image = GeneralRes.Frame_MultipleSpans_SwivelBearingOfRafter;
                    picbxColumn.Image = GeneralRes.Column_MultipleSpans_SwivelBearingOfRafter;
                    break;
                case FramePropertiesOfSteppedColumn.OneSpan_RigidBearingOfRafter:
                    picbxFrame.Image = GeneralRes.Frame_OneSpan_RigidBearingOfRafter;
                    picbxColumn.Image = GeneralRes.Column_OneSpan_RigidBearingOfRafter;
                    break;
                case FramePropertiesOfSteppedColumn.MultipleSpans_RigidBearingOfRafter:
                    picbxFrame.Image = GeneralRes.Frame_MultipleSpans_RigidBearingOfRafter;
                    picbxColumn.Image = GeneralRes.Column_MultipleSpans_RigidBearingOfRafter;
                    break;
            }
        }    

        private void ValidationOfInput(object sender, EventArgs e)   // обработка ввода данных в текстбоксы
        {
            CleanResultTextboxes();
            TextBox textBox = sender as TextBox;
            double a;
            if ((double.TryParse(textBox.Text, out a) && a > 0) || textBox.Text == string.Empty)
                textBox.BackColor = Color.White;
            else
                textBox.BackColor = Color.FromArgb(255, 128, 128);

            if (textBox.Name == tbForceTop.Name || textBox.Name == tbForceBottom.Name)
            {
                if ((double.TryParse(textBox.Text, out a) && a == 0) || textBox.Text == string.Empty)
                    textBox.BackColor = Color.White;

                double b;
                TextBox currentForceTB = tbForceTop;
                TextBox otherForceTB = tbForceBottom;

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
        }

        private void ValidationOfInputOfForceValue(object sender, EventArgs e)
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

        private bool DataValidation()  // метод проверяющий наличие пустых текстбоксов, в случае наличия таковых - выдает ошибку со списком пустых полей
        {
            int quantityOfInputAbsent = 0;
            string msgInputAbsent = string.Empty;

            int quantityOfValuesNotNumbers = 0;
            string msgValuesNotNumbers = string.Empty;
           
            int quantityOfValuesInvalid = 0;
            string msgValuesInvalid = string.Empty;

            bool F1plusF2Zero = false;
            string msgInputF1plusF2Zero = string.Empty;

            TextBox[] arrTextBox = {
                                    tbForceTop,
                                    tbMomInertTop,
                                    tbLengthTop,
                                    tbForceBottom,
                                    tbMomInertBottom,
                                    tbLengthBottom,                                    
                                   };
            // массив обозначений полей для текста ошибки
            string[] arrNameForErr = {
                                      GeneralRes.F2,
                                      GeneralRes.I2,
                                      GeneralRes.l2,
                                      GeneralRes.F1,
                                      GeneralRes.I1,
                                      GeneralRes.l1,                                      
                                     };           

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
                                  designOfEffectiveLenghtOfSteppedColumn.forceTop,
                                  designOfEffectiveLenghtOfSteppedColumn.momInertTop,
                                  designOfEffectiveLenghtOfSteppedColumn.lengthTop,
                                  designOfEffectiveLenghtOfSteppedColumn.forceBottom,
                                  designOfEffectiveLenghtOfSteppedColumn.momInertBottom,
                                  designOfEffectiveLenghtOfSteppedColumn.lengthBottom,
                                 };
            // цикл проверки на отрицательность и равенство нулю значений для видимых и доступных текстбоксов, с последующей конвертацией отличных от нуля положительных значений
            for (int i = 0; i < arrValues.Length; i++)
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
                        if (i != 0 && i != 3)
                        {
                            if (arrValues[i] <= 0)
                            {
                                msgValuesInvalid = msgValuesInvalid + " " + arrNameForErr[i] + ",";
                                quantityOfValuesInvalid++;
                            }
                        }
                        else
                        {
                            if (i == 3 && arrValues[0] == 0 && arrValues[3] == 0)
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

            // присваивание полученных значений полям объекта класса расчета
            designOfEffectiveLenghtOfSteppedColumn.forceTop = arrValues[0];
            designOfEffectiveLenghtOfSteppedColumn.momInertTop = arrValues[1];
            designOfEffectiveLenghtOfSteppedColumn.lengthTop = arrValues[2];
            designOfEffectiveLenghtOfSteppedColumn.forceBottom = arrValues[3];
            designOfEffectiveLenghtOfSteppedColumn.momInertBottom = arrValues[4];
            designOfEffectiveLenghtOfSteppedColumn.lengthBottom = arrValues[5];

            if (quantityOfInputAbsent > 0 || quantityOfValuesNotNumbers > 0 || quantityOfValuesInvalid > 0 || F1plusF2Zero == true)
            {
                MessageBox.Show(msgInputAbsent + msgValuesNotNumbers + msgValuesInvalid + msgInputF1plusF2Zero, GeneralRes.Attention);
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
                designOfEffectiveLenghtOfSteppedColumn.muBottom = -1;
                tbEffectLengthBottom.Text = string.Empty;
                designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtBottom = -1;
                tbMuTop.Text = string.Empty;
                designOfEffectiveLenghtOfSteppedColumn.muTop = -1;
                tbEffectLengthTop.Text = string.Empty;
                designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtTop = -1;
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

        public void SetLastControlState()
        {
            designOfEffectiveLenghtOfSteppedColumn.ReadEffectiveLength();
            {
                framePropertiesOfSteppedColumn = designOfEffectiveLenghtOfSteppedColumn.framePropertiesSteppedColumn;
                if ((framePropertiesOfSteppedColumn & FramePropertiesOfSteppedColumn.NumberOfSpans) == 0)
                    rbtnOneSpan.Checked = true;
                else
                    rbtnMultipleSpans.Checked = true;
                if ((framePropertiesOfSteppedColumn & FramePropertiesOfSteppedColumn.BearingOfRafter) == 0)
                    rbtnSwivelBearing.Checked = true;
                else
                    rbtnRigidBearing.Checked = true;

                SetTextboxLastValue(tbForceTop, designOfEffectiveLenghtOfSteppedColumn.forceTop);
                SetTextboxLastValue(tbMomInertTop, designOfEffectiveLenghtOfSteppedColumn.momInertTop);
                SetTextboxLastValue(tbLengthTop, designOfEffectiveLenghtOfSteppedColumn.lengthTop);
                SetTextboxLastValue(tbForceBottom, designOfEffectiveLenghtOfSteppedColumn.forceBottom);
                SetTextboxLastValue(tbMomInertBottom, designOfEffectiveLenghtOfSteppedColumn.momInertBottom);
                SetTextboxLastValue(tbLengthBottom, designOfEffectiveLenghtOfSteppedColumn.lengthBottom);
                SetTextboxLastValue(tbMuTop, Math.Round(designOfEffectiveLenghtOfSteppedColumn.muTop, 3));
                SetTextboxLastValue(tbEffectLengthTop, Math.Round(designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtTop, 3));
                SetTextboxLastValue(tbMuBottom, Math.Round(designOfEffectiveLenghtOfSteppedColumn.muBottom, 3));
                SetTextboxLastValue(tbEffectLengthBottom, Math.Round(designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtBottom, 3));

                if (tbMuBottom.Text == String.Empty && tbEffectLengthBottom.Text == String.Empty)
                    actualResults = false;
                else
                    actualResults = true;
            }                        
        }

        public void UploadLastData()
        {
            designOfEffectiveLenghtOfSteppedColumn.WriteEffectiveLength();
        }

    // Обработка событий
        // обработка изменения состояния радиокнопок
        private void rbtnOneSpan_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnOneSpan.Checked)
                framePropertiesOfSteppedColumn = framePropertiesOfSteppedColumn & ~FramePropertiesOfSteppedColumn.NumberOfSpans;   // xxxx xxx0 | см. описание unum FrameProperties
            else
                framePropertiesOfSteppedColumn = framePropertiesOfSteppedColumn | FramePropertiesOfSteppedColumn.NumberOfSpans;    // xxxx xxx1 | см. описание unum FrameProperties
            DrawPictures();                
        }

        private void rbtnSwivelBearing_CheckedChanged(object sender, EventArgs e)
        {
            CleanResultTextboxes();
            if (rbtnSwivelBearing.Checked)
                framePropertiesOfSteppedColumn = framePropertiesOfSteppedColumn & ~FramePropertiesOfSteppedColumn.BearingOfRafter;   // xxxx xx0x | см. описание unum FrameProperties
            else
                framePropertiesOfSteppedColumn = framePropertiesOfSteppedColumn | FramePropertiesOfSteppedColumn.BearingOfRafter;    // xxxx xx1x | см. описание unum FrameProperties
            DrawPictures(); 
        }    
        
        // обработка нажатия кнопки "Расчет"
        private void btnDesign_Click(object sender, EventArgs e)
        {
            if (!actualResults)
            {
                // проверка текстбоксов на соответствие ограничениям и конвертация допустимых значений
                if (!DataValidation())
                    return;
                // передача параметров рамы
                designOfEffectiveLenghtOfSteppedColumn.framePropertiesSteppedColumn = framePropertiesOfSteppedColumn;

                // определение расчетных параметров
                designOfEffectiveLenghtOfSteppedColumn.CalculateParametrsOfDesign();
                // проверка расчетных параметров
                string msgСheckoutOfParametrs;
                if (!designOfEffectiveLenghtOfSteppedColumn.СheckoutOfParametrsOfDesign(out msgСheckoutOfParametrs))
                {
                    MessageBox.Show(msgСheckoutOfParametrs, GeneralRes.Attention);
                    return;
                }

                // вычисление расчетной длины
                actualResults = designOfEffectiveLenghtOfSteppedColumn.CalculateEffectiveLength();

                // вывод результатов
                tbMuTop.Text = Math.Round(designOfEffectiveLenghtOfSteppedColumn.muTop, 3).ToString();
                tbEffectLengthTop.Text = Math.Round(designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtTop, 3).ToString();
                tbMuBottom.Text = Math.Round(designOfEffectiveLenghtOfSteppedColumn.muBottom, 3).ToString();
                tbEffectLengthBottom.Text = Math.Round(designOfEffectiveLenghtOfSteppedColumn.effectiveLenghtBottom, 3).ToString();
            }
        }
    }
}

