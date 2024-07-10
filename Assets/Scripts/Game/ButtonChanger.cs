using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChanger : MonoBehaviour { // 설정의 버튼 텍스트 변경

   public Sprite[] buttonFaces; // 버튼 이미지 저장 배열
   private Button button; // 버튼 컴포넌트 참조 변수

   // 버튼 컴포넌트를 가져옴
   private void Awake() { button = GetComponent<Button>(); }

   // ======== UI -> Lock Rotation ========
   public void SwitchRotationButtons() {
      if (!PlayerSettings.SettingsOn && !PlayerSettings.GameWon) {
         if (button.image.sprite == buttonFaces[0]) {
            button.image.sprite = buttonFaces[1];
            PlayerSettings.CameraDisable = false;
         }
         else {
            button.image.sprite = buttonFaces[0];
            PlayerSettings.CameraDisable = true;
         }
      }
   }
   
   // ======== OverheadUI -> Settings -> Timer ========
   // PlayerSettings에 Timer on/off 정보 전달
   // Timer 텍스트 변경
   public void SwitchTimerButton() {
      if (PlayerSettings.TimerOn) {
         PlayerSettings.TimerOn = false;
         GetComponentInChildren<Text>().text = "Timer: OFF"; // Timer -> Text의 속성
      }
      else {
         PlayerSettings.TimerOn = true;
         GetComponentInChildren<Text>().text = "Timer: ON";
      }
   }
}