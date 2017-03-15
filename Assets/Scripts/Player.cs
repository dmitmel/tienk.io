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

namespace Deepio {
    public class Player : MonoBehaviour {
        public Rigidbody2D movementRoot;
        public Transform rotationRoot;
        public Gun[] guns;

        StatsHolder stats;

        [Space]
        public float accelerationMultiplier = 2;
        public float autoSpinSpeed;

        bool autoSpinEnabled, autoFireEnabled;

        void Start() {
            stats = GetComponent<StatsHolder>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.E)) autoSpinEnabled = !autoSpinEnabled;
            if (autoSpinEnabled) {
                rotationRoot.localRotation *= Quaternion.Euler(0, 0, autoSpinSpeed);
            } else {
                Vector2 positionOnScreen = transform.position;
                Vector2 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);
                rotationRoot.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                autoFireEnabled = !autoFireEnabled;
                if (!autoFireEnabled) {
                    foreach (Gun gun in guns)
                        gun.StopFiring();
                }
            }

            if (autoFireEnabled) {
                foreach (Gun gun in guns)
                    if (!gun.isFiring)
                        gun.StartFiring();
            } else {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) {
                    foreach (Gun gun in guns)
                        if (!gun.isFiring)
                            gun.StartFiring();
                } else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
                    foreach (Gun gun in guns)
                        gun.StopFiring();
                }
            }
        }

        void FixedUpdate() {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            movementRoot.AddForce(new Vector2(horizontalAxis, verticalAxis).normalized *
                                  stats.movementSpeed.statValue * accelerationMultiplier);
            movementRoot.velocity = new Vector2(
                Mathf.Clamp(movementRoot.velocity.x, -stats.movementSpeed.statValue, stats.movementSpeed.statValue),
                Mathf.Clamp(movementRoot.velocity.y, -stats.movementSpeed.statValue, stats.movementSpeed.statValue)
            );
        }

        float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
}
