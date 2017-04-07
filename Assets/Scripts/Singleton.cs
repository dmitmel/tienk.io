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

namespace Deepio {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        static GameObject _instanceObject;
        static T _instance;

        public static T instance {
            get {
                if (_instance == null) {
                    T[] objects = FindObjectsOfType<T>();
                    int count = objects.Length;
                    if (count == 0) {
                        Debug.LogError($"[Singleton] There're no objects on scene with type of '{typeof(T)}'");
                    } else {
                        if (count > 1)
                            Debug.LogError($"[Singleton] There're multiple instances of '{typeof(T)}'");
                        _instance = objects[0];
                        _instanceObject = _instance.gameObject;
                    }
                }

                if (_instance == null)
                    Debug.LogError($"[Singleton] There're no objects on scene with type of '{typeof(T)}'");

                return _instance;
            }
        }

        public static GameObject singletonGameObject {
            get { return _instanceObject; }
        }
    }
}
