using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings { // 플레이어 설정 관리 (정적 class)

   private static int cubeSize;           // 큐브의 크기
   private static bool settingsOn;        // 설정이 활성화되었는지 여부
   private static bool gameWon;           // 게임이 승리 상태인지 여부
   private static bool timerOn;           // 타이머가 실행 중인지 여부
   private static bool cameraDisable;     // 카메라가 비활성화되었는지 여부
   private static bool faceRotation;      // 면 회전이 활성화되었는지 여부
   private static bool cubeRotation;      // 큐브 회전이 활성화되었는지 여부
   private static bool scrambling;        // 섞기 기능이 활성화되었는지 여부

   // 큐브 크기를 가져오고 설정하는 공용 설정
   public static int CubeSize {
      get { return cubeSize; }
      set { cubeSize = value; }
   }
   // 설정 활성화 여부를 가져오고 설정하는 공용 설정
   public static bool SettingsOn {
      get { return settingsOn; }
      set { settingsOn = value; }
   }
   // 게임 승리 여부를 가져오고 설정하는 공용 설정
   public static bool GameWon {
      get { return gameWon; }
      set { gameWon = value; }
   }
   // 타이머 활성화 여부를 가져오고 설정하는 공용 설정
   public static bool TimerOn {
      get { return timerOn; }
      set { timerOn = value; }
   }
   // 카메라 비활성화 여부를 가져오고 설정하는 공용 설정
   public static bool CameraDisable {
      get { return cameraDisable; }
      set { cameraDisable = value; }
   }
   // 면 회전 활성화 여부를 가져오고 설정하는 공용 설정
   public static bool FaceRotation {
      get { return faceRotation; }
      set { faceRotation = value; }
   }
   // 큐브 회전 활성화 여부를 가져오고 설정하는 공용 설정
   public static bool CubeRotation {
      get { return cubeRotation; }
      set { cubeRotation = value; }
   }
   // 섞기 기능 활성화 여부를 가져오고 설정하는 공용 설정
   public static bool Scrambling {
      get { return scrambling; }
      set { scrambling = value; }
   }
}
