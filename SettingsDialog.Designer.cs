using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelHelper
{
	partial class SettingsDialog
	{
		private ComboBox comboBoxUnit;
		private NumericUpDown numericCanvasWidth;
		private NumericUpDown numericCanvasHeight;
		private NumericUpDown numericGridWidth;
		private NumericUpDown numericGridHeight;
		private Button buttonLineColor;
		private Panel panelLineColor;
		private NumericUpDown numericLineThickness;
		private Button buttonOK;
		private Button buttonCancel;
		private CheckBox checkBoxAuto;
		private NumericUpDown numericColumns;
		private NumericUpDown numericRows;
		private NumericUpDown numericHorizontalSpacing;
		private NumericUpDown numericVerticalSpacing;
		private Button buttonBackgroundColor;
		private Panel panelBackgroundColor;

		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.Text = "新建画布设置";
			this.ClientSize = new Size(350, 450);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			TableLayoutPanel mainTable = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 2,
				RowCount = 10, 
				CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
				Padding = new Padding(10),
				AutoSize = true
			};

			mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
			mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

			for (int i = 0; i < 9; i++)
			{
				mainTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}
			mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

			mainTable.Controls.Add(new Label { Text = "单位:", AutoSize = true }, 0, 0);
			comboBoxUnit = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
			comboBoxUnit.Items.AddRange(new object[] { "像素", "英寸", "厘米" });
			mainTable.Controls.Add(comboBoxUnit, 1, 0);

			mainTable.Controls.Add(new Label { Text = "画布宽度:", AutoSize = true }, 0, 1);
			numericCanvasWidth = new NumericUpDown { Minimum = 1, Maximum = 10000, Value = 100 };
			mainTable.Controls.Add(numericCanvasWidth, 1, 1);

			mainTable.Controls.Add(new Label { Text = "画布高度:", AutoSize = true }, 0, 2);
			numericCanvasHeight = new NumericUpDown { Minimum = 1, Maximum = 10000, Value = 100 };
			mainTable.Controls.Add(numericCanvasHeight, 1, 2);

			mainTable.Controls.Add(new Label { Text = "列数:", AutoSize = true }, 0, 3);
			numericColumns = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 10 };
			mainTable.Controls.Add(numericColumns, 1, 3);

			mainTable.Controls.Add(new Label { Text = "行数:", AutoSize = true }, 0, 4);
			numericRows = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 10 };
			mainTable.Controls.Add(numericRows, 1, 4);

			mainTable.Controls.Add(new Label { Text = "线条粗细:", AutoSize = true }, 0, 5);
			numericLineThickness = new NumericUpDown { Minimum = 1, Maximum = 20, Value = 1 };
			mainTable.Controls.Add(numericLineThickness, 1, 5);


			checkBoxAuto = new CheckBox { Text = "自动计算间距", AutoSize = true };
			mainTable.Controls.Add(checkBoxAuto, 0, 5);
			mainTable.SetColumnSpan(checkBoxAuto, 2);

			mainTable.Controls.Add(new Label { Text = "水平间距:", AutoSize = true }, 0, 6);
			numericHorizontalSpacing = new NumericUpDown { Minimum = 1, Maximum = 10000, DecimalPlaces = 2 };
			mainTable.Controls.Add(numericHorizontalSpacing, 1, 6);

			mainTable.Controls.Add(new Label { Text = "垂直间距:", AutoSize = true }, 0, 7);
			numericVerticalSpacing = new NumericUpDown { Minimum = 1, Maximum = 10000, DecimalPlaces = 2 };
			mainTable.Controls.Add(numericVerticalSpacing, 1, 7);

			mainTable.Controls.Add(new Label { Text = "线条颜色:", AutoSize = true }, 0, 8);
			panelLineColor = new Panel { BackColor = Color.Black, Size = new Size(20, 20), BorderStyle = BorderStyle.FixedSingle };
			buttonLineColor = new Button { Text = "选择...", AutoSize = true };
			FlowLayoutPanel lineColorPanel = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				AutoSize = true
			};
			lineColorPanel.Controls.Add(panelLineColor);
			lineColorPanel.Controls.Add(buttonLineColor);
			mainTable.Controls.Add(lineColorPanel, 1, 8);

			mainTable.Controls.Add(new Label { Text = "背景颜色:", AutoSize = true }, 0, 9);
			panelBackgroundColor = new Panel { BackColor = Color.Transparent, Size = new Size(20, 20), BorderStyle = BorderStyle.FixedSingle };
			checkBoxTransparent = new CheckBox { Text = "透明", AutoSize = true };
			buttonBackgroundColor = new Button { Text = "选择...", AutoSize = true };
			FlowLayoutPanel bgColorPanel = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				AutoSize = true
			};
			bgColorPanel.Controls.Add(panelBackgroundColor);
			bgColorPanel.Controls.Add(buttonBackgroundColor);
			bgColorPanel.Controls.Add(checkBoxTransparent);
			mainTable.Controls.Add(bgColorPanel, 1, 9);

			TableLayoutPanel buttonPanel = new TableLayoutPanel
			{
				Dock = DockStyle.Bottom,
				Height = 45,
				ColumnCount = 2,
				ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
				Padding = new Padding(5)
			};
			buttonOK = new Button { Text = "确定", DialogResult = DialogResult.OK, Height = 30 };
			buttonCancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Height = 30 };
			buttonPanel.Controls.Add(buttonOK, 0, 0);
			buttonPanel.Controls.Add(buttonCancel, 1, 0);

			this.Controls.Add(mainTable);
			this.Controls.Add(buttonPanel);

			buttonLineColor.Click += buttonLineColor_Click;
			buttonBackgroundColor.Click += buttonBackgroundColor_Click;
			buttonOK.Click += buttonOK_Click;
			buttonCancel.Click += buttonCancel_Click;
			checkBoxAuto.CheckedChanged += checkBoxAuto_CheckedChanged;
			numericColumns.ValueChanged += numericColumns_ValueChanged;
			numericRows.ValueChanged += numericRows_ValueChanged;

			this.ResumeLayout(true);
		}

	}
}
