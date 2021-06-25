using GK_Projekt4.Figures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_Projekt4
{

    public partial class MainForm : Form
    {
        bool backfaceCulling = false;
        bool perspectiveCorrection = false;
        bool zBuffering = false;
        bool lightingModel = false;
        Camera currentCamera;
        Point? lastMousePosition;
        const int ScrollCameraTranslation = 10;
        int framesCount = 0;
        bool rotatingCamera = false;

        public MainForm()
        {
            InitializeComponent();
            pictureBox.MouseWheel += new MouseEventHandler(this.pictureBox_MouseWheel);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string CombinedPath = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\Scenes");
            openFileDialog.InitialDirectory = Path.GetFullPath(CombinedPath);
            saveFileDialog.InitialDirectory = Path.GetFullPath(CombinedPath);

            camerasListBox.Items.Add(
                new Camera(60, 20, 1000,
                new double[] { 0, 0, 0 }, new double[] { 0, 200, -200 }));
            camerasListBox.SelectedIndex = 0;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            using (var bitmap = new DirectBitmap(pictureBox.Width, pictureBox.Height))
            {
                var P = currentCamera.GetProjectionMatrix((double)pictureBox.Width / pictureBox.Height);
                var V = currentCamera.GetViewMatrix();
                var PV = Utils.MultiplyMatrices(P, V);
                double[,] zBufferArray = null;
                if (zBuffering)
                {
                    zBufferArray = new double[bitmap.Width, bitmap.Height];
                    for (int i = 0; i < bitmap.Width; i++)
                        for (int j = 0; j < bitmap.Height; j++)
                            zBufferArray[i, j] = 10d;
                }
                var lights = lightingModel ? 
                    lightsListBox.Items.Cast<Light>().ToList() : null;
                foreach (Figure figure in figuresListBox.Items)
                {
                    var clippingSpaceCoords =
                        Utils.MultiplyMatrices(PV, figure.WorldPointsCoords);
                    var clippedTriangles = SutherlandHodgman.ClipTriangles(
                        clippingSpaceCoords, figure);
                    if (figure.Fill == Figure.FillType.Texture)
                    {
                        figure.Texture.LockBits();
                        figure.NormalMap.LockBits();
                    }
                    for (int i = 0; i < clippedTriangles.Count; i++)
                    {
                        clippedTriangles[i].Draw(bitmap,
                                backfaceCulling, zBufferArray, perspectiveCorrection,
                                lights, currentCamera.P);
                    }
                    if (figure.Fill == Figure.FillType.Texture)
                    {
                        figure.Texture.UnlockBits();
                        figure.NormalMap.UnlockBits();
                    }
                }
                e.Graphics.DrawImage(bitmap.Bitmap, 0, 0);
            }
            framesCount++;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private double[,] NormalizeVertices(double[,] vertices)
        {
            double[,] result = (double[,])vertices.Clone();
            for (int i=0; i<vertices.GetLength(1); i++)
            {
                double w = vertices[3, i];
                for (int j = 0; j < 3; j++)
                    result[j, i] = vertices[j, i] / w;
                result[3, i] = 1;
            }
            return result;
        }

        private void addCuboidMenuItem_Click(object sender, EventArgs e)
        {
            AddFigure(new Cuboid(100, 100, 100));
        }

        private void addSphereMenuItem_Click(object sender, EventArgs e)
        {
            AddFigure(new Sphere(50, 20, 20));
        }

        private void addCylinderMenuItem_Click(object sender, EventArgs e)
        {
            AddFigure(new Cylinder(50, 100, 30));
        }

        private void addConeMenuItem_Click(object sender, EventArgs e)
        {
            AddFigure(new Cone(50, 100, 30));
        }

        private void AddFigure(Figure figure)
        {
            figuresListBox.Items.Add(figure);
            figuresListBox.SelectedItem = figure;
        }

        private void figuresListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = figuresListBox.SelectedItem;
        }

        private void addCameraMenuItem_Click(object sender, EventArgs e)
        {
            Camera camera = new Camera(60, 20, 1000,
                new double[] { 0, 0, 0 }, new double[] { 0, 200, -200 });
            camerasListBox.Items.Add(camera);
            camerasListBox.SelectedItem = camera;
        }

        private void addLightMenuItem_Click(object sender, EventArgs e)
        {
            Light light = new Light(200, 200, -400);
            lightsListBox.Items.Add(light);
            lightsListBox.SelectedItem = light;
        }

        private void figuresListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (figuresListBox.SelectedIndex != -1)
            {
                camerasListBox.ClearSelected();
                lightsListBox.ClearSelected();
                propertyGrid.SelectedObject = figuresListBox.SelectedItem;
            }
            else if (camerasListBox.SelectedIndex == -1
                && lightsListBox.SelectedIndex == -1
                && propertyGrid.SelectedObject != null) //ten else if jest dla przypadku, w którym był wybrany jakiś obiekt na którejś liście ale jest deselectowany i żaden inny nie jest wybrany
                propertyGrid.SelectedObject = null;
        }

        private void camerasListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (camerasListBox.SelectedIndex != -1)
            {
                figuresListBox.ClearSelected();
                lightsListBox.ClearSelected();
                propertyGrid.SelectedObject = camerasListBox.SelectedItem;
                currentCamera = camerasListBox.SelectedItem as Camera;
            }
            else if (figuresListBox.SelectedIndex == -1
                && lightsListBox.SelectedIndex == -1
                && propertyGrid.SelectedObject != null) //ten else if jest dla przypadku, w którym był wybrany jakiś obiekt na którejś liście ale jest deselectowany i żaden inny nie jest wybrany
                propertyGrid.SelectedObject = null;
        }

        private void lightsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lightsListBox.SelectedIndex != -1)
            {
                figuresListBox.ClearSelected();
                camerasListBox.ClearSelected();
                propertyGrid.SelectedObject = lightsListBox.SelectedItem;
            }
            else if (figuresListBox.SelectedIndex == -1
                && camerasListBox.SelectedIndex == -1
                && propertyGrid.SelectedObject != null) //ten else if jest dla przypadku, w którym był wybrany jakiś obiekt na którejś liście ale jest deselectowany i żaden inny nie jest wybrany
                propertyGrid.SelectedObject = null;
        }

        private void deleteFigureMenuItem_Click(object sender, EventArgs e)
        {
            if (figuresListBox.SelectedIndex != -1)
            {
                figuresListBox.Items.RemoveAt(figuresListBox.SelectedIndex);
            }
        }

        private void deleteCameraMenuItem_Click(object sender, EventArgs e)
        {
            if (camerasListBox.SelectedIndex != -1 && camerasListBox.Items.Count > 1)
            {
                camerasListBox.Items.RemoveAt(camerasListBox.SelectedIndex);
                currentCamera = camerasListBox.Items[0] as Camera;
            }
        }

        private void deleteLightMenuItem_Click(object sender, EventArgs e)
        {
            if (lightsListBox.SelectedIndex != -1)
            {
                lightsListBox.Items.RemoveAt(lightsListBox.SelectedIndex);
            }
        }

        private void backfaceCullingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            backfaceCulling = backfaceCullingCheckbox.Checked;
        }

        private void zBufferingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            zBuffering = zBufferingCheckbox.Checked;
        }

        private void lightingModelCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            lightingModel = lightingModelCheckbox.Checked;
        }

        private void perspectiveCorrectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            perspectiveCorrection = perspectiveCorrectionCheckbox.Checked;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            lastMousePosition = e.Location;
            rotatingCamera = (ModifierKeys & Keys.Shift) == Keys.Shift;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            lastMousePosition = null;
        }

        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                currentCamera.ChangeDistanceFromTarget(e.Delta > 0 ?
                    ScrollCameraTranslation : -ScrollCameraTranslation);
            else
                currentCamera.Translate(e.Delta>0 ? ScrollCameraTranslation : 
                    -ScrollCameraTranslation, 0, 0);
        }

        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            fpsLabel.Text = framesCount.ToString();
            framesCount = 0;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastMousePosition == null)
                return;
            if (rotatingCamera)
            {
                double horAngle = -2 * Math.PI * (e.X - lastMousePosition.Value.X) / pictureBox.Width;
                double verAngle = -Math.PI * (e.Y - lastMousePosition.Value.Y) / pictureBox.Height;
                currentCamera.Rotate(horAngle, verAngle);
            }
            else
            {
                int offsetY = e.Y - lastMousePosition.Value.Y;
                int offsetX = e.X - lastMousePosition.Value.X;
                currentCamera.Translate(0, -offsetX, offsetY);
                //odwróciłem osie poziomą i pionową ruchu kamery, by jej ruch był bardziej intuicyjny
                //oznacza to, że przesuwając myszką w lewo idziemy kamerą w prawo i odwrotnie, to samo dla osi pionowej
            }
            lastMousePosition = e.Location;
        }

        private void normalAnimationRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (normalAnimationRadio.Checked)
                timer.Interval = 40;
        }

        private void oneFpsAnimationRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (oneFpsAnimationRadio.Checked)
                timer.Interval = 1000;
        }

        private void offAnimationRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (offAnimationRadio.Checked)
            {
                timer.Enabled = false;
                pictureBox.Enabled = false;
            }
            else
            {
                timer.Enabled = true;
                pictureBox.Enabled = true;
            }
        }

        private void saveFileBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var data = new SceneData(
                    figuresListBox.Items.Cast<Figure>().ToArray(),
                    camerasListBox.Items.Cast<Camera>().ToArray(),
                    lightsListBox.Items.Cast<Light>().ToArray(),
                    backfaceCulling,
                    zBuffering,
                    lightingModel,
                    perspectiveCorrection
                );
                BinaryFormatter bf = new BinaryFormatter();
                var fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                bf.Serialize(fs, data);
                fs.Close();
            }
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                var fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                SceneData data = (SceneData)bf.Deserialize(fs);
                fs.Close();

                backfaceCullingCheckbox.Checked = data.BackfaceCulling;
                zBufferingCheckbox.Checked = data.ZBuffer;
                perspectiveCorrectionCheckbox.Checked = data.PerspectiveCorrection;
                lightingModelCheckbox.Checked = data.Lighting;

                figuresListBox.SelectedIndex = -1;
                camerasListBox.SelectedIndex = -1;
                lightsListBox.SelectedIndex = -1;

                figuresListBox.Items.Clear();
                camerasListBox.Items.Clear();
                lightsListBox.Items.Clear();

                figuresListBox.Items.AddRange(data.Figures);
                camerasListBox.Items.AddRange(data.Cameras);
                lightsListBox.Items.AddRange(data.Lights);

                camerasListBox.SelectedIndex = 0;
                currentCamera = data.Cameras[0];
            }
        }

    }
}
