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

namespace Tienkio {
    [System.Serializable]
    public class TankUpgradeEvent : UnityEvent<Tank> { }

    public class TankUpgrader : MonoBehaviour {
        public ScoreCounter scoreCounter;
        public TankUpgradeEvent onTankUpgrade;
        public Material bodyMaterial;

        TankUpgradeNode currentTank;
        [HideInInspector]
        public TankUpgradeNode[] upgrades;

        protected Tank currentTankBody;

        void Start() {
            currentTank = TankUpgradeTree.instance.tankUpgradeTree[0];
            UpdateTank();
        }

        void Update() {
            upgrades = currentTank.GetAvailableTanksForLevel(scoreCounter.levelIndex);
        }

        public void UpgradeToTier(int tierIndex) {
            currentTank = upgrades[tierIndex];
            if (currentTank.prefab != null) UpdateTank();
        }

        void UpdateTank() {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (currentTankBody != null) {
                position = currentTankBody.transform.position;
                rotation = currentTankBody.transform.rotation;
                DestroyImmediate(currentTankBody.gameObject);
            }
            currentTankBody = Instantiate(currentTank.prefab, position, rotation, transform);

            currentTankBody.meshRenderer.material = bodyMaterial;

            onTankUpgrade.Invoke(currentTankBody);
        }
    }
}
