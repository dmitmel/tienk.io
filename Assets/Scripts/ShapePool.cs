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
    public class ShapePool : MonoBehaviour {
        public GameObject shapePrefab;
        public int initialPoolSize;
        public float garbageCollectionDelay = 10;
        public float garbageCollectionInterval = 0.4f;

        Queue<GameObject> pool;
        List<GameObject> objectsToIntialize;

        float nextGCStart = -1, nextShapeRemove = -1;

        new Transform transform;

        void Awake() {
            transform = base.transform;
        }

        void Start() {
            pool = new Queue<GameObject>(initialPoolSize);
            objectsToIntialize = new List<GameObject>(initialPoolSize);
        }

        void LateUpdate() {
            foreach (GameObject shape in objectsToIntialize) {
                shape.GetComponentInChildren<Shape>().Start();
            }

            objectsToIntialize.Clear();
        }

        void Update() {
            float now = Time.time;

            if (pool.Count > 0) {
                if (nextGCStart > 0 && now >= nextGCStart) {
                    nextGCStart = -1;
                    nextShapeRemove = now + garbageCollectionInterval;
                }

                if (nextShapeRemove > 0 && now >= nextShapeRemove) {
                    nextShapeRemove = now + garbageCollectionInterval;
                    GameObject shape = pool.Dequeue();
                    Destroy(shape);
                }
            }
        }

        public GameObject GetFromPool(Vector2 position, Quaternion rotation) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextShapeRemove = -1;

            if (pool.Count == 0) {
                return Instantiate(shapePrefab, position, rotation, transform);
            } else {
                GameObject shape = pool.Dequeue();

                shape.SetActive(true);

                Transform shapeTransform = shape.transform;
                shapeTransform.position = position;
                shapeTransform.rotation = rotation;

                objectsToIntialize.Add(shape);

                return shape;
            }
        }

        public void PutIntoPool(GameObject shape) {
            nextGCStart = Time.time + garbageCollectionDelay;
            nextShapeRemove = -1;

            pool.Enqueue(shape);
            shape.SetActive(false);
            objectsToIntialize.Remove(shape);
        }
    }
}
