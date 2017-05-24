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

namespace Tienkio.Pools {
    public class PoolManager : MonoBehaviour {
        public PoolObject prefab;
        public int initialPoolSize;
        public float garbageCollectionDelay = 10;
        public float garbageCollectionInterval = 0.4f;

        Queue<PoolObject> pool;
        List<PoolObject> objectsToIntialize;

        float nextGCStart = -1, nextObjectRemove = -1;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        void Start() {
            pool = new Queue<PoolObject>(initialPoolSize);
            objectsToIntialize = new List<PoolObject>(initialPoolSize);
        }

        void LateUpdate() {
            foreach (PoolObject obj in objectsToIntialize) {
                obj.onGetFromPool.Invoke();
            }

            objectsToIntialize.Clear();
        }

        void Update() {
            float now = Time.time;

            if (pool.Count > 0) {
                if (nextGCStart > 0 && now >= nextGCStart) {
                    nextGCStart = -1;
                    nextObjectRemove = now + garbageCollectionInterval;
                }

                if (nextObjectRemove > 0 && now >= nextObjectRemove) {
                    nextObjectRemove = now + garbageCollectionInterval;
                    PoolObject poolObj = pool.Dequeue();
                    Destroy(poolObj.gameObject);
                }
            }
        }

        public PoolObject GetFromPool(Vector3 position, Quaternion rotation) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextObjectRemove = -1;

            if (pool.Count == 0) {
                PoolObject poolObj = Instantiate(prefab, position, rotation, transform);
                poolObj.pool = this;
                return poolObj;
            } else {
                PoolObject poolObj = pool.Dequeue();

                poolObj.gameObject.SetActive(true);

                Transform objTransform = poolObj.transform;
                objTransform.position = position;
                objTransform.rotation = rotation;

                objectsToIntialize.Add(poolObj);

                return poolObj;
            }
        }

        public void PutIntoPool(PoolObject poolObj) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextObjectRemove = -1;

            pool.Enqueue(poolObj);
            poolObj.gameObject.SetActive(false);
            objectsToIntialize.Remove(poolObj);
        }
    }
}
