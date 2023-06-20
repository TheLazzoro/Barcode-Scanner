using AForge.Video.DirectShow;
using ZXing;

namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice captureDevice;

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (var device in filterInfoCollection)
            {
                // We can select any video capture device, but here we select the first one
                captureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            }

            if (captureDevice == null)
            {
                throw new Exception("No video device was found.");
            }

            captureDevice.NewFrame += CaptureDevice_NewFrame;
        }

        private void CaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null)
            {
                textBox.Invoke(new MethodInvoker(delegate ()
                {
                    textBox.Text = result.ToString();
                }));

            }
            pictureBox1.Image = bitmap;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            captureDevice.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            captureDevice.SignalToStop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

    }
}