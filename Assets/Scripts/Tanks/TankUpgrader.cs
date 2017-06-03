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
using UnityEngine.Events;
using Tienkio.Data;

namespace Tienkio.Tanks {
    public class TankUpgrader : MonoBehaviour {
        public ScoreCounter scoreCounter;

        public UnityEvent onUpgrade, onNewUpgradesAvailable;

        [HideInInspector]
        public TankUpgradeNode currentUpgradeNode;
        [HideInInspector]
        public TankUpgradeNode[] availableUpgrades;

        TankController tankController;
        Rigidbody tankRigidbody;

        Tank currentTankBody;

        new Transform transform;

        int lastAvailableUpgradesCount;

        void Awake() {
            transform = base.transform;
            tankController = GetComponent<TankController>();
            tankRigidbody = GetComponent<Rigidbody>();
        }

        void Start() {
            currentUpgradeNode = TankUpgradeTree.instance.tankUpgradeTree[0];
            UpdateTank();
        }

        void Update() {
            availableUpgrades = currentUpgradeNode.GetAvailableTanksForLevel(scoreCounter.currentLevel.index);

            int availableUpgradesCount = availableUpgrades.Length;
            if (lastAvailableUpgradesCount != availableUpgradesCount) {
                lastAvailableUpgradesCount = availableUpgradesCount;
                onNewUpgradesAvailable.Invoke();
            }
        }

        public void UpgradeToTier(int tierIndex) {
            lastAvailableUpgradesCount = 0;
            currentUpgradeNode = availableUpgrades[tierIndex];
            if (currentUpgradeNode.prefab != null) UpdateTank();
            onUpgrade.Invoke();
        }

        void UpdateTank() {
            if (currentTankBody != null) DestroyImmediate(currentTankBody.gameObject);

            currentTankBody = Instantiate(currentUpgradeNode.prefab, transform);

            tankController.guns = currentTankBody.guns;
            foreach (Gun gun in tankController.guns) {
                gun.tank = tankController;
                gun.tankRigidbody = tankRigidbody;
            }

            var bodyRenderer = currentTankBody.GetComponent<MeshRenderer>();
            bodyRenderer.material = tankController.bodyMaterial;
        }
    }
}
