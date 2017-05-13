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
    public class Stat : MonoBehaviour {
        public ScoreCounter scoreCounter;
        public int level;
        public int maxLevel = 7;
        public float baseValue, holderLevelBonus, statLevelBonus;

        public float Value { get; private set; }

        public UnityEvent onUpgrade;

        int lastStatLevel, lastHolderLevel;

        void Start() {
            lastStatLevel = level;
            lastHolderLevel = scoreCounter.currentLevel.index;
            Value = ComputeValue();
            onUpgrade.Invoke();
        }

        void FixedUpdate() {
            if (lastStatLevel != level) {
                level = Mathf.Clamp(level, 0, maxLevel);
                lastStatLevel = level;
                Value = ComputeValue();
            }

            int holderLevel = scoreCounter.currentLevel.index;
            if (lastHolderLevel != holderLevel) {
                lastHolderLevel = holderLevel;
                Value = ComputeValue();
            }
        }

        float ComputeValue() {
            return baseValue + holderLevelBonus * scoreCounter.currentLevel.index + statLevelBonus * lastStatLevel;
        }

        public void Upgrade() {
            if (lastStatLevel < maxLevel && scoreCounter.upgradePoints > 0) {
                level += 1;
                scoreCounter.upgradePoints -= 1;
                onUpgrade.Invoke();
            }
        }

        public void OnRespawn() {
            lastStatLevel = level = 0;
            lastHolderLevel = scoreCounter.currentLevel.index;
            Value = ComputeValue();
        }
    }
}
