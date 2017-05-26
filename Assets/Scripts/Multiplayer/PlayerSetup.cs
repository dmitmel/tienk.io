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
using UnityEngine.Networking;

namespace Tienkio.Multiplayer {
    public class PlayerSetup : NetworkBehaviour {
        public GameObject[] gameObjectsToEnable;
        public Behaviour[] behavioursToEnable;

        GameObject sceneCamera;

        void Start() {
            if (isLocalPlayer) {
                sceneCamera = UnityEngine.Camera.main.gameObject;
                sceneCamera.SetActive(false);
                foreach (var gameObjectToEnable in gameObjectsToEnable) gameObjectToEnable.SetActive(true);
                foreach (var behaviourToEnable in behavioursToEnable) behaviourToEnable.enabled = true;
            }
        }

        void OnDisable() {
            if (isLocalPlayer) {
                sceneCamera.SetActive(true);
                foreach (var gameObjectToEnable in gameObjectsToEnable) gameObjectToEnable.SetActive(false);
                foreach (var behaviourToEnable in behavioursToEnable) behaviourToEnable.enabled = false;
            }
        }
    }
}
