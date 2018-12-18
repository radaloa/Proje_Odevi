using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO.Ports;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Math.Geometry;
using AForge.Math;




namespace ObjeTakibi
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection kameralar;
        private VideoCaptureDevice kameram;
        public Form1()
        {
            InitializeComponent();
        }
        int mod=0;
        int R, G, B;
        int gen, yuk;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar1.Maximum = 255;
            trackBar1.Minimum = 0;
            trackBar1.TickFrequency = 1;
            trackBar1.LargeChange = 1;
            trackBar1.SmallChange = 1;
            label1.Text = "Kırmızı: " + trackBar1.Value.ToString();
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.Maximum = 255;
            trackBar2.Minimum = 0;
            trackBar2.TickFrequency = 1;
            trackBar2.LargeChange = 1;
            trackBar2.SmallChange = 1;
            label2.Text = "Yeşil: " + trackBar2.Value.ToString();
            G = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            trackBar3.Maximum = 255;
            trackBar3.Minimum = 0;
            trackBar3.TickFrequency = 1;
            trackBar3.LargeChange = 1;
            trackBar3.SmallChange = 1;
            label3.Text = "Mavi: " + trackBar3.Value.ToString();
            B = trackBar3.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (kameram.IsRunning)
                {
                    kameram.Stop();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            serialPort1.BaudRate = 9600;
            serialPort1.Open();

            if (serialPort1.IsOpen)
            {
                MessageBox.Show("Port Açık!");
            }

          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            MessageBox.Show("Port Kapandı ");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            switch (mod)
            {
                case 1:
                    
                    break;
                case 2:
                    serialPort1.Write("2");
                    break;
                case 3:
                    serialPort1.Write("3");
                    break;
                case 4:
                    serialPort1.Write("4");
                    break;
                case 5:
                    serialPort1.Write("5");
                    break;
                case 6:
                    serialPort1.Write("6");
                    break;
                case 7:
                    serialPort1.Write("7");
                    break;
                case 8:
                    serialPort1.Write("8");
                    break;
                case 9:
                    serialPort1.Write("9");
                    break;
                default:
                    serialPort1.Write("0");
                    break;

                        

            }
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.DataSource = SerialPort.GetPortNames();
            serialPort1.PortName = comboBox2.SelectedItem.ToString();

            kameralar = new FilterInfoCollection(FilterCategory.VideoInputDevice); //Bilgisayarımızda bulunan kameraların bilgilerini AForge kütüphanesi kullanarak aldık ve foreach kullanarak dizi oluşturduk. Dizi elemanlarını ise combobox1'e atadık.

            foreach (FilterInfo item in kameralar)
            {
                comboBox1.Items.Add(item.Name);
            }
            

            comboBox1.SelectedIndex = 0;//Bilgisayarın kamerası dışında kamera kullanmadığımız için 1. sırada kendi kameramız bulunacak. Otomatik olarak dizinin başlangıç değerini seçtik.
        }

        private void dosyaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                serialPort1.Write("0");

                if (kameram.IsRunning)
                {
                    kameram.Stop();
                }
                this.Close();
            
        }
    }

        private void button1_Click(object sender, EventArgs e)
        {
            kameram = new VideoCaptureDevice(kameralar[comboBox1.SelectedIndex].MonikerString);
            kameram.NewFrame += new NewFrameEventHandler(kameram_NewFrame); //NewFrameEventHandler=Video kaynağından gelen verinin tek tek karelere dönüşmesini ve üzerinde görüntü işleme yapabilmemize imkan tanıyor. Bu framelar belirlenen kameram_Frame'de toplanacak  
            kameram.Start();
        }

        private void kameram_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap goruntu1 = (Bitmap)eventArgs.Frame.Clone(); // Elde edilen görüntünün klonunu belirlenen özelliklerle göstermek için klon görüntü alıyoruz.
            Bitmap goruntu2 = (Bitmap)eventArgs.Frame.Clone();
            Mirror ayna = new Mirror(false,true);
            ayna.ApplyInPlace(goruntu1);

            if (radioButton1.Checked) //Manuel Seçim
            {
                EuclideanColorFiltering oklid = new EuclideanColorFiltering();//Renk filtrelemesi kullanacağımız için Öklid filtresi uyguladık.
                oklid.CenterColor = new RGB(Color.FromArgb(R, G, B)); //Trackbarlardan alınacak verilerin değerleri Merkez rengi belirlemek üzere alınmakta
                oklid.Radius = 100; // Merkez belirlendikten sonra uzayda oluşturulacak kürenin çapı belirlendi
                oklid.ApplyInPlace(goruntu1); //Filtre kaynak görüntü klonuna uygulandı4
            }
            if (radioButton2.Checked) //Kırmızı Seçim
            {
                EuclideanColorFiltering oklid = new EuclideanColorFiltering();
                oklid.CenterColor = new RGB(Color.FromArgb(220, 0, 0)); 
                oklid.Radius = 100; 
                oklid.ApplyInPlace(goruntu1);
            }
            if (radioButton3.Checked) //Yeşil Seçim
            {
                EuclideanColorFiltering oklid = new EuclideanColorFiltering();
                oklid.CenterColor = new RGB(Color.FromArgb(0, 220, 0));
                oklid.Radius = 100;
                oklid.ApplyInPlace(goruntu1);
            }
            if (radioButton4.Checked) //Mavi Seçim
            {
                EuclideanColorFiltering oklid = new EuclideanColorFiltering();
                oklid.CenterColor = new RGB(Color.FromArgb(0, 60, 220));
                oklid.Radius = 100;
                oklid.ApplyInPlace(goruntu1);
            }

                BitmapData objectsData = goruntu1.LockBits(new Rectangle(0, 0, goruntu1.Width, goruntu1.Height), ImageLockMode.ReadOnly, goruntu1.PixelFormat);
                Grayscale grifiltresi = new Grayscale(0.2125, 0.7154, 0.0721);
                UnmanagedImage grayImage = grifiltresi.Apply(new UnmanagedImage(objectsData));
                goruntu1.UnlockBits(objectsData);

                BlobCounter parcalar = new BlobCounter(); //Obje takibi yapmamız için gereken parçacık sayıcıyı oluşturduk.
                parcalar.MinHeight = 25; //Parçacıkların minimum yüksekliği belirlendi.
                parcalar.MinWidth = 25; // Parçacıkların minimum genişliği belirlendi.
                parcalar.FilterBlobs = true; //Belirlenen değerler dışında kalan parçalar filtrelendi.
                parcalar.ObjectsOrder = ObjectsOrder.Size;
                parcalar.ProcessImage(grayImage); //Parçacık sayma işleminin uygulanacağı görüntü seçildi.
                Rectangle[] rects = parcalar.GetObjectsRectangles(); //parcaların seçili olarak gösterileceği dikdörtgen yapının bilgileri rects dizisine atandı.
                foreach (Rectangle recs in rects)
                {
                Graphics g = Graphics.FromImage(goruntu1);

                if (rects.Length > 0) // Eğer 0 dan fazla parça varsa aşağıdaki işlemler uygulanacak.
                    {
                        Rectangle objectRect = rects[0]; // Diziye atılan elaman etrafında dikdörtgen çizdirdik
                                                         // g adında bir grafik objesi oluşturduk ve kaynak olarak goruntu1'i seçtik
                    gen = objectRect.X + (objectRect.Width / 2); //dikdörtgenin genişliğini gen adında bir değişkene atadık
                    yuk = objectRect.Y + (objectRect.Height / 2);    //dikdörgenin yüksekliğini yuk adından bir değişkene atadık.

                    if (gen < 210 && yuk < 160)
                    {
                        serialPort1.Write("1");
                    }
                    if ((210 < gen && gen < 420) && yuk < 160)

                    {
                        serialPort1.Write("2");
                    }
                    if (420 < gen && gen < 630 && yuk < 160)
                    {
                        serialPort1.Write("3");
                    }
                    if ((gen < 210 && (160 < yuk && yuk < 320)))
                    {
                        serialPort1.Write("4");
                    }
                    if ((210 < gen && gen < 420) && (160 < yuk && yuk < 320))
                    {
                        serialPort1.Write("5");
                    }
                    if ((420 < gen && gen < 630) && (yuk < 320 && yuk > 160))
                    {
                        serialPort1.Write("6");
                    }
                    if (gen < 210 && (320 < yuk && yuk < 480))
                    {
                        serialPort1.Write("7");
                    }
                    if ((210 < gen && gen < 420) && (320 < yuk && yuk < 480))
                    {
                        serialPort1.Write("8");
                    }
                    if ((420 < gen && gen < 630) && (320 < yuk && yuk < 480))
                    {
                        serialPort1.Write("9");
                    }

                    using (Pen pen = new Pen(Color.FromArgb(10, 200, 10), 2)) //Dikdörtgen çizimini hangi obje etrafında yapılacağını ve dikdörtgenin R,G,B ve alpha değerlerini belirledik.
                        {

                            g.DrawRectangle(pen, objectRect);


                        }

                    g.DrawString("x Kor: "+gen.ToString() + "Y Kor: "+yuk.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(gen, yuk));

                    g.Dispose();



                    }


                    
                }
            pictureBox1.Image = goruntu1;





        }

        public void takip(Bitmap goruntu1)
        {
           
   
          
        }
    }
}
