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
        [SerializeField]
        float _health;
        public float bodyDamage;
        public int score;
        public int damageComputationCycles;

        [Space]
        public GameObject parent;
        public GameObject healthBarObject;

        [Space]
        public float rotationSpeed;
        public float randomMovementSpeed;
        public float knockback;
        Vector2 randomVelocity;
        Rigidbody2D rigidbody;

        Vector3 healthBarOffset;
        ObjectWithHealth health;

        void Start() {
            health = GetComponent<ObjectWithHealth>();
            healthBarOffset = healthBarObject.transform.localPosition;
            health.health = health.maxHealth = _health;

            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.angularVelocity = Mathf.Lerp(-1, 1, Random.value) * rotationSpeed;
            rigidbody.velocity = Random.insideUnitCircle * randomMovementSpeed;
        }

        void Damage(float damage) {
            health.Damage(damage);
            if (health.health <= 0) {
                ShapeSpawner.instance.SpawnShape();
                Destroy(parent);
            }
        }

        void OnTriggerEnter2D(Collider2D collider) {
            if (collider.CompareTag("Bullet")) {
                var bullet = collider.GetComponent<Bullet>();
                Rigidbody2D bulletRigidbody = collider.attachedRigidbody;

                Vector2 bulletDirection = bulletRigidbody.velocity.normalized;
                rigidbody.AddForce(bulletDirection * bullet.knockback, ForceMode2D.Impulse);

                float bulletDamagePerCycle = bullet.damage / damageComputationCycles;
                float bodyDamagePerCycle = bodyDamage / damageComputationCycles;

                for (int cycle = 0; cycle < damageComputationCycles && health.health > 0 && bullet.health > 0; cycle++) {
                    Damage(bulletDamagePerCycle);
                    bullet.health -= bodyDamagePerCycle;
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider = collision.collider;
            if (collider.CompareTag("Player")) {
                var player = collider.GetComponent<ObjectWithHealth>();
                var playerStats = collider.GetComponent<StatsHolder>();
                player.Damage(bodyDamage);
                Damage(playerStats.bodyDamage.statValue);
            }

            Rigidbody2D colliderRigidbody = collider.attachedRigidbody;
             if (rigidbody != null) {
                 Vector2 colliderDirection = colliderRigidbody.velocity.normalized;
                 colliderRigidbody.AddForce(colliderDirection * -knockback);
                 rigidbody.AddForce(colliderDirection * knockback);
             }
        }

        void Update() {
            healthBarObject.transform.localPosition = healthBarOffset + transform.localPosition;
        }
    }
}
