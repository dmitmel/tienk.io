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

namespace Tienkio.Utilities {
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        static T _instance;

        public static T instance {
            get { return _instance; }
        }

        public static bool instanceExists { get { return _instance != null; } }

        protected virtual void Awake() {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = (T) this;
        }

        void OnDestroy() {
            if (_instance == this)
                _instance = null;
        }
    }
}
