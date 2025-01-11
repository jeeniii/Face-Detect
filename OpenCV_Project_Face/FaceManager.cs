using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace OpenCV_Project_Face
{
    public partial class FaceManager : Form
    {
        private VideoCapture capture;   // 카메라 객체
        private CascadeClassifier faceCascade;  // Haar Cascade 객체 (얼굴)
        private CascadeClassifier eyeCascade;   // Haar Cascade 객체 (눈)
        private Mat frame;  // 현재 프레임
        private Bitmap image;
        private bool isCameraRunning = false;

        public FaceManager()
        {
            InitializeComponent();

            // Haar Cascade 파일 로드 (OpenCV 제공)
            faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
            eyeCascade = new CascadeClassifier("haarcascade_eye.xml");

            // VideoCapture 초기화 (기본 카메라: 0)
            capture = new VideoCapture(0);
            if (!capture.IsOpened())
            {
                MessageBox.Show("카메라를 열 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            // Mat 객체 초기화
            frame = new Mat();
        }
        private void FaceManager_Load(object sender, EventArgs e)
        {
            // 타이머를 사용하여 주기적으로 프레임 처리
            Timer timer = new Timer();
            timer.Interval = 30;    // 30ms마다 호출
            timer.Tick += ProcessFrame;
            timer.Start();

            isCameraRunning = true; // 카메라 시작 플래그 설정
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // 카메라 시작/정지 버튼 클릭 이벤트 처리
            if (isCameraRunning)
            {
                isCameraRunning = false;    // 카메라 정지
                pictureBox1.Image = null;   // PictureBox 비우기
                btnStartStop.Text = "Start Camera"; // 버튼 텍스트 변경
            }
            else
            {
                isCameraRunning = true; // 카메라 시작
                btnStartStop.Text = "Stop Camera"; // 버튼 텍스트 변경
            }
        }

        private void FaceManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 카메라 리소스 해제
            capture.Release();
            capture.Dispose();
            faceCascade.Dispose();
            frame.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (!isCameraRunning) return;

            // 현재 프레임 가져오기
            capture.Read(frame);
            if (frame.Empty()) return;

            // 그레이스케일로 변환
            using (var grayFrame = new Mat())
            {
                Cv2.CvtColor(frame,grayFrame,ColorConversionCodes.BGR2GRAY);

                // 얼굴 검출
                var faces = faceCascade.DetectMultiScale(grayFrame, 1.1, 4, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

                // 얼굴 영역 내부에서 눈 검출
                foreach (var face in faces)
                {
                    // 얼굴에 빨간색 사각형 그리기
                    Cv2.Rectangle(frame, face, Scalar.Red, 2);

                    // 얼굴에 텍스트 추가
                    Cv2.PutText(frame, "face", new OpenCvSharp.Point(face.X + 5, face.Y - 10), HersheyFonts.HersheySimplex, 0.8, Scalar.Red, 2);

                    // 얼굴 영역 추출
                    var faceROI = new Mat(grayFrame, face);

                    // 얼굴의 상단 절반 영역으로 제한
                    var eyeRegion = new Rect(0, 0, face.Width, face.Height / 2);
                    var upperFaceROI = new Mat(faceROI,eyeRegion);

                    // 눈 검출
                    var eyes = eyeCascade.DetectMultiScale(upperFaceROI, 1.1, 4, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(15, 15));

                    foreach (var eye in eyes)
                    {
                        // 눈의 좌표를 전체 이미지 기준으로 변환
                        var eyeRect = new Rect(face.X + eye.X, face.Y + eye.Y, eye.Width, eye.Height);

                        // 눈에 파란색 사각형 그리기
                        Cv2.Rectangle(frame,eyeRect, Scalar.Blue, 2);

                        // 눈에 텍스트 추가
                        Cv2.PutText(frame, "eye", new OpenCvSharp.Point(eyeRect.X + 5, eyeRect.Y - 10), HersheyFonts.HersheySimplex, 0.8, Scalar.Blue, 2);
                    }
                }
            }

            // PictureBox에 프레임 표시
            image = BitmapConverter.ToBitmap(frame);
            pictureBox1.Image = image;
        }
    }
}
