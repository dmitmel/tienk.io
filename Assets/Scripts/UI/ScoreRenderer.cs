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
using Tienkio.Data;

namespace Tienkio.UI {
    public class ScoreRenderer : MonoBehaviour {
        public ScoreCounter counter;
        public TankUpgrader tankUpgrader;

        [Space]
        public Text scoreLabel;
        public Text levelLabel;
        public Text upgradePointsLabel;

        [Space]
        public FilledBar levelBar;
        public FilledBar scoreToNextUpgradeBar;

        TankUpgradeTree tankUpgradeTree;

        void Start() {
            tankUpgradeTree = TankUpgradeTree.instance;
        }

        public void UpdateScoreLabel() {
            scoreLabel.text = string.Format("Score: {0}", counter.score.ToString("#,##0"));
        }

        public void UpdateLevelLabel() {
            levelLabel.text = string.Format("Lvl {0} {1}", counter.currentLevel.index,
                                            tankUpgrader.currentUpgradeNode.tankName);
        }

        public void UpdateLevelBar() {
            int score = counter.score;
            Level currentLevel = counter.currentLevel;

            if (currentLevel.scoreToNextLevel > 0) {
                int scoreInLevel = score - currentLevel.neededScore;
                levelBar.value = (float) scoreInLevel / currentLevel.scoreToNextLevel;
            } else {
                levelBar.value = 1;
            }
        }

        public void UpdateUpgradePointsLabel() {
            upgradePointsLabel.text = string.Format("x{0}", counter.upgradePoints);
        }

        public void UpdateScoreToNextUpgradeBar() {
            if (tankUpgradeTree != null) {
                TankUpgradeNode[] tankUpgradeNodes = tankUpgradeTree.tankUpgradeTree;
                int[] unlockedTanks = tankUpgrader.currentUpgradeNode.unlockedTanks;

                TankUpgradeNode minUpgrade = null;

                foreach (int unlockedTank in unlockedTanks) {
                    TankUpgradeNode unlockedTankNode = tankUpgradeNodes[unlockedTank];
                    if (minUpgrade == null)
                        minUpgrade = unlockedTankNode;
                    else if (unlockedTankNode.minLevel < minUpgrade.minLevel)
                        minUpgrade = unlockedTankNode;
                }

                if (minUpgrade != null) {
                    Level minUpgradeLevel = counter.levels[minUpgrade.minLevel];
                    int minScoreOfNextUpgrade = minUpgradeLevel.neededScore;
                    scoreToNextUpgradeBar.value = (float) counter.score / minScoreOfNextUpgrade;
                } else {
                    scoreToNextUpgradeBar.value = 1;
                }
            }
        }
    }
}
