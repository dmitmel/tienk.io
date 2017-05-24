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

using System;
using UnityEngine;
using Tienkio.Utilities;
using Tienkio.Tanks;

namespace Tienkio.Data {
    [Serializable]
    public class TankUpgradeNode {
        [HideInInspector]
        public TankUpgradeTree tree;

        public string tankName;
        public Sprite previewIcon;
        public int minLevel;
        public Tank prefab;
        public int[] unlockedTanks;

        public TankUpgradeNode[] GetAvailableTanksForLevel(int currentLevel) {
            TankUpgradeNode[] availableTanks = new TankUpgradeNode[unlockedTanks.Length];
            int availableTanksLength = 0;

            foreach (var unlockedTankID in unlockedTanks) {
                TankUpgradeNode unlockedTank = tree.tankUpgradeTree[unlockedTankID];
                if (currentLevel >= unlockedTank.minLevel) {
                    availableTanks[availableTanksLength] = unlockedTank;
                    availableTanksLength++;
                }
            }

            TankUpgradeNode[] trimmedAvailableTanks = new TankUpgradeNode[availableTanksLength];
            Array.Copy(availableTanks, trimmedAvailableTanks, availableTanksLength);

            return trimmedAvailableTanks;
        }
    }

    public class TankUpgradeTree : PersistentSingleton<TankUpgradeTree> {
        public TankUpgradeNode[] tankUpgradeTree;

#if UNITY_EDITOR
        void OnValidate() {
            foreach (var node in tankUpgradeTree) node.tree = this;
        }
#endif
    }
}
