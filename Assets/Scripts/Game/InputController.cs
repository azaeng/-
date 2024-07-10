using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {  // 카메라와 큐브 회전
   // 카메라
   public float orbitDampening = 10f; // 카메라 회전 감쇠 속도
   public float cameraDistance = 10f; // 카메라와 객체 간의 거리
   public float perspectiveZoomSpeed = 0.5f; // 줌 속도

   private Transform cameraPivot; // 카메라 회전의 중심
   private Vector3 localRotation; // 로컬 회전 벡터
   // 큐브
   private BigCube bigCube; // 큐브 객체
   private GameObject firstHit; // 첫 번째 터치한 큐브 조각
   private Vector3 firstHitNormal; // 첫 번째 클릭한 면의 벡터
   private Vector3 firstHitCenter; // 첫 번째 클릭한 큐브 조각의 중심
   private GameObject secondHit; // 두 번째 터치한 큐브 조각
   private Vector3 secondHitNormal; // 두 번째 클릭한 면의 벡터
   private Vector3 secondHitCenter; // 두 번째 클릭한 큐브 조각의 중심
   private float offset; // 큐브 크기 오프셋
   private float rotationAngle = 90f; // 회전 각도

   // 초기 설정
   void Start () {
      PlayerSettings.CameraDisable = true; // 카메라 비활성화
      PlayerSettings.CubeRotation = false; // 큐브 회전 비활성화
      this.cameraPivot = this.transform.parent; // 카메라 피벗 설정
      bigCube = FindObjectOfType<BigCube>(); // BigCube 객체 찾기
      offset = PlayerSettings.CubeSize * 0.5f - 0.5f; // 큐브 오프셋 계산
   }
	
	void LateUpdate () {
      if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
         // 큐브가 클릭되었는지 확인
         Ray whatCubeTouched = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit initialTest = new RaycastHit();
         bool cubeWasTouched = false;
         if (Physics.Raycast(whatCubeTouched, out initialTest)) {
            cubeWasTouched = initialTest.transform.gameObject.GetComponent<Cubelet>().inPlay;
         }

         if (cubeWasTouched) { // 큐브 회전
            // 최초 클릭 위치 기록
            if (Input.GetMouseButtonDown(0)) {
               if (!PlayerSettings.CubeRotation) {
                  PlayerSettings.FaceRotation = true;
                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                  RaycastHit hit;
                  if (Physics.Raycast(ray, out hit, 100)) {
                     firstHitNormal = hit.normal; // 첫 번째 클릭 면의 벡터 저장
                     firstHitCenter = hit.transform.gameObject.GetComponent<Renderer>().bounds.center; // 첫 번째 클릭 중심 저장
                     firstHit = hit.transform.parent.gameObject; // 첫 번째 클릭 오브젝트 저장
                  }
               }
            }
            // 클릭 떼면 방향이 선택 보고
            if (Input.GetMouseButtonUp(0)) {
               if (PlayerSettings.FaceRotation == true && !PlayerSettings.CubeRotation) {
                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                  RaycastHit hit;
                  if (Physics.Raycast(ray, out hit, 100)) {
                     secondHitNormal = hit.normal; // 두 번째 클릭 면의 벡터 저장
                     secondHitCenter = hit.transform.gameObject.GetComponent<Renderer>().bounds.center; // 두 번째 클릭 중심 저장
                     secondHit = hit.transform.parent.gameObject; // 두 번째 클릭 오브젝트 저장
                  }
                  Vector3 move = secondHitCenter - firstHitCenter; // 이동 벡터 계산
                  move.Normalize(); // 이동 벡터 정규화

                  DoTheRotation(move); // 회전 실행
               }
            }
         }
         else { // 큐브 클릭이 아닐 경우 큐브 카메라 회전
            // 카메라 궤도 이동 처리
            if (PlayerSettings.CameraDisable && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon) {
               if (Input.GetMouseButton(0)) {
                  localRotation.x += Input.GetAxis("Mouse X") * 4.0f; // 마우스 X축 이동에 따른 회전
                  localRotation.y -= Input.GetAxis("Mouse Y") * 4.0f; // 마우스 Y축 이동에 따른 회전

                  PlayerSettings.CubeRotation = true; // 큐브 회전 상태 설정
               }
            }

            // 줌 인/줌 아웃 처리
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) >= 0.0f) {
               Camera.main.fieldOfView -= scrollDelta * perspectiveZoomSpeed * 100f; // 줌 조정
               Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 90f); // 줌 범위 제한
            }

            // 실제 카메라 리그 변환
            if (PlayerSettings.CubeRotation) {
               Quaternion targetLocation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
               this.cameraPivot.rotation = Quaternion.Slerp(this.cameraPivot.rotation, targetLocation, Time.deltaTime * orbitDampening); // 카메라 회전 적용
            }

            PlayerSettings.CubeRotation = false; // 큐브 회전 상태 해제
         }
      }
   }

   // 회전 방향 확인 메서드
   private bool ConfirmWhichRotation(Vector3 normal, Vector3 tester, Vector3 move, char axis) {
      Vector3 sum = normal + tester;
      if (axis == 'X') { sum = new Vector3(Mathf.Abs(move.x), Mathf.Abs(sum.y), Mathf.Abs(sum.z)); }
      else if (axis == 'Y') { sum = new Vector3(Mathf.Abs(sum.x), Mathf.Abs(move.y), Mathf.Abs(sum.z)); }
      else if (axis == 'Z') { sum = new Vector3(Mathf.Abs(sum.x), Mathf.Abs(sum.y), Mathf.Abs(move.z)); }
      return sum == new Vector3(1, 1, 1);
   }

   // 다른 평면에서의 클릭 확인 메서드
   private bool CheckForHitOnDifferentPlanes(Vector3 fromNormal, Vector3 fromCompare, Vector3 toNormal, Vector3 toCompare) {
      fromNormal = new Vector3(Mathf.Abs(fromNormal.x), Mathf.Abs(fromNormal.y), Mathf.Abs(fromNormal.z));
      toNormal = new Vector3(Mathf.Abs(toNormal.x), Mathf.Abs(toNormal.y), Mathf.Abs(toNormal.z));
      return (fromNormal == fromCompare && toNormal == toCompare);
   }

   // 큐브 회전
   private void DoTheRotation(Vector3 move) {

      if (firstHitNormal == secondHitNormal) { // 동일한 면에서 이동이 발생한 경우
         if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 0, 1), move, 'Y')) { // 회전할 축과 방향 결정
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.x * move.y * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 1, 0), move, 'Z')) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.x * move.z * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 0, 1), move, 'X')) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.y * move.x * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(1, 0, 0), move, 'Z')) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.y * move.z * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 1, 0), move, 'X')) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.z * move.x * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(1, 0, 0), move, 'Y')) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.z * move.y * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
         }
      }
      else { // 서로 다른 면에서 이동이 발생한 경우
         if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 0, 1), secondHitNormal, new Vector3(0, 1, 0))) {  // 회전할 축과 방향 결정
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.z * secondHitNormal.y * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 1, 0), secondHitNormal, new Vector3(0, 0, 1))) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.y * secondHitNormal.z * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 0, 1), secondHitNormal, new Vector3(1, 0, 0))) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.z * secondHitNormal.x * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(1, 0, 0), secondHitNormal, new Vector3(0, 0, 1))) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.x * secondHitNormal.z * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 1, 0), secondHitNormal, new Vector3(1, 0, 0))) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.y * secondHitNormal.x * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(1, 0, 0), secondHitNormal, new Vector3(0, 1, 0))) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.x * secondHitNormal.y * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
         }
      }
   }
   
   // 카메라 설정 토글
   public void toggleCameraSettings() {PlayerSettings.CameraDisable = !PlayerSettings.CameraDisable;}
}
