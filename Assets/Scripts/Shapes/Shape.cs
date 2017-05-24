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
using Tienkio.Utilities;
using Tienkio.Pools;
using Tienkio.Tanks;

namespace Tienkio.Shapes {
    public class Shape : MonoBehaviour {
        public float bodyDamage;
        public int score;
        public int damageComputationCycles = 20;
        public float bodyDamageForBullets;

        [Space]
        public PoolObject poolObject;

        [Space]
        public float randomRotationSpeed;
        public float randomMovementSpeed;
        new Rigidbody rigidbody;

        Health healthBar;

        void Awake() {
            healthBar = GetComponent<Health>();
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Start() {
            rigidbody.angularVelocity = new Vector3(
                Random.Range(randomRotationSpeed, randomRotationSpeed),
                0,
                Random.Range(randomRotationSpeed, randomRotationSpeed)
            );
            rigidbody.velocity = Random.insideUnitCircle * randomMovementSpeed;
            healthBar.health = healthBar.maxHealth;
        }

        void OnTriggerEnter(Collider collider) {
            if (collider.CompareTag("Bullet")) {
                var bullet = collider.GetComponent<Bullet>();
                Rigidbody bulletRigidbody = collider.attachedRigidbody;

                Vector3 bulletDirection = bulletRigidbody.velocity.normalized;
                rigidbody.AddForce(bulletDirection * bullet.knockback, ForceMode.Impulse);

                float bulletDamagePerCycle = bullet.damage / damageComputationCycles;
                float bodyDamagePerCycle = bodyDamageForBullets / damageComputationCycles;

                for (int cycle = 0; cycle < damageComputationCycles && healthBar.health > 0 && bullet.health > 0; cycle++) {
                    healthBar.health -= bulletDamagePerCycle;
                    bullet.health -= bodyDamagePerCycle;
                }

                if (healthBar.health <= 0 && bullet.tank.healthBar.health > 0) {
                    bullet.tank.scoreCounter.score += score;
                    ShapePool.instance.SpawnShape();
                    ShapePool.instance.DestroyShape(poolObject);
                }
            }
        }

        void OnCollisionEnter(Collision collision) {
            GameObject collider = collision.gameObject;
            if (collider.CompareTag("Tank")) {
                var tank = collider.GetComponent<TankController>();

                tank.healthBar.health -= bodyDamage;
                healthBar.health -= tank.stats.bodyDamage.Value;
                if (healthBar.health <= 0 && tank.healthBar.health > 0) {
                    tank.scoreCounter.score += score;
                    ShapePool.instance.SpawnShape();
                    ShapePool.instance.DestroyShape(poolObject);
                }
            }
        }
    }
}
