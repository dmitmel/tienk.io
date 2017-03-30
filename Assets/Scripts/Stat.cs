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
using UnityEngine.UI;

namespace Deepio {
    public class Stat : MonoBehaviour {
        public int statLevel;
        public int maxLevel = 7;
        public float baseValue, holderLevelBonus, statLevelBonus;

        public float statValue { get; private set; }

        [Space]
        public Button statButton;
        public Transform bars;

        StatsHolder holder;
        ScoreCounter scoreCounter;

        int lastStatLevel, lastHolderLevel, lastUpgradePoints;

        void Start() {
            holder = StatsHolder.instance;
            scoreCounter = ScoreCounter.instance;
            lastStatLevel = statLevel;
            lastHolderLevel = holder.level;
            statValue = ComputeValue();
            UpdateUI();
        }

        void Update() {
            if (lastStatLevel != statLevel) {
                if (statLevel < 0 || statLevel > maxLevel)
                    throw new ArgumentOutOfRangeException("statLevel", statLevel, $"statLevel must be >= 0 and <= {maxLevel}");
                lastStatLevel = statLevel;
                statValue = ComputeValue();
                UpdateUI();
            }

            if (lastHolderLevel != holder.level) {
                lastHolderLevel = holder.level;
                statValue = ComputeValue();
            }

            if (lastUpgradePoints != scoreCounter.upgradePoints) {
                lastUpgradePoints = scoreCounter.upgradePoints;
                statButton.interactable = IsInteractable();
            }

            if (!Player.isSingletonAlive) {
                statButton.interactable = false;
            }
        }

        float ComputeValue() {
            return baseValue + holderLevelBonus * holder.level + statLevelBonus * lastStatLevel;
        }

        public void Upgrade() {
            if (lastStatLevel < 7 && scoreCounter.upgradePoints > 0) {
                statLevel += 1;
                scoreCounter.upgradePoints -= 1;
            }
        }

        void UpdateUI() {
            for (int i = 0; i < statLevel && i < bars.childCount; i++) {
                Transform child = bars.GetChild(i);
                child.gameObject.SetActive(true);
            }

            for (int i = statLevel; i < maxLevel && i < bars.childCount; i++) {
                Transform child = bars.GetChild(i);
                child.gameObject.SetActive(false);
            }

            statButton.interactable = IsInteractable();
        }

        bool IsInteractable() {
            return scoreCounter.upgradePoints > 0 && statLevel < maxLevel;
        }
    }
}