using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
   
   public int scrambleTimes;
   public float scrambleRotationTime;
   public BigCube bigCubePrefab;
   public GameObject winMessage;
   public GameObject settings;
   public Image backgroundImage;
   public Text timer;
   public Text backToGame;
   
   private BigCube bigCubeInstance;
   private bool cameraModeBefore;
   private float time;
   private int seconds;
   private int minutes;
   private string timeSoFar;

	// 초기화 메서드
	void Awake () {
      PlayerSettings.TimerOn = true; // 타이머 활성화
      cameraModeBefore = false; // 초기 카메라 모드 상태 설정
      BeginGame(); // 게임 시작
   }

   private void BeginGame() { // 게임 시작 메서드
      PlayerSettings.SettingsOn = false; // 설정 비활성화
      PlayerSettings.GameWon = false; // 게임 승리 상태 초기화
      PlayerSettings.FaceRotation = false; // 면 회전 비활성화
      PlayerSettings.CubeRotation = false; // 큐브 회전 비활성화
      PlayerSettings.Scrambling = false; // 섞기 비활성화
      bigCubeInstance = Instantiate(bigCubePrefab) as BigCube; // BigCube 인스턴스 생성
      bigCubeInstance.transform.position = transform.position; // 위치 설정
      bigCubeInstance.GenerateCube(); // 큐브 생성
      Invoke("ScrambleCube", 0.5f); // 0.5초 후 섞기 시작
   }
	
	void Update () {
      if (!PlayerSettings.SettingsOn && !PlayerSettings.GameWon && !PlayerSettings.Scrambling) {
         time += Time.deltaTime; // 경과 시간 증가
      }

      if (PlayerSettings.TimerOn) { // 타이머가 켜져 있을 때 시간 업데이트
         minutes = Mathf.FloorToInt(time / 60F); // 분 계산
         seconds = Mathf.FloorToInt(time - minutes * 60); // 초 계산
         timeSoFar = string.Format("{0:0}:{1:00}", minutes, seconds); // 시간 포맷팅

         timer.text = "Time: " + timeSoFar; // 타이머 텍스트 업데이트
      }
      else {timer.text = "";} // 타이머 텍스트 숨기기
   }

   public void GameWasWon() { // 게임에서 이겼을 때
      winMessage.gameObject.SetActive(true); // 승리 메시지 활성화
      PlayerSettings.GameWon = true; // 게임 승리 상태 설정
   }

   public void ToggleSettings() {
      if (!PlayerSettings.GameWon && !PlayerSettings.Scrambling) {
         if (!PlayerSettings.SettingsOn) {
            cameraModeBefore = PlayerSettings.CameraDisable; // 현재 카메라 모드 저장
            PlayerSettings.SettingsOn = true; // 설정 활성화
            PlayerSettings.CameraDisable = true; // 카메라 비활성화
            settings.SetActive(true); // 설정 오브젝트 활성화
            backToGame.gameObject.SetActive(true); // "게임으로 돌아가기" 텍스트 활성화
            backgroundImage.color = new Color(0.4f, 0.4f, 0.4f); // 배경 이미지 어둡게
         }
         else {
            settings.SetActive(false); // 설정 오브젝트 비활성화
            backToGame.gameObject.SetActive(false); // "게임으로 돌아가기" 텍스트 비활성화
            PlayerSettings.SettingsOn = false; // 설정 비활성화
            PlayerSettings.FaceRotation = false; // 면 회전 비활성화
            PlayerSettings.CameraDisable = cameraModeBefore; // 원래 카메라 모드 복원
            backgroundImage.color = new Color(1f, 1f, 1f); // 배경 이미지 밝게
         }
      }
   }

   public void ScrambleCube() {
      if (PlayerSettings.SettingsOn) { ToggleSettings(); } // 설정이 켜져 있으면 설정 끔
      StartCoroutine(bigCubeInstance.ScrambleCube(scrambleTimes, scrambleRotationTime)); // 큐브 섞기 시작
      time = 0.0f; // 경과 시간 초기화
   }

   public void RestartGame() {
      StopAllCoroutines(); // 모든 코루틴 정지
      winMessage.gameObject.SetActive(false); // 승리 메시지 비활성화
      backToGame.gameObject.SetActive(false); // "게임으로 돌아가기" 텍스트 비활성화
      settings.SetActive(false); // 설정 오브젝트 비활성화
      backgroundImage.color = new Color32(255, 255, 255, 255); // 배경 이미지 원래 색상으로 복원
      Destroy(bigCubeInstance.gameObject); // BigCube 인스턴스 파괴
      SceneManager.LoadScene(1); // 현재 씬 다시 로드
   }

   public void ReturnToMenu() {
      if (PlayerSettings.SettingsOn) { ToggleSettings(); } // 설정이 켜져 있으면 설정 끔
      PlayerSettings.GameWon = false; // 게임 승리 상태 초기화
      StopAllCoroutines(); // 모든 코루틴 정지
      Destroy(bigCubeInstance.gameObject); // BigCube 인스턴스 파괴
      SceneManager.LoadScene(0); // 메뉴 씬 로드
   }
}
