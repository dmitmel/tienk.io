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
using UnityEngine.UI;
using System;

namespace Tienkio.Tanks {
    [Serializable]
    public class Level {
        public int index;
        public int neededScore;
        public int scoreToNextLevel;
        public int respawnLevel;
        public bool givesUpgradePoint;
    }

    public class ScoreCounter : MonoBehaviour {
        [SerializeField]
        int _score, _upgradePoints;
        public Level[] levels;
        [HideInInspector]
        public Level currentLevel;

        [Space]
        public UnityEvent onScoreChange;
        public UnityEvent onUpgradePointsChange;

        [Space]
        public Text scoreLabel;

        public int score {
            get { return _score; }
            set {
                if (_score != value) {
                    _score = value;
                    currentLevel = ComputeLevel();
                    if (scoreLabel != null) scoreLabel.text = FormatScore();
                    onScoreChange.Invoke();
                }
            }
        }

        public int upgradePoints {
            get { return _upgradePoints; }
            set {
                if (_upgradePoints != value) {
                    _upgradePoints = value;
                    onUpgradePointsChange.Invoke();
                }
            }
        }

        string FormatScore() {
            string suffix = _score >= 1e6 ? "M" : _score >= 1000 ? "k" : "";
            float valueForFormatting =
                _score >= 1e6 ? (float) Math.Round(_score / 1e6, 1) :
                _score >= 1000 ? (float) Math.Round(_score / 1000.0, 1) : _score;
            return valueForFormatting.ToString(string.Format("#,##0.#{0}", suffix));
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (levels.Length > 0) {
                for (int i = 0; i < levels.Length - 1; i++) {
                    Level level = levels[i];
                    Level nextLevel = levels[i + 1];
                    level.index = i;
                    level.scoreToNextLevel = Mathf.Max(nextLevel.neededScore - level.neededScore, 0);
                }

                Level lastLevel = levels[levels.Length - 1];
                lastLevel.index = levels.Length - 1;
                lastLevel.scoreToNextLevel = 0;
            }

            currentLevel = ComputeLevel();
        }
#endif

        Level ComputeLevel() {
            int prevUpgradePoints = _upgradePoints;

            for (int i = 0; i < levels.Length - 1; i++) {
                Level level = levels[i];
                Level nextLevel = levels[i + 1];
                if (level.neededScore <= _score) {
                    if (level.index > currentLevel.index && level.givesUpgradePoint) _upgradePoints++;
                    if (_score < nextLevel.neededScore) {
                        if (prevUpgradePoints != _upgradePoints) onUpgradePointsChange.Invoke();
                        return level;
                    }
                }
            }

            Level lastLevel = levels[levels.Length - 1];
            if (lastLevel.index > currentLevel.index && lastLevel.givesUpgradePoint) _upgradePoints++;
            if (prevUpgradePoints != _upgradePoints) onUpgradePointsChange.Invoke();
            return levels[levels.Length - 1];
        }

        public void OnRespawn() {
            score = 0;
            upgradePoints = 0;
        }
    }
}
