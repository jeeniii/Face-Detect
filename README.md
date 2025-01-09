# Face-Detect

## 프로젝트 소개
이 프로젝트는 **OpenCV**와 **OpenCvSharp** 라이브러리를 활용하여 **실시간 얼굴 인식** 기능을 제공하는 애플리케이션입니다. Haar Cascade를 사용하여 얼굴과 눈을 탐지하며, C#과 .NET Framework 환경에서 구현되었습니다.

---

## 주요 기능
- **실시간 얼굴 인식**: 웹캠으로 얼굴을 실시간 감지.
- **눈 인식**: 감지된 얼굴 내부에서 눈을 추가로 탐지.
- **시각적 표시**: 얼굴과 눈을 감지하여 화면에 사각형으로 표시.
- **카메라 제어**: 버튼을 통해 카메라 시작 및 중지 기능 제공.

## 기술 스택
- **언어**: C#
- **프레임워크**: .NET Framework 4.8
- **라이브러리**:
  - [OpenCvSharp](https://github.com/shimat/opencvsharp)
  - OpenCV Haar Cascade Classifier