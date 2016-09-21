using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignOfEffectiveLenghtOfColumns
{
    partial class FormForDesignOfEffectiveLenghtOfColumns : Form
    {
        // инициализация обьектов классов определения расчетных длинн
        EffectiveLenghtOfSimpleColumn effectiveLenghtOfSimpleColumn = new EffectiveLenghtOfSimpleColumn();
        EffectiveLenghtOfSteppedColumn effectiveLenghtOfSteppedColumn = new EffectiveLenghtOfSteppedColumn();
        EffectiveLengthOfColumnWithIncompleteCoupling effectiveLenghtOfColumnWithIncompleteCoupling = new EffectiveLengthOfColumnWithIncompleteCoupling();
        // инициализация элементов
        System.Windows.Forms.TabControl TypeOfColumns;
        System.Windows.Forms.TabPage SimpleColumn;
        System.Windows.Forms.TabPage SteppedColumn;
        System.Windows.Forms.TabPage ColumnWithIncompleteCoupling;

        // конструктор формы
        public FormForDesignOfEffectiveLenghtOfColumns()
        {
            MyInitializeComponent();
            // заполнение контролов
            effectiveLenghtOfSimpleColumn.SetLastControlState();
            effectiveLenghtOfSteppedColumn.SetLastControlState();
            effectiveLenghtOfColumnWithIncompleteCoupling.SetLastControlState();
        }
    
        private void MyInitializeComponent()
        {
            // иконка))
            Icon = GeneralRes.EffLength;
            // собственно содержание формы
            TypeOfColumns = new System.Windows.Forms.TabControl();
            SimpleColumn = new System.Windows.Forms.TabPage();
            SteppedColumn = new System.Windows.Forms.TabPage();
            ColumnWithIncompleteCoupling = new System.Windows.Forms.TabPage();
            SimpleColumn.SuspendLayout();
            SteppedColumn.SuspendLayout();
            ColumnWithIncompleteCoupling.SuspendLayout();
            TypeOfColumns.SuspendLayout();
            SuspendLayout();
            // 
            // TypeOfColumns
            // 
            TypeOfColumns.Controls.Add(SimpleColumn);
            TypeOfColumns.Controls.Add(SteppedColumn);
            TypeOfColumns.Controls.Add(ColumnWithIncompleteCoupling);
            TypeOfColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            TypeOfColumns.Location = new System.Drawing.Point(0, 0);
            TypeOfColumns.Name = "TypeOfColumns";
            TypeOfColumns.SelectedIndex = 0;
            TypeOfColumns.Size = new System.Drawing.Size(536, 620);
            TypeOfColumns.TabIndex = 0;
            // 
            // SimpleColumn
            // 
            SimpleColumn.BackColor = System.Drawing.SystemColors.Control;
            SimpleColumn.Controls.Add(effectiveLenghtOfSimpleColumn);
            SimpleColumn.Location = new System.Drawing.Point(4, 22);
            SimpleColumn.Name = "SimpleColumn";
            SimpleColumn.Padding = new System.Windows.Forms.Padding(3);
            SimpleColumn.Size = new System.Drawing.Size(528, 594);
            SimpleColumn.TabIndex = 0;
            SimpleColumn.Text = GeneralRes.ColumnsOfConstantSection;
            // 
            // SteppedColumn
            // 
            SteppedColumn.BackColor = System.Drawing.SystemColors.Control;
            SteppedColumn.Controls.Add(effectiveLenghtOfSteppedColumn);
            SteppedColumn.Location = new System.Drawing.Point(4, 22);
            SteppedColumn.Name = "SteppedColumn";
            SteppedColumn.Padding = new System.Windows.Forms.Padding(3);
            SteppedColumn.Size = new System.Drawing.Size(528, 594);
            SteppedColumn.TabIndex = 1;
            SteppedColumn.Text = GeneralRes.SteppedColumns;
            // 
            // ColumnWithIncompleteCoupling
            // 
            ColumnWithIncompleteCoupling.BackColor = System.Drawing.SystemColors.Control;
            ColumnWithIncompleteCoupling.Controls.Add(effectiveLenghtOfColumnWithIncompleteCoupling);
            ColumnWithIncompleteCoupling.Location = new System.Drawing.Point(4, 22);
            ColumnWithIncompleteCoupling.Name = "ColumnWithIncompleteCoupling";
            ColumnWithIncompleteCoupling.Padding = new System.Windows.Forms.Padding(3);
            ColumnWithIncompleteCoupling.Size = new System.Drawing.Size(528, 594);
            ColumnWithIncompleteCoupling.TabIndex = 2;
            ColumnWithIncompleteCoupling.Text = GeneralRes.ColumnWithIncompleteCoupling;
            //
            // EffectiveLenghtOfSimpleColumn
            //
            effectiveLenghtOfSimpleColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            effectiveLenghtOfSimpleColumn.Location = new System.Drawing.Point(3, 3);
            effectiveLenghtOfSimpleColumn.Name = "effectiveLenghtOfColumn";
            effectiveLenghtOfSimpleColumn.Size = new System.Drawing.Size(522, 588);
            effectiveLenghtOfSimpleColumn.TabIndex = 0;
            //
            // EffectiveLenghtOfSteppedColumn
            //
            effectiveLenghtOfSteppedColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            effectiveLenghtOfSteppedColumn.Location = new System.Drawing.Point(3, 3);
            effectiveLenghtOfSteppedColumn.Name = "controlEffectiveLenghtOfSteppedColumn";
            effectiveLenghtOfSteppedColumn.Size = new System.Drawing.Size(522, 588);
            effectiveLenghtOfSteppedColumn.TabIndex = 0;
            //
            // EffectiveLenghtOfColumnWithIncompleteCoupling
            //
            effectiveLenghtOfColumnWithIncompleteCoupling.Dock = System.Windows.Forms.DockStyle.Fill;
            effectiveLenghtOfColumnWithIncompleteCoupling.Location = new System.Drawing.Point(3, 3);
            effectiveLenghtOfColumnWithIncompleteCoupling.Name = "controlEffectiveLenghtOfColumnWithIncompleteCoupling";
            effectiveLenghtOfColumnWithIncompleteCoupling.Size = new System.Drawing.Size(522, 588);
            effectiveLenghtOfColumnWithIncompleteCoupling.TabIndex = 0;
            // 
            // FormForDesignOfEffectiveLenghtOfColumns
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            MinimumSize = new System.Drawing.Size(552, 656);
            MaximizeBox = false;
            Controls.Add(TypeOfColumns);
            Name = "FormForDesignOfEffectiveLenghtOfColumns";
            Text = GeneralRes.DeterminationOfEffectiveLengthOfSteelColumns;
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormForDesignOfEffectiveLenghtOfColumns_FormClosing);
            SimpleColumn.ResumeLayout(false);
            SteppedColumn.ResumeLayout(false);
            ColumnWithIncompleteCoupling.ResumeLayout(false);
            TypeOfColumns.ResumeLayout(false);
            ResumeLayout(false); 
        }

        // обработка события закрытия формы
        private void FormForDesignOfEffectiveLenghtOfColumns_FormClosing(object sender, FormClosingEventArgs e)
        {
            effectiveLenghtOfSimpleColumn.UploadLastData();
            effectiveLenghtOfSteppedColumn.UploadLastData();
            effectiveLenghtOfColumnWithIncompleteCoupling.UploadLastData();
        }
    }
}
