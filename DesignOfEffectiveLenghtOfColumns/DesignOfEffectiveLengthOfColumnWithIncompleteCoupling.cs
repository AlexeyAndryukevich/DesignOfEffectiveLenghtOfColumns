using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace DesignOfEffectiveLenghtOfColumns
{
    class DesignOfEffectiveLengthOfColumnWithIncompleteCoupling : IMyDesignClass
    {     
     // инициализация необходимых полей, их значений по умолчанию и свойств
        FramePropertiesOfColumnWithIncompleteCoupling _framePropertiesOfColumnWithIncompleteCoupling = 0x00;
        double _heightBottom = 6;
        double _heightFree = 2;
        double _momInertBottom = -1;
        double _forceBottom = -1;
        double _heightTop = -1;
        double _momInertTop = -1;
        double _forceTop = -1;
        double _muBottom = -1;
        double _muTop = -1;
        double _effectLengthBottom = -1;
        double _effectLengthTop = -1;
        double _effortBottom = -1;
        double _effortTop = -1;

        public FramePropertiesOfColumnWithIncompleteCoupling framePropertiesOfColumnWithIncompleteCoupling
        {
            get { return _framePropertiesOfColumnWithIncompleteCoupling; }
            set { _framePropertiesOfColumnWithIncompleteCoupling = value; }
        }
        public double heightBottom
        {
            get { return _heightBottom; }
            set { _heightBottom = value; }
        }
        public double heightFree
        {
            get { return _heightFree; }
            set { _heightFree = value; }
        }
        public double momInertBottom
        {
            get { return _momInertBottom; }
            set { _momInertBottom = value; }
        }
        public double forceBottom
        {
            get { return _forceBottom; }
            set { _forceBottom = value; }
        }
        public double heightTop
        {
            get { return _heightTop; }
            set { _heightTop = value; }
        }
        public double momInertTop
        {
            get { return _momInertTop; }
            set { _momInertTop = value; }
        }
        public double forceTop
        {
            get { return _forceTop; }
            set { _forceTop = value; }
        }
        public double muBottom
        {
            get { return _muBottom; }
            set { _muBottom = value; }
        }
        public double muTop
        {
            get { return _muTop; }
            set { _muTop = value; }
        }
        public double effectLengthBottom
        {
            get { return _effectLengthBottom; }
            set { _effectLengthBottom = value; }
        }
        public double effectLengthTop
        {
            get { return _effectLengthTop; }
            set { _effectLengthTop = value; }
        }

        // запись и чтение последних данных
        const string fileName = "LastDataColumnWithIncompleteCoupling.dat";

        public void WriteEffectiveLength()    // Ф-ция сохраняющая состояние контролов и их значения
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(Convert.ToByte(_framePropertiesOfColumnWithIncompleteCoupling));
                writer.Write(_heightBottom);
                writer.Write(_heightFree);
                writer.Write(_momInertBottom);
                writer.Write(_forceBottom);
                writer.Write(_heightTop);
                writer.Write(_momInertTop);
                writer.Write(_forceTop);
                writer.Write(_muBottom);
                writer.Write(_muTop);
                writer.Write(_effectLengthBottom);
                writer.Write(_effectLengthTop);
            }
        }

        public void ReadEffectiveLength()  // Ф-ция восстанавливающая состояние контролов и их значения
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    _framePropertiesOfColumnWithIncompleteCoupling = (FramePropertiesOfColumnWithIncompleteCoupling)reader.ReadByte();
                    _heightBottom = reader.ReadDouble();
                    _heightFree = reader.ReadDouble();
                    _momInertBottom = reader.ReadDouble();
                    _forceBottom = reader.ReadDouble();
                    _heightTop = reader.ReadDouble();
                    _momInertTop = reader.ReadDouble();
                    _forceTop = reader.ReadDouble();
                    _muBottom = reader.ReadDouble();
                    _muTop = reader.ReadDouble();
                    _effectLengthBottom = reader.ReadDouble();
                    _effectLengthTop = reader.ReadDouble();
                }             
            }
        }

        public bool СheckoutOfParametrsOfDesign(out string message)    // ф-я определения расчетных параметров
        {
            message = string.Empty;
            bool result = true;     

            // проверка отношения I2/I1
            if (_momInertTop / _momInertBottom < 0.01)
            {
                message = message + GeneralRes.RatioOfRigidityExceedBottomLimitColumnWithIncompleteCoupling + "\n";
                result = false;
            }
            else if (_momInertTop / _momInertBottom > 16)
            {
                message = message + GeneralRes.RatioOfRigidityExceedTopLimitColumnWithIncompleteCoupling + "\n";
                result = false;
            }

            // проверка отношения l2/l1
            if (_heightTop / _heightBottom < 0.01)
            {
                message = message + GeneralRes.RatioOfLengthExceedBottomLimitColumnWithIncompleteCoupling + "\n";
                result = false;
            }
            else if (_heightTop / _heightBottom > 100)
            {
                message = message + GeneralRes.RatioOfLengthExceedTopLimitColumnWithIncompleteCoupling + "\n";
                result = false;
            }
            return result;
        }

    // Основной функционал (расчетная часть)
        // вспомогательные функции определения коэффициентов
        private double fi_1(double _nu)
        {
            if (_nu == 0)
                return 1;
            else
                return _nu * _nu / (3 * (1 - _nu / Math.Tan(_nu)));
        }

        private double fi_2(double _nu)
        {
            if (_nu == 0)
                return 1;
            else
                return (1 - _nu / Math.Tan(_nu)) / (4 * (Math.Tan(0.5 * _nu) / (0.5 * _nu) - 1));
        }

        private double fi_4(double _nu)
        {
            if (_nu == 0)
                return 1;
            else
                return fi_1(0.5 * _nu);
        }

        private double eta_1(double _nu)
        {
            if (_nu == 0)
                return 1;
            else
                return _nu * _nu / (3 * (Math.Tan(_nu) / _nu - 1));
        }

        private double eta_2(double _nu)
        {
            return fi_4(_nu) - _nu*_nu / 12;
        }

        private double eta_3(double _nu)
        {
            if (_nu == 0)
                return 1;
            else
                return fi_4(_nu);
        }

        private double Delta_CrossbraceOnBottom(double _nu_1, double _k)  // метод вычисляющий уравнение равновесия по методу перемещений, служит для определения значения коэффициента НЮ (НЮ=ПИ/МЮ) методом подбора
        {
            // внутренние переменные: COB = CrossbraceOnBottom. Для того чтоб не пересекались параметны со случаям связи внизу
            double _momInertBottom_COB = 1000;         // просто принятая величина - при расчете сокращается
            double _momInertTop_COB = _momInertBottom_COB;
            double _heightTop_COB = _heightFree;
            double _heightBottom_COB = _heightBottom - _heightFree;  

            double _r_11 = (3 * _momInertTop_COB / (_heightTop_COB * _heightTop_COB * _heightTop_COB)) * eta_1(_k * _nu_1);
            double _r_22;
            if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                _r_22 = 3 * (_momInertBottom_COB * fi_1(_nu_1) / _heightBottom_COB + _momInertTop_COB * fi_1(_k * _nu_1) / _heightTop_COB);
            else
                _r_22 = 3 * _momInertTop_COB * fi_1(_nu_1 * _k) / _heightTop_COB + 4 * _momInertBottom_COB * fi_2(_nu_1) / _heightBottom_COB;
            double _r_12 = -(3 * _momInertTop_COB / (_heightTop_COB * _heightTop_COB)) * fi_1(_k * _nu_1);
            double _Delta = _r_11 * _r_22 - _r_12 * _r_12;
            return _Delta;
        }

        private double Delta_CrossbraceOnTop(double _nu, double _n_1, double _alfa_1)  // метод вычисляющий уравнение равновесия по методу перемещений, служит для определения значения коэффициента НЮ (НЮ=ПИ/МЮ) методом подбора
        {
            // чтоб не создавать путаницу в следующих формулах
            double _beta_1 = 1 / _alfa_1;
            double _n_2 = 1 / _n_1;

            if (_effortTop == 0)
            {
                if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                    return eta_1(_nu) * (fi_1(_nu) + _n_2) - fi_1(_nu) * fi_1(_nu);
                else
                    return eta_2(_nu) * (4 * fi_2(_nu) + 3 * _n_2) - 3 * fi_4(_nu) * fi_4(_nu);
            }
            else
            {
                if (_heightBottom / (_heightBottom + _heightTop) > 0.5)
                {
                    if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                        return eta_1(_nu) * (_n_1 * fi_1(_nu) + fi_1(_alfa_1 * _nu)) - _n_1 * fi_1(_nu) * fi_1(_nu);
                    else
                        return eta_2(_nu) * (4 * _n_1 * fi_2(_nu) + 3 * fi_1(_alfa_1 * _nu)) - 3 * _n_1 * fi_4(_nu) * fi_4(_nu);
                }
                else
                { 
                    if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                        return eta_1(_beta_1 * _nu) * (fi_1(_beta_1 * _nu) + _n_2 * fi_1(_nu)) - fi_1(_beta_1 * _nu) * fi_1(_beta_1 * _nu);
                    else
                        return eta_2(_beta_1 * _nu) * (4 * fi_2(_beta_1 * _nu) + 3 * _n_2 * fi_1(_nu)) - 3 * fi_4(_beta_1 * _nu) * fi_4(_beta_1 * _nu);
                }
            }
        }
        
        public bool CalculateEffectiveLength()  
        {
            if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.PositionOfCrossbrace) == 0)
                return EffectiveLength_CrossbraceOnBottom();
            else
                return EffectiveLength_CrossbraceOnTop();
        }

        public bool EffectiveLength_CrossbraceOnBottom() 
        {
            // внутренние переменные: COB = CrossbraceOnBottom. Для того чтоб не пересекались параметны со случаям связи внизу
            double _momInertBottom_COB = 1000;         // просто принятая величина - при расчете сокращается
            double _momInertTop_COB = _momInertBottom_COB;
            double _effortBottom_COB = 100;             // просто принятая величина - при расчете сокращается
            double _effortTop_COB = _effortBottom_COB;
            double _heightTop_COB = _heightFree;                
            double _heightBottom_COB = _heightBottom - _heightFree;  

            double _ratio_c_h =  _heightTop_COB / (_heightTop_COB + _heightBottom_COB);
           
            if (_ratio_c_h < 0.0004)   // взято по большему числу (соответствует случаю жесткого опирания) при котором начинается расхождение в решении
            {
                _muBottom = 1;
                if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) != 0)
                    _muBottom = 0.699155659644251;
                _effectLengthBottom = (_heightBottom_COB + _heightTop_COB) * _muBottom;
            }
            else if (_ratio_c_h > 0.94) // взято по меньшему числу (соответствует случаю жесткого опирания) при котором начинается расхождение в решении
            {
                // коэффициента многочлена
                double A = 26.8039804658184;
                double B = -144.66369637388;
                double C = 335.329724418713;
                double D = -432.819052207775;
                double E = 334.681253176409;
                double F = -152.421514004539;
                double G = 35.4379703352476;
                double H = -2.67255024762824;
                double K = 1.33362951719697;
                double L = -0.00974507202565207;
                double M = 1;
                if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) != 0)
                {
                    A = -8.889612598174450;
                    B = 54.042766065750400;
                    C = -144.878688912181000;
                    D = 224.519625152912000;
                    E = -220.741588933359000;
                    F = 140.975088634012000;
                    G = -56.023527638741500;
                    H = 10.887832827030400;
                    K = 0.997885151343686;
                    L = 0.411064592807836;
                    M = 0.699155659644251;
                }

                // многочлен приближенно описывающий кривую параметра МЮ
                _muBottom = A * Math.Pow(_ratio_c_h, 10) + B * Math.Pow(_ratio_c_h, 9) + C * Math.Pow(_ratio_c_h, 8) +
                        D * Math.Pow(_ratio_c_h, 7) + E * Math.Pow(_ratio_c_h, 6) + F * Math.Pow(_ratio_c_h, 5) +
                        G * Math.Pow(_ratio_c_h, 4) + H * Math.Pow(_ratio_c_h, 3) + K * Math.Pow(_ratio_c_h, 2) +
                        L * Math.Pow(_ratio_c_h, 1) + M;
                _effectLengthBottom = (_heightBottom_COB + _heightTop_COB) * _muBottom;
            }
            else
            {
                double _nu_1 = 0;

                // соотношение Мю1 к Мю2, используемый для перехода от Мю1 к Мю2
                double _k = _heightTop_COB / _heightBottom_COB * Math.Sqrt((_momInertBottom_COB * _effortTop_COB) / (_momInertTop_COB * _effortBottom_COB));

                // пределы для первого приближения
                double[] _arr_nu1 = new double[11];
                if ((_framePropertiesOfColumnWithIncompleteCoupling & FramePropertiesOfColumnWithIncompleteCoupling.BearingOfColumn) == 0)
                {
                    _arr_nu1[0] = 3.141592653589790;
                    _arr_nu1[1] = 2.793897039910340;
                    _arr_nu1[2] = 2.383135231418560;
                    _arr_nu1[3] = 1.945698334628100;
                    _arr_nu1[4] = 1.530955179927610;
                    _arr_nu1[5] = 1.165561185207210;
                    _arr_nu1[6] = 0.853330611194454;
                    _arr_nu1[7] = 0.588281965430730;
                    _arr_nu1[8] = 0.362382314229780;
                    _arr_nu1[9] = 0.168294555592276;
                    _arr_nu1[10] = 0.0000001;
                }
                else
                {
                    _arr_nu1[0] = 4.493409457900000;
                    _arr_nu1[1] = 3.736351197500000;
                    _arr_nu1[2] = 2.948940955619890;
                    _arr_nu1[3] = 2.256144219612150;
                    _arr_nu1[4] = 1.695973939544250;
                    _arr_nu1[5] = 1.251580405566150;
                    _arr_nu1[6] = 0.896291545909490;
                    _arr_nu1[7] = 0.607776624496171;
                    _arr_nu1[8] = 0.369561923182908;
                    _arr_nu1[9] = 0.169814163815365;
                    _arr_nu1[10] = 0.0000001;
                }

                // определение пределов первого приближения                
                double _num = (_ratio_c_h * 10);
                if (_num % 1 == 0)  // совпало с одним пределом
                {
                    _nu_1 = _arr_nu1[(int)_num];
                }
                else  // между двух пределов
                {
                    int _num_down = (int)Math.Floor(_ratio_c_h * 10);
                    int _num_up = (int)Math.Ceiling(_ratio_c_h * 10);
                    double _nu_1_down = _arr_nu1[_num_down];
                    double _nu_1_up = _arr_nu1[_num_up];
                    _effectLengthBottom = _heightBottom_COB;
                    _effectLengthTop = _heightTop_COB;

                    // определение методом подбора такого Ню1, при котором точность расчетной длины обеспечена до 3го знака с учетом округления (отличие расчетной длины между двумя последними итерациями не превышают 0,0001)
                    while (Math.Abs(_effectLengthTop - _effectLengthBottom) > 0.0001)
                    {
                        double _Delta_down = Delta_CrossbraceOnBottom(_nu_1_down, _k);
                        double _Delta_up = Delta_CrossbraceOnBottom(_nu_1_up, _k);
                        _nu_1 = _nu_1_up + (_Delta_up - 0) * (_nu_1_down - _nu_1_up) / (_Delta_up - _Delta_down);
                        if (Math.Abs(_Delta_up) > Math.Abs(_Delta_down))
                            _nu_1_up = _nu_1;
                        else
                            _nu_1_down = _nu_1;
                        _muBottom = Math.PI / _nu_1_down;
                        _muTop = Math.PI / (_nu_1_up * _k);
                        _effectLengthBottom = _heightBottom_COB * _muBottom;
                        _effectLengthTop = _heightTop_COB * _muTop;
                    }
                }
                // по найденному Ню1 определяются все остальные параметры
                double _nu_2 = _nu_1 * _k;
                _muBottom = Math.PI / _nu_1;
                _muTop = Math.PI / _nu_2;
                _effectLengthBottom = _heightBottom_COB * _muBottom;
                _effectLengthTop = _heightTop_COB * _muTop;
                _muBottom = _effectLengthBottom / (_heightBottom_COB + _heightTop_COB);
            }
            return true;
        }

        public bool EffectiveLength_CrossbraceOnTop()
        {         
            {
                double _nu = 0;
                double _nu_1 = 0;
                double _nu_2 = 0;

                // переход от сил к усилиям
                _effortTop = _forceTop;
                _effortBottom = _forceTop + _forceBottom;

                // соотношение Мю1 к Мю2, используемый для перехода от Мю1 к Мю2
                double _alfa_1 = _heightTop / _heightBottom * Math.Sqrt((_effortTop * _momInertBottom) / (_effortBottom * _momInertTop));

                // соотношение погонной жесткости
                double _n_1 = (_momInertBottom * _heightTop) / (_momInertTop * _heightBottom);
               
                // определение пределов первого приближения
                double _nu_down = 0;
                double _nu_up = 0.001;
                double _Delta_down = Delta_CrossbraceOnTop(_nu_down, _n_1, _alfa_1); 
                double _Delta_up = Delta_CrossbraceOnTop(_nu_up, _n_1, _alfa_1);
                double _effectLengtBottom_down = _heightBottom * 0.7;
                double _effectLengtBottom_up = _heightBottom * 2;
                double _muBottom_down = Math.PI / _nu_down;
                double _muBottom_up = Math.PI / _nu_up;                
                while (_Delta_up * _Delta_down >= 0) 
                {
                    if (_nu_up > 2 * Math.PI)
                        return false;
                    _nu_down += 0.001;
                    _nu_up += 0.001;
                    _Delta_down = Delta_CrossbraceOnTop(_nu_down, _n_1, _alfa_1);  
                    _Delta_up = Delta_CrossbraceOnTop(_nu_up, _n_1, _alfa_1);  
                }

                // определение методом подбора такого Ню1, при котором точность расчетной длины обеспечена до 3го знака с учетом округления (отличие расчетной длины между двумя последними итерациями не превышают 0,0001)
                while (Math.Abs(_effectLengtBottom_up - _effectLengtBottom_down) > 0.0001)
                {
                    _Delta_down = Delta_CrossbraceOnTop(_nu_down, _n_1, _alfa_1);  
                    _Delta_up = Delta_CrossbraceOnTop(_nu_up, _n_1, _alfa_1);  
                    _nu = _nu_up + (_Delta_up - 0) * (_nu_down - _nu_up) / (_Delta_up - _Delta_down);
                    if (Math.Abs(_Delta_up) > Math.Abs(_Delta_down))
                        _nu_up = _nu;
                    else
                        _nu_down = _nu;
                    _muBottom_down = Math.PI / _nu_down; 
                    _muBottom_up = Math.PI / _nu_up; 
                    _effectLengtBottom_down = _heightBottom * _muBottom_down; 
                    _effectLengtBottom_up = _heightBottom * _muBottom_up;                    
                }
               
                    if (_heightBottom / (_heightBottom + _heightTop) > 0.5 || _effortTop == 0)
                    {
                        _nu_1 = _nu;
                        _nu_2 = _nu_1 * _alfa_1;
                    }
                    else
                    {
                        _nu_2 = _nu;
                        double _beta_1 = 1 / _alfa_1;
                        _nu_1 = _nu_2 * _beta_1;
                    } 

                // по найденым Ню1 и Ню2 определяются все остальные параметры
                _muBottom = Math.PI / _nu_1;
                _muTop = Math.PI / _nu_2;
                _effectLengthBottom = _heightBottom * _muBottom;
                _effectLengthTop = _heightTop * _muTop;
                return true;
            }
        }
    }
}
