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

namespace Tienkio.Camera {
    [System.Serializable]
    public class CameraPerson {
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public float movementSppeed, offsetChangeSpeed, rotationSpeed;
    }

    public class CameraController : MonoBehaviour {
        public Transform player;
        public new Transform camera;

        public bool isFirstPerson;

        public CameraPerson firstPerson;
        public CameraPerson thirdPerson;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        void FixedUpdate() {
            if (Input.GetButtonDown("Change Camera")) isFirstPerson = !isFirstPerson;

            if (player != null) {
                CameraPerson person = isFirstPerson ? firstPerson : thirdPerson;

                Vector3 positionOffset = person.positionOffset;
                Quaternion rotationOffset = Quaternion.Euler(person.rotationOffset);
                float movementSpeed = person.movementSppeed;
                float offsetChangeSpeed = person.offsetChangeSpeed;
                float rotationSpeed = person.rotationSpeed;

                if (movementSpeed > 0) {
                    transform.position = Vector3.Lerp(transform.position, player.position,
                                                      Time.deltaTime * movementSpeed);
                } else {
                    transform.position = player.position;
                }

                if (offsetChangeSpeed > 0) {
                    camera.localPosition = Vector3.Lerp(camera.localPosition, positionOffset,
                                                        Time.deltaTime * offsetChangeSpeed);
                } else {
                    camera.localPosition = positionOffset;
                }

                if (rotationSpeed > 0) {
                    transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation * rotationOffset,
                                                         Time.deltaTime * rotationSpeed);
                } else {
                    transform.rotation = player.rotation * rotationOffset;
                }
            }
        }
    }
}
