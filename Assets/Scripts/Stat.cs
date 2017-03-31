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

namespace Deepio {
    public class Stat : MonoBehaviour {
        public ScoreCounter scoreCounter;
        public int level;
        public int maxLevel = 7;
        public float baseValue, holderLevelBonus, statLevelBonus;

        public float value { get; private set; }

        int lastStatLevel, lastHolderLevel;

        void Start() {
            lastStatLevel = level;
            lastHolderLevel = scoreCounter.currentLevel.index;
            value = ComputeValue();
        }

        void Update() {
            if (lastStatLevel != level) {
                if (level < 0 || level > maxLevel)
                    throw new ArgumentOutOfRangeException("statLevel", level, $"statLevel must be >= 0 and <= {maxLevel}");
                lastStatLevel = level;
                value = ComputeValue();
            }

            int holderLevel = scoreCounter.currentLevel.index;
            if (lastHolderLevel != holderLevel) {
                lastHolderLevel = holderLevel;
                value = ComputeValue();
            }
        }

        float ComputeValue() {
            return baseValue + holderLevelBonus * scoreCounter.currentLevel.index + statLevelBonus * lastStatLevel;
        }

        public void Upgrade() {
            if (lastStatLevel < 7 && scoreCounter.upgradePoints > 0) {
                level += 1;
                scoreCounter.upgradePoints -= 1;
            }
        }
    }
}
