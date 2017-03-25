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
    public class Player : Singleton<Player> {
        public Rigidbody2D movementRoot;
        public Transform rotationRoot;
        public Gun[] guns;

        StatsHolder stats;

        [Space]
        public float accelerationMultiplier = 2;
        public float autoSpinSpeed;

        bool autoSpinEnabled, autoFireEnabled;

        void Start() {
            stats = StatsHolder.instance;
        }

        void Update() {
            if (KeyBindings.instance.autoSpin.isDown) autoSpinEnabled = !autoSpinEnabled;
            if (autoSpinEnabled) {
                rotationRoot.localRotation *= Quaternion.Euler(0, 0, autoSpinSpeed);
            } else {
                float angle = AngleBetweenTwoPoints(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    transform.position);
                rotationRoot.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (KeyBindings.instance.autoFire.isDown) {
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
                if (KeyBindings.instance.fire.isPressed) {
                    foreach (Gun gun in guns)
                        if (!gun.isFiring)
                            gun.StartFiring();
                } else if (KeyBindings.instance.fire.isUp) {
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
