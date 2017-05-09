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

namespace Tienkio {
    public class ScoreRenderer : MonoBehaviour {
        public ScoreCounter counter;

        public Text scoreLabel, levelLabel, upgradePointsLabel;
        public LevelBar levelBar;

        //string FormatScore() {
        //    string suffix = score >= 1e6 ? "M" : score >= 1000 ? "k" : "";
        //    float valueForFormatting =
        //        score >= 1e6 ? (float) Math.Round(score / 1e6, 1) :
        //        score >= 1000 ? (float) Math.Round(score / 1000.0, 1) : score;
        //    return valueForFormatting.ToString($"#,##0.#{suffix}");
        //}

        public void OnScoreChange() {
            scoreLabel.text = string.Format("Score: {0}", counter.score.ToString("#,##0"));

            levelLabel.text = string.Format("Lvl {0} Tank", counter.currentLevel.index);
            levelBar.UpdateBar();
        }

        public void OnUpgradePointsChange() {
            upgradePointsLabel.text = string.Format("x{0}", counter.upgradePoints);
        }
    }
}
