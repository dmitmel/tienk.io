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
using Tienkio.Data;
using Tienkio.Tanks;

namespace Tienkio.UI {
    [System.Serializable]
    public class TankUpgradeButton {
        public GameObject button;
        public Text tankName;
        public Image previewIcon;
    }

    public class TankUpgradeRenderer : MonoBehaviour {
        public TankUpgrader upgrader;
        public TankUpgradeButton[] upgradeButtons;

        int lastUpgradesCount;

        void Start() {
            UpdateUI();
        }

        void Update() {
            int upgradesCount = upgrader.upgrades.Length;
            if (upgradesCount != lastUpgradesCount) {
                lastUpgradesCount = upgradesCount;
                UpdateUI();
            }
        }

        void UpdateUI() {
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(lastUpgradesCount > 0);
            }

            if (lastUpgradesCount > 0) {
                for (int i = 0; i < lastUpgradesCount && i < upgradeButtons.Length; i++) {
                    TankUpgradeButton upgradeButton = upgradeButtons[i];
                    upgradeButton.button.SetActive(true);
                    TankUpgradeNode upgrade = upgrader.upgrades[i];
                    upgradeButton.tankName.text = upgrade.tankName;
                    upgradeButton.previewIcon.sprite = upgrade.previewIcon;
                }

                for (int i = lastUpgradesCount; i < upgradeButtons.Length; i++) {
                    TankUpgradeButton upgradeButton = upgradeButtons[i];
                    upgradeButton.button.SetActive(false);
                }
            }
        }

        public void IgnoreUpgrades() {
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }
    }
}
