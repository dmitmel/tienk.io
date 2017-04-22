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
    public class Player : Singleton<Player> {
        public GameObject parent;

        public float accelerationMultiplier = 2;
        public float autoSpinSpeed;

        new Transform transform;
        new Rigidbody2D rigidbody;
        Tank tank;

        bool autoSpinEnabled, autoFireEnabled;

        void Awake() {
            transform = base.transform;
            rigidbody = GetComponent<Rigidbody2D>();
            tank = GetComponent<Tank>();
        }

        void Update() {
            if (tank.healthBar.health <= 0) Destroy(parent);

            if (KeyBindings.instance.autoSpin.isDown) autoSpinEnabled = !autoSpinEnabled;
            if (autoSpinEnabled) {
                transform.rotation *= Quaternion.Euler(0, 0, autoSpinSpeed);
            } else {
                float angle = Vectors.Angle2D(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
            }

            if (KeyBindings.instance.autoFire.isDown) {
                autoFireEnabled = !autoFireEnabled;
                if (!autoFireEnabled) {
                    foreach (Gun gun in tank.guns)
                        gun.StopFiring();
                }
            }

            if (autoFireEnabled) {
                foreach (Gun gun in tank.guns)
                    if (!gun.isFiring)
                        gun.StartFiring();
            } else {
                if (KeyBindings.instance.fire.isPressed) {
                    foreach (Gun gun in tank.guns)
                        if (!gun.isFiring)
                            gun.StartFiring();
                } else if (KeyBindings.instance.fire.isUp) {
                    foreach (Gun gun in tank.guns)
                        gun.StopFiring();
                }
            }
        }

        void FixedUpdate() {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");
            var velocity = new Vector2(horizontalAxis, verticalAxis);
            if (velocity.sqrMagnitude > 1) velocity.Normalize();

            float movementSpeed = tank.stats.movementSpeed.value;

            rigidbody.AddForce(velocity * movementSpeed * accelerationMultiplier);
            rigidbody.velocity = new Vector2(
                Mathf.Clamp(rigidbody.velocity.x, -movementSpeed, movementSpeed),
                Mathf.Clamp(rigidbody.velocity.y, -movementSpeed, movementSpeed)
            );
        }
    }
}
