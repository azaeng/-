using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCube : MonoBehaviour { // 작은 큐브로 큰 큐브 생성

   public Transform cameraPivot; // 카메라 축
   public SmallCube smallCubePrefab; // 작은 큐브 프리팹
   public float rotationTime; // 회전 시간

   private SmallCube[,,] smallCubes; // 작은 큐브 배열
   private bool currentlyRotating; // 현재 회전 중인지 여부
   private GameManager gameManager; // 게임 매니저
   
   private void Awake() {
      currentlyRotating = false;
      cameraPivot = Camera.main.transform.parent; // 카메라 축 설정
      gameManager = FindObjectOfType<GameManager>(); // 게임 매니저 찾기
   }
   public void GenerateCube() {
      // 작은 큐브 배열 초기화
      smallCubes = new SmallCube[PlayerSettings.CubeSize, PlayerSettings.CubeSize, PlayerSettings.CubeSize];
      CreateCube(); // 큐브 생성
   }

   private void CreateCube() {
      for (int z = 0; z < PlayerSettings.CubeSize; z++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int x = 0; x < PlayerSettings.CubeSize; x++) {
               SmallCube newSmallCube = Instantiate(smallCubePrefab) as SmallCube; // 작은 큐브 인스턴스화
               smallCubes[x, y, z] = newSmallCube; // 작은 큐브 배열에 저장
               newSmallCube.transform.parent = transform; // 큰 큐브의 자식으로 설정
               newSmallCube.transform.localPosition = new Vector3(x - PlayerSettings.CubeSize * 0.5f + 0.5f, y - PlayerSettings.CubeSize * 0.5f + 0.5f, z - PlayerSettings.CubeSize * 0.5f + 0.5f); // 위치 설정
               SetActiveCubelets(newSmallCube, x, y, z); // 활성화된 큐브 설정
               smallCubes[x, y, z].SetMaterials(); // 재질 설정
            }
         }
      }
   }

   public bool CheckWinCondition() {
      List<Cubelet> cubelets = new List<Cubelet>();
      for (int z = 0; z < PlayerSettings.CubeSize; z++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int x = 0; x < PlayerSettings.CubeSize; x++) { smallCubes[x, y, z].GetCubeletsInPlay(cubelets); } // 플레이 중인 큐브 수집
         }
      }

      CubeletColors southColor = CubeletColors.Black;
      CubeletColors northColor = CubeletColors.Black;
      CubeletColors eastColor = CubeletColors.Black;
      CubeletColors westColor = CubeletColors.Black;
      CubeletColors topColor = CubeletColors.Black;
      CubeletColors bottomColor = CubeletColors.Black;
      foreach (Cubelet cubelet in cubelets) {
         if (cubelet.direction == CubeletDirection.South)
            southColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.North)
            northColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.East)
            eastColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.West)
            westColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.Top)
            topColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.Bottom)
            bottomColor = cubelet.color;
      }

      foreach (Cubelet cubelet in cubelets) {
         if (cubelet.direction == CubeletDirection.South)
            if (cubelet.color != southColor) 
               return false;
         if (cubelet.direction == CubeletDirection.North)
            if (cubelet.color != northColor)
               return false;
         if (cubelet.direction == CubeletDirection.East)
            if (cubelet.color != eastColor)
               return false;
         if (cubelet.direction == CubeletDirection.West)
            if (cubelet.color != westColor)
               return false;
         if (cubelet.direction == CubeletDirection.Top)
            if (cubelet.color != topColor)
               return false;
         if (cubelet.direction == CubeletDirection.Bottom)
            if (cubelet.color != bottomColor)
               return false;
      }

      return true; // 모든 큐브의 색이 일치하면 승리 조건 충족
  }

   public IEnumerator ScrambleCube(int scrambleTimes, float scrambleRotationTime) {
      PlayerSettings.Scrambling = true; // 섞기 기능 활성화
      float oldRotationTime = rotationTime;
      rotationTime = scrambleRotationTime; // 회전 시간 설정

      for (int i = 0; i < scrambleTimes; i++) {
         PlayerSettings.FaceRotation = true;
         int rotationType = Random.Range(0, 3); // 회전 축 랜덤 선택
         int rotationIndex = Random.Range(0, PlayerSettings.CubeSize); // 회전할 면 사이즈에 따라 랜덤 선택
         int rotationAngle = Random.Range(-1, 1) < 0 ? -90 : 90; // 회전 각도 랜덤 설정
         switch (rotationType) {
            case 0:
               yield return StartCoroutine(RotateAlongX(rotationAngle, rotationIndex)); // X축 회전
               break;
            case 1:
               yield return StartCoroutine(RotateAlongY(rotationAngle, rotationIndex)); // Y축 회전
               break;
            case 2:
               yield return StartCoroutine(RotateAlongZ(rotationAngle, rotationIndex)); // Z축 회전
               break;
            default:
               break;
         }
      }

      rotationTime = oldRotationTime; // 원래 회전 시간 복원
      PlayerSettings.Scrambling = false; // 섞기 기능 비활성화
   }

   public IEnumerator RotateAlongY(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true; // 회전 상태 설정
         GameObject newRotation = new GameObject(); // 회전을 위한 임시 오브젝트 생성
         newRotation.transform.position = new Vector3(0f, 0f, 0f); // 위치 초기화
         float elapsedTime = 0;

         // 회전할 큐브 부모 변경
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[x, rotationIndex, z].transform.parent = newRotation.transform;
            }
         }

         // 회전
         Quaternion quaternion = Quaternion.Euler(0f, angle, 0f);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // 회전 완료 후 다시 부모 설정
         newRotation.transform.rotation = quaternion;
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[x, rotationIndex, z].transform.parent = transform;
            }
         }

         // 회전 후 위치 및 방향 재설정
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'Y');
         
         Destroy(newRotation); // 임시 오브젝트 삭제
         currentlyRotating = false; // 회전 상태 해제

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon(); // 승리 조건 체크 및 처리
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }
   public IEnumerator RotateAlongX(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true; // 회전 상태 설정
         GameObject newRotation = new GameObject(); // 회전을 위한 임시 오브젝트 생성
         newRotation.transform.position = new Vector3(0f, 0f, 0f); // 위치 초기화
         float elapsedTime = 0;

         // 회전할 큐브 부모 변경
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[rotationIndex, y, z].transform.parent = newRotation.transform;
            }
         }

         // 회전
         Quaternion quaternion = Quaternion.Euler(angle, 0f, 0f);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // 회전 완료 후 다시 부모 설정
         newRotation.transform.rotation = quaternion;
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[rotationIndex, y, z].transform.parent = transform;
            }
         }

         // 회전 후 위치 및 방향 재설정
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'X');

         Destroy(newRotation); // 임시 오브젝트 삭제
         currentlyRotating = false; // 회전 상태 해제

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon(); // 승리 조건 체크 및 처리
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }
   public IEnumerator RotateAlongZ(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true; // 회전 상태 설정
         GameObject newRotation = new GameObject(); // 회전을 위한 임시 오브젝트 생성
         newRotation.transform.position = new Vector3(0f, 0f, 0f); // 위치 초기화
         float elapsedTime = 0;

         // 회전할 큐브 부모 변경
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               smallCubes[x, y, rotationIndex].transform.parent = newRotation.transform;
            }
         }

         // 회전
         Quaternion quaternion = Quaternion.Euler(0f, 0f, angle);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // 회전 완료 후 다시 부모 설정
         newRotation.transform.rotation = quaternion;
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               smallCubes[x, y, rotationIndex].transform.parent = transform;
            }
         }

         // 회전 후 위치 및 방향 재설정
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'Z');

         Destroy(newRotation); // 임시 오브젝트 삭제
         currentlyRotating = false; // 회전 상태 해제

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon(); // 승리 조건 체크 및 처리
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }

   private SmallCube[,,] ResetPositionAfterRotation() {

      float multi = PlayerSettings.CubeSize / 2f - 0.5f;
      SmallCube[,,] newSmallCubes = new SmallCube[PlayerSettings.CubeSize, PlayerSettings.CubeSize, PlayerSettings.CubeSize];

      for (int x = 0; x < PlayerSettings.CubeSize; x++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {

               for (int x2 = 0; x2 < PlayerSettings.CubeSize; x2++) {
                  for (int y2 = 0; y2 < PlayerSettings.CubeSize; y2++) {
                     for (int z2 = 0; z2 < PlayerSettings.CubeSize; z2++) {
                        
                        if (smallCubes[x2, y2, z2].transform.position == new Vector3(-multi + x, -multi + y, -multi + z)) {
                           newSmallCubes[x, y, z] = smallCubes[x2, y2, z2];
                        }

                     }
                  }
               }

            }
         }
      }

      return newSmallCubes; // 새로운 작은 큐브 배열 반환
   }
   private void ChangeCubeletsDirections(float angle, int rotationIndex, char rotationAlong) {
      // X축으로 회전
      if (rotationAlong == 'X') {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               if (angle > 0) {
                  smallCubes[rotationIndex, y, z].ChangeDirectionsAfterXRotationClockwise();
               }
               else {
                  smallCubes[rotationIndex, y, z].ChangeDirectionsAfterXRotationCounterClockwise();
               }
            }
         }
      }

      // Y축으로 회전
      else if (rotationAlong == 'Y') {
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               if (angle > 0) {
                  smallCubes[x, rotationIndex, z].ChangeDirectionsAfterYRotationClockwise();
               }
               else {
                  smallCubes[x, rotationIndex, z].ChangeDirectionsAfterYRotationCounterClockwise();
               }
            }
         }
      }

      // Z축으로 회전
      else if (rotationAlong == 'Z') {
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               if (angle > 0) {
                  smallCubes[x, y, rotationIndex].ChangeDirectionsAfterZRotationClockwise();
               }
               else {
                  smallCubes[x, y, rotationIndex].ChangeDirectionsAfterZRotationCounterClockwise();
               }
            }
         }
      }
   }

   private void SetActiveCubelets(SmallCube cube, int x, int y, int z) {
      // 하단 행렬
      if (x == 0 && y == 0 && z == 0) {
         cube.SetBottomWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z == 0) {
         cube.SetSouthBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z == 0) {
         cube.SetBottomEastSouthCorner();
      }
      if (x == 0 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestBottomSide();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetBottomMiddle();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastBottomSide();
      }
      if (x == 0 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetBottomWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetBottomEastNorthCorner();
      }

      // 중단 행렬
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetMiddleWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetSouthMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetMiddleEastSouthCorner();
      }
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastMiddleSide();
      }
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetMiddleWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetMiddlemEastNorthCorner();
      }

      // 상단 행렬
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopEastSouthCorner();
      }
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestTopSide();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetTopMiddle();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastTopSide();
      }
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetTopWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthTopSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetTopEastNorthCorner();
      }
   }
   
}