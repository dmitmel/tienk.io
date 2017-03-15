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
using System.Collections;

namespace Deepio {
    [System.Serializable]

    public class Gun : MonoBehaviour {
        public GameObject bullet;
        public float bulletOffset;
        public Vector2 bulletSize = Vector2.one;

        [Space]
        public float moveBackwardsOnShot;
        public int moveBackwardsSteps;
        bool isMovingBackwards;

        [Space]
        public StatsHolder stats;
        public float bulletSpeed, bulletPenetration, bulletDamage, reload;
        public float bulletFlyTime, shootDelay, recoil, knockback;

        [Space]
        public Rigidbody2D tank;
        public float tankRelativeVelocityMultiplier = 1;

        public bool isFiring { get; private set; }

        float nextFire;
        float firingStartTime = -1;

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
                nextFire = now + 1 / (reload * stats.reload.statValue);

                GameObject newBullet = Instantiate(
                    bullet,
                    transform.position + transform.rotation * new Vector2(bulletOffset, 0),
                    Quaternion.identity);

                newBullet.GetComponent<Rigidbody2D>().velocity =
                             (Vector2) (transform.rotation * Vector2.right * (stats.bulletSpeed.statValue * bulletSpeed)) +
                             tank.velocity * tankRelativeVelocityMultiplier;

                var newBulletController = newBullet.GetComponent<Bullet>();
                newBulletController.damage = bulletDamage * stats.bulletDamage.statValue;
                newBulletController.health = bulletPenetration * stats.bulletPenetration.statValue;
                newBulletController.flyTime = bulletFlyTime;
                newBulletController.knockback = knockback;

                if (!isMovingBackwards) StartCoroutine(MoveBackwards());

                tank.AddForce(transform.rotation * Vector2.right * -recoil, ForceMode2D.Impulse);
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
