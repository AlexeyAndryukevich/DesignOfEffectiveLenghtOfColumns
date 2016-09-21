using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace DesignOfEffectiveLenghtOfColumns
{
    class DesignOfEffectiveLenghtOfSimpleColumn : IMyDesignClass
    {  
    // инициализация необходимых полей, их значений по умолчанию и свойств
        FramePropertiesOfSimpleColumn _framePropertiesSimpleColumn = 0x00;
        private bool _momInertValues = true;
        private bool _duplicateLeftToRight = true;
        private double _height = 9; 
        private double _spanLeft = 24; 
        private double _momInertOfColumn = 30000;
        private double _momInertOfGirderLeft = 200000;
        private double _momInertOfGirderLeftBottom = -1;
        private double _momInertRatioLeft = -1;
        private double _momInertRatioLeftBottom = -1;
        private int _numberOfSpans = -1;
        private double _spanRight = -1;
        private double _momInertOfGirderRight = -1;
        private double _momInertOfGirderRightBottom = -1;
        private double _momInertRatioRight = -1;
        private double _momInertRatioRightBottom = -1;
        private double _rigidRatioLeft = 1;
        private double _rigidRatioRight = 1;
        private double _rigidRatioLeftBottom = 1;
        private double _rigidRatioRightBottom = 1;
        private double _mu = -1;
        private double _effectiveLenght = -1;

        public FramePropertiesOfSimpleColumn framePropertiesSimpleColumn
        {
            get { return _framePropertiesSimpleColumn; }
            set { _framePropertiesSimpleColumn = value; }
        }
        public bool momInertValues
        {
            get { return _momInertValues; }
            set { _momInertValues = value; }
        }
        public bool duplicateLeftToRight
        {
            get { return _duplicateLeftToRight; }
            set { _duplicateLeftToRight = value; }
        }
        public double height
        {
            get { return _height; }
            set { _height = value; }
        }
        public double spanLeft
        {
            get { return _spanLeft; }
            set { _spanLeft = value; }
        }
        public double momInertOfColumn
        {
            get { return _momInertOfColumn; }
            set { _momInertOfColumn = value; }
        }
        public double momInertOfGirderLeft
        {
            get { return _momInertOfGirderLeft; }
            set { _momInertOfGirderLeft = value; }
        }
        public double momInertOfGirderLeftBottom
        {
            get { return _momInertOfGirderLeftBottom; }
            set { _momInertOfGirderLeftBottom = value; }
        }
        public double momInertRatioLeft
        {
            get { return _momInertRatioLeft; }
            set { _momInertRatioLeft = value; }
        }
        public double momInertRatioLeftBottom
        {
            get { return _momInertRatioLeftBottom; }
            set { _momInertRatioLeftBottom = value; }
        }
        public int numberOfSpans
        {
            get { return _numberOfSpans; }
            set { _numberOfSpans = value; }
        }
        public double spanRight
        {
            get { return _spanRight; }
            set { _spanRight = value; }
        }
        public double momInertOfGirderRight
        {
            get { return _momInertOfGirderRight; }
            set { _momInertOfGirderRight = value; }
        }
        public double momInertOfGirderRightBottom
        {
            get { return _momInertOfGirderRightBottom; }
            set { _momInertOfGirderRightBottom = value; }
        }
        public double momInertRatioRight
        {
            get { return _momInertRatioRight; }
            set { _momInertRatioRight = value; }
        }
        public double momInertRatioRightBottom
        {
            get { return _momInertRatioRightBottom; }
            set { _momInertRatioRightBottom = value; }
        }
        public double mu
        {
            get { return _mu; }
            set { _mu = value; }
        }
        public double effectiveLenght
        {
            get { return _effectiveLenght; }
            set { _effectiveLenght = value; }
        }

        // поле и методы для сохранения/восстановления последнего сочтояния юзерконтрола
        const string fileName = "LastDataSimpleColumn.dat";
        public void WriteEffectiveLength()    // метод, сохраняющий состояние контролов и их значения
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(Convert.ToByte(_framePropertiesSimpleColumn));
                writer.Write(_momInertValues);
                writer.Write(_duplicateLeftToRight);
                writer.Write(_height);
                writer.Write(_spanLeft);
                writer.Write(_momInertOfColumn);
                writer.Write(_momInertOfGirderLeft);
                writer.Write(_momInertOfGirderLeftBottom);
                writer.Write(_momInertRatioLeft);
                writer.Write(_momInertRatioLeftBottom);
                writer.Write(_numberOfSpans);
                writer.Write(_spanRight);
                writer.Write(_momInertOfGirderRight);
                writer.Write(_momInertOfGirderRightBottom);
                writer.Write(_momInertRatioRight);
                writer.Write(_momInertRatioRightBottom);
                writer.Write(_mu);
                writer.Write(_effectiveLenght);
            }
        }

        public void ReadEffectiveLength()  // метод, восстанавливающий состояние контролов и их значения
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    _framePropertiesSimpleColumn = (FramePropertiesOfSimpleColumn)reader.ReadByte();
                    _momInertValues = reader.ReadBoolean();
                    _duplicateLeftToRight = reader.ReadBoolean();
                    _height = reader.ReadDouble();
                    _spanLeft = reader.ReadDouble();
                    _momInertOfColumn = reader.ReadDouble();
                    _momInertOfGirderLeft = reader.ReadDouble();
                    _momInertOfGirderLeftBottom = reader.ReadDouble();
                    _momInertRatioLeft = reader.ReadDouble();
                    _momInertRatioLeftBottom = reader.ReadDouble();
                    _numberOfSpans = reader.ReadInt32();
                    _spanRight = reader.ReadDouble();
                    _momInertOfGirderRight = reader.ReadDouble();
                    _momInertOfGirderRightBottom = reader.ReadDouble();
                    _momInertRatioRight = reader.ReadDouble();
                    _momInertRatioRightBottom = reader.ReadDouble();
                    _mu = reader.ReadDouble();
                    _effectiveLenght = reader.ReadDouble();
                }               
            }           
        } 

        // вспомагательные расчетные функции
        private void MomInertRatio()    // Ф-я, приводящая исходные значения моментов инерции к используемым в расчете
        {
            if (_momInertValues == true)
            {
                _rigidRatioLeft = _momInertOfColumn / _momInertOfGirderLeft;
                _rigidRatioRight = _momInertOfColumn / _momInertOfGirderRight;
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                {
                    _rigidRatioLeftBottom = _momInertOfColumn / _momInertOfGirderLeftBottom;
                    _rigidRatioRightBottom = _momInertOfColumn / _momInertOfGirderRightBottom;
                }
            }
            else
            {
                _rigidRatioLeft = _momInertRatioLeft;
                _rigidRatioRight = _momInertRatioRight;
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) != 0)
                {
                    _rigidRatioLeftBottom = _momInertRatioLeftBottom;
                    _rigidRatioRightBottom = _momInertRatioRightBottom;
                }
            }
        }

        // Ф-ции определения параметров "n" (для верхнего ригеля) и "p" (для нижнего), необходимых для нахождения "mu"
        // СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.8
        // для крайних колонн:
        private double Param_N_Or_P_Exterme(double height, double span, double rigidRatio)  
        {
            return height / (span * rigidRatio);
        }

        // для средних колонн:
        private double Param_N_Or_P_Middle(int numberOfSpans, double paramExterme1, double paramExterme2)
        {
            return numberOfSpans * (paramExterme1 + paramExterme2) / (numberOfSpans + 1);
        }

        // Ф-ция определения параметра "mu" для свободных многоэтажных рам
        // СП16.13330.2011 ф-лы 143, 144 (табл.31) | СНиП II-23-81* ф-лы 70.а, 70.б (табл.17,а) | ДБН В.2.6-198:2014 ф-лы 13.5, 13.6 (табл.13.8)
        private double CoefMuMultiplestoreyUnfree(double p, double n)
        {
            if (n <= 0.2)
                return (p + 0.68) * Math.Sqrt(n + 0.22) / Math.Sqrt(0.68 * p * (p + 0.9) * (n + 0.08) + 0.1 * n);
            else
                return (p + 0.63) * Math.Sqrt(n + 0.28) / Math.Sqrt(p * n * (p + 0.9) + 0.1 * n);
        }

    // основные расчетные функции
        // определение случая расчета коэффициента "мю"
        // СП16.13330.2011 п.10.3.4, табл.31 | СНиП II-23-81* п.6.10*, табл.17,а | ДБН В.2.6-198:2014 п.13.3.4, табл.13.8
        private void CalculateMu()
        {
            MomInertRatio();

            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.FrameType) == 0 && (_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.NumberOfStoreys) == 0)
            {
                FreeFrameOneStorey();
            }
            else if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.FrameType) == 0 && (_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.NumberOfStoreys) != 0)
            {
                FreeFrameMultipleStoreys();
            }
            else if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.FrameType) != 0)
            {
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.NumberOfStoreys) == 0)   // решили с Юрием Давидовичем реализовать данный случай, и считать его как для нижнего этажа многоэтажной рамы
                    _framePropertiesSimpleColumn = _framePropertiesSimpleColumn | FramePropertiesOfSimpleColumn.NumberOfStoreys;   // xxxx xx1x                                
               
                UnfreeFrameMultipleStoreys();
            }
        }

        private void FreeFrameOneStorey()    // определение коэфф. "мю" для одноэтажных свободных рам
        {
            // определение "n"
            double n = 0;             
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) == 0)   // для крайней колонны
            {
                n = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeft);   // по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.3
            }
            else    // для средней колонны
            {
                double n1;
                double n2;
                n1 = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeft);
                n2 = Param_N_Or_P_Exterme(_height, _spanRight, _rigidRatioRight);
                n = Param_N_Or_P_Middle(_numberOfSpans, n1, n2);   // по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.3
            }

            // определение "mu"
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.BearingType) == 0)   // для шарнирного опирания
            {
                mu = 2 * Math.Sqrt(1 + (0.38 / n));    // по СП16.13330.2011 ф-ла 141 (табл.31) | СНиП II-23-81* ф-ла 68 (табл.17,а) | ДБН В.2.6-198:2014 ф-ла 13.3 (табл.13.3)
            }
            else    // для жесткого опирания
            {
                mu = Math.Sqrt((n + 0.56) / (n + 0.14));    // по СП16.13330.2011 ф-ла 142 (табл.31) | СНиП II-23-81* ф-ла 69 (табл.17,а) | ДБН В.2.6-198:2014 ф-ла 13.4 (табл.13.4)
            }            
        }

        private void FreeFrameMultipleStoreys()   // определение коэфф. "мю" для многоэтажных свободных рам
        {
            int _numberOfSpansForDesign = 0;  //"расчетное значение" количества пролетов введено так как все (видимые и невидимые) поля формы передаются в этот класс, для невидимых применяется флаг - значение "-1", но в данном расчетном случае для крайней колонны невидиму параметру необходимо присвоить значение 1, это привело бы к перетиранию флага, по которому идет восстановление последних данных на форме
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)   // средняя колонна
                _numberOfSpansForDesign = _numberOfSpans;
            else    // крайняя колонна
                _numberOfSpansForDesign = 1;   // как для однопролетной рамы, см. примечание к табл. норматива 
            // определение "p"
            double p = 0; 
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey1stBit) == 0 
                && (_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) == 0)  // для нижнего этажа
            {
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.BearingType) == 0)   // шарнирное опирание                
                    p = 0;     // значения при шарнирном опирании колонн на фундаменты по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.8
                else                
                    p = 1000000;   // принятое с Юрием Давидовичем значение бесконечности при жестком опирании колонн на фундаменты по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.8
            }
            else   // для среднего и верхнего этажей
            {
                double p1 = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeftBottom);
                double p2 = 0;
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)   // средняя колонна                
                    p2 = Param_N_Or_P_Exterme(_height, _spanRight, _rigidRatioRightBottom);
                p = Param_N_Or_P_Middle(_numberOfSpansForDesign, p1, p2);   // по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.3
            }

            // определение "n"
            double n = 0;
            double n1 = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeft);
            double n2 = 0;
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)   // крайняя колонна   
                n2 = Param_N_Or_P_Exterme(_height, _spanRight, _rigidRatioRight);
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey1stBit) == 0)   // для нижнего и среднего этажей многоэтажной свободной рамы
                n = Param_N_Or_P_Middle(_numberOfSpansForDesign, n1, n2);       // по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.3     
            else   // для верхнего этажа многоэтажной свободной рамы            
                n = 2 * Param_N_Or_P_Middle(_numberOfSpansForDesign, n1, n2);   // по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.3

            // определение "mu"
            mu = CoefMuMultiplestoreyUnfree(p, n);
        }

        private void UnfreeFrameMultipleStoreys()  // определение коэфф. "мю" для многоэтажных несвободных рам (одноэтажные считаются как колонны нижнего этажа многоэтажных рам)
        {
            // определение "p"
            double p = 0;
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey1stBit) == 0 
                && (_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey2ndBit) == 0)  // для нижнего этажа
            {
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.BearingType) == 0)  // шарнирное опирание                
                    p = 0;   // значения при шарнирном опирании колонн на фундаменты по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.8
                else
                    p = 1000000;   // принятое с Юрием Давидовичем значение бесконечности при жестком опирании колонн на фундаменты по СП16.13330.2011 табл.31 | СНиП II-23-81* табл.17,а | ДБН В.2.6-198:2014 табл.13.8
            }
            else  // для среднего и верхнего этажей многоэтажной несвободной рамы
            {
                double p1 = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeftBottom);
                double p2 = 0;
                if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)  // средняя колонна             
                    p2 = Param_N_Or_P_Exterme(_height, _spanRight, _rigidRatioRightBottom);
                p = 0.5 * (p1 + p2);    // по СП16.13330.2011 табл.31 | СНиП II-23-81* п.6.10*, табл.17,а | ДБН В.2.6-198:2014 табл.13.3
            }

            // определение "n"
            double n = 0;
            double n1 = Param_N_Or_P_Exterme(_height, _spanLeft, _rigidRatioLeft);
            double n2 = 0;
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedColumn) != 0)  // средняя колонна            
                n2 = Param_N_Or_P_Exterme(_height, _spanRight, _rigidRatioRight);            
            if ((_framePropertiesSimpleColumn & FramePropertiesOfSimpleColumn.ConcernedStorey1stBit) == 0)  // для нижнего и среднего этажей многоэтажной несвободной рамы           
                n = 0.5 * (n1 + n2);   // по СП16.13330.2011 табл.31 | СНиП II-23-81* п.6.10*, табл.17,а | ДБН В.2.6-198:2014 табл.13.3         
            else  // для верхнего этажа многоэтажной несвободной рамы            
                n = n1 + n2;           // по СП16.13330.2011 табл.31 | СНиП II-23-81* п.6.10*, табл.17,а | ДБН В.2.6-198:2014 табл.13.3
            
            // определение "mu"
            mu = Math.Sqrt((1 + 0.46 * (p + n) + 0.18 * p * n) / (1 + 0.93 * (p + n) + 0.71 * p * n));   // по СП16.13330.2011 ф-ла 145 (табл.31) | СНиП II-23-81* (табл.17,а) | ДБН В.2.6-198:2014 ф-ла 13.7 (табл.13.8)
        }

        public bool CalculateEffectiveLength()
        {
            CalculateMu();
            _effectiveLenght = _mu * _height;
            return true;
        }
    }
}
