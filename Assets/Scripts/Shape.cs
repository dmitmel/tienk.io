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
    public class Shape : MonoBehaviour {
        public float bodyDamage;
        public int score;
        public int damageComputationCycles = 20;
        public float bodyDamageForBullets;

        [Space]
        public GameObject parent;

        [Space]
        public float rotationSpeed;
        public float randomMovementSpeed;
        Vector2 randomVelocity;
        new Rigidbody2D rigidbody;

        ObjectWithHealth healthBar;

        void Awake() {
            healthBar = GetComponent<ObjectWithHealth>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Start() {
            rigidbody.angularVelocity = Mathf.Lerp(-1, 1, Random.value) * rotationSpeed;
            rigidbody.velocity = Random.insideUnitCircle * randomMovementSpeed;
            healthBar.health = healthBar.maxHealth;
        }

        void OnTriggerEnter2D(Collider2D collider) {
            if (collider.CompareTag("Bullet")) {
                var bullet = collider.GetComponent<Bullet>();
                Rigidbody2D bulletRigidbody = collider.attachedRigidbody;

                Vector2 bulletDirection = bulletRigidbody.velocity.normalized;
                rigidbody.AddForce(bulletDirection * bullet.knockback, ForceMode2D.Impulse);

                float bulletDamagePerCycle = bullet.damage / damageComputationCycles;
                float bodyDamagePerCycle = bodyDamageForBullets / damageComputationCycles;

                for (int cycle = 0; cycle < damageComputationCycles && healthBar.health > 0 && bullet.health > 0; cycle++) {
                    healthBar.health -= bulletDamagePerCycle;
                    bullet.health -= bodyDamagePerCycle;
                }

                if (healthBar.health <= 0) {
                    ShapeSpawner.instance.SpawnShape();
                    bullet.tank.scoreCounter.score += score;
                    ShapeSpawner.instance.DestroyShape(parent);
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider = collision.collider;
            if (collider.CompareTag("Tank")) {
                var tank = collider.GetComponent<Tank>();

                tank.healthBar.health -= bodyDamage;
                healthBar.health -= tank.stats.bodyDamage.value;
                if (healthBar.health <= 0) {
                    ShapeSpawner.instance.SpawnShape();
                    tank.scoreCounter.score += score;
                    ShapeSpawner.instance.DestroyShape(parent);
                }
            }
        }
    }
}
