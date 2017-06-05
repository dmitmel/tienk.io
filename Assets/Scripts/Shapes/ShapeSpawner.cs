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
using Tienkio.Pools;
using System.Collections.Generic;
using Tienkio.Utilities;

namespace Tienkio.Shapes {
    [System.Serializable]
    public class ShapeSpawnerPool {
        public string name;
        [Range(0, 1)]
        public float chance;
        public PoolManager poolManager;
    }

    public class ShapeSpawner : Singleton<ShapeSpawner> {
        public ShapeSpawnerPool[] pools;
        public int shapesCount;

#if UNITY_EDITOR
        void OnValidate() {
            if (pools.Length > 0) {
                for (int i = 0; i < pools.Length - 1; i++) {
                    var shapeSpawnerPool = pools[i];
                    shapeSpawnerPool.chance = Mathf.Clamp01(shapeSpawnerPool.chance);
                }

                ShapeSpawnerPool lastShapeSpawnerPool = pools[pools.Length - 1];
                lastShapeSpawnerPool.chance = 1;
            }

            shapesCount = Mathf.Max(shapesCount, 0);
        }
#endif

        void Start() {
            for (int i = 0; i < shapesCount; i++) SpawnShape();
        }

        public void SpawnShape() {
            PoolManager pool = SelectPool();
            if (pool != null) pool.GetFromPool();
        }

        public PoolManager SelectPool() {
            List<ShapeSpawnerPool> availablePools = new List<ShapeSpawnerPool>(pools.Length);

            foreach (ShapeSpawnerPool shapeSpawnerPool in pools)
                if (shapeSpawnerPool.poolManager.HasObjects)
                    availablePools.Add(shapeSpawnerPool);

            if (availablePools.Count > 0) {
                for (int i = 0; i < availablePools.Count - 1; i++) {
                    var shapeSpawnerPool = availablePools[i];
                    float random = Random.value;
                    if (random <= shapeSpawnerPool.chance) return shapeSpawnerPool.poolManager;
                }

                int lastAvailablePoolIndex = availablePools.Count - 1;
                ShapeSpawnerPool lastAvailablePool = availablePools[lastAvailablePoolIndex];
                return lastAvailablePool.poolManager;
            } else {
                return null;
            }
        }
    }
}
