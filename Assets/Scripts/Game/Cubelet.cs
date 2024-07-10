using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 두 컴포넌트가 반드시 존재하게 해줌 (없으면 추가)
[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Cubelet : MonoBehaviour { // 큐브면

   public bool inPlay;  // 게임 내에서 사용 중인지 여부
   public CubeletDirection direction;  // 방향을 나타내는 변수
   public CubeletColors color;   // 색상을 나타내는 변수
}
