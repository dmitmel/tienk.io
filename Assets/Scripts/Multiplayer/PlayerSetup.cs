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
using Tienkio.Camera;
using Tienkio.Tanks;

namespace Tienkio.Multiplayer {
    public class PlayerSetup : NetworkBehaviour {
        PlayerControls playerControls;

        void Awake() {
            playerControls = GetComponent<PlayerControls>();
        }

        void Start() {
            if (isLocalPlayer) {
                playerControls.enabled = true;

                GameObject sceneCamera = MultiplayerCameras.instance.sceneCamera;
                sceneCamera.SetActive(false);

                GameObject playerCamera = MultiplayerCameras.instance.playerCamera;
                playerCamera.SetActive(true);
                var playerCameraController = playerCamera.GetComponent<CameraController>();
                playerCameraController.player = transform;
            }
        }

        void OnDisable() {
            if (isLocalPlayer) {
                GameObject sceneCamera = MultiplayerCameras.instance.sceneCamera;
                sceneCamera.SetActive(true);

                GameObject playerCamera = MultiplayerCameras.instance.playerCamera;
                playerCamera.SetActive(false);
            }
        }
    }
}
