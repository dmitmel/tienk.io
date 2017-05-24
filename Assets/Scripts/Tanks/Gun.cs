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

using System.Collections;
using UnityEngine;
using Tienkio.Pools;

namespace Tienkio.Tanks {
    [System.Serializable]
    public class GunStatsMultipliers {
        public float bulletSpeed = 1, bulletPenetration = 1, bulletDamage = 1, reload = 1;
    }

    public class Gun : MonoBehaviour {
        public float bulletOffset;
        public float bulletSize = 1;

        [Space]
        public float moveBackwardsOnShot;
        public int moveBackwardsSteps;
        bool isMovingBackwards;

        [Space]
        public GunStatsMultipliers statsMultipliers;
        public float bulletFlyTime, shootDelay, recoil, bulletKnockback, bulletSpread;

        public TankController tank;
        public Rigidbody tankRigidbody;

        bool isFiring;
        float nextFire;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        public void StopFiring() {
            isFiring = false;
        }

        void FixedUpdate() {
            if (isFiring) Fire();
        }

        public void Fire() {
            float now = Time.time;

            if (isFiring) {
                if (now >= nextFire) {
                    nextFire = now + (statsMultipliers.reload * tank.stats.reload.Value);

                    SpawnBullet();

                    if (!isMovingBackwards) StartCoroutine(MoveBackwards());

                    tankRigidbody.AddForce(transform.rotation * Vector3.down * recoil, ForceMode.Impulse);
                }
            } else {
                if (now >= nextFire)
                    nextFire = now + shootDelay * (statsMultipliers.reload * tank.stats.reload.Value);
                isFiring = true;
            }
        }

        void SpawnBullet() {
            Vector3 newBulletPosition = transform.position + transform.rotation * new Vector3(0, bulletOffset, 0);
            PoolObject newBullet = BulletPool.instance.GetFromPool(newBulletPosition, Quaternion.identity);

            Vector3 scale = transform.lossyScale;
            newBullet.transform.localScale = new Vector3(scale.x * bulletSize, scale.x * bulletSize, scale.z * bulletSize);

            var newBulletRenderer = newBullet.GetComponent<MeshRenderer>();
            newBulletRenderer.material = tank.bodyMaterial;

            var newBulletController = newBullet.GetComponent<Bullet>();
            var newBulletRigidbody = newBullet.GetComponent<Rigidbody>();

            float halfBulletSpread = bulletSpread / 2;
            var newBulletRotation = transform.rotation * Quaternion.Euler(
                Random.Range(-halfBulletSpread, halfBulletSpread),
                0,
                Random.Range(-halfBulletSpread, halfBulletSpread)
            );

            float bulletSpeed = tank.stats.bulletSpeed.Value * statsMultipliers.bulletSpeed;
            var bulletVelocity = newBulletRotation * Vector3.up * bulletSpeed;

            newBulletController.normalVelocity = bulletVelocity;
            newBulletRigidbody.velocity = bulletVelocity + tankRigidbody.velocity;

            newBulletController.tank = tank;
            newBulletController.damage = statsMultipliers.bulletDamage * tank.stats.bulletDamage.Value;
            newBulletController.health = statsMultipliers.bulletPenetration * tank.stats.bulletPenetration.Value;
            newBulletController.knockback = bulletKnockback;
            newBulletController.flyTime = bulletFlyTime;
        }

        IEnumerator MoveBackwards() {
            isMovingBackwards = true;

            float step = 1f / moveBackwardsSteps;

            var startPosition = transform.localPosition;
            var endPosition = transform.localPosition - transform.localRotation * new Vector3(0, moveBackwardsOnShot, 0);

            for (float t = 0; t < 1; t += step) {
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            for (float t = 0; t < 1; t += step) {
                transform.localPosition = Vector3.Lerp(endPosition, startPosition, t);
                yield return null;
            }

            transform.localPosition = startPosition;

            isMovingBackwards = false;
        }
    }
}
