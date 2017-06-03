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
        public ScoreCounter scoreCounter;
        public TankUpgradeButton[] upgradeButtons;

        public void UpdateUpgradeButtons() {
            int availableUpgradesCount = upgrader.availableUpgrades.Length;

            if (availableUpgradesCount > 0) {
                for (int i = 0; i < availableUpgradesCount && i < upgradeButtons.Length; i++) {
                    TankUpgradeButton upgradeButton = upgradeButtons[i];
                    upgradeButton.button.SetActive(true);

                    TankUpgradeNode upgrade = upgrader.availableUpgrades[i];
                    upgradeButton.tankName.text = upgrade.tankName;
                    upgradeButton.previewIcon.sprite = upgrade.previewIcon;
                }

                for (int i = availableUpgradesCount; i < upgradeButtons.Length; i++) {
                    TankUpgradeButton upgradeButton = upgradeButtons[i];
                    upgradeButton.button.SetActive(false);
                }
            }
        }
    }
}
