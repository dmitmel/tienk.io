//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;

namespace Tienkio.UI {
    public class Crosshair : MonoBehaviour {
        public new UnityEngine.Camera camera;
        public RectTransform canvas;
        public Transform player;
        public float rayDistance = 2;

        RectTransform rectTransform;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            Vector3 shootPoint = player.position + player.rotation * Vector3.forward * rayDistance;
            // Camera.WorldToViewportPoint returns vector in the range (0, 0)..(1, 1)
            // This vector must be mapped to the range (-0.5, -0.5)..(0.5, 0.5) and scaled to the size of canvas to
            // use it with centered RectTransform
            Vector3 shootPointInViewport = camera.WorldToViewportPoint(shootPoint) - Vector3.one * 0.5f;
            rectTransform.anchoredPosition = Vector2.Scale(shootPointInViewport, canvas.sizeDelta);
        }
    }
}
