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
using System;

namespace Tienkio.Pools {
    public class PoolManager : MonoBehaviour {
        public PoolObject prefab;
        public int poolSize;

        List<PoolObject> inactiveObjects, activeObjects;

        public bool HasObjects { get { return inactiveObjects.Count > 0; } }

#if UNITY_EDITOR
        void OnValidate() {
            poolSize = Math.Max(0, poolSize);
        }
#endif

        void Awake() {
            inactiveObjects = new List<PoolObject>(poolSize);
            activeObjects = new List<PoolObject>(poolSize);
        }

        void Start() {
            for (int i = 0; i < poolSize; i++) Create();
        }

        public void Alloc(int objects) {
            if (objects > 0) poolSize += objects;
        }

        void Create() {
            PoolObject poolObject = Instantiate(prefab, transform);
            poolObject.gameObject.SetActive(false);
            poolObject.pool = this;

            inactiveObjects.Add(poolObject);
        }

        public void Free(int objects) {
            if (objects > 0) poolSize = Math.Max(poolSize - objects, 0);
        }

        public void Clear() {
            poolSize = 0;
        }

        void Remove() {
            int lastInactiveObjectIndex = inactiveObjects.Count - 1;
            PoolObject poolObject = inactiveObjects[lastInactiveObjectIndex];
            inactiveObjects.RemoveAt(lastInactiveObjectIndex);

            DestroyImmediate(poolObject.gameObject);
        }

        void Update() {
            poolSize = Math.Max(poolSize, 0);

            int actualPoolSize = inactiveObjects.Count + activeObjects.Count;
            if (actualPoolSize < poolSize) {
                for (; actualPoolSize < poolSize; actualPoolSize++) Create();
            } else if (actualPoolSize > poolSize) {
                for (; actualPoolSize > poolSize && inactiveObjects.Count > 0; actualPoolSize--) Remove();
            }
        }

        public PoolObject GetFromPool() {
            if (inactiveObjects.Count > 0) {
                int lastInactiveObjectIndex = inactiveObjects.Count - 1;
                PoolObject poolObject = inactiveObjects[lastInactiveObjectIndex];
                inactiveObjects.RemoveAt(lastInactiveObjectIndex);
                activeObjects.Add(poolObject);

                poolObject.gameObject.SetActive(true);

                return poolObject;
            }

            return null;
        }

        public void PutIntoPool(PoolObject poolObject) {
            if (ReferenceEquals(poolObject.pool, this)) {
                bool objectIsInPool = activeObjects.Remove(poolObject);
                if (objectIsInPool) {
                    inactiveObjects.Add(poolObject);
                    poolObject.gameObject.SetActive(false);
                }
            }
        }
    }
}
