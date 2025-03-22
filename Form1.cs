using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PixelHelper
{
	public partial class Form1 : Form
	{
		private Panel panelCanvas;
		private int canvasWidth, canvasHeight;
		private Color lineColor = Color.Black;
		private int lineThickness = 1;
		private Color backgroundColor = Color.Transparent;
		private float horizontalSpacing;
		private float verticalSpacing;
		private int Columns;
		private int Rows;
		private float scale = 1.0f;

		public Form1()
		{
			InitializeComponent();

			ToolStrip toolStrip = new ToolStrip();
			ToolStripDropDownButton fileButton = new ToolStripDropDownButton("�ļ�");
			ToolStripDropDownButton otherButton = new ToolStripDropDownButton("����");

			ToolStripMenuItem newMenuItem = new ToolStripMenuItem("�½�");
			ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("����");
			ToolStripMenuItem saveAsMenuItem = new ToolStripMenuItem("���Ϊ");

			ToolStripMenuItem helpMenuItem = new ToolStripMenuItem("����");

			newMenuItem.Click += NewMenuItem_Click;
			saveMenuItem.Click += SaveMenuItem_Click;
			helpMenuItem.Click += HelpMenuItem_Click;

			fileButton.DropDownItems.Add(newMenuItem);
			fileButton.DropDownItems.Add(saveMenuItem);
			fileButton.DropDownItems.Add(saveAsMenuItem);

			otherButton.DropDownItems.Add(helpMenuItem);

			toolStrip.Items.Add(fileButton);
			toolStrip.Items.Add(otherButton);

			this.Controls.Add(toolStrip);

			panelCanvas = new Panel
			{
				Dock = DockStyle.Fill,
				BackColor = Color.Transparent
			};
			panelCanvas.Paint += PanelCanvas_Paint;
			this.Controls.Add(panelCanvas);
			panelCanvas.BringToFront();

			panelCanvas.MouseWheel += PanelCanvas_MouseWheel;

		}
		private void PanelCanvas_Paint(object sender, PaintEventArgs e)
		{
			//if (horizontalSpacing <= 0 || verticalSpacing <= 0) return;

			e.Graphics.ScaleTransform(scale, scale);

			Rectangle canvasRect = new Rectangle(0, 0, canvasWidth, canvasHeight);

			if (backgroundColor.A == 255)
			{
				e.Graphics.FillRectangle(new SolidBrush(backgroundColor), canvasRect);
			}
			else
			{
				using (Brush brush = new HatchBrush(
					HatchStyle.SmallCheckerBoard,
					Color.Silver,
					Color.White))
				{
					e.Graphics.FillRectangle(brush, canvasRect);
				}
			}

			using (Pen pen = new Pen(lineColor, lineThickness))
			{
				float cellWidth = (float)canvasWidth / Columns;
				float cellHeight = (float)canvasHeight / Rows;

				for (int i = 0; i <= Columns; i++)
				{
					float x = i * cellWidth;
					e.Graphics.DrawLine(pen, x, 0, x, canvasHeight);
				}
				for (int i = 0; i <= Rows; i++)
				{
					float y = i * cellHeight;
					e.Graphics.DrawLine(pen, 0, y, canvasWidth, y);
				}
			}
		}

		private void NewMenuItem_Click(object sender, EventArgs e)
		{
			using (var settings = new SettingsDialog())
			{
				if (settings.ShowDialog() == DialogResult.OK)
				{
					canvasWidth = settings.CanvasWidthPixels;
					canvasHeight = settings.CanvasHeightPixels;
					horizontalSpacing = settings.HorizontalSpacing;
					verticalSpacing = settings.VerticalSpacing;
					Columns = settings.Columns;
					Rows = settings.Rows;
					backgroundColor = settings.BackgroundColor;
					lineColor = settings.LineColor;
					lineThickness = settings.LineThickness;

					panelCanvas.Size = new Size(canvasWidth, canvasHeight);
					panelCanvas.Invalidate();
				}
			}
		}

		private void SaveMenuItem_Click(object sender, EventArgs e)
		{
			if (canvasWidth <= 0 || canvasHeight <= 0)
			{
				MessageBox.Show("���ȴ���������", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (SaveFileDialog saveDialog = new SaveFileDialog
			{
				Filter = "PNG ͼƬ|*.png|JPEG ͼƬ|*.jpg|BMP ͼƬ|*.bmp",
				Title = "���滭��"
			})
			{
				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						using (Bitmap bmp = new Bitmap(canvasWidth+lineThickness, canvasHeight+ lineThickness))
						{
							using (Graphics g = Graphics.FromImage(bmp))
							{
								if (backgroundColor == Color.Transparent)
								{
									g.Clear(Color.Transparent);
								}
								else if (backgroundColor.A < 255)
								{
									g.Clear(Color.FromArgb(255, backgroundColor));
								}
								else
								{
									g.Clear(backgroundColor);
								}

								using (Pen pen = new Pen(lineColor, lineThickness))
								{
									float cellWidth = (float)canvasWidth / (Columns);
									float cellHeight = (float)canvasHeight / (Rows);

									for (int i = 0; i <= Columns; i++)
									{
										float x = i * cellWidth;
										g.DrawLine(pen, x, 0, x, canvasHeight);

									}

									for (int i = 0; i <= Rows; i++)
									{
										float y = i * cellHeight;
										g.DrawLine(pen, 0, y, canvasWidth, y);
									}
								}
							}

							string ext = Path.GetExtension(saveDialog.FileName).ToLower();
							ImageFormat format = ImageFormat.Png;
							if (ext == ".jpg")
							{
								format = ImageFormat.Jpeg;
								if (backgroundColor == Color.Transparent)
								{
									using (Graphics g = Graphics.FromImage(bmp))
									{
										g.Clear(Color.White);
									}
								}
							}
							else if (ext == ".bmp")
							{
								format = ImageFormat.Bmp;
							}

							bmp.Save(saveDialog.FileName, format);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show($"����ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void PanelCanvas_MouseWheel(object sender, MouseEventArgs e)
		{
			float zoomStep = 0.1f;

			if (e.Delta > 0)
			{
				scale += zoomStep;
			}
			else
			{
				scale -= zoomStep;
				if (scale < 0.1f)
				{
					scale = 0.1f;
				}
			}

			panelCanvas.Invalidate();
		}

		private void HelpMenuItem_Click(object sender, EventArgs e)
		{
			var str = $"�����ߴ磺{canvasWidth}x{canvasHeight}px\n";
			str += $"������С��{canvasWidth / 2.54f:0.00}cm x {canvasHeight / 2.54f:0.00}cm\n";
			str += $"�����ֱ��ʣ�{canvasWidth / horizontalSpacing:0.00}x{canvasHeight / verticalSpacing:0.00}px/mm\n";
			if (canvasWidth <= 0 && canvasHeight <= 0)
			{
				str = "";
			}
			MessageBox.Show("�汾:v0.11\n" + str, "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}