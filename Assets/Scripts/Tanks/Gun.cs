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

        [HideInInspector]
        public TankController tank;
        [HideInInspector]
        public Rigidbody tankRigidbody;

        PoolManager bulletPool;

        public int requiredBullets {
            get {
                return Mathf.CeilToInt(bulletFlyTime / (tank.stats.reload.Value * statsMultipliers.reload));
            }
        }

        bool isFiring;
        float nextFire;

        new Transform transform;

        void Awake() {
            transform = base.transform;
            bulletPool = BulletPool.poolManager;
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
            PoolObject bullet = bulletPool.GetFromPool();

            var bulletRenderer = bullet.GetComponent<MeshRenderer>();
            bulletRenderer.material = tank.bodyMaterial;

            var bulletController = bullet.GetComponent<Bullet>();
            var bulletRigidbody = bullet.GetComponent<Rigidbody>();

            float halfBulletSpread = bulletSpread / 2;
            var bulletRotation = transform.rotation * Quaternion.Euler(
                Random.Range(-halfBulletSpread, halfBulletSpread),
                0,
                Random.Range(-halfBulletSpread, halfBulletSpread)
            );

            var bulletTransform = bullet.transform;
            bulletTransform.position = transform.position + transform.rotation * new Vector3(0, bulletOffset, 0);
            bulletTransform.rotation = bulletRotation;
            Vector3 gunScale = transform.lossyScale;
            bullet.transform.localScale = new Vector3(gunScale.x * bulletSize, gunScale.x * bulletSize, gunScale.z * bulletSize);

            float bulletSpeed = tank.stats.bulletSpeed.Value * statsMultipliers.bulletSpeed;
            var bulletVelocity = bulletRotation * Vector3.up * bulletSpeed;

            bulletController.normalVelocity = bulletVelocity;
            bulletRigidbody.velocity = bulletVelocity + tankRigidbody.velocity;

            bulletController.tank = tank;
            bulletController.damage = statsMultipliers.bulletDamage * tank.stats.bulletDamage.Value;
            bulletController.health = statsMultipliers.bulletPenetration * tank.stats.bulletPenetration.Value;
            bulletController.knockback = bulletKnockback;
            bulletController.flyTime = bulletFlyTime;
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
