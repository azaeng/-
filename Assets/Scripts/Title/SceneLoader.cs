using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour { // 씬이동(타이틀화면에서만 사용)

   public GameObject introScreen;      // 인트로 화면
   public GameObject chooseSizeScreen; // 큐브 사이즈 선택 화면

   // ======== 인트로 화면 ========
   public void intoChooseSize() {
      introScreen.SetActive(false);       // 인트로 화면 비활성화
      chooseSizeScreen.SetActive(true);   // 큐브 사이즈 선택 화면 활성화
   }
   
   // 앱 종료
   public void QuitGame() {
      StopAllCoroutines(); // 모든 코루틴 정지
      Application.Quit(); // 애플리케이션 종료
   }

   // ======== 큐브 사이즈 선택 화면 ========
   // index = 1, Game 화면으로 전환
   // PlayerSettings에 큐브 사이즈 정보 전달
   public void LoadCube2(int index) { // 2*2*2큐브
      PlayerSettings.CubeSize = 2;
      SceneManager.LoadScene(index);
   }
   public void LoadCube3(int index) { // 3*3*3큐브
      PlayerSettings.CubeSize = 3;
      SceneManager.LoadScene(index);
   }
   public void LoadCube4(int index) { // 4*4*4큐브
      PlayerSettings.CubeSize = 4;
      SceneManager.LoadScene(index);
   }
}
