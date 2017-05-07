﻿//
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
    public enum ControlsType { WASDMovement, WASDTilt }

    public class PlayerControls : Singleton<PlayerControls> {
        public Tank tank;
        Transform tankTransform;
        Rigidbody tankRigidbody;

        [Space]
        public ControlsType controlsType;
        public bool inversedControls;
        public Vector2 rotationSensitivity = new Vector2(10, 10);
        Vector3 currentVelocity = Vector3.zero;

        public float accelerationMultiplier = 2;
        //public float autoSpinSpeed;

        bool autoSpinEnabled, autoFireEnabled;

        public void OnTankUpgrade(Tank tankBody) {
            tank = tankBody;
            tankTransform = tankBody.transform;
            tankRigidbody = tankBody.GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            if (tank.healthBar.health <= 0) Destroy(gameObject);

            //if (KeyBindings.instance.autoSpin.isDown) autoSpinEnabled = !autoSpinEnabled;
            //if (autoSpinEnabled) {
            //    transform.rotation *= Quaternion.Euler(0, 0, autoSpinSpeed);
            //} else {
            //    float angle = Vectors.Angle2D(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //    transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
            //}

            if (KeyBindings.instance.autoFire.isDown) {
                if (autoFireEnabled) {
                    foreach (Gun gun in tank.guns)
                        gun.StopFiring();
                }
                autoFireEnabled = !autoFireEnabled;
            }

            if (autoFireEnabled) {
                foreach (Gun gun in tank.guns)
                    gun.Fire();
            } else {
                if (KeyBindings.instance.fire.isPressed) {
                    foreach (Gun gun in tank.guns)
                        gun.Fire();
                } else if (KeyBindings.instance.fire.isUp) {
                    foreach (Gun gun in tank.guns)
                        gun.StopFiring();
                }
            }

            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            float movementSpeed = tank.stats.movementSpeed.value;

            float rotationX = 0, rotationY = 0;

            switch (controlsType) {
                case ControlsType.WASDMovement:
                    rotationX = Input.GetAxis("Mouse X") * rotationSensitivity.x;
                    rotationY = Input.GetAxis("Mouse Y") * rotationSensitivity.y;

                    currentVelocity = new Vector3(horizontalAxis, 0, verticalAxis);
                    break;

                case ControlsType.WASDTilt:
                    rotationX = horizontalAxis * rotationSensitivity.x;
                    rotationY = verticalAxis * rotationSensitivity.y;

                    currentVelocity = new Vector3(0, 0, Mathf.Clamp01(currentVelocity.z + Input.GetAxis("Speed")));
                    break;
            }

            if (inversedControls) rotationY = -rotationY;
            var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            var yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
            tankTransform.localRotation *= yQuaternion * xQuaternion;

            if (currentVelocity.sqrMagnitude > 1) currentVelocity.Normalize();
            tankRigidbody.AddRelativeForce(currentVelocity * movementSpeed * accelerationMultiplier);
            if (tankRigidbody.velocity.sqrMagnitude > movementSpeed * movementSpeed) tankRigidbody.velocity.Normalize();
        }
    }
}