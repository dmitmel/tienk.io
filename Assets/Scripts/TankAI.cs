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
using System.Linq;
using System;

namespace Tienkio {
    public class TankAI : MonoBehaviour {
        public Tank tank;
        Rigidbody tankRigidbody;
        Transform tankTransform;
        public float accelerationMultiplier = 2;
        public float minSqrDistance;

        [Space]
        public string[] objectsAttackPriority;
        public float targetChooseInterval;

        [Space]
        public Vector3 spawnFieldMin, spawnFieldMax;

        List<Transform> enemies = new List<Transform>();
        Transform target;

        float nextTargetChooseTime;
        int lastUpgradePoints;

        new Transform transform;

        void Awake() {
            transform = base.transform;
            tankRigidbody = tank.GetComponent<Rigidbody>();
            tankTransform = tank.transform;
        }

        void Start() {
            tank.transform.position = new Vector3(
                UnityEngine.Random.Range(spawnFieldMin.x, spawnFieldMax.x),
                UnityEngine.Random.Range(spawnFieldMin.y, spawnFieldMax.y),
                UnityEngine.Random.Range(spawnFieldMin.z, spawnFieldMax.z)
            );
        }

        void Respawn() {
            Start();

            tank.scoreCounter.OnRespawn();
            tank.stats.OnRespawn();
            tank.healthBar.OnRespawn();
        }

        void OnTriggerEnter(Collider collider) {
            if (collider.gameObject != tank.gameObject && objectsAttackPriority.Contains(collider.tag))
                enemies.Add(collider.transform);
        }

        void Update() {
            if (tank.healthBar.health <= 0) Respawn();

            for (int i = 0; i < tank.scoreCounter.upgradePoints; i++) {
                Stat stat = RandomStat();
                stat.Upgrade();
            }

            if (enemies.Count > 0 || target == null) {
                float now = Time.time;
                if (now >= nextTargetChooseTime) {
                    nextTargetChooseTime = now + targetChooseInterval;
                    target = ChooseTarget();

                    foreach (Gun gun in tank.guns)
                        if (!gun.isFiring)
                            gun.StartFiring();
                }
            }

            if (target != null) {
                float movementSpeed = tank.stats.movementSpeed.value;

                float sqrDistanceToTarget = (transform.position - target.position).sqrMagnitude;
                float distanceStabilization = sqrDistanceToTarget < minSqrDistance ? -1 : 1;

                tankTransform.LookAt(target);
                tankRigidbody.AddRelativeForce(Vector3.forward * movementSpeed * accelerationMultiplier * distanceStabilization);
                if (tankRigidbody.velocity.sqrMagnitude > movementSpeed * movementSpeed) tankRigidbody.velocity.Normalize();
            } else {
                foreach (Gun gun in tank.guns)
                    gun.StopFiring();
            }
        }

        Stat RandomStat() {
            switch (UnityEngine.Random.Range(0, 6)) {
                case 0:
                    return tank.stats.healthRegen;
                case 1:
                    return tank.stats.maxHealth;
                case 2:
                    return tank.stats.bodyDamage;
                case 3:
                    return tank.stats.bulletSpeed;
                case 4:
                    return tank.stats.bulletPenetration;
                case 5:
                    return tank.stats.bulletDamage;
                case 6:
                    return tank.stats.reload;
                case 7:
                    return tank.stats.movementSpeed;
                default:
                    throw new NotImplementedException();
            }
        }

        Transform ChooseTarget() {
            Transform currentTarget = null;
            int currentTargetPriority = 0;
            float currentTargetHealth = 0;
            float sqrDistanceToCurrentTarget = 0;

            bool isFirst = true;

            foreach (Transform enemy in enemies) {
                if (enemy == null) continue;

                if (isFirst) {
                    currentTarget = enemy;
                    currentTargetPriority = GetAttackPriorityFor(currentTarget);
                    currentTargetHealth = enemy.GetComponent<ObjectWithHealth>().health;
                    sqrDistanceToCurrentTarget = (transform.position - currentTarget.position).sqrMagnitude;

                    isFirst = false;
                } else {
                    int enemyPriority = GetAttackPriorityFor(enemy);
                    float enemyHealth = enemy.GetComponent<ObjectWithHealth>().health;
                    float sqrDistanceToEnemy = (transform.position - enemy.position).sqrMagnitude;

                    if (enemyPriority > currentTargetPriority ||
                        (enemyPriority == currentTargetPriority && enemyHealth < currentTargetHealth) ||
                        (enemyPriority == currentTargetPriority && enemyHealth == currentTargetHealth &&
                            sqrDistanceToEnemy < sqrDistanceToCurrentTarget)) {
                        currentTarget = enemy;
                        sqrDistanceToCurrentTarget = sqrDistanceToEnemy;
                        currentTargetPriority = enemyPriority;
                    }
                }
            }

            return currentTarget;
        }

        int GetAttackPriorityFor(Transform enemy) {
            return Array.IndexOf(objectsAttackPriority, enemy.tag);
        }

        void OnTriggerExit(Collider collider) {
            Transform colliderTransform = collider.transform;
            bool colliderIsEnemy = enemies.Remove(colliderTransform);
            if (colliderIsEnemy && colliderTransform == target) target = null;
        }
    }
}
