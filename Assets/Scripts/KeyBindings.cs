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
    [System.Serializable]
    public class KeyBinding {
        public KeyCode key1;
        public KeyCode key2;

        public bool isPressed {
            get { return Input.GetKey(key1) || Input.GetKey(key2); }
        }
        public bool isDown {
            get { return Input.GetKeyDown(key1) || Input.GetKeyDown(key2); }
        }
        public bool isUp {
            get { return Input.GetKeyUp(key1) || Input.GetKeyUp(key2); }
        }
    }

    public class KeyBindings : Singleton<KeyBindings> {
        public KeyBinding fire, autoFire, autoSpin;
    }
}
