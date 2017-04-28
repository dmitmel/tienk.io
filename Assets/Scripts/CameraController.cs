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

namespace Tienkio {
    public class CameraController : MonoBehaviour {
        public Rigidbody player;
        public float movementSpeed = 1, rotationSpeed = 1;

        new Transform transform;

        Vector3 velocity;
        Vector3 posOffset;
        Quaternion rotOffset;

        void Awake() {
            transform = base.transform;
        }

        void Start() {
            posOffset = transform.position - player.position;
            rotOffset = transform.rotation * Quaternion.Inverse(player.rotation);
        }

        void Update() {
            if (player != null) {
                transform.position = Vector3.Lerp(transform.position, player.position + posOffset,
                                                  Time.deltaTime * movementSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation * rotOffset,
                                                     Time.deltaTime * rotationSpeed);
            }
        }
    }
}
