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

namespace Tienkio {
    [System.Serializable]
    public class GunStatsMultipliers {
        public float bulletSpeed = 1, bulletPenetration = 1, bulletDamage = 1, reload = 1;
    }

    public class Gun : MonoBehaviour {
        public PoolManager bulletPool;
        public float bulletOffset;
        public Vector3 bulletSize = Vector3.one;

        [Space]
        public float moveBackwardsOnShot;
        public int moveBackwardsSteps;
        bool isMovingBackwards;

        [Space]
        public GunStatsMultipliers statsMultipliers;
        public float bulletFlyTime, shootDelay, recoil, bulletKnockback, bulletSpread;

        [Space]
        public Tank tank;
        Rigidbody tankRigidbody;
        public float tankRelativeVelocityMultiplier = 1;

        public bool isFiring { get; private set; }

        float nextFire;
        float firingStartTime = -1;

        new Transform transform;

        void Awake() {
            transform = base.transform;
            tankRigidbody = tank.GetComponent<Rigidbody>();
        }

        public void StartFiring() {
            float now = Time.time;
            if (firingStartTime < 0 && !isFiring) {
                firingStartTime = now + shootDelay;
            } else if ((isFiring || now >= firingStartTime) && now >= nextFire) {
                isFiring = true;
                firingStartTime = -1;
                nextFire = now;
            }
        }

        public void StopFiring() {
            isFiring = false;
            firingStartTime = -1;
        }

        void FixedUpdate() {
            if (isFiring) Fire();
        }

        public void Fire() {
            float now = Time.time;
            if (now >= nextFire) {
                nextFire = now + 1 / (statsMultipliers.reload * tank.stats.reload.value);

                Vector3 newBulletPosition = transform.position + transform.rotation * new Vector3(0, bulletOffset, 0);
                PoolObject newBullet = bulletPool.GetFromPool(newBulletPosition, Quaternion.identity);

                var newBulletController = newBullet.GetComponent<Bullet>();
                var newBulletRigidbody = newBullet.GetComponent<Rigidbody>();

                float halfBulletSpread = bulletSpread / 2;
                var newBulletRotation = transform.rotation * Quaternion.Euler(
                    Random.Range(-halfBulletSpread, halfBulletSpread),
                    0,
                    Random.Range(-halfBulletSpread, halfBulletSpread)
                );

                float bulletSpeed = tank.stats.bulletSpeed.value * statsMultipliers.bulletSpeed;
                var normalBulletVelocity = newBulletRotation * Vector3.up * bulletSpeed;

                newBulletController.normalVelocity = normalBulletVelocity;
                newBulletRigidbody.velocity = normalBulletVelocity + tankRigidbody.velocity * tankRelativeVelocityMultiplier;

                newBulletController.tank = tank;
                newBulletController.damage = statsMultipliers.bulletDamage * tank.stats.bulletDamage.value;
                newBulletController.health = statsMultipliers.bulletPenetration * tank.stats.bulletPenetration.value;
                newBulletController.knockback = bulletKnockback;
                newBulletController.flyTime = bulletFlyTime;

                if (!isMovingBackwards) StartCoroutine(MoveBackwards());

                tankRigidbody.AddForce(transform.rotation * Vector3.down * recoil, ForceMode.Impulse);
            }
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
