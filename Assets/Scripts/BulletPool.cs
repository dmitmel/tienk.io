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
using System.Collections.Generic;

namespace Deepio {
    public class BulletPool : Singleton<BulletPool> {
        public GameObject bulletPrefab;
        public int initialPoolSize;
        public float garbageCollectionDelay;
        public float garbageCollectionInterval;

        Queue<GameObject> pool;
        List<GameObject> objectsToIntialize;

        float nextGCStart = -1, nextBulletRemove = -1;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        void Start() {
            pool = new Queue<GameObject>(initialPoolSize);
            objectsToIntialize = new List<GameObject>(initialPoolSize);
        }

        void LateUpdate() {
            foreach (GameObject bullet in objectsToIntialize) {
                bullet.GetComponent<Bullet>().Start();
            }

            objectsToIntialize.Clear();
        }

        void Update() {
            float now = Time.time;

            if (pool.Count > 0) {
                if (nextGCStart > 0 && now >= nextGCStart) {
                    nextGCStart = -1;
                    nextBulletRemove = now + garbageCollectionInterval;
                }

                if (nextBulletRemove > 0 && now >= nextBulletRemove) {
                    nextBulletRemove = now + garbageCollectionInterval;
                    GameObject bullet = pool.Dequeue();
                    Destroy(bullet);
                }
            }
        }

        public GameObject InstantiateBullet(Vector2 position, Quaternion rotation) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextBulletRemove = -1;

            if (pool.Count == 0) {
                return Instantiate(bulletPrefab, position, rotation, transform);
            } else {
                GameObject bullet = pool.Dequeue();

                bullet.SetActive(true);

                Transform bulletTransform = bullet.transform;
                bulletTransform.position = position;
                bulletTransform.rotation = rotation;

                objectsToIntialize.Add(bullet);

                return bullet;
            }
        }

        public void DestroyBullet(GameObject bullet) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextBulletRemove = -1;

            pool.Enqueue(bullet);
            bullet.SetActive(false);
            objectsToIntialize.Remove(bullet);
        }
    }
}
