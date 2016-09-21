using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace DesignOfEffectiveLenghtOfColumns
{
    class DesignOfEffectiveLenghtOfSteppedColumn : IMyDesignClass
    {
    // инициализация необходимых полей, их значений по умолчанию и свойств
        FramePropertiesOfSteppedColumn _framePropertiesSteppedColumn = 0x00;
        double _forceTop = 50;
        double _momInertTop = 30000;
        double _lengthTop = 4;
        double _forceBottom = 30;
        double _momInertBottom = 200000;
        double _lengthBottom = 8;
        double _muTop = -1;
        double _muBottom = -1;
        double _effectiveLenghtTop = -1;
        double _effectiveLenghtBottom = -1;
        double _n = 1;
        double _m = 1;
        double _ro = 1;
        double _alfa_1 = 1;

        public FramePropertiesOfSteppedColumn framePropertiesSteppedColumn
        {
            get { return _framePropertiesSteppedColumn; }
            set { _framePropertiesSteppedColumn = value; }
        }
        public double forceTop
        {
            get { return _forceTop; }
            set { _forceTop = value; }
        }
        public double momInertTop
        {
            get { return _momInertTop; }
            set { _momInertTop = value; }
        }
        public double lengthTop
        {
            get { return _lengthTop; }
            set { _lengthTop = value; }
        }
        public double forceBottom
        {
            get { return _forceBottom; }
            set { _forceBottom = value; }
        }
        public double momInertBottom
        {
            get { return _momInertBottom; }
            set { _momInertBottom = value; }
        }
        public double lengthBottom
        {
            get { return _lengthBottom; }
            set { _lengthBottom = value; }
        }
        public double muTop
        {
            get { return _muTop; }
            set { _muTop = value; }
        }
        public double effectiveLenghtTop
        {
            get { return _effectiveLenghtTop; }
            set { _effectiveLenghtTop = value; }
        }
        public double muBottom
        {
            get { return _muBottom; }
            set { _muBottom = value; }
        }
        public double effectiveLenghtBottom
        {
            get { return _effectiveLenghtBottom; }
            set { _effectiveLenghtBottom = value; }
        }
        
    // запись и чтение последних данных
        const string fileName = "LastDataSteppedColumn.dat";

        public void WriteEffectiveLength()    // Ф-ция сохраняющая состояние контролов и их значения
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(Convert.ToByte(_framePropertiesSteppedColumn));
                writer.Write(_forceTop);
                writer.Write(_momInertTop);
                writer.Write(_lengthTop);
                writer.Write(_forceBottom);
                writer.Write(_momInertBottom);
                writer.Write(_lengthBottom);
                writer.Write(_muTop);
                writer.Write(_muBottom);
                writer.Write(_effectiveLenghtTop);
                writer.Write(_effectiveLenghtBottom);                
            }
        }

        public void ReadEffectiveLength()  // Ф-ция восстанавливающая состояние контролов и их значения
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    _framePropertiesSteppedColumn = (FramePropertiesOfSteppedColumn)reader.ReadByte();
                    _forceTop = reader.ReadDouble();
                    _momInertTop = reader.ReadDouble();
                    _lengthTop = reader.ReadDouble();
                    _forceBottom = reader.ReadDouble();
                    _momInertBottom = reader.ReadDouble();
                    _lengthBottom = reader.ReadDouble();
                    _muTop = reader.ReadDouble();
                    _muBottom = reader.ReadDouble();
                    _effectiveLenghtTop = reader.ReadDouble();
                    _effectiveLenghtBottom = reader.ReadDouble();
                }                
            }            
        } 
        
        public void CalculateParametrsOfDesign()    // ф-я определения расчетных параметров
        {
            _n = (_momInertTop * _lengthBottom) / (_lengthTop * _momInertBottom);
            _m = _lengthTop / _lengthBottom;
            _ro = _forceTop / (_forceBottom + _forceTop);   // по пособию к СНиП примечанию к табл.28 ro=1/betta, betta=(F1+F2)/F2, как видно при вычислении Betta может произойти деление на ноль, но так как betta в чистом виде нигде не фигурирует, а везде используется 1/betta, вместе с Юрием Давидовичем решили отказаться от нее и сразу вычислять ro.
            _alfa_1 = Math.Sqrt(_m * _ro / _n);
        }

        public bool СheckoutOfParametrsOfDesign(out string message)    // ф-я определения расчетных параметров
        {
            message = string.Empty;
            bool result = true;

            // проверка значения n в зависимости от alfa_1 по посл. столбцу табл. 28 пособ. к СНиП II-23-81*
            if ((_framePropertiesSteppedColumn & FramePropertiesOfSteppedColumn.NumberOfSpans) == 0)  // для однопролетной рамы
            {
                if (_alfa_1 != 0)  // т.к. при alfa1=0 происходит деление на ноль, в результате получается что n<=бесконечности, что всегда верно
                {
                    if ((_alfa_1 <= 1 && _n > 2.5 / (_alfa_1 * Math.Sqrt(_alfa_1)) - 1.5) ||
                        (_alfa_1 > 1 && _n > 1.35 - 0.35 * _alfa_1))
                    {
                        message = message + GeneralRes.RatioOfLinearRigidityExceedLimit + "\n";
                        result = false;
                    }
                }
            }

            // проверка отношения I2/I1 по посл. столбцу табл. 28 пособ. к СНиП II-23-81*
            if (_momInertTop / _momInertBottom < 0.04)
            {
                message = message + GeneralRes.RatioOfRigidityExceedBottomLimit + "\n";
                result = false;
            }
            else if (_momInertTop / _momInertBottom > 1)
            {
                message = message + GeneralRes.RatioOfRigidityExceedTopLimit + "\n";
                result = false;
            }

            // проверка отношения l2/l1 по посл. столбцу табл. 28 пособ. к СНиП II-23-81*
            if (_lengthTop / _lengthBottom < 0.1)
            {
                message = message + GeneralRes.RatioOfLengthExceedBottomLimit + "\n";
                result = false;
            }
            else if (_lengthTop / _lengthBottom > 2)
            {
                message = message + GeneralRes.RatioOfLengthExceedTopLimit + "\n";
                result = false;
            }  
            return result;
        }

        private void CalculateMuBottom()  // по табл. 28 пособ. к СНиП II-23-81*
        {
            switch (_framePropertiesSteppedColumn)
            {
                case FramePropertiesOfSteppedColumn.OneSpan_SwivelBearingOfRafter:
                    _muBottom = designMuBottomOneSpanSwivelBearing();
                    break;
                case FramePropertiesOfSteppedColumn.MultipleSpans_SwivelBearingOfRafter:
                    _muBottom = designMuBottomMultipleSpansSwivelBearing();
                    break;
                case FramePropertiesOfSteppedColumn.OneSpan_RigidBearingOfRafter:
                    _muBottom = designMuBottomOneSpanRigidBearingOfRafte();
                    break;
                case FramePropertiesOfSteppedColumn.MultipleSpans_RigidBearingOfRafter:
                    _muBottom = designMuBottomMultipleSpansRigidBearing();
                    break;
            }        
        }

        private double designMuBottomOneSpanSwivelBearing()  // по п.1 табл. 28 пособ. к СНиП II-23-81*
        {
            double _mu;
            _mu = Math.Sqrt((_n * _alfa_1*_alfa_1 * (66 - _alfa_1*_alfa_1) * (_alfa_1*_alfa_1 * (2.2*_n + 1) + 2) +
                (16.6 - 0.5*_alfa_1) * (1.3*_alfa_1*_alfa_1*_alfa_1*_alfa_1 + 2)) / 
                (0.18 * _n * _alfa_1*_alfa_1 * (66 - _alfa_1*_alfa_1) + 3.6 * (2.3 + _alfa_1*_alfa_1)));
            return _mu;
        }

        private double designMuBottomOneSpanRigidBearingOfRafte()  // по п.2 табл. 28 пособ. к СНиП II-23-81*
        {
            double _mu = 0;
            if (_alfa_1 <= 1.2)
                _mu = Math.Sqrt(((0.76 - 0.13*_alfa_1) * (_alfa_1*_alfa_1 * (5.3*_n + 1) + (5.3 + _n))) / (_n + 1));
            else
                _mu = Math.Sqrt(((0.56 + 0.039*_alfa_1) * (_alfa_1*_alfa_1 * (5.3*_n + 1) + (5.3 + _n))) / (_n + 1));            
            return _mu;
        }

        private double designMuBottomMultipleSpansSwivelBearing()  // по п.3 табл. 28 пособ. к СНиП II-23-81*
        {
            double _mu;
            _mu = Math.Sqrt((_m * (_n + 1.5) * (_ro + _m) + 0.22*_n + (_m*_m + 1.1*_n*(_m - _ro)) * _m*_ro/Math.Sqrt(_n)) / 
                (0.4 * ((_n + _m*_m) + 3*_n*_m*(_m + 1))));
            return _mu;
        }

        private double designMuBottomMultipleSpansRigidBearing()  // по п.4 табл. 28 пособ. к СНиП II-23-81*
        {
            double _mu;
            _mu = Math.Sqrt((_m*_m * (3.5 - _ro*_ro) + (_m*_m + 0.4*_m + 1) * (0.26 * (_n + 6) * (_n + 0.15) + 
                _m * _ro * (1 / Math.Sqrt(_n) + 1))) / (_m*_m + _n*_n + 4*_n * (_m*_m + 1.5*_m + 1)));
            return _mu;
        }

        private void CalculateMuTop()    // по п.И.2 СП 63.13330.2011, ф-ла И.5
        {
            if (_alfa_1 == 0)     // т.к. при alfa1=0 происходит деление на ноль, в результате получается что muTop="бесконечность", что больше трех, следовательно muTop=3
                _muTop = 3;
            else
            {
                _muTop = _muBottom / _alfa_1;
                if (_muTop > 3)
                    _muTop = 3;
            }
        }

        public bool CalculateEffectiveLength()
        {
            CalculateMuBottom();
            CalculateMuTop();
            _effectiveLenghtBottom = _lengthBottom * _muBottom;
            _effectiveLenghtTop = _lengthTop * _muTop;
            return true;
        }
    }
}
