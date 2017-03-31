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
    public class Tank : MonoBehaviour {
        public StatsHolder stats;
        public ScoreCounter scoreCounter;
        public Gun[] guns;

        [Space]
        public int damageComputationCycles = 20;
        public float bodyDamageForBulletMultiplier = 1;

        TankHealth healthBar;
        Rigidbody2D rigidbody;

        void Start() {
            healthBar = GetComponent<TankHealth>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void OnTriggerEnter2D(Collider2D collider) {
            if (collider.CompareTag("Bullet")) {
                var bullet = collider.GetComponent<Bullet>();
                if (bullet.tank == this) return;

                Rigidbody2D bulletRigidbody = collider.attachedRigidbody;

                Vector2 bulletDirection = bulletRigidbody.velocity.normalized;
                rigidbody.AddForce(bulletDirection * bullet.knockback, ForceMode2D.Impulse);

                float bulletDamagePerCycle = bullet.damage / damageComputationCycles;
                float bodyDamagePerCycle = stats.bodyDamage.value * bodyDamageForBulletMultiplier / damageComputationCycles;

                for (int cycle = 0; cycle < damageComputationCycles && healthBar.health > 0 && bullet.health > 0; cycle++) {
                    healthBar.health -= bulletDamagePerCycle;
                    bullet.health -= bodyDamagePerCycle;
                }

                if (healthBar.health <= 0) bullet.tank.scoreCounter.score += scoreCounter.score;
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider = collision.collider;
            if (collider.CompareTag("Tank")) {
                var tankHealthBar = collider.GetComponent<ObjectWithHealth>();
                var tank = collider.GetComponent<Tank>();

                tankHealthBar.health -= stats.bodyDamage.value;

                if (tankHealthBar.health <= 0) scoreCounter.score += tank.scoreCounter.score;
            }
        }
    }
}
