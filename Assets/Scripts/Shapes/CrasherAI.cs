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

using System.Collections.Generic;
using UnityEngine;

namespace Tienkio {
    public class CrasherAI : MonoBehaviour {
        public Rigidbody crasher;
        Transform crasherTransform;
        public float acceleration, movementSpeed;

        public float targetChooseInterval;

        List<Transform> enemies = new List<Transform>();
        Transform target;

        float nextTargetChooseTime;

        new Transform transform;

        void Awake() {
            transform = base.transform;
            crasherTransform = crasher.transform;
        }

        void OnTriggerEnter(Collider collider) {
            Transform colliderTransform = collider.transform;
            if (colliderTransform.CompareTag("Tank"))
                enemies.Add(collider.transform);
        }

        void FixedUpdate() {
            if (enemies.Count > 0 || target == null || !target.gameObject.activeInHierarchy) {
                float now = Time.time;
                if (now >= nextTargetChooseTime) {
                    nextTargetChooseTime = now + targetChooseInterval;
                    target = ChooseTarget();
                }
            }

            if (target != null) {
                crasherTransform.LookAt(target);
                crasher.AddRelativeForce(Vector3.forward * acceleration);
                if (crasher.velocity.sqrMagnitude > movementSpeed * movementSpeed) crasher.velocity.Normalize();
            }

            transform.position = crasherTransform.position;
        }

        Transform ChooseTarget() {
            Transform currentTarget = null;
            float distanceToCurrentTarget = 0;

            bool isFirst = true;

            for (int i = 0; i < enemies.Count; i++) {
                Transform enemy = enemies[i];

                if (enemy == null || !enemy.gameObject.activeInHierarchy) {
                    enemies.Remove(enemy);
                    i--;
                    continue;
                }

                if (isFirst) {
                    currentTarget = enemy;
                    distanceToCurrentTarget = (transform.position - currentTarget.position).sqrMagnitude;

                    isFirst = false;
                } else {
                    float distanceToEnemy = (transform.position - enemy.position).sqrMagnitude;

                    if (distanceToEnemy < distanceToCurrentTarget) {
                        currentTarget = enemy;
                        distanceToCurrentTarget = distanceToEnemy;
                    }
                }
            }

            return currentTarget;
        }

        void OnTriggerExit(Collider collider) {
            Transform colliderTransform = collider.transform;
            bool colliderIsEnemy = enemies.Remove(colliderTransform);
            if (colliderIsEnemy && colliderTransform == target) {
                target = ChooseTarget();
                nextTargetChooseTime = Time.time + targetChooseInterval;
            }
        }
    }
}
