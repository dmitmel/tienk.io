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

namespace Deepio {
    public class CrasherAI : MonoBehaviour {
        public Rigidbody2D crasher;
        public float acceleration, movementSpeed;

        public float targetChooseInterval;

        List<Transform> enemies = new List<Transform>();
        Transform target;

        float nextTargetChooseTime;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        void OnTriggerEnter2D(Collider2D collider) {
            Transform colliderTransform = collider.transform;
            if (colliderTransform.CompareTag("Tank"))
                enemies.Add(collider.transform);
        }

        void Update() {
            if (enemies.Count > 0 || target == null) {
                float now = Time.time;
                if (now >= nextTargetChooseTime) {
                    nextTargetChooseTime = now + targetChooseInterval;
                    target = ChooseTarget();
                }
            }

            if (target != null) {
                crasher.rotation = Vectors.Angle2D(crasher.position, target.position) + 90;
                crasher.AddRelativeForce(Vector2.up * acceleration);
                crasher.velocity = new Vector2(
                    Mathf.Clamp(crasher.velocity.x, -movementSpeed, movementSpeed),
                    Mathf.Clamp(crasher.velocity.y, -movementSpeed, movementSpeed)
                );
            }
        }

        Transform ChooseTarget() {
            Transform currentTarget = null;
            float sqrDistanceToCurrentTarget = 0;

            bool isFirst = true;

            foreach (Transform enemy in enemies) {
                if (enemy == null) continue;

                if (isFirst) {
                    currentTarget = enemy;
                    sqrDistanceToCurrentTarget = (transform.position - currentTarget.position).sqrMagnitude;

                    isFirst = false;
                } else {
                    float sqrDistanceToEnemy = (transform.position - enemy.position).sqrMagnitude;

                    if (sqrDistanceToEnemy < sqrDistanceToCurrentTarget) {
                        currentTarget = enemy;
                        sqrDistanceToCurrentTarget = sqrDistanceToEnemy;
                    }
                }
            }

            return currentTarget;
        }

        void OnTriggerExit2D(Collider2D collider) {
            Transform colliderTransform = collider.transform;
            bool colliderIsEnemy = enemies.Remove(colliderTransform);
            if (colliderIsEnemy && colliderTransform == target) target = null;
        }
    }
}
