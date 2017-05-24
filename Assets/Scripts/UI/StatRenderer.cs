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
using UnityEngine.UI;
using Tienkio.Tanks;

namespace Tienkio.UI {
    public class StatRenderer : MonoBehaviour {
        public Stat stat;
        public Button upgradeButton;
        public Transform upgradeBars;

        void Start() {
            UpdateBars();
            UpdateButton();
        }

        void FixedUpdate() {
            if (!PlayerControls.instanceExists)
                upgradeButton.interactable = false;
        }

        public void UpdateBars() {
            for (int i = 0; i < stat.level && i < upgradeBars.childCount; i++) {
                Transform child = upgradeBars.GetChild(i);
                child.gameObject.SetActive(true);
            }

            for (int i = stat.level; i < upgradeBars.childCount; i++) {
                Transform child = upgradeBars.GetChild(i);
                child.gameObject.SetActive(false);
            }

            UpdateButton();
        }

        public void UpdateButton() {
            upgradeButton.interactable = stat.scoreCounter.upgradePoints > 0 && stat.level < stat.maxLevel;
        }
    }
}
