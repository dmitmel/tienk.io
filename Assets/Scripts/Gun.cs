﻿//
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
using System.Collections;

namespace Deepio {
    [System.Serializable]
    public class GunStatsMultipliers {
        public float bulletSpeed = 1, bulletPenetration = 1, bulletDamage = 1, reload = 1;
    }

    public class Gun : MonoBehaviour {
        [Space]
        public GameObject bullet;
        public float bulletOffset;
        public Vector2 bulletSize = Vector2.one;

        [Space]
        public float moveBackwardsOnShot;
        public int moveBackwardsSteps;
        bool isMovingBackwards;

        [Space]
        public GunStatsMultipliers statsMultipliers;
        public float bulletFlyTime, shootDelay, recoil, bulletKnockback, bulletSpread;

        [Space]
        public Tank tank;
        Rigidbody2D tankRigidbody;
        public float tankRelativeVelocityMultiplier = 1;

        public bool isFiring { get; private set; }

        float nextFire;
        float firingStartTime = -1;

        void Start() {
            tankRigidbody = tank.GetComponent<Rigidbody2D>();
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

        void Update() {
            if (isFiring) Fire();
        }

        public void Fire() {
            float now = Time.time;
            if (now >= nextFire) {
                nextFire = now + 1 / (statsMultipliers.reload * tank.stats.reload.value);

                Vector2 newBulletPosition = transform.position + transform.rotation * new Vector2(0, bulletOffset);
                float halfBulletSpread = bulletSpread / 2;
                Quaternion newBulletRotation = Quaternion.Euler(0, 0, Random.Range(-halfBulletSpread, halfBulletSpread));
                GameObject newBullet = Instantiate(bullet, newBulletPosition, newBulletRotation);

                Vector2 normalBulletVelocity = transform.rotation * newBulletRotation * Vector2.up *
                                                        (tank.stats.bulletSpeed.value * statsMultipliers.bulletSpeed);

                newBullet.GetComponent<Rigidbody2D>().velocity =
                             normalBulletVelocity + tankRigidbody.velocity * tankRelativeVelocityMultiplier;

                var newBulletController = newBullet.GetComponent<Bullet>();
                newBulletController.tank = tank;
                newBulletController.normalVelocity = normalBulletVelocity;
                newBulletController.damage = statsMultipliers.bulletDamage * tank.stats.bulletDamage.value;
                newBulletController.health = statsMultipliers.bulletPenetration * tank.stats.bulletPenetration.value;
                newBulletController.flyTime = bulletFlyTime;
                newBulletController.knockback = bulletKnockback;

                if (!isMovingBackwards) StartCoroutine(MoveBackwards());

                tankRigidbody.AddForce(transform.rotation * Vector2.down * recoil, ForceMode2D.Impulse);
            }
        }

        IEnumerator MoveBackwards() {
            isMovingBackwards = true;

            float step = 1f / moveBackwardsSteps;

            Vector2 startPosition = transform.localPosition;
            Vector2 endPosition = transform.localPosition - transform.localRotation * new Vector3(moveBackwardsOnShot, 0);

            for (float t = 0; t < 1; t += step) {
                transform.localPosition = Vector2.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            for (float t = 0; t < 1; t += step) {
                transform.localPosition = Vector2.Lerp(endPosition, startPosition, t);
                yield return null;
            }

            transform.localPosition = startPosition;

            isMovingBackwards = false;
        }
    }
}
