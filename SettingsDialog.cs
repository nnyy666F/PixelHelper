using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelHelper
{
	public partial class SettingsDialog : Form
	{
		public int CanvasWidthPixels { get; private set; }
		public int CanvasHeightPixels { get; private set; }
		public int GridWidthPixels { get; private set; }
		public int GridHeightPixels { get; private set; }
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public float HorizontalSpacing { get; private set; }
		public float VerticalSpacing { get; private set; }
		public bool AutoSpacing { get; private set; }
		public Color LineColor { get; private set; } = Color.Black;
		public int LineThickness { get; private set; } = 1;
		public Color BackgroundColor { get; private set; } = Color.Transparent;
		public bool IsBackgroundTransparent { get; private set; }
		private CheckBox checkBoxTransparent;

		public SettingsDialog()
		{
			InitializeComponent();
			comboBoxUnit.SelectedIndex = 0;
			checkBoxAuto.Checked = true;
			UpdateSpacingAuto();
		}
		private void UpdateSpacingAuto()
		{
			if (checkBoxAuto.Checked)
			{
				double unitFactor = GetUnitFactor();

				double canvasWidthPixels = (double)numericCanvasWidth.Value * unitFactor;
				double canvasHeightPixels = (double)numericCanvasHeight.Value * unitFactor;
				int cols = (int)numericColumns.Value;
				int rows = (int)numericRows.Value;

				double horizontalSpacingPixel = cols > 0 ? canvasWidthPixels / cols : 0;
				double verticalSpacingPixel = rows > 0 ? canvasHeightPixels / rows : 0;
				numericHorizontalSpacing.Value = (decimal)(horizontalSpacingPixel / unitFactor);
				numericVerticalSpacing.Value = (decimal)(verticalSpacingPixel / unitFactor);
			}
		}

		private double GetUnitFactor()
		{
			if (comboBoxUnit.SelectedItem == null) {
				MessageBox.Show("请选择单位！");
				return -1;
			}
			switch (comboBoxUnit.SelectedItem.ToString())
			{
				case "英寸": return 96;
				case "厘米": return 37.79527559055118;
				default: return 1;
			}
		}
		private void buttonLineColor_Click(object sender, EventArgs e)
		{
			using (ColorDialog colorDialog = new ColorDialog())
			{
				if (colorDialog.ShowDialog() == DialogResult.OK)
				{
					LineColor = colorDialog.Color;
					panelLineColor.BackColor = LineColor;
				}
			}
		}
		private void numericColumns_ValueChanged(object sender, EventArgs e) => UpdateSpacingAuto();
		private void numericRows_ValueChanged(object sender, EventArgs e) => UpdateSpacingAuto();
		private void checkBoxAuto_CheckedChanged(object sender, EventArgs e)
		{
			numericHorizontalSpacing.Enabled = !checkBoxAuto.Checked;
			numericVerticalSpacing.Enabled = !checkBoxAuto.Checked;
			UpdateSpacingAuto();
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (comboBoxUnit == null || numericCanvasWidth == null || numericCanvasHeight == null ||
				numericColumns == null || numericRows == null || numericHorizontalSpacing == null ||
				numericVerticalSpacing == null)
			{
				MessageBox.Show("系统错误：控件未初始化！");
				return;
			}

			if (comboBoxUnit.SelectedItem == null)
			{
				MessageBox.Show("请选择单位！");
				return;
			}

			if (checkBoxTransparent.Checked)
			{
				BackgroundColor = Color.Transparent;
			}
			else if (panelBackgroundColor.BackColor.A < 255)
			{
				BackgroundColor = Color.FromArgb(255, panelBackgroundColor.BackColor);
			}
			else
			{
				BackgroundColor = panelBackgroundColor.BackColor;
			}
			
			try
			{
				double unitFactor = GetUnitFactor();
				CanvasWidthPixels = (int)((double)numericCanvasWidth.Value * unitFactor);
				CanvasHeightPixels = (int)((double)numericCanvasHeight.Value * unitFactor);

				HorizontalSpacing = (float)((double)numericHorizontalSpacing.Value * unitFactor);
				VerticalSpacing = (float)((double)numericVerticalSpacing.Value * unitFactor);

				Columns = (int)numericColumns.Value;
				Rows = (int)numericRows.Value;
				LineColor = panelLineColor.BackColor;
				LineThickness = (int)numericLineThickness.Value;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"输入错误：{ex.Message}");
				return;
			}

			DialogResult = DialogResult.OK;
			Close();
		}
		private void buttonBackgroundColor_Click(object sender, EventArgs e)
		{
			using (ColorDialog colorDialog = new ColorDialog { AllowFullOpen = true, AnyColor = true, FullOpen = true })
			{
				if (colorDialog.ShowDialog() == DialogResult.OK)
				{
					BackgroundColor = colorDialog.Color;
					panelBackgroundColor.BackColor = BackgroundColor;
				}
			}
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

	}
}