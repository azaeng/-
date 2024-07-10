using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmallCube : MonoBehaviour {  // 여러 Cubelet 객체들로 작은 큐브 생성

   public Material[] materials;  // 큐브 표면에 사용할 재질 배열
   public Cubelet[] cubelets;  // 큐브의 각 면을 구성하는 Cubelet 객체 배열

   private void Awake() {
      cubelets = GetComponentsInChildren<Cubelet>();  // 모든 자식 Cubelet을 cubelets 배열에 할당
      for (int i = 0; i < cubelets.Length; i++) {
         // 각 Cubelet의 이름을 Enum으로 파싱하여 방향 설정
         cubelets[i].direction = (CubeletDirection)Enum.Parse(typeof(CubeletDirection), cubelets[i].name, true);
         }
   }

   // 현재 플레이 중인 큐브면을 가져와서 cubes 리스트에 추가
   public void GetCubeletsInPlay(List<Cubelet> cubes) {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) { cubes.Add(cubelets[i]); }
      }
   }

   // Y축 시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterYRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }

   // Y축 반시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterYRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }

   // X축 시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterXRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.Bottom;
            else if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   // X축 반시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterXRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.Bottom;
            else if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   // Z축 시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterZRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.Bottom;
         }
      }
   }
   // Z축 반시계 방향 회전 후 큐브면의 방향 변경
   public void ChangeDirectionsAfterZRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.Bottom;
         }
      }
   }

   // 큐브면의 재질과 색상을 설정
   public void SetMaterials() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.Bottom];
               cubelets[i].color = CubeletColors.Yellow;
            }
            if (cubelets[i].direction == CubeletDirection.Top) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.Top];
               cubelets[i].color = CubeletColors.White;
            }
            if (cubelets[i].direction == CubeletDirection.North) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.North];
               cubelets[i].color = CubeletColors.Green;
            }
            if (cubelets[i].direction == CubeletDirection.South) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.South];
               cubelets[i].color = CubeletColors.Blue;
            }
            if (cubelets[i].direction == CubeletDirection.East) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.East];
               cubelets[i].color = CubeletColors.Red;
            }
            if (cubelets[i].direction == CubeletDirection.West) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.West];
               cubelets[i].color = CubeletColors.Orange;
            }
         }
         else { // 플레이 중이 아닌 큐브렛은 검은색으로 설정
            cubelets[i].GetComponent<MeshRenderer>().material = materials[materials.Length - 1];
            cubelets[i].color = CubeletColors.Black;
         }
      }
   }

   // 큐브의 특정 섹션 설정
   // 하단 병렬
   public void SetBottomWestSouthCorner() {
      for(int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetBottomEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetBottomWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetBottomEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetSouthBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetEastBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetWestBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetNorthBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetBottomMiddle() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   
   // 중단 병렬
   public void SetMiddleWestSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.South || cubelets[i].direction == CubeletDirection.West) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetSouthMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetMiddleEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetWestMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.West) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetEastMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.East) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetMiddleWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetNorthMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetMiddlemEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.North || cubelets[i].direction == CubeletDirection.East) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }

   // 상단 병렬
   public void SetTopWestSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetTopEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetTopWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetTopEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetTopBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.South) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetEastTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetWestTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetNorthTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.North) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
   public void SetTopMiddle() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top) { cubelets[i].inPlay = true; }
         else { cubelets[i].inPlay = false; Destroy(cubelets[i].GetComponent<Collider>()); }
      }
   }
}